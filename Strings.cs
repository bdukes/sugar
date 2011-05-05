// 
//   SubSonic - http://subsonicproject.com
// 
//   The contents of this file are subject to the New BSD
//   License (the "License"); you may not use this file
//   except in compliance with the License. You may obtain a copy of
//   the License at http://www.opensource.org/licenses/bsd-license.php
//  
//   Software distributed under the License is distributed on an 
//   "AS IS" basis, WITHOUT WARRANTY OF ANY KIND, either express or
//   implied. See the License for the specific language governing
//   rights and limitations under the License.
// 
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

public static class Strings {

    public static bool Matches(this string source, string compare) {
        return String.Equals(source, compare, StringComparison.InvariantCultureIgnoreCase);
    }

    public static bool MatchesTrimmed(this string source, string compare) {
        return String.Equals(source.Trim(), compare.Trim(), StringComparison.InvariantCultureIgnoreCase);
    }

    public static bool MatchesRegex(this string inputString, string matchPattern) {
        return Regex.IsMatch(inputString, matchPattern,
            RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
    }

    /// <summary>
    /// Strips the last specified chars from a string.
    /// </summary>
    /// <param name="sourceString">The source string.</param>
    /// <param name="removeFromEnd">The remove from end.</param>
    /// <returns></returns>
    public static string Chop(this string sourceString, int removeFromEnd) {
        string result = sourceString;
        if ((removeFromEnd > 0) && (sourceString.Length > removeFromEnd - 1))
            result = result.Remove(sourceString.Length - removeFromEnd, removeFromEnd);
        return result;
    }

    /// <summary>
    /// Strips the last specified chars from a string.
    /// </summary>
    /// <param name="sourceString">The source string.</param>
    /// <param name="backDownTo">The back down to.</param>
    /// <returns></returns>
    public static string Chop(this string sourceString, string backDownTo) {
        int removeDownTo = sourceString.LastIndexOf(backDownTo);
        int removeFromEnd = 0;
        if (removeDownTo > 0)
            removeFromEnd = sourceString.Length - removeDownTo;

        string result = sourceString;

        if (sourceString.Length > removeFromEnd - 1)
            result = result.Remove(removeDownTo, removeFromEnd);

        return result;
    }

    /// <summary>
    /// Plurals to singular.
    /// </summary>
    /// <param name="sourceString">The source string.</param>
    /// <returns></returns>
    public static string PluralToSingular(this string sourceString) {
        return sourceString.MakeSingular();
    }

    /// <summary>
    /// Singulars to plural.
    /// </summary>
    /// <param name="sourceString">The source string.</param>
    /// <returns></returns>
    public static string SingularToPlural(this string sourceString) {
        return sourceString.MakePlural();
    }

    /// <summary>
    /// Make plural when count is not one
    /// </summary>
    /// <param name="number">The number of things</param>
    /// <param name="sourceString">The source string.</param>
    /// <returns></returns>
    public static string Pluralize(this int number, string sourceString) {
        if (number == 1)
            return String.Concat(number, " ", sourceString.MakeSingular());
        return String.Concat(number, " ", sourceString.MakePlural());
    }

    /// <summary>
    /// Removes the specified chars from the beginning of a string.
    /// </summary>
    /// <param name="sourceString">The source string.</param>
    /// <param name="removeFromBeginning">The remove from beginning.</param>
    /// <returns></returns>
    public static string Clip(this string sourceString, int removeFromBeginning) {
        string result = sourceString;
        if (sourceString.Length > removeFromBeginning)
            result = result.Remove(0, removeFromBeginning);
        return result;
    }

    /// <summary>
    /// Removes chars from the beginning of a string, up to the specified string
    /// </summary>
    /// <param name="sourceString">The source string.</param>
    /// <param name="removeUpTo">The remove up to.</param>
    /// <returns></returns>
    public static string Clip(this string sourceString, string removeUpTo) {
        int removeFromBeginning = sourceString.IndexOf(removeUpTo);
        string result = sourceString;

        if (sourceString.Length > removeFromBeginning && removeFromBeginning > 0)
            result = result.Remove(0, removeFromBeginning);

        return result;
    }

    /// <summary>
    /// Strips the last char from a a string.
    /// </summary>
    /// <param name="sourceString">The source string.</param>
    /// <returns></returns>
    public static string Chop(this string sourceString) {
        return Chop(sourceString, 1);
    }

    /// <summary>
    /// Strips the last char from a a string.
    /// </summary>
    /// <param name="sourceString">The source string.</param>
    /// <returns></returns>
    public static string Clip(this string sourceString) {
        return Clip(sourceString, 1);
    }

    /// <summary>
    /// Fasts the replace.
    /// </summary>
    /// <param name="original">The original.</param>
    /// <param name="pattern">The pattern.</param>
    /// <param name="replacement">The replacement.</param>
    /// <returns></returns>
    public static string FastReplace(this string original, string pattern, string replacement) {
        return FastReplace(original, pattern, replacement, StringComparison.InvariantCultureIgnoreCase);
    }

    /// <summary>
    /// Fasts the replace.
    /// </summary>
    /// <param name="original">The original.</param>
    /// <param name="pattern">The pattern.</param>
    /// <param name="replacement">The replacement.</param>
    /// <param name="comparisonType">Type of the comparison.</param>
    /// <returns></returns>
    public static string FastReplace(this string original, string pattern, string replacement,
                                        StringComparison comparisonType) {
        if (original == null)
            return null;

        if (String.IsNullOrEmpty(pattern))
            return original;

        int lenPattern = pattern.Length;
        int idxPattern = -1;
        int idxLast = 0;

        StringBuilder result = new StringBuilder();

        while (true) {
            idxPattern = original.IndexOf(pattern, idxPattern + 1, comparisonType);

            if (idxPattern < 0) {
                result.Append(original, idxLast, original.Length - idxLast);
                break;
            }

            result.Append(original, idxLast, idxPattern - idxLast);
            result.Append(replacement);

            idxLast = idxPattern + lenPattern;
        }

        return result.ToString();
    }

    /// <summary>
    /// Returns text that is located between the startText and endText tags.
    /// </summary>
    /// <param name="sourceString">The source string.</param>
    /// <param name="startText">The text from which to start the crop</param>
    /// <param name="endText">The endpoint of the crop</param>
    /// <returns></returns>
    public static string Crop(this string sourceString, string startText, string endText) {
        int startIndex = sourceString.IndexOf(startText, StringComparison.CurrentCultureIgnoreCase);
        if (startIndex == -1)
            return String.Empty;

        startIndex += startText.Length;
        int endIndex = sourceString.IndexOf(endText, startIndex, StringComparison.CurrentCultureIgnoreCase);
        if (endIndex == -1)
            return String.Empty;

        return sourceString.Substring(startIndex, endIndex - startIndex);
    }

    /// <summary>
    /// Removes excess white space in a string.
    /// </summary>
    /// <param name="sourceString">The source string.</param>
    /// <returns></returns>
    public static string Squeeze(this string sourceString) {
        char[] delim = { ' ' };
        string[] lines = sourceString.Split(delim, StringSplitOptions.RemoveEmptyEntries);
        StringBuilder sb = new StringBuilder();
        foreach (string s in lines) {
            if (!String.IsNullOrEmpty(s.Trim()))
                sb.Append(s + " ");
        }
        //remove the last pipe
        string result = Chop(sb.ToString());
        return result.Trim();
    }

    /// <summary>
    /// Removes all non-alpha numeric characters in a string
    /// </summary>
    /// <param name="sourceString">The source string.</param>
    /// <returns></returns>
    public static string ToAlphaNumericOnly(this string sourceString) {
        return Regex.Replace(sourceString, @"\W*", "");
    }

    /// <summary>
    /// Creates a string array based on the words in a sentence
    /// </summary>
    /// <param name="sourceString">The source string.</param>
    /// <returns></returns>
    public static string[] ToWords(this string sourceString) {
        string result = sourceString.Trim();
        return result.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
    }

    /// <summary>
    /// Strips all HTML tags from a string
    /// </summary>
    /// <param name="htmlString">The HTML string.</param>
    /// <returns></returns>
    public static string StripHTML(this string htmlString) {
        return StripHTML(htmlString, String.Empty);
    }

    /// <summary>
    /// Strips all HTML tags from a string and replaces the tags with the specified replacement
    /// </summary>
    /// <param name="htmlString">The HTML string.</param>
    /// <param name="htmlPlaceHolder">The HTML place holder.</param>
    /// <returns></returns>
    public static string StripHTML(this string htmlString, string htmlPlaceHolder) {
        const string pattern = @"<(.|\n)*?>";
        string sOut = Regex.Replace(htmlString, pattern, htmlPlaceHolder);
        sOut = sOut.Replace("&nbsp;", String.Empty);
        sOut = sOut.Replace("&amp;", "&");
        sOut = sOut.Replace("&gt;", ">");
        sOut = sOut.Replace("&lt;", "<");
        return sOut;
    }

    public static List<string> FindMatches(this string source, string find) {
        Regex reg = new Regex(find, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);

        List<string> result = new List<string>();
        foreach (Match m in reg.Matches(source))
            result.Add(m.Value);
        return result;
    }

    /// <summary>
    /// Converts a generic List collection to a single comma-delimitted string.
    /// </summary>
    /// <param name="list">The list.</param>
    /// <returns></returns>
    public static string ToDelimitedList(this IEnumerable<string> list) {
        return ToDelimitedList(list, ",");
    }

    /// <summary>
    /// Converts a generic List collection to a single string using the specified delimitter.
    /// </summary>
    /// <param name="list">The list.</param>
    /// <param name="delimiter">The delimiter.</param>
    /// <returns></returns>
    public static string ToDelimitedList(this IEnumerable<string> list, string delimiter) {
        StringBuilder sb = new StringBuilder();
        foreach (string s in list)
            sb.Append(String.Concat(s, delimiter));
        string result = sb.ToString();
        result = Chop(result);
        return result;
    }

    /// <summary>
    /// Strips the specified input.
    /// </summary>
    /// <param name="sourceString">The source string.</param>
    /// <param name="stripValue">The strip value.</param>
    /// <returns></returns>
    public static string Strip(this string sourceString, string stripValue) {
        if (!String.IsNullOrEmpty(stripValue)) {
            string[] replace = stripValue.Split(new[] { ',' });
            for (int i = 0; i < replace.Length; i++) {
                if (!String.IsNullOrEmpty(sourceString))
                    sourceString = Regex.Replace(sourceString, replace[i], String.Empty);
            }
        }
        return sourceString;
    }

    /// <summary>
    /// Converts ASCII encoding to Unicode
    /// </summary>
    /// <param name="asciiCode">The ASCII code.</param>
    /// <returns></returns>
    public static string AsciiToUnicode(this int asciiCode) {
        Encoding ascii = Encoding.UTF32;
        char c = (char)asciiCode;
        Byte[] b = ascii.GetBytes(c.ToString());
        return ascii.GetString((b));
    }



    /// <summary>
    /// Formats the args using String.Format with the target string as a format string.
    /// </summary>
    /// <param name="fmt">The format string passed to String.Format</param>
    /// <param name="args">The args passed to String.Format</param>
    /// <returns></returns>
    public static string ToFormattedString(this string fmt, params object[] args) {
        return String.Format(fmt, args);
    }

    /// <summary>
    /// Strings to enum.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Value">The value.</param>
    /// <returns></returns>
    public static T ToEnum<T>(this string Value) {
        T oOut = default(T);
        Type t = typeof(T);
        foreach (FieldInfo fi in t.GetFields()) {
            if (fi.Name.Matches(Value))
                oOut = (T)fi.GetValue(null);
        }

        return oOut;
    }
}


public static class Inflector {
    private static readonly List<InflectorRule> _plurals = new List<InflectorRule>();
    private static readonly List<InflectorRule> _singulars = new List<InflectorRule>();
    private static readonly List<string> _uncountables = new List<string>();

