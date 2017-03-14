using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DocumentAdder.Types;
using DocumentFormat.OpenXml.Packaging;
using Word = Microsoft.Office.Interop.Word;

namespace DocumentAdder.Actions.DocumentAction
{
    public class DocumentActions
    {
        private readonly char[] _separators =
        {
            //стоп-символы
            '.', ';', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '_', '+', '=', '\\', '/',
            '<', '>', '\'', '№', '?', ':', '`', '~', ' ', '\t', '\n', '\r'
        };

        private readonly string[] _stopWords = {         
            //стоп-цифры
                //римские
                    "I", "II", "III", "IIII", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII", "XIII", "XX", "XXX", "M",
                    "І", "ІІ", "ІІІ", "ІІІІ", "М", "ІХ", "Х", "ХІ", "ХІІ", "ХІІІ", "ХХ", "ХХХ",
            //стоп-буквы
                //киррилические
                "а", "і", "й", "о", "з", "в", "я", "е", "с", "к", "э", "у",
                //латинские
                "a", "u", "i",
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

        public List<string> GetFilePaths(ObservableCollection<RepositoryPath> collectionPathsToRepository, string filesType)
        {
            //коллекция с путями к *файлам* документов
            List<string> filePaths = new List<string>();

            foreach (var item in collectionPathsToRepository)
            {
                if (item.StorageType == InternalStorageType.Directory)
                {
                    foreach (string documentFile in Directory.GetFiles(item.StoragePath, "*.*").
                    Where(s => filesType.Contains(Path.GetExtension(s).ToLower())))
                    {
                        //do work here
                        Console.WriteLine(documentFile);
                    }
                }
                else if (item.StorageType == InternalStorageType.FTP)
                {

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
        public List<string> GetWordCanonedTokens(string wordFilePath)
        {
            List<string> tokenList = new List<string>();
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
                        Word.Application app = new Word.Application();
                        //присваиваем новое имя (такое же, только без расширения, чтобы сохранилось нормас)
                        //Word.Interop принимает в качестве параметров объекты object, даже если там строка
                        //странно, но по другому никак
                        object newFileName = wordFilePath.Remove(wordFilePath.Length - 4, 4);
                        //открываем указанный документ
                        app.Documents.Open(ref fileName, miss, miss, miss, miss, miss, miss, miss, miss, miss, miss, miss, miss, miss, miss, miss);
                        //получаем открытый документ для манипуляций
                        Word.Document doc = app.ActiveDocument;                        
                        //пересохраняем его
                        doc.SaveAs2(newFileName, Word.WdSaveFormat.wdFormatXMLDocument);
                        //закрываем документ
                        doc.Close();
                        //удаляем старый файл
                        File.Delete((string) fileName);
                        //переприсваиваем имена
                        fileName = newFileName;
                        //"обнуляем" ненужные переменные
                        newFileName = null;
                        app.Quit();
                    }
                    catch (Exception e)
                    {
                        Console.Error.WriteLine(e.Message);
                    }
                }
                string rawText = null; //необработанный текст

                //открываем документ, используя оператор using (аналог try с ресурсами в Java)
                using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(fileName as string, false))
                {
                    //получаем весь текст
                    rawText = wordDocument.MainDocumentPart.Document.Body.InnerText;
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
        /// Очищает текст используя базу (словарь) стоп-слов
        /// </summary>
        /// <param name="text">Неочищенный текст, который нужно канонизировать</param>
        /// <returns>Коллекцию слов из текста, которые готовы к употреблению =)</returns>
        private List<string> TextPurify(string text)
        {
            //разделяем ввесь текст на отдельные слова
            List<string> rawTokens = text.Split(_separators).ToList();

            //проходимся по этому списку слов в linq-выражении
            List<string> purifiedTokens = rawTokens.Select(word => word.ToCharArray().Where(n => !char.IsDigit(n)).ToArray()).Select(purified => new string(purified)).ToList();

            //из этой коллекции удаляем все пустые элементы и стоп-слова используя linq
            purifiedTokens.RemoveAll(item => _stopWords.Contains(item) || string.IsNullOrWhiteSpace(item));

            return purifiedTokens;
        }

    }
}
