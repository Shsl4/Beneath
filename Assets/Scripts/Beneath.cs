using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public static partial class Beneath
{

    public static DataHolder data;
    
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
    
    public static void LoadThen<T>(AssetReference asset, Action<AsyncOperationHandle<T>> action) { asset.LoadAssetAsync<T>().Completed += action; }
    public static void Load<T>(AssetReference asset) { asset.LoadAssetAsync<T>(); }

    public static void InstantiateSafeThen(AssetReference asset, Action<AsyncOperationHandle<GameObject>> action)
    {
        LoadThen<GameObject>(asset, handle => { asset.InstantiateAsync().Completed += action; } );
    }

    public static void DropItem(Vector2 location, int itemID)
    {
        InstantiateSafeThen(AssetReferences.Item, handle =>
        {
            
            GameObject newObject = handle.Result;
            newObject.SetActive(false);
            PickupItem itemComponent = newObject.GetComponent<PickupItem>();
            newObject.transform.position = location;
            itemComponent.RefreshWithID(itemID);
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

    public static void DebugLog(string text)
    {
        Debug.Log(text);
    }
    
    public static bool IsSubclassOfRawGeneric(Type generic, Type toCheck) {
        while (toCheck != null && toCheck != typeof(object)) {
            var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
            if (generic == cur) {
                return true;
            }
            toCheck = toCheck.BaseType;
        }
        return false;
    }
    
}