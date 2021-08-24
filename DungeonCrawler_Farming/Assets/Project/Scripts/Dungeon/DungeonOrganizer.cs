using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.Dungeon
{
    public class DungeonOrganizer : MonoBehaviour
    {
        public List<Room_Generation> generatedRooms;
        public List<GameObject> endRooms;


        public void AddToDungeonList(Room_Generation _newRoom)
        {
            generatedRooms.Add(_newRoom);
        }

        public void AddEndPointRoom(GameObject _newEndPoint)
        {
            endRooms.Add(_newEndPoint);
        }

        public void DeleteList()
        {
            generatedRooms.Clear();
        }


    }
}


