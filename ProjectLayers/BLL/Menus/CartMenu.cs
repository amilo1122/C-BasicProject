using Shared.Models;
using System.Text.Json;

namespace BLL.Menus
{
    public class CartMenu
    {
        List<Menu> cartMenu = new List<Menu> {
            new Menu { Name = "Оформить заказ", Callback = "checkout" },
            new Menu { Name = "Изменить корзину", Callback = "changeCart" },
            new Menu { Name = "Вернуться в каталог", Callback = "browseCatalog" },
        };

        public string GetCartMenu()
        {
            return JsonSerializer.Serialize(cartMenu);
        }
    }
}