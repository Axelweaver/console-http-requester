using System;
using HttpRequester.Helpers;

namespace HttpRequester
{
    /// <summary>
    /// Отправка http запросов
    /// </summary>
    class Program
    {
        /// <summary>
        /// Вход в программу
        /// </summary>
        /// <param name="args">Аргументы</param>
        static void Main(string[] args)
        {
            var consoleHelper = new ConsoleHelper();

            if (args == null || args.Length < 2)
            {
                consoleHelper.WriteMessageWithTimeStamp("Не хватает аргументов при запуске", ConsoleColor.DarkRed);

                return;
            }

            var method = args[0];
            var url = args[1];

            var jsonConverter = new JsonConvertAdapter();
            var requestHelper = new RequestHelper(jsonConverter);

            var login = Properties.Settings.Default.login;
            var password = Properties.Settings.Default.password;

            try
            {
                object responseObject = null;

                if (method.Equals("post", StringComparison.OrdinalIgnoreCase))
                {
                    consoleHelper.WriteMessageWithTimeStamp($"Посылаем POST запрос на адрес {url}");
                    responseObject = requestHelper.Post<object>(url, null, login, password).Result;
                }

                if (method.Equals("get", StringComparison.OrdinalIgnoreCase))
                {
                    consoleHelper.WriteMessageWithTimeStamp($"Посылаем GET запрос на адрес {url}");
                    responseObject = requestHelper.Get<object>(url, login, password).Result;
                }

                consoleHelper.WriteMessageWithTimeStamp($"Сервер вернул ответ:\n{responseObject}", ConsoleColor.DarkGreen);
            }
            catch (Exception e)
            {
                consoleHelper.WriteMessageWithTimeStamp($"{e.Message}\n{e}", ConsoleColor.Red);

                consoleHelper.ReadLine();
            }
        }
    }
}
