using LibraryProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace LibraryProject.TagHelpers
{
    [HtmlTargetElement("td", Attributes = "i-role")]
    public class RolUsuarioTH : TagHelper
    {
        private UserManager<AppUsuario> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public RolUsuarioTH(UserManager<AppUsuario> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HtmlAttributeName("i-role")]
        public string Role { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            List<string> nombres = new List<string>();
            IdentityRole role = await _roleManager.FindByIdAsync(Role);

            if (role != null)
            {
                foreach (var usuarios in _userManager.Users)
                {
                    if (usuarios != null && await _userManager.IsInRoleAsync(usuarios, role.Name))
                        nombres.Add(usuarios.UserName);
                }
            }

            output.Content.SetContent(nombres.Count == 0 ? "No hay usuarios" : string.Join(", ", nombres));
        }
    }
}
