using Assets.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
    public abstract class UserInterface : MonoBehaviour, IUserInterface
    {

        public GameObject MasterView => gameObject.transform.root.gameObject;
        
        public bool IsDisplayed() { return gameObject.activeSelf; }

        public abstract void OnSelect(BaseEventData eventData);

        public abstract void OnSubmit(BaseEventData eventData);

        public abstract void OnCancel(BaseEventData eventData);
        
    }
}