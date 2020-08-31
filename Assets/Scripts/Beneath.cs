using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public static class Beneath
{

    public static DataHolder Data;
    
    public enum EquipResult
    {
        
        Success,
        AlreadyEquipped,
        Error
        
    }    
    
    public enum UnEquipResult
    {
        
        Success,
        InventoryFull,
        Error
        
    }
    
    [Serializable]
    public class SaveData
    {

        public string playerName;
        public string roomName;
        public int playerHealth;
        public int playerExp;
        public int playerMoney;
        public InventoryItem playerArmor;
        public InventoryItem playerWeapon;
        public InventoryItem[] playerInventory;
        public int playTime;
        public float[] saveLocation;

        public SaveData(string playerName, string roomName, int playerHealth, int playerExp, int playerMoney, InventoryItem playerArmor, InventoryItem playerWeapon, InventoryItem[] playerInventory, int playTime, Vector2 saveLocation)
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

    public static class Assets
    {
        public static readonly AssetReference Item = new AssetReference("Assets/Prefabs/Items/Item.prefab");
        public static readonly AssetReference PlayerCharacter = new AssetReference("Assets/Prefabs/Characters/BeneathPlayer.prefab");
        public static readonly AssetReference DialogBox = new AssetReference("Assets/Prefabs/UI/DialogBox.prefab");
        public static readonly AssetReference EscapeMenu = new AssetReference("Assets/Prefabs/UI/EscapeMenu.prefab");
        public static readonly AssetReference SaveMenu = new AssetReference("Assets/Prefabs/UI/SaveMenu.prefab");
        public static readonly AssetReference ResumeMenu = new AssetReference("Assets/Prefabs/UI/ResumeMenu.prefab");
    }

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
            Data.RestartStopwatch();
        }

        public static SaveData LoadProgress()
        {
            string savePath = Application.persistentDataPath + "/beneath.data";

            if (File.Exists(savePath))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(savePath, FileMode.Open);

                if (stream.CanRead && stream.Length > 0)
                {
                    SaveData loadedData = formatter.Deserialize(stream) as SaveData;
                    stream.Close();
                    return loadedData;
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

            return new SaveData(Data.PlayerName, SceneManager.GetActiveScene().name, Data.PlayerHealth, Data.PlayerXP, Data.PlayerMoney, Data.PlayerArmor.GetItem(), Data.PlayerWeapon.GetItem(), new InventoryItem[0], elapsedTime + Data.ElapsedSessionTime, Data.player.GetPosition());

        }
        
        public static bool HasProgress() { return LoadProgress() != null; }

        public static void ResumeGame()
        {
            Data.ResumeFromSave(LoadProgress());
        }

        public static void BeginGameWithName(string name)
        {
            Data.BeginWithName(name);
        }

    }
    
    public static void LoadThen<T>(AssetReference asset, Action<AsyncOperationHandle<T>> action) { asset.LoadAssetAsync<T>().Completed += action; }
    public static void Load<T>(AssetReference asset) { asset.LoadAssetAsync<T>(); }

    public static void InstantiateSafeThen(AssetReference asset, Action<AsyncOperationHandle<GameObject>> action)
    {
        LoadThen<GameObject>(asset, handle => { asset.InstantiateAsync().Completed += action; } );
    }

    public static void DropItem(Vector2 location, InventoryItem item)
    {
        InstantiateSafeThen(Assets.Item, handle =>
        {
            
            GameObject newObject = handle.Result;
            newObject.SetActive(false);
            PickupItem itemComponent = newObject.GetComponent<PickupItem>();
            newObject.transform.position = location;
            itemComponent.FromInventoryItem(item);
            newObject.SetActive(true);

        });
        
    }

    public static void DelayThen(MonoBehaviour caller, float delay, Action action)
    {
        caller.StartCoroutine(DelayAndInvoke(delay, action));
    }
    private static IEnumerator DelayAndInvoke(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action.Invoke();
    }
    
    public static void DelayOneFrameThen(MonoBehaviour caller, Action action)
    {
        caller.StartCoroutine(DelayOneFrameAndInvoke(action));
    }

    private static IEnumerator DelayOneFrameAndInvoke(Action action)
    {
        yield return new WaitForEndOfFrame();
        action.Invoke();
    }

    public static void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    
}