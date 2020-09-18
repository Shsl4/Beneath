using System.Diagnostics;
using UI;
using UI.EscapeMenu;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;

#endif

[RequireComponent(typeof(CameraFader))]
public class BeneathInstance : MonoBehaviour
{
    
    public string PlayerName { get; private set; }
    public int PlayerExp { get; private set; }        
    public int PlayerHealth { get; private set;}
    public int PlayerGold { get; private set; }    
    public CameraFader Fader { get; private set; }
    public int PlayerMaxHealth => DetermineMaxHealth();
    public int PlayerLevel => DetermineLevel();
    public int ExpBeforeLevelUp => DetermineExpBeforeLevelUp();
    public int ElapsedSessionTime => _stopwatch.Elapsed.Seconds;
    
    public InventorySlot ArmorSlot { get; } = new InventorySlot();
    public InventorySlot WeaponSlot { get; } = new InventorySlot();
    public Inventory PlayerInventory { get; } = new Inventory(8);
    
    private int DetermineMaxHealth() { return 20 + 2 * PlayerLevel; }
    private int DetermineExpBeforeLevelUp() { return 15 + PlayerExp / 3; }
    private int DetermineLevel() { return 0; }

    private EscapeMenuManager _escapeMenu;
    private DialogBoxManager _dialogBox;
    private Stopwatch _stopwatch;

    public DialogBoxManager DialogBox
    {
        get
        {
            if (_dialogBox) { return _dialogBox; }
            _dialogBox = Instantiate((GameObject)Beneath.AssetReferences.DialogBox.Asset).GetComponent<DialogBoxManager>();
            return _dialogBox;
        }
    }

    public EscapeMenuManager EscapeMenu
    {
        get
        {

            if (_escapeMenu) { return _escapeMenu; }
            _escapeMenu = Instantiate((GameObject)Beneath.AssetReferences.EscapeMenu.Asset).GetComponent<EscapeMenuManager>();
            return _escapeMenu;
                
        }
    }


    [HideInInspector]
    public ControllableCharacter player;
        
    protected void Awake()
    {
        
        DontDestroyOnLoad(gameObject);

        Fader = GetComponent<CameraFader>();
        Beneath.instance = this;
        
        Beneath.AssetReferences.Item.LoadAssetAsync<GameObject>();
        Beneath.AssetReferences.DialogBox.LoadAssetAsync<GameObject>();
        Beneath.AssetReferences.EscapeMenu.LoadAssetAsync<GameObject>();
        Beneath.AssetReferences.PlayerCharacter.LoadAssetAsync<GameObject>();
        Beneath.AssetReferences.SaveMenu.LoadAssetAsync<GameObject>();
        Beneath.AssetReferences.ResumeMenu.LoadAssetAsync<GameObject>();
        Beneath.AssetReferences.NullKeySprite.LoadAssetAsync<Sprite>();

        Beneath.Items.GetDefinedItemsCount();
        
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
        ArmorSlot.SetItem(Beneath.Items.GetItemWithID(data.playerArmor));
        WeaponSlot.SetItem(Beneath.Items.GetItemWithID(data.playerWeapon));
        PlayerHealth = data.playerHealth;
        PlayerName = data.playerName;
        PlayerGold = data.playerMoney;
        PlayerExp = data.playerExp;

        for (int i = 0; i < data.playerInventory.Length; i++) { PlayerInventory.GetSlot(i).SetItem(Beneath.Items.GetItemWithID(data.playerInventory[i])); }

        SceneManager.LoadSceneAsync(data.roomName).completed += handle =>
        {
            Vector2 location = new Vector2(data.saveLocation[0], data.saveLocation[1]);
            player = Instantiate((GameObject) Beneath.AssetReferences.PlayerCharacter.Asset, location, new Quaternion()).GetComponent<ControllableCharacter>();
            _stopwatch = Stopwatch.StartNew();
        };

    }

    public void BeginWithName(string newName)
    {
        
        PlayerName = newName;
        PlayerHealth = PlayerMaxHealth;
        SceneManager.LoadSceneAsync("Dev Lab - Main Room").completed += handle =>
        {
            player = Instantiate((GameObject)Beneath.AssetReferences.PlayerCharacter.Asset).GetComponent<ControllableCharacter>();
            Fader.fadeColor = Color.black;
            Fader.FadeOut(5.0f);
            _stopwatch = Stopwatch.StartNew();
        };

    }

    public void RestartStopwatch()
    {
        _stopwatch.Restart();
    }

    public int[] MakeSerializableInventory()
    {
        
        int[] itemIDs = new int[8];

        for (int i =0; i < PlayerInventory.GetSlots().Length; i++)
        {
            var item = PlayerInventory.GetSlots()[i].GetItem();
            itemIDs[i] = item?.id ?? -1;
        }

        return itemIDs;
        
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
    [CustomEditor(typeof(BeneathInstance))]
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