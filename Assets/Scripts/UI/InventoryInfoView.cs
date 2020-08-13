using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class InventoryInfoView : MonoBehaviour
    {
        
        public GameObject NameBox;
        public GameObject TypeBox;
        public GameObject ValueBox;

        public void UpdateInfo(InventoryItem item)
        {

            NameBox.GetComponent<TMP_Text>().text = "Name: " + item.Name;
            TypeBox.GetComponent<TMP_Text>().text = "Type: " + item.Type;
            ValueBox.GetComponent<TMP_Text>().text = "Value: " + item.Value;

        }

    }
}
