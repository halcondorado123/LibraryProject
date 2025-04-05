//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;
//using LibraryProject.Models; // Asegúrate de incluir el espacio de nombres donde está tu modelo de usuario

//namespace LibraryProject.Controllers.Library
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class AuthController : ControllerBase
//    {
//        private readonly UserManager<AppUsuario> _userManager;
//        private readonly SignInManager<AppUsuario> _signInManager;
//        private readonly IConfiguration _configuration;

//        public AuthController(UserManager<AppUsuario> userManager, SignInManager<AppUsuario> signInManager, IConfiguration configuration)
//        {
//            _userManager = userManager;
//            _signInManager = signInManager;
//            _configuration = configuration;
//        }

//        [HttpPost("login")]
//        public async Task<IActionResult> Login([FromBody] LoginUser model)
//        {
//            var user = await _userManager.FindByNameAsync(model.UserName);
//            if (user == null)
//                return Unauthorized("Usuario no encontrado.");

//            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
//            if (result.Succeeded)
//            {
//                var token = GenerateToken(user);
//                return Ok(new { Token = token });
//            }

//            return Unauthorized("Credenciales inválidas.");
//        }

//        private string GenerateToken(AppUsuario user)
//        {
//            var claims = new List<Claim>
//            {
//                new Claim(ClaimTypes.NameIdentifier, user.Id),
//                new Claim(ClaimTypes.Name, user.UserName),
//                new Claim(ClaimTypes.Email, user.Email)
//            };

//            var roles = _userManager.GetRolesAsync(user).Result;
//            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

//            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
//            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
//            var token = new JwtSecurityToken(
//                issuer: _configuration["Jwt:Issuer"],
//                audience: _configuration["Jwt:Audience"],
//                claims: claims,
//                expires: DateTime.Now.AddHours(1),
//                signingCredentials: creds
//            );

//            return new JwtSecurityTokenHandler().WriteToken(token);
//        }
//    }
//}
