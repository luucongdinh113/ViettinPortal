using Mita.Business.BusinessEnum;
using Mita.Business.Helpers;
using System;

namespace Mita.Business.BusinessObjects
{
    public class BusinessException : Exception
    {
        public AppErrorCode ErrorCode { get; set; }

        public object[] ErrorParams { get; set; }

        public BusinessException(AppErrorCode code, params object[] inputParams)
            : base(CommonUtils.GetErrorMessage(code, inputParams))
        {
            this.ErrorCode = code;
            this.ErrorParams = inputParams;
        }

        public BusinessException(AppErrorCode code, Exception innerException)
            : base(CommonUtils.GetErrorMessage(code), innerException)
        {
            this.ErrorCode = code;
        }
    }
}
