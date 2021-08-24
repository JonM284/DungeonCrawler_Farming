using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Level_Generation_Manager : MonoBehaviour
{
    public float roomSize;
    public int numOfKeys;
    public Vector2Int numberOfRooms;
    public GameObject[] roomTypes;
    public List<GameObject> availableRooms, allRooms;
    public GameObject keyObj, trophyObj;
   

    private Vector2Int trophyRoomCord;
    private Vector2Int middleRoomCord;
    public Vector2 middleRoomPos;

    private bool has_Generated = false;

   

    private void Start()
    {
        
    }


    void Find_Middle_Room()
    {
        //x != Mathf.CeilToInt(numberOfRooms.x / 2) && y != Mathf.CeilToInt(numberOfRooms.y / 2)
        int _x = Mathf.CeilToInt(numberOfRooms.x / 2);
        int _y = Mathf.CeilToInt(numberOfRooms.y / 2);

        middleRoomCord = new Vector2Int(_x, _y);

        Debug.Log($"Middle Room: {_x}, {_y}");

        Pick_Random_Trophy_Room();
    }

    void Pick_Random_Trophy_Room()
    {
        int _random_X = Random.value > 0.5f ? Random.Range(0, middleRoomCord.x - 1) : Random.Range(middleRoomCord.x + 1, numberOfRooms.x);
        int _random_Y = Random.value > 0.5f ? Random.Range(0, middleRoomCord.y - 1) : Random.Range(middleRoomCord.y + 1, numberOfRooms.y);

        trophyRoomCord = new Vector2Int(_random_X, _random_Y);

        Debug.Log($"Trophy Room: {_random_X}, {_random_Y}");

        Generate_Grid();
    }

    

    void Generate_Grid()
    {
        for (int x = 0; x < numberOfRooms.x; x++)
        {
            for (int y = 0; y < numberOfRooms.y; y++)
            {
                GameObject currentRoom;
                float xPos = transform.position.x + x * roomSize;
                float yPos = transform.position.z + y * roomSize;

                Vector3 _room_Pos = new Vector3(xPos, transform.position.y, yPos);

                if (x == middleRoomCord.x && y == middleRoomCord.y)
                {
                    //currentRoom = PhotonNetwork.Instantiate(roomTypes[3].name, _room_Pos, Quaternion.identity);
                    //currentRoom.GetComponent<Experimental_Room>().MarkMiddleRoom();
                    middleRoomPos = _room_Pos;
                }
                else if (x == trophyRoomCord.x && y == trophyRoomCord.y)
                {
                    //currentRoom = PhotonNetwork.Instantiate(roomTypes[0].name, _room_Pos, Quaternion.identity);
                    //PhotonNetwork.Instantiate(trophyObj.name, new Vector3(_room_Pos.x, _room_Pos.y + 2, _room_Pos.z), Quaternion.identity);
                    //currentRoom.GetComponent<Experimental_Room>().Mark_Trophy_Room();
                }
                else
                {
                    if ((x == 0 && y == 0) || (x == numberOfRooms.x -1 && y == 0) 
                        || (x == 0 && y == numberOfRooms.y - 1) || (x == numberOfRooms.x -1 && y == numberOfRooms.y -1))
                    {
                       // currentRoom = PhotonNetwork.Instantiate(roomTypes[1].name, _room_Pos, Quaternion.identity);
                    }
                    //else currentRoom = PhotonNetwork.Instantiate(roomTypes[Random.Range(0, roomTypes.Length)].name, _room_Pos, Quaternion.identity);
                    //if (currentRoom.GetComponent<Experimental_Room>() != null) currentRoom.GetComponent<Experimental_Room>().Room_Generation();
                    //currentRoom.transform.name = $"Room: {x}, {y}";
                    //availableRooms.Add(currentRoom);
                }

                
                //currentRoom.GetComponent<Experimental_Room>().SetRoomCord(x, y);
                
                
                
                
                
                //allRooms.Add(currentRoom);
            }
        }
        Generate_Key_Rooms();
        has_Generated = true;
    }

    void Generate_Key_Rooms()
    {
        for (int i = 0; i < numOfKeys; i++)
        {
            int _random_Key_Room = Random.Range(0, availableRooms.Count);
            Debug.Log($"Key can be found in room {availableRooms[_random_Key_Room].name}");
            //availableRooms[_random_Key_Room].GetComponent<Experimental_Room>().Mark_Key_Room();
            Vector3 _room_Pos = availableRooms[_random_Key_Room].transform.position;
            //PhotonNetwork.Instantiate(keyObj.name, new Vector3(_room_Pos.x, _room_Pos.y + 2, _room_Pos.z), Quaternion.identity);
            availableRooms.RemoveAt(_random_Key_Room);
        }
    }

    void Erase_Level()
    {
        for (int i = 0; i < allRooms.Count; i++)
        {
            Destroy(allRooms[i]);
        }
        allRooms.Clear();
        availableRooms.Clear();
        Debug.ClearDeveloperConsole();
        has_Generated = false;
    }


   

}
