using BLL;
using Shared.Models;
using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

// Объявляем переменную для хранения ключа
const string key = @"c:/apikey/apikey.txt";
// Читаем ключ из файла
var apiString = ReadApiKey(key);

var botClient = new TelegramBotClient(apiString);
Settings settings = new Settings();

// Объявляем токен отмены
using var cts = new CancellationTokenSource();
// Объявляем переменную статуса
var State = 0;
// Объявляем словарь для обработки меню ролей пользователя
Dictionary<long, bool> _changeRoleDict = new Dictionary<long, bool>();
// Объявляем словарь для обработки меню ролей пользователя
Dictionary<long, bool> _addUserDict = new Dictionary<long, bool>();
// Объявляем словарь для хранения выбранного id товара
Dictionary<long, int> _goodIdDict = new Dictionary<long, int>();
// Объявляем словарь для хранения выбранного наименования категории
Dictionary<long, string> _categoryNameDict = new Dictionary<long, string>();
// Объявляем словарь для хранения реквизитов нового товара
Dictionary<long, Good> _goodsDict = new Dictionary<long, Good>();
// Объявляем словарь для хранения id пользователя
Dictionary<long, int> _userIdDict = new Dictionary<long, int>();
// Объявляем массив методов для обработки сообщений от пользователей
List<Method> methods = new List<Method>()
{
    new Method(1, MisundestatingAnswer),
    new Method(2, AddNewCategory),
    new Method(3, DeleteCategory),
    new Method(4, CategoryExists),
    new Method(5, RenameCategory),
    new Method(6, CheckCategory),
    new Method(7, GetGoodName),
    new Method(8, GetGoodDescription),
    new Method(9, GetGoodPrice),
    new Method(10, GetGoodQuantity),
    new Method(11, GetGoodUrl),
    new Method(12, DeleteGood),
    new Method(13, SetNewGoodName),
    new Method(14, SetNewGoodDescription),
    new Method(15, SetNewGoodPrice),
    new Method(16, SetNewGoodQuantity),
    new Method(17, SetNewGoodUrl),
    new Method(18, RequestNewUserRole),
    new Method(19, SetGood)    
};


// Объявляем настройки получения обновлений
var receiverOptions = new ReceiverOptions
{
    AllowedUpdates = { }
};

// Компануем и начинаем получать обновления от телеграмма
botClient.StartReceiving(
    //название метода, в котором обрабатываем обновление бота
    HandleUpdatesAsync,
    //название метода, в котором обработываем обшибки
    HandleErrorAsync,
    //настройки получений обновлений
    receiverOptions,
    //токен отмены
    cancellationToken: cts.Token);

var me = await botClient.GetMeAsync();

Console.WriteLine($"Начал прослушку @{me.Username}");
Console.ReadLine();

// Указываем токен отмены
cts.Cancel();

