using BLL.Menus;
using DAL.Repositories;
using Shared.Interfaces;
using Shared.Models;

namespace BLL
{
    public class Settings
    {
        ICategoriesRepository categoryRepo = new DapperCategoriesRepository();
        IGoodsRepository goodsRepo = new DapperGoodsRepository();
        IUsersRepository usersRepo = new DapperUsersRepository();
        ICartsRepository cartRepo = new DapperCartsRepository();
        IOrdersRepository orderRepo = new DapperOrdersRepository();

        #region Menus
        // Загружаем главного меню в UI с проверкой роли
        public string LoadMainMenu(long id)
        {
            MainMenu mainMenu = new MainMenu();
            var userMenu = mainMenu.GetMainMenu(usersRepo.CheckUser(id));
            return userMenu;
        }

        // Загружаем меню кнопок выбранного товара
        public string LoadGoodsItemMenu()
        {
            GoodsItemMenu goodsItemMenu = new GoodsItemMenu();
            return goodsItemMenu.GetGoodsItemMenu();
        }

        // Загружаем меню кнопок корзины
        public string LoadCartMenu()
        {
            CartMenu cartMenu = new CartMenu();
            return cartMenu.GetCartMenu();
        }

        // Загружаем меню кнопок для изменения корзины
        public string LoadCartChangeMenu()
        {
            CartChangeMenu cartChangeMenu = new CartChangeMenu();
            return cartChangeMenu.GetCartChangeMenu();
        }

        // Загружаем меню кнопок для изменения количества
        public string LoadChangeQuantityMenu()
        {
            ChangeQuantityMenu changeQuantityMenu = new ChangeQuantityMenu();
            return changeQuantityMenu.GetChangeQuantityMenu();
        }

        // Загружаем меню кнопок для перехода из оформленного заказа
        public string LoadOrderMenu()
        {
            OrderMenu orderMenu = new OrderMenu();
            return orderMenu.GetOrderMenu();
        }

        // Загружаем меню кнопок для изменения товара
        public string LoadChangeGoodMenu()
        {
            ChangeGoodMenu changeGoodMenu = new ChangeGoodMenu();
            return changeGoodMenu.GetChangeGoodMenu();
        }

        // Загружаем меню кнопок для выбора роли пользователя
        public string LoadUserRolesMenu()
        {
            UserRolesMenu userRolesMenu = new UserRolesMenu();
            return userRolesMenu.GetUserRolesMenu();
        }

        #endregion

        #region Goods

        // Возвращаем список товаров по его id категории
        public List<Good> GetGoods(int categorId)
        {
            return goodsRepo.GetAllGoods(categorId);
        }

        // Возвращаем список всех товаров
        public List<Good> GetGoods()
        {
            return goodsRepo.GetAllGoods();
        }

        // Возвращаем товар по его id
        public Good GetGood(int goodId)
        {
            return goodsRepo.GetGood(goodId);
        }

        // Изменяем количество выбранного товара в корзине пользователя
        public bool ChangeCartGoodQuantity(long userId, int goodId, int quantity)
        {
            return cartRepo.AddQuantity(userId, goodId, quantity);
        }

        // Изменяем количество выбранного товара
        public bool ChangeGoodQuantity(int goodId, int quantity)
        {
            return goodsRepo.ChangeQuantity(goodId, quantity);
        }

        // Добавляем новый товар
        public bool AddGood(int categoryId, string name, string description, decimal price, int quantity, string url)
        {
            return goodsRepo.Add(categoryId, name, description, price, quantity, url);
        }

        // Удаляем товар
        public bool DeleteGood(string name)
        {
            return goodsRepo.Delete(name);
        }

        // Меняем id категории выбранного товара
        public bool ChangeGoodCategoryId(int id, int categoryId)
        {
            return goodsRepo.ChangeCategoryId(id, categoryId);
        }

        // Меняем наименование товара
        public bool ChangeGoodName(int id, string name)
        {
            return goodsRepo.ChangeName(id, name);
        }

        // Меняем описание товара
        public bool ChangeGoodDescription(int id, string description)
        {
            return goodsRepo.ChangeDescription(id, description);
        }

        // Меняем стоимость товара
        public bool ChangeGoodPrice(int id, decimal price)
        {
            return goodsRepo.ChangePrice(id, price);
        }

        // Меняем ссылку на изображение товара
        public bool ChangeGoodUrl(int id, string url)
        {
            return goodsRepo.ChangeGoodUrl(id, url);
        }

