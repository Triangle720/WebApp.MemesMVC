using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApp.MemesMVC.Models;

namespace WebApp.MemesMVC.Security
{
    public static class JWTManager
    {
        public static string Secret { get; set; }
        public static string ExpireTimeInMinutes { get; set; }
        public static Task<JwtSecurityToken> AssignToken(UserModel user)
        {
            SymmetricSecurityKey symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
            SigningCredentials signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256Signature);

            List<Claim> claims = new List<Claim>();

            switch (user.Role)
            {
                case RoleTypes.USER:
                    claims.Add(new Claim(ClaimTypes.Role, Enum.GetName(typeof(RoleTypes), RoleTypes.USER)));
                    break;
                case RoleTypes.MODERATOR:
                    claims.Add(new Claim(ClaimTypes.Role, Enum.GetName(typeof(RoleTypes), RoleTypes.MODERATOR)));
                    break;
                case RoleTypes.ADMIN:
                    claims.Add(new Claim(ClaimTypes.Role, Enum.GetName(typeof(RoleTypes), RoleTypes.ADMIN)));
                    break;
            }

            var token = new JwtSecurityToken(
                issuer: "INO",
                audience: user.Login.ToString(),
                expires: DateTime.Now.AddMinutes(int.Parse(ExpireTimeInMinutes)),
                signingCredentials: signingCredentials,
                claims: claims
                );

            return Task.FromResult(token);
        }
    }
}
