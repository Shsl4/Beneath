using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Battle
{
    public class BattleHeart : MonoBehaviour
    {

        private Image _image;
        private BoxCollider2D _boxCollider;
        private Rigidbody2D _rigidbody;
        private PlayerInput _playerInput;
        private float _horizontalInput;
        private float _verticalInput;
        private const float HeartSpeed = .5f;

        void Awake()
        {
            _image = FindObjectOfType<Image>();
            _boxCollider = FindObjectOfType<BoxCollider2D>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _playerInput = GetComponent<PlayerInput>();
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
            position.x += 1000.0f * _horizontalInput * Time.deltaTime * HeartSpeed;
            position.y += 1000.0f * _verticalInput * Time.deltaTime * HeartSpeed;
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
        
        private Image _image;
        private BoxCollider2D _collider;

        protected void OnEnable()
        {
            _image = ((BattleHeart)serializedObject.targetObject).GetComponent<Image>();
            _collider = ((BattleHeart)serializedObject.targetObject).GetComponent<BoxCollider2D>();
        }

        public override void OnInspectorGUI()
        {

            DrawDefaultInspector();
            serializedObject.Update();

            if (GUILayout.Button("Auto-Size Collider"))
            {
                
                var bounds = _image.rectTransform.rect.size;
                Vector3 lossyScale = ((BattleHeart)serializedObject.targetObject).transform.lossyScale;
        
                _collider.size = new Vector2(bounds.x / lossyScale.x,
                    bounds.y / lossyScale.y);
                
            }
            
            serializedObject.ApplyModifiedProperties();

        }
        
    }
#endif

}
