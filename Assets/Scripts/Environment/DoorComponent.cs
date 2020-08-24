using System;
using System.Collections;
using Interfaces;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Environment
{

    [RequireComponent(typeof(AudioSource), typeof(SpriteRenderer))]
    public class DoorComponent : DialogComponent
    {

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

        public override void Interact(GameObject source)
        {
            if (source.GetComponent<ControllableCharacter>() != null)
            {
                
                ControllableCharacter character = source.GetComponent<ControllableCharacter>();

                if (IsOpened)
                {
/*
                    if (!travelToScene)
                    {
                        return;
                    }
                    
                    character.TravelToScene(travelToScene.name);
                    return; */
                }
                
                if (CanOpen())
                {

                    if (String.IsNullOrEmpty(textToDisplay))
                    {
                        textToDisplay = "The door opened"; 
                    }


                    spriteRenderer.sprite = openedSprite;
                    StartCoroutine(DisplayText(character, openSound, textToDisplay));
                    IsOpened = true;

                }
                else
                {
 
                    if (String.IsNullOrEmpty(closedText))
                    {
                        closedText = "It's closed.";
                    }
                    
                    StartCoroutine(DisplayText(character, closedSound, closedText));
                    
                }
            }
        }

        protected virtual bool CanOpen() { return false; }

        IEnumerator DisplayText(ControllableCharacter character, AudioClip clip, string text)
        {
            
            if (clip)
            {
                audioSource.PlayOneShot(clip, 1.0f);
                yield return new WaitForSeconds(clip.length);
            }
            
            character.OpenDialogBoxWithText(text);

        }

    }
}