namespace Homework4
{
    public sealed class UserPresenter : IUserPresenter
    {
        public ICharacterInfoPresenter InfoPresenter { get; }
        public ICharacterStatsPresenter StatsPresenter { get; }

        public UserPresenter(ICharacterInfoPresenter infoPresenter, ICharacterStatsPresenter statsPresenter)
        {
            InfoPresenter = infoPresenter;
            StatsPresenter = statsPresenter;
        }
        public void Dispose()
        {
            InfoPresenter.Dispose();
            StatsPresenter.Dispose();
        }
    }
}