using TMPro;

public static partial class Beneath
{
    public static class TextHelpers
    {

        public static void SetIdealPointSize(TMP_Text textBox, int lineCount)
        {
            
            string test = "";
            for (int i = 0; i < lineCount; i++)
            {
                test += "a\n";
            }

            textBox.text = test;
            textBox.enableAutoSizing = true;
            textBox.ForceMeshUpdate(true);
            float fontSize = textBox.fontSize;
            textBox.enableAutoSizing = false;
            textBox.fontSize = fontSize;
            textBox.text = "";
            textBox.ForceMeshUpdate(true);
            
        }
        
    }
}