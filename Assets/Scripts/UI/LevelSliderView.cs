using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Homework4
{
    public class LevelSliderView : MonoBehaviour
    {
        [SerializeField] private Image _slider;
        [SerializeField] private Text _requiredExperienceText;
        [SerializeField] private Text _currentExperienceText;

        private readonly float _duration = 0.3f;
        private float _requiredExperience;
        private int _currentExperience;

        public void RequireExperienceUpdate(int experience)
        {
            var exp = (int)_requiredExperience;
            DOTween.To(() => exp, RequiredSetter, experience, _duration);
            _requiredExperience = experience;
        }
        public void CurrentExperienceUpdate(int experience)
        {
            var exp = _currentExperience;
            DOTween.To(() => exp, CurrentSetter, experience, _duration);
            
            var endValue = experience > 0 ? experience / _requiredExperience : 0;
            DOTween.To(() => _slider.fillAmount, value => _slider.fillAmount = value, endValue, _duration);
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