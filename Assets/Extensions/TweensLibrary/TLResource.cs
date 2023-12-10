using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Extensions
{
    public class TLResource : ScriptableObject
    {
        [HideInInspector]
        [SerializeField]
        private TLElement[] _elements = new TLElement[0];

        public float TweensTimeScale { get { return DOTween.timeScale; } set { DOTween.timeScale = value; } }

        public void AddElement(TLElement element)
        {
            var list = _elements.ToList();
            list.Add(element);
            _elements = list.ToArray();
        }
        public void RemoveElement(string elementName)
        {
            var list = _elements.ToList();
            list.Remove(GetElement(elementName));
            _elements = list.ToArray();
        }
        public TLElement GetElement(string elementName)
        {
            return _elements.FirstOrDefault(e => e.Name == elementName);
        }
        public TLElement[] GetElements()
        {
            return _elements;
        }
        public void MoveElementUp(TLElement element)
        {
            var list = _elements.ToList();
            var index = list.IndexOf(element);
            if(index <= 0)
                return;

            list.RemoveAt(index);
            list.Insert(index - 1, element);

            _elements = list.ToArray();
        }
        public void MoveElementDown(TLElement element)
        {
            var list = _elements.ToList();
            var index = list.IndexOf(element);
            if(index < 0 || index >= list.Count - 1)
                return;

            list.RemoveAt(index);
            list.Insert(index + 1, element);

            _elements = list.ToArray();
        }
    }
}