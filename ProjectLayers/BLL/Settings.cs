using BLL.Menus;
using DAL.FileStores;
using DAL.Repositories;
using Shared.Interfaces;
using Shared.Models;

namespace BLL
{
    public class Settings
    {
        ICategoryRepository categoryRepo = new ListCategoryRepositoty();
        IGoodsRepository goodsRepo = new ListGoodsRepository();
        IUserRepository usersRepo = new ListUserRepository();
        ICartRepository cartRepo = new FileCartRepository();
        IOrderRepository orderRepo = new FileOrderRepository();

        IniFile CategoryIni, GoodsIni, CartIni, OrdersIni, OrderItemsIni, UsersIni;
        
        string categoryFileName = "Categories.ini";
        string goodsFileName = "Goods.ini";
        string cartFileName = "Cart.ini";
        string ordersFileName = "Orders.ini";
        string orderItemsFileName = "OrderItems.ini";
        string usersFileName = "Users.ini";

        #region Menus
        // Загружаем главного меню в UI с проверкой роли
        public string LoadMainMenu(long id)
        {
            MainMenu mainMenu = new MainMenu();
            var userMenu = mainMenu.GetMainMenu(usersRepo.CheckUser(id));
            SaveUsers();
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
            var allGoods = goodsRepo.GetAllGoods();
            return allGoods.Where(x => x.CategoryId == categorId).ToList();
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
        public void ChangeQuantity(long userId, int goodId, int quantity)
        {
            cartRepo.UpdateQuantity(userId, goodId, quantity);
            SaveCart();
        }

        // Изменяем количество выбранного товара
        public void ChangeQuantity(int goodId, int quantity)
        {
            goodsRepo.ChangeQuantity(goodId, quantity);
            SaveGoods();
        }

        // Добавляем новый товар
        public void AddGood(int categoryId, string name, string description, decimal price, int quantity, string url)
        {
            goodsRepo.Add(categoryId, name, description, price, quantity, url);
            SaveGoods();
        }

        // Удаляем товар
        public bool DeleteGood(string name)
        {
            if (goodsRepo.Delete(name))
            {
                SaveGoods();
                return true;
            }
            else
            {
                return false;
            }
        }

        // Меняем id категории выбранного товара
        public void ChangeGoodCategoryId(int id, int categoryId)
        {
            goodsRepo.ChangeCategoryId(id, categoryId);
            SaveGoods();
        }

        // Меняем наименование товара
        public void ChangeGoodName(int id, string name)
        {
            goodsRepo.ChangeName(id, name);
            SaveGoods();
        }

        // Меняем описание товара
        public void ChangeGoodDescription(int id, string description)
        {
            goodsRepo.ChangeDescription(id, description);
            SaveGoods();
        }

        // Меняем стоимость товара
        public void ChangeGoodPrice(int id, decimal price)
        {
            goodsRepo.ChangePrice(id, price);
            SaveGoods();
        }

        // Меняем ссылку на изображение товара
        public void ChangeGoodUrl(int id, string url)
        {
            goodsRepo.ChangeGoodUrl(id, url);
            SaveGoods();
        }

        // Загружаем категории и товары из файла
        private void LoadGoods()
        {
            CategoryIni = new IniFile(categoryFileName);
            GoodsIni = new IniFile(goodsFileName);

            var categoryFile = File.ReadAllLines(categoryFileName);
            var goodsFile = File.ReadAllLines(goodsFileName);
            List<string> categorySection = new List<string>();
            List<string> goodsSection = new List<string>();

            foreach (var s in categoryFile)
            {
                if (s.StartsWith("["))
                {
                    string newline = s.Replace("[", "");
                    newline = newline.Replace("]", "");
                    categorySection.Add(newline);
                }
            }

            foreach (var s in goodsFile)
            {
                if (s.StartsWith("["))
                {
                    string newline = s.Replace("[", "");
                    newline = newline.Replace("]", "");
                    goodsSection.Add(newline);
                }
            }

            if (!File.Exists(categoryFileName) || !File.Exists(goodsFileName))
            {
                Console.WriteLine("Загрузочные файлы отсутствуют. Нажмите ввод для продолжения....");
                Console.ReadKey();
            }
            else
            {
                foreach (var s in categorySection)
                {
                    categoryRepo.Add(CategoryIni.Read("Name", s), Int32.Parse(CategoryIni.Read("Id", s)));
                }
                foreach (var s in goodsSection)
                {
                    int categoryId = Int32.Parse(GoodsIni.Read("CategoryId", s));
                    string name = GoodsIni.Read("Name", s);
                    string description = GoodsIni.Read("Description", s);
                    int id = Int32.Parse(GoodsIni.Read("Id", s));
                    decimal price = Decimal.Parse(GoodsIni.Read("Price", s));
                    int quantity = Int32.Parse(GoodsIni.Read("Quantity", s));
                    string url = GoodsIni.Read("Url", s);
                    goodsRepo.Add(categoryId, name, description, id, price, quantity, url);
                }
            }
        }

        // Сохраняем категории и товары в файл
        private void SaveGoods()
        {
            CategoryIni = new IniFile(categoryFileName);
            GoodsIni = new IniFile(goodsFileName);

            if (File.Exists(categoryFileName))
            {
                File.Delete(categoryFileName);
            }
            if (File.Exists(goodsFileName))
            {
                File.Delete(goodsFileName);
            }

            foreach (var item in categoryRepo.GetCategoryList())
            {
                CategoryIni.Write("Name", item.Name, item.Id.ToString());
                CategoryIni.Write("Id", item.Id.ToString(), item.Id.ToString());
            }
            foreach (var item in goodsRepo.GetAllGoods())
            {
                GoodsIni.Write("CategoryId", item.CategoryId.ToString(), item.Id.ToString());
                GoodsIni.Write("Name", item.Name, item.Id.ToString());
                GoodsIni.Write("Description", item.Description, item.Id.ToString());
                GoodsIni.Write("Id", item.Id.ToString(), item.Id.ToString());
                GoodsIni.Write("Price", item.Price.ToString(), item.Id.ToString());
                GoodsIni.Write("Quantity", item.Quantity.ToString(), item.Id.ToString());
                GoodsIni.Write("Url", item.Url.ToString(), item.Id.ToString());
            }
        }
        #endregion

        #region Carts
        // Сохраняем корзину в файл
        public void SaveCart()
        {
            CartIni = new IniFile(cartFileName);

            foreach (var item in cartRepo.GetAllCart())
            {
                CartIni.Write("Id", item.Id.ToString(), item.Id.ToString());
                CartIni.Write("UserId", item.UserId.ToString(), item.Id.ToString());
                CartIni.Write("GoodsId", item.GoodId.ToString(), item.Id.ToString());
                CartIni.Write("Quantity", item.Quantity.ToString(), item.Id.ToString());
            }
        }

        // Добавляем товар и его количество в корзину
        public void AddToCart(long userId, int goodsId, int quantity)
        {
            var userCart = cartRepo.GetUserCart(userId);

            if (userCart.Where(x => x.GoodId == goodsId).ToList().Count < 1)
            {
                cartRepo.Add(userId, goodsId, quantity);
            }
            else
            {
                cartRepo.UpdateQuantity(userId, goodsId, quantity);
            }
            SaveCart();
        }

        // Возвращаем список товаров корзины
        public List<GoodsView> GetCart(long userId)
        {
            List<GoodsView> cartView = new List<GoodsView>();
            var userCart = cartRepo.GetUserCart(userId);
            Good good;
            foreach (var item in userCart)
            {
                good = goodsRepo.GetGood(item.GoodId);
                GoodsView cw = new GoodsView();
                cw.GoodId = good.Id;
                cw.GoodName = good.Name;
                cw.GoodPrice = good.Price;
                cw.Quantity = item.Quantity;
                cartView.Add(cw);
            }
            return cartView;
        }

        // Удаляем товар по id из корзины пользователя
        public void RemoveGoodFromCart(long userId, int goodId)
        {
            DeleteGoodCartIni(userId, goodId);
            cartRepo.Remove(userId, goodId);
        }

        // Очищаем корзину пользователя
        public void ClearUserCart(long userId)
        {
            var userCart = cartRepo.GetUserCart(userId);
            foreach (var item in userCart)
            {
                DeleteGoodCartIni(userId, item.GoodId);
                cartRepo.Remove(userId, item.GoodId);
            }
        }

        // Уменьшаем доступное количество товара и обновляем корзину текущего пользователя
        private decimal ReserveGoods(long userId)
        {
            var userCart = cartRepo.GetUserCart(userId);
            decimal totalSum = 0;
            foreach (var item in userCart)
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
                    totalSum = totalSum + currentGood.Price * currentGood.Quantity;
                    goodsRepo.ChangeQuantity(item.GoodId, 0);
                }
            }
            return totalSum;
        }

