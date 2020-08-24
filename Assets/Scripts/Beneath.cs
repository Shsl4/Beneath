using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Assets.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

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
    
    public class SaveData
    {

        public string PlayerName;
        public string RoomName;
        public int PlayerHealth;
        public int PlayerExp;
        public int PlayerMoney;
        public InventoryItem PlayerArmor;
        public InventoryItem PlayerWeapon;
        public int PlayTime;

    }

    public static class Assets
    {
        public static readonly AssetReference Item = new AssetReference("Assets/Prefabs/Items/Item.prefab");
        public static readonly AssetReference PlayerCharacter = new AssetReference("Assets/Prefabs/Characters/BeneathPlayer.prefab");
        public static readonly AssetReference DialogBox = new AssetReference("Assets/Prefabs/UI/DialogBox.prefab");
        public static readonly AssetReference EscapeMenu = new AssetReference("Assets/Prefabs/UI/EscapeMenu.prefab");
        public static readonly AssetReference SaveMenu = new AssetReference("Assets/Prefabs/UI/SaveMenu.prefab");
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
            string savePath = Application.persistentDataPath + "/beneath.data";
            FileStream stream = new FileStream(savePath, FileMode.Create);
            formatter.Serialize(stream, MakeSerializableProgress());
            stream.Close();
        }

        public static SaveData LoadProgress()
        {
            string savePath = Application.persistentDataPath + "/beneath.data";

            if (File.Exists(savePath))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(savePath, FileMode.Open);

                SaveData loadedData = formatter.Deserialize(stream) as SaveData;
                stream.Close();
                return loadedData;
            }
            return null;
        }

        public static SaveData MakeSerializableProgress()
        {
            return new SaveData();
        }
        
        public static bool HasProgress() { return LoadProgress() != null; }

        public static void ResumeGame()
        {

            if (HasProgress())
            {

                SaveData data = LoadProgress();
                SceneManager.LoadSceneAsync(data.RoomName).completed += operation =>
                {
                    
                };

            }
            else
            {
                SceneManager.LoadSceneAsync("Room 01").completed += operation =>
                {
                    Data.Spawn();
                };
                
            }
            
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