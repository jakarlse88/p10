using System.IO;
using Newtonsoft.Json;

namespace Abarnathy.AssessmentService.Utilities
{
    internal static class JsonUtilities
    {
        // Source: https://johnthiriet.com/efficient-api-calls/
        internal static T DeserializeJsonFromStream<T>(Stream stream)
        {
            if (stream == null || stream.CanRead == false)
                return default(T);

            using (var sr = new StreamReader(stream))
            using (var jtr = new JsonTextReader(sr))
            {
                var js = new JsonSerializer();
                var searchResult = js.Deserialize<T>(jtr);
                return searchResult;
            }
        }
    }
}