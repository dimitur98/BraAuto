using System.Web;

namespace BraAuto.Helpers.Extensions
{
    public static class StringExtensions
    {
        public static string UrlEncode(this string s)
        {
            return HttpUtility.UrlEncode(s);
        }

        public static string ToSortByText(this string sortBy, bool sortDesc)
        {
            var text = string.Empty;

            switch (sortBy.Split('.').Last())
            {
                case "is_approved": 
                    text = $"Approved {(sortDesc ? "First" : "Last")}";
                    break;
                case "date":
                case "created_at":
                    text = sortDesc ? "Newest First" : "Oldest First";
                    break;
                case "price":
                    text = sortDesc ? "Price High-Low" : "Price Low-High";
                    break;
                case "star_rating":
                    text = sortDesc ? "Rating High-Low" : "Rating Low-High";
                    break;
                case "name":
                    text = sortDesc ? "Name Z-A" : "Name A-Z";
                    break;
            }

            return text;
        }
    }
}
