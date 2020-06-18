using LearnWebApi.Infrastructure;
using LearnWebApi.Models;
using Serilog;
using System.Linq;
using System.Threading.Tasks;

namespace LearnWebApi.Repository
{
    public interface IStrongPasswordRepository
    {
        Task StoreUserData(string username, string password);
        Task<UserData> GetUserData(string username);
        Task UpdateUserData(string username, string password);
    }

    public class StrongPasswordRepository : IStrongPasswordRepository
    {
        private readonly ILocalCache _localCache;
        private readonly UserDataContext _userDataContext;

        public StrongPasswordRepository(ILocalCache localCache, UserDataContext userDataContext)
        {
            _localCache = localCache;            
            _userDataContext = userDataContext;
        }

        public async Task<UserData> GetUserData(string username)
        {
            UserData userdata = _localCache.Get(username);
            if(userdata != null)
            {
                Log.Information($"Data found in cache for username:{userdata.Id} and password:{userdata.Password}");
                return userdata;
            }

            userdata = (from u in _userDataContext.Userlist where u.Username == username select u).ToList().First();
            if (userdata != null)
            {
                Log.Information($"Data found in DB for username:{userdata.Id} and password:{userdata.Password}");
                _localCache.Set(username, userdata);
            }
            return userdata;
        }

        public async Task UpdateUserData(string username, string password)
        {
            /*
             * Out of two below approaches of Linq, which one is best to follow.
             */
            //var userdata = _userDataContext.Userlist.Where(u => u.Username == username).ToList().First();
            var userdata = (from u in _userDataContext.Userlist where u.Username == username select u).ToList().First();
            userdata.Password = password;
            await _userDataContext.SaveChangesAsync();
        }

        public async Task StoreUserData(string username, string password)
        {
            await _userDataContext.Userlist.AddAsync(new UserData { Username = username, Password = password});
            await _userDataContext.SaveChangesAsync();
        }

        public async Task DeleteUserData(string username)
        {
            _userDataContext.Userlist.Remove(new UserData { Username = username });
            await _userDataContext.SaveChangesAsync();
        }
    }
}
