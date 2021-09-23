using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Project.Scripts.Player;

namespace Project.Scripts.Misc
{
    public class CameraBehavior : MonoBehaviour
    {
        [Tooltip("How long in seconds it will take the camera to move to the end position")]
        [SerializeField]
        private float moveSpeed;

        private Player_Movement player;

        private Vector2 currentPosition;

        public void AssignPlayer()
        {

        }

        public void MoveCamera(Vector3 _newPos)
        {
            transform.DOMove(_newPos, moveSpeed);
        }

        /// <summary>
        /// Shake camera due to some event.
        /// </summary>
        /// <param name="_Max_Time">Duration of camera shake.</param>
        /// <param name="_magnitude">Strength of camera shake.</param>
        /// <param name="_dir">Direction of shake: 0=Vert+Hor, 1=Hor, 2=Vert</param>
        public void ShakeCamera(float _Max_Time, float _magnitude, int _dir)
        {
            //commented out, however might use this later on. Keep in mind
            //Time.timeScale = slowedTime;
            StartCoroutine(CameraShake(_magnitude, _Max_Time, _dir));
        }


        IEnumerator CameraShake(float _mag, float _time, int _dir)
        {
            //TODO: Change this to duration of effect
            while (Time.timeScale < 0.8f)
            {
                float Xposition = 0;
                float Yposition = 0;
                if (_dir == 1)
                {
                    Xposition = Random.Range(-1f, 1f) * _mag;
                }
                else if (_dir == 2)
                {
                    Yposition = Random.Range(-1f, 1f) * _mag;
                }
                else
                {
                    Xposition = Random.Range(-1f, 1f) * _mag;
                    Yposition = Random.Range(-1f, 1f) * _mag;
                }

                transform.position = new Vector2(Xposition + currentPosition.x, Yposition + currentPosition.y);
                yield return null;
            }

            transform.position = currentPosition;


        }
    }
}
