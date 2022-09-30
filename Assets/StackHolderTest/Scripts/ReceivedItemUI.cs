using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.StackHolderTest.Scripts
{
    public class ReceivedItemUI : MonoBehaviour
    {
        [SerializeField]
        private ItemReceiver itemReceiver = default;

        [SerializeField]
        private TextMeshProUGUI amountTMPro = default;

        private int receivedAmount;

        private void Awake()
        {
            Assert.IsNotNull(itemReceiver);
            Assert.IsNotNull(amountTMPro);
        }

        private void Start()
        {
            UpdateAmountText();

            itemReceiver.ItemReceived += OnItemReceived;
        }

        private void OnDestroy()
        {
            itemReceiver.ItemReceived -= OnItemReceived;            
        }

        private void OnItemReceived(ItemReceiver sender, PickableItem receivedItem)
        {
            receivedAmount++;

            UpdateAmountText();
        }

        private void UpdateAmountText()
        {
            amountTMPro.text = receivedAmount.ToString();            
        }
    }
}