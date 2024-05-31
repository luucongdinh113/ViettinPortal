using Mita.Business.Base;
using Mita.Business.Model;
using System;

namespace Mita.Business.BusinessServices
{
    public class NodeErrorService : BaseService<NodeErrorService>
    {
        public void SaveReportError(MitaContext mitaContext, NodeError nodeError)
        {
            if (nodeError == null)
            {
                throw new ArgumentNullException("nodeError");
            }

            nodeError.ErrorTime = DateTime.Now;
            mitaContext.NodeErrors.Add(nodeError);
            mitaContext.SaveChanges();
        }
    }
}
