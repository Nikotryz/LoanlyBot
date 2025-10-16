using Telegram.Bot.Types;

namespace LoanlyBot.BotService
{
    public class CommandHandlerFactory
    {
        private readonly IEnumerable<ICommandHandler> _handlers;

        public CommandHandlerFactory(IEnumerable<ICommandHandler> handlers) => _handlers = handlers;

        public ICommandHandler? GetHandler(Message message) => _handlers.FirstOrDefault(x => x.CanHandle(message));
    }
}
