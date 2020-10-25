using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public static partial class Beneath
{
    [Serializable]
    public class SaveData
    {
        
        public int roomIndex;
        public int playTime;
        public float[] saveLocation;
        public string playerName;
        public int playerHealth;
        public int playerExp;
        public int playerMoney;
        public int playerArmor;
        public int playerWeapon;
        public int[] playerInventory;

        public SaveData(string playerName, int roomIndex, int playerHealth, int playerExp, int playerMoney, int playerArmor, int playerWeapon, int[] playerInventory, int playTime, Vector2 saveLocation)
        {
            this.playerName = playerName;
            this.roomIndex = roomIndex;
            this.playerHealth = playerHealth;
            this.playerExp = playerExp;
            this.playerMoney = playerMoney;
            this.playerArmor = playerArmor;
            this.playerWeapon = playerWeapon;
            this.playerInventory = playerInventory;
            this.playTime = playTime;
            this.saveLocation = new []{saveLocation.x, saveLocation.y};
        }

        public string GetSavedRoomName()
        {
            return SceneUtility.GetScenePathByBuildIndex(roomIndex) != "" ? Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(roomIndex)) : "Room " + roomIndex;
        }
        
    }
}