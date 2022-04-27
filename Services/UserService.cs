using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using back.Entity;
using back.Models;
using Microsoft.AspNetCore.Identity;
using BC = BCrypt.Net.BCrypt;
using Microsoft.AspNetCore.Mvc;

namespace back.Services
{
    public interface IUserService
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
        User GetById(string id);
        Task<AuthenticateResponse> Registation(AuthenticateRequest model);
    }

    public class UserService : IUserService
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications


        private readonly AppSettings _appSettings;
        private readonly SignInManager<User> _signInManager;

        public UserService(IOptions<AppSettings> appSettings, SignInManager<User> signInManager)
        {
            _appSettings = appSettings.Value;
            _signInManager = signInManager;
        }
        public async Task<AuthenticateResponse> Registation(AuthenticateRequest model)
        {
            var user = new User() { UserName = model.Username };
          var resut = await _signInManager.UserManager.CreateAsync(user, model.Password);

            if (resut.Succeeded)
            { 
                var token = generateJwtToken(user);
                return new AuthenticateResponse(user, token); 
            }
            throw new Exception();

        }
        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {

            var user = _signInManager.UserManager.Users.FirstOrDefault(x=>x.UserName==model.Username);
            // return null if user not found
            if (user == null) return null;
            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password,false,false);
            if (!result.Succeeded)
                return null;
            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        //public IEnumerable<User> GetAll()
        //{
        //    return _users;
        //}

        public User GetById(string id)
        {
            return _signInManager.UserManager.Users.FirstOrDefault(x => x.Id == id);
        }

        // helper methods

        private string generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}