    /// <summary>
    /// Initializes the <see cref="Inflector"/> class.
    /// </summary>
    static Inflector() {
        AddPluralRule("$", "s");
        AddPluralRule("s$", "s");
        AddPluralRule("(ax|test)is$", "$1es");
        AddPluralRule("(octop|vir)us$", "$1i");
        AddPluralRule("(alias|status)$", "$1es");
        AddPluralRule("(bu)s$", "$1ses");
        AddPluralRule("(buffal|tomat)o$", "$1oes");
        AddPluralRule("([ti])um$", "$1a");
        AddPluralRule("sis$", "ses");
        AddPluralRule("(?:([^f])fe|([lr])f)$", "$1$2ves");
        AddPluralRule("(hive)$", "$1s");
        AddPluralRule("([^aeiouy]|qu)y$", "$1ies");
        AddPluralRule("(x|ch|ss|sh)$", "$1es");
        AddPluralRule("(matr|vert|ind)ix|ex$", "$1ices");
        AddPluralRule("([m|l])ouse$", "$1ice");
        AddPluralRule("^(ox)$", "$1en");
        AddPluralRule("(quiz)$", "$1zes");

        AddSingularRule("s$", String.Empty);
        AddSingularRule("ss$", "ss");
        AddSingularRule("(n)ews$", "$1ews");
        AddSingularRule("([ti])a$", "$1um");
        AddSingularRule("((a)naly|(b)a|(d)iagno|(p)arenthe|(p)rogno|(s)ynop|(t)he)ses$", "$1$2sis");
        AddSingularRule("(^analy)ses$", "$1sis");
        AddSingularRule("([^f])ves$", "$1fe");
        AddSingularRule("(hive)s$", "$1");
        AddSingularRule("(tive)s$", "$1");
        AddSingularRule("([lr])ves$", "$1f");
        AddSingularRule("([^aeiouy]|qu)ies$", "$1y");
        AddSingularRule("(s)eries$", "$1eries");
        AddSingularRule("(m)ovies$", "$1ovie");
        AddSingularRule("(x|ch|ss|sh)es$", "$1");
        AddSingularRule("([m|l])ice$", "$1ouse");
        AddSingularRule("(bus)es$", "$1");
        AddSingularRule("(o)es$", "$1");
        AddSingularRule("(shoe)s$", "$1");
        AddSingularRule("(cris|ax|test)es$", "$1is");
        AddSingularRule("(octop|vir)i$", "$1us");
        AddSingularRule("(alias|status)$", "$1");
        AddSingularRule("(alias|status)es$", "$1");
        AddSingularRule("^(ox)en", "$1");
        AddSingularRule("(vert|ind)ices$", "$1ex");
        AddSingularRule("(matr)ices$", "$1ix");
        AddSingularRule("(quiz)zes$", "$1");

        AddIrregularRule("person", "people");
        AddIrregularRule("man", "men");
        AddIrregularRule("child", "children");
        AddIrregularRule("sex", "sexes");
        AddIrregularRule("tax", "taxes");
        AddIrregularRule("move", "moves");

        AddUnknownCountRule("equipment");
        AddUnknownCountRule("information");
        AddUnknownCountRule("rice");
        AddUnknownCountRule("money");
        AddUnknownCountRule("species");
        AddUnknownCountRule("series");
        AddUnknownCountRule("fish");
        AddUnknownCountRule("sheep");
    }

