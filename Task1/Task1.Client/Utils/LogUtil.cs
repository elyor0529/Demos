using System.Reflection;
using log4net;

namespace Task1.Client.Utils
{
    public static class LogUtil
    {
        public static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    }
}
