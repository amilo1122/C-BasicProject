using BLL;
using BLL.Menus;
using Shared.Models;
using System.Text.Json;

namespace UI1
{
    public class ConsoleUI
    {
        //private static MainMenu _menus;

        //public Program()
        //{
        //    _menus = new MainMenu();
        //}

        static void Main(string[] args)
        {
            Settings settings = new Settings();
                        
            Console.Write("Приветствую! Введите свой id: ");
            int userId = Int32.Parse(Console.ReadLine());

            List<Menu> mainMenu = JsonSerializer.Deserialize<List<Menu>>(settings.LoadMenu(userId));

            if (mainMenu != null)
            {
                foreach (var button in mainMenu)
                {
                    Console.WriteLine(button.Name);
                }
            }
            

            Console.ReadKey();
        }
    }
}
