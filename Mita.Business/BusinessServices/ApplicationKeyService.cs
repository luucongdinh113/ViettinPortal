using EncryptionLibrary;
using Mita.Business.Base;
using Mita.Business.Helpers;
using Mita.Business.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mita.Business.BusinessServices
{
    public class ApplicationKeyService : BaseService<ApplicationKeyService>
    {
        public class ApplicationKeyInfo
        {
            public string ApplicationId { get; set; }
            public string ApplicationName { get; set; }
            public string DisplayText { get; set; }
            public string Description { get; set; }
        }

        public class SearchCriteria
        {
            public string ApplicationFilter { get; set; }
        }

        public ApplicationKeyInfo GetApplicationKey(string applicationId)
        {
            using (var context = MitaContext.GetContextInstance())
            {
                var selectedItem = (from x in context.ApplicationKeys
                                    where x.ApplicationId == applicationId
                                    select new ApplicationKeyInfo()
                                    {
                                        ApplicationId = x.ApplicationId,
                                        ApplicationName = x.ApplicationName,
                                        DisplayText = x.DisplayText,
                                        Description = x.Description
                                    }).FirstOrDefault();

                return selectedItem;
            }
        }

        public List<ApplicationKeyInfo> GetListApplicationKey(SearchCriteria searchCriteria)
        {
            using (var context = MitaContext.GetContextInstance())
            {
                var query = (from e in context.ApplicationKeys
                             select e);

                if (searchCriteria != null)
                {
                    if (!string.IsNullOrEmpty(searchCriteria.ApplicationFilter))
                    {
                        query = query.Where(x => x.DisplayText.Contains(searchCriteria.ApplicationFilter));
                    }
                }
                var listApplication = query.Select(x => new ApplicationKeyInfo()
                {
                    ApplicationId = x.ApplicationId,
                    ApplicationName = x.ApplicationName,
                    DisplayText = x.DisplayText,
                    Description = x.Description
                }).ToList();

                return listApplication;
            }
        }

        public string GetSerialUnlock(string applicationId, string serialCode)
        {
            var selectedItem = GetApplicationKey(applicationId);

            if (selectedItem != null)
            {
                return EncryptionUtils.MD5EncryptData(
                    serialCode + "-" + CommonUtils.Decrypt(selectedItem.ApplicationName) + "-" + CommonUtils.Decrypt(selectedItem.ApplicationId));
            }

            return string.Empty;
        }

        public string GetInstrumentExpireKey(string applicationId, string instrumentId, DateTime expireDate)
        {
            var mess = $"Ins:{instrumentId}|Expire:{expireDate:yyyyMMdd}";

            var selectedItem = GetApplicationKey(applicationId);
            if (selectedItem != null)
            {
                return EncryptionUtils.EncryptData(mess,
                    CommonUtils.Decrypt(selectedItem.ApplicationName),
                    CommonUtils.Decrypt(selectedItem.ApplicationId));
            }

            return string.Empty;
        }

        public void SyncNewApp(ApplicationKeyInfo application)
        {
            if (application == null)
            {
                throw new ArgumentNullException("application");
            }

            using (var context = MitaContext.GetContextInstance())
            {
                var existsApp = context.ApplicationKeys
                .FirstOrDefault(u => u.ApplicationId
                    .Equals(CommonUtils.EncryptData(application.ApplicationId)));

                if (existsApp == null)
                {
                    var dbItem = new ApplicationKey()
                    {
                        ApplicationId = CommonUtils.EncryptData(application.ApplicationId),
                        ApplicationName = CommonUtils.EncryptData(application.ApplicationName),
                        DisplayText = application.DisplayText,
                        Description = application.Description,
                    };
                    context.ApplicationKeys.Add(dbItem);
                    context.SaveChanges();
                }
            }
        }

        private int Exponent(int aNum, int SM)
        {
            int num1 = 1;
            if (SM == 0)
            {
                return 1;
            }
            int num2 = Math.Abs(SM);
            int num3 = 0;
            int num4 = checked(num2 - 1);
            int num5 = num3;
            while (num5 <= num4)
            {
                num1 *= aNum;
                checked { ++num5; }
            }

            return num1;
        }

        public string GetClientSerialUnlock(string strCrackAllow, int AppNum)
        {
            var checkSeri = string.Empty;
            int aNum = 0;
            int num1 = 0;
            int num2 = checked(strCrackAllow.Length - 1);
            int startIndex = num1;
            while (startIndex <= num2)
            {
                aNum += (int)strCrackAllow[startIndex];
                ++startIndex;
            }

            var x = (AppNum * Exponent(aNum, 3) + 2003);
            checkSeri = string.Format("{0:X2}", Convert.ToInt32(Convert.ToSingle(x)));
            return checkSeri;
        }
    }
}
