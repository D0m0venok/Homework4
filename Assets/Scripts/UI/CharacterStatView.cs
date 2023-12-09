using System;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Homework4
{
    public sealed class CharacterStatView : MonoBehaviour
    {
        [SerializeField] private Text _nameText;
        [SerializeField] private Text _valueText;

        private readonly float _duration = 0.2f;
        private IDisposable _disposable;

        public void Init(ICharacterStatPM stat)
        {
            _nameText.text = stat.Name + ": ";
            _valueText.text = stat.Value.Value.ToString();
            
            _disposable = stat.Value.SkipLatestValueOnSubscribe().
                Subscribe(i => UpdateValue(i.ToString()));
        }
        public void Dispose()
        {
            _disposable.Dispose();
        }
        
        private void UpdateValue(string value)
        {
            _valueText.transform.DOScale(0f, _duration * 0.5f).SetLoops(2, LoopType.Yoyo).
                OnStepComplete(() => _valueText.text = value);
        }
    }
}
