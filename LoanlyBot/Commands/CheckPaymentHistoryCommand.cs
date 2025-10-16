using LoanlyBot.BotService;
using LoanlyBot.Models;
using LoanlyBot.Services;
using Serilog;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace LoanlyBot.Commands
{
    public class CheckPaymentHistoryCommand : ICommandHandler
    {
        private readonly ITelegramBotClient _botClient;
        private readonly UserStateMachine _stateMachine;
        private readonly DBService _dBService;
        private readonly ILogger _logger;

        public CheckPaymentHistoryCommand(ITelegramBotClient botClient, UserStateMachine stateMachine, DBService dbService, ILogger logger)
        {
            _botClient = botClient;
            _stateMachine = stateMachine;
            _dBService = dbService;
            _logger = logger;
        }

        public bool CanHandle(Message message) => message.Text == CommandConstants.PAYMENT_HISTORY && !_stateMachine.HasState(message.Chat.Id);

        public async Task HandleAsync(Message message)
        {
            var allPayments = await _dBService.GetAll<Payment>();
            if (allPayments.Count == 0)
            {
                await _botClient.SendMessage(chatId: message.Chat.Id, text: "Платежи отсутствуют");
            }
            else
            {
                var needPayments = allPayments.Where(x => x.SenderId == message.Chat.Id || x.RecipientId == message.Chat.Id).GroupBy(x => x.Date.Date).OrderByDescending(x => x.Key);
                var text = new StringBuilder();
                foreach (var payments in needPayments)
                {
                    text.Append($"{payments.Key.AddHours(7).ToShortDateString()}:\n\n");
                    var data = payments.ToList().OrderByDescending(x => x.Date);
                    foreach (var payment in data)
                    {
                        text.Append($"{payment.Date.AddHours(7).ToShortTimeString()}   {payment.Amount}\u20BD   {payment.Description}\n");
                    }
                    text.Append('\n');
                }
                await _botClient.SendMessage(chatId: message.Chat.Id, text: text.ToString());
            }
            _logger.Information("{0} checked payments", message?.From?.Username);
        }
    }
}
