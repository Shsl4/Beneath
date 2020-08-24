using Assets.Scripts.UI;
using Interfaces.UI;
using TMPro;
using UI.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public abstract class BeneathButton<T> : BeneathSelectable<T> where T : UIManager
    {
        public TMP_Text TextBox => GetComponent<TMP_Text>();

        public override void OnCancel(BaseEventData eventData) {}

        public sealed override void OnSubmit(BaseEventData eventData)
        {
            if (SubmitSound)
            {
                Manager.Source.PlayOneShot(SubmitSound);
            }
            ExecuteAction();
        }
        
        protected abstract void ExecuteAction();
        
    }
}
