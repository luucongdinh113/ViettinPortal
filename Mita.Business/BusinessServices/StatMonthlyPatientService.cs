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
    public class StatMonthlyPatientService : BaseService<StatMonthlyPatientService>
    {
        public void SaveStatMonthlyPatient(MitaContext mitaContext, StatMonthlyPatient statMonthlyPatient)
        {
            if (statMonthlyPatient == null)
            {
                throw new ArgumentNullException("statMonthlyPatient");
            }

            var existsNode = mitaContext.StatMonthlyPatients
                .FirstOrDefault(u => u.SiteSendInfo.Equals(statMonthlyPatient.SiteSendInfo, StringComparison.OrdinalIgnoreCase)
                && u.StatMonth == statMonthlyPatient.StatMonth);

            if (existsNode != null)
            {
                existsNode.CountPatient = statMonthlyPatient.CountPatient;
            }
            else
            {
                mitaContext.StatMonthlyPatients.Add(statMonthlyPatient);
            }
            mitaContext.SaveChanges();
        }
    }
}
