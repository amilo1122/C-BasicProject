using BLL;
using BLL.Menus;
using Shared.Models;
using System.Text.Json;

namespace UI1
{
    public class ConsoleUI
    {
        static void Main(string[] args)
        {
            Settings settings = new Settings();
                        
            Console.Write("Приветствую! Введите свой id: ");
            long userId = Int64.Parse(Console.ReadLine());

            List<Menu> mainMenu = JsonSerializer.Deserialize<List<Menu>>(settings.LoadMainMenu(userId));

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
