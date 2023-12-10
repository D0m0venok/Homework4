namespace Homework4
{
    public interface IUserPresenter
    {
        public ICharacterInfoPresenter InfoPresenter { get; }
        public ICharacterStatsPresenter StatsPresenter { get; }

        public void Dispose();
    }
}