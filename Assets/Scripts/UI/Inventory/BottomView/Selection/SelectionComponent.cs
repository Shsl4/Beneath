using UnityEngine;

namespace UI.Inventory.BottomView.Selection
{
    public class SelectionComponent : MonoBehaviour
    {

        public UseButton Use => GetComponentInChildren<UseButton>();
        public DiscardButton Discard => GetComponentInChildren<DiscardButton>();
        
    }
}
