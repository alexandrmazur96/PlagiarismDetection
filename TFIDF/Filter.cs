using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFIDF
{
    public class Filter
    {
        StopWordsHandler swh;

        //public string[] HandlingDocument(List<string> document)
        //{
        //    string[] resultDoc = new string[document.Count];
        //    foreach (string item in document)
        //    {
        //        //if(!swh.isStopWord(item)) resultDoc
        //    }
        //}

        public Filter()
        {
            swh = new StopWordsHandler();
        }
    }
}
