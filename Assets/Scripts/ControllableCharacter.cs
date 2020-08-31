using System;
using System.Collections;
using Attributes;
using Environment;
using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ControllableCharacter : Character, IDamageable, IInventory
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
    private CameraFader _fader;
    private Animator _animator;
    private ConfinerShapeFinder _confinerShapeFinder;
    private static readonly int XInput = Animator.StringToHash("XInput");
    private static readonly int YInput = Animator.StringToHash("YInput");
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");

    private static Inventory PlayerInventory => Beneath.Data.PlayerInventory;
    private static InventorySlot CharacterArmor => Beneath.Data.PlayerArmor;
    private static InventorySlot CharacterWeapon => Beneath.Data.PlayerWeapon;
    
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
        _fader = GetComponent<CameraFader>();
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
            foreach (var attribute in CharacterWeapon.GetItem().Attributes)
            {
                if (attribute is DamageAttribute damageAttribute)
                {
                    characterDamage = damageAttribute.DamageAmount;
                }
            }
        }

        if (CharacterArmor.GetItem() != null)
        {
            foreach (var attribute in CharacterArmor.GetItem().Attributes)
            {
                if (attribute is DamageModifierAttribute modifier)
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
    
    public bool CanBeDamaged()
    {
        return _immune;
    }

    public int GetHealth()
    {
        return _health;
    }

    public int GetMaxHealth()
    {
        return _maxHealth;
    }

    public void DropItemFromSlot(int index)
    {

        if (PlayerInventory.GetSlot(index) != null)
        {
            Beneath.DropItem(_rigidbody.position, PlayerInventory.GetSlot(index).GetItem());
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

        if (Beneath.Data.PlayerInventory.GetSlot(index).GetItem() != null &&
            Beneath.Data.PlayerInventory.GetSlot(index).GetItem().type == ItemTypes.Weapon)
        {

            if (CharacterWeapon.GetItem() == null)
            {
                CharacterWeapon.SetItem(Beneath.Data.PlayerInventory.GetSlot(index).GetItem());
                ClearItemFromSlot(index);
                return Beneath.EquipResult.Success;
            }

            return Beneath.EquipResult.AlreadyEquipped;

        }

        return Beneath.EquipResult.Error;
        
    }

    public Beneath.EquipResult EquipArmor(int index)
    {

        if (Beneath.Data.PlayerInventory.GetSlot(index).GetItem() != null &&
            Beneath.Data.PlayerInventory.GetSlot(index).GetItem().type == ItemTypes.Armor)
        {

            if (CharacterArmor.GetItem() == null)
            {
                CharacterArmor.SetItem(Beneath.Data.PlayerInventory.GetSlot(index).GetItem());
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
            Beneath.DropItem(_rigidbody.position, CharacterArmor.GetItem());
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
            Beneath.DropItem(_rigidbody.position, CharacterWeapon.GetItem());
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

    public Inventory GetInventory() { return Beneath.Data.PlayerInventory; }

    public bool PickupItem(InventoryItem item)
    {

        bool result = false;
        string message;

        if (!Beneath.Data.PlayerInventory.IsFull())
        {
            if (Beneath.Data.PlayerInventory.GetNextEmptySlot().SetItem(item))
            {
                message = "You picked up \"" + item.name + "\".";
                result = true;
            }
            else
            {
                message = "You tried to pick up \"" + item.name + "\", but it failed.";
            }
        }
        else
        {
            message = "You tried to pick up \"" + item.name + "\", but your inventory was full.";
        }

        Beneath.Data.DialogBox.OpenWithText(message);

        return result;
    }

    public void OnInteract()
    {
        
        Vector2 start = _rigidbody.position + Vector2.up * 0.2f;
        RaycastHit2D hit = Physics2D.Raycast(start, _lookDirection, characterRange, LayerMask.GetMask("Interaction"));

        Debug.DrawRay(new Vector3(start.x, start.y, 0),
            new Vector3(_lookDirection.x, _lookDirection.y, 0) * characterRange, Color.red, 2.0f);

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
        Beneath.Data.EscapeMenu.Open();
    }

    public int Damage(int damageAmount, GameObject source)
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

    private void OnDeath()
    {


    }

    public void TravelToScene(string scene)
    {
        TravelToSceneAtLocation(scene, new Vector2(0, 0));
    }
    
    public void TravelToSceneAtLocation(string scene, Vector2 location)
    {
        StartCoroutine(TravelToScene(scene, location));
    }

    private void OnSceneTravelled(string fromScene, string toScene, Vector2 fromLocation, Vector2 toLocation)
    {
        _rigidbody.position = toLocation;
        //FindObjectOfType<RoomLinker>()?.OnRoomEntered(fromScene, fromLocation);
        _fader.FadeOut(0.25f);
        _confinerShapeFinder.Refresh();
    }

    private IEnumerator TravelToScene(string toScene, Vector2 toLocation)
    {
        DontDestroyOnLoad(gameObject);
        _fader.FadeIn(0.25f);
        String fromScene = SceneManager.GetActiveScene().name;
        yield return new WaitForSeconds(0.25f);
        SceneManager.LoadSceneAsync(toScene).completed += operation =>
        {
            var location = _rigidbody.position;
            Vector2 entry = new Vector2(location.x, location.y);
            OnSceneTravelled(fromScene,toScene, entry, toLocation);
        };

    }
}