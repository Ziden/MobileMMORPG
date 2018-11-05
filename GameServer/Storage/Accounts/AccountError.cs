using System;

namespace Storage.Login
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
    INVALID_PASSWORD = 3,
    USER_OR_PASSWORD_INVALID = 4
}
