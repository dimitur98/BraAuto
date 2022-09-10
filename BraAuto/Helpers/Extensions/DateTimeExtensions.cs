namespace BraAuto.Helpers.Extensions
{
    public static class DateTimeExtensions
    {
        private static IConfiguration _config;
        private static IConfiguration Config => _config ??= new HttpContextAccessor().HttpContext.RequestServices.GetService<IConfiguration>();

        public static string ToWebDateFormat(this DateTime dt,bool showDay = true, bool showTime = false)
        {
            return ToWebDateFormat(dt, showDay, showTime, String.Empty);
        }

        public static string ToWebDateFormat(this DateTime dt,bool showDay, bool showTime, string defaultText)
        {
            if (dt > DateTime.MinValue)
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                string format = showDay ? config.GetValue<string>("DateTime:WebDateFormat") : config.GetValue<string>("DateTime:WebMonthYearFormat");
                if (showTime) { format += " " + config.GetValue<string>("DateTime:WebTimeFormat"); }

                return dt.ToString(format);
            }

            return defaultText;
        }
    }
}
