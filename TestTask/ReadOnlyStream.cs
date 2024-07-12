using System.IO;
using System.Text;

namespace TestTask
{
    public class ReadOnlyStream : IReadOnlyStream
    {
        private Stream _localStream;
        private StreamReader _sReader;

        /// <summary>
        /// Флаг окончания файла.
        /// </summary>
        // TODO : Заполнять данный флаг при достижении конца файла/стрима при чтении
        // DONE : При создании - true, при успешном открытии стрима - false,
        // при достижении конца - true
        public bool IsEof { get; private set; }

        /// <summary>
        /// Конструктор класса. 
        /// Т.к. происходит прямая работа с файлом, необходимо 
        /// обеспечить ГАРАНТИРОВАННОЕ закрытие файла после окончания работы с таковым!
        /// </summary>
        /// <param name="fileFullPath">Полный путь до файла для чтения</param>
        public ReadOnlyStream(string fileFullPath)
        {
            IsEof = true;

            // TODO : Заменить на создание реального стрима для чтения файла!
            // DONE : Открываем стрим для чтения cуществующего файла,
            // расположенный по полному пути fileFullPath
            if (File.Exists(fileFullPath))
            {
                _localStream = File.OpenRead(fileFullPath);
                IsEof = false;
                _sReader = new StreamReader(_localStream, Encoding.UTF8, true);
            }
        }


        /// <summary>
        /// Ф-ция чтения следующего символа из потока.
        /// Если произведена попытка прочитать символ после достижения конца файла, метод 
        /// должен бросать соответствующее исключение
        /// </summary>
        /// <returns>Считанный символ.</returns>
        public char ReadNextChar()
        {
            // TODO : Необходимо считать очередной символ из _localStream
            // DONE : Считываем по байтам, если достигнем EOF - бросаем исключение
            var ch = _sReader.Read();
            if (ch == -1)
            {
                IsEof = true;
                throw new EndOfStreamException("EOF");
            }
            return (char)ch;
        }

        /// <summary>
        /// Сбрасывает текущую позицию потока на начало, если стрим существует.
        /// </summary>
        public void ResetPositionToStart()
        {
            if (!IfStreamExist())
            {
                IsEof = true;
                return;
            }
            _localStream.Position = 0;
            IsEof = false;
        }

        /// <summary>
        /// Корректно закрывает существующий поток. Присваивает флагу IsEof значение true.
        /// </summary>
        public void CloseStream()
        {
            IsEof = true;
            if (!IfStreamExist())
                return;
            _localStream.Close();
            _localStream = null;
            _sReader.Close();
            _sReader = null;
        }

        /// <summary>
        /// Проверяет существует ли поток.
        /// </summary>
        /// <returns></returns>
        private bool IfStreamExist() => _localStream != null;
    }
}
