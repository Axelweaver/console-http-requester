using Newtonsoft.Json;

namespace HttpRequester.Adapters
{
    /// <summary>
    /// Адаптер конвертера json
    /// </summary>
    public class JsonConvertAdapter
    {
        /// <summary>
        /// Десериализовать объект из json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public T Deserialize<T>(string json)
        {
            T result = JsonConvert.DeserializeObject<T>(json);

            return result;
        }

        /// <summary>
        /// Сериализовать объект в json
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string Serialize(object value)
        {
            var result = JsonConvert.SerializeObject(value);

            return result;
        }
    }
}
