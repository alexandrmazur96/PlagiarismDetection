using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DocumentAdder.Helpers
{
    public static class Verifications
    {
        public static bool isFtpValid(string path)
        {
            return (new Uri(path).Scheme == Uri.UriSchemeFtp && Uri.IsWellFormedUriString(path, UriKind.Absolute)) ? true : false;
            //return Uri.IsWellFormedUriString(path, UriKind.Absolute) ? true : false;
        }
    }
}
