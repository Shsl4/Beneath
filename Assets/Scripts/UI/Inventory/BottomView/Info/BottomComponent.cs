using System;
using System.Collections;
using UI.General;
using UI.Inventory.BottomView.Selection;
using UnityEngine;

namespace UI.Inventory.BottomView.Info
{
    public class BottomComponent : MonoBehaviour
    {
        public TextViewComponent TextView => GetComponentInChildren<TextViewComponent>(true);
        public SelectionComponent Selection => GetComponentInChildren<SelectionComponent>();

        private IEnumerator _revealRoutine;

        public void UpdateInfo(ItemData itemData)
        {
            
            string output = "";

            if (itemData != null)
            {
                
                output += "Name: " + itemData.name + "\n\n";
                output += itemData.FormatDescription();

            }
            else
            {
                output = "This slot is empty.";
            }
            
            TextView.RevealText(output);
            
        }

    }
}
