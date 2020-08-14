using Assets.Scripts.UI.Inventory.BottomView.Selection;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.Inventory.BottomView.Info
{
    public class BottomComponent : MonoBehaviour
    {

        public InfoComponent Info { get; private set; }
        public EmptyComponent Empty { get; private set; }
        public SelectionComponent Selection => GetComponentInChildren<SelectionComponent>();

        void Start()
        {
            Info = GetComponentInChildren<InfoComponent>();
            Empty = GetComponentInChildren<EmptyComponent>();
        }
        
        public void UpdateInfo(InventoryItem item)
        {
            
            if (item != null)
            {
                Info.NameBox.GetComponent<TMP_Text>().text = "NAME: " + item.Name.ToUpper();
                Info.TypeBox.GetComponent<TMP_Text>().text = "TYPE: " + item.Type.ToString().ToUpper();
                Info.ValueBox.GetComponent<TMP_Text>().text = "VALUE: " + (item.Value > 0 ? item.Value.ToString() : "NONE");
                Empty.gameObject.SetActive(false);
                Info.gameObject.SetActive(true);
            }
            else
            {
                Info.gameObject.SetActive(false);
                Empty.gameObject.SetActive(true);
            }
        }
    }
}
