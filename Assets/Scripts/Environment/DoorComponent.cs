using System;
using System.Collections;
using System.Reflection;
using Interfaces;
using UnityEditor;
using UnityEngine;

namespace Environment
{

    [RequireComponent(typeof(AudioSource), typeof(SpriteRenderer))]
    public class DoorComponent : RoomLinker, IInteractable
    {
        public static class UnlockConditions
        {
            public static bool False() { return false; }
            public static bool True() { return true; }
            public static bool InventoryTest() { return Beneath.data.PlayerInventory.GetSlot(0).GetItem() != null; }
        }

        [TextArea] public string openText = "The door opened.";
        [TextArea] public string closedText;
        
        public bool openedByDefault;
        public Sprite openedSprite;
        public Sprite closedSprite;
        public AudioClip closedSound;
        public AudioClip openSound;
        
        [HideInInspector] public SpriteRenderer spriteRenderer;
        [HideInInspector] public AudioSource audioSource;
        [HideInInspector] public int invokeIndex;
        public bool IsOpened { get; private set; }
        
        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            audioSource = GetComponent<AudioSource>();
            
            if (openedByDefault)
            {
                spriteRenderer.sprite = openedSprite;
                IsOpened = true;
            }
            else
            {
                spriteRenderer.sprite = closedSprite;
                IsOpened = false;
            }
            
        }

        public void Interact(GameObject source)
        {
            if (source.GetComponent<ControllableCharacter>() != null)
            {
                
                if (IsOpened)
                {
                    Travel();
                    return;
                }
                
                if (CanOpen())
                {

                    if (String.IsNullOrEmpty(openText))
                    {
                        openText = "The door opened"; 
                    }
                    
                    spriteRenderer.sprite = openedSprite;
                    StartCoroutine(DisplayText(openSound, openText));
                    IsOpened = true;

                }
                else
                {
 
                    if (String.IsNullOrEmpty(closedText))
                    {
                        closedText = "It's closed.";
                    }
                    
                    StartCoroutine(DisplayText(closedSound, closedText));
                    
                }
            }
        }

        protected virtual bool CanOpen()
        {
            if (invokeIndex < 0 || invokeIndex > typeof(UnlockConditions).GetMethods().Length - 1) { return false; }
            return (bool)typeof(UnlockConditions).GetMethods()[invokeIndex].Invoke(this, new object[0]);
        }

        IEnumerator DisplayText(AudioClip clip, string text)
        {
            
            Beneath.data.player.DisableInput();
            
            if (clip)
            {
                audioSource.PlayOneShot(clip, 1.0f);
                yield return new WaitForSeconds(clip.length);
            }
            
            Beneath.data.DialogBox.OpenWithText(text);
        }

    }
    
#if UNITY_EDITOR
    [CustomEditor(typeof(DoorComponent))]
    [CanEditMultipleObjects]
    public class DoorComponentEditor : RoomLinkerEditor
    {
        
        private SerializedProperty _invokeIndex;

        protected override void OnEnable()
        {
            base.OnEnable();
            _invokeIndex = serializedObject.FindProperty("invokeIndex") ;
        }

        public override void OnInspectorGUI()
        {
            
            base.OnInspectorGUI();
            serializedObject.Update();

            MethodInfo[] methods = typeof(DoorComponent.UnlockConditions).GetMethods();
            
            // I use -4 here to remove the last 4 methods which are always default 
            // system ones, and does not interest us (ToString, Equals, GetHash...)
            string[] methodNames = new string[methods.Length - 4];

            for (int i = 0; i < methods.Length - 4; i++)
            {
                methodNames[i] = methods[i].DeclaringType.Name + "." + methods[i].Name;
            }
            
            _invokeIndex.intValue = EditorGUILayout.Popup("Unlock Condition", _invokeIndex.intValue, methodNames);
            
            serializedObject.ApplyModifiedProperties();

        }
        
    }
#endif
    
}