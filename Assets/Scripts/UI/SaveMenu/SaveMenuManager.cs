using Dummies;
using UI;

namespace Assets.Scripts.UI.SaveMenu
{
    public class SaveMenuManager : UIManager
    {

        public SaveButton SaveBtn => GetComponentInChildren<SaveButton>();
        public BackDummy Return => GetComponentInChildren<BackDummy>();
        public void OnSaved()
        {
            
        }
    }
}