        // Удаляем товар по id из файла корзины пользователя
        private void DeleteGoodCartIni(long userId, int goodId)
        {
            CartIni = new IniFile(cartFileName);
            var userCart = cartRepo.GetUserCart(userId);
            foreach (var item in userCart)
            {
                if (item.GoodId == goodId)
                {
                    CartIni.DeleteSection(item.Id.ToString());
                }
            }
        }

        // Загружаем корзину из файла
        private void LoadCart()
        {
            CartIni = new IniFile(cartFileName);

            var cartFile = File.ReadAllLines(cartFileName);
            List<string> cartSection = new List<string>();

            foreach (var s in cartFile)
            {
                if (s.StartsWith("["))
                {
                    string newline = s.Replace("[", "");
                    newline = newline.Replace("]", "");
                    cartSection.Add(newline);
                }
            }

            if (!File.Exists(cartFileName))
            {
                throw new Exception("Загрузочные файлы отсутствуют. Нажмите ввод для продолжения....");
            }
            else
            {
                foreach (var s in cartSection)
                {
                    cartRepo.Add(Int64.Parse(CartIni.Read("UserId", s)), Int32.Parse(CartIni.Read("GoodsId", s)), Int32.Parse(CartIni.Read("Quantity", s)), Int32.Parse(CartIni.Read("Id", s)));
                }
            }
        }
        #endregion

