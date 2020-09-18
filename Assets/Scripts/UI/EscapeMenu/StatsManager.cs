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

            var hasWeapon = Beneath.instance.WeaponSlot.HasItem();
            var hasArmor = Beneath.instance.ArmorSlot.HasItem();
            
            nameText.text = "\"" + Beneath.instance.PlayerName + "\"";
            lvText.text = "LV: " + Beneath.instance.PlayerLevel;
            hpText.text = "HP: " + Beneath.instance.PlayerHealth + " / " + Beneath.instance.PlayerMaxHealth;
            
            attackText.text = "AT: " + (hasWeapon ? Beneath.instance.WeaponSlot.GetItem().GetAttribute<DamageAttribute>().DamageAmount : 0);
            defenseText.text = "DF: " + (hasArmor ? Beneath.instance.ArmorSlot.GetItem().GetAttribute<DefenseAttribute>().DefenseAmount : 0);
            
            expText.text = "EXP: " + Beneath.instance.PlayerExp;
            nextText.text = "NEXT: " + Beneath.instance.ExpBeforeLevelUp;
            
            weaponText.text = "WEAPON: " + (hasWeapon ? Beneath.instance.WeaponSlot.GetItem().name : "NONE");
            armorText.text = "ARMOR: " + (hasArmor ? Beneath.instance.ArmorSlot.GetItem().name : "NONE");
            
            goldText.text = "GOLD: " + Beneath.instance.PlayerGold;
        }
        
    }
}
