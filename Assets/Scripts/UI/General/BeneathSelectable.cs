﻿using Interfaces.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.General
{
    public abstract class BeneathSelectable<T> : Selectable, IBeneathSelectable, IBeneathUIComponent where T : UIManager
    {
        protected T Manager => GetComponentsInParent<T>(true)[0];
        public AudioClip selectSound;
        public AudioClip submitSound;

        protected virtual void ApplyStyles()
        {
            
            ColorBlock customColors = new ColorBlock();
            Navigation nav = new Navigation {mode = Navigation.Mode.Explicit};
            customColors.normalColor = Color.white;
            customColors.pressedColor = Color.white;
            customColors.disabledColor = Color.gray;
            customColors.selectedColor = Color.yellow;
            customColors.highlightedColor = Color.yellow;
            customColors.colorMultiplier = 1.0f;
            customColors.fadeDuration = 0.1f;
            colors = customColors;
            navigation = nav;
            
        }
        
#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            ApplyStyles();
        }
#endif
        
        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            if (selectSound)
            {
                Manager.source.PlayOneShot(selectSound);
            }
        }

        public abstract void OnSubmit(BaseEventData eventData);
        public abstract void OnCancel(BaseEventData eventData);

        public UIManager GetManager()
        {
            return Manager;
        }
    }
}
