using UI;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.SaveMenu
{
    public class SaveButton : BeneathButton<SaveMenuManager>
    {
        protected override void ExecuteAction()
        {
            Beneath.SaveManager.SaveProgress();
            Manager.OnSaved();
        }
        
        public override void OnCancel(BaseEventData eventData)
        {
            Manager.NavigateBack();
        }
        
    }
}