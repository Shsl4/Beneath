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
            Beneath.Data.player.TravelToSceneAtLocation(sceneName, travelPosition);
            return true;
        }
        
    }
    
#if UNITY_EDITOR
    
    [CustomEditor(typeof(RoomLinker))]
    [CanEditMultipleObjects]
    public class RoomLinkerEditor : Editor
    {
        
        SerializedProperty sceneName;
        SerializedProperty travelPosition;
        int selectedScene = -1;
        int selectedTravelTarget = -1;
        Vector2 travelOffset;

        void OnEnable()
        {
            sceneName = serializedObject.FindProperty("sceneName") ;
            travelPosition = serializedObject.FindProperty("travelPosition");
        }
        
        public override void OnInspectorGUI()
        {
            
            DrawDefaultInspector();
            
            if (!String.IsNullOrEmpty(sceneName.stringValue))
            {
                EditorGUILayout.HelpBox("Set to travel to \'" + sceneName.stringValue + "\' at position " + travelPosition.vector2Value, MessageType.Info);
            }
            else
            {
                EditorGUILayout.HelpBox("No value has been configured. Travel will not occur.", MessageType.Error);
            }
            
            serializedObject.Update();

            string[] availableScenes = new string[SceneManager.sceneCountInBuildSettings];
            string[] shortNames = new string[SceneManager.sceneCountInBuildSettings];

            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                availableScenes[i] = SceneUtility.GetScenePathByBuildIndex(i);
                shortNames[i] = GetShortSceneName(SceneUtility.GetScenePathByBuildIndex(i));
            }

            selectedScene = EditorGUILayout.Popup("Build Scenes", selectedScene, shortNames);

            if (selectedScene < 0) { return; }

            if (!SceneManager.GetSceneByName(shortNames[selectedScene]).isLoaded)
            {
                if (GUILayout.Button("Load Scene"))
                {
                    EditorSceneManager.OpenScene(availableScenes[selectedScene], OpenSceneMode.Additive);
                }
            }
            else
            {

                if (GUILayout.Button("Unload Scene"))
                {
                    EditorSceneManager.CloseScene(SceneManager.GetSceneByName(shortNames[selectedScene]), true);
                }

                int size = SceneManager.GetSceneByName(shortNames[selectedScene]).rootCount;
                string[] objectNames = new string[size];

                for (int i = 0; i < size; i++)
                {
                    objectNames[i] = SceneManager.GetSceneByName(shortNames[selectedScene]).GetRootGameObjects()[i].name;
                }
                
                selectedTravelTarget = EditorGUILayout.Popup("Travel Target", selectedTravelTarget, objectNames);
                travelOffset = EditorGUILayout.Vector2Field("Travel Offset", travelOffset);

                if (!GUILayout.Button("Save Location")) { return; }
                
                travelPosition.vector2Value = travelOffset;
                
                if (selectedTravelTarget >= 0)
                {

                    GameObject gameObject = SceneManager.GetSceneByName(shortNames[selectedScene]).GetRootGameObjects()[selectedTravelTarget];

                    if (gameObject)
                    {
                        Vector3 position = gameObject.transform.position;
                        travelPosition.vector2Value += new Vector2(position.x, position.y);
                    }
                    
                }
                
                sceneName.stringValue = shortNames[selectedScene];
                EditorSceneManager.CloseScene(SceneManager.GetSceneByName(shortNames[selectedScene]), true);
                travelOffset = Vector2.zero;
                selectedTravelTarget = -1;

            }

            serializedObject.ApplyModifiedProperties();
            
        }

        private string GetShortSceneName(string path)
        {
            return Path.GetFileNameWithoutExtension(Path.GetFileName(path));
        }
        
    }
#endif
}

