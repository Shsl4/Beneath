using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public static partial class Beneath
{
    public static class SaveManager
    {
        public static void SaveProgress()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            SaveData progress = MakeSerializableProgress();
            string savePath = Application.persistentDataPath + "/beneath.data";
            FileStream stream = new FileStream(savePath, FileMode.Create);
            formatter.Serialize(stream, progress);
            stream.Close();
            instance.RestartStopwatch();
        }

        public static SaveData LoadProgress()
        {
            string savePath = Application.persistentDataPath + "/beneath.data";

            if (File.Exists(savePath))
            {
                BinaryFormatter formatter = new BinaryFormatter();

                try
                {

                    FileStream stream = new FileStream(savePath, FileMode.Open);

                    if (stream.CanRead && stream.Length > 0)
                    {
                        SaveData loadedData = formatter.Deserialize(stream) as SaveData;
                        stream.Close();
                        return loadedData;
                    }
                    
                }
                catch (IOException e)
                {
                    Console.WriteLine(e);
                }

            }
            return null;
        }

        public static SaveData MakeSerializableProgress()
        {

            int elapsedTime = 0;
            
            if (HasProgress())
            {
                elapsedTime = LoadProgress().playTime;
            }

            int armorID = instance.ArmorSlot.GetItem() != null ? instance.ArmorSlot.GetItem().id : -1;
            int weaponID = instance.WeaponSlot.GetItem() != null ? instance.WeaponSlot.GetItem().id : -1;
            
            return new SaveData(instance.PlayerName, SceneManager.GetActiveScene().name, instance.PlayerHealth, instance.PlayerExp, instance.PlayerGold, armorID, weaponID, instance.MakeSerializableInventory(), elapsedTime + instance.ElapsedSessionTime, instance.player.GetPosition());

        }
        
        public static bool HasProgress() { return LoadProgress() != null; }

        public static void ResumeGame()
        {
            instance.ResumeFromSave(LoadProgress());
        }

        public static void BeginGameWithName(string name)
        {
            instance.BeginWithName(name);
        }

    }
}