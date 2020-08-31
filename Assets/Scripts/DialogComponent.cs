using Interfaces;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DialogComponent : MonoBehaviour, IInteractable 
{

    [TextArea]
    public string textToDisplay = "...";
    
    public virtual void Interact(GameObject source)
    {
        if (source.GetComponent<ControllableCharacter>() != null)
        {
            Beneath.Data.DialogBox.OpenWithText(textToDisplay);
        }
    }
}