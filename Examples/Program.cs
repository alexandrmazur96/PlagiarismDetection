using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Examples
{
    class Program
    {
        public static bool IsFileNameValid(string fileName)
        {
            var regex = new Regex(@"^[a-zA-Zа-яА-Я -]+_[a-zA-Zа-яА-Я0-9 -]+_([a-zA-Zа-яА-Я0-9– -])+$");
            return regex.IsMatch(fileName);
        }

        static void Main(string[] args)
        {
            var a1 = "abc123";
            var if1 = new Regex(@"[abcf]");
            var if2 = new Regex(@"[164]");
        }
    }
}
