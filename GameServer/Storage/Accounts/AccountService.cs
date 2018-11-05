using Storage.Players;
using System;

namespace Storage.Login
{
    public class AccountService
    {
        public static Player RegisterAccount(string login, string password, string email, Player playerTemplate)
        {
            var account = LoginDao.GetUserId(login);
            if (account != null)
            {
                throw new AccountError(AccountErrorCode.ACCOUNT_ALREADY_EXISTS, "Login already registered");
            }

            playerTemplate.Login = login;
            playerTemplate.Password = password;
            playerTemplate.Email = email;
            playerTemplate.UserId = Guid.NewGuid().ToString();

            LoginDao.SetUserId(login, playerTemplate.UserId);
            RedisHash<Player>.Set(playerTemplate);
            return playerTemplate;
        }

        public static Player Login(string login, string password)
        {
            if (!LoginDao.LoginExists(login))
            {
                throw new AccountError(AccountErrorCode.USER_OR_PASSWORD_INVALID, "Invalid username or password");
            }
            string userId = LoginDao.GetUserId(login);
            var u = RedisHash<Player>.Get(userId);
            if ((string)u.Password != password)
            {
                throw new AccountError(AccountErrorCode.USER_OR_PASSWORD_INVALID, "Invalid username or password");
            }
            Session session = new Session()
            {
                PlayerUid = u.UserId,
                SessionUid = Guid.NewGuid().ToString(),
                DateStarted = DateTime.Now.ToString()
            };
            u.SessionId = session.SessionUid;
            RedisHash<Session>.Set(session);
            RedisHash<Player>.Set(u);
            return u;
        }
    }
}
