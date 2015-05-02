using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TooDoo.Helpers
{
    public static class StringHelpers
    {
        public static string ToShortenedString(this string s, int numberOfCharacters)
        {
            if (s.Length >= numberOfCharacters)
            {
                return s.Substring(0, numberOfCharacters) + "...";
            }

            return s;
        }

        public static string ToShortDateTimeNullable(this DateTime? date)
        {
            if (date != null)
            {
                return date.Value.ToString("dd/MM/yyyy H:mm");
            }

            return null;
        }
    }
}