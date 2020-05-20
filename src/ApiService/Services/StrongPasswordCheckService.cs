using LearnWebApi.Models;
using LearnWebApi.Repository;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LearnWebApi.Services
{
    public interface IStrongPasswordCheckService
    {
        Task<bool> CheckPasswordStrengthAsync(string key, string password);
        Task<UserData> GetUserData(string key);
        Task UpdateUserData(string key, string password);
    }

    public class StrongPasswordCheckService : IStrongPasswordCheckService
    {
        private readonly IStrongPasswordRepository _strongPasswordRepository;

        public StrongPasswordCheckService(IStrongPasswordRepository strongPasswordRepository)
        {
            _strongPasswordRepository = strongPasswordRepository;
        }

        public async Task<bool> CheckPasswordStrengthAsync(string key, string password)
        {
            var regexItem = new Regex("^[a-zA-Z0-9 ]*$");

            if (password.Length < 6)
            {
                return false;
            }

            if(!password.Any(char.IsUpper) || !password.Any(char.IsLower) || !password.Any(char.IsDigit))
            {
                return false;
            }

            if (!password.Any(ch => !Char.IsLetterOrDigit(ch)))
            {
                return false;
            }

            await _strongPasswordRepository.StoreUserData(key, password);
            return true;
        }

        public async Task<UserData> GetUserData(string key)
        {
            return await _strongPasswordRepository.GetUserData(key);
        }

        public async Task UpdateUserData(string key, string password)
        {
            await _strongPasswordRepository.UpdateUserData(key, password);
        }
    }
}
