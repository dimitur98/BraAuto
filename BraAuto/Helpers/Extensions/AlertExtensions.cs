using BraAuto.ViewModels.Helpers;
using Newtonsoft.Json;

namespace BraAuto.Helpers.Extensions
{
    public static class AlertExtensions
    {
        public static string SerializeAlert(this Alert alert)
        {
            return JsonConvert.SerializeObject(alert);
        }

        public static Alert DeserializeAlert(this string alert)
        {
            return JsonConvert.DeserializeObject<Alert>(alert);
        }
    }
}
