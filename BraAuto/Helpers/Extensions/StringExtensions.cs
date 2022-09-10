using System.Web;

namespace BraAuto.Helpers.Extensions
{
    public static class StringExtensions
    {
        public static string UrlEncode(this string s)
        {
            return HttpUtility.UrlEncode(s);
        }
    }
}