    /// <summary>
    /// Adds the irregular rule.
    /// </summary>
    /// <param name="singular">The singular.</param>
    /// <param name="plural">The plural.</param>
    private static void AddIrregularRule(string singular, string plural) {
        AddPluralRule(String.Concat("(", singular[0], ")", singular.Substring(1), "$"),
            String.Concat("$1", plural.Substring(1)));
        AddSingularRule(String.Concat("(", plural[0], ")", plural.Substring(1), "$"),
            String.Concat("$1", singular.Substring(1)));
    }

    /// <summary>
    /// Adds the unknown count rule.
    /// </summary>
    /// <param name="word">The word.</param>
    private static void AddUnknownCountRule(string word) {
        _uncountables.Add(word.ToLower());
    }

    /// <summary>
    /// Adds the plural rule.
    /// </summary>
    /// <param name="rule">The rule.</param>
    /// <param name="replacement">The replacement.</param>
    private static void AddPluralRule(string rule, string replacement) {
        _plurals.Add(new InflectorRule(rule, replacement));
    }

    /// <summary>
    /// Adds the singular rule.
    /// </summary>
    /// <param name="rule">The rule.</param>
    /// <param name="replacement">The replacement.</param>
    private static void AddSingularRule(string rule, string replacement) {
        _singulars.Add(new InflectorRule(rule, replacement));
    }

