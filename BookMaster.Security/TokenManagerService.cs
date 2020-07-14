using BookMaster.Security.Framework;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BookMaster.Security
{
    public class TokenManagerService
    {
        private readonly AwsParameterStore _awsParameterStore;
        private readonly IConfiguration _configuration;

        public TokenManagerService(AwsParameterStore awsParameterStore, IConfiguration configuration)
        {
            _awsParameterStore = awsParameterStore;
            _configuration = configuration;
        }

        public async Task<string> GenerateToken(string username, string password)
        {            
            string realPassword = await _awsParameterStore.GetParameterValueAsync(_configuration["passwordPath"], false);
            if(realPassword == password)
            {
                string tokenKey = await _awsParameterStore.GetParameterValueAsync(_configuration["tokenPath"], false);
                byte[] key = Convert.FromBase64String(tokenKey);
                SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
                SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] {
                      new Claim(ClaimTypes.Name, username)}),
                    Expires = DateTime.UtcNow.AddMinutes(30),
                    SigningCredentials = new SigningCredentials(securityKey,
                    SecurityAlgorithms.HmacSha256Signature)
                };

                JwtSecurityTokenHandler securityTokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken token = securityTokenHandler.CreateJwtSecurityToken(descriptor);
                return securityTokenHandler.WriteToken(token);
            }
            throw new Exception("Invalid Password");
        }

        public async Task<ClaimsPrincipal> GetPrincipal(string token)
        {
            try
            {
                string tokenKey = await _awsParameterStore.GetParameterValueAsync(_configuration["tokenPath"], false);
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                if (jwtToken == null) return null;
                byte[] key = Convert.FromBase64String(tokenKey);
                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
                SecurityToken securityToken;
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, parameters, out securityToken);
                return principal;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> ValidateToken(string token)
        {
            ClaimsPrincipal principal = await GetPrincipal(token);
            if (principal == null) return false;
            ClaimsIdentity identity;
            try
            {
                identity = (ClaimsIdentity)principal.Identity;
            }
            catch (NullReferenceException)
            {
                return false;
            }
            identity.FindFirst(ClaimTypes.Name);
            return true;
        }
    }
}
