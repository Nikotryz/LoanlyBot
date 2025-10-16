namespace LoanlyBot.Commands
{
    public static class CommandConstants
    {
        // general commands
        public const string START = "/start";
        public const string HELP = "/help";
        public const string ADMIN = "/admin";

        // main commands
        public const string ADD_LOAN = "Добавить долг";
        public const string ADD_PAYMENT = "Добавить платеж";
        public const string CHECK_LOANS = "Просмотреть долги";
        public const string PAYMENT_HISTORY = "История платежей";

        // admin commands
        public const string ADD_PRODUCT_COMMAND = "Добавить товар в каталог";
        public const string DELETE_PRODUCT_COMMAND = "Удалить товар из каталога";
        public const string ADD_INGRIDIENTS_TO_WAREHOUSE_COMMAND = "Добавить провизию на склад";
        public const string CHECK_INGREDIENTS_COMMAND = "Ингредиенты на складе";
        public const string CHANGE_BASE_DELIVERY_COST = "Изменить базовую стоимость доставки";
        public const string CHANGE_DELIVERY_COST_PER_KILOMETER = "Изменить стоимость километра доставки";

        // admin & courier commands
        public const string CONFIRM_ORDER_COMMAND = "Принять заказ";
        public const string CANCEL_ORDER_COMMAND = "Отменить заказ";
        public const string ORDER_IS_READY_COMMAND = "Заказ готов";

        // courier commands
        public const string CHECK_ACTIVE_ORDER_COMMAND = "Просмотреть активный заказ";
        public const string CHECK_COURIER_ORDER_HISTORY_COMMAND = "История выполненных заказов";
        public const string ORDER_IS_DELIVERED_COMMAND = "Заказ доставлен";

        // warning about unpermissioned command
        public const string UNPERMISSIONED_COMMAND_WARNING = "У вас нет прав на выполнение этой команды";
    }
}
