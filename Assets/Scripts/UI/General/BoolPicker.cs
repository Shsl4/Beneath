namespace UI.General
{
    public class BoolPicker : OptionPicker
    {
        public override string[] GetChoices()
        {
            return new [] {"NO", "YES"};
        }

        public bool BoolValue()
        {
            return Selected != 0;
        }
        
    }
}