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
    public int PlayerXp { get; private set; }        
    public int PlayerHealth { get; private set;}        
    public int PlayerMoney { get; private set; }        
    public int ElapsedSessionTime => _stopwatch.Elapsed.Seconds;
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

        if (!PlayerPrefs.HasKey("masterVolume"))
        {
            PlayerPrefs.SetFloat("masterVolume", 1.0f);
        }

        AudioListener.volume = PlayerPrefs.GetFloat("masterVolume");

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
        PlayerXp = data.playerExp;

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
    
    /*
     *     public int Damage(int damageAmount, GameObject source)
    {

        int inflictedDamage = 0;

        if (CanBeDamaged())
        {
            if (_health - damageAmount <= 0)
            {
                inflictedDamage = damageAmount - _health;
                _health = 0;
                OnDeath();
            }
            else
            {
                _health -= damageAmount;
                inflictedDamage = damageAmount;
            }
        }

        return inflictedDamage;

    }

    public int Heal(int healAmount, GameObject source)
    {

        int givenHealth;

        if (_health + healAmount >= _maxHealth)
        {
            givenHealth = _health - healAmount;
            _health = _maxHealth;
        }
        else
        {
            _health += healAmount;
            givenHealth = healAmount;
        }

        return givenHealth;

    }
     */
    
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