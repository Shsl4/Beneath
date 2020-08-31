using Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
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
        
        protected override void ExecuteAction()
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
        
        public override void OnCancel(BaseEventData eventData)
        {
            Manager.EventSys.SetSelectedGameObject(Manager.LastSubmit);
            Manager.LastSubmit = null;
            Manager.DisableSelection();
        }
        
    }
}
