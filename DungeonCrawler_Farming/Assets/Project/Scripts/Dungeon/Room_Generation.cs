using System.Collections;
using UnityEngine;

namespace Project.Scripts.Dungeon
{
    [System.Serializable]
    public class RandomRooms
    {
        public enum Directions
        {
            Top,
            Bottom,
            Left,
            Right
        };

        public Directions currentType;
        public GameObject[] roomPrefabs;
    }


    public class Room_Generation : MonoBehaviour
    {

        public bool isInitialRoom;

        public DungeonOrganizer dungeonManager;

        public bool isValidLocation = true;
        public bool verifiedSpot = false;
        public bool executedWaitCheck = false;

        [SerializeField] private RandomRooms[] spawnableRooms;
        

        public int currentLvl;
        public int difficultyLvl;

        private float calcPercentage;


        public DoorCheck[] door_checkers;
        public Connection_Points_Generation[] points;

        [SerializeField] private Transform parent; 

        // Start is called before the first frame update
        void Start()
        {
            if (isInitialRoom) StartCoroutine(GenerateSurroundingRooms());
        }

        public void CheckSurroundings()
        {
            StartCoroutine(WaitToCheck());
        }

        public void StartGeneration(int _crntlvl, int _maxlvl)
        {
            currentLvl = _crntlvl;
            difficultyLvl = _maxlvl;

            StartCoroutine(GenerateSurroundingRooms());
        }

        public IEnumerator GenerateSurroundingRooms()
        {
            calcPercentage = currentLvl / difficultyLvl;
            if (currentLvl <= difficultyLvl)
            {
                for (int i = 0; i < points.Length; i++)
                {
                    if (points[i].isValid)
                    {
                        int _randomRoom = 0;
                        if (!isInitialRoom)
                        {
                            if (calcPercentage < 0.33f)
                                _randomRoom = Random.value <= 0.75f ? Random.Range(0, spawnableRooms[i].roomPrefabs.Length - 1) :
                                    Random.Range(0, spawnableRooms[i].roomPrefabs.Length);
                            else if (calcPercentage >= 0.33f && calcPercentage < 0.66f)
                                _randomRoom = Random.value <= 0.5f ? Random.Range(0, spawnableRooms[i].roomPrefabs.Length - 1) :
                                    Random.Range(0, spawnableRooms[i].roomPrefabs.Length);
                            else if (calcPercentage >= 0.66f && calcPercentage < 0.8f)
                                _randomRoom = Random.value <= 0.2 ? Random.Range(0, spawnableRooms[i].roomPrefabs.Length - 1) :
                                    Random.Range(spawnableRooms[i].roomPrefabs.Length - 1, spawnableRooms[i].roomPrefabs.Length);

                            if (currentLvl >= difficultyLvl - 1)
                                _randomRoom = spawnableRooms[i].roomPrefabs.Length - 1;


                        }
                        else
                        {
                            _randomRoom = 0;
                        }
                        //Instantiate a random room, DEPENDING ON CURRENT LEVEL. Every time a new room is generated in a loop, the level goes up
                        yield return new WaitForSeconds(0.1f);

                        if (points[i].isValid)
                        {
                            //if point is valid, instantiate room at room check position
                            //TODO: Change roomPrefabs[0] to be randomly chosen.
                            GameObject _newRoom = Instantiate(spawnableRooms[i].roomPrefabs[_randomRoom], new Vector2(points[i].transform.position.x, points[i].transform.position.y)
                            , Quaternion.identity, parent) as GameObject;
                            var newRoom = _newRoom.GetComponent<Room_Generation>();
                            
                            //If the new room has a room generation component (aka not a 1 door room)
                            if (_newRoom.GetComponent<Room_Generation>())
                            {
                                _newRoom.GetComponent<Room_Generation>().SetParent(parent);
                                newRoom.CheckSurroundings();
                                yield return new WaitUntil(() => newRoom.executedWaitCheck);
                                if (_newRoom != null) newRoom.StartGeneration(this.currentLvl + 1, this.difficultyLvl);
                                dungeonManager.AddToDungeonList(newRoom);
                                newRoom.dungeonManager = this.dungeonManager;
                            }
                            else
                            {
                                dungeonManager.AddEndPointRoom(_newRoom);
                            }
                        }

                        yield return new WaitForSeconds(0.5f);
                    }
                    else
                    {
                        yield return new WaitForEndOfFrame();
                    }
                }
                //StartCoroutine(GenerateDoors());
            }
        }

        public void SetParent(Transform _parent)
        {
            parent = _parent;
        }

        public IEnumerator GenerateDoors()
        {
            for (int i = 0; i < door_checkers.Length; i++)
            {
                if (door_checkers[i].isObstructed)
                {
                    if (door_checkers[i].adj_Obj_type == DoorCheck.next_Obj.Wall)
                    {
                        if (door_checkers[i].adjacent_Object.GetComponentInParent<Room_Generation>() != null)
                        {
                            door_checkers[i].GenerateObject(2);
                        }
                        else
                        {
                            door_checkers[i].GenerateObject(1);
                            Debug.Log($"<color=orange>Name:{door_checkers[i].adjacent_Object.name} Added wall</color>");
                        }
                    }

                }
                else
                {
                    door_checkers[i].GenerateObject(0);
                    Debug.Log($"<color=yellow>Added door</color>");
                }
                door_checkers[i].enabled = false;
            }
            //StopCoroutine(GenerateDoors());
            yield return new WaitForEndOfFrame();
        }

        public IEnumerator WaitToCheck()
        {
            yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
            if (!isValidLocation && !verifiedSpot)
            {
                executedWaitCheck = true;
                GameObject.Destroy(this.gameObject);
            }
            else if (isValidLocation && !verifiedSpot)
            {
                verifiedSpot = true;
                executedWaitCheck = true;
            }

            yield return null;
        }


    }
}

