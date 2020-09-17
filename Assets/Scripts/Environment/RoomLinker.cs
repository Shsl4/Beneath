using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace Environment
{
    public abstract class RoomLinker : MonoBehaviour
    {
        
        [HideInInspector] public string sceneName;
        [HideInInspector] public Vector2 travelPosition;
        
        protected bool Travel()
        {
            if (String.IsNullOrEmpty(sceneName)) { return false; }
            Beneath.data.player.TravelToSceneAtLocation(sceneName, travelPosition);
            return true;
        }
        
    }
    
#if UNITY_EDITOR
    
    [CustomEditor(typeof(RoomLinker))]
    [CanEditMultipleObjects]
    public class RoomLinkerEditor : Editor
    {
        private SerializedProperty _sceneName;
        private SerializedProperty _travelPosition;
        private Vector2 _travelOffset;
        private int _selectedScene = -1;
        private int _selectedTravelTarget = -1;
        private int _lastLoadedScene = -1;
        private int _objectSceneIndex;

        protected virtual void OnEnable()
        {
            _sceneName = serializedObject.FindProperty("sceneName");
            _travelPosition = serializedObject.FindProperty("travelPosition");
            _objectSceneIndex = SceneManager.GetActiveScene().buildIndex;
        }

        protected void OnDisable()
        {
            if (_selectedScene >= 0 && SceneManager.GetSceneByBuildIndex(_selectedScene).isLoaded)
            {
                EditorSceneManager.CloseScene(SceneManager.GetSceneByBuildIndex(_selectedScene), true);
            }
        }

        public override void OnInspectorGUI()
        {
            
            DrawDefaultInspector();
            EditorGUILayout.Space();
            serializedObject.Update();
            
            if (!String.IsNullOrEmpty(_sceneName.stringValue))
            {
                
                EditorGUILayout.HelpBox("Set to travel to \'" + _sceneName.stringValue + "\' at position " + _travelPosition.vector2Value, MessageType.Info);
                if (GUILayout.Button("Reset travel location"))
                {
                    _sceneName.stringValue = null;
                    _travelPosition.vector2Value = Vector2.zero;
                    serializedObject.ApplyModifiedProperties();
                }

                EditorGUILayout.Space();
                return;
                
            }
            
            EditorGUILayout.HelpBox("No value has been configured. Travel will not occur.", MessageType.Error);
            
            string[] availableScenes = new string[SceneManager.sceneCountInBuildSettings];
            string[] shortNames = new string[SceneManager.sceneCountInBuildSettings];

            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                availableScenes[i] = SceneUtility.GetScenePathByBuildIndex(i);
                shortNames[i] = GetShortSceneName(SceneUtility.GetScenePathByBuildIndex(i));
            }

            _selectedScene = EditorGUILayout.Popup("Build Scenes", _selectedScene, shortNames);

            if (_selectedScene >= 0)
            {
                if (!SceneManager.GetSceneByBuildIndex(_selectedScene).isLoaded)
                {
                    if (GUILayout.Button("Load Scene"))
                    {
                        if (_lastLoadedScene > 0)
                        {
                            EditorSceneManager.CloseScene(SceneManager.GetSceneByBuildIndex(_lastLoadedScene), true);
                        }
                        
                        EditorSceneManager.OpenScene(availableScenes[_selectedScene], OpenSceneMode.Additive);
                        _lastLoadedScene = _selectedScene;
                    }
                }
                else
                {
                    if (GUILayout.Button("Unload Scene"))
                    {
                        if (_selectedScene != _objectSceneIndex)
                        {
                            EditorSceneManager.CloseScene(SceneManager.GetSceneByBuildIndex(_selectedScene), true);
                        }
                        else
                        {
                            Debug.Log("Cannot unload the scene in which the target object is located.");
                        }
                    }

                    int size = SceneManager.GetSceneByBuildIndex(_selectedScene).rootCount;
                    string[] objectNames = new string[size];

                    for (int i = 0; i < size; i++)
                    {
                        objectNames[i] = SceneManager.GetSceneByBuildIndex(_selectedScene).GetRootGameObjects()[i].name;
                    }

                    _selectedTravelTarget = EditorGUILayout.Popup("Travel Target", _selectedTravelTarget, objectNames);
                    _travelOffset = EditorGUILayout.Vector2Field("Travel Offset", _travelOffset);

                    if (GUILayout.Button("Save Location"))
                    {
                        _travelPosition.vector2Value = _travelOffset;

                        if (_selectedTravelTarget >= 0)
                        {
                            GameObject gameObject = SceneManager.GetSceneByName(shortNames[_selectedScene]).GetRootGameObjects()[_selectedTravelTarget];

                            if (gameObject)
                            {
                                Vector3 position = gameObject.transform.position;
                                _travelPosition.vector2Value += new Vector2(position.x, position.y);
                            }

                        }

                        _sceneName.stringValue = shortNames[_selectedScene];
                        EditorSceneManager.CloseScene(SceneManager.GetSceneByName(shortNames[_selectedScene]), true);
                        _travelOffset = Vector2.zero;
                        _selectedTravelTarget = -1;
                        
                    }
                    
                }

            }
            
            serializedObject.ApplyModifiedProperties();
            EditorGUILayout.Space();

        }

        private string GetShortSceneName(string path)
        {
            return Path.GetFileNameWithoutExtension(Path.GetFileName(path));
        }
        
    }
#endif
}

