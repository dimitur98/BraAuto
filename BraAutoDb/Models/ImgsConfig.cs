using Newtonsoft.Json;

namespace BraAutoDb.Models
{
    public class ImgsConfig
    {
        public IEnumerable<string> Urls { get; set; }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        public static ImgsConfig Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<ImgsConfig>(json);
        }
    }
}
