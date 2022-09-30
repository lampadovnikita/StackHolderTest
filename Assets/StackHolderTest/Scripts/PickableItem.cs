using System;
using UnityEngine;

namespace Assets.StackHolderTest.Scripts
{
    public class PickableItem : MonoBehaviour
    {
        public event Action<PickableItem> ItemPicked;

        [SerializeField]
        private PickableItemType type;

        public PickableItemType Type => type;

        public void PickUp()
        {
            ItemPicked?.Invoke(this);
        }
    }
}