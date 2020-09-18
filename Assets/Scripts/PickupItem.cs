using System;
using Attributes;
using Interfaces;
using UI.General;
using UnityEditor;
using UnityEngine;

public class PickupItem : MonoBehaviour, IInteractable
{
    
    public int itemID;
    public SpriteRenderer Renderer => GetComponent<SpriteRenderer>();
    public BoxCollider2D Collider => GetComponent<BoxCollider2D>();
    
    public void Interact(GameObject source)
    {
        if (source.GetComponent<IInventory>() != null)
        {
            if (source.GetComponent<IInventory>().PickupItem(Beneath.Items.GetItemWithID(itemID)))
            {
                Destroy(gameObject);
            }
        }
    }

    public void Awake()
    {
        RefreshWithID(itemID);
    }

    public void RefreshWithID(int id)
    {
        
        if (!Beneath.Items.IsItemIDValid(id)) return;
        
        ItemData representedItem = Beneath.Items.GetItemWithID(id);
        Collider.size = new Vector2(1, 1);
        Renderer.sprite = representedItem.sprite;
        
    }

}

    
#if UNITY_EDITOR

[CustomEditor(typeof(PickupItem), true)]
[CanEditMultipleObjects]
public class ItemEditor : Editor
{
        
    private SerializedProperty _itemID;

    private bool _boxFoldout;
        
    protected void OnEnable()
    {
        _itemID = serializedObject.FindProperty("itemID");
    }
    
    public override void OnInspectorGUI()
    {
            
        serializedObject.Update();
        
        string[] options = new string[Beneath.Items.GetDefinedItemsCount()];

        for (int i = 0; i < Beneath.Items.GetDefinedItemsCount(); i++)
        {
            options[i] = Beneath.Items.GetItemWithID(i).name;
        }
        
        _itemID.intValue = EditorGUILayout.Popup("Represented Item", _itemID.intValue, options);
        
        serializedObject.ApplyModifiedProperties();
            
    }
    
}
    
#endif
