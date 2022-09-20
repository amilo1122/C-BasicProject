using Shared.Models;
using System.Text.Json;

namespace BLL.Menus
{
    public class UserRolesMenu
    {
        List<Menu> userRolesMenu = new List<Menu> {
            new Menu { Name = "Administrator", Callback = "Administrator" },
            new Menu { Name = "Manager", Callback = "Manager" },
            new Menu { Name = "Customer", Callback = "Customer" },
        };

        public string GetUserRolesMenu()
        {
            return JsonSerializer.Serialize(userRolesMenu);
        }
    }
}