//using LibraryProject.Application.Interface.Identity;
//using LibraryProject.Domain.Entities.UserAuthentication;
//using LibraryProject.Infraestructure.Interface.Identity;
//using Microsoft.AspNetCore.Identity;

//namespace LibraryProject.Application.Services.Identity
//{
//    public class AccountApplication : IAccountApplication
//    {
//        private readonly IAccountRepository _accountRepository;

//        public AccountApplication(IAccountRepository accountRepository)
//        {
//            _accountRepository = accountRepository;
//        }

//        public async Task<SignInResult> LoginAsync(Login login)
//        {
//            // Recuperamos el usuario por correo
//            var usuario = await _accountRepository.GetUserByEmailAsync(login.Email);
//            if (usuario != null)
//            {
//                return await _accountRepository.SignInAsync(usuario, login.Password);
//            }
//            return SignInResult.Failed;
//        }

//        public async Task LogoutAsync()
//        {
//            await _accountRepository.SignOutAsync();
//        }
//    }
//}
