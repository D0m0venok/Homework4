using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Extensions
{
    public enum TLComponentTriggerType
    {
        None,
        Awake,
        Start,
        OnEnable,
        OnDown,
        OnUp,
        OnClick,
        Custom
    }

    [AddComponentMenu("TweensLibrary/TLComponent")]
    public class TLComponent : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        [SerializeField]
        private GameObject _tweenTarget;
        [SerializeField]
        private TLComponentTriggerType _triggerType;
        [SerializeField]
        private string _customTrigger;
        [SerializeField]
        private bool _destroyOnComplete;
        [SerializeField]
        private bool _fromLibrary;
        [SerializeField]
        private string _name;
        [SerializeField]
        private TLElementActionType _actionType;
        [SerializeField]
        private TLElementTweenType _tweenType = TLElementTweenType.To;
        [SerializeField]
        private float _fromFloat;
        [SerializeField]
        private Vector3 _fromVector;
        [SerializeField]
        private Color _fromColor = Color.white;
        [SerializeField]
        private float _toFloat;
        [SerializeField]
        private Vector3 _toVector;
        [SerializeField]
        private Color _toColor = Color.white;
        [SerializeField]
        private float _time = 0.5f;
        [SerializeField]
        private bool _useRandomTime;
        [SerializeField]
        private float _minTime = 0.5f;
        [SerializeField]
        private float _maxTime = 1;
        [SerializeField]
        private Ease _ease = Ease.Linear;
        [SerializeField]
        private bool _isRelative;
        [SerializeField]
        private bool _resetOnComplete;
        [SerializeField]
        private float _delay;
        [SerializeField]
        private int _loops = 1;
        [SerializeField]
        private LoopType _loopType;
        [SerializeField]
        private bool _isTimeScaleIndependent;
        [SerializeField]
        private bool _useEvents;
        [SerializeField]
        private UnityEvent _onStartEvent;
        [SerializeField]
        private UnityEvent _onCompleteEvent;

        private void Awake()
        {
            if(_triggerType == TLComponentTriggerType.Awake)
                Play();
        }
        private void Start()
        {
            if(_triggerType == TLComponentTriggerType.Start)
                Play();
        }
        private void OnEnable()
        {
            if(_triggerType == TLComponentTriggerType.OnEnable)
                Play();
        }
        private void OnValidate()
        {
            if (_tweenTarget == null)
                _tweenTarget = gameObject;
        }
        private void OnMouseDown()
        {
            if(_triggerType == TLComponentTriggerType.OnDown)
                Play();
        }
        private void OnMouseUp()
        {
            if(_triggerType == TLComponentTriggerType.OnUp)
                Play();
        }
        private void OnMouseUpAsButton()
        {
            if(_triggerType == TLComponentTriggerType.OnClick)
                Play();
        }

        public void Play()
        {
            PlayAndReturnTween();
        }
        public Tweener PlayAndReturnTween()
        {
            var target = _tweenTarget == null ? gameObject : _tweenTarget;
            Tweener tween;

            if (_fromLibrary)
            {
                tween = TL.Play(_name, target);
            }
            else
            {
                object from = null;
                object to = null;

                switch (_actionType.GetValueType())
                {
                    case TLElementValueType.Vector:
                        from = _fromVector;
                        to = _toVector;
                        break;
                    case TLElementValueType.Float:
                        from = _fromFloat;
                        to = _toFloat;
                        break;
                    case TLElementValueType.Color:
                        from = _fromColor;
                        to = _toColor;
                        break;
                }

                var time = _useRandomTime ? Random.Range(_minTime, _maxTime) : _time;
                tween = TL.Play(new TLElement(_actionType, _tweenType, from, to, time, _ease, _isRelative, _resetOnComplete), target);
            }

            Action onComplete = null;
            if (_useEvents)
            {
                if (_onStartEvent != null)
                    tween.OnStart(() => _onStartEvent.Invoke());
                if (_onCompleteEvent != null)
                    onComplete += () => _onCompleteEvent.Invoke();
            }

            if (_destroyOnComplete)
                onComplete += () => Destroy(gameObject);

            if (onComplete != null)
                tween.OnComplete(() => onComplete());

            return tween.SetDelay(_delay).SetLoops(_loops, _loopType).SetUpdate(_isTimeScaleIndependent);
        }
        public void Trigger(string trigger)
        {
            if (_customTrigger == trigger)
                Play();
        }
        public Tweener TriggerAndReturnTween(string trigger)
        {
            return _customTrigger == trigger ? PlayAndReturnTween() : null;
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            if(_triggerType == TLComponentTriggerType.OnDown && CheckInteractable())
                Play();
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            if(_triggerType == TLComponentTriggerType.OnUp && CheckInteractable())
                Play();
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if(_triggerType == TLComponentTriggerType.OnClick && CheckInteractable())
                Play();
        }

        private bool CheckInteractable()
        {
            if (_selectable == null)
                _selectable = GetComponent<Selectable>();

            return _selectable != null ? _selectable.IsInteractable() : true;
        }

        private Selectable _selectable;
    }
}