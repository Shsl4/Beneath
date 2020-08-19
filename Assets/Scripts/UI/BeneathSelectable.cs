using Interfaces.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public abstract class BeneathSelectable<T> : Selectable, ISelectableInterface where T : MasterInterface
    {
        public T Master => GetComponentInParent<T>();
        private void ApplyStyles()
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
        protected override void Reset()
        {
            base.Reset();
            ApplyStyles();
        }
        public abstract void OnSubmit(BaseEventData eventData);
        public abstract void OnCancel(BaseEventData eventData);
        
    }
}
