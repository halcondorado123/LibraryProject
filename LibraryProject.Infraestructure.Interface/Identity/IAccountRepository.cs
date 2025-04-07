using LibraryProject.Domain.Entities.UserAttributes;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Infraestructure.Interface.Identity
{
    public interface IAccountRepository
    {
        Task<AppUsuario> GetUserByEmailAsync(string email);
        Task<SignInResult> SignInAsync(AppUsuario user, string password);
        Task SignOutAsync();
    }
}