        #region Users
        // Сохраняем пользователей в файл
        public void SaveUsers()
        {
            UsersIni = new IniFile(usersFileName);

            if (File.Exists(usersFileName))
            {
                File.Delete(usersFileName);
            }

            foreach (var item in usersRepo.GetAllUsers())
            {
                UsersIni.Write("Id", item.Id.ToString(), item.Id.ToString());
                UsersIni.Write("Role", item.Role.ToString(), item.Id.ToString());
            }
        }

        // Создаем нового пользователя
        public bool AddUser(long id, Role role)
        {
            var flag = usersRepo.Add(id, role);
            SaveUsers();
            return flag;
        }

        // Меняем роль существующего пользователя
        public void ChangeRole(long id, Role role)
        {
            usersRepo.ChangeRole(id, role);
            SaveUsers();
        }

        // Выводим список пользователей
        public List<User> GetAllUsers()
        {
            return usersRepo.GetAllUsers();
        }

        // Удаляем пользователя по id
        public void DeleteUser(long id)
        {
            usersRepo.Delete(id);
            SaveUsers();
        }

        // Загружаем пользователей из файла
        private void LoadUsers()
        {
            UsersIni = new IniFile(usersFileName);

            var usersFile = File.ReadAllLines(usersFileName);
            List<string> usersSection = new List<string>();

            foreach (var s in usersFile)
            {
                if (s.StartsWith("["))
                {
                    string newline = s.Replace("[", "");
                    newline = newline.Replace("]", "");
                    usersSection.Add(newline);
                }
            }

            if (!File.Exists(cartFileName))
            {
                throw new Exception("Загрузочные файлы отсутствуют. Нажмите ввод для продолжения....");
            }
            else
            {
                foreach (var s in usersSection)
                {
                    usersRepo.Add(new User(Int64.Parse(UsersIni.Read("Id", s)), Enum.Parse<Role>(UsersIni.Read("Role", s))));
                }
            }
        }
        #endregion

        #region Categories
        // Возвращаем список категорий товаров (каталог)
        public List<Category> GetCatalog()
        {
            if (categoryRepo.Browse().Count > 0)
            {
                return categoryRepo.Browse();
            }
            else
            {
                return null;
            }
        }

        // Добавляем новую категорию
        public void AddNewCategory(string name)
        {
            categoryRepo.Add(name);
            SaveGoods();
        }

        // Удаляем выбранному категорию по наименованию
        public bool DeleteCategory(string name)
        {
            if (categoryRepo.Delete(name))
            {
                SaveGoods();
                return true;
            }
            else
            {
                return false;
            }
        }

