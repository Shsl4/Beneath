using System.Collections;
using Attributes;
using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ControllableCharacter : Character, IInventory
{
    
    public float characterSpeed = 1.0f;
    public float characterRange = 1.5f;

    private float _horizontalInput;
    private float _verticalInput;
    private Vector2 _lookDirection = new Vector2(1, 0);
    private bool _immune;
    private int _health;
    private int _maxHealth;
    
    private Camera _camera;
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _renderer;
    private BoxCollider2D _collider;
    private PlayerInput _playerInput;
    private Animator _animator;
    private ConfinerShapeFinder _confinerShapeFinder;
    private static readonly int XInput = Animator.StringToHash("XInput");
    private static readonly int YInput = Animator.StringToHash("YInput");
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");

    private static Inventory PlayerInventory => Beneath.instance.PlayerInventory;
    private static InventorySlot CharacterArmor => Beneath.instance.ArmorSlot;
    private static InventorySlot CharacterWeapon => Beneath.instance.WeaponSlot;
    
    public Vector2 GetPosition() { return _rigidbody.position; }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        _camera = GetComponent<Camera>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<BoxCollider2D>();
        _playerInput = GetComponent<PlayerInput>();
        _animator = GetComponent<Animator>();
        _confinerShapeFinder = GetComponentInChildren<ConfinerShapeFinder>(true);
    }
    
    public void Update()
    {
        
        Vector2 move = new Vector2(_horizontalInput, _verticalInput);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            _animator.SetBool(IsWalking, true);
            _animator.SetFloat(XInput, move.x);
            _animator.SetFloat(YInput, move.y);
            _lookDirection.Set(move.x, move.y);
            _lookDirection.Normalize();
        }
        else
        {
            _animator.SetBool(IsWalking, false);
        }
        
    }


    void FixedUpdate()
    {
        if (_playerInput.inputIsActive)
        {
            UpdatePosition();
        }
    }
    
    private void UpdatePosition()
    {
        Vector2 position = _rigidbody.position;
        position.x += 10.0f * _horizontalInput * Time.deltaTime * characterSpeed;
        position.y += 10.0f * _verticalInput * Time.deltaTime * characterSpeed;
        _rigidbody.MovePosition(position);
    }
    
    public int GetCharacterDamage()
    {

        int characterDamage = 5;

        if (CharacterWeapon.GetItem() != null)
        {
            foreach (var attribute in CharacterWeapon.GetItem().attributes)
            {
                if (attribute is DamageAttribute damageAttribute)
                {
                    characterDamage = damageAttribute.damageAmount;
                }
            }
        }

        if (CharacterArmor.GetItem() != null)
        {
            foreach (var attribute in CharacterArmor.GetItem().attributes)
            {
                if (attribute is DamageMultiplierAttribute modifier)
                {
                    if (modifier.multiplier > 0.0f)
                    {
                        characterDamage = Mathf.RoundToInt(characterDamage * modifier.multiplier);
                    }
                }
            }
        }

        return characterDamage;

    }

    public void EnableInput() { _playerInput.enabled = true;}
    public void DisableInput() { _playerInput.enabled = false;}
    
    public void DropItemFromSlot(int index)
    {

        if (PlayerInventory.GetSlot(index) != null)
        {
            Beneath.DropItem(_rigidbody.position, PlayerInventory.GetSlot(index).GetItem().id);
            PlayerInventory.GetSlot(index).Clear();
        }
    }

    public void ClearItemFromSlot(int index)
    {
        if (PlayerInventory.GetSlot(index) != null)
        {
            PlayerInventory.GetSlot(index).Clear();
        }
    }

    public Beneath.EquipResult EquipWeapon(int index)
    {

        if (Beneath.instance.PlayerInventory.GetSlot(index).GetItem() != null &&
            Beneath.instance.PlayerInventory.GetSlot(index).GetItem().type == ItemTypes.Weapon)
        {

            if (CharacterWeapon.GetItem() == null)
            {
                CharacterWeapon.SetItem(Beneath.instance.PlayerInventory.GetSlot(index).GetItem());
                ClearItemFromSlot(index);
                return Beneath.EquipResult.Success;
            }

            return Beneath.EquipResult.AlreadyEquipped;

        }

        return Beneath.EquipResult.Error;
        
    }

    public Beneath.EquipResult EquipArmor(int index)
    {

        if (Beneath.instance.PlayerInventory.GetSlot(index).GetItem() != null &&
            Beneath.instance.PlayerInventory.GetSlot(index).GetItem().type == ItemTypes.Armor)
        {

            if (CharacterArmor.GetItem() == null)
            {
                CharacterArmor.SetItem(Beneath.instance.PlayerInventory.GetSlot(index).GetItem());
                ClearItemFromSlot(index);
                return Beneath.EquipResult.Success;
            }

            return Beneath.EquipResult.AlreadyEquipped;

        }

        return Beneath.EquipResult.Error;
    }

    public Beneath.UnEquipResult UnEquipArmor(bool discard)
    {

        if (CharacterArmor.GetItem() == null)
        {
            return Beneath.UnEquipResult.Error;
        }

        if (discard)
        {
            Beneath.DropItem(_rigidbody.position, CharacterArmor.GetItem().id);
            CharacterArmor.Clear();
            return Beneath.UnEquipResult.Success;
        }

        if (PlayerInventory.IsFull())
        {
            return Beneath.UnEquipResult.InventoryFull;
        }

        PlayerInventory.GetNextEmptySlot().SetItem(CharacterArmor.GetItem());
        CharacterArmor.Clear();
        return Beneath.UnEquipResult.Success;

    }

    public Beneath.UnEquipResult UnEquipWeapon(bool discard)
    {

        if (CharacterWeapon.GetItem() == null)
        {
            return Beneath.UnEquipResult.Error;
        }

        if (discard)
        {
            Beneath.DropItem(_rigidbody.position, CharacterWeapon.GetItem().id);
            CharacterWeapon.Clear();
            return Beneath.UnEquipResult.Success;
        }

        if (PlayerInventory.IsFull())
        {
            return Beneath.UnEquipResult.InventoryFull;
        }

        PlayerInventory.GetNextEmptySlot().SetItem(CharacterWeapon.GetItem());
        CharacterWeapon.Clear();
        return Beneath.UnEquipResult.Success;

    }

    public Inventory GetInventory() { return Beneath.instance.PlayerInventory; }

    public bool PickupItem(ItemData itemData)
    {

        bool result = false;
        string message;

        if (!Beneath.instance.PlayerInventory.IsFull())
        {
            if (Beneath.instance.PlayerInventory.GetNextEmptySlot().SetItem(itemData))
            {
                message = "You picked up \"" + itemData.name + "\".";
                result = true;
            }
            else
            {
                message = "You tried to pick up \"" + itemData.name + "\", but it failed.";
            }
        }
        else
        {
            message = "You tried to pick up \"" + itemData.name + "\", but your inventory was full.";
        }

        Beneath.instance.DialogBox.OpenWithText(message);

        return result;
    }

    public void OnInteract()
    {
        
        Vector2 start = _rigidbody.position + Vector2.up * 0.2f;
        RaycastHit2D hit = Physics2D.Raycast(start, _lookDirection, characterRange, LayerMask.GetMask("Interaction"));
        
        if (hit.collider != null && hit.collider.gameObject.GetComponent<IInteractable>() != null)
        {
            IInteractable interactable = hit.collider.gameObject.GetComponent<IInteractable>();
            interactable.Interact(gameObject);
        }

    }

    public void OnMove(InputValue value)
    {

        Vector2 val = value.Get<Vector2>();
        _horizontalInput = val.x;
        _verticalInput = val.y;

    }

    public void OnEscape()
    {
        Beneath.instance.EscapeMenu.Open();
    }
    
    public void TravelToSceneAtLocation(string scene, Vector2 location)
    {
        StartCoroutine(TravelToScene(scene, location));
    }

    private void OnSceneTravelled(Vector2 toLocation)
    {
        _rigidbody.position = toLocation;
        Beneath.instance.Fader.FadeOut(0.25f);
        _confinerShapeFinder.Refresh();
    }

    private IEnumerator TravelToScene(string toScene, Vector2 toLocation)
    {
        Beneath.instance.Fader.FadeIn(0.25f);
        yield return new WaitForSeconds(0.25f);
        SceneManager.LoadSceneAsync(toScene).completed += operation =>
        {
            OnSceneTravelled(toLocation);
        };

    }
}