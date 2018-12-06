using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HttpRequester.Helpers
{
    /// <summary>
    /// Работа с http запросами
    /// </summary>
    public class RequestHelper
    {
        /// <summary>
        /// Конвертер json
        /// </summary>
        private readonly JsonConvertAdapter _jsonConvertAdapter;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="jsonConvertAdapter">Конвертер json</param>
        public RequestHelper(JsonConvertAdapter jsonConvertAdapter)
        {
            _jsonConvertAdapter = jsonConvertAdapter;
        }

        /// <summary>
        /// Отравить запрос методом POST
        /// </summary>
        /// <typeparam name="T">Тип возвращаемого объекта</typeparam>
        /// <param name="url">Адрес запроса</param>
        /// <param name="content">Объект тела запроса</param>
        /// <param name="username">Имя пользователя</param>
        /// <param name="password">Пароль</param>
        /// <returns></returns>
        public async Task<T> Post<T>(string url, object content, string username = null, string password = null)
        {
            using (var httpClient = new HttpClient())
            {
                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    SetAuth(httpClient, username, password);
                }

                var uri = new Uri(url);

                var json = content is null ? string.Empty : _jsonConvertAdapter.Serialize(content);

                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync(uri, httpContent))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"{response.StatusCode} {response.ReasonPhrase}\n{response}");
                    }

                    T result = await GetObjectFromResponse<T>(response);

                    return result;
                }
            }
        }

        /// <summary>
        /// Отправить http запрос методом GET
        /// </summary>
        /// <typeparam name="T">Тип возвращаемого объекта</typeparam>
        /// <param name="url">Адрес запроса</param>
        /// <param name="username">Имя пользователя</param>
        /// <param name="password">Пароль</param>
        /// <returns></returns>
        public async Task<T> Get<T>(string url, string username = null, string password = null)
        {
            using (var httpClient = new HttpClient())
            {
                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    SetAuth(httpClient, username, password);
                }

                var uri = new Uri(url);

                using (var response = await httpClient.GetAsync(uri))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"{response.StatusCode} {response.ReasonPhrase}\n{response}");
                    }

                    T result = await GetObjectFromResponse<T>(response);

                    return result;
                }
            }
        }

        /// <summary>
        /// Получить объект из ответа http запроса
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="response">Ответ сервера</param>
        /// <returns></returns>
        private async Task<T> GetObjectFromResponse<T>(HttpResponseMessage response)
        {
            var resultString = await response.Content.ReadAsStringAsync();

            T result = _jsonConvertAdapter.Deserialize<T>(resultString);

            return result;
        }

        /// <summary>
        /// Установить авторизацию
        /// </summary>
        /// <param name="httpClient">http клиент</param>
        /// <param name="username">Логин</param>
        /// <param name="password">Пароль</param>
        private void SetAuth(HttpClient httpClient, string username, string password)
        {
            var phrase = $"{username}:{password}";

            var bytesPhrase = Encoding.UTF8.GetBytes(phrase);

            var base64StringPhrase = Convert.ToBase64String(bytesPhrase);

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64StringPhrase);
        }
    }
}
