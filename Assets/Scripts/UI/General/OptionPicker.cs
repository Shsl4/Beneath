using UnityEditor;
using UnityEngine.EventSystems;

namespace UI.General
{
    public abstract class OptionPicker : BeneathButton<UIManager>
    {
        public int Selected { get; private set; }
        
        public abstract string[] GetChoices();

        protected override void Awake()
        {
            base.Awake();
            allowBackNavigation = true;
            Refresh();
        }
        
        public void SetSelected(int index)
        {
            if (index >= 0 && index < GetChoices().Length)
            {
                Selected = index;
                Refresh();
            }
        }

        public string GetSelectedValue()
        {
            if (GetChoices().Length > 0 && Selected >= 0 && Selected < GetChoices().Length)
            {
                return GetChoices()[Selected];
            }

            return "OOB";
        }
        
        public override void OnMove(AxisEventData eventData)
        {
            switch (eventData.moveDir)
            {
                case MoveDirection.Right:

                    if (selectSound)
                    {
                        Manager.source.PlayOneShot(selectSound);
                    }
                    
                    if (Selected + 1 >= GetChoices().Length)
                    {
                        Selected = 0;
                    }
                    else
                    {
                        Selected++;
                    }
                    
                    Refresh();
                    break;
                
                case MoveDirection.Left:
                    
                    if (selectSound)
                    {
                        Manager.source.PlayOneShot(selectSound);
                    }
                    
                    if (Selected - 1 < 0)
                    {
                        Selected = GetChoices().Length - 1;
                    }
                    else
                    {
                        Selected--;
                    }
                    Refresh();
                    
                    break;
            }
            
        }

        private void Refresh()
        {
            TextBox.text = "< " + GetSelectedValue() + " >";
        }
        
    }
    
#if UNITY_EDITOR

    [CustomEditor(typeof(OptionPicker), true)]
    [CanEditMultipleObjects]
    public class OptionPickerEditor : BeneathButtonEditor
    {

        OptionPickerEditor()
        {
            ShowDefaultNavigation = false;
        }
        
    }
#endif
    
}