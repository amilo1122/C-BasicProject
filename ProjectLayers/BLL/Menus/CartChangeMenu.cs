using Shared.Models;
using System.Text.Json;

namespace BLL.Menus
{
    public class CartChangeMenu
    {
        List<Menu> cartChangeMenu = new List<Menu> {
            new Menu { Name = "Изменить количество", Callback = "changeQuantity" },
            new Menu { Name = "Удалить товар", Callback = "deleteGoodFromCart" },
        };

        public string GetCartChangeMenu()
        {
            return JsonSerializer.Serialize(cartChangeMenu);
        }
    }
}
