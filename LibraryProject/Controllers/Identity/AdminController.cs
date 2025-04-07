using AutoMapper;
using LibraryProject.Application.DTO.Identity;
using LibraryProject.Application.DTO.Identity.AdminDTO;
using LibraryProject.Domain.Entities.UserAttributes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LibraryProject.Controllers.Identity
{
    public class AdminController : Controller
    {
        private readonly IMapper _mapper;
        private UserManager<AppUsuario> _userManager;
        private IPasswordHasher<AppUsuario> _passwordHasher;
        private IPasswordValidator<AppUsuario> _passwordValidator;
        private IUserValidator<AppUsuario> _userValidator;

        public AdminController(UserManager<AppUsuario> userManager, IPasswordHasher<AppUsuario> passwordHash, IPasswordValidator<AppUsuario> passwordValidator, IUserValidator<AppUsuario> userValidator, IMapper mapper)
        {
            _userManager = userManager;
            _passwordHasher = passwordHash;
            _passwordValidator = passwordValidator;
            _userValidator = userValidator;
            _mapper = mapper;
        }

        public IActionResult Index(int page = 1, int pageSize = 5)
        {
            var totalUsers = _userManager.Users.Count(); // Total de usuarios
            var users = _userManager.Users
                .OrderBy(u => u.UserName) // Ordenar por nombre
                .Skip((page - 1) * pageSize) // Saltar los registros de páginas anteriores
                .Take(pageSize) // Tomar solo los registros necesarios
                .ToList();

            var totalPages = (int)Math.Ceiling((double)totalUsers / pageSize); // Total de páginas

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(users);
        }

        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserDTO userDto)
        {
            if (ModelState.IsValid)
            {
                var appUser = _mapper.Map<AppUsuario>(userDto);

                IdentityResult result = await _userManager.CreateAsync(appUser, userDto.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }

            return View(userDto);
        }

        public async Task<IActionResult> Update(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                var userDto = _mapper.Map<UpdateUserDTO>(user);
                return View(userDto);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateUserDTO userDTO)
        {
            var user = await _userManager.FindByIdAsync(userDTO.Id);

            if (user == null)
                return RedirectToAction("Index");

            // Validación de Email
            if (string.IsNullOrWhiteSpace(userDTO.Email))
            {
                ModelState.AddModelError("", "El Email no puede estar vacío");
                return View(userDTO);
            }

            user.Email = userDTO.Email;

            var validEmail = await _userValidator.ValidateAsync(_userManager, user);
            if (!validEmail.Succeeded)
            {
                Errors(validEmail);
                return View(userDTO);
            }

            // Validación de Password (opcional)
            if (!string.IsNullOrWhiteSpace(userDTO.Password))
            {
                var validPass = await _passwordValidator.ValidateAsync(_userManager, user, userDTO.Password);
                if (!validPass.Succeeded)
                {
                    Errors(validPass);
                    return View(userDTO);
                }

                user.PasswordHash = _passwordHasher.HashPassword(user, userDTO.Password);
            }

            // AutoMapper para otras propiedades
            _mapper.Map(userDTO, user);  // Mapea Nombre → UserName, Edad, Salario, PaisId, etc.

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
                return RedirectToAction("Index");

            Errors(result); // Agrega errores al ModelState
            return View(userDTO);
        }
        
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                ModelState.AddModelError("", "Usuario no encontrado");
                return View("Index", _userManager.Users);
            }

            // Intentar eliminar
            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            // Si falla, mostrar errores
            Errors(result);
            return View("Index", _userManager.Users);
        }

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }
    }
}