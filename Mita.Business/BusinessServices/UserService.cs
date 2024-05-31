using log4net;
using Mita.Business.Base;
using Mita.Business.BusinessEnum;
using Mita.Business.BusinessObjects;
using Mita.Business.Helpers;
using Mita.Business.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Mita.Business.BusinessServices
{

    public class UserService : BaseService<UserService>
    {
        private class CreateNewUserParams
        {
            public LoginUserInfo LoginUserInfo { get; set; }
            public string Password { get; set; }
        }

        public class LoginParams
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class ChangePassParams
        {
            public string UserId { get; set; }
            public string OldPass { get; set; }
            public string NewPass { get; set; }
        }

        public class ResetPassParams
        {
            public string UserId { get; set; }
            public string NewPass { get; set; }
        }

        public class UpdateRightParams
        {
            public string UserId { get; set; }
            public List<UserRightCode> UpdateRights { get; set; }

            public UpdateRightParams()
            {
                UpdateRights = new List<UserRightCode>();
            }
        }

        public class ShortUserInfo
        {
            public string UserId { get; set; }

            public string FullName { get; set; }

            public string DisplayName
            {
                get { return string.Format("{0}: {1}", UserId, FullName); }
            }

            public override string ToString()
            {
                return DisplayName;
            }
        }

        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static RNGCryptoServiceProvider _cryptoServiceProvider = new RNGCryptoServiceProvider();

        private UserRightCode ConvertUserRightCode(UserRight userRight)
        {
            return CommonUtils.GetEnumFromValue<UserRightCode>(userRight.RightCode);
        }

        public LoginUserInfo GetSystemAdminLoginInfo()
        {
            LoginUserInfo loginUserInfo = new LoginUserInfo()
            {
                UserId = CommonConstant.SystemAdmin,
                FullName = CommonConstant.SystemAdmin,
                Status = UserStatus.Enable,
                Rights = new List<UserRightCode>()
            };

            foreach (UserRightCode right in Enum.GetValues(typeof(UserRightCode)))
            {
                loginUserInfo.Rights.Add(right);
            }

            return loginUserInfo;
        }

        private string ConvertFromHashToString(byte[] hash)
        {
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                strBuilder.Append(hash[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

        public string GeneratePasswordSystemAdmin()
        {
            string salt = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1)
                .ToString(CommonConstant.DateFormatDisplay);

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(CommonConstant.ApplicationId + salt));

            return ConvertFromHashToString(hash);
        }

        private LoginUserInfo GetLoginUserInfo(LoginParams inputParam)
        {
            if (inputParam.Username.ToLower().Equals(CommonConstant.SystemAdmin.ToLower()))
            {
                string password = GeneratePasswordSystemAdmin();
                if (inputParam.Password.Equals(password))
                {
                    return this.GetSystemAdminLoginInfo();
                }
                else
                {
                    throw new BusinessException(AppErrorCode.PasswordNotMatch);
                }
            }
            else
            {
                using (var context = MitaContext.GetContextInstance())
                {
                    var existsUser = context.UserInfoes.FirstOrDefault(u => u.UserId.ToLower()
                        .Equals(inputParam.Username.ToLower()));

                    if (existsUser == null)
                    {
                        throw new BusinessException(AppErrorCode.PasswordNotMatch);
                    }
                    else if (!existsUser.Status.ToString().Equals(UserStatus.Enable.ToString()))
                    {
                        throw new BusinessException(AppErrorCode.UserNotActive);
                    }
                    else
                    {
                        string passwordHash = GeneratePasswordHash(inputParam.Password,
                            existsUser.PasswordSalt);
                        if (!passwordHash.Equals(existsUser.Password))
                        {
                            throw new BusinessException(AppErrorCode.PasswordNotMatch);
                        }

                        return this.GetLoginUserInfo(inputParam.Username);
                    }
                }
            }
        }

        public LoginUserInfo Login(string username, string password)
        {
            return GetLoginUserInfo(new LoginParams()
            {
                Username = username,
                Password = password
            });
        }

        public string GeneratePasswordHash(string password, string salt)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(
                password + salt));

            string paswordHash = ConvertFromHashToString(hash);
            return paswordHash;
        }

        public LoginUserInfo GetLoginUserInfo(string userId)
        {
            LoginUserInfo result = null;

            if (!string.IsNullOrEmpty(userId))
            {
                if (CommonConstant.SystemAdmin.ToLower().Equals(userId.ToLower()))
                {
                    result = GetSystemAdminLoginInfo();
                }
                else
                {
                    using (var context = MitaContext.GetContextInstance())
                    {
                        var dbItem = context.UserInfoes
                            .FirstOrDefault(u => u.UserId.ToLower().Equals(userId.ToLower()));

                        if (dbItem != null)
                        {
                            var existsUser = new LoginUserInfo()
                            {
                                FullName = dbItem.FullName,
                                UserId = dbItem.UserId,
                                Status = CommonUtils.GetEnumFromValue<UserStatus>(dbItem.Status)
                            };

                            var rights = dbItem.UserRights.ToList();

                            existsUser.Rights.AddRange(dbItem.UserRights.ToList().ConvertAll(ConvertUserRightCode));

                            result = existsUser;
                        }
                    }
                }
            }

            return result;
        }

        public bool DeleteUser(string userId)
        {
            using (var context = MitaContext.GetContextInstance())
            {
                var userInfo = (from u in context.UserInfoes
                                where u.UserId.Equals(userId)
                                select u).FirstOrDefault();

                var listRightDelete = userInfo.UserRights.ToList();

                foreach (var rightDelete in listRightDelete)
                {
                    context.UserRights.Remove(rightDelete);
                }
                context.UserInfoes.Remove(userInfo);
                context.SaveChanges();
            }
            return true;
        }

        public LoginUserInfo CreateNewUser(LoginUserInfo loginUserInfo, string password)
        {
            var inputParam = new CreateNewUserParams()
            {
                LoginUserInfo = loginUserInfo,
                Password = password
            };

            if (inputParam.LoginUserInfo.UserId.Equals(CommonConstant.SystemAdmin))
            {
                throw new BusinessException(AppErrorCode.UserIdExists);
            }

            using (var context = MitaContext.GetContextInstance())
            {
                var existsUser = context.UserInfoes
                    .FirstOrDefault(u => u.UserId.ToLower()
                    .Equals(inputParam.LoginUserInfo.UserId.ToLower()));

                if (existsUser != null)
                {
                    throw new BusinessException(AppErrorCode.UserIdExists);
                }
                else
                {
                    string passwordSalt = Guid.NewGuid().ToString();
                    string paswordHash = GeneratePasswordHash(inputParam.Password, passwordSalt);

                    var dbItem = new UserInfo()
                    {
                        UserId = inputParam.LoginUserInfo.UserId,
                        FullName = inputParam.LoginUserInfo.FullName,
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                        Password = paswordHash,
                        PasswordSalt = passwordSalt,
                        Status = UserStatus.Enable.ToString()
                    };
                    context.UserInfoes.Add(dbItem);

                    foreach (var rightCode in inputParam.LoginUserInfo.Rights)
                    {
                        context.UserRights.Add(new UserRight()
                        {
                            CreateDate = DateTime.Now,
                            UpdateDate = DateTime.Now,
                            UserId = inputParam.LoginUserInfo.UserId,
                            RightCode = rightCode.ToString()
                        });
                    }

                    context.SaveChanges();

                    return this.GetLoginUserInfo(inputParam.LoginUserInfo.UserId);
                }
            }
        }

        public LoginUserInfo UpdateUser(LoginUserInfo loginUserInfo)
        {
            using (var context = MitaContext.GetContextInstance())
            {
                var serverTime = MitaContext.GetCurrentTime();

                var dbItem = context.UserInfoes
                    .FirstOrDefault(c => c.UserId.ToLower().Equals(
                        loginUserInfo.UserId));

                dbItem.FullName = loginUserInfo.FullName;
                dbItem.UpdateDate = serverTime;
                context.SaveChanges();
            }
            return this.GetLoginUserInfo(loginUserInfo.UserId);
        }

        public bool ChangePassword(ChangePassParams passParams)
        {
            using (var context = MitaContext.GetContextInstance())
            {
                var selectedUser = (from u in context.UserInfoes
                                    where u.UserId.ToLower().Equals(passParams.UserId.ToLower())
                                    select u).FirstOrDefault();

                if (selectedUser == null)
                {
                    throw new BusinessException(AppErrorCode.UserIdNotExists);
                }
                else
                {
                    string passwordHash = GeneratePasswordHash(
                        passParams.OldPass, selectedUser.PasswordSalt);
                    if (!passwordHash.Equals(selectedUser.Password))
                    {
                        throw new BusinessException(AppErrorCode.PasswordNotMatch);
                    }
                    else
                    {
                        string passwordSalt = Guid.NewGuid().ToString();
                        passwordHash = GeneratePasswordHash(passParams.NewPass, passwordSalt);

                        selectedUser.Password = passwordHash;
                        selectedUser.PasswordSalt = passwordSalt;

                        context.SaveChanges();

                        return true;
                    }
                }
            }
        }

        public bool ResetPassword(ResetPassParams passParams)
        {
            using (var context = MitaContext.GetContextInstance())
            {
                var selectedUser = (from u in context.UserInfoes
                                    where u.UserId.ToLower().Equals(passParams.UserId.ToLower())
                                    select u).FirstOrDefault();

                if (selectedUser == null)
                {
                    throw new BusinessException(AppErrorCode.UserIdNotExists);
                }
                else
                {
                    selectedUser.PasswordSalt = Guid.NewGuid().ToString();
                    selectedUser.Password = GeneratePasswordHash(
                        passParams.NewPass, selectedUser.PasswordSalt);
                    context.SaveChanges();
                }
            }
            return true;
        }

        public List<ShortUserInfo> GetAllShortUserInfo()
        {
            using (var context = MitaContext.GetContextInstance())
            {
                var userStatusEnable = UserStatus.Enable.ToString();
                var listUsers = (from userInfo in context.UserInfoes
                                 where userInfo.Status.Equals(userStatusEnable)
                                 select new ShortUserInfo()
                                 {
                                     UserId = userInfo.UserId,
                                     FullName = userInfo.FullName
                                 }).ToList();

                return listUsers;
            }
        }

        public bool UpdateUserRights(UpdateRightParams rightParams)
        {
            using (var context = MitaContext.GetContextInstance())
            {
                var listRights = (from u in context.UserRights
                                  where u.UserInfo.UserId.ToLower().Equals(rightParams.UserId.ToLower())
                                  select u);

                foreach (var right in listRights)
                {
                    context.UserRights.Remove(right);
                }
                context.SaveChanges();

                var serverTime = MitaContext.GetCurrentTime();

                foreach (var rightCode in rightParams.UpdateRights)
                {
                    context.UserRights.Add(new UserRight()
                    {
                        CreateDate = serverTime,
                        UpdateDate = serverTime,
                        UserId = rightParams.UserId,
                        RightCode = rightCode.ToString()
                    });
                }
                context.SaveChanges();
            }
            return true;
        }
    }
}
