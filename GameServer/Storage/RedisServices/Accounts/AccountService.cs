using System;

namespace Storage.RedisServices.Accounts
{
    public class AccountService : RedisService
    {
        public User RegisterAccount(string login, string password, string email)
        {
            var account = Redis.db.StringGet($"login:{login}");
            if (account.HasValue)
            {
                throw new AccountError(AccountErrorCode.ACCOUNT_ALREADY_EXISTS, "Login already registered");
            }
            Redis.db.StringSet($"login:{login}", password);
            User user = new User()
            {
                login = login,
                password = password,
                email = email,
                uuid = Guid.NewGuid().ToString()
            };
            Redis.db.StringSet($"u:{user.uuid}", Serialize(user));
            return user;
        }
    }
}
