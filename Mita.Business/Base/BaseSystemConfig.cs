using log4net;
using Mita.Business.BusinessServices;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Threading;

namespace Mita.Business.Base
{
    public class BaseSystemConfig<T> where T : BaseSystemConfig<T>, new()
    {
        private const string DateTimeConfigFormat = "yyyy/MM/dd HH:mm:ss.fff";

        /// <summary>
        /// 	Logger
        /// </summary>
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected static T _instance = null;

        private Dictionary<string, string> _mapSystemConfigValues = new Dictionary<string, string>();

        private static AutoResetEvent _lockExclusiveAccess = new AutoResetEvent(true); // initial unlock
        private static AutoResetEvent _lockExclusiveReloadConfig = new AutoResetEvent(true); // initial unlock

        public static T GetSystemConfig()
        {
            try
            {
                _lockExclusiveAccess.WaitOne();

                if (_instance == null)
                {
                    _instance = new T();
                    _instance.ReloadConfig();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                throw;
            }
            finally
            {
                _lockExclusiveAccess.Set();
            }
            return _instance;
        }

        public void ReloadConfig()
        {
            try
            {
                _lockExclusiveReloadConfig.WaitOne();
                _mapSystemConfigValues.Clear();

                var mapConfig = SystemConfigService.GetInstance().GetSystemConfig();
                foreach (string key in mapConfig.Keys)
                {
                    _mapSystemConfigValues.Add(key, mapConfig[key]);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                throw;
            }
            finally
            {
                _lockExclusiveReloadConfig.Set();
            }
        }

        private void SaveConfigValueRaw(string key, string rawValue)
        {
            SystemConfigService.GetInstance().SaveSystemConfigValue(key, rawValue);

            if (!_mapSystemConfigValues.ContainsKey(key))
            {
                _mapSystemConfigValues.Add(key, rawValue);
            }
            else
            {
                _mapSystemConfigValues[key] = rawValue;
            }
        }

        protected void SaveConfigValue(string key, string value)
        {
            try
            {
                _lockExclusiveReloadConfig.WaitOne();
                SaveConfigValueRaw(key, value);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                throw;
            }
            finally
            {
                _lockExclusiveReloadConfig.Set();
            }
        }

        protected void SaveConfigValue(string key, int value)
        {
            string valueRaw = value.ToString();
            SaveConfigValueRaw(key, valueRaw);
        }

        protected void SaveConfigValue(string key, bool value)
        {
            string valueRaw = value.ToString();
            SaveConfigValueRaw(key, valueRaw);
        }

        protected void SaveConfigValue(string key, DateTime value)
        {
            string valueRaw = value.ToString(DateTimeConfigFormat);
            SaveConfigValueRaw(key, valueRaw);
        }

        protected int GetConfigValue(string key, int defaultValue)
        {
            string defaultValueRaw = defaultValue.ToString();
            int result = defaultValue;
            string resultRaw = GetConfigValue(key, defaultValueRaw);

            if (!int.TryParse(resultRaw, out result))
            {
                try
                {
                    _lockExclusiveReloadConfig.WaitOne();
                    result = defaultValue;
                    SaveConfigValue(key, defaultValue);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message, ex);
                    throw;
                }
                finally
                {
                    _lockExclusiveReloadConfig.Set();
                }
            }

            return result;
        }

        protected bool GetConfigValue(string key, bool defaultValue)
        {
            string defaultValueRaw = defaultValue.ToString();
            bool result = defaultValue;
            string resultRaw = GetConfigValue(key, defaultValueRaw);

            if (!bool.TryParse(resultRaw, out result))
            {
                try
                {
                    _lockExclusiveReloadConfig.WaitOne();
                    result = defaultValue;
                    SaveConfigValue(key, defaultValue);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message, ex);
                    throw;
                }
                finally
                {
                    _lockExclusiveReloadConfig.Set();
                }
            }

            return result;
        }

        protected DateTime GetConfigValue(string key, DateTime defaultValue)
        {
            string defaultValueRaw = defaultValue.ToString(DateTimeConfigFormat);
            DateTime result = defaultValue;
            string resultRaw = GetConfigValue(key, defaultValueRaw);

            if (!DateTime.TryParseExact(resultRaw, DateTimeConfigFormat,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            {
                try
                {
                    _lockExclusiveReloadConfig.WaitOne();
                    result = defaultValue;
                    SaveConfigValueRaw(key, defaultValueRaw);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message, ex);
                    throw;
                }
                finally
                {
                    _lockExclusiveReloadConfig.Set();
                }
            }

            return result;
        }

        protected string GetConfigValue(string key, string defaultValue)
        {
            bool updateDefaultValue = false;
            string valueRaw = null;
            string result = defaultValue;

            try
            {
                _lockExclusiveReloadConfig.WaitOne();
                if (!_mapSystemConfigValues.TryGetValue(key, out valueRaw))
                {
                    updateDefaultValue = true;
                }
                else if (valueRaw == null)
                {
                    updateDefaultValue = true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                throw;
            }
            finally
            {
                _lockExclusiveReloadConfig.Set();
            }

            if (updateDefaultValue)
            {
                try
                {
                    _lockExclusiveReloadConfig.WaitOne();

                    result = defaultValue;
                    valueRaw = defaultValue.ToString();
                    SaveConfigValueRaw(key, valueRaw);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message, ex);
                    throw;
                }
                finally
                {
                    _lockExclusiveReloadConfig.Set();
                }
            }
            else
            {
                result = valueRaw;
            }

            return result;
        }
    }
}
