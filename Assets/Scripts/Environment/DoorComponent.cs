using System;
using System.Collections;
using Interfaces;
using UnityEditor;
using UnityEngine;

namespace Environment
{

    [RequireComponent(typeof(AudioSource), typeof(SpriteRenderer))]
    public class DoorComponent : RoomLinker, IInteractable
    {

        [TextArea] public string openText = "The door opened.";
        [TextArea] public string closedText;
        
        public bool openedByDefault;
        public Sprite openedSprite;
        public Sprite closedSprite;

        public AudioClip closedSound;
        public AudioClip openSound;

        public bool IsOpened { get; private set; }

        [HideInInspector] public SpriteRenderer spriteRenderer;
        [HideInInspector] public AudioSource audioSource;
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

        protected virtual bool CanOpen() { return true; }

        IEnumerator DisplayText(AudioClip clip, string text)
        {
            
            if (clip)
            {
                audioSource.PlayOneShot(clip, 1.0f);
                yield return new WaitForSeconds(clip.length);
            }
            
            Beneath.Data.DialogBox.OpenWithText(text);

        }

    }
    
    #if UNITY_EDITOR
    
    [CustomEditor(typeof(DoorComponent))]
    [CanEditMultipleObjects]
    public class DoorComponentEditor : RoomLinkerEditor
    {
    }
    #endif
}