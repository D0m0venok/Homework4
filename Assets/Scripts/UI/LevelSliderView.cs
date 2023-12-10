using System;
using DG.Tweening;
using Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Homework4
{
    [Serializable]
    public class LevelSliderView
    {
        [SerializeField] private Image _sliderFillImage;
        [SerializeField] private Text _requiredExperienceText;
        [SerializeField] private Text _currentExperienceText;
        
        private float _requiredExperience;
        private int _currentExperience;

        public void RequireExperienceUpdate(int experience)
        {
            var tween = TL.Get(TLNames.ValueChanged);
            var exp = (int)_requiredExperience;
            DOTween.To(() => exp, RequiredSetter, experience, tween.GetTime()).SetEase(tween.Ease);
            _requiredExperience = experience;
        }
        public void CurrentExperienceUpdate(int experience)
        {
            var tween = TL.Get(TLNames.ValueChanged);
            var exp = _currentExperience;
            DOTween.To(() => exp, CurrentSetter, experience, tween.GetTime()).SetEase(tween.Ease);
            
            var endValue = experience > 0 ? experience / _requiredExperience : 0;
            DOTween.To(() => _sliderFillImage.fillAmount, value => _sliderFillImage.fillAmount = value, endValue, tween.GetTime())
                .SetEase(tween.Ease);
            _currentExperience = experience;
        }
        
        private void RequiredSetter(int value)
        { 
            _requiredExperienceText.text = value.ToString();
            
        }
        private void CurrentSetter(int value)
        { 
            _currentExperienceText.text = value.ToString();
        }
    }
}