using System;

namespace HttpRequester.Helpers
{
    /// <summary>
    /// Работа с консолью
    /// </summary>
    public class ConsoleHelper
    {
        /// <summary>
        /// Считать строку из консоли
        /// </summary>
        /// <returns></returns>
        public string ReadLine()
        {
            return Console.ReadLine();
        }

        /// <summary>
        /// Спросить пользователя продолжить
        /// </summary>
        /// <returns></returns>
        public bool GetToStop()
        {
            if (!GetYorN("Продолжить?"))
            {
                WriteMessageWithTimeStamp("Обработка остановлена пользователем");
                Console.ReadLine();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Запросить пользователя Y/N
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="defaultYes">Значение по умолчаню Y</param>
        /// <returns></returns>
        public bool GetYorN(string message, bool defaultYes = true)
        {
            var isCorrectInput = false;
            while (!isCorrectInput)
            {
                var input = GetStringFromConsole($"{message} Y/N (default {(defaultYes ? "Y" : "N")})", true);

                isCorrectInput = string.IsNullOrEmpty(input) ||
                                 input.Equals("Y", StringComparison.OrdinalIgnoreCase) ||
                                 input.Equals("N", StringComparison.OrdinalIgnoreCase);



                if (!string.IsNullOrEmpty(input) &&
                    input.Equals((!defaultYes ? "Y" : "N"), StringComparison.OrdinalIgnoreCase))
                {
                    return !defaultYes;
                }

                if (!isCorrectInput)
                {
                    WriteMessageWithTimeStamp("Некорретный ввод. Повторите", ConsoleColor.Red);
                }
            }

            return defaultYes;
        }
        /// <summary>
        /// Получить строку из консоли
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="maybeNull">Ввод может быть пустым</param>
        /// <returns></returns>
        public string GetStringFromConsole(string message, bool maybeNull = false)
        {
            string value = null;

            while (string.IsNullOrEmpty(value))
            {
                WriteMessageWithTimeStamp(message, ConsoleColor.Yellow);
                value = Console.ReadLine();

                if (string.IsNullOrEmpty(value) && !maybeNull)
                {
                    WriteMessageWithTimeStamp("Некорректный ввод, введите ещё раз", ConsoleColor.Red);
                }
                else if (string.IsNullOrEmpty(value) && maybeNull)
                {
                    return null;
                }
            }

            return value;
        }

        /// <summary>
        /// Получить численное значение
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="defaultValue">Значение по умолчанию</param>
        /// <returns></returns>
        public int GetIntFromConsole(string message, int defaultValue = 0)
        {
            int value = 0;

            var isParsed = false;

            while (!isParsed)
            {
                var defaultMessage = defaultValue == 0
                    ? null
                    : $" (default {defaultValue})";

                WriteMessageWithTimeStamp($"{message}{defaultMessage}", ConsoleColor.Yellow);

                var stingValue = Console.ReadLine();

                isParsed = int.TryParse(stingValue, out value);

                if (!isParsed && defaultValue == 0)
                {
                    WriteMessageWithTimeStamp("Некорректный ввод, введите ещё раз", ConsoleColor.Red);
                }
                else if (!isParsed && defaultValue != 0)
                {
                    return defaultValue;
                }
            }

            return value;
        }
        /// <summary>
        /// Отобразить сообщение в консоли с датой и временем
        /// </summary>
        /// <param name="message"></param>
        /// <param name="color"></param>
        public void WriteMessageWithTimeStamp(string message, ConsoleColor? color = ConsoleColor.White)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write($"[{DateTime.Now:dd.MM.yyyy HH:mm:ss:ffffff}]");
            Console.ForegroundColor = color ?? ConsoleColor.White;
            Console.Write($" {message}\n");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
