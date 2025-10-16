using LoanlyBot.BotService;
using LoanlyBot.Models;
using LoanlyBot.Services;
using Serilog;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace LoanlyBot.Commands
{
    public class CheckLoansCommand : ICommandHandler
    {
        private readonly ITelegramBotClient _botClient;
        private readonly UserStateMachine _stateMachine;
        private readonly DBService _dBService;
        private readonly ILogger _logger;

        public CheckLoansCommand(ITelegramBotClient botClient, UserStateMachine stateMachine, DBService dbService, ILogger logger)
        {
            _botClient = botClient;
            _stateMachine = stateMachine;
            _dBService = dbService;
            _logger = logger;
        }

        public bool CanHandle(Message message) => message.Text == CommandConstants.CHECK_LOANS && !_stateMachine.HasState(message.Chat.Id);

        public async Task HandleAsync(Message message)
        {
            var loaners = await _dBService.GetAll<Loaner>();
            if (loaners.Count == 0)
            {
                await _botClient.SendMessage(chatId: message.Chat.Id, text: "Долги отсутствуют");
            }
            else
            {
                var text = new StringBuilder();
                foreach (var loaner in loaners.Where(x => x.UserId != message.Chat.Id))
                {
                    text.Append($"{loaner.Name}: {loaner.HisLoan} \u20BD\n");
                }
                await _botClient.SendMessage(chatId: message.Chat.Id, text: text.ToString());
            }
            _logger.Information("{0} checked loans", message?.From?.Username);
        }
    }
}
