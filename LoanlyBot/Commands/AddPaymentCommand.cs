using LoanlyBot.BotService;
using LoanlyBot.Models;
using LoanlyBot.Services;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace LoanlyBot.Commands
{
    internal class AddPaymentCommand : ICommandHandler
    {
        private readonly ITelegramBotClient _botClient;
        private readonly UserStateMachine _stateMachine;
        private readonly DBService _dBService;
        private readonly ILogger _logger;

        public AddPaymentCommand(ITelegramBotClient botClient, UserStateMachine stateMachine, DBService dbService, ILogger logger)
        {
            _botClient = botClient;
            _stateMachine = stateMachine;
            _dBService = dbService;
            _logger = logger;
        }

        public bool CanHandle(Message message) => message.Text == CommandConstants.ADD_PAYMENT || _stateMachine.GetState(message.Chat.Id) == UserState.EnteringPayment;

        public async Task HandleAsync(Message message)
        {
            var state = _stateMachine.GetState(message.Chat.Id);
            if (state != UserState.EnteringPayment)
            {
                await _botClient.SendMessage(chatId: message.Chat.Id, text: $"Для добавления платежа введите имя должника, сумму и описание через пробел");
                _stateMachine.UpdateState(message.Chat.Id, UserState.EnteringPayment);
            }
            else
            {
                var data = message.Text.Split(' ');
                var name = data[0];
                var amount = decimal.Parse(data[1]);
                var description = data[2];

                var loaners = await _dBService.GetAll<Loaner>();
                var loaner = loaners?.FirstOrDefault(x => x.Name == name);

                if (loaners.Count != 0 && loaner != null)
                {
                    loaner.HisLoan -= amount;
                    await _dBService.Update(loaner);
                    await _dBService.Create(new Payment
                    {
                        SenderId = loaner.UserId,
                        RecipientId = message.Chat.Id,
                        Date = DateTime.Now,
                        Amount = amount,
                        Description = description
                    });
                    await _botClient.SendMessage(chatId: message.Chat.Id, text: $"Платеж успешно добавлен");
                    _logger.Information("{0} added a new payment", message?.From?.Username);
                }
                else
                {
                    await _botClient.SendMessage(chatId: message.Chat.Id, text: $"Должник не найден");
                    _logger.Information("{0} tried to add a new payment", message?.From?.Username);
                }
                _stateMachine.DeleteState(message.Chat.Id);
            }
        }
    }
}
