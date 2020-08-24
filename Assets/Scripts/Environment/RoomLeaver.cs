using System;
using UnityEngine;

namespace Environment
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class RoomLeaver : MonoBehaviour
    {

        private string _previousScene;
        private Vector2 _previousLocation;

        public void OnRoomEntered(string fromScene, Vector2 fromLocation)
        {
            _previousScene = fromScene;
            _previousLocation = fromLocation;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.GetComponent<ControllableCharacter>())
            {
                other.gameObject.GetComponent<ControllableCharacter>().TravelToSceneAtLocation(_previousScene, _previousLocation);
            }        
        }
        
    }
}
