using Shared.Models;
using System.Text.Json;

namespace BLL.Menus
{
    public class ChangeGoodMenu
    {
        List<Menu> changeGoodMenu = new List<Menu> {
            new Menu { Name = "Изменить категорию товара", Callback = "changeGoodCategory" },
            new Menu { Name = "Изменить наименование", Callback = "changeGoodName" },
            new Menu { Name = "Изменить описание", Callback = "changeGoodDescription" },
            new Menu { Name = "Изменить стоимость", Callback = "changeGoodPrice" },
            new Menu { Name = "Изменить количество", Callback = "changeGoodQuantity" },
            new Menu { Name = "Изменить ссылку на изображение", Callback = "changeGoodUrl" }
        };

        public string GetChangeGoodMenu()
        {
            return JsonSerializer.Serialize(changeGoodMenu);
        }
    }
}