// Обрабатываем Update
async Task HandleUpdatesAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    try
    {
        switch (update.Type)
        {
            case (UpdateType.Message):
                await BotOnMessageReceived(botClient, update.Message);
                break;
            case (UpdateType.CallbackQuery):
                await InlineModeProcessing(botClient, update.CallbackQuery);
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}

async Task BotOnMessageReceived(ITelegramBotClient botClient, Message? message)
{
    Console.WriteLine($"Receive message type {message.Type}");
    if (message.Type != MessageType.Text)
    {
        return;
    }
    switch (message.Text)
    {
        case ("/start"):
            await LoadMainMenu(botClient, message);
            break;
        default:
            await OnMessageProcessing(botClient, message);
            break;
    }
}

// Обрабатываем CallbackQuery сообщения
async Task InlineModeProcessing(ITelegramBotClient botClient, CallbackQuery callbackQuery)
{
    Console.WriteLine($"Receive message type {callbackQuery.Message.Type}");

    switch (callbackQuery.Data)
    {
        case string s when s.StartsWith("mainMenu"):
            await LoadMainMenu(botClient, callbackQuery.Message);
            break;
        case string s when s.StartsWith("browseCatalog"):
            await DisplayCatalog(botClient, callbackQuery);
            break;
        case string s when s.StartsWith("categories"):
            Console.WriteLine(callbackQuery.Data);
            await DisplayGoods(botClient, callbackQuery);
            break;
        case string s when s.StartsWith("goods"):
            Console.WriteLine(callbackQuery.Id);
            await DisplayGoodsItem(botClient, callbackQuery);
            break;
        case string s when s.StartsWith("addToCart"):
            Console.WriteLine(callbackQuery.Id);
            await AddToCart(botClient, callbackQuery);
            break; 
        case string s when s.StartsWith("browseCart"):
            Console.WriteLine(callbackQuery.Id);
            await DisplayCart(botClient, callbackQuery);
            break; 
        case string s when s.StartsWith("changeCart"):
            Console.WriteLine(callbackQuery.Id);
            await ChangeCart(botClient, callbackQuery);
            break;
        case string s when s.StartsWith("changeQuantity"):
            Console.WriteLine(callbackQuery.Id);
            await DisplayChangeQuantityButtons(botClient, callbackQuery);
            break;
        case string s when s.StartsWith("deleteGoodFromCart"):
            Console.WriteLine(callbackQuery.Id);
            await DeleteGoodFromCart(botClient, callbackQuery);
            break;
        case string s when s.StartsWith("increase"):
            Console.WriteLine(callbackQuery.Id);
            await IncreaseQuantity(botClient, callbackQuery);
            break;
        case string s when s.StartsWith("reduce"):
            Console.WriteLine(callbackQuery.Id);
            await ReduceQuantity(botClient, callbackQuery);
            break;
        case string s when s.StartsWith("checkout"):
            Console.WriteLine(callbackQuery.Id);
            await Checkout(botClient, callbackQuery);
            break; 
        case string s when s.StartsWith("myOrders"):
            Console.WriteLine(callbackQuery.Id);
            await DisplayMyOrders(botClient, callbackQuery);
            break; 
        case string s when s.StartsWith("addCategory"):
            Console.WriteLine(callbackQuery.Id);
            await RequestNewCategoryName(botClient, callbackQuery);
            break;
        case string s when s.StartsWith("deleteCategory"):
            Console.WriteLine(callbackQuery.Id);
            await RequestCategoryName(botClient, callbackQuery);
            break;
        case string s when s.StartsWith("renameCategory"):
            Console.WriteLine(callbackQuery.Id);
            await RenameCategoryRequest(botClient, callbackQuery);
            break; 
        case string s when s.StartsWith("addGood"):
            Console.WriteLine(callbackQuery.Id);
            await NewGoodRequest(botClient, callbackQuery);
            break;
        case string s when s.StartsWith("deleteGoodFromRep"):
            Console.WriteLine(callbackQuery.Id);
            await RequestGoodToDelete(botClient, callbackQuery);
            break; 
        case string s when s.StartsWith("modifyGood"):
            Console.WriteLine(callbackQuery.Id);
            await SelectGoodToChange(botClient, callbackQuery);
            break;
        case string s when s.StartsWith("goodToChange"):
            Console.WriteLine(callbackQuery.Id);
            await LoadChangeGoodMenu(botClient, callbackQuery);
            break;
        case string s when s.StartsWith("changeGoodCategory"):
            Console.WriteLine(callbackQuery.Id);
            await DisplayCategoriesListToChange(botClient, callbackQuery);
            break;
        case string s when s.StartsWith("changeGoodNewCategory"):
            Console.WriteLine(callbackQuery.Id);
            await ChangeGoodNewCategoryName(botClient, callbackQuery);
            break; 
        case string s when s.StartsWith("changeGoodName"):
            Console.WriteLine(callbackQuery.Id);
            await ChangeGoodNewName(botClient, callbackQuery);
            break; 
        case string s when s.StartsWith("changeGoodDescription"):
            Console.WriteLine(callbackQuery.Id);
            await ChangeGoodDescription(botClient, callbackQuery);
            break;
        case string s when s.StartsWith("changeGoodPrice"):
            Console.WriteLine(callbackQuery.Id);
            await ChangeGoodPrice(botClient, callbackQuery);
            break;
        case string s when s.StartsWith("changeGoodQuantity"):
            Console.WriteLine(callbackQuery.Id);
            await ChangeGoodQuantity(botClient, callbackQuery);
            break;
        case string s when s.StartsWith("changeGoodUrl"):
            Console.WriteLine(callbackQuery.Id);
            await ChangeGoodUrl(botClient, callbackQuery);
            break;
        case string s when s.StartsWith("changeRole"):
            Console.WriteLine(callbackQuery.Id);
            SetChangeRoleFlag(callbackQuery, true);
            SetNewUserFlag(callbackQuery, true);
            await DisplayUsers(botClient, callbackQuery.Message);
            break;
        case string s when s.StartsWith("userToChange"):
            Console.WriteLine(callbackQuery.Id);
            await CheckAddUserFlag(botClient, callbackQuery);            
            break;
        case string s when (s.StartsWith("Administrator") || s.StartsWith("Manager") || s.StartsWith("Customer")):
            Console.WriteLine(callbackQuery.Id);
            await CheckChangeRoleFlag(botClient, callbackQuery);
            break;
        case string s when s.StartsWith("createUser"):
            Console.WriteLine(callbackQuery.Id);
            SetChangeRoleFlag(callbackQuery, false);
            await RequestNewUserId(botClient, callbackQuery);
            break;
        case string s when s.StartsWith("deleteUser"):
            Console.WriteLine(callbackQuery.Id);
            SetNewUserFlag(callbackQuery, false);
            await DisplayUsers(botClient, callbackQuery.Message);
            break;
    }
}

// Читаем API ключ
string ReadApiKey(string path)
{
    return System.IO.File.ReadAllText(path);
}

// Проверяем метод вызова
async Task CheckAddUserFlag(ITelegramBotClient botClient, CallbackQuery callbackQuery)
{
    var id = callbackQuery.Message.Chat.Id;
    if (_addUserDict[id])
    {
        await SaveSelectedUser(botClient, callbackQuery);
        await DisplayUserRoles(botClient, callbackQuery.Message);
    }
    else
    {
        var userId = Int32.Parse(callbackQuery.Data.Replace("userToChange", ""));
        settings.DeleteUser(userId);
        await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Выбранный пользователь удален");

        await LoadMainMenu(botClient, callbackQuery.Message);
    }
}

// Устанавливаем флаг о новом пользователе
void SetNewUserFlag(CallbackQuery callbackQuery, bool flag)
{
    var id = callbackQuery.Message.Chat.Id;
    if (_addUserDict.ContainsKey(id))
    {
        _addUserDict.Remove(id);
    }
    _addUserDict[id] = flag;
}

// Сохраняем выбранного пользователя в словарь
async Task SaveSelectedUser(ITelegramBotClient botClient, CallbackQuery callbackQuery)
{
    var userId = Int32.Parse(callbackQuery.Data.Replace("userToChange", ""));
    var id = callbackQuery.Message.Chat.Id;
    if (_userIdDict.ContainsKey(id))
    {
        _userIdDict.Remove(id);
    }
    _userIdDict[id] = userId;
}

// Устанавливаем флаг о новой роли
void SetChangeRoleFlag(CallbackQuery callbackQuery, bool flag)
{
    var id = callbackQuery.Message.Chat.Id;
    if (_changeRoleDict.ContainsKey(id))
    {
        _changeRoleDict.Remove(id);
    }
    _changeRoleDict[id] = flag;
}

// Выводим список пользователей
async Task DisplayUsers(ITelegramBotClient botClient, Message message)
{
    var users = settings.GetAllUsers();
    List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();
    if (users != null)
    {
        foreach (var user in users)
        {
            buttons.Add(InlineKeyboardButton.WithCallbackData(text: $"ID:{user.Id} Role:{user.Role}", callbackData: "userToChange" + user.Id));
        }
        InlineKeyboardMarkup usersKeyboard = new InlineKeyboardMarkup(SetOneColumnMenu(buttons).ToArray());
        await botClient.SendTextMessageAsync(message.Chat.Id, "Выберите пользователя:", replyMarkup: usersKeyboard);
    }
    else
    {
        await botClient.SendTextMessageAsync(message.Chat.Id, "Список пользователей пуст!");
    }
}

// Создаем нового пользователя
async Task CreateNewUser(ITelegramBotClient botClient, CallbackQuery callbackQuery)
{
    var id = callbackQuery.Message.Chat.Id;
    var flag = settings.AddUser(_userIdDict[id], Enum.Parse<Role>(callbackQuery.Data));
    if (flag)
    {
        await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Новый пользователь добавлен");        
    }
    else
    {
        await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Пользователь уже существует");
    }
    await LoadMainMenu(botClient, callbackQuery.Message);
}

// Проверяем метод вызова
async Task CheckChangeRoleFlag(ITelegramBotClient botClient, CallbackQuery callbackQuery)
{
    var id = callbackQuery.Message.Chat.Id;
    if (_changeRoleDict[id])
    {
        await ChangeUserRole(botClient, callbackQuery);
    }
    else
    {
        await CreateNewUser(botClient, callbackQuery);
    }
}

// Запрашиваем данные нового пользователя
async Task RequestNewUserId(ITelegramBotClient botClient, CallbackQuery callbackQuery)
{
    await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Введите id нового пользователя:");
    State = 18;
}

// Изменяем роль существующего пользователя
async Task ChangeUserRole(ITelegramBotClient botClient, CallbackQuery callbackQuery)
{
    var id = callbackQuery.Message.Chat.Id;
    settings.ChangeRole(_userIdDict[id], Enum.Parse<Role>(callbackQuery.Data));
    await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Роль пользователя изменена:");

    await LoadMainMenu(botClient, callbackQuery.Message);
}

// Выводим кнопки ролей пользователя
async Task DisplayUserRoles(ITelegramBotClient botClient, Message message)
{
    List<Menu> userRolesMenu = JsonSerializer.Deserialize<List<Menu>>(settings.LoadUserRolesMenu());
    List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();

    foreach (var button in userRolesMenu)
    {
        buttons.Add(InlineKeyboardButton.WithCallbackData(text: button.Name, callbackData: button.Callback));
    }

    InlineKeyboardMarkup userRolesKeyboard = new InlineKeyboardMarkup(SetTwoColumnsMenu(buttons).ToArray());
    await botClient.SendTextMessageAsync(message.Chat.Id, "Выберите необходимую роль:", replyMarkup: userRolesKeyboard);
}

// Запрашиваем новую ссылку на изображение
async Task ChangeGoodUrl(ITelegramBotClient botClient, CallbackQuery callbackQuery)
{
    await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Введите новую ссылку на изображение товара:");
    State = 17;
}

// Запрашиваем новое количество доступного товара
async Task ChangeGoodQuantity(ITelegramBotClient botClient, CallbackQuery callbackQuery)
{
    await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Введите новое количество товара:");
    State = 16;
}

// Запрашиваем новую стоимость товара
async Task ChangeGoodPrice(ITelegramBotClient botClient, CallbackQuery callbackQuery)
{
    await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Введите новую стоимость товара:");
    State = 15;
}

// Запрашиваем новое описание товара
async Task ChangeGoodDescription(ITelegramBotClient botClient, CallbackQuery callbackQuery)
{
    await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Введите новое описание товара:");
    State = 14;
}

// Запрашиваем новое наименование товара
async Task ChangeGoodNewName(ITelegramBotClient botClient, CallbackQuery callbackQuery)
{
    await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Введите новое наименование товара:");
    State = 13;
}

// Меняем категорию товара
async Task ChangeGoodNewCategoryName(ITelegramBotClient botClient, CallbackQuery callbackQuery)
{
    var categoryId = Int32.Parse(callbackQuery.Data.Replace("changeGoodNewCategory", ""));
    var id = callbackQuery.Message.Chat.Id;
    if (!_goodIdDict.ContainsKey(id))
    {
        await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Сначала выберите товар для изменения!");
        return;
    }
    settings.ChangeGoodCategoryId(_goodIdDict[id], categoryId);
    _goodIdDict.Remove(id);

    await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Категория товара изменена");
    await LoadMainMenu(botClient, callbackQuery.Message);
}

// Выводим список категорий для изменения товара
async Task DisplayCategoriesListToChange(ITelegramBotClient botClient, CallbackQuery callbackQuery)
{
    List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();
    var categories = settings.GetCatalog();

    if (categories != null)
    {
        foreach (var category in categories)
        {
            buttons.Add(InlineKeyboardButton.WithCallbackData(text: $"ID:{category.Id} Name:{category.Name}", callbackData: "changeGoodNewCategory" + category.Id));
        }
        InlineKeyboardMarkup goodsKeyboard = new InlineKeyboardMarkup(SetOneColumnMenu(buttons).ToArray());
        await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Выберите категорию для перемещения товара:", replyMarkup: goodsKeyboard);
    }
    else
    {
        await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Список категорий пуст!");
    }
}

// Сохраняем выбранный товар в словарь и выводим кнопки меню изменения товара
async Task LoadChangeGoodMenu(ITelegramBotClient botClient, CallbackQuery callbackQuery)
{
    var goodId = Int32.Parse(callbackQuery.Data.Replace("goodToChange", ""));
    var id = callbackQuery.Message.Chat.Id;
    if (_goodIdDict.ContainsKey(id))
    {
        _goodIdDict.Remove(id);
    }
    _goodIdDict[id] = goodId;
    
    List<Menu> changeGoodMenu = JsonSerializer.Deserialize<List<Menu>>(settings.LoadChangeGoodMenu());
    List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();

    foreach (var button in changeGoodMenu)
    {
        buttons.Add(InlineKeyboardButton.WithCallbackData(text: button.Name, callbackData: button.Callback));
    }

    InlineKeyboardMarkup changeGoodKeyboard = new InlineKeyboardMarkup(SetTwoColumnsMenu(buttons).ToArray());
    await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Выберите пункт меню:", replyMarkup: changeGoodKeyboard);
}


// Запрашиваем наименование товара для изменения
async Task SelectGoodToChange(ITelegramBotClient botClient, CallbackQuery callbackQuery)
{
    List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();
    var goods = settings.GetGoods();

    if (goods != null)
    {
        foreach (var good in goods)
        {
            buttons.Add(InlineKeyboardButton.WithCallbackData(text: $"ID:{good.Id} Name:{good.Name}", callbackData: "goodToChange" + good.Id));
        }
        InlineKeyboardMarkup goodsKeyboard = new InlineKeyboardMarkup(SetOneColumnMenu(buttons).ToArray());
        await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Выберите товар для изменения:", replyMarkup: goodsKeyboard);
    }
    else
    {
        await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Список товаров пуст!");
    }
}

// Запрашиваем наименование товара для удаления
async Task RequestGoodToDelete(ITelegramBotClient botClient, CallbackQuery callbackQuery)
{
    await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Введите наименование товара для удаления:");
    State = 12;
}

// Запрашиваем данные для нового товара
async Task NewGoodRequest(ITelegramBotClient botClient, CallbackQuery callbackQuery)
{
    await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Введите наименование категории для размещения нового товара:");
    State = 6;
}

// Запрашиваем новое наименование для категории
async Task RenameCategoryRequest(ITelegramBotClient botClient, CallbackQuery callbackQuery)
{
    await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Введите наименование категории для изменения:");
    State = 4;
}

// Запрашиваем наименование категории для удаления
async Task RequestCategoryName(ITelegramBotClient botClient, CallbackQuery callbackQuery)
{
    await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Введите наименование категории для удаления:");
    State = 3;
}

// Запрашиваем наименование новой категории
async Task RequestNewCategoryName(ITelegramBotClient botClient, CallbackQuery callbackQuery)
{
    await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Введите наименование новой категории:");
    State = 2;
}

// Выводим список заказов пользователя
async Task DisplayMyOrders(ITelegramBotClient botClient, CallbackQuery callbackQuery)
{
    var id = callbackQuery.Message.Chat.Id;
    var userOrders = settings.GetOrders(id);

    foreach (var order in userOrders)
    {
        await botClient.SendTextMessageAsync(
        chatId: callbackQuery.Message.Chat.Id,
        text: $"<b>Order id: {order.Id}</b> Total Sum: <b>{order.TotalSum} rub</b> Created: {order.CreatedDate}",
        parseMode: ParseMode.Html,
        disableNotification: true);
    }

    List<Menu> orderMenu = JsonSerializer.Deserialize<List<Menu>>(settings.LoadOrderMenu());
    List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();

    foreach (var button in orderMenu)
    {
        buttons.Add(InlineKeyboardButton.WithCallbackData(text: button.Name, callbackData: button.Callback));
    }

    InlineKeyboardMarkup orderKeyboard = new InlineKeyboardMarkup(SetTwoColumnsMenu(buttons).ToArray());
    await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Выберите пункт меню:", replyMarkup: orderKeyboard);
}

// Формируем заказ и выводим список его товаров
async Task Checkout(ITelegramBotClient botClient, CallbackQuery callbackQuery)
{
    var id = callbackQuery.Message.Chat.Id;
    var currentOrder = settings.AddOrder(id);
    if (currentOrder != null)
    {
        var orderItems = settings.GetOrderView(currentOrder.Id);
        await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Ваш заказ:");
        await botClient.SendTextMessageAsync(
            chatId: callbackQuery.Message.Chat.Id,
            text: $"<b>Order id: {currentOrder.Id}</b>{Environment.NewLine}Total Sum: <b>{currentOrder.TotalSum} rub</b>{Environment.NewLine}Created: {currentOrder.CreatedDate}",
            parseMode: ParseMode.Html,
            disableNotification: true);
        await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Список товаров заказа:");

        foreach (var item in orderItems)
        {
            await botClient.SendTextMessageAsync(
            chatId: callbackQuery.Message.Chat.Id,
            text: $"ID-{item.GoodId}{Environment.NewLine}Name: <b>{item.GoodName}</b>{Environment.NewLine}Price: {item.GoodPrice} rub{Environment.NewLine}Quantity: {item.Quantity}",
            parseMode: ParseMode.Html,
            disableNotification: true);
        }
    }
    else
    {
        await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "В корзине нет товаров, доступных к заказу!");
    }
     
    List<Menu> orderMenu = JsonSerializer.Deserialize<List<Menu>>(settings.LoadOrderMenu());
    List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();

    foreach (var button in orderMenu)
    {
        buttons.Add(InlineKeyboardButton.WithCallbackData(text: button.Name, callbackData: button.Callback));
    }

    InlineKeyboardMarkup orderKeyboard = new InlineKeyboardMarkup(SetTwoColumnsMenu(buttons).ToArray());
    await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Выберите пункт меню:", replyMarkup: orderKeyboard);
}

