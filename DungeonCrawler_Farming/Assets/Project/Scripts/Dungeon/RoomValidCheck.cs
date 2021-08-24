using UnityEngine;

namespace Project.Scripts.Dungeon
{
    public class RoomValidCheck : MonoBehaviour
    {
        public Room_Generation m_manager;
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.tag == "Room")
            {
                m_manager.isValidLocation = false;
            }
        }
    }
}

