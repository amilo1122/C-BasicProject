using Shared.Models;
using System.Text.Json;

namespace BLL.Menus
{
    public class MainMenu
    {
        List<Menu> administratorMenu = new List<Menu> {
            new Menu { Name = "Просмотр каталога", Callback = "browseCatalog" },
            new Menu { Name = "Добавить категорию", Callback = "addCategory" },
            new Menu { Name = "Переименовать категорию", Callback = "renameCategory" },
            new Menu { Name = "Удалить категорию", Callback = "deleteCategory" },
            new Menu { Name = "Добавить товар", Callback = "addGood" },
            new Menu { Name = "Изменить товар", Callback = "modifyGood" },
            new Menu { Name = "Удалить товар", Callback = "deleteGoodFromRep" },
            new Menu { Name = "Перейти в корзину", Callback = "browseCart" },
            new Menu { Name = "Создать пользователя", Callback = "createUser" },
            new Menu { Name = "Удалить пользователя", Callback = "deleteUser" },
            new Menu { Name = "Изменить роль", Callback = "changeRole" },
            new Menu { Name = "Мои заказы", Callback = "myOrders" }
        };

        List<Menu> managerMenu = new List<Menu> {
            new Menu { Name = "Просмотр каталога", Callback = "browseCatalog" },
            new Menu { Name = "Добавить категорию", Callback = "addCategory" },
            new Menu { Name = "Переименовать категорию", Callback = "renameCategory" },
            new Menu { Name = "Удалить категорию", Callback = "deleteCategory" },
            new Menu { Name = "Добавить товар", Callback = "addGood" },
            new Menu { Name = "Изменить товар", Callback = "modifyGood" },
            new Menu { Name = "Удалить товар", Callback = "deleteGoodFromRep" },
            new Menu { Name = "Перейти в корзину", Callback = "browseCart" },
            new Menu { Name = "Мои заказы", Callback = "myOrders" }
        };

        List<Menu> customerMenu = new List<Menu> {
            new Menu { Name = "Просмотр каталога", Callback = "browseCatalog" },
            new Menu { Name = "Перейти в корзину", Callback = "browseCart" },
            new Menu { Name = "Мои заказы", Callback = "myOrders" }
        };

        public string? GetMainMenu(Role role)
        {
            switch (role)
            {
                case Role.Administrator:
                    return JsonSerializer.Serialize(administratorMenu);
                case Role.Manager:
                    return JsonSerializer.Serialize(managerMenu);
                case Role.Customer:
                    return JsonSerializer.Serialize(customerMenu);
                default:
                    return null;
            }

        }

    }
}