// Уменьшаем количество выбранного товара 
async Task ReduceQuantity(ITelegramBotClient botClient, CallbackQuery callbackQuery)
{
    var id = callbackQuery.Message.Chat.Id;
    settings.ChangeCartGoodQuantity(id, _goodIdDict[id], -1);
    await DisplayCart(botClient, callbackQuery);
}

// Увеличиваем количество выбранного товара 
async Task IncreaseQuantity(ITelegramBotClient botClient, CallbackQuery callbackQuery)
{
    var id = callbackQuery.Message.Chat.Id;
    settings.ChangeCartGoodQuantity(id, _goodIdDict[id], 1);
    await DisplayCart(botClient, callbackQuery);
}

// Удаляем выбранный товар из корзины и файла
async Task DeleteGoodFromCart(ITelegramBotClient botClient, CallbackQuery callbackQuery)
{
    var id = callbackQuery.Message.Chat.Id;
    settings.RemoveGoodFromCart(id, _goodIdDict[id]);
    await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Товар удален из корзины...");
    await DisplayCart(botClient, callbackQuery);
}

// Выводим кнопки для изменения количества
async Task DisplayChangeQuantityButtons(ITelegramBotClient botClient, CallbackQuery callbackQuery)
{
    List<Menu> changeQuantityMenu = JsonSerializer.Deserialize<List<Menu>>(settings.LoadChangeQuantityMenu());
    List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();

    foreach (var button in changeQuantityMenu)
    {
        buttons.Add(InlineKeyboardButton.WithCallbackData(text: button.Name, callbackData: button.Callback));
    }

    var id = callbackQuery.Message.Chat.Id;
    var selectedGood = settings.GetCart(id).Single(x => x.GoodId == _goodIdDict[id]);
    InlineKeyboardMarkup changeQuantityKeyboard = new InlineKeyboardMarkup(SetTwoColumnsMenu(buttons).ToArray());
    await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Измените количество", replyMarkup: changeQuantityKeyboard);
    await botClient.SendTextMessageAsync(
        chatId: callbackQuery.Message.Chat.Id,
        text: $"--{selectedGood.GoodId}-- <b>{selectedGood.GoodName}</b> - {selectedGood.Quantity} шт.",
        parseMode: ParseMode.Html,
        disableNotification: true);   
}

