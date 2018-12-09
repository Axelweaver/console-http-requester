using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using HttpRequester.Adapters;

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
        /// Послать http запрос
        /// </summary>
        /// <typeparam name="T">Тип возращаемого объекта</typeparam>
        /// <param name="method">Метод</param>
        /// <param name="url">URL адрес</param>
        /// <param name="content">Объек для сериализации (json)</param>
        /// <param name="username">Логин</param>
        /// <param name="password">Пароль</param>
        /// <param name="mediaType">Тип тела запроса</param>
        /// <returns></returns>
        public async Task<T> SendRequest<T>(HttpMethod method, 
                                            string url, 
                                            object content = null, 
                                            string username = null, 
                                            string password = null,
                                            string mediaType = "application/json")
        {
            using (var httpClient = GetHttpClient(username, password))
            {
                var uri = new Uri(url);

                var requestMessage = new HttpRequestMessage(method, uri);

                if (content != null)
                {

                    if (content is string)
                    {
                        requestMessage.Content = new StringContent((string)content, Encoding.UTF8, mediaType);                          
                    }
                    else
                    {
                        var json =  _jsonConvertAdapter.Serialize(content);

                        requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");                        
                    }

                }
                

                using (var response = await httpClient.SendAsync(requestMessage))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"{response.StatusCode} {response.ReasonPhrase}\n{response}");
                    }

                    var result = await GetObjectFromResponse<T>(response);

                    return result;
                }
            }
        }

        /// <summary>
        /// Послать http запрос
        /// </summary>
        /// <param name="method">Метод</param>
        /// <param name="url">URL адрес</param>
        /// <param name="content">Объек для сериализации (json)</param>
        /// <param name="username">Логин</param>
        /// <param name="password">Пароль</param>
        /// <param name="mediaType">Тип тела запроса</param>
        /// <returns></returns>
        public async Task SendRequest(HttpMethod method,
                                      string url,
                                      object content = null,
                                      string username = null,
                                      string password = null,
                                      string mediaType = null)
        {
            await SendRequest<object>(method, url, content, username, password, mediaType);
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
            return await SendRequest<T>(HttpMethod.Post, url, content, username, password);
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
            return await SendRequest<T>(HttpMethod.Get, url, null, username, password);
        }

        /// <summary>
        /// Получить httpClient с заданием BasicAuth
        /// </summary>
        /// <param name="username">Логин</param>
        /// <param name="password">Пароль</param>
        /// <returns></returns>
        public HttpClient GetHttpClient(string username = null, string password = null)
        {
            var httpClient = new HttpClient();

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                SetAuth(httpClient, username, password);
            }

            return httpClient;
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
