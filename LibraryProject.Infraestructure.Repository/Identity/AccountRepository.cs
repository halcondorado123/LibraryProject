//using LibraryProject.Domain.Entities.UserAttributes;
//using LibraryProject.Infraestructure.Interface.Identity;
//using Microsoft.AspNetCore.Identity;
//using System.Threading.Tasks;

//namespace LibraryProject.Infraestructure.Repositories
//{
//    public class AccountRepository : IAccountRepository
//    {
//        private readonly UserManager<AppUsuario> _userManager;
//        private readonly SignInManager<AppUsuario> _signInManager;

//        public AccountRepository(UserManager<AppUsuario> userManager, SignInManager<AppUsuario> signInManager)
//        {
//            _userManager = userManager;
//            _signInManager = signInManager;
//        }

//        public async Task<AppUsuario> GetUserByEmailAsync(string email)
//        {
//            return await _userManager.FindByEmailAsync(email);
//        }

//        public async Task<SignInResult> SignInAsync(AppUsuario usuario, string password)
//        {
//            return await _signInManager.PasswordSignInAsync(usuario, password, false, false);
//        }

//        public async Task SignOutAsync()
//        {
//            await _signInManager.SignOutAsync();
//        }
//    }
//}