        // Задаем новое имя выбранной категории
        public void RenameCategory(string oldName, string newName)
        {
            categoryRepo.Rename(oldName, newName);
            SaveGoods();
        }

        // Проверяем наличие запрошенной категории
        public bool CategoryExists(string name)
        {
            if (categoryRepo.isExists(name))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Возвращаем имя категории по id
        public int GetCategoryId(string name)
        {
            return categoryRepo.GetCategoryId(name);
        }
        #endregion

        #region Orders
        // Возвращаем список товаров для заказа
        public List<GoodsView> GetOrderView(int orderId)
        {
            List<GoodsView> orderView = new List<GoodsView>();
            var userOrderItems = orderRepo.GetOrderItems(orderId);
            Good good;
            foreach (var item in userOrderItems)
            {
                good = goodsRepo.GetGood(item.GoodId);
                GoodsView cw = new GoodsView();
                cw.GoodId = good.Id;
                cw.GoodName = good.Name;
                cw.GoodPrice = good.Price;
                cw.Quantity = item.Quantity;
                orderView.Add(cw);
            }
            return orderView;
        }
        
        // Сформировать заказ
        public Order AddOrder(long userId)
        {
            var totalSum = ReserveGoods(userId);
            var orderId = orderRepo.Add(userId, totalSum);
            var orderItems = cartRepo.GetUserCart(userId).ToList();
            foreach (var item in orderItems)
            {
                orderRepo.AddOrderItems(orderId, item.GoodId, goodsRepo.GetGood(item.GoodId).Price, item.Quantity);
            }
            SaveOrder();
            SaveGoods();
            ClearUserCart(userId);
            return orderRepo.GetAllOrders().Single(x => x.Id == orderId);
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

        // Сохраняем заказ в файл
        private void SaveOrder()
        {
            OrdersIni = new IniFile(ordersFileName);
            OrderItemsIni = new IniFile(orderItemsFileName);

            foreach (var item in orderRepo.GetAllOrders())
            {
                OrdersIni.Write("Id", item.Id.ToString(), item.Id.ToString());
                OrdersIni.Write("UserId", item.UserId.ToString(), item.Id.ToString());
                OrdersIni.Write("TotalSum", item.TotalSum.ToString(), item.Id.ToString());
                OrdersIni.Write("CreatedDate", item.CreatedDate.ToString(), item.Id.ToString());
            }
            foreach (var item in orderRepo.GetAllOrdersItems())
            {
                OrderItemsIni.Write("Id", item.Id.ToString(), item.Id.ToString());
                OrderItemsIni.Write("OrderId", item.OrderId.ToString(), item.Id.ToString());
                OrderItemsIni.Write("GoodId", item.GoodId.ToString(), item.Id.ToString());
                OrderItemsIni.Write("Price", item.Price.ToString(), item.Id.ToString());
                OrderItemsIni.Write("Quantity", item.Quantity.ToString(), item.Id.ToString());
            }
        }

        // Загружаем заказы из файла
        private void LoadOrders()
        {
            OrdersIni = new IniFile(ordersFileName);

            var ordersFile = File.ReadAllLines(ordersFileName);
            List<string> ordersSection = new List<string>();

            foreach (var s in ordersFile)
            {
                if (s.StartsWith("["))
                {
                    string newline = s.Replace("[", "");
                    newline = newline.Replace("]", "");
                    ordersSection.Add(newline);
                }
            }

            if (!File.Exists(ordersFileName))
            {
                throw new Exception("Загрузочные файлы отсутствуют. Нажмите ввод для продолжения....");
            }
            else
            {
                foreach (var s in ordersSection)
                {
                    var userId = Int64.Parse(OrdersIni.Read("UserId", s));
                    var totalSum = Decimal.Parse(OrdersIni.Read("TotalSum", s));
                    var id = Int32.Parse(OrdersIni.Read("Id", s));
                    var date = Convert.ToDateTime(OrdersIni.Read("CreatedDate", s));
                    orderRepo.Add(userId, totalSum, id, date);
                }
            }
        }
        #endregion


        // Загружаем репозитории
        public void LoadRepositories()
        {
            LoadGoods();
            LoadCart();
            LoadOrders();
            LoadUsers();
        } 
    }
}
