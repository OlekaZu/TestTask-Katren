using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TestTask
{
    public class Program
    {

        /// <summary>
        /// Программа принимает на входе 2 пути до файлов.
        /// Анализирует в первом файле кол-во вхождений каждой буквы (регистрозависимо). Например А, б, Б, Г и т.д.
        /// Анализирует во втором файле кол-во вхождений парных букв (не регистрозависимо). Например АА, Оо, еЕ, тт и т.д.
        /// По окончанию работы - выводит данную статистику на экран.
        /// </summary>
        /// <param name="args">Первый параметр - путь до первого файла.
        /// Второй параметр - путь до второго файла.</param>
        private static void Main(string[] args)
        {
            IReadOnlyStream inputStream1 = new ReadOnlyStream(args[0]);
            IReadOnlyStream inputStream2 = new ReadOnlyStream(args[1]);

            IList<LetterStats> singleLetterStats = FillSingleLetterStats(inputStream1);
            IList<LetterStats> doubleLetterStats = FillDoubleLetterStats(inputStream2);

            RemoveCharStatsByType(singleLetterStats, CharType.Vowels);
            RemoveCharStatsByType(doubleLetterStats, CharType.Consonants);

            PrintStatistic(singleLetterStats);
            PrintStatistic(doubleLetterStats);

            // TODO : Необжодимо дождаться нажатия клавиши, прежде чем завершать выполнение программы.
            // DONE : Ждем нажатия клавиши Enter.
            Console.Write("Press <Enter> to exit.");
            while (Console.ReadKey().Key != ConsoleKey.Enter)
            { }
            inputStream1.CloseStream();
            inputStream2.CloseStream();
        }

        /// <summary>
        /// Ф-ция считывающая из входящего потока все буквы, и возвращающая коллекцию статистик вхождения каждой буквы.
        /// Статистика РЕГИСТРОЗАВИСИМАЯ!
        /// </summary>
        /// <param name="stream">Стрим для считывания символов для последующего анализа</param>
        /// <returns>Коллекция статистик по каждой букве, что была прочитана из стрима.</returns>
        private static IList<LetterStats> FillSingleLetterStats(IReadOnlyStream stream)
        {
            stream.ResetPositionToStart();
            IList<LetterStats> list = new List<LetterStats>();
            while (!stream.IsEof)
            {
                try
                {
                    string letter = stream.ReadNextChar().ToString();
                    //Console.WriteLine(letter);
                    // TODO : заполнять статистику с использованием метода IncStatistic. Учёт букв - регистрозависимый.
                    // DONE : Проверяем русская буква или нет. Заполняем статистику, если буква уже существует в списке.
                    // Если нет - создаем новую статистику и добавляем её в список.
                    if (Helpers.IsRussianLetter(letter))
                        FormStaticticList(list, letter);
                }
                catch (EndOfStreamException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return list;
        }

        /// <summary>
        /// Ф-ция считывающая из входящего потока все буквы, и возвращающая коллекцию статистик вхождения парных букв.
        /// В статистику должны попадать только пары из одинаковых букв, например АА, СС, УУ, ЕЕ и т.д.
        /// Статистика - НЕ регистрозависимая!
        /// </summary>
        /// <param name="stream">Стрим для считывания символов для последующего анализа</param>
        /// <returns>Коллекция статистик по каждой букве, что была прочитана из стрима.</returns>
        private static IList<LetterStats> FillDoubleLetterStats(IReadOnlyStream stream)
        {
            stream.ResetPositionToStart();
            IList<LetterStats> list = new List<LetterStats>();
            try
            {
                char chPrevious = stream.ReadNextChar();
                while (!stream.IsEof)
                {
                    // TODO : заполнять статистику с использованием метода IncStatistic. Учёт букв - НЕ регистрозависимый.
                    // DONE : Проверяем парные ли буквы, проверяем русские ли буквы и заполняем статистику.
                    char chNext = stream.ReadNextChar();
                    if (Helpers.CompareCharsCaseIgnore(chPrevious, chNext)
                        && Helpers.IsRussianLetter(chNext.ToString()))
                    {
                        var letter = chPrevious.ToString() + chNext.ToString();
                        FormStaticticList(list, letter);
                        chPrevious = stream.ReadNextChar();
                    }
                    else
                    {
                        chPrevious = chNext;
                    }
                }
            }
            catch (EndOfStreamException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return list;
        }

        /// <summary>
        /// Ф-ция перебирает все найденные буквы/парные буквы, содержащие в себе только гласные или согласные буквы.
        /// (Тип букв для перебора определяется параметром charType)
        /// Все найденные буквы/пары соответствующие параметру поиска - удаляются из переданной коллекции статистик.
        /// </summary>
        /// <param name="letters">Коллекция со статистиками вхождения букв/пар</param>
        /// <param name="charType">Тип букв для анализа</param>
        private static void RemoveCharStatsByType(IList<LetterStats> letters, CharType charType)
        {
            // TODO : Удалить статистику по запрошенному типу букв.
            // DONE : Удаляем все буквы/пары, содержащие в себе только гласные или согласные буквы.
            for (int i = letters.Count - 1; i >= 0; --i)
            {
                if (letters[i].LetterType == charType)
                    letters.RemoveAt(i);
            }
        }

        /// <summary>
        /// Ф-ция выводит на экран полученную статистику в формате "{Буква} : {Кол-во}. {Тип}"
        /// Каждая буква - с новой строки.
        /// Выводить на экран необходимо предварительно отсортировав набор по алфавиту.
        /// В конце отдельная строчка с ИТОГО, содержащая в себе общее кол-во найденных букв/пар
        /// </summary>
        /// <param name="letters">Коллекция со статистикой</param>
        private static void PrintStatistic(IEnumerable<LetterStats> letters)
        {
            // TODO : Выводить на экран статистику. Выводить предварительно отсортировав по алфавиту!
            // DONE: Формат вывода "{Буква} : {Кол-во}. {Тип}" в алфавитном порядке. В конце строчка - ИТОГО.
            Console.WriteLine("Итоговая статистика:");
            var sorted = letters.OrderBy(x => x.Letter);
            foreach (LetterStats letterStats in sorted)
                Console.WriteLine(letterStats.ToString());
            Console.WriteLine("Итого: {0}", sorted.Sum(x => x.Count));
        }

        /// <summary>
        /// Формируем статистику по текущей букве/паре букв, добавляем её в коллекцию, 
        /// если такой буквы в ней ещё нет или увеличиваем счётчик статистики, если коллекция
        /// уже содержит такую статистику.
        /// </summary>
        /// <param name="list">Коллекция со статистиками вхождения букв/пар</param>
        /// <param name="letter">Буква/пара букв</param>
        private static void FormStaticticList(IList<LetterStats> list, string letter)
        {
            var letterStats = list.FirstOrDefault(x => x.Letter.Equals(letter));
            if (String.IsNullOrEmpty(letterStats.Letter))
            {
                list.Add(new LetterStats()
                {
                    Letter = letter,
                    LetterType = Helpers.DefineLetterType(letter),
                    Count = 1
                });
            }
            else
            {
                var index = list.IndexOf(letterStats);
                IncStatistic(ref letterStats);
                list[index] = letterStats;
            }
        }

        /// <summary>
        /// Метод увеличивает счётчик вхождений по переданной структуре.
        /// </summary>
        /// <param name="letterStats"></param>
        private static void IncStatistic(ref LetterStats letterStats)
        {
            letterStats.Count++;
        }
    }
}
