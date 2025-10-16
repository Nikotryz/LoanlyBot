using LoanlyBot.BotService;
using LoanlyBot.Models;
using LoanlyBot.Services;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace LoanlyBot.Commands
{
    public class AddLoanCommand : ICommandHandler
    {
        private readonly ITelegramBotClient _botClient;
        private readonly UserStateMachine _stateMachine;
        private readonly DBService _dBService;
        private readonly ILogger _logger;

        public AddLoanCommand(ITelegramBotClient botClient, UserStateMachine stateMachine, DBService dbService, ILogger logger)
        {
            _botClient = botClient;
            _stateMachine = stateMachine;
            _dBService = dbService;
            _logger = logger;
        }

        public bool CanHandle(Message message) => message.Text == CommandConstants.ADD_LOAN || _stateMachine.GetState(message.Chat.Id) == UserState.EnteringLoan;

        public async Task HandleAsync(Message message)
        {
            var state = _stateMachine.GetState(message.Chat.Id);
            if (state != UserState.EnteringLoan)
            {
                await _botClient.SendMessage(chatId: message.Chat.Id, text: $"Для добавления долга введите имя должника, сумму и описание через пробел");
                _stateMachine.UpdateState(message.Chat.Id, UserState.EnteringLoan);
            }
            else
            {
                var data = message.Text.Split(' ');
                var name = data[0];
                var amount = decimal.Parse(data[1]);
                var description = data[2];

                var loaners = await _dBService.GetAll<Loaner>();
                var loaner = loaners?.FirstOrDefault(x => x.Name == name);

                if (loaners.Count == 0 || loaner == null)
                {
                    var newId = 1L;
                    newId = loaners.Count != 0 ? loaners.Max(x => x.UserId) + 1 : newId;
                    await _dBService.Create(new Loaner
                    {
                        UserId = newId,
                        Name = name,
                        HisLoan = amount
                    });
                    await _dBService.Create(new Payment
                    {
                        SenderId = message.Chat.Id,
                        RecipientId = newId,
                        Date = DateTime.Now,
                        Amount = -amount,
                        Description = description
                    });
                }
                else
                {
                    loaner.HisLoan += amount;
                    await _dBService.Update(loaner);
                    await _dBService.Create(new Payment
                    {
                        SenderId = message.Chat.Id,
                        RecipientId = loaner.UserId,
                        Date = DateTime.Now,
                        Amount = -amount,
                        Description = description
                    });
                }
                await _botClient.SendMessage(chatId: message.Chat.Id, text: $"Долг успешно добавлен");
                _stateMachine.DeleteState(message.Chat.Id);
                _logger.Information("{0} added a new loan", message?.From?.Username);
            }
        }
    }
}
