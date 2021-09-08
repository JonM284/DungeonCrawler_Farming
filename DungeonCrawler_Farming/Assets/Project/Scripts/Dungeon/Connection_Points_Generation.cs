using UnityEngine;

namespace Project.Scripts.Dungeon
{
    public class Connection_Points_Generation : MonoBehaviour
    {

        public bool isValid = true;

        public GameObject obstructingRoom;

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.gameObject.tag == "Room")
            {
                isValid = false;
                obstructingRoom = other.gameObject;
                Debug.Log("Has hit room");
            }
        }

    }
}

