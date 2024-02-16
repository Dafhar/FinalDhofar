using DhofarAppApi.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DhofarAppApi.Services
{
    public class JWTTokenServices
    {
        private IConfiguration _configuration;

        private SignInManager<User> _signInManager;

        public static List<string> RevokedTokens { get; } = new List<string>();

        public JWTTokenServices(IConfiguration configuration, SignInManager<User> signInManager)
        {
            _configuration = configuration;
            _signInManager = signInManager;
        }

        public static TokenValidationParameters GetValidationParamerts(IConfiguration configuration)
        {
            return new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = GetSecurityKey(configuration),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                LifetimeValidator = LifetimeValidator,

            };
        }

        private static SecurityKey GetSecurityKey(IConfiguration configuration)
        {
            var secret = configuration["JWT:Secret"];
            if (secret == null)
            {
                throw new InvalidOperationException("JWT:Secret Key not Found");
            }

            var secretByets = Encoding.UTF8.GetBytes(secret);
            return new SymmetricSecurityKey(secretByets);
        }

        public async Task<string> GetToken(User user, TimeSpan expireIn)
        {
            var principal = await _signInManager.CreateUserPrincipalAsync(user);

            if (principal == null)
            {
                throw new InvalidOperationException("The principla not found");
            }

            var signingKey = GetSecurityKey(_configuration);
            var token = new JwtSecurityTokenHandler().CreateToken(new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow + expireIn,
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
                Subject = principal.Identity as ClaimsIdentity,
                Claims = new Dictionary<string, object>
                {
                    {
                        JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()
                    }
                }
            });
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken token, TokenValidationParameters parameters)
        {
            var now = DateTime.UtcNow;
            return (expires != null && expires > now && notBefore <= now) && !IsTokenRevoked(token.Id);
        }
        public void RevokeToken(string jti)
        {
            RevokedTokens.Add(jti);
        }

        public static bool IsTokenRevoked(string jti)
        {
            return RevokedTokens.Contains(jti);
        }

        public JwtSecurityToken DecodeJwt(string jwtToken)
        {
            if (!string.IsNullOrEmpty(jwtToken) && jwtToken.StartsWith("Bearer "))
            {
                jwtToken = jwtToken.Substring("Bearer ".Length).Trim();
            }
            else
            {
                throw new ArgumentException("JWT token not found in headers");
            }

            var handler = new JwtSecurityTokenHandler();
            if (handler.CanReadToken(jwtToken))
            {
                return handler.ReadJwtToken(jwtToken);
            }
            else
            {
                throw new ArgumentException("Invalid JWT token");
            }
        }
    }
}