    /// <summary>
    /// Makes the plural.
    /// </summary>
    /// <param name="word">The word.</param>
    /// <returns></returns>
    public static string MakePlural(this string word) {
        return ApplyRules(_plurals, word);
    }

    /// <summary>
    /// Makes the singular.
    /// </summary>
    /// <param name="word">The word.</param>
    /// <returns></returns>
    public static string MakeSingular(this string word) {
        return ApplyRules(_singulars, word);
    }

    /// <summary>
    /// Applies the rules.
    /// </summary>
    /// <param name="rules">The rules.</param>
    /// <param name="word">The word.</param>
    /// <returns></returns>
    private static string ApplyRules(IList<InflectorRule> rules, string word) {
        string result = word;
        if (!_uncountables.Contains(word.ToLower())) {
            for (int i = rules.Count - 1; i >= 0; i--) {
                string currentPass = rules[i].Apply(word);
                if (currentPass != null) {
                    result = currentPass;
                    break;
                }
            }
        }
        return result;
    }

    /// <summary>
    /// Converts the string to title case.
    /// </summary>
    /// <param name="word">The word.</param>
    /// <returns></returns>
    public static string ToTitleCase(this string word) {
        return Regex.Replace(Humanize(AddUnderscores(word)), @"\b([a-z])",
            match => match.Captures[0].Value.ToUpper());
    }

