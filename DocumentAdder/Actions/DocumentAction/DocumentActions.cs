using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DocumentAdder.Helpers;
using DocumentAdder.Types;
using DocumentAdder.Types.DataBase;
using DocumentAdder.Types.Exceptions;
using DocumentFormat.OpenXml.Packaging;
using Iveonik.Stemmers;
using MongoDB.Bson;
using Word = Microsoft.Office.Interop.Word;

namespace DocumentAdder.Actions.DocumentAction
{
    public class DocumentActions
    {
        #region Private members

        #region Methods       

        /// <summary>
        /// Очищает текст используя базу (словарь) стоп-слов
        /// </summary>
        /// <param name="text">Неочищенный текст, который нужно канонизировать</param>
        /// <returns>Коллекцию слов из текста, которые готовы к употреблению =)</returns>
        private static List<string> TextPurify(string text)
        {
            //разделяем ввесь текст на отдельные слова
            var rawTokens = text.Split(Separators).ToList();

            //проходимся по этому списку слов в linq-выражении
            var canonedTokens = rawTokens.Select(word => word.ToCharArray().Where(n => !char.IsDigit(n)).ToArray()).Select(purified => new string(purified)).ToList();

            //из этой коллекции удаляем все пустые элементы и стоп-слова используя linq
            canonedTokens.RemoveAll(item => StopWords.Contains(item.ToLower()) || string.IsNullOrWhiteSpace(item));

            //также удаляются все стоп-символы из слов в коллекции
            var purifiedTokens = (from item in canonedTokens let regex = new Regex("[0-9/|_!@#$%^&*()_+=?:;.,{}№><«»'\"`~" + @"\\[\]– -]*") select regex.Replace(item, "")).ToList();

            //устанавливаются все слова в Lower Case
            var purifiedLowerCaseTokens = purifiedTokens.Select(purifiedToken => purifiedToken.ToLower()).ToList();

            var stemmedLowerCaseTokens = new List<string>();
            var cyrillicStemmer = new RussianStemmer();
            var latinStemmer = new EnglishStemmer();
            foreach (var purifiedLowerCaseToken in purifiedLowerCaseTokens)
            {
                switch (Verifications.GetFontType(purifiedLowerCaseToken))
                {
                    case FontType.Cyrillic:
                        stemmedLowerCaseTokens.Add(cyrillicStemmer.Stem(purifiedLowerCaseToken));
                        break;
                    case FontType.Latin:
                        stemmedLowerCaseTokens.Add(latinStemmer.Stem(purifiedLowerCaseToken));
                        break;
                    case FontType.Other:
                    case FontType.Numbers:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return stemmedLowerCaseTokens;
        }

        /// <summary>
        /// Возвращает количество всех документов.
        /// </summary>
        /// <returns>Количество всех документов в MongoDb.</returns>
        private static int GetCountAllDocuments()
        {
            return DataBase.GetInstance().GetAllDocuments().Count;
        }

        /// <summary>
        /// Асинхронно возвращает количество всех документов.
        /// </summary>
        /// <returns>Количество всех документов в MongoDb.</returns>
        private static async Task<int> GetCountAllDocumentsAsync()
        {
            var documentsList = await DataBase.GetInstance().GetAllDocumentsAsync();
            return documentsList.Count;
        }

        /// <summary>
        /// Возвращает количество документов, в которых есть нужное слово.
        /// </summary>
        /// <param name="token">Искомое слово.</param>
        /// <returns>Количество документов, которые содержат искомое слово.</returns>
        private static int GetCountDocumentWithToken(string token)
        {
            return DataBase.GetInstance().GetAllDocuments(new BsonDocument(
                new BsonElement("DocumentTokens", token))).Count;
        }

        /// <summary>
        /// Асинхронно возвращает количество документов, в которых есть нужное слово.
        /// </summary>
        /// <param name="token">Искомое слово.</param>
        /// <returns>Количество документов, которые содержат искомое слово.</returns>
        private static async Task<int> GetCountDocumentWithTokenAsync(string token)
        {
            var documentList = await DataBase.GetInstance().GetAllDocumentsAsync(new BsonDocument(
                new BsonElement("DocumentTokens", token)));
            return documentList.Count;
        }

        #endregion

        #region Const Data

        /// <summary>
        /// Разделители, для extension-метода класса string - Split.
        /// Необходимы, для разбиение текста по указанным разделителям.
        /// </summary>
        private static readonly char[] Separators =
        {
            //стоп-символы
            '.', ',', ';', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '_', '+', '=', '\\', '/',
            '<', '>', '\'', '№', '?', ':', '`', '~', ' ', '\t', '\n', '\r', '|', '[', ']', '{', '}', '"', '–',
            '«', '»'
        };

        /// <summary>
        /// Коллекция стоп-слов, которые не несут смысловой нагрузки в тексте 
        /// и которые необходимо удалить.
        /// </summary>
        private static readonly string[] StopWords = {         
            //стоп-цифры
                //римские
                    "I", "II", "III", "IIII", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII", "XIII", "XX", "XXX", "M",
                    "І", "ІІ", "ІІІ", "ІІІІ", "М", "ІХ", "Х", "ХІ", "ХІІ", "ХІІІ", "ХХ", "ХХХ",
            //стоп-буквы
                //киррилические
                "а", "б", "в", "г", "д", "е", "ё", "ж", "з", "и", "й", "к", "л", "м", "н", "о",
                "п", "р", "с", "т", "у", "ф", "х", "ц", "ч", "ш", "щ", "ъ", "ы", "ь", "э", "ю", "я",
                "і", "ї", "ґ", 
                //латинские
                "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u",
                "v", "w", "x", "y", "z",
            //стоп-слова
                //киррилические
                "без", "біля", "близько", "ім’я", "інтересах", "бік", "залежності", "від", "міру", "напрямі", "до", "порівнянні", "процесі",
                "результаті", "ролі", "силу", "сторону", "супроводі", "ході", "верх", "вздовж", "уздовж", "вище",
                "від", "близько", "імені","віддалік", "далеко", "залежно", "збоку", "ліворуч", "недалеко","незалежно",
                "неподалеку", "неподалечку", "неподалік", "оподалік", "оподаль", "подалі", "подаль", "поодалік", "поодаль", "праворуч",
                "раніше", "раніш", "відносно", "відповідно" ,"внаслідок", "унаслідок","во", "вперед", "уперед", "вподовж", "уподовж",
                "впродовж", "упродовж", "впрост", "упрост", "всередині", "усередині", "вслід", "услід","всупереч", "усупереч", "для","до",
                "відповідно", "не", "подібно", "впритиск", "упритиск", "впритул", "упритул", "довкіл", "із", "зі", "зо", "метою", "нагоди",
                "приводу", "розрахунку", "згідно", "нарівні", "порівняно", "поряд", "разом", "судячи", "за", "вслід", "услід", "винятком",
                "допомогою", "посередництвом", "рахунок", "слідом", "завдяки", "задля", "замість", "заради", "збоку", "зверх", "зверху",
                "звиш", "здовж", "з-за", "із-за", "ззаду", "іззаду", "з-над", "з-перед", "з-під", "із-під", "з-поза", "з-поміж", "з-проміж",
                "з-понад", "з-поперед", "з-посеред", "з-серед", "зсередини", "ізсередини", "ід", "к", "ік", "кінець", "коло", "коштом", "край",
                "крізь", "крім", "круг", "кругом", "мимо", "між", "зважаючи", "адресу", "базі", "благо", "випадок", "відміну", "ґрунті",
                "засадах", "знак", "зразок", "користь", "кшталт", "межі", "основі", "підставі", "противагу", "честь", "чолі", "незважаючи",
                "навкіл", "навколо", "навкруг", "навкруги", "навперейми", "надовкола", "надокола", "назустріч", "наокіл", "наоколо", "наокруг",
                "наокруги", "наперед", "напереді", "напередодні", "наперекір", "напереріз", "наприкінці", "напроти", "навпроти", "насеред",
                "наспід", "насподі", "настрічу", "насупроти", "насупротив", "недалеко", "неподалеку", "неподалечку", "неподалік", "нижче",
                "об", "обабіч", "обаполи", "обік", "обіч", "обіруч", "окіл", "окрай", "окрім", "окроме", "округ", "округи", "опісля", "оподалік",
                "оподаль", "опостін", "опостінь", "опріч", "опріче", "опроче", "осторонь", "перед", "переді", "передо", "півперек", "під",
                "знаком", "приводом", "час", "після", "по", "побік", "побіля", "побіч", "поблизу", "поверх", "повз", "поз", "поуз", "повище",
                "подовж", "поза", "позад", "позаду", "поздовж", "покрай", "помимо", "поміж", "помежи", "помість", "понад", "понаді", "понадо",
                "пообіч", "поперед", "попереду", "поперек", "попід", "попліч", "попри", "поряд", "посеред", "посередині", "почерез", "пред",
                "преді", "предо", "при", "пріч", "про", "проз", "проміж", "промеж", "просто", "проти", "против", "противно", "протягом", "ради",
                "раніше", "раніш", "серед", "середи", "середу", "скрізь", "спереду", "стосовно", "супроти", "супротив", "вигляді", "випадку",
                "відповідності", "до", "відповідь", "на", "зв’язку", "узбіч", "ціною", "через", "шляхом", "щодо",

                "еще","него","сказать", "нее","со", "без", "же", "ней", "совсем", "более","жизнь","нельзя","так",
                "больше", "за","нет","такой", "будет","зачем", "ни", "там", "будто","здесь", "нибудь", "тебя",
                "бы", "никогда", "тем", "был", "из", "ним", "теперь", "была","из-за", "них", "то", "были", "или", "ничего",
                "тогда", "было", "им", "но", "того", "быть", "иногда", "ну", "тоже", "их", "только", "вам", "об", "том",
                "вас", "кажется", "один", "тот", "вдруг", "как", "он", "три", "ведь", "какая", "она", "тут", "во", "какой",
                "они", "ты", "вот", "когда", "опять", "впрочем", "конечно", "от", "уж", "все", "которого", "перед", "уже",
                "всегда", "которые", "по", "хорошо", "всего", "кто", "под", "хоть", "всех", "куда", "после", "чего", "всю", "ли",
                "потом", "человек", "вы", "лучше", "потому", "чем", "между", "почти", "через", "где", "меня", "при", "что",
                "говорил", "мне", "про", "чтоб", "да", "много", "раз", "чтобы", "даже", "может", "разве", "чуть", "два", "можно",
                "эти", "для", "мой", "сам", "этого", "до", "моя", "свое", "этой", "другой", "мы", "свою", "этом", "его", "на",
                "себе", "этот", "ее", "над", "себя", "эту", "ей", "надо", "сегодня", "ему", "наконец", "сейчас", "если",
                "нас", "сказал", "есть", "не", "сказала", "это", "тех", "дает", "те",
                "нужен", "вообще", "этих", "даст", "ко", "ваш", "млн", "млрд", "тыс", "рис", "рис.", 
                //латинские
                "about","above","according","across","actually","ad","adj","ae","af","after","afterwards","ag","again","against","ai","al","all","almost","alone","along","already","also","although","always","am","among","amongst","an","and","another","any","anyhow","anyone","anything","anywhere","ao","aq","ar","are","aren","aren't","around","arpa","as","at","au","aw","az",
                "ba","bb","bd","be","became","because","become","becomes","becoming","been","before","beforehand","begin","beginning","behind","being","below","beside","besides","between","beyond","bf","bg","bh","bi","billion","bj","bm","bn","bo","both","br","bs","bt","but","buy","bv","bw","by","bz",
                "ca","can","can't","cannot","caption","cc","cd","cf","cg","ch","ci","ck","cl","click","cm","cn","co","co.","com","copy","could","couldn","couldn't","cr","cs","cu","cv","cx","cy","cz",
                "de","did","didn","didn't","dj","dk","dm","do","does","doesn","doesn't","don","don't","down","during","dz",
                "each","ec","edu","ee","eg","eh","eight","eighty","either","else","elsewhere","end","ending","enough","er","es","et","etc","even","ever","every","everyone","everything","everywhere","except",
                "few","fi","fifty","find","first","five","fj","fk","fm","fo","for","former","formerly","forty","found","four","fr","free","from","further","fx",
                "ga","gb","gd","ge","get","gf","gg","gh","gi","gl","gm","gmt","gn","go","gov","gp","gq","gr","gs","gt","gu","gw","gy",
                "had","has","hasn","hasn't","have","haven","haven't","he","he'd","he'll","he's","help","hence","her","here","here's","hereafter","hereby","herein","hereupon","hers","herself","him","himself","his","hk","hm","hn","home","homepage","how","however","hr","ht","htm","html","http","hu","hundred",
                "i'd","i'll","i'm","i've","i.e.","id","ie","if","ii","il","im","in","inc","inc.","indeed","information","instead","int","into","io","iq","ir","is","isn","isn't","it","it's","its","itself",
                "je","jm","jo","join","jp",
                "ke","kg","kh","ki","km","kn","koo","kp","kr","kw","ky","kz",
                "la","last","later","latter","lb","lc","least","less","let","let's","li","like","likely","lk","ll","lr","ls","lt","ltd","lu","lv","ly",
                "ma","made","make","makes","many","maybe","mc","md","me","meantime","meanwhile","mg","mh","microsoft","might","mil","million","miss","mk","ml","mm","mn","mo","more","moreover","most","mostly","mp","mq","mr","mrs","ms","msie","mt","mu","much","must","mv","mw","mx","my","myself","mz",
                "na","namely","nc","ne","neither","net","netscape","never","nevertheless","new","next","nf","ng","ni","nine","ninety","nl","no","nobody","none","nonetheless","noone","nor","not","nothing","now","nowhere","np","nr","nu","null","nz",
                "of","off","often","om","on","once","one","one's","only","onto","or","org","other","others","otherwise","our","ours","ourselves","out","over","overall","own",
                "pa","page","pe","per","perhaps","pf","pg","ph","pk","pl","pm","pn","pr","pt","pw","py",
                "qa", "rather","re","recent","recently","reserved","ring","ro","ru","rw",
                "sa","same","sb","sc","sd","se","seem","seemed","seeming","seems","seven","seventy","several","sg","sh","she","she'd","she'll","she's","should","shouldn","shouldn't","si","since","site","six","sixty","sj","sk","sl","sm","sn","so","some","somehow","someone","something","sometime","sometimes","somewhere","sr","st","still","stop","su","such","sv","sy","sz",
                "taking","tc","td","ten","text","tf","tg","test","th","than","that","that'll","that's","the","their","them","themselves","then","thence","there","there'll","there's","thereafter","thereby","therefore","therein","thereupon","these","they","they'd","they'll","they're","they've","thirty","this","those","though","thousand","three","through","throughout","thru","thus","tj","tk","tm","tn","to","together","too","toward","towards","tp","tr","trillion","tt","tv","tw","twenty","two","tz",
                "ua","ug","uk","um","under","unless","unlike","unlikely","until","up","upon","us","use","used","using","uy","uz",
                "va","vc","ve","very","vg","vi","via","vn","vu",
                "was","wasn","wasn't","we","we'd","we'll","we're","we've","web","webpage","website","welcome","well","were","weren","weren't","wf","what","what'll","what's","whatever","when","whence","whenever","where","whereafter","whereas","whereby","wherein","whereupon","wherever","whether","which","while","whither","who","who'd","who'll","who's","whoever","whole","whom","whomever","whose","why","will","with","within","without","won","won't","would","wouldn","wouldn't","ws","www",
                "ye","yes","yet","you","you'd","you'll","you're","you've","your","yours","yourself","yourselves","yt","yu",
                "za", "zm", "zr"
        };

        /// <summary>
        /// Словарь для замены английский букв, которые одинаково пишутся на киррилице.
        /// </summary>
        private static readonly Dictionary<char, char> Replacement = new Dictionary<char, char>()
        {
            ['a'] = 'а',
            ['A'] = 'А',
            ['B'] = 'В',
            ['c'] = 'с',
            ['C'] = 'С',
            ['e'] = 'е',
            ['E'] = 'Е',
            ['H'] = 'Н',
            ['i'] = 'і',
            ['I'] = 'I',
            ['K'] = 'К',
            ['M'] = 'М',
            ['o'] = 'о',
            ['O'] = 'О',
            ['p'] = 'р',
            ['P'] = 'Р',
            ['T'] = 'Т',
            ['x'] = 'х',
            ['X'] = 'х'
        };

        #endregion

        #endregion

        /// <summary>
        /// Возвращает пути к файлам.
        /// </summary>
        /// <returns>Коллекцию с путями к файлов.</returns>
        public static List<string> GetFilePaths()
        {
            //коллекция с путями к *файлам* документов
            var filePaths = new List<string>();

            foreach (var item in ProgramSettings.GetInstance().CollectionsPaths)
            {
                switch (item.StorageType)
                {
                    case InternalStorageType.Directory:
                        filePaths.AddRange(Directory.GetFiles(item.StoragePath, "*.*").Where(s =>
                        {
                            var extension = Path.GetExtension(s);
                            return extension != null && ProgramSettings.GetInstance().FileTypes.Contains(extension.ToLower());
                        }));
                        break;
                    case InternalStorageType.FTP:

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return filePaths;
        }

        public static async Task<int> GetFileCountsAsync(ObservableCollection<RepositoryPath> collectionPath, string fileTypes)
        {
            int filesCount = 0;
            foreach (var item in collectionPath)
            {
                if (item.StorageType == InternalStorageType.Directory)
                {
                    //filesCount += new DirectoryInfo(item.StoragePath).GetFiles().Count();
                    filesCount += Directory.GetFiles(item.StoragePath, "*.*").Where(s =>
                    {
                        var extension = Path.GetExtension(s);
                        return extension != null && fileTypes.Contains(extension.ToLower());
                    }).Count();
                }
                if (item.StorageType == InternalStorageType.FTP)
                {
                    try
                    {
                        var list = new List<string>();
                        //Создаем новое подключение ftp по адресу, указаному в коллекции
                        FtpWebRequest ftpReq = (FtpWebRequest)WebRequest.Create(item.StoragePath);

                        //указываем не кешировать запрос
                        ftpReq.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);

                        //выбираем команду для ftp, в данном случае - получить подробную информацию о содержимом
                        ftpReq.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

                        //задаем учетные данные для ftp (логин и пароль)
                        ftpReq.Credentials = new NetworkCredential(item.FTPLogin, item.FTPPassword);

                        //получаем ответ от ftp-сервера:
                        using (var response = await ftpReq.GetResponseAsync())
                        {
                            using (var stream = response.GetResponseStream())
                            {
                                if (stream != null)
                                    using (var reader = new StreamReader(stream, true))
                                    {
                                        while (!reader.EndOfStream)
                                        {
                                            list.Add(await reader.ReadLineAsync());
                                        }
                                    }
                            }
                        }
                        filesCount += list.Count;
                    }
                    catch (Exception e)
                    {
                        System.Windows.MessageBox.Show(e.Source + "\n" + e.Message);
                    }
                }
            }
            return filesCount;
        }

        /// <summary>
        /// Очищает (канонизирует) документ MS Office Word. 
        /// Используя при этом OpenXML и Microsoft.Office.Interop.Word (COM-порт MS Office)
        /// </summary>
        /// <param name="wordFilePath">Путь к документу MS Office Word</param>
        /// <returns>Возвращает очищенный (канонизированный) список слов, готовых к работе в алгоритме TF*IDF</returns>
        public static List<string> GetWordCanonedTokens(string wordFilePath)
        {
            var tokenList = new List<string>();
            try
            {
                object fileName = wordFilePath;
                //отсутствующий параметр
                object miss = System.Reflection.Missing.Value;

                //если документ в старом формате .doc, пересохраняем его в новый .docx
                //используя COM порт
                //это необходимое зло
                var extension = Path.GetExtension(wordFilePath);
                if (extension != null && extension.Equals(".doc"))
                {
                    try
                    {
                        var app = new Word.Application();
                        //присваиваем новое имя (такое же, только без расширения, чтобы сохранилось нормас)
                        //Word.Interop принимает в качестве параметров объекты object, даже если там строка
                        //странно, но по другому никак
                        object newFileName = wordFilePath.Remove(wordFilePath.Length - 4, 4);
                        //открываем указанный документ
                        app.Documents.Open(ref fileName, miss, miss, miss, miss, miss, miss, miss, miss, miss, miss, miss, miss, miss, miss, miss);
                        //получаем открытый документ для манипуляций
                        var doc = app.ActiveDocument;
                        //пересохраняем его
                        doc.SaveAs2(newFileName, Word.WdSaveFormat.wdFormatXMLDocument);
                        //закрываем документ
                        doc.Close();
                        //удаляем старый файл
                        File.Delete((string)fileName);
                        //переприсваиваем имена                        
                        var tmpFileName = (string)newFileName;
                        tmpFileName += ".docx";
                        fileName = tmpFileName;
                        app.Quit();
                    }
                    catch (Exception e)
                    {
                        Console.Error.WriteLine(e.Message);
                    }
                }
                string rawText; //необработанный текст

                //открываем документ, используя оператор using (аналог try с ресурсами в Java)
                using (var wordDocument = WordprocessingDocument.Open(fileName as string, false))
                {
                    //получаем весь текст
                    var paraList = wordDocument.MainDocumentPart.Document.Body.ChildElements;
                    var sb = new StringBuilder();
                    foreach (var paragraph in paraList)
                    {
                        sb.Append(paragraph.InnerText + " ");
                    }
                    rawText = sb.ToString();
                }

                //очищаем или канонизируем его, превращая в список
                tokenList = TextPurify(rawText);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
            return tokenList;
        }

        /// <summary>
        /// Заменяет латинские буквы, которые пишутся как кириллица на их кириллические аналоги.
        /// </summary>
        /// <param name="purifiedTokensList">Коллекция очищенных слов. Передается строго по ссылке (ref)</param>
        public static void Cyrillify(ref List<string> purifiedTokensList)
        {
            var tmpList = new List<string>(purifiedTokensList);
            foreach (var purifiedToken in tmpList)
            {
                var sb = new StringBuilder(purifiedToken);
                foreach (var kvp in Replacement)
                {
                    sb.Replace(kvp.Key, kvp.Value);
                }
                purifiedTokensList = purifiedTokensList.ItemReplace(purifiedToken, sb.ToString());
            }
        }

        /// <summary>
        /// Составляет словарь (TF вектор) с парами [Слово:к-ство употреблений в документе].
        /// </summary>
        /// <param name="doneList">Полностью готовый для обработки (подсчета) текст.</param>
        /// <returns>Словарь (TF вектор).</returns>
        public static Dictionary<string, double> MakeTfVector(List<string> doneList)
        {
            var tfVector = new Dictionary<string, double>();
            var tmp = new List<string>(doneList);
            //к-ство слов в документе
            var tokensCount = doneList.Count;
            foreach (string token in tmp)
            {
                var currentTokenCount = doneList.FindAll(x => token.Equals(x)).Count;
                doneList.RemoveAll(x => token.Equals(x));
                if (!tfVector.ContainsKey(token))
                {
                    tfVector.Add(token, (double)currentTokenCount / tokensCount);
                }
            }
            return tfVector;
        }

        /// <summary>
        /// Создание IDF-вектора.
        /// </summary>
        /// <returns>IDF-вектор.</returns>
        public static List<IdfItem> MakeIdfVector(Dictionary<string, double> tfVector)
        {
            var documentsList = DataBase.GetInstance().GetAllDocuments();
            var documentsCount = documentsList.Count;
            return tfVector.Select(tfItem => new IdfItem
            {
                IdfId = default(ObjectId),
                Token = tfItem.Key,
                IdfValue = (double)GetCountDocumentWithToken(tfItem.Key) / documentsCount
            }).ToList();
        }

        /// <summary>
        /// Асинхронное создание IDF-вектора.
        /// </summary>
        /// <returns>IDF-вектор.</returns>
        public static async Task<List<IdfItem>> MakeIdfVectorAsync(Dictionary<string, double> tfVector)
        {
            var documentsList = DataBase.GetInstance().GetAllDocuments();
            var documentsCount = documentsList.Count;
            var idfVector = new List<IdfItem>();
            foreach (var tfItem in tfVector)
            {
                idfVector.Add(new IdfItem
                {
                    IdfId = default(ObjectId),
                    Token = tfItem.Key,
                    IdfValue = (double)await GetCountDocumentWithTokenAsync(tfItem.Key) / documentsCount
                });
            }
            return idfVector;
        }

        /// <summary>
        /// Создание TF-вектора.
        /// </summary>
        /// <param name="tfVector">TF-вектор документа.</param>
        /// <returns>TF*IDF-вектор.</returns>
        public static Dictionary<string, double> MakeTfIdfVector(Dictionary<string, double> tfVector)
        {
            var idfVector = DataBase.GetInstance().GetIdfVector();
            var tfIdfVector = new Dictionary<string, double>();
            int index = 0;
            foreach (var tf in tfVector)
            {
                if (idfVector.ContainsIdf(tf.Key))
                {
                    tfIdfVector.Add(tf.Key, tf.Value * idfVector[index].IdfValue);
                }
                index++;
            }
            return tfIdfVector;
        }

        /// <summary>
        /// Асинхронное создание TF*IDF-вектора.
        /// </summary>
        /// <param name="tfVector">TF-вектор документа.</param>
        /// <returns>TF*IDF-вектор.</returns>
        public static async Task<Dictionary<string, double>> MakeTfIdfVectorAsync(Dictionary<string, double> tfVector)
        {
            var idfVector = await DataBase.GetInstance().GetIdfVectorAsync();
            var tfIdfVector = new Dictionary<string, double>();
            int index = 0;
            foreach (var tf in tfVector)
            {
                if (idfVector.ContainsIdf(tf.Key))
                {
                    tfIdfVector.Add(tf.Key, tf.Value * idfVector[index].IdfValue);
                    index++;
                }
            }
            return tfIdfVector;
        }

        /// <summary>
        /// Разделяет имя файла на составные части: [Автор, Группа, Имя].
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        /// <returns>Массив составных частей из имени файла в виде [Author, Group, DocumentName]</returns>
        public static string[] SplitFileName(string fileName)
        {
            if (Verifications.IsFileNameValid(fileName))
            {
                return fileName.Split('_');
            }
            throw new FileNameFormatException("Неправильный формат входного файла!");
        }        
    }
}
