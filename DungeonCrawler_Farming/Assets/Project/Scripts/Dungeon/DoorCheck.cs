using UnityEngine;

namespace Project.Scripts.Dungeon
{
    public class DoorCheck : MonoBehaviour
    {

        public GameObject current_Attached_Wall;
        public GameObject adjacent_Object;
        public GameObject spawnableBreakWall;
        [Tooltip("0=Door, 1=NormalWall, 2=BreakableWall")]
        public GameObject[] obstructionObjects;

        public bool isObstructed = false;
        public Color testColor;

        public enum next_Obj
        {
            None,
            Wall,
            Door
        };

        public enum Desired_Obj
        {
            Door,
            NormalWall,
            BreakableWall
        }

        public next_Obj adj_Obj_type = next_Obj.None;
        public Desired_Obj myType = Desired_Obj.Door;

        /// <summary>
        /// Turn ON the desired gameobject.
        /// </summary>
        /// <param name="_desiredInt">0=Door, 1=NormalWall, 2=BreakableWall</param>
        public void GenerateObject(int _desiredInt)
        {
            for (int i = 0; i < obstructionObjects.Length; i++)
            {
                if (obstructionObjects[i].activeInHierarchy) obstructionObjects[i].SetActive(false);
            }

            obstructionObjects[_desiredInt].SetActive(true);
            if (_desiredInt == 1) myType = Desired_Obj.NormalWall;
            if (_desiredInt != 0) current_Attached_Wall.SetActive(false);
            if (_desiredInt == 2)
            {
                Instantiate(spawnableBreakWall, adjacent_Object.transform.position, adjacent_Object.transform.rotation, adjacent_Object.transform.parent);
                adjacent_Object.SetActive(false);
                myType = Desired_Obj.BreakableWall;
                Debug.Log($"<color=#00ff00>Breakable wall added in place of: </color>{adjacent_Object.name}");
            }

        }

        public void SetOnlySpecificActive(int _desiredInt)
        {
            for (int i = 0; i < obstructionObjects.Length; i++)
            {
                if (obstructionObjects[i].activeInHierarchy) obstructionObjects[i].SetActive(false);
            }
            current_Attached_Wall.SetActive(false);
            obstructionObjects[_desiredInt].SetActive(true);
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.tag == "Door")
            {
                adj_Obj_type = next_Obj.Door;
                isObstructed = true;
            }
            else if (other.gameObject.tag == "Wall")
            {
                adj_Obj_type = next_Obj.Wall;
                isObstructed = true;
            }
            adjacent_Object = other.gameObject;
        }


        private void OnDrawGizmos()
        {
            if (myType == Desired_Obj.BreakableWall)
            {

                Gizmos.color = testColor;
                Gizmos.DrawCube(transform.position, transform.localScale * 2);
            }
            else if (myType == Desired_Obj.NormalWall)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawCube(transform.position, transform.localScale * 2);
            }
        }
    }
}

