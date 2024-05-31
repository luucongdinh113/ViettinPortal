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
    public class NodeMonitorService : BaseService<NodeMonitorService>
    {
        public void SyncNodeMonitor(MitaContext mitaContext, NodeMonitor nodeMonitor)
        {
            if (nodeMonitor == null)
            {
                throw new ArgumentNullException("application");
            }

            var existsNode = mitaContext.NodeMonitors
                .FirstOrDefault(u => u.HostName.Equals(nodeMonitor.HostName, StringComparison.OrdinalIgnoreCase) 
                && u.SiteName.Equals(nodeMonitor.SiteName, StringComparison.OrdinalIgnoreCase));

            if (existsNode == null)
            {
                mitaContext.NodeMonitors.Add(nodeMonitor);
                mitaContext.SaveChanges();
            }
            else
            {
                existsNode.CheckTime = DateTime.Now;
                mitaContext.SaveChanges();
            }
        }
    }
}
