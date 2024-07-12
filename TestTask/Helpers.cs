using System.Linq;
using System.Text.RegularExpressions;

namespace TestTask
{
    public static class Helpers
    {
        private const string _vowels = "ауоыиэяюёеАУОЫИЭЯЮЁЕ";
        private const string _pattern = "[аА-яЯёЁ]";

        /// <summary>
        /// Определяем гласная или согласная буква/пара букв.
        /// </summary>
        /// <param name="letter">Буква/пара букв.</param>
        /// <returns></returns>
        public static CharType DefineLetterType(string letter) =>
            letter.Any(ch => _vowels.Contains(ch)) ? CharType.Vowels : CharType.Consonants;

        /// <summary>
        /// Определяем является ли буква/пара букв кирриллицей.
        /// </summary>
        /// <param name="letter">Буква/пара букв.</param>
        /// <returns></returns>
        public static bool IsRussianLetter(string letter) =>
            Regex.IsMatch(letter, _pattern);

        /// <summary>
        /// Сравниваем символы независимо от регистра.
        /// </summary>
        /// <param name="one">Первый символ.</param>
        /// <param name="two">Второй символ.</param>
        /// <returns></returns>
        public static bool CompareCharsCaseIgnore(char one, char two) =>
            char.ToUpperInvariant(one) == char.ToUpperInvariant(two);
    }
}
