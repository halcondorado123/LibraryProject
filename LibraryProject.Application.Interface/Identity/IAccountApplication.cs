using LibraryProject.Domain.Entities.UserAuthentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Application.Interface.Identity
{
    public interface IAccountApplication
    {
        Task<SignInResult> LoginAsync(Login login);
        Task LogoutAsync();
    }
}
