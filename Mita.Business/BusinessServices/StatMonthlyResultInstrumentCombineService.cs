using Mita.Business.Base;
using Mita.Business.Model;
using System;
using System.Linq;

namespace Mita.Business.BusinessServices
{
    public class StatMonthlyResultInstrumentCombineService : BaseService<StatMonthlyResultInstrumentCombineService>
    {
        public void SaveStatMonthlyResultInstrumentCombine(MitaContext mitaContext, StatMonthlyResultInstrumentCombine stat)
        {
            if (stat == null)
            {
                throw new ArgumentNullException("stat");
            }

            var existsNode = mitaContext.StatMonthlyResultInstrumentCombines
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
                mitaContext.StatMonthlyResultInstrumentCombines.Add(stat);
            }
            mitaContext.SaveChanges();
        }
    }
}
