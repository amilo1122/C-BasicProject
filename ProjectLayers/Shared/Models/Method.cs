
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Shared.Models
{
    public delegate Task DelegateMethod(ITelegramBotClient botClient, Message message);
    public class Method
    {
        public int State { get; set; }
        public DelegateMethod DelegateMethod { get; set; }

        public Method(int state, DelegateMethod delegateMethod)
        {
            State = state;
            DelegateMethod = delegateMethod;
        }
    }
}
