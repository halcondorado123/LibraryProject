using AutoMapper;
using LibraryProject.Application.DTO.Identity.RoleDTO;
using LibraryProject.Application.Interface.Identity;
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

    public async Task<IActionResult> Update(string id)
    {
        var dto = await _roleService.GetRoleWithUsersAsync(id);
        return dto == null ? NotFound() : View(dto);
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
        return View(updateDto);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> GetUserCountInRole(string roleId)
    {
        var count = await _roleService.GetUserCountInRoleAsync(roleId);
        return Json(count);
    }
}
