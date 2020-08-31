using UnityEngine.EventSystems;

namespace UI.EscapeMenu.Buttons
{
    public class StatsButton : BeneathButton<EscapeMenuManager>
    {
        protected override void ExecuteAction()
        {
            Manager.LastSubmit = gameObject;
            Manager.DisableSelection();
            Manager.StatsMgr.Open();
        }
        
        public override void OnCancel(BaseEventData eventData)
        {
            Manager.NavigateBack();
        }
    }
}
