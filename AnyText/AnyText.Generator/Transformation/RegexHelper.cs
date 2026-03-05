using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Transformation
{
    internal static class RegexHelper
    {
        public static string TransformRegex(string regex, string surroundCharacter)
        {
            var analyzed = 0;
            int analyzedPipe;
            while ((analyzedPipe = regex.IndexOf('|', analyzed)) != -1)
            {
                var openParentheses = regex[0..analyzedPipe].Count(c => c == '(');
                var closedParentheses = regex[0..analyzedPipe].Count(c => c == ')');
                if (openParentheses == closedParentheses)
                {
                    regex = regex.Substring(0, analyzedPipe + 1) + "^" + regex.Substring(analyzedPipe + 1);
                }
                analyzed = analyzedPipe + 2;
            }
            return "^" + surroundCharacter + regex + surroundCharacter;
        }
    }
}
