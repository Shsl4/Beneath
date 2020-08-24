using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(AudioSource))]
    public abstract class UIManager : MonoBehaviour
    {
        
        [HideInInspector]
        public ControllableCharacter Viewer;
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

        public virtual void Open(ControllableCharacter character)
        {
            gameObject.SetActive(true);
            Viewer = character;
        }
        
        public virtual void Close()
        {
            gameObject.SetActive(false);
            Viewer = null;
            LastSubmit = null;

            if (HasParent())
            {
                GetFirstParent().EnableSelection();
                EventSys.SetSelectedGameObject(GetFirstParent().LastSubmit);
                GetFirstParent().LastSubmit = null;
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
            else if (HasParent())
            {
                Close();
            }
            else
            {
                Viewer.CloseActiveInterface();
            }
        }

        public virtual void EnableSelection() {}
        public virtual void DisableSelection() {}
        
    }
}