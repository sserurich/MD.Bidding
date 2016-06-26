using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using MD.Bidding.EF.Models;
using System.IO;

namespace MD.Bidding.Helpers
{
    public class StringHelper
    {
        /// <summary>
        ///   Get full of media file
        /// </summary>
        /// <param name="text">Media Id.</param>
        /// <returns></returns>
        public static string ReversePath(string path, char delimeter)
        {
            var rPath = string.Empty;
            if (!string.IsNullOrEmpty(path))
            {
                string[] folders = path.Split(new char[] { delimeter }, StringSplitOptions.RemoveEmptyEntries);
                if (folders.Any())
                {
                    Array.Reverse(folders);
                    foreach (var f in folders)
                    {
                        rPath += delimeter + f;
                    }
                }
            }
            return rPath;
        }

        public static string FormatNames(AspNetUser aspNetUser)
        {
            if (aspNetUser != null)
                return aspNetUser.FirstName + " " + aspNetUser.LastName + " (" + aspNetUser.UserName + ")";
            else
                return string.Empty;
        }

        public static string[] SplitWords(string s)
        {
            return Regex.Split(s, @"\W+");
        }

        public static string URLFriendly(string title)
        {
            if (title == null) return "";

            const int maxlen = 80;
            int len = title.Length;
            bool prevdash = false;
            var sb = new StringBuilder(len);
            char c;

            for (int i = 0; i < len; i++)
            {
                c = title[i];
                if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'))
                {
                    sb.Append(c);
                    prevdash = false;
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    // tricky way to convert to lowercase
                    sb.Append((char)(c | 32));
                    prevdash = false;
                }
                else if (c == ' ' || c == ',' || c == '.' || c == '/' ||
                    c == '\\' || c == '-' || c == '_' || c == '=')
                {
                    if (!prevdash && sb.Length > 0)
                    {
                        sb.Append('-');
                        prevdash = true;
                    }
                }
                else if ((int)c >= 128)
                {
                    int prevlen = sb.Length;
                    sb.Append(RemapInternationalCharToAscii(c));
                    if (prevlen != sb.Length) prevdash = false;
                }
                if (i == maxlen) break;
            }

            if (prevdash)
                return sb.ToString().Substring(0, sb.Length - 1);
            else
                return sb.ToString();
        }

        public static string RemapInternationalCharToAscii(char c)
        {
            string s = c.ToString().ToLowerInvariant();
            if ("àåáâäãåą".Contains(s))
            {
                return "a";
            }
            else if ("èéêëę".Contains(s))
            {
                return "e";
            }
            else if ("ìíîïı".Contains(s))
            {
                return "i";
            }
            else if ("òóôõöøőð".Contains(s))
            {
                return "o";
            }
            else if ("ùúûüŭů".Contains(s))
            {
                return "u";
            }
            else if ("çćčĉ".Contains(s))
            {
                return "c";
            }
            else if ("żźž".Contains(s))
            {
                return "z";
            }
            else if ("śşšŝ".Contains(s))
            {
                return "s";
            }
            else if ("ñń".Contains(s))
            {
                return "n";
            }
            else if ("ýÿ".Contains(s))
            {
                return "y";
            }
            else if ("ğĝ".Contains(s))
            {
                return "g";
            }
            else if (c == 'ř')
            {
                return "r";
            }
            else if (c == 'ł')
            {
                return "l";
            }
            else if (c == 'đ')
            {
                return "d";
            }
            else if (c == 'ß')
            {
                return "ss";
            }
            else if (c == 'Þ')
            {
                return "th";
            }
            else if (c == 'ĥ')
            {
                return "h";
            }
            else if (c == 'ĵ')
            {
                return "j";
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Helper method to remove any characters from a string that aren't numeric characters
        /// </summary>
        /// <param name="text">The original text</param>
        /// <returns>The text after all non numeric characters have been removed</returns>
        public static string RemoveNonNumeric(string text)
        {
            var result = new StringBuilder(text.Length);

            foreach (char c in text)
            {
                if (c >= '0' && c <= '9')
                {
                    result.Append(c);
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// General Helper split search terms
        /// </summary>
        public static string[] TokenizePlainText(string text)
        {
            string delimitersString = " `~!@#$%^&*()_+-=[]{}\\|;':'\",<.>/?\n\r\t";
            char[] delimiters = delimitersString.Select(p => (char)p).ToArray();
            string[] tokens = text.Split(delimiters).Where(p => p.Length > 1).ToArray();
            return tokens;
        }

        /// <summary>
        /// Helper split search terms
        /// </summary>
        /// <param name="text">The original text</param>
        /// <returns>List of words</returns>
        public static string[] TokenizeSearchTerms(string text)
        {
            string delimitersString = " `~!@#$%^&*()_+-=[]{}\\|;:\",<.>/?\n\r\t";
            char[] delimiters = delimitersString.Select(p => (char)p).ToArray();
            string[] tokens = text.Split(delimiters).Where(p => p.Length > 1).ToArray();
            return tokens;
        }

        public static string SanitizeWords(string term, string[] excludedWords)
        {
            var words = StringHelper.TokenizeSearchTerms(term).ToList();
            var results = words.Except(excludedWords.ToList()).ToList();
            string cleanWords = string.Empty;
            foreach (var word in results)
            {
                cleanWords += word + " ";
            }

            cleanWords.Trim();
            return cleanWords;
        }
    }
}
