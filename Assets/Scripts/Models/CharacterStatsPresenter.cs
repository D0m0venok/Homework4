using UniRx;
using Unity.VisualScripting;

namespace Homework4
{
    public sealed class CharacterStatsPresenter : ICharacterStatsPresenter
    {
        private readonly CompositeDisposable _disposable = new ();
        private readonly ReactiveCollection<ICharacterStatPM> _stats = new();

        public IReadOnlyReactiveCollection<ICharacterStatPM> Stats => _stats;

        public CharacterStatsPresenter(CharacterInfo characterInfo)
        {
            _stats.AddRange(characterInfo.Stats);
            characterInfo.Stats.ObserveAdd().Subscribe(action => OnCharacterStatAdded(action.Value)).AddTo(_disposable);
            characterInfo.Stats.ObserveRemove().Subscribe(action => OnCharacterStatRemoved(action.Value)).AddTo(_disposable);
        }
        public void Dispose()
        {
            _disposable.Dispose();
        }
        
        private void OnCharacterStatAdded(ICharacterStatPM stat)
        {
            _stats.Add(stat);
        }
        private void OnCharacterStatRemoved(ICharacterStatPM stat)
        {
            _stats.Remove(stat);
        }
    }

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