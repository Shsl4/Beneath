using System;
using Interfaces;
using UnityEngine;

public class InteractableObject : MonoBehaviour, IInteractable, IIdentifiable
{

    public string objectName;
    
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
        return objectName;
    }
    
}