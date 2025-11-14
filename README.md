# LoanlyBot

LoanlyBot — Telegram-бот, позволяющий вести учет своих долгов и долгов Вам.

## Функции

- Добавление долга (своего)
- Добавление долга (чужого)
- Добавление платежа (в счет погашения своего долга)
- Добавление платежа (в счет погашения чужого долга)
- Просмотр суммы долгов
- Просмотр истории платежей

<!--

## Начало работы (Docker)

- Склонировать репозиторий: `git clone https://github.com/Nikotryz/HallOfFame`
- В корневую директорию проекта добавить файл appsettings.json: [Пример содержимого файла](#пример-appsettings.json)
  - Укажите токен созданного вами бота;
  - Укажите строку подключения к вашей БД.
- Открыть консоль в директории скачанного репозитория;
- Ввести команду `docker compose build`. Начнется сборка приложения, дождитесь окончания;
- Ввести команду `docker compose up -d` для запуска приложения;
- Открыть в браузере [http://localhost:8080/swagger/index.html](url).

## Пример appsettings.json

`
{
  "ApiKeys": {<br/>
    "TelegramBotToken": "8226672812:AAHc3-SHasjbVS8toqiSVgFpBHWSqr4QFBI"
  },
  "ConnectionStrings": {
    "PostgreSQL": "Server=postgres_db;Port=5432;Database=loanlybot_db;Username=postgres;Password=123456"
  }
}
`

-->
