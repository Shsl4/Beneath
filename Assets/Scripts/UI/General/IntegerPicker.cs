namespace UI.General
{
    public class IntegerPicker : OptionPicker
    {

        public int maxInteger = 3;
        
        private string[] _ints;
        
        protected override void Awake()
        {
            base.Awake();
            _ints = new string[maxInteger];
        }

        public override string[] GetChoices()
        {
            for (int i = 0; i < maxInteger; i++)
            {
                _ints[i] = i.ToString();
            }

            return _ints;

        }
    }
}