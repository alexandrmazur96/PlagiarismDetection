using System;
using System.Text.RegularExpressions;

namespace DocumentAdder.Helpers
{
    public static class Verifications
    {

        //private static readonly Regex Template = new Regex(@"^[a-zA-Zа-яА-Я]+_[a-zA-Zа-яА-Я0-9-]+_([a-zA-Zа-яА-Я0-9 -])+$");

        public static bool IsFtpValid(string path)
        {
            return (new Uri(path).Scheme == Uri.UriSchemeFtp && Uri.IsWellFormedUriString(path, UriKind.Absolute));
            //return Uri.IsWellFormedUriString(path, UriKind.Absolute) ? true : false;
        }

        public static bool IsFileNameValid(string fileName)
        {
            var regex = new Regex(@"^[a-zA-Zа-яА-Я -]+_[a-zA-Zа-яА-Я0-9 -]+_([a-zA-Zа-яА-Я0-9– -])+$");
            return regex.IsMatch(fileName);
        }        
    }
}
