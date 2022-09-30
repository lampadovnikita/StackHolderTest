using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.StackHolderTest.Scripts
{
    public class ItemPicker : MonoBehaviour
    {
        [SerializeField]
        private Transform itemsStackOrigin = default;

        [SerializeField]
        private float itemsOffset;

        private Stack itemsStack;

        private void Awake()
        {
            Assert.IsNotNull(itemsStackOrigin);

            itemsStack = new Stack();
        }

        private void OnTriggerEnter(Collider other)
        {
            PickableItem pickableItem = other.gameObject.GetComponent<PickableItem>();
            if (pickableItem == null)       
            {
                return;
            }

            pickableItem.PickUp();

            pickableItem.transform.parent = itemsStackOrigin.transform;
            pickableItem.transform.position = itemsStackOrigin.TransformPoint(0f, itemsOffset * itemsStack.Count, 0f);
            pickableItem.transform.rotation = Quaternion.LookRotation(itemsStackOrigin.transform.forward, itemsStackOrigin.transform.up);

            itemsStack.Push(pickableItem);
        }

        public PickableItem PopItem()
        {
            if (itemsStack.Count == 0)
            {
                return null;
            }

            return itemsStack.Pop() as PickableItem;
        }

        public PickableItem PeekItem()
        {
            if (itemsStack.Count == 0)
            {
                return null;
            }

            return itemsStack.Peek() as PickableItem;
        }
    }
}