    /// <summary>
    /// Converts the string to human case.
    /// </summary>
    /// <param name="lowercaseAndUnderscoredWord">The lowercase and underscored word.</param>
    /// <returns></returns>
    public static string Humanize(this string lowercaseAndUnderscoredWord) {
        return MakeInitialCaps(Regex.Replace(lowercaseAndUnderscoredWord, @"_", " "));
    }

    /// <summary>
    /// Convert string to proper case
    /// </summary>
    /// <param name="sourceString">The source string.</param>
    /// <returns></returns>
    public static string ToProper(this string sourceString) {
        string propertyName = sourceString.ToPascalCase();
        return propertyName;
    }

    /// <summary>
    /// Converts the string to pascal case.
    /// </summary>
    /// <param name="lowercaseAndUnderscoredWord">The lowercase and underscored word.</param>
    /// <returns></returns>
    public static string ToPascalCase(this string lowercaseAndUnderscoredWord) {
        return ToPascalCase(lowercaseAndUnderscoredWord, true);
    }

    /// <summary>
    /// Converts text to pascal case...
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="removeUnderscores">if set to <c>true</c> [remove underscores].</param>
    /// <returns></returns>
    public static string ToPascalCase(this string text, bool removeUnderscores) {
        if (String.IsNullOrEmpty(text))
            return text;

        text = text.Replace("_", " ");
        string joinString = removeUnderscores ? String.Empty : "_";
        string[] words = text.Split(' ');
        if (words.Length > 1 || words[0].IsUpperCase()) {
            for (int i = 0; i < words.Length; i++) {
                if (words[i].Length > 0) {
                    string word = words[i];
                    string restOfWord = word.Substring(1);

                    if (restOfWord.IsUpperCase())
                        restOfWord = restOfWord.ToLower(CultureInfo.CurrentUICulture);

                    char firstChar = char.ToUpper(word[0], CultureInfo.CurrentUICulture);
                    words[i] = String.Concat(firstChar, restOfWord);
                }
            }
            return String.Join(joinString, words);
        }
        return String.Concat(words[0].Substring(0, 1).ToUpper(CultureInfo.CurrentUICulture), words[0].Substring(1));
    }

