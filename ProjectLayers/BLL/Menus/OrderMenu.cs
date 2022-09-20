using Shared.Models;
using System.Text.Json;

namespace BLL.Menus
{
    public class OrderMenu
    {
        List<Menu> orderMenu = new List<Menu> {
            new Menu { Name = "Вернуться в каталог", Callback = "browseCatalog" },
            new Menu { Name = "Главное меню", Callback = "mainMenu" },
        };

        public string GetOrderMenu()
        {
            return JsonSerializer.Serialize(orderMenu);
        }
    }
}
