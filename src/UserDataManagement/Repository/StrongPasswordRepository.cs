using LearnWebApi.Infrastructure;
using LearnWebApi.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnWebApi.Repository
{
    public interface IStrongPasswordRepository
    {
        void StoreUserData(string key, string password);
        Task<UserData> GetUserData(string key);
    }

    public class StrongPasswordRepository : IStrongPasswordRepository
    {
        private readonly ILocalCache _localCache;
        private readonly UserDataContext _userDataContext;

        public StrongPasswordRepository(ILocalCache localCache)
        {
            _localCache = localCache;
            _userDataContext = new UserDataContext(new DbContextOptionsBuilder<UserDataContext>()
                .UseInMemoryDatabase(databaseName: "userlist")
                .Options);            
        }

        public async Task<UserData> GetUserData(string key)
        {
            //var userdata = _localCache.Get(key) ?? await _userDataContext.FindAsync<UserData>(key);
            UserData userdata = _localCache.Get(key);
            if(userdata != null)
            {
                Log.Information($"Data found in cache for key:{userdata.Id} and password:{userdata.Password}");
                return userdata;
            }
            
            userdata = await _userDataContext.FindAsync<UserData>(key);
            if(userdata != null)
            {
                Log.Information($"Data found in DB for key:{userdata.Id} and password:{userdata.Password}");
                _localCache.Set(key, userdata);
            }
            return userdata;
        }

        public async void StoreUserData(string id, string password)
        {
            await _userDataContext.AddAsync(new UserData { Id = id, Password = password});
            await _userDataContext.SaveChangesAsync();
        }
    }
}
