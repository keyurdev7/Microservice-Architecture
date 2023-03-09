using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Test.Application.Features.Blog.Commands.Delete;
using Test.Application.Features.Blog.Commands.Upsert;
using Test.Application.Features.Blog.Queries.GetBlogById;
using Test.Application.Features.Blog.Queries.GetBlogList;
using Test.Application.Features.User.Command.UserLogin;
using Test.Application.Persistence.Blog;
using Test.Application.Persistence.User;
using Test.Domain.ComplexObject;
using Test.Domain.Model;
using Test.Infrastructure.Persistence;
using Test.Infrastructure.Repositories.Base;


namespace Test.Infrastructure.Repositories.User
{

    public class UserRespository : BaseRepository, IUserRespository
    {
        private readonly DatabaseContext _context;
        private readonly JwtTokenConfig _jwtTokenConfig;
        private readonly byte[] _secret;
        private readonly ConcurrentDictionary<string, RefreshToken> _usersRefreshTokens;  // can store in a database or a distributed cache
        public UserRespository(DatabaseContext dbContext, JwtTokenConfig jwtTokenConfig) : base(dbContext)
        {
            _context = dbContext;
            _jwtTokenConfig = jwtTokenConfig;
            _secret = Encoding.ASCII.GetBytes(jwtTokenConfig.Secret);
            _usersRefreshTokens = new ConcurrentDictionary<string, RefreshToken>();
        }

        public Task<UsersViewModel> UserLogin(UserLoginCommand query)
        {
            var user = _context.User.Where(m => m.UserName == query.username && m.Password == query.password)
                .Select(x => new UsersViewModel() { 
                    UserName = x.UserName,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    UserId =x.UserId,
                }).FirstOrDefault();

            var claims = new[]
            {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim("UserId",user.UserId.ToString())
            };
            var jwt = GenerateTokens(user.UserName, claims, DateTime.Now);
            user.token = jwt.AccessToken;

            return Task.FromResult(user);
        }

        private JwtAuthResult GenerateTokens(string username, Claim[] claims, DateTime now)
        {
            var shouldAddAudienceClaim = string.IsNullOrWhiteSpace(claims?.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Aud)?.Value);
            var jwtToken = new JwtSecurityToken(
                _jwtTokenConfig.Issuer,
                shouldAddAudienceClaim ? _jwtTokenConfig.Audience : string.Empty,
                claims,
                expires: now.AddMinutes(_jwtTokenConfig.AccessTokenExpiration),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(_secret), SecurityAlgorithms.HmacSha256Signature));
            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            var refreshToken = new RefreshToken
            {
                UserName = username,
                TokenString = GenerateRefreshTokenString(),
                ExpireAt = now.AddMinutes(_jwtTokenConfig.RefreshTokenExpiration)
            };
            _usersRefreshTokens.AddOrUpdate(refreshToken.TokenString, refreshToken, (s, t) => refreshToken);

            return new JwtAuthResult
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
        private static string GenerateRefreshTokenString()
        {
            var randomNumber = new byte[32];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