    /// <summary>
    /// Converts the string to camel case.
    /// </summary>
    /// <param name="lowercaseAndUnderscoredWord">The lowercase and underscored word.</param>
    /// <returns></returns>
    public static string ToCamelCase(this string lowercaseAndUnderscoredWord) {
        return MakeInitialLowerCase(ToPascalCase(lowercaseAndUnderscoredWord));
    }

    /// <summary>
    /// Adds the underscores.
    /// </summary>
    /// <param name="pascalCasedWord">The pascal cased word.</param>
    /// <returns></returns>
    public static string AddUnderscores(this string pascalCasedWord) {
        return
            Regex.Replace(
                Regex.Replace(Regex.Replace(pascalCasedWord, @"([A-Z]+)([A-Z][a-z])", "$1_$2"), @"([a-z\d])([A-Z])",
                    "$1_$2"), @"[-\s]", "_").ToLower();
    }

    /// <summary>
    /// Makes the initial caps.
    /// </summary>
    /// <param name="word">The word.</param>
    /// <returns></returns>
    public static string MakeInitialCaps(this string word) {
        return String.Concat(word.Substring(0, 1).ToUpper(), word.Substring(1).ToLower());
    }

    /// <summary>
    /// Makes the initial lower case.
    /// </summary>
    /// <param name="word">The word.</param>
    /// <returns></returns>
    public static string MakeInitialLowerCase(this string word) {
        return String.Concat(word.Substring(0, 1).ToLower(), word.Substring(1));
    }

    /// <summary>
    /// Adds the ordinal suffix.
    /// </summary>
    /// <param name="number">The number.</param>
    /// <returns></returns>
    public static string AddOrdinalSuffix(this string number) {
        if (number.IsNumeric()) {
            int n = int.Parse(number);
            int nMod100 = n % 100;

            if (nMod100 >= 11 && nMod100 <= 13)
                return String.Concat(number, "th");

            switch (n % 10) {
                case 1:
                    return String.Concat(number, "st");
                case 2:
                    return String.Concat(number, "nd");
                case 3:
                    return String.Concat(number, "rd");
                default:
                    return String.Concat(number, "th");
            }
        }
        return number;
    }

    /// <summary>
    /// Converts the underscores to dashes.
    /// </summary>
    /// <param name="underscoredWord">The underscored word.</param>
    /// <returns></returns>
    public static string ConvertUnderscoresToDashes(this string underscoredWord) {
        return underscoredWord.Replace('_', '-');
    }


    #region Nested type: InflectorRule

    /// <summary>
    /// Summary for the InflectorRule class
    /// </summary>
    private class InflectorRule {
        /// <summary>
        /// 
        /// </summary>
        public readonly Regex regex;

        /// <summary>
        /// 
        /// </summary>
        public readonly string replacement;

        /// <summary>
        /// Initializes a new instance of the <see cref="InflectorRule"/> class.
        /// </summary>
        /// <param name="regexPattern">The regex pattern.</param>
        /// <param name="replacementText">The replacement text.</param>
        public InflectorRule(string regexPattern, string replacementText) {
            regex = new Regex(regexPattern, RegexOptions.IgnoreCase);
            replacement = replacementText;
        }

        /// <summary>
        /// Applies the specified word.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <returns></returns>
        public string Apply(string word) {
            if (!regex.IsMatch(word))
                return null;

            string replace = regex.Replace(word, replacement);
            if (word == word.ToUpper())
                replace = replace.ToUpper();

            return replace;
        }
    }

    #endregion
}