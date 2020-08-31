using System.Diagnostics;
using UI;
using UI.EscapeMenu;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;

#endif

public class DataHolder : MonoBehaviour
{
    
    public string PlayerName { get; private set; }
    public int PlayerXP { get; private set; }        
    public int PlayerHealth { get; private set;}        
    public int PlayerMoney { get; private set; }        
    public int ElapsedSessionTime { get => _stopwatch.Elapsed.Seconds; }
    public InventorySlot PlayerArmor { get; } = new InventorySlot();
    public InventorySlot PlayerWeapon { get; } = new InventorySlot();
    public Inventory PlayerInventory { get; } = new Inventory(8);

    private EscapeMenuManager _escapeMenu;
    private DialogBoxManager _dialogBox;
    private Stopwatch _stopwatch;

    public DialogBoxManager DialogBox
    {
        get
        {
            if (_dialogBox) { return _dialogBox; }
            _dialogBox = Instantiate((GameObject)Beneath.Assets.DialogBox.Asset).GetComponent<DialogBoxManager>();
            return _dialogBox;
        }
    }

    public EscapeMenuManager EscapeMenu
    {
        get
        {

            if (_escapeMenu) { return _escapeMenu; }
            _escapeMenu = Instantiate((GameObject)Beneath.Assets.EscapeMenu.Asset).GetComponent<EscapeMenuManager>();
            return _escapeMenu;
                
        }
    }


    [HideInInspector]
    public ControllableCharacter player;
        
    protected void Awake()
    {
        
        DontDestroyOnLoad(gameObject);
        Beneath.Data = this;
        
        Beneath.Assets.Item.LoadAssetAsync<GameObject>();
        Beneath.Assets.DialogBox.LoadAssetAsync<GameObject>();
        Beneath.Assets.EscapeMenu.LoadAssetAsync<GameObject>();
        Beneath.Assets.PlayerCharacter.LoadAssetAsync<GameObject>();
        Beneath.Assets.SaveMenu.LoadAssetAsync<GameObject>();
        Beneath.Assets.ResumeMenu.LoadAssetAsync<GameObject>();

        SceneManager.LoadScene("MainMenu");
    }

    public void ResumeFromSave(Beneath.SaveData data)
    {
        
        PlayerName = data.playerName;
        PlayerArmor.SetItem(data.playerArmor);
        PlayerWeapon.SetItem(data.playerWeapon);
        PlayerHealth = data.playerHealth;
        PlayerName = data.playerName;
        PlayerMoney = data.playerMoney;
        PlayerXP = data.playerExp;

        for (int i = 0; i < data.playerInventory.Length; i++) { PlayerInventory.GetSlot(i).SetItem(data.playerInventory[i]); }

        SceneManager.LoadSceneAsync(data.roomName).completed += handle =>
        {
            Vector2 location = new Vector2(data.saveLocation[0], data.saveLocation[1]);
            player = Instantiate((GameObject) Beneath.Assets.PlayerCharacter.Asset, location, new Quaternion()).GetComponent<ControllableCharacter>();
            _stopwatch = Stopwatch.StartNew();
        };

    }

    public void BeginWithName(string newName)
    {
        
        PlayerName = newName;
        
        SceneManager.LoadSceneAsync("Room 01").completed += handle =>
        {
            player = Instantiate((GameObject)Beneath.Assets.PlayerCharacter.Asset).GetComponent<ControllableCharacter>();
            _stopwatch = Stopwatch.StartNew();
        };

    }

    public void RestartStopwatch()
    {
        _stopwatch.Restart();
    }

    public InventoryItem[] MakeSerializableInventory()
    {
        
        InventoryItem[] inventoryItems = new InventoryItem[8];

        for (int i =0; i < PlayerInventory.GetSlots().Length; i++)
        {
            inventoryItems[i] = PlayerInventory.GetSlots()[i].GetItem();
        }

        return inventoryItems;
        
    }
    
#if UNITY_EDITOR
    [CustomEditor(typeof(DataHolder))]
    [CanEditMultipleObjects]
    public class DataHolderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Cook AddressableAssets"))
            {
                AddressableAssetSettings.BuildPlayerContent();
            }
        }
    }
#endif

    
}