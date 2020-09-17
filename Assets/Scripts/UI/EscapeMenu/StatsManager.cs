using Attributes;
using TMPro;
using UI.General;

namespace UI.EscapeMenu
{
    public class StatsManager : UIManager
    {

        public TMP_Text nameText;
        public TMP_Text lvText;
        public TMP_Text hpText;
        public TMP_Text attackText;
        public TMP_Text defenseText;
        public TMP_Text expText;
        public TMP_Text nextText;
        public TMP_Text weaponText;
        public TMP_Text armorText;
        public TMP_Text goldText;

        public override void Open()
        {
            base.Open();
            Refresh();
        }

        private void Refresh()
        {

            var hasWeapon = Beneath.data.WeaponSlot.HasItem();
            var hasArmor = Beneath.data.ArmorSlot.HasItem();
            
            nameText.text = "\"" + Beneath.data.PlayerName + "\"";
            lvText.text = "LV: " + Beneath.data.PlayerLevel;
            hpText.text = "HP: " + Beneath.data.PlayerHealth + " / " + Beneath.data.PlayerMaxHealth;
            
            attackText.text = "AT: " + (hasWeapon ? Beneath.data.WeaponSlot.GetItem().GetAttribute<DamageAttribute>().DamageAmount : 0);
            defenseText.text = "DF: " + (hasArmor ? Beneath.data.ArmorSlot.GetItem().GetAttribute<DefenseAttribute>().DefenseAmount : 0);
            
            expText.text = "EXP: " + Beneath.data.PlayerExp;
            nextText.text = "NEXT: " + Beneath.data.ExpBeforeLevelUp;
            
            weaponText.text = "WEAPON: " + (hasWeapon ? Beneath.data.WeaponSlot.GetItem().name : "NONE");
            armorText.text = "ARMOR: " + (hasArmor ? Beneath.data.ArmorSlot.GetItem().name : "NONE");
            
            goldText.text = "GOLD: " + Beneath.data.PlayerGold;
        }
        
    }
}
