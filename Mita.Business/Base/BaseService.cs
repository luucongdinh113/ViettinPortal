using log4net;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Mita.Business.Base
{
    public abstract class BaseService<T> : BaseSingleton<T> where T : BaseSingleton<T>, new()
    {
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected string FormatListData(List<int> listValues)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var value in listValues)
            {
                stringBuilder.AppendFormat(", {0}", value);
            }

            if (stringBuilder.Length > 0)
            {
                stringBuilder.Remove(0, 2);
            }

            return stringBuilder.ToString();
        }

        protected string FormatListData(List<string> listValues)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var value in listValues)
            {
                stringBuilder.AppendFormat(", '{0}'", value);
            }

            if (stringBuilder.Length > 0)
            {
                stringBuilder.Remove(0, 2);
            }

            return stringBuilder.ToString();
        }
    }
}