        // Проверяем товар на нулевое доступное количество
        public bool isAvailableToBuy(int goodId)
        {
            var good = goodsRepo.GetGood(goodId);
            if (good.Quantity > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
              
        #endregion

        #region Carts
        
        // Добавляем товар и его количество в корзину
        public bool AddToCart(long userId, int goodsId, int quantity)
        {
            var flag = cartRepo.Add(userId, goodsId, quantity);
            if (!flag)
            {
                flag = cartRepo.AddQuantity(userId, goodsId, quantity);
                if (flag)
                {
                    return true;
                }
                else
                {
                    return false;
                }                
            }
            else
            {
                return true;
            }
        }

        // Возвращаем список товаров корзины
        public List<GoodView> GetCart(long userId)
        {
            return cartRepo.GetUserCartGoodView(userId);
        }

        // Удаляем товар по id из корзины пользователя
        public bool RemoveGoodFromCart(long userId, int goodId)
        {
            return cartRepo.Delete(userId, goodId);
        }

        // Очищаем корзину пользователя
        public void ClearUserCart(long userId)
        {
            var userCart = cartRepo.GetUserCart(userId);
            if (userCart != null)
            {
                foreach (var item in userCart)
                {
                    cartRepo.Delete(userId, item.GoodId);
                }
            }           
        }

        // Уменьшаем доступное количество товара и обновляем корзину текущего пользователя
        private decimal ReserveGoods(List<Cart> orderItems)
        {
            decimal totalSum = 0;
            foreach (var item in orderItems)
            {
                var currentGood = goodsRepo.GetGood(item.GoodId);
                if (item.Quantity <= currentGood.Quantity)
                {
                    var newQuantity = currentGood.Quantity - item.Quantity;
                    goodsRepo.ChangeQuantity(item.GoodId, newQuantity);
                    totalSum = totalSum + currentGood.Price * item.Quantity;
                }
                else
                {
                    goodsRepo.ChangeQuantity(item.GoodId, 0);
                    totalSum = totalSum + currentGood.Price * currentGood.Quantity;
                }
            }
            return totalSum;
        }

        #endregion

        #region Users

        // Создаем нового пользователя
        public bool AddUser(long id, Role role)
        {
            var flag = usersRepo.Add(id, role);
            return flag;
        }

        // Меняем роль существующего пользователя
        public bool ChangeRole(long id, Role role)
        {
            return usersRepo.ChangeRole(id, role);
        }

        // Выводим список пользователей
        public List<User> GetAllUsers()
        {
            return usersRepo.GetAllUsers();
        }

        // Удаляем пользователя по id
        public bool DeleteUser(long id)
        {
            return usersRepo.Delete(id);
        }

        #endregion

        #region Categories

        // Возвращаем список категорий товаров (каталог)
        public List<Category> GetCatalog()
        {
            return categoryRepo.GetAllCategories();
        }

        // Добавляем новую категорию
        public bool AddNewCategory(string name)
        {
            return categoryRepo.Add(name);
        }

        // Удаляем выбранному категорию по наименованию
        public bool DeleteCategory(string name)
        {
            return categoryRepo.Delete(name);
        }

        // Задаем новое имя выбранной категории
        public bool RenameCategory(string oldName, string newName)
        {
            return categoryRepo.Rename(oldName, newName);
        }

        // Проверяем наличие запрошенной категории
        public bool CategoryExists(string name)
        {
            return categoryRepo.isExists(name);
        }

        // Возвращаем id категории по наименованию, при отсутствии возвращает -1
        public int GetCategoryId(string name)
        {
            return categoryRepo.GetCategoryId(name);
        }

        #endregion

        #region Orders
        // Возвращаем список товаров для заказа
        public List<GoodView> GetOrderView(int orderId)
        {
            List<GoodView> orderView = new List<GoodView>();
            var userOrderItems = orderRepo.GetOrderItems(orderId);
            Good good;
            foreach (var item in userOrderItems)
            {
                good = goodsRepo.GetGood(item.GoodId);
                GoodView cw = new GoodView();
                cw.GoodId = good.Id;
                cw.GoodName = good.Name;
                cw.GoodPrice = good.Price;
                cw.Quantity = item.Quantity;
                orderView.Add(cw);
            }
            return orderView;
        }
        
        // Сформировать заказ
        public Order? AddOrder(long userId)
        {
            var cartItems = UpdateCart(userId);
            var orderId = -1;
            if (cartItems.Count() > 0)
            {
                var totalSum = ReserveGoods(cartItems);
                orderId = orderRepo.AddOrder(userId, totalSum);
                foreach (var item in cartItems)
                {
                    orderRepo.AddOrderItems(orderId, item.GoodId, goodsRepo.GetGood(item.GoodId).Price, item.Quantity);
                }
                ClearUserCart(userId);
                return orderRepo.GetOrder(orderId);
            }
            else
            {
                return null;
            }
        }             

        // Возвращаем все товары запрошенного заказа
        public List<OrderItems> GetOrderItems(int orderId)
        {
            return orderRepo.GetOrderItems(orderId);
        } 

        // Возвращаем список всех заказов пользователя
        public List<Order> GetOrders(long userId)
        {
            return orderRepo.GetUserOrders(userId);
        }

        // Возвращаем содержие корзины с учетом текущего остатка товаров
        private List<Cart> UpdateCart(long userId)
        {
            var userCart = cartRepo.GetUserCart(userId);
            foreach (var item in userCart)
            {
                var good = goodsRepo.GetGood(item.GoodId);

                if (good.Quantity == 0)
                {
                    cartRepo.AddQuantity(userId, good.Id, -item.Quantity);
                }
            }
            return cartRepo.GetUserCart(userId);
        }

        #endregion
    }
}
