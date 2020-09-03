using UnityEngine;

namespace UI.Inventory.BottomView.Selection
{
    public class SelectionComponent : MonoBehaviour
    {

        public UseButton UseBeneath => GetComponentInChildren<UseButton>();
        public DiscardButton DiscardBeneath => GetComponentInChildren<DiscardButton>();
        
    }
}
