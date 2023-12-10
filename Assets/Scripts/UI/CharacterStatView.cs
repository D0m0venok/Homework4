using System;
using DG.Tweening;
using Extensions;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Homework4
{
    public sealed class CharacterStatView : MonoBehaviour
    {
        [SerializeField] private Text _nameText;
        [SerializeField] private Text _valueText;
        
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
            var tween = TL.Get(TLNames.ScaleZero);
            _valueText.transform.DOScale((Vector3)tween.To, tween.GetTime() * 0.5f).SetLoops(2, LoopType.Yoyo)
                .SetEase(tween.Ease).OnStepComplete(() => _valueText.text = value);
        }
    }
}
