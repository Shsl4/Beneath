namespace Interfaces
{
    public interface IDialogBoxResponder
    {
        void DisplayWithText(string text, bool select);
        void DisplayEnded();
    }
}