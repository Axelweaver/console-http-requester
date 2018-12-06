using System;
using HttpRequester.Adapters;
using HttpRequester.Converters;
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

            var httpMethodConverterList = new HttpMethodConverterList();
            var httpMethodConverter = new HttpMethodConverter(httpMethodConverterList);

            var method = httpMethodConverter.GetMethod(args[0]);
            var url = args[1];

            var jsonConverter = new JsonConvertAdapter();
            var requestHelper = new RequestHelper(jsonConverter);

            var login = Properties.Settings.Default.login;
            var password = Properties.Settings.Default.password;

            try
            {
                consoleHelper.WriteMessageWithTimeStamp($"Посылаем {args[0].ToUpper()} запрос на адрес {url}");

                object responseObject = requestHelper.SendRequest(method, url, null, login, password);

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
