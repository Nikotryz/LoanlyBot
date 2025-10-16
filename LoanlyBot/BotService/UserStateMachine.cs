namespace LoanlyBot.BotService
{
    public class UserStateMachine
    {
        private readonly Dictionary<long, UserState> _userStates = new();

        public bool HasState(long userId) => _userStates.ContainsKey(userId);

        public UserState? GetState(long userId) => HasState(userId) ? _userStates[userId] : null;

        public void UpdateState(long userId, UserState state) => _userStates[userId] = state;

        public void DeleteState(long userId) => _userStates.Remove(userId);
    }

    public enum UserState
    {
        EnteringLoan,
        EnteringPayment
    }
}
