using System;
using System.Linq;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Homework4
{
    public sealed class UserPopupShower : MonoBehaviour
    {
        [SerializeField] private Sprite[] _sprites;
        
        private UserPopup _userPopup;
        private IUserPresenter _userPresenter;
        private UserPresenterFactory _factory;
        private UserInfo _userInfo;
        private PlayerLevel _playerLevel;
        private CharacterInfo _characterInfo;

        [Inject]
        private void Construct(UserPresenterFactory factory, UserPopup userPopup)
        {
            _factory = factory;
            _userPopup = userPopup;
        }
        
        private void Start()
        {
            Show();
        }
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
                Show();
        }

        public void Show()
        {
            if(_userPopup.gameObject.activeSelf)
                return;
            
            _userInfo = new UserInfo();
            _playerLevel = new PlayerLevel();
            _characterInfo = new CharacterInfo();
            
            ChangeName();
            ChangeDescription();
            ChangeIcon();
            
            AddExperience();
            
            for (var i = 0; i < 4; i++)
            {
                AddStat();
            }
            
            _userPresenter = _factory.Create(_userInfo, _playerLevel, _characterInfo);
            _userPopup.Show(_userPresenter);
        }
        public void ChangeName()
        {
            _userInfo.ChangeName(GetRandomWord());
        }
        public void ChangeDescription()
        {
            var str = string.Join(" ", Enumerable.Range(0, Random.Range(4, 9)).Select(s => GetRandomWord()));
            _userInfo.ChangeDescription(str);
        }
        public void ChangeIcon()
        {
            _userInfo.ChangeIcon(GetRandomSprite());
        }
        public void AddExperience()
        {
            _playerLevel.AddExperience(GetRandomInt(0, _playerLevel.RequiredExperience));
        }
        public void AddStat()
        {
            var stat = new CharacterStat(GetRandomWord());
            stat.ChangeValue(GetRandomInt(5, 31));
            _characterInfo.AddStat(stat);
        }
        public void RemoveStat()
        {
            var stats = _characterInfo.GetStats();
            if(stats.Length == 0)
                return;
            
            var stat = stats[GetRandomInt(0, stats.Length)];
            _characterInfo.RemoveStat(stat);
        }
        public void ChangeRandomStat()
        {
            var stats = _characterInfo.GetStats();
            if(stats.Length == 0)
                return;
            
            var stat = stats[GetRandomInt(0, stats.Length)];
            stat.ChangeValue(stat.Value + GetRandomInt(1, 5));
        }

        private Sprite GetRandomSprite()
        {
            return _sprites[GetRandomInt(0, _sprites.Length)];
        }

        private string GetRandomWord()
        {
            return Enumerable.Range('a', 26).Select(c => (char)c).OrderBy(c => Random.value).
                Take(GetRandomInt(5, 10)).Aggregate(string.Empty, (str, c) => str + c);
        }
        private int GetRandomInt(int start, int last)
        {
            return Random.Range(start, last);
        }
    }
}