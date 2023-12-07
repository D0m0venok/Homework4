using Extensions;
using UnityEngine;

namespace Homework4
{
    public sealed class StatPool : GameObjectsPool<CharacterStatView>
    {
        public StatPool(CharacterStatView prefab, Transform activeContainer = null, Transform disableContainer = null, int initSize = 0, int maxSize = int.MaxValue)
            : base(prefab, activeContainer, disableContainer, initSize, maxSize){}
    }
}