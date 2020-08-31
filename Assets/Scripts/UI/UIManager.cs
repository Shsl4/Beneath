using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    [RequireComponent(typeof(AudioSource))]
    public abstract class UIManager : MonoBehaviour
    {
        
        [HideInInspector]
        public GameObject LastSubmit;
        [HideInInspector]
        public AudioSource Source;

        public GameObject FirstSelected;
        public EventSystem EventSys => EventSystem.current;
        
        protected virtual void Awake()
        {
            Source = GetComponent<AudioSource>();
        }
        
        public virtual void OnEnable()
        {
            StartCoroutine(DelaySelection());
        }
        
        private IEnumerator DelaySelection()
        {
            
            EventSys.SetSelectedGameObject(null);
            yield return new WaitForEndOfFrame();
            EventSys.SetSelectedGameObject(FirstSelected);
            
        }

        public virtual void Open()
        {
            Beneath.Data.player?.DisableInput();
            gameObject.SetActive(true);
        }
        
        public virtual void Close()
        {
            gameObject.SetActive(false);
            LastSubmit = null;

            if (HasParent())
            {
                GetFirstParent().EnableSelection();
                EventSys.SetSelectedGameObject(GetFirstParent().LastSubmit);
                GetFirstParent().LastSubmit = null;
            }
            else
            {
                Beneath.Data.player.EnableInput();
            }
        }
        
        public bool IsOpened() { return gameObject.activeInHierarchy; }

        public bool HasParent() { return GetParents().Length > 1; }

        public UIManager GetFirstParent()
        {
            return GetParents()[1];
        }

        public UIManager[] GetParents()
        {
            return GetComponentsInParent<UIManager>(true);
        }

        public virtual void NavigateBack()
        {

            if (LastSubmit != null)
            {
                EventSys.SetSelectedGameObject(LastSubmit);
                LastSubmit = null;
            }
            else
            {
                Close();
            }
        }

        public virtual void EnableSelection() {}
        public virtual void DisableSelection() {}
        
    }
}