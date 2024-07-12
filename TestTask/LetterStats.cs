namespace TestTask
{
    /// <summary>
    /// Статистика вхождения буквы/пары букв
    /// </summary>
    public struct LetterStats
    {
        /// <summary>
        /// Буква/Пара букв для учёта статистики.
        /// </summary>
        public string Letter;

        /// <summary>
        /// Кол-во вхождений буквы/пары.
        /// </summary>
        public int Count;

        /// <summary>
        /// Гласная или согласная буква/пара.
        /// </summary>
        public CharType LetterType;

        public override string ToString()
        {
            return $"{Letter}: {Count}. {LetterType}";
        }
    }
}
