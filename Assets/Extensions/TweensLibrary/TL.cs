using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;

namespace Extensions
{
    public static class TL
    {
        static TL()
        {
            _resource = Resources.Load<TLResource>("TL/TLResource");
        }

        public static float TweensTimeScale
        {
            get { return Check() ? _resource.TweensTimeScale : 1; }
            set
            {
                if (Check())
                    _resource.TweensTimeScale = value;
            }
        }

        public static TLElement Get(string name)
        {
            var element = Check() ? _resource.GetElement(name) : null;
            if(element == null)
                Debug.LogException(new Exception("TweensLibrary: element " + name + " is not found"));
            return element;
        }
        public static TLElement[] GetAll()
        {
            return Check() ? _resource.GetElements() : null;
        }
        public static Tweener Play(string name, GameObject target)
        {
            var element = Get(name);
            if (element == null)
                return null;

            return PlayTween(element, target, element.TweenType, element.From, element.To, element.IsRelative, element.ResetOnComplete);
        }
        public static Tweener Play(TLElement element, GameObject target)
        {
            if (!Check())
                return null;

            return PlayTween(element, target, element.TweenType, element.From, element.To, element.IsRelative, element.ResetOnComplete);
        }
        public static Tweener PlayTo(string name, GameObject target, object to, bool isRelative = false, bool resetOnComplete = false)
        {
            var element = Get(name);
            if (element == null)
                return null;

            return PlayTween(element, target, TLElementTweenType.To, element.From, to, isRelative, resetOnComplete);
        }
        public static Tweener PlayFrom(string name, GameObject target, object from, bool isRelative = false, bool resetOnComplete = false)
        {
            var element = Get(name);
            if (element == null)
                return null;

            return PlayTween(element, target, TLElementTweenType.From, from, element.To, isRelative, resetOnComplete);
        }
        public static Tweener PlayFromTo(string name, GameObject target, object from, object to, bool isRelative = false, bool resetOnComplete = false)
        {
            var element = Get(name);
            if (element == null)
                return null;

            return PlayTween(element, target, TLElementTweenType.FromTo, from, to, isRelative, resetOnComplete);
        }
        public static List<Tweener> Trigger(GameObject gameObject, string trigger, bool includeChildren = false, TweenCallback onCompleteAction = null)
        {
            var tweens = (includeChildren ? gameObject.GetComponentsInChildren<TLComponent>() : gameObject.GetComponents<TLComponent>())
                .Select(c => c.TriggerAndReturnTween(trigger)).Where(t => t != null).ToList();
        
            if(onCompleteAction == null)
                return tweens;

            var last = tweens.GetLongest();
            if (last != null)
                last.OnComplete(onCompleteAction);
            else
                onCompleteAction();
        
            return tweens;
        }
        public static TLElementValueType GetValueType(this TLElementActionType actionType)
        {
            return (TLElementValueType) ((int)actionType / 100);
        }
        public static Tweener GetLongest(this IEnumerable<Tweener> tweens)
        {
            return tweens.OrderBy(t => t.Duration() * t.Loops() + t.Delay()).LastOrDefault();
        }
        public static T OnComplete<T>(this T t, TweenCallback action, bool removeOtherActions = false) where T : Tween
        {
            if (t == null || !t.IsActive())
                return t;

            if(removeOtherActions)
                t.onComplete = action;
            else
                t.onComplete += action;
            return t;
        }

