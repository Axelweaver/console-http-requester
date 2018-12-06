using System.Collections.Generic;
using System.Net.Http;

namespace HttpRequester.Converters
{
    /// <summary>
    /// Словарь соответствия строка - http метод
    /// </summary>
    public class HttpMethodConverterList
    {
        /// <summary>
        /// Словарь соответствия строка - http метод
        /// </summary>
        public Dictionary<string, HttpMethod> List =>
                            new Dictionary<string, HttpMethod>(7)
                            {
                                {"post", HttpMethod.Post},
                                {"get", HttpMethod.Get},
                                {"put", HttpMethod.Put },
                                {"delete", HttpMethod.Delete },
                                {"head", HttpMethod.Head },
                                {"options", HttpMethod.Options },
                                {"trace", HttpMethod.Trace }
                            };
    }
}
