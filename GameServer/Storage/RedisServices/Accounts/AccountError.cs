using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.RedisServices.Accounts
{
    public class AccountError : Exception
    {
        public AccountError(AccountErrorCode errorCode, string errorMessage)
        {
            this.ErrorCode = (int)errorCode;
            this.ErrorMessage = errorMessage;
        }

        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}

public enum AccountErrorCode
{
    ACCOUNT_ALREADY_EXISTS = 1,
    INVALID_USERNAME = 2,
    INVALID_PASSWORD = 3
}
