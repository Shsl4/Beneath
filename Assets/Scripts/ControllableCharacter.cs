using Interfaces;
using Interfaces.UI;
using UI;
using UI.Inventory;
using UI.Menu;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ControllableCharacter : Character
{
    public float characterSpeed = 1.0f;
    public float characterRange = 1.5f;
        
    private float _horizontalInput;
    private float _verticalInput;
    private Vector2 _lookDirection = new Vector2(1,0);
    private MenuManager _menuManager;
    private MasterInterface _activeInterface;
    
    public override void Update() 
    {
        
        base.Update();

        Vector2 move = new Vector2(_horizontalInput, _verticalInput);
            
        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            _lookDirection.Set(move.x, move.y);
            _lookDirection.Normalize();
        }

    }

    public void OnInteract()
    {
            
        Vector2 start = RigidBody.position + Vector2.up * 0.2f;
        RaycastHit2D hit = Physics2D.Raycast(start, _lookDirection, characterRange, LayerMask.GetMask("Interaction"));
            
        Debug.DrawRay(new Vector3(start.x, start.y, 0), new Vector3(_lookDirection.x, _lookDirection.y, 0) * characterRange, Color.red, 2.0f);

        if (hit.collider != null && hit.collider.gameObject.GetComponent<IInteractable>() != null)
        {
            IInteractable interactable = hit.collider.gameObject.GetComponent<IInteractable>();
            interactable.Interact(gameObject);
        }
            
    }

    public void OnFire()
    {
            
        Vector2 start = RigidBody.position /*+ Vector2.up * 0.2f*/;
        RaycastHit2D[] hits = Physics2D.RaycastAll(start, _lookDirection, characterRange);
        Debug.DrawRay(new Vector3(start.x, start.y, 0), new Vector3(_lookDirection.x, _lookDirection.y, 0) * characterRange, Color.green, 2.0f);
            
        foreach (var hit in hits)
        {
            if (hit.collider.gameObject.Equals(gameObject)) { }
            else
            {
                if (hit.collider.gameObject.GetComponent<DamageableObject>() != null)
                {
                    DamageableObject damageable = hit.collider.gameObject.GetComponent<DamageableObject>();
                    damageable.Damage(GetCharacterDamage(), gameObject);
                    return;
                }
            }
        }
            
    }
        
    public void OnMove(InputValue value)
    {

        Vector2 val = value.Get<Vector2>();
        _horizontalInput = val.x;
        _verticalInput = val.y;

    }

    void FixedUpdate()
    {
        if (_activeInterface == null)
        {
            UpdatePosition();
        }
    }

    private void UpdatePosition()
    {
        
        Vector2 position = RigidBody.position;
        position.x += 10.0f * _horizontalInput * Time.deltaTime * characterSpeed;
        position.y += 10.0f * _verticalInput * Time.deltaTime * characterSpeed;
        RigidBody.MovePosition(position);
        
    }
    
    public void OnEscape()
    {
        if (_menuManager == null)
        {
            Beneath.InstantiateSafeThen(Beneath.Assets.Menu, instHandle =>
            {
                _menuManager = instHandle.Result.GetComponent<MenuManager>();
                OpenUserInterface(_menuManager);

            });
            
        }
        else
        {
            OpenUserInterface(_menuManager);
        }
        
    }
    
    public void OpenUserInterface(MasterInterface master)
    {

        if (_activeInterface == null)
        {

            _activeInterface = master;
            _activeInterface.Open(this);

        }

    }

    public void CloseActiveInterface()
    {

        if (_activeInterface != null)
        {
            
            _activeInterface.Close();
            _activeInterface = null;

        }
        
    }
        
}