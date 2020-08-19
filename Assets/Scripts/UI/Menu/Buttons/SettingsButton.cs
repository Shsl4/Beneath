using UnityEngine.EventSystems;

namespace UI.Menu.Buttons
{
    public class SettingsButton : BeneathButton<MenuManager>
    {
        protected override void ExecuteAction()
        {
            Master.LastSubmit = gameObject;
            Master.DisableSelection();
            Master.SettingsMgr.Open(Master.Viewer);
        }
        
        public override void OnCancel(BaseEventData eventData)
        {
            Master.NavigateBack();
        }
        
    }
}