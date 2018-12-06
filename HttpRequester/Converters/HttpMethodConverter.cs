using System;
using System.Collections.Generic;
using System.Net.Http;

namespace HttpRequester.Converters
{
    /// <summary>
    /// Конвертер строки в http метод
    /// </summary>
    public class HttpMethodConverter
    {
        /// <summary>
        /// Словарь методов
        /// </summary>
        private readonly Dictionary<string, HttpMethod> _methods;

        /// <summary>
        /// Конструктор1
        /// </summary>
        /// <param name="list"></param>
        public HttpMethodConverter(HttpMethodConverterList list)
        {
            _methods = list.List;
        }

        /// <summary>
        /// Получить HttpMethod
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public HttpMethod GetMethod(string name)
        {
            var key = name.ToLower();

            if (!_methods.ContainsKey(key))
            {
                throw new Exception("Invalid method");
            }

            var method = _methods[key];

            return method;
        }
    }
}
