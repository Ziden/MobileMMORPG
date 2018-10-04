using System;

namespace Storage.RedisServices.Accounts
{
    public class AccountService : RedisService
    {
        public bool RegisterAccount(string login, string password)
        {
            var account = Redis.db.StringGet($"Logins:{login}");
            if (account.HasValue)
            {
                throw new AccountError(AccountErrorCode.ACCOUNT_ALREADY_EXISTS, "Login already registered");
            }
            Redis.db.StringSet
            return true;
        }
    }
}
