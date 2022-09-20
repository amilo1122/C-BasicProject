using Shared.Models;
using System.Text.Json;

namespace BLL.Menus
{
    public class GoodsItemMenu
    {
        List<Menu> goodsItemMenu = new List<Menu> {
            new Menu { Name = "Добавить в корзину", Callback = "addToCart" },
            new Menu { Name = "К списку товаров", Callback = "categories" },
            new Menu { Name = "Перейти в корзину", Callback = "browseCart" },
            new Menu { Name = "Главное меню", Callback = "mainMenu" }
        };

        public string GetGoodsItemMenu()
        {
            return JsonSerializer.Serialize(goodsItemMenu);
        }
    }    
}
