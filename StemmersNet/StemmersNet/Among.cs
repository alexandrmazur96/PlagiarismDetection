namespace Iveonik.Stemmers
{

    internal class Among
    {

        public readonly int SSize; /* search string */
        public readonly char[] S; /* search string */
        public readonly int SubstringI; /* index to longest matching substring */
        public readonly int Result; /* result of the lookup */
        public delegate bool BoolDel();
        public readonly BoolDel Method; /* method to use if substring matches */

        public Among(string s, int substringI, int result, BoolDel linkMethod)
        {
            SSize = s.Length;
            S = s.ToCharArray();
            SubstringI = substringI;
            Result = result;
            Method = linkMethod;
        }
    }
}
