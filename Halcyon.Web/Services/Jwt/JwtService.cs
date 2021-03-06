﻿using Halcyon.Web.Data;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Halcyon.Web.Services.Jwt
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _jwtSettings;

        public JwtService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public JwtResult GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecurityKey));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.EmailAddress),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName)
            };

            foreach (var role in user.UserRoles.Select(ur => ur.Role))
            {
                claims.Add(new Claim("role", role.Name));
            }

            var token = new JwtSecurityToken(
                _jwtSettings.Issuer,
                _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddSeconds(_jwtSettings.ExpiresIn),
                signingCredentials: credentials);

            return new JwtResult
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresIn = _jwtSettings.ExpiresIn
            };
        }
    }
}