        private static bool Check()
        {
            if (_resource != null)
                return true;

            Debug.LogError("TweensLibrary: resource file is not found!");
            return false;
        }
        private static Tweener PlayTween(TLElement element, GameObject target, TLElementTweenType tweenType, object from, object to, bool isRelative, bool resetOnComplete)
        {
            if (element.IsGeneric)
            {
                Debug.LogError("TweensLibrary: can't play generic element " + target.name);
                return null;
            }

            Tweener tween = null;

            var time = element.GetTime();
            ITLTweenDesc description = null;

            switch (element.ActionType)
            {
                case TLElementActionType.PositionX:
                {
                    var desc = new TLTweenDesc<float>(tweenType, () => target.transform.position.x, v => target.transform.position = target.transform.position.SetX(v), 
                        v => target.transform.position += new Vector3(v, 0, 0), from, to, isRelative);
                    tween = target.transform.DOMoveX(desc.Value, time);
                    description = desc;
                    break;
                }
                case TLElementActionType.PositionY:
                {
                    var desc = new TLTweenDesc<float>(tweenType, () => target.transform.position.y, v => target.transform.position = target.transform.position.SetY(v), 
                        v => target.transform.position += new Vector3(0, v, 0), from, to, isRelative);
                    tween = target.transform.DOMoveY(desc.Value, time);
                    description = desc;
                    break;
                }
                case TLElementActionType.PositionZ:
                {
                    var desc = new TLTweenDesc<float>(tweenType, () => target.transform.position.z, v => target.transform.position = target.transform.position.SetZ(v), 
                        v => target.transform.position += new Vector3(0, 0, v), from, to, isRelative);
                    tween = target.transform.DOMoveZ(desc.Value, time);
                    description = desc;
                    break;
                }
                case TLElementActionType.RotationX:
                {
                    var desc = new TLTweenDesc<float>(tweenType, () => target.transform.eulerAngles.x, v => target.transform.eulerAngles = target.transform.eulerAngles.SetX(v), 
                        v => target.transform.eulerAngles += new Vector3(v, 0, 0), from, to, isRelative);
                    tween = target.transform.DORotate(target.transform.eulerAngles.SetX(desc.Value), time);
                    description = desc;
                    break;
                }
                case TLElementActionType.RotationY:
                {
                    var desc = new TLTweenDesc<float>(tweenType, () => target.transform.eulerAngles.y, v => target.transform.eulerAngles = target.transform.eulerAngles.SetY(v), 
                        v => target.transform.eulerAngles += new Vector3(0, v, 0), from, to, isRelative);
                    tween = target.transform.DORotate(target.transform.eulerAngles.SetY(desc.Value), time);
                    description = desc;
                    break;
                }
                case TLElementActionType.RotationZ:
                {
                    var desc = new TLTweenDesc<float>(tweenType, () => target.transform.eulerAngles.z, v => target.transform.eulerAngles = target.transform.eulerAngles.SetZ(v), 
                        v => target.transform.eulerAngles += new Vector3(0, 0, v), from, to, isRelative);
                    tween = target.transform.DORotate(target.transform.eulerAngles.SetZ(desc.Value), time);
                    description = desc;
                    break;
                }
                case TLElementActionType.ScaleX:
                {
                    var desc = new TLTweenDesc<float>(tweenType, () => target.transform.localScale.x, v => target.transform.localScale = target.transform.localScale.SetX(v), 
                        v => target.transform.localScale += new Vector3(v, 0, 0), from, to, isRelative);
                    tween = target.transform.DOScaleX(desc.Value, time);
                    description = desc;
                    break;
                }
                case TLElementActionType.ScaleY:
                {
                    var desc = new TLTweenDesc<float>(tweenType, () => target.transform.localScale.y, v => target.transform.localScale = target.transform.localScale.SetY(v), 
                        v => target.transform.localScale += new Vector3(0, v, 0), from, to, isRelative);
                    tween = target.transform.DOScaleY(desc.Value, time);
                    description = desc;
                    break;
                }
                case TLElementActionType.ScaleZ:
                {
                    var desc = new TLTweenDesc<float>(tweenType, () => target.transform.localScale.z, v => target.transform.localScale = target.transform.localScale.SetZ(v), 
                        v => target.transform.localScale += new Vector3(0, 0, v), from, to, isRelative);
                    tween = target.transform.DOScaleZ(desc.Value, time);
                    description = desc;
                    break;
                }
                case TLElementActionType.LocalPositionX:
                {
                    var desc = new TLTweenDesc<float>(tweenType, () => target.transform.localPosition.x, v => target.transform.localPosition = target.transform.localPosition.SetX(v), 
                        v => target.transform.localPosition += new Vector3(v, 0, 0), from, to, isRelative);
                    tween = target.transform.DOLocalMoveX(desc.Value, time);
                    description = desc;
                    break;
                }
                case TLElementActionType.LocalPositionY:
                {
                    var desc = new TLTweenDesc<float>(tweenType,() => target.transform.localPosition.y, v => target.transform.localPosition = target.transform.localPosition.SetY(v), 
                        v => target.transform.localPosition += new Vector3(0, v, 0), from, to, isRelative);
                    tween = target.transform.DOLocalMoveY(desc.Value, time);
                    description = desc;
                    break;
                }
                case TLElementActionType.LocalPositionZ:
                {
                    var desc = new TLTweenDesc<float>(tweenType,() => target.transform.localPosition.z, v => target.transform.localPosition = target.transform.localPosition.SetZ(v), 
                        v => target.transform.localPosition += new Vector3(0, 0, v), from, to, isRelative);
                    tween = target.transform.DOLocalMoveZ(desc.Value, time);
                    description = desc;
                    break;
                }
                case TLElementActionType.LocalRotationX:
                {
                    var desc = new TLTweenDesc<float>(tweenType, () => target.transform.localEulerAngles.x, v => target.transform.localEulerAngles = target.transform.localEulerAngles.SetX(v), 
                        v => target.transform.localEulerAngles += new Vector3(v, 0, 0), from, to, isRelative);
                    tween = target.transform.DOLocalRotate(target.transform.localEulerAngles.SetX(desc.Value), time);
                    description = desc;
                    break;
                }
                case TLElementActionType.LocalRotationY:
                {
                    var desc = new TLTweenDesc<float>(tweenType, () => target.transform.localEulerAngles.y, v => target.transform.localEulerAngles = target.transform.localEulerAngles.SetY(v), 
                        v => target.transform.localEulerAngles += new Vector3(0, v, 0), from, to, isRelative);
                    tween = target.transform.DOLocalRotate(target.transform.localEulerAngles.SetY(desc.Value), time);
                    description = desc;
                    break;
                }
                case TLElementActionType.LocalRotationZ:
                {
                    var desc = new TLTweenDesc<float>(tweenType, () => target.transform.localEulerAngles.z, v => target.transform.localEulerAngles = target.transform.localEulerAngles.SetZ(v), 
                        v => target.transform.localEulerAngles += new Vector3(0, 0, v), from, to, isRelative);
                    tween = target.transform.DOLocalRotate(target.transform.localEulerAngles.SetZ(desc.Value), time);
                    description = desc;
                    break;
                }
                case TLElementActionType.Fade:
                {
                    var canvasGroup = target.GetComponentInChildren<CanvasGroup>();
                    if (canvasGroup != null)
                    {
                        var desc = new TLTweenDesc<float>(tweenType, () => canvasGroup.alpha, v => canvasGroup.alpha = v, 
                            v => canvasGroup.alpha += v, from, to, isRelative);
                        tween = canvasGroup.DOFade(desc.Value, time);
                        description = desc;
                        break;
                    }

                    var graphics = target.GetComponentInChildren<UnityEngine.UI.Graphic>();
                    if (graphics != null)
                    {
                        var desc = new TLTweenDesc<float>(tweenType, () => graphics.color.a, v => graphics.color = graphics.color.SetAlpha(v), 
                            v => graphics.color += new Color(0, 0, 0, v), from, to, isRelative);
                        tween = graphics.DOFade(desc.Value, time);
                        description = desc;
                        break;
                    }

                    var sprite = target.GetComponentInChildren<SpriteRenderer>();
                    if (sprite != null)
                    {
                        var desc = new TLTweenDesc<float>(tweenType, () => sprite.color.a, v => sprite.color = sprite.color.SetAlpha(v), 
                            v => sprite.color += new Color(0, 0, 0, v), from, to, isRelative);
                        tween = sprite.DOFade(desc.Value, time);
                        description = desc;
                        break;
                    }

                    Debug.LogError("TweensLibrary: can't fade " + target.name);
                    return null;
                }
                case TLElementActionType.Position:
                {
                    var desc = new TLTweenDesc<Vector3>(tweenType, () => target.transform.position, v => target.transform.position = v, 
                        v => target.transform.position += v, from, to, isRelative);
                    tween = target.transform.DOMove(desc.Value, time);
                    description = desc;
                    break;
                }
                case TLElementActionType.Rotation:
                {
                    var desc = new TLTweenDesc<Vector3>(tweenType, () => target.transform.eulerAngles, v => target.transform.eulerAngles = v, 
                        v => target.transform.eulerAngles += v, from, to, isRelative);
                    tween = target.transform.DORotate(desc.Value, time);
                    description = desc;
                    break;
                }
                case TLElementActionType.Scale:
                {
                    var desc = new TLTweenDesc<Vector3>(tweenType, () => target.transform.localScale, v => target.transform.localScale = v, 
                        v => target.transform.localScale += v, from, to, isRelative);
                    tween = target.transform.DOScale(desc.Value, time);
                    description = desc;
                    break;
                }
                case TLElementActionType.LocalPosition:
                {
                    var desc = new TLTweenDesc<Vector3>(tweenType, () => target.transform.localPosition, v => target.transform.localPosition = v, 
                        v => target.transform.localPosition += v, from, to, isRelative);
                    tween = target.transform.DOLocalMove(desc.Value, time);
                    description = desc;
                    break;
                }
                case TLElementActionType.LocalRotation:
                {
                    var desc = new TLTweenDesc<Vector3>(tweenType, () => target.transform.localEulerAngles, v => target.transform.localEulerAngles = v, 
                        v => target.transform.localEulerAngles += v, from, to, isRelative);
                    tween = target.transform.DOLocalRotate(desc.Value, time, RotateMode.FastBeyond360);
                    break;
                }
                case TLElementActionType.Color:
                {
                    var graphics = target.GetComponentInChildren<UnityEngine.UI.Graphic>();
                    if (graphics != null)
                    {
                        var desc = new TLTweenDesc<Color>(tweenType, () => graphics.color, v => graphics.color = v, 
                            v => graphics.color += v, from, to, isRelative);
                        tween = graphics.DOColor(desc.Value, time);
                        description = desc;
                        break;
                    }

                    var sprite = target.GetComponentInChildren<SpriteRenderer>();
                    if (sprite != null)
                    {
                        var desc = new TLTweenDesc<Color>(tweenType, () => sprite.color, v => sprite.color = v, 
                            v => sprite.color += v, from, to, isRelative);
                        tween = sprite.DOColor(desc.Value, time);
                        description = desc;
                        break;
                    }

                    Debug.LogError("TweensLibrary: can't color " + target.name);
                    return null;
                }
            }

            if (description == null)
                return null;

            var t = tween.SetTarget(target).SetEase(element.Ease).SetRelative(description.IsRelative);

            if (resetOnComplete)
                t.OnComplete(() => description.Reset());

            return t;
        }
        private static Color SetAlpha(this Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }
        private static Vector3 SetX(this Vector3 vector, float x)
        {
            return new Vector3(x, vector.y, vector.z);
        }
        private static Vector3 SetY(this Vector3 vector, float y)
        {
            return new Vector3(vector.x, y, vector.z);
        }
        private static Vector3 SetZ(this Vector3 vector, float z)
        {
            return new Vector3(vector.x, vector.y, z);
        }

        private static readonly TLResource _resource;

        private interface ITLTweenDesc
        {
            bool IsRelative { get; }

            void Reset();
        }

        private class TLTweenDesc<T> : ITLTweenDesc where T: struct
        {
            public TLTweenDesc(TLElementTweenType tweenType, DOGetter<T> getter, DOSetter<T> setter, DOSetter<T> relativeSetter, object from, object to, bool isRelative)
            {
                _initValue = getter();

                switch (tweenType)
                {
                    case TLElementTweenType.From:
                        Value = _initValue;
                        if (isRelative)
                            relativeSetter((T)from);
                        else
                            setter((T)from);
                        IsRelative = false;
                        break;
                    case TLElementTweenType.To:
                        Value = (T)to;
                        IsRelative = isRelative;
                        break;
                    case TLElementTweenType.FromTo:
                        Value = (T)to;
                        if (isRelative)
                            relativeSetter((T)from);
                        else
                            setter((T)from);
                        IsRelative = isRelative;
                        break;
                }

                _setter = setter;
            }

            public T Value { get; private set; }
            public bool IsRelative { get; private set; }

            public void Reset()
            {
                _setter(_initValue);
            }

            private readonly DOSetter<T> _setter;
            private readonly T _initValue;
        }
    }
}
