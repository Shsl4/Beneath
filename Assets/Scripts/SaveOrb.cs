using Interfaces;
using UI.SaveMenu;
using UnityEngine;

public class SaveOrb : MonoBehaviour, IInteractable
{
    public void Interact(GameObject source)
    {
            
        Beneath.InstantiateSafeThen(Beneath.AssetReferences.SaveMenu, handle =>
        {
            handle.Result.GetComponent<SaveMenuManager>().Open();
        });
            
    }
}