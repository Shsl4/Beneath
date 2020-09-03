using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Battle
{
    public class BattleHeart : MonoBehaviour
    {

        private SpriteRenderer _spriteRenderer;
        private BoxCollider2D _boxCollider;
        private Rigidbody2D _rigidbody;
        private PlayerInput _playerInput;
        private CameraFader _fader;
        private float _horizontalInput;
        private float _verticalInput;
        private const float HeartSpeed = .5f;

        void Awake()
        {
            _spriteRenderer = FindObjectOfType<SpriteRenderer>();
            _boxCollider = FindObjectOfType<BoxCollider2D>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _playerInput = GetComponent<PlayerInput>();
            _fader = GetComponent<CameraFader>();
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
            position.x += 10.0f * _horizontalInput * Time.deltaTime * HeartSpeed;
            position.y += 10.0f * _verticalInput * Time.deltaTime * HeartSpeed;
            _rigidbody.MovePosition(position);
        }
        
        public void OnMove(InputValue value)
        {

            Vector2 val = value.Get<Vector2>();
            _horizontalInput = val.x;
            _verticalInput = val.y;

        }

    }
    
#if UNITY_EDITOR
    [CustomEditor(typeof(BattleHeart))]
    [CanEditMultipleObjects]
    public class BattleHeartEditor : Editor
    {
        
        private SpriteRenderer _spriteRenderer;
        private BoxCollider2D _collider;

        protected void OnEnable()
        {
            _spriteRenderer = ((BattleHeart)serializedObject.targetObject).GetComponent<SpriteRenderer>();
            _collider = ((BattleHeart)serializedObject.targetObject).GetComponent<BoxCollider2D>();
        }

        public override void OnInspectorGUI()
        {

            DrawDefaultInspector();
            serializedObject.Update();

            if (GUILayout.Button("Auto-Size Collider"))
            {
                
                var bounds = _spriteRenderer.bounds;
                Vector3 lossyScale = ((BattleHeart)serializedObject.targetObject).transform.lossyScale;
        
                _collider.size = new Vector3(bounds.size.x / lossyScale.x,
                    bounds.size.y / lossyScale.y,
                    bounds.size.z / lossyScale.z);
                
            }
            
            serializedObject.ApplyModifiedProperties();

        }
        
    }
#endif

}
