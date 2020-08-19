using System.Collections;
using Interfaces.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public abstract class MasterInterface : MonoBehaviour
    {
        
        [HideInInspector]
        public ControllableCharacter Viewer;
        [HideInInspector]
        public GameObject LastSubmit;
        public GameObject FirstSelectable;

        public EventSystem EventSys => EventSystem.current;
        
        public virtual void OnEnable()
        {
            StartCoroutine(DelaySelection());
        }
        
        private IEnumerator DelaySelection()
        {
            
            EventSys.SetSelectedGameObject(null);
            yield return new WaitForEndOfFrame();
            EventSys.SetSelectedGameObject(FirstSelectable);
            
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
            }
        }
        
        public bool IsOpened() { return gameObject.activeInHierarchy; }

        public bool HasParent() { return GetParents().Length > 1; }

        public MasterInterface GetFirstParent()
        {
            return GetParents()[1];
        }

        public MasterInterface[] GetParents()
        {
            return GetComponentsInParent<MasterInterface>(true);
        }

        public void NavigateBack()
        {
            if (HasParent())
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