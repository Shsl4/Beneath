using UnityEditor;
using UnityEngine;

namespace Environment
{
    public class RoomLinkTrigger : RoomLinker
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.GetComponent<ControllableCharacter>())
            {
                Travel();
            }        
        }
    }
    
    #if UNITY_EDITOR
    [CustomEditor(typeof(RoomLinkTrigger))]
    [CanEditMultipleObjects]
    public class LinkTriggerEditor : RoomLinkerEditor
    {

    }
    #endif
    
}