using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Extensions
{
    public enum TLElementActionType
    {
        Position,
        Rotation,
        Scale,
        LocalPosition,
        LocalRotation,

        Fade = 100,
        PositionX,
        PositionY,
        PositionZ,
        RotationX,
        RotationY,
        RotationZ,
        ScaleX,
        ScaleY,
        ScaleZ,
        LocalPositionX,
        LocalPositionY,
        LocalPositionZ,
        LocalRotationX,
        LocalRotationY,
        LocalRotationZ,

        Color = 200
    }

    public enum TLElementTweenType
    {
        From,
        To,
        FromTo
    }

    public enum TLElementValueType
    {
        Vector,
        Float,
        Color
    }

    [Serializable]
    public class TLElement
    {
        [SerializeField]
        private bool _foldout = true;
        [SerializeField]
        private string _name = "New element";
        [SerializeField]
        private bool _isGeneric = true;
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

        public TLElement()
        {
        
        }
        public TLElement(TLElementActionType actionType, TLElementTweenType tweenType, object from, object to, float time, Ease ease, bool isRelative, bool resetOnComplete)
        {
            _isGeneric = false;
            _actionType = actionType;
            _tweenType = tweenType;

            switch (_actionType.GetValueType())
            {
                case TLElementValueType.Vector:
                    _fromVector = from as Vector3? ?? Vector3.zero;
                    _toVector = to as Vector3? ?? Vector3.zero;
                    break;
                case TLElementValueType.Float:
                    _fromFloat = from as float? ?? 0;
                    _toFloat = to as float? ?? 0;
                    break;
                case TLElementValueType.Color:
                    _fromColor = from as Color? ?? Color.white;
                    _toColor = to as Color? ?? Color.white;
                    break;
            }

            _time = time;
            _ease = ease;
            _isRelative = isRelative;
            _resetOnComplete = resetOnComplete;
        }

        public string Name { get { return _name; }}
        public bool IsGeneric { get { return _isGeneric; }}
        public TLElementActionType ActionType { get { return _actionType; }}
        public TLElementTweenType TweenType { get { return _tweenType; }}
        public object From
        {
            get
            {
                switch (ActionType.GetValueType())
                {
                    case TLElementValueType.Vector:
                        return _fromVector;
                    case TLElementValueType.Float:
                        return _fromFloat;
                    case TLElementValueType.Color:
                        return _fromColor;
                    default:
                        return null;
                }
            }
        }
        public object To
        {
            get
            {
                switch (ActionType.GetValueType())
                {
                    case TLElementValueType.Vector:
                        return _toVector;
                    case TLElementValueType.Float:
                        return _toFloat;
                    case TLElementValueType.Color:
                        return _toColor;
                    default:
                        return null;
                }
            }
        }
        public Ease Ease { get { return _ease; }}
        public bool IsRelative { get { return _isRelative; }}
        public bool ResetOnComplete { get { return _resetOnComplete; }}

        public float GetTime()
        {
            return _useRandomTime ? Random.Range(_minTime, _maxTime) : _time;
        }
    }
}