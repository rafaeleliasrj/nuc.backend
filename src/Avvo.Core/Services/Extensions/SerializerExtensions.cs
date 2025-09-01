using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Avvo.Core.Services.Extensions
{
    public static class SerializerExtensions
    {
        private static readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            },
            Formatting = Formatting.None,
            NullValueHandling = NullValueHandling.Include
        };

        /// <summary>
        /// Serializa um objeto para uma string JSON, usando lowerCamelCase,
        /// sem formatação e caso existam propriedadescom valor null serão
        /// incluídas.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Serialize(this object value)
        {
            if (value == null)
                return null;

            return JsonConvert.SerializeObject(value, jsonSerializerSettings);
        }

        /// <summary>
        /// Deserializa uma string contendo um JSON válido
        /// para uma instância da classe informada.
        /// </summary>
        /// <param name="json"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Deserialize<T>(this string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return default(T);

            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
