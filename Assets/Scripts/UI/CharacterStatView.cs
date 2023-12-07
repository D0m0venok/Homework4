using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Homework4
{
    public sealed class CharacterStatView : MonoBehaviour
    {
        [SerializeField] private Text _nameText;
        [SerializeField] private Text _valueText;

        private readonly float _duration = 0.2f;

        public void Init(string stateName, string value)
        {
            _nameText.text = stateName + ": ";
            _valueText.text = value;
        }
        public void UpdateValue(string value)
        {
            _valueText.transform.DOScale(0f, _duration * 0.5f).SetLoops(2, LoopType.Yoyo).
                OnStepComplete(() => _valueText.text = value);
        }
    }
}
