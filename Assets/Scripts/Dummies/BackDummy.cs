using UI;
using UnityEngine.EventSystems;

namespace Dummies
{
    public class BackDummy : BeneathButton<UIManager>
    {
        
        protected override void ExecuteAction()
        {
            Manager.NavigateBack();
        }

        public override void OnCancel(BaseEventData eventData)
        {
            Manager.NavigateBack();
        }
    }
}
