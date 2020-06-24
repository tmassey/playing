using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace playing.Core.Extentions
{
    public static class StringExtensions
    {
        public static bool IsNumeric(this string value)
        {
            double num;
            return (Double.TryParse(value, out num));
        }

        public static string CleanNumberString(this string number)
        {
            if (number.Contains("."))
                return number.Trim('0').Trim('.');
            else return number.TrimStart('0');
        }

        public static Guid? ToGuid(this string id)
        {
            if (String.IsNullOrEmpty(id))
                return null;

            Guid identityID;
            if (!Guid.TryParse(id, out identityID))
                return null;

            return identityID;
        }

        public static string Pluralize(this string s)
        {
            return Pluralizer.Pluralize(2, s);
        }

        public static String Singularize(this String s)
        {
            return Singularizer.Singularize(s);
        }

        public static string GetDigits(this String s)
        {
            if (String.IsNullOrEmpty(s))
                return "";
            return Regex.Replace(s, "[^0-9]", "");
        }

        public static string SqlParameterSafetyCheckAndUppercase(this string sql)
        {
            return sql == null ? null : sql.Replace("%", "").Replace("@", "").ToUpper().Trim();
        }

        public static int? SqlParameterSafetyCheckNullableInt(this string sql)
        {
            if (String.IsNullOrEmpty(sql))
                return null;

            int value = 0;
            Int32.TryParse(sql, out value);
            if (value > 0)
            {
                return value;
            }

            return null;
        }

        public static string Right(this string sValue, int iMaxLength)
        {
            //Check if the value is valid
            if (String.IsNullOrEmpty(sValue))
            {
                //Set valid empty string as string could be null
                sValue = String.Empty;
            }
            else if (sValue.Length > iMaxLength)
            {
                //Make the string no longer than the max length
                sValue = sValue.Substring(sValue.Length - iMaxLength, iMaxLength);
            }

            //Return the string
            return sValue;
        }

        public static string TrimNonalphanumeric(this string sValue)
        {
            if (String.IsNullOrEmpty(sValue))
                return sValue;

            var rgx = new Regex("[^a-zA-Z0-9]");
            sValue = rgx.Replace(sValue, "");
            return sValue;
        }


        public static string TrimPhoneNumber(this string sValue)
        {
            //// Can't decided how to format.

            //if (string.IsNullOrEmpty(sValue))
            //    return sValue;

            //var rgx = new Regex("[^X0-9, ,//-]");
            //sValue = rgx.Replace(sValue, "");

            return sValue;
        }

        public static string FormatPhoneNumberWithPhoenixes(this string phoneNumber)
        {
            if (IsDefinitelyNotAPhoneNumber(phoneNumber)) return phoneNumber;

            var rgx = new Regex("[^X0-9,//g,-]");
            var trimmedPhoneNumber = rgx.Replace(phoneNumber, "").Replace("-", "");
            if (trimmedPhoneNumber.Length == 10)
                return trimmedPhoneNumber.Insert(3, "-").Insert(7, "-");
            if (trimmedPhoneNumber.Length == 7)
                return trimmedPhoneNumber.Insert(3, "-");

            return phoneNumber;
        }

        private static bool IsDefinitelyNotAPhoneNumber(string phoneNumber)
        {
            // a string is definitely not a phone number if it has any letters or an "@" in it
            return Regex.IsMatch(phoneNumber, @"[a-zA-Z@]+");
        }

        public static DateTime ToDateTimeFromYearMonth(this string ym)
        {
            var nullDate = new DateTime(1900, 1, 1);

            if (String.IsNullOrEmpty(ym) || ym.Length != 6)
                return nullDate;

            var y = ym.Substring(0, 4);
            var m = ym.Substring(4, 2);

            int year;
            int month;
            if (Int32.TryParse(y, out year) == false)
                return nullDate;
            if (Int32.TryParse(m, out month) == false)
                return nullDate;

            return new DateTime(year, month, 1);
        }

        public class Singularizer
        {
            private static readonly IList<string> Unpluralizables =
                new List<string>
                {
                    "equipment",
                    "information",
                    "rice",
                    "money",
                    "species",
                    "series",
                    "fish",
                    "sheep",
                    "deer"
                };

            private static readonly IDictionary<string, string> Singularizations =
                new Dictionary<string, string>
                {
                    // Start with the rarest cases, and move to the most common
                    {"people", "person"},
                    {"oxen", "ox"},
                    {"children", "child"},
                    {"feet", "foot"},
                    {"teeth", "tooth"},
                    {"geese", "goose"},
                    // And now the more standard rules.
                    {"(.*)ives?", "$1ife"},
                    {"(.*)ves?", "$1f"},
                    // ie, wolf, wife
                    {"(.*)men$", "$1man"},
                    {"(.+[aeiou])ys$", "$1y"},
                    {"(.+[^aeiou])ies$", "$1y"},
                    {"(.+)zes$", "$1"},
                    {"([m|l])ice$", "$1ouse"},
                    {"matrices", @"matrix"},
                    {"indices", @"index"},
                    {"(.+[^aeiou])ices$", "$1ice"},
                    {"(.*)ices$", @"$1ex"},
                    // ie, Matrix, Index
                    {"(octop|vir)i$", "$1us"},
                    {"(.+(s|x|sh|ch))es$", @"$1"},
                    {"(.+)s$", @"$1"}
                };

            public static string Singularize(string word)
            {
                if (Unpluralizables.Contains(word.ToLowerInvariant()))
                {
                    return word;
                }

                if (word.ToLowerInvariant().EndsWith("ss"))
                {
                    return word;
                }

                foreach (var singularization in Singularizations)
                {
                    if (Regex.IsMatch(word, singularization.Key))
                    {
                        return Regex.Replace(word, singularization.Key, singularization.Value);
                    }
                }

                return word;
            }

            public static bool IsPlural(string word)
            {
                if (Unpluralizables.Contains(word.ToLowerInvariant()))
                {
                    return true;
                }

                foreach (var singularization in Singularizations)
                {
                    if (Regex.IsMatch(word, singularization.Key))
                    {
                        return true;
                    }
                }

                return false;
            }

        }

        public static class Pluralizer
        {
            private static readonly IList<string> Unpluralizables = new List<string>
            {
                "equipment",
                "information",
                "rice",
                "money",
                "species",
                "series",
                "fish",
                "sheep",
                "deer"
            };

            private static readonly IDictionary<string, string> Pluralizations = new Dictionary<string, string>
            {
                // Start with the rarest cases, and move to the most common
                {"person", "people"},
                {"ox", "oxen"},
                {"child", "children"},
                {"foot", "feet"},
                {"tooth", "teeth"},
                {"goose", "geese"},
                // And now the more standard rules.
                {"(.*)fe?$", "$1ves"}, // ie, wolf, wife
                {"(.*)man$", "$1men"},
                {"(.+[aeiou]y)$", "$1s"},
                {"(.+[^aeiou])y$", "$1ies"},
                {"(.+z)$", "$1zes"},
                {"([m|l])ouse$", "$1ice"},
                {"(.+)(e|i)x$", @"$1ices"}, // ie, Matrix, Index
                {"(octop|vir)us$", "$1i"},
                {"(.+(s|x|sh|ch))$", @"$1es"},
                {"(.+)", @"$1s"}
            };

            public static string Pluralize(int count, string singular)
            {
                if (count == 1)
                    return singular;

                if (Unpluralizables.Contains(singular))
                    return singular;

                var plural = "";

                foreach (var pluralization in Pluralizations)
                {
                    if (Regex.IsMatch(singular, pluralization.Key))
                    {
                        plural = Regex.Replace(singular, pluralization.Key, pluralization.Value);
                        break;
                    }
                }

                return plural;
            }
        }
    }
}