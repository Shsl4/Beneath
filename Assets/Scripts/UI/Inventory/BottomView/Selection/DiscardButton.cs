using Interfaces;
using UI.General;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory.BottomView.Selection
{
    public class DiscardButton : BeneathButton<UIManager>
    {
        
        protected override void ApplyStyles()
        {
            
            ColorBlock customColors = new ColorBlock();
            Navigation nav = new Navigation {mode = Navigation.Mode.Explicit};
            customColors.normalColor = Color.white;
            customColors.pressedColor = Color.white;
            customColors.disabledColor = Color.gray;
            customColors.selectedColor = Color.red;
            customColors.highlightedColor = Color.red;
            customColors.colorMultiplier = 1.0f;
            customColors.fadeDuration = 0.1f;
            colors = customColors;
            navigation = nav;
            
        }        
        
        protected override void SubmitAction()
        {
            if (Manager is ISlotEventResponder responder)
            {
                responder.DropItemFromActiveSlot(true);
            }
            
            if (Manager is IDialogBoxResponder dialogBoxResponder)
            {
                Manager.DisableSelection();
                dialogBoxResponder.DisplayWithText("The item was thrown away.", true);
            }
        }
        
        protected override void CancelAction()
        {
            
            if (Manager is ISlotEventResponder responder)
            {
                responder.GetActiveSlot()?.Select();
                Manager.DisableSelection();
            }
            
        }
        
    }
}
