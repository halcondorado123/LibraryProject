//using Microsoft.AspNetCore.Mvc;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;

//namespace LibraryProject.Controllers.Library
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class AuthController : Controller
//    {
//        private readonly IConfiguration? _configuration;

//        private readonly ILogger<LoginController> _logger;

//        public AuthController(IConfiguration? configuration, ILogger<LoginController> logger)
//        {
//            _configuration = configuration;
//            _logger = logger;
//        }

//        [HttpPost]
//        public IActionResult Login([FromBody] LoginUser userLogin)
//        {
//            _logger.LogDebug("Metodo Login iniciado");

//            IActionResult response = Unauthorized();

//            var user = Autenticar(userLogin);

//            if (user != null)
//            {
//                // Crear token
//                var token = GenerarToken(user);

//                response = Ok(new { Token = token });
//            }
//            return response;
//        }

//        private UserModel? Autenticar(LoginUser? userLogin)
//        {
//            _logger.LogDebug("Metodo Autenticar iniciado");

//            var usuarioActual = UserConstants.Usuarios.FirstOrDefault(u => u.UserName?.ToLower() == userLogin?.UserName?.ToLower() && u.Password == userLogin?.Password);

//            if (usuarioActual != null)
//            {
//                _logger.LogInformation("La autenticación fue exitosa");
//                return usuarioActual;
//            }
//            _logger.LogWarning("La autenticación fue nula");
//            return null;
//        }

//        private string? GenerarToken(UserModel usuario)
//        {
//            _logger.LogDebug("Metodo GenerarToken iniciado");

//            DateTime time = DateTime.Now;

//            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
//            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

//            var claims = new List<Claim>();

//            claims.Add(new Claim(ClaimTypes.NameIdentifier, usuario?.UserName ?? ""));
//            claims.Add(new Claim(ClaimTypes.Email, usuario?.EmailAdress ?? ""));
//            claims.Add(new Claim(ClaimTypes.GivenName, usuario?.Name ?? ""));
//            claims.Add(new Claim(ClaimTypes.Surname, usuario?.LastName ?? ""));
//            claims.Add(new Claim(ClaimTypes.Role, usuario?.Rol ?? ""));

//            var token = new JwtSecurityToken(
//                _configuration?["Jwt:Issuer"],
//                _configuration?["Jwt:Audience"],
//                claims,
//                expires: time.AddHours(6).AddMinutes(20),
//                signingCredentials: credentials);

//            _logger.LogInformation("Se ha generado el token exitosamente");
//            return new JwtSecurityTokenHandler().WriteToken(token);
//        }
//    }
//}
