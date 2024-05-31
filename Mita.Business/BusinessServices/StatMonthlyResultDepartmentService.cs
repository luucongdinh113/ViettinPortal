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
    public class StatMonthlyResultDepartmentService : BaseService<StatMonthlyResultDepartmentService>
    {
        public void SaveStatMonthlyResultDepartment(MitaContext mitaContext, StatMonthlyResultDepartment stat)
        {
            if (stat == null)
            {
                throw new ArgumentNullException("stat");
            }

            var existsNode = mitaContext.StatMonthlyResultDepartments
                .FirstOrDefault(u => u.SiteSendInfo.Equals(stat.SiteSendInfo, StringComparison.OrdinalIgnoreCase)
                && u.StatMonth == stat.StatMonth
                && u.LocationID == stat.LocationID
                && u.TestCode.Equals(stat.SiteSendInfo, StringComparison.OrdinalIgnoreCase));

            if (existsNode != null)
            {
                existsNode.CountTest = stat.CountTest;
            }
            else
            {
                mitaContext.StatMonthlyResultDepartments.Add(stat);
            }
            mitaContext.SaveChanges();
        }
    }
}
