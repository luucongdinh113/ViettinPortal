using Mita.Business.Base;
using Mita.Business.BusinessEnum;
using Mita.Business.BusinessObjects;
using Mita.Business.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mita.Business.BusinessServices
{
    public class CustomerService : BaseService<CustomerService>
    {
        public class CustomerInfo
        {
            public Guid SysId { get; set; }
            public string CustomerId { get; set; }
            public string CustomerName { get; set; }
            public string Description { get; set; }
        }

        public class SearchCriteria
        {
            public string CustomerIdFilter { get; set; }

            public string CustomerNameFilter { get; set; }

            public Guid? SysId { get; set; }
        }

        public CustomerInfo GetCustomer(Guid sysId)
        {
            return GetListCustomer(new SearchCriteria
            {
                SysId = sysId
            }).FirstOrDefault();
        }

        public CustomerInfo GetCustomer(string customerId)
        {
            return GetListCustomer(new SearchCriteria
            {
                CustomerIdFilter = customerId
            }).FirstOrDefault();
        }

        public CustomerInfo CreateNewCustomer(CustomerInfo customerInfo, string userAccess)
        {
            using (var context = MitaContext.GetContextInstance())
            {
                var existsUser = context.Customers
                    .FirstOrDefault(u => u.CustomerId.ToLower()
                    .Equals(customerInfo.CustomerId.ToLower()));

                if (existsUser != null)
                {
                    throw new BusinessException(AppErrorCode.CustomerIdExists);
                }

                existsUser = context.Customers
                    .FirstOrDefault(u => u.SysId
                    .Equals(customerInfo.SysId));

                if (existsUser != null)
                {
                    throw new BusinessException(AppErrorCode.CustomerSysIdExists);
                }

                var dbItem = new Customer()
                {

                    SysId = customerInfo.SysId,
                    CustomerId = customerInfo.CustomerId,
                    CustomerName = customerInfo.CustomerName,
                    Description = customerInfo.Description,
                    InTime = DateTime.Now,
                    UpdateTime = DateTime.Now,
                    UserI = userAccess,
                    UserU = userAccess
                };
                context.Customers.Add(dbItem);
                context.SaveChanges();

                return customerInfo;
            }
        }

        public void DeleteCustomer(Guid sysId, string userAccess)
        {
            using (var context = MitaContext.GetContextInstance())
            {
                var existsUser = context.Customers
                    .FirstOrDefault(u => u.SysId
                    .Equals(sysId));

                if (existsUser != null)
                {
                    context.Customers.Remove(existsUser);
                    context.SaveChanges();
                }
            }
        }

        public List<CustomerInfo> GetListCustomer(SearchCriteria searchCriteria)
        {
            using (var context = MitaContext.GetContextInstance())
            {
                var query = (from e in context.Customers
                             select e);

                if (searchCriteria != null)
                {
                    if (!string.IsNullOrEmpty(searchCriteria.CustomerIdFilter))
                    {
                        query = query.Where(x => x.CustomerId == searchCriteria.CustomerIdFilter);
                    }

                    if (!string.IsNullOrEmpty(searchCriteria.CustomerNameFilter))
                    {
                        query = query.Where(x => x.CustomerName.Contains(searchCriteria.CustomerNameFilter));
                    }

                    if (searchCriteria.SysId != null)
                    {
                        query = query.Where(x => x.SysId == searchCriteria.SysId.GetValueOrDefault());
                    }
                }
                query = query.OrderBy(x => x.CustomerId);

                var result = query.Select(x => new CustomerInfo()
                {
                    SysId = x.SysId,
                    CustomerId = x.CustomerId,
                    CustomerName = x.CustomerName,
                    Description = x.Description
                }).ToList();

                return result;
            }
        }
    }
}
