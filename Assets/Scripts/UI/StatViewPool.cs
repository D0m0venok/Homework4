using Extensions;
using UnityEngine;

namespace Homework4
{
    public sealed class StatViewPool : GameObjectsPool<CharacterStatView>
    {
        public StatViewPool(CharacterStatView prefab, Transform activeContainer = null, Transform disableContainer = null, int initSize = 0, int maxSize = int.MaxValue)
            : base(prefab, activeContainer, disableContainer, initSize, maxSize){}
    }
}