using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace moviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration config;
        public AccountController(UserManager<ApplicationUser> userManager, IConfiguration config) { 
            this.userManager = userManager;
            this.config = config;
        }

        // create account new user "register" " postt"
        [HttpPost("register")]

        public async Task<IActionResult> Registration(RegisterUserDto userDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            ApplicationUser user = new ApplicationUser();
            user.UserName = userDto.UserName;
            user.Email = userDto.Email;

            IdentityResult result = await userManager.CreateAsync(user , userDto.Password);

            if(!result.Succeeded)
                return BadRequest(result.Errors.FirstOrDefault());

            return Ok("Account Add Sucess");
        }

        [HttpPost("login")]

        public async Task<IActionResult> Login(LoginUserDto userDto)
        {
            if(!ModelState.IsValid)
                return Unauthorized();

            ApplicationUser user = await userManager.FindByNameAsync(userDto.UserName);

            if (user is null)
                return Unauthorized();

            bool found =  await userManager.CheckPasswordAsync(user, userDto.Password);
           
            if (!found)
                return Unauthorized();
            // claims token
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString() ));

            // get role

            var roles = await userManager.GetRolesAsync(user);
            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }
            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Secret"]) );

            SigningCredentials signingCred = new(
                securityKey,
                SecurityAlgorithms.HmacSha256
                );

            // create token
            JwtSecurityToken myToken = new JwtSecurityToken(
                issuer: config["JWT:ValidIssuer"],
                audience: config["JWT:ValidAudiance"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: signingCred
                );

            return Ok(new
            {
                token= new JwtSecurityTokenHandler().WriteToken(myToken) ,
                expiration = myToken.ValidTo
            });

        }

    }
}
