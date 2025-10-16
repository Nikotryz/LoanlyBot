using Telegram.Bot.Types;

namespace LoanlyBot.BotService
{
    public interface ICommandHandler
    {
        Task HandleAsync(Message message);
        bool CanHandle(Message message);
    }
}
