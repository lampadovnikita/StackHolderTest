using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.StackHolderTest.Scripts
{
    public class ItemReceiver : MonoBehaviour
    {
        public event Action<ItemReceiver, PickableItem> ItemReceived;

        [SerializeField]
        private PickableItemType typeToReceive = default;

        [SerializeField]
        [Tooltip("Delay between picking of items in seconds")]
        private float receivingDelay = 1f;

        private ItemPicker currentPicker;

        private void Awake()
        {
            Assert.IsTrue(receivingDelay > 0f);
        }

        private void OnTriggerEnter(Collider other)
        {
            ItemPicker itemPicker = other.gameObject.GetComponent<ItemPicker>();
            if ((itemPicker == null) || (currentPicker != null))
            {
                return;
            }

            currentPicker = itemPicker;
            StartCoroutine(ReceivingRoutine(itemPicker));
        }

        private void OnTriggerExit(Collider other)
        {
            ItemPicker itemPicker = other.gameObject.GetComponent<ItemPicker>();
            if (itemPicker == null)
            {
                return;
            }

            if (currentPicker.gameObject == itemPicker.gameObject)
            {
                currentPicker = null;

                StopAllCoroutines();
            }
        }

        private IEnumerator ReceivingRoutine(ItemPicker itemPicker)
        {
            while (true)
            {
                PickableItem pickableItem = itemPicker.PeekItem();
                if (pickableItem == null)
                {
                    break;
                }

                if (pickableItem.Type != typeToReceive)
                {
                    break;
                }

                itemPicker.PopItem();
                Destroy(pickableItem.gameObject);

                ItemReceived?.Invoke(this, pickableItem);

                yield return new WaitForSeconds(receivingDelay);
            }
        }
    }
}