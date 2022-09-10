using System.Collections.Specialized;

namespace BraAuto.Helpers.Extensions
{
    public static class IQueryCollectionExtensions
    {
        public static NameValueCollection ToNameValueCollection(this IQueryCollection query)
        {
            var col = new NameValueCollection();

            foreach (var key in query.Keys)
            {
                col.Add(key, query[key]);
            }

            return col;
        }
    }
}
