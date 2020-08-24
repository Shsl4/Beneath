using UnityEngine.EventSystems;

namespace UI.EscapeMenu.Buttons
{
    public class SettingsButton : BeneathButton<EscapeMenuManager>
    {
        protected override void ExecuteAction()
        {
            Manager.LastSubmit = gameObject;
            Manager.DisableSelection();
            Manager.SettingsMgr.Open(Manager.Viewer);
        }
        
        public override void OnCancel(BaseEventData eventData)
        {
            Manager.NavigateBack();
        }
        
    }
}