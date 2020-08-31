using System;
using System.Collections;
using UI.Inventory.BottomView.Selection;
using UnityEngine;

namespace UI.Inventory.BottomView.Info
{
    public class BottomComponent : MonoBehaviour
    {
        public TextViewComponent TextView => GetComponentInChildren<TextViewComponent>(true);
        public SelectionComponent Selection => GetComponentInChildren<SelectionComponent>();

        private IEnumerator _revealRoutine;

        public void UpdateInfo(InventoryItem item)
        {
            
            String output = "";

            if (item != null)
            {
                
                output += "Name: " + item.name + "\n\n";
                output += item.FormatDescription();

            }
            else
            {
                output = "This slot is empty.";
            }
            
            TextView.RevealText(output);
            
        }

    }
}
