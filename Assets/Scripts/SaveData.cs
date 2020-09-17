using System;
using UnityEngine;

public static partial class Beneath
{
    [Serializable]
    public class SaveData
    {
        
        public string roomName;
        public int playTime;
        public float[] saveLocation;
        
        public string playerName;
        public int playerHealth;
        public int playerExp;
        public int playerMoney;
        public int playerArmor;
        public int playerWeapon;
        public int[] playerInventory;
        


        public SaveData(string playerName, string roomName, int playerHealth, int playerExp, int playerMoney, int playerArmor, int playerWeapon, int[] playerInventory, int playTime, Vector2 saveLocation)
        {
            this.playerName = playerName;
            this.roomName = roomName;
            this.playerHealth = playerHealth;
            this.playerExp = playerExp;
            this.playerMoney = playerMoney;
            this.playerArmor = playerArmor;
            this.playerWeapon = playerWeapon;
            this.playerInventory = playerInventory;
            this.playTime = playTime;
            this.saveLocation = new []{saveLocation.x, saveLocation.y};
        }
    }
}