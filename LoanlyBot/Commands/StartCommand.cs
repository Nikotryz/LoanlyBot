using LoanlyBot.BotService;
using LoanlyBot.Keyboards;
using LoanlyBot.Services;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace LoanlyBot.Commands
{
    public class StartCommand : ICommandHandler
    {
        private readonly ITelegramBotClient _botClient;
        private readonly UserStateMachine _stateMachine;
        private readonly DBService _dBService;
        private readonly ILogger _logger;

        public StartCommand(ITelegramBotClient botClient, UserStateMachine stateMachine, DBService dbService, ILogger logger)
        {
            _botClient = botClient;
            _stateMachine = stateMachine;
            _dBService = dbService;
            _logger = logger;
        }

        public bool CanHandle(Message message) => message.Text == CommandConstants.START && !_stateMachine.HasState(message.Chat.Id);

        public async Task HandleAsync(Message message)
        {
            var user = await _dBService.GetById<Models.Loaner>(message.Chat.Id);
            if (user == null)
            {
                await _botClient.SendMessage(chatId: message.Chat.Id, text: $"Рады видеть Вас, новый пользователь! Выберите команду из нижней панели или введите {CommandConstants.HELP} для получения полной информации.", replyMarkup: MainMenuKeyboard.Get());
                await _dBService.Create(new Models.Loaner
                {
                    UserId = message.Chat.Id,
                    Name = message.Chat.Username!
                });
                _logger.Information("New user added to data base: {0}", message?.From?.Username);
            }
            else
            {
                await _botClient.SendMessage(chatId: message.Chat.Id, text: $"Выберите команду из нижней панели или введите {CommandConstants.HELP} для получения полной информации.", replyMarkup: MainMenuKeyboard.Get());
                _logger.Information("{0} has entered the /start command", message?.From?.Username);
            }
        }
    }
}
