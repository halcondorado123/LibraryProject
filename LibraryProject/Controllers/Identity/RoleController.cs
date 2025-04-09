using AutoMapper;
using LibraryProject.Application.DTO.Identity.RoleDTO;
using LibraryProject.Application.Interface.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

public class RoleController : Controller
{
    private readonly IRoleService _roleService;
    private readonly IMapper _mapper;

    public RoleController(IRoleService roleService, IMapper mapper)
    {
        _roleService = roleService;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index(int page = 1, int pageSize = 5)
    {
        var roles = await _roleService.GetAllPaginatedAsync(page, pageSize);
        var (totalPages, _) = await _roleService.GetPaginationDataAsync(pageSize);

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = totalPages;

        return View(roles);
    }

    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(CreateRoleDTO dto)
    {
        if (ModelState.IsValid)
        {
            if (await _roleService.CreateRoleAsync(dto))
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "No se pudo crear el rol.");
        }

        return View(dto);
    }

    [HttpGet]
    public async Task<IActionResult> Update(string id)
    {
        var dto = await _roleService.GetRoleWithUsersAsync(id);
        if (dto == null)
            return NotFound();

        return View(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Update(ModifyRoleDTO dto)
    {
        if (ModelState.IsValid)
        {
            await _roleService.ModifyUsersInRoleAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        var updateDto = _mapper.Map<UpdateRoleDTO>(dto);
        return View(updateDto); // Deja que el framework busque 'Views/Role/Update.cshtml'
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        if (string.IsNullOrEmpty(id))
            return Json(new { success = false, message = "ID no válido." });

        var deleted = await _roleService.DeleteRoleAsync(id);

        if (!deleted)
            return Json(new { success = false, message = "No se pudo eliminar el rol." });

        return Json(new { success = true, message = "Rol eliminado exitosamente." });
    }


    [HttpGet]
    public async Task<IActionResult> GetUsuariosConRoles([FromQuery] UserWithRoleDTO dto)
    {
        var lista = await _roleService.GetUserWithRoleDTO(dto);
        return Json(lista);
    }

    [HttpGet]
    public async Task<IActionResult> UsuariosConRoles(int page = 1, int pageSize = 10)
    {
        var roles = await _roleService.GetAllUsersWithRolesAsync(); // esto devuelve todos los roles

        int totalRoles = roles.Count();
        int totalPages = (int)Math.Ceiling((double)totalRoles / pageSize);

        var rolesPaginados = roles
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = totalPages;

        return View(rolesPaginados);
    }
}
