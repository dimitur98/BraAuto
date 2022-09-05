using NLog;

namespace BraAuto.Helpers.Log
{
    public class LogHelper : ILogHelper
    {
        private readonly Logger Log = LogManager.GetLogger("*");

        public void Error(Exception ex) => this.Log.Error(ex, ex.Message);

        public void Info(string message) => this.Log.Info(message);
    }
}
