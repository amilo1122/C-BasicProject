using Shared.Models;
using System.Text.Json;

namespace BLL.Menus
{
    public class ChangeQuantityMenu
    {
        List<Menu> changeQuantityMenu = new List<Menu> {
            new Menu { Name = "+", Callback = "increaseQuantity" },
            new Menu { Name = "-", Callback = "reduceQuantity" },
            new Menu { Name = "Вернуться в корзину", Callback = "browseCart" },
        };

        public string GetChangeQuantityMenu()
        {            
            return JsonSerializer.Serialize(changeQuantityMenu);
        }
    }
}
