using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using LoanlyBot.BotService;
using LoanlyBot.Commands;
using LoanlyBot.Services;
using LoanlyBot.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LoanlyBot
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var token = config.GetSection("ApiKeys").GetValue<string>("TelegramBotToken");
            var connectionString = config.GetConnectionString("PostgreSQL");

            var logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .Filter.ByExcluding(logEvent =>
                    logEvent.Properties.ContainsKey("SourceContext") &&
                    logEvent.Properties["SourceContext"].ToString().Contains("System.Net.Http"))
                .CreateLogger();

            var builder = Host.CreateDefaultBuilder()
                .UseSerilog(logger)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddSerilog();
                    logging.SetMinimumLevel(LogLevel.Information);
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<Serilog.ILogger>(provider => logger);
                    services.AddSingleton<ITelegramBotClient>(sp => new TelegramBotClient(token!));
                    services.AddSingleton<BotService.BotService>();
                    services.AddSingleton<UpdateHandler>();
                    services.AddSingleton<UserStateMachine>();
                    services.AddSingleton<CommandHandlerFactory>();
                    services.AddSingleton<ICommandHandler, StartCommand>();
                    services.AddSingleton<ICommandHandler, AddLoanCommand>();
                    services.AddSingleton<ICommandHandler, AddPaymentCommand>();
                    services.AddSingleton<ICommandHandler, CheckLoansCommand>();
                    services.AddSingleton<ICommandHandler, CheckPaymentHistoryCommand>();
                    services.AddDbContext<PostgresContext>(options =>
                        options.UseNpgsql(connectionString)
                    );
                    services.AddSingleton<DBService>();
                    services.AddHostedService<BotService.BotService>();
                })
                .Build();

            await builder.RunAsync();
        }
    }
}
