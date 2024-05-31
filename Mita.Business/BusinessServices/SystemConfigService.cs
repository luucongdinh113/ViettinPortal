using Mita.Business.Base;
using Mita.Business.Model;
using System.Collections.Generic;
using System.Linq;

namespace Mita.Business.BusinessServices
{
    public class SystemConfigService : BaseService<SystemConfigService>
    {

        public void SaveSystemConfigValue(string key, string value)
        {
            if (GetSystemConfigValue(key) == null)
            {
                InsertSystemConfigValue(key, value);
            }
            else
            {
                UpdateSystemConfigValue(key, value);
            }
        }

        private void UpdateSystemConfigValue(string key, string value)
        {
            using (var context = MitaContext.GetContextInstance())
            {
                var dbItem = context.SystemConfigs
                .FirstOrDefault(c => c.ConfigID.ToLower().Equals(
                    key));

                if (dbItem != null)
                {
                    dbItem.Value = value;
                    context.SaveChanges();
                }
            }
        }

        private void InsertSystemConfigValue(string key, string value)
        {
            using (var context = MitaContext.GetContextInstance())
            {
                var dbItem = new SystemConfig()
                {
                    ConfigID = key,
                    Value = value
                };
                context.SystemConfigs.Add(dbItem);
                context.SaveChanges();
            }
        }

        public string GetSystemConfigValue(string key)
        {
            using (var context = MitaContext.GetContextInstance())
            {
                var dbItem = context.SystemConfigs
                .FirstOrDefault(c => c.ConfigID.ToLower().Equals(
                    key));

                if (dbItem != null)
                {
                    return dbItem.Value;
                }
            }
            return null;
        }

        public Dictionary<string, string> GetSystemConfig()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            using (var context = MitaContext.GetContextInstance())
            {

                var listdbItem = context.SystemConfigs.ToList();
                foreach (var item in listdbItem)
                {
                    result.Add(item.ConfigID, item.Value);
                }
            }
            return result;
        }
    }
}
