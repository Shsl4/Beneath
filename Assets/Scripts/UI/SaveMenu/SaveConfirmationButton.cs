using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.SaveMenu
{
    public class SaveConfirmationButton : BeneathButton<SaveMenuManager>
    {
        protected override void ExecuteAction()
        {
            Manager.Close();
        }

        public override void OnCancel(BaseEventData eventData)
        {
            Manager.Close();
        }
    }
}