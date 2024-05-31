using Mita.Business.Base;
using Mita.Business.Model;
using System;
using System.Linq;

namespace Mita.Business.BusinessServices
{
    public class StatMonthlyResultService : BaseService<StatMonthlyResultService>
    {
        public void SaveStatMonthlyResult(MitaContext mitaContext, StatMonthlyResult statMonthlyResult)
        {
            if (statMonthlyResult == null)
            {
                throw new ArgumentNullException("statMonthlyResult");
            }

            var existsNode = mitaContext.StatMonthlyResults
                .FirstOrDefault(u => u.SiteSendInfo.Equals(statMonthlyResult.SiteSendInfo, StringComparison.OrdinalIgnoreCase)
                && u.StatMonth == statMonthlyResult.StatMonth
                && u.TestCode.Equals(statMonthlyResult.SiteSendInfo, StringComparison.OrdinalIgnoreCase));

            if (existsNode != null)
            {
                existsNode.CountTest = statMonthlyResult.CountTest;
            }
            else
            {
                mitaContext.StatMonthlyResults.Add(statMonthlyResult);
            }
            mitaContext.SaveChanges();
        }
    }
}
