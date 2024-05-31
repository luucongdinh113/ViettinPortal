using EncryptionLibrary;
using Mita.Business.Base;
using Mita.Business.BusinessEnum;
using Mita.Business.BusinessObjects;
using Mita.Business.Helpers;
using Mita.Business.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mita.Business.BusinessServices
{
    public class MaintenanceBasicService : BaseService<MaintenanceBasicService>
    {
        public void SaveDailyMaintenanceBasic(MitaContext mitaContext, DailyMaintenanceBasic maintenanceBasic)
        {
            if (maintenanceBasic == null)
            {
                throw new ArgumentNullException("application");
            }

            var existsNode = mitaContext.DailyMaintenanceBasics
                .FirstOrDefault(u => u.SiteSendInfo.Equals(maintenanceBasic.SiteSendInfo, StringComparison.OrdinalIgnoreCase)
                && u.CheckDate >= maintenanceBasic.CheckDate.Date
                && u.CheckDate < maintenanceBasic.CheckDate.Date.AddDays(1));

            if (existsNode != null)
            {
                existsNode.CheckDate = maintenanceBasic.CheckDate;
                existsNode.CountPatient = maintenanceBasic.CountPatient;
                existsNode.CountResult = maintenanceBasic.CountPatient;
                existsNode.MaxDateIN = maintenanceBasic.MaxDateIN;
                existsNode.MinDateIN = maintenanceBasic.MinDateIN;
            }
            else
            {
                mitaContext.DailyMaintenanceBasics.Add(maintenanceBasic);
            }
            mitaContext.SaveChanges();
        }
    }
}
