//using LibraryProject.Domain.Entities.UserAttributes;
//using LibraryProject.Infraestructure.Data.DbContext;
//using LibraryProject.Infraestructure.Interface.Identity;
//using LibraryProject.Models;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace LibraryProject.Infraestructure.Repository.Identity
//{
//    public class AdminRepository : IAdminRepository
//    {
//        private readonly UserDbContext _context;

//        public AdminRepository(UserDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<int> GetTotalUsuariosAsync()
//        {
//            return await _context.Users.CountAsync();
//        }

//        public async Task<IEnumerable<AppUsuario>> GetUsuariosPaginatedAsync(int page, int pageSize)
//        {
//            return await _context.Users
//                .OrderBy(u => u.UserName)
//                .Skip((page - 1) * pageSize)
//                .Take(pageSize)
//                .ToListAsync();
//        }
//    }
//}
