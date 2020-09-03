using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.General
{
    [RequireComponent(typeof(AudioSource))]
    public abstract class UIManager : MonoBehaviour
    {
        
        [HideInInspector]
        public AudioSource source;

        public GameObject firstSelected;
        public EventSystem EventSys => EventSystem.current;
        
        protected virtual void Awake()
        {
            source = GetComponent<AudioSource>();
            if (!firstSelected)
            {
                Debug.LogWarning("The object " + gameObject.name + " has no first selected component");
            }
        }
        
        public virtual void OnEnable()
        {
            StartCoroutine(DelaySelection());
        }
        
        private IEnumerator DelaySelection()
        {
            
            EventSys.SetSelectedGameObject(null);
            yield return new WaitForEndOfFrame();
            EventSys.SetSelectedGameObject(firstSelected);
            
        }

        public virtual void Open()
        {
            Beneath.Data.player?.DisableInput();
            gameObject.SetActive(true);
        }
        
        public virtual void Close()
        {

            if (HasParent())
            {
                GetFirstParent().EnableSelection();
            }
            else
            {
                Beneath.Data.player.EnableInput();
            }
            
            gameObject.SetActive(false);
            
        }

        public virtual void CloseAll()
        {
            if (HasParent())
            {
                GetFirstParent().CloseAll();
            }
            
            Close();

        }
        
        public bool IsOpen() { return gameObject.activeInHierarchy; }

        public bool HasParent() { return GetParents().Length > 1; }

        public UIManager GetFirstParent()
        {
            // Index 1 because index 0 is this object itself.
            return GetParents()[1];
        }

        public UIManager[] GetParents()
        {
            return GetComponentsInParent<UIManager>(true);
        }

        public virtual void EnableSelection() {}
        public virtual void DisableSelection() {}
        
    }
}