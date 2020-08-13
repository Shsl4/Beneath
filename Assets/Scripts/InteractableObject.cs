using System;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts
{
    public class InteractableObject : MonoBehaviour, IInteractable, IIdentifiable
    {

        public string ObjectName;
    
        public void Interact(GameObject source)
        {

            String sourceName = source.name;
        
            if (source.GetComponent<IIdentifiable>() != null)
            {
                sourceName = source.GetComponent<IIdentifiable>().GetIdentifiableName();
            }
        
            Debug.Log(sourceName + " interacted with " + GetIdentifiableName());
        
        }

        public string GetIdentifiableName()
        {
            return ObjectName;
        }
    
    }
}