// Выводим сообщение "Введите ID товара" и переводим state-машину в следующее состояние
async Task ChangeCart(ITelegramBotClient botClient, CallbackQuery callbackQuery)
{
    await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Введите ID товара:");
    State = 19;
}

// Выводим корзину текущиего пользователя
async Task DisplayCart(ITelegramBotClient botClient, CallbackQuery callbackQuery)
{
    List<Menu> cartMenu = JsonSerializer.Deserialize<List<Menu>>(settings.LoadCartMenu());
    List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();

    var id = callbackQuery.Message.Chat.Id;
    var userCart = settings.GetCart(id);

    await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "<<<Ваша корзина>>>");
    decimal totalPrice = 0;
    foreach (var cart in userCart)
    {
        await botClient.SendTextMessageAsync(
        chatId: callbackQuery.Message.Chat.Id,
        text: $"--{cart.GoodId}-- <b>{cart.GoodName}</b> - {cart.GoodPrice} руб. - {cart.Quantity} шт. - <b>{cart.GoodPrice * cart.Quantity} руб.</b>",
        parseMode: ParseMode.Html,
        disableNotification: true);
        totalPrice += cart.GoodPrice * cart.Quantity;
    }

    foreach(var button in cartMenu)
    {
        buttons.Add(InlineKeyboardButton.WithCallbackData(text: button.Name, callbackData: button.Callback));
    }

    await botClient.SendTextMessageAsync(
    chatId: callbackQuery.Message.Chat.Id,
    text: $"<b>Общая сумма: </b> - <u>{totalPrice} руб.</u>",
    parseMode: ParseMode.Html,
    disableNotification: true,
    replyMarkup: new InlineKeyboardMarkup(SetTwoColumnsMenu(buttons).ToArray()));
}

// Добавляем выбранный товар в корзину текущего пользователя
async Task AddToCart(ITelegramBotClient botClient, CallbackQuery callbackQuery)
{
    var id = callbackQuery.Message.Chat.Id;
    int goodId = Int32.Parse(callbackQuery.Data.Replace("addToCart",""));
    settings.AddToCart(id, goodId, 1);
    await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Товар добавлен в корзину");
}

// Выводим карточку товара
async Task DisplayGoodsItem(ITelegramBotClient botClient, CallbackQuery callbackQuery)
{
    var id = Int32.Parse(callbackQuery.Data.Replace("goods", ""));
    var item = settings.GetGood(id);
    List<Menu> goodsItemMenu = JsonSerializer.Deserialize<List<Menu>>(settings.LoadGoodsItemMenu());
    List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();

    foreach (var button in goodsItemMenu)
    {
        if (button.Callback == "addToCart")
        {
            buttons.Add(InlineKeyboardButton.WithCallbackData(text: button.Name, callbackData: button.Callback + item.Id));
        }
        else
        {
            buttons.Add(InlineKeyboardButton.WithCallbackData(text: button.Name, callbackData: button.Callback + item.CategoryId));
        }        
    }

    if (item.Url != "")
    {
        await botClient.SendPhotoAsync(
        chatId: callbackQuery.Message.Chat.Id,
        photo: item.Url,
        parseMode: ParseMode.Html);
    }

    await botClient.SendTextMessageAsync(
        chatId: callbackQuery.Message.Chat.Id,
        text: $"<b>{item.Name}</b>{Environment.NewLine}<u>Описание товара:</u> {item.Description}{Environment.NewLine}<u>Цена:</u> <b>{item.Price} руб.</b>{Environment.NewLine}<u>Доступное количество:</u> {item.Quantity}",
        parseMode: ParseMode.Html,
        disableNotification: true,
        replyMarkup: new InlineKeyboardMarkup(SetOneColumnMenu(buttons).ToArray()));
}

// Выводим список товаров выбранной категории
async Task DisplayGoods(ITelegramBotClient botClient, CallbackQuery callbackQuery)
{
    var id = Int32.Parse(callbackQuery.Data.Replace("categories", ""));
    List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();
    var goods = settings.GetGoods(id);
    foreach (var item in goods)
    {
        buttons.Add(InlineKeyboardButton.WithCallbackData(text: item.Name, callbackData: "goods" + item.Id));
    }

    InlineKeyboardMarkup mainMenuKeyboard = new InlineKeyboardMarkup(SetOneColumnMenu(buttons).ToArray());

    await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Выберите товар:", replyMarkup: mainMenuKeyboard);
}

// Выводим категории товаров
async Task DisplayCatalog(ITelegramBotClient botClient, CallbackQuery callbackQuery)
{
    List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();
    var categories = settings.GetCatalog();

    if (categories != null)
    {
        foreach (var category in categories)
        {
            buttons.Add(InlineKeyboardButton.WithCallbackData(text: category.Name, callbackData: "categories" + category.Id));
        }
        InlineKeyboardMarkup categoriesKeyboard = new InlineKeyboardMarkup(SetTwoColumnsMenu(buttons).ToArray());
        await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Выберите категорию:", replyMarkup: categoriesKeyboard);
    }
    else
    {
        await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Список категорий пуст!");
    }
}

// Обрабатываем запросы типа Message
async Task OnMessageProcessing(ITelegramBotClient botClient, Message message)
{
    Method method = methods.FirstOrDefault(x => x.State == State);
    if (method != null)
    {
        method.DelegateMethod(botClient, message);
    }
}

// Запрашиваем роль нового пользователя
async Task RequestNewUserRole(ITelegramBotClient botClient, Message message)
{
    if (Int32.TryParse(message.Text, out int userId))
    {
        var id = message.Chat.Id;
        _userIdDict[id] = userId;
        await DisplayUserRoles(botClient, message);
    }
    else
    {
        await botClient.SendTextMessageAsync((message.Chat.Id), "Введите корректный id пользователя");
    }        
}

// Меняем ссылку на изображение товара
async Task SetNewGoodUrl(ITelegramBotClient botClient, Message message)
{
    if (CheckUrl(message.Text))
    {
        var id = message.Chat.Id;
        settings.ChangeGoodUrl(_goodIdDict[id], message.Text);
        await botClient.SendTextMessageAsync((message.Chat.Id), "Ссылка на изображение товара изменена");
    }
    else
    {
        await botClient.SendTextMessageAsync((message.Chat.Id), "Введите корректную ссылку на изображение товара");
    }

    await LoadMainMenu(botClient, message);
}

// Меняем количество доступного товара
async Task SetNewGoodQuantity(ITelegramBotClient botClient, Message message)
{
    if (Int32.TryParse(message.Text, out int quantity))
    {
        var id = message.Chat.Id;
        settings.ChangeGoodQuantity(_goodIdDict[id], quantity);
        await botClient.SendTextMessageAsync(message.Chat.Id, "Количество доступного товара изменено");

        await LoadMainMenu(botClient, message);
    }
    else
    {
        await botClient.SendTextMessageAsync(message.Chat.Id, "Введите корректное количество доступного товара");
    }
}

// Меняем стоимость товара
async Task SetNewGoodPrice(ITelegramBotClient botClient, Message message)
{
    if (Decimal.TryParse(message.Text, out decimal price))
    {
        var id = message.Chat.Id;
        settings.ChangeGoodPrice(_goodIdDict[id], price);
        await botClient.SendTextMessageAsync(message.Chat.Id, "Стоимость товара изменен");

        await LoadMainMenu(botClient, message);
    }
    else
    {
        await botClient.SendTextMessageAsync(message.Chat.Id, "Введите корректную стоимость товара");
    }    
}

// Меняем описание товара
async Task SetNewGoodDescription(ITelegramBotClient botClient, Message message)
{
    var id = message.Chat.Id;
    settings.ChangeGoodDescription(_goodIdDict[id], message.Text);
    await botClient.SendTextMessageAsync(message.Chat.Id, "Описание товара изменено");

    await LoadMainMenu(botClient, message);
}

// Меняем наименование товара
async Task SetNewGoodName(ITelegramBotClient botClient, Message message)
{
    if (!String.IsNullOrEmpty(message.Text))
    {
        var id = message.Chat.Id;
        settings.ChangeGoodName(_goodIdDict[id], message.Text);
        await botClient.SendTextMessageAsync(message.Chat.Id, "Наименование товара изменено");
    }
    else
    {
        await botClient.SendTextMessageAsync(message.Chat.Id, "Наименование товара не может быть пустым!");
    }

    await LoadMainMenu(botClient, message);
}

// Удаляем выбранный товар
async Task DeleteGood(ITelegramBotClient botClient, Message message)
{
    if (settings.DeleteGood(message.Text))
    {
        await botClient.SendTextMessageAsync(message.Chat.Id, "Товар удален");
    }
    else
    {
        await botClient.SendTextMessageAsync(message.Chat.Id, "Товар не найден!");
    }

    // Загрузка кнопок главного меню
    await LoadMainMenu(botClient, message);
}

// Получаем ссылку на изображение нового товара
async Task GetGoodUrl(ITelegramBotClient botClient, Message message)
{
    if (CheckUrl(message.Text))
    {
        var id = message.Chat.Id;
        Good good = _goodsDict[id];
        var categoryId = good.CategoryId;
        var name = good.Name;
        var description = good.Description;
        var price = good.Price;
        var quantity = good.Quantity;
        var url = message.Text;
        _goodsDict.Remove(id);
        good.CategoryId = categoryId;
        good.Name = name;
        good.Description = description;
        good.Price = price;
        good.Quantity = quantity;
        good.Url = url;
        _goodsDict[id] = good;
        settings.AddGood(categoryId, name, description, price, quantity, url);
        _goodsDict.Remove(id);

        await botClient.SendTextMessageAsync(message.Chat.Id, "Новый товар успешно добавлен");
        State = 0;

        // Загрузка кнопок главного меню
        await LoadMainMenu(botClient, message);
    }
    else
    {
        await botClient.SendTextMessageAsync(message.Chat.Id, "Введите корректную ссылку на изображение:");
    }    
}

// Проверяем корректность введенной ссылки на изображение
bool CheckUrl(string uriName)
{
    Uri uriResult;
    bool result = Uri.TryCreate(uriName, UriKind.Absolute, out uriResult)
        && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    return result;
}

// Получаем количество нового товара
async Task GetGoodQuantity(ITelegramBotClient botClient, Message message)
{
    bool flag = Int32.TryParse(message.Text, out int quantity);
    if (flag && quantity > 0)
    {
        var id = message.Chat.Id;
        Good good = _goodsDict[id];
        var categoryId = good.CategoryId;
        var name = good.Name;
        var description = good.Description;
        var price = good.Price;
        _goodsDict.Remove(id);
        good.CategoryId = categoryId;
        good.Name = name;
        good.Description = description;
        good.Price = price;
        good.Quantity = quantity;
        _goodsDict[id] = good;
        await botClient.SendTextMessageAsync(message.Chat.Id, "Введите ссылку на изображение:");
        State = 11;
    }
    else
    {
        await botClient.SendTextMessageAsync(message.Chat.Id, "Введите корректное количество товара:");
    }
}

// Получаем стоимость нового товара
async Task GetGoodPrice(ITelegramBotClient botClient, Message message)
{
    bool flag = Decimal.TryParse(message.Text, out decimal price);
    if (flag && price > 0)
    {
        var id = message.Chat.Id;
        Good good = _goodsDict[id];
        var categoryId = good.CategoryId;
        var name = good.Name;
        var description = good.Description;
        _goodsDict.Remove(id);
        good.CategoryId = categoryId;
        good.Name = name;
        good.Description = description;
        good.Price = price;
        _goodsDict[id] = good;
        await botClient.SendTextMessageAsync(message.Chat.Id, "Введите количество товара:");
        State = 10;
    }
    else
    {
        await botClient.SendTextMessageAsync(message.Chat.Id, "Введите корректную стоимость товара:");
    }
}

// Получаем описание нового товара
async Task GetGoodDescription(ITelegramBotClient botClient, Message message)
{
    var id = message.Chat.Id;
    Good good = _goodsDict[id];
    var categoryId = good.CategoryId;
    var name = good.Name;
    _goodsDict.Remove(id);
    good.CategoryId = categoryId;
    good.Name = name;
    good.Description = message.Text;
    _goodsDict[id] = good;
    await botClient.SendTextMessageAsync(message.Chat.Id, "Введите стоимость товара:");
    State = 9;
}

// Получаем наименование нового товара
async Task GetGoodName(ITelegramBotClient botClient, Message message)
{
    if (!String.IsNullOrEmpty(message.Text))
    {
        var id = message.Chat.Id;
        Good good = _goodsDict[id];
        var categoryId = good.CategoryId;
        _goodsDict.Remove(id);
        good.CategoryId = categoryId;
        good.Name = message.Text;
        _goodsDict[id] = good;
        await botClient.SendTextMessageAsync(message.Chat.Id, "Введите описание товара:");
        State = 8;
    }
    else
    {
        await botClient.SendTextMessageAsync(message.Chat.Id, "Введите корректное наименование товара:");
    }
}

// Проверка катогории для добавления товара
async Task CheckCategory(ITelegramBotClient botClient, Message message)
{
    if (settings.CategoryExists(message.Text))
    {
        var id = message.Chat.Id;
        await botClient.SendTextMessageAsync(message.Chat.Id, "Введите наименование товара:");
        Good good = new Good();
        var categoryId = settings.GetCategoryId(message.Text);
        good.CategoryId = categoryId;
        _goodsDict[id] = good;
        State = 7;
    }
    else
    {
        await botClient.SendTextMessageAsync(message.Chat.Id, "Категория не найдена");
    }
}

// Проверка существования категории
async Task CategoryExists(ITelegramBotClient botClient, Message message)
{
    if (settings.CategoryExists(message.Text))
    {
        var id = message.Chat.Id;
        await botClient.SendTextMessageAsync(message.Chat.Id, "Введите новое имя категории:");
        _categoryNameDict[id] = message.Text;
        State = 5;
    }
    else
    {
        await botClient.SendTextMessageAsync(message.Chat.Id, "Категория не найдена");
    }
}

// Переименовываем выбранную категорию
async Task RenameCategory(ITelegramBotClient botClient, Message message)
{
    var id = message.Chat.Id;
    settings.RenameCategory(_categoryNameDict[id], message.Text);
    _categoryNameDict.Remove(id);

    await botClient.SendTextMessageAsync(message.Chat.Id, "Категория изменена");

    // Загрузка кнопок главного меню
    await LoadMainMenu(botClient, message);
}

// Удаляем выбранную категорию
async Task DeleteCategory(ITelegramBotClient botClient, Message message)
{
    if (settings.DeleteCategory(message.Text))
    {
        await botClient.SendTextMessageAsync(message.Chat.Id, "Категория удалена");
    }
    else
    {
        await botClient.SendTextMessageAsync(message.Chat.Id, "Категория не найдена");
    }

    // Загрузка кнопок главного меню
    await LoadMainMenu(botClient, message);
}

// Добавляем новую категорию, если не существует
async Task AddNewCategory(ITelegramBotClient botClient, Message message)
{
    settings.AddNewCategory(message.Text);

    await botClient.SendTextMessageAsync(message.Chat.Id, "Категория добавлена");

    // Загрузка кнопок главного меню
    await LoadMainMenu(botClient, message);
}

// Выводим меню изменения количества выбранного товара
async Task DisplayCartChangeMenu(ITelegramBotClient botClient, Message message)
{
    List<Menu> cartChangeMenu = JsonSerializer.Deserialize<List<Menu>>(settings.LoadCartChangeMenu());
    List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();
    
    foreach (var button in cartChangeMenu)
    {
        buttons.Add(InlineKeyboardButton.WithCallbackData(text: button.Name, callbackData: button.Callback));
    }

    InlineKeyboardMarkup cartChangeMenuKeyboard = new InlineKeyboardMarkup(SetTwoColumnsMenu(buttons).ToArray());

    await botClient.SendTextMessageAsync(message.Chat.Id, "Выберите команду:", replyMarkup: cartChangeMenuKeyboard);

}

// Проверяем наличие товара по id для изменения корзины
async Task SetGood(ITelegramBotClient botClient, Message message)
{
    int usetInput; 
    if(Int32.TryParse(message.Text, out usetInput))
    {
        var id = message.Chat.Id;
        if (settings.GetCart(id).Exists(x => x.GoodId == usetInput))
        {
            if (_goodIdDict.ContainsKey(id))
            {
                _goodIdDict.Remove(id);
            }
            _goodIdDict[id] = usetInput;
            State = 0;
            await DisplayCartChangeMenu(botClient, message);
        }
        else
        {
            await botClient.SendTextMessageAsync(message.Chat.Id, "Введите корректный id товара");
        }
    }
    else
    {
        await botClient.SendTextMessageAsync(message.Chat.Id, "Введите корректный id товара");
    }
}

// Выводим кнопки главного меню
async Task LoadMainMenu(ITelegramBotClient botClient, Message message)
{
    var id = message.Chat.Id;
    List<Menu> mainMenu = JsonSerializer.Deserialize<List<Menu>>(settings.LoadMainMenu(id));
    List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();

    foreach (var button in mainMenu)
    {
        buttons.Add(InlineKeyboardButton.WithCallbackData(text: button.Name, callbackData: button.Callback));
    }

    InlineKeyboardMarkup mainMenuKeyboard = new InlineKeyboardMarkup(SetTwoColumnsMenu(buttons).ToArray());

    await botClient.SendTextMessageAsync(message.Chat.Id, "Выберите команду:", replyMarkup: mainMenuKeyboard);
}

// Выводим кнопки в две колонки
List<InlineKeyboardButton[]> SetTwoColumnsMenu(List<InlineKeyboardButton> buttons)
{
    List<InlineKeyboardButton[]> twoColumnsMenu = new List<InlineKeyboardButton[]>();
    for (var i = 0; i < buttons.Count; i++)
    {
        if (buttons.Count - 1 == i)
        {
            twoColumnsMenu.Add(new[] { buttons[i] });
        }
        else
            twoColumnsMenu.Add(new[] { buttons[i], buttons[i + 1] });
        i++;
    }
    return twoColumnsMenu;
}

// Выводим кнопки в одну колонку
List<InlineKeyboardButton[]> SetOneColumnMenu(List<InlineKeyboardButton> buttons)
{
    List<InlineKeyboardButton[]> oneColumnMenu = new List<InlineKeyboardButton[]>();
    for (var i = 0; i < buttons.Count; i++)
    {
        oneColumnMenu.Add(new[] { buttons[i] });
    }
    return oneColumnMenu;
}

// Выводим сообщение о непонятной команде, когда пользователь пишет сообщение и State=0, далее выводим главное меню
async Task MisundestatingAnswer(ITelegramBotClient botClient, Message message)
{
    await botClient.SendTextMessageAsync(message.Chat.Id, text: "Я не понимаю такой команды...");

    await LoadMainMenu(botClient, message);
}

// Обработка ошибок
Task HandleErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException
            => $"Ошибка телеграм API:\n{apiRequestException.ErrorCode}\n{apiRequestException.Message}",
        _ => exception.ToString()
    };
    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}