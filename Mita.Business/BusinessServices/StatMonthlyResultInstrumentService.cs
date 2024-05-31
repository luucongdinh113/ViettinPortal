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
    public class StatMonthlyResultInstrumentService : BaseService<StatMonthlyResultInstrumentService>
    {
        public void SaveStatMonthlyResultInstrument(MitaContext mitaContext, StatMonthlyResultInstrument stat)
        {
            if (stat == null)
            {
                throw new ArgumentNullException("stat");
            }

            var existsNode = mitaContext.StatMonthlyResultInstruments
                .FirstOrDefault(u => u.SiteSendInfo.Equals(stat.SiteSendInfo, StringComparison.OrdinalIgnoreCase)
                && u.StatMonth == stat.StatMonth
                && u.InstrumentID == stat.InstrumentID
                && u.TestCode.Equals(stat.SiteSendInfo, StringComparison.OrdinalIgnoreCase));

            if (existsNode != null)
            {
                existsNode.CountTest = stat.CountTest;
            }
            else
            {
                mitaContext.StatMonthlyResultInstruments.Add(stat);
            }
            mitaContext.SaveChanges();
        }
    }
}
