using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Data;
using System.Security.Cryptography;
using System.Text;
using API.Entities;
using API.DTOs;
using Microsoft.EntityFrameworkCore;
using API.Interfaces;

namespace API.Controllers
{
    public class AccountsController : BaseAPIController
    {
        private readonly DataContext _dbContext;
        private readonly ITokenService _tokenService;

        public AccountsController(DataContext dbContext, ITokenService tokenService)
        {
            _dbContext = dbContext;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTOs>> Register(RegisterDTOs register)
        {
            if (await isUsernameTaken(register.Username))
            {
                return BadRequest("Username is already taken!");
            }

            using var hmac = new HMACSHA512();
            var user = new AppUser
            {
                UserName = register.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(register.Password)),
                PasswordSalt = hmac.Key,
            };
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return new UserDTOs
            {
                Username = user.UserName,
                Token = _tokenService.CreatedToken(user),
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTOs>> Login(LoginDTOs login)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.UserName == login.Username);

            if (user == null) return Unauthorized("Username is Invalid");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(login.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Password is Invalid");
            }

            return new UserDTOs
            {
                Username = user.UserName,
                Token = _tokenService.CreatedToken(user),
            };
        }

        private async Task<bool> isUsernameTaken(string username)
        {
            return await _dbContext.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}