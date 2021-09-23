using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

namespace Project.Scripts.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player_Movement : MonoBehaviour
    {

        //variables shown in inspector
        [SerializeField]
        private float walkSpeed;

        [SerializeField]
        private float dodgeSpeed;

        [SerializeField]
        private float m_dodgeTimerMax;

        [SerializeField]
        private float m_damageTimerMax;

        [SerializeField]
        private float m_damageColorChangeSpeed;

        [SerializeField]
        private float m_damageKnockbackTimerMax;

        [SerializeField]
        private float maxHealth;

        [SerializeField]
        private float m_currentHealth;

        [Space]

        [Header("Temporary Variables")]
        [SerializeField]
        private Color normalColor;
        [SerializeField]
        private Color dashingColor, damageColor = Color.white;
        [SerializeField]
        private SpriteRenderer characterSprite;

        //private variables

        private Rewired.Player m_player;

        private float m_dodgeTimerCurrent = 0;
        private float m_damageTimerCurrent = 0;
        private float m_dashTimerMax;
        [SerializeField]
        private float m_currentSpeed;
        private Vector2 vel;
        [SerializeField]
        private Vector2 dashDir;
        //debug direction
        private Vector2 tempDir;
        private float m_horizontalInput, m_verticalInput;

        private bool m_isDodging = false;
        private bool m_isTakingDamage = false;

        private Rigidbody2D rb;

        private void Awake()
        {
            m_player = ReInput.players.GetPlayer(0);
            rb = GetComponent<Rigidbody2D>();
            m_currentHealth = maxHealth;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Check_Input();
            Check_Cooldown();
        }

        private void FixedUpdate()
        {
            Move_Player();
        }


        private void Check_Input()
        {
            if (!m_isDodging)
            {
                m_horizontalInput = m_player.GetAxisRaw("Horizontal");
                m_verticalInput = m_player.GetAxisRaw("Vertical");

                vel.x = m_horizontalInput * m_currentSpeed;
                vel.y = m_verticalInput * m_currentSpeed;

                Vector2 tempDir = new Vector2(m_horizontalInput, m_verticalInput);
                if (tempDir.magnitude > 0.1f)
                {
                    dashDir = tempDir;
                }
            }
            else
            {
                vel.x = dashDir.x * m_currentSpeed;
                vel.y = dashDir.y * m_currentSpeed;
            }



            if (m_player.GetButtonDown("Dodge"))
            {
                Do_Dodge_Action(m_dodgeTimerMax, vel);
            }

            if (m_player.GetButtonDown("Attack"))
            {

                Debug.Log("<color=red>Player Attacked.</color>");
            }
        }

        void Check_Cooldown()
        {
            if (m_isDodging && m_dodgeTimerCurrent < m_dashTimerMax)
            {
                m_dodgeTimerCurrent += Time.deltaTime;
            }

            if (m_isDodging && m_dodgeTimerCurrent >= m_dashTimerMax)
            {
                Reset_Dodge_Variables();
            }

            if (m_isTakingDamage && m_damageTimerCurrent < m_damageTimerMax)
            {
                m_damageTimerCurrent += Time.deltaTime;
            }

            if (m_isTakingDamage && m_damageTimerCurrent >= m_damageTimerMax)
            {
                ResetDamageVariables();
            }
        }

        /// <summary>
        /// No purpose yet, possibly if player gets an item that allows them to avoid taking damage
        /// </summary>
        /// <param name="_maxTime"></param>
        void ActivateInvincibility(float _maxTime)
        {

        }

        void TakeDamage(float _damage, Vector3 _damagerPos)
        {
            var damageDir = transform.position - _damagerPos;
            Do_Dodge_Action(m_damageKnockbackTimerMax, new Vector2(damageDir.x, damageDir.y));
            m_currentHealth -= _damage;
            StartCoroutine(FlashingSprite());
        }

        void StartDamageCooldown()
        {
            m_isTakingDamage = true;
        }


        /// <summary>
        /// Start Dash Actions
        /// </summary>
        void Do_Dodge_Action(float _time, Vector2 _dir)
        {
            m_isDodging = true;
            dashDir = _dir;
            characterSprite.color = dashingColor;
            m_dashTimerMax = _time;
        }
        /// <summary>
        /// Reset Dash variables
        /// </summary>
        void Reset_Dodge_Variables()
        {
            m_isDodging = false;
            m_dodgeTimerCurrent = 0;
            characterSprite.color = normalColor;
        }

        void ResetDamageVariables()
        {
            m_isTakingDamage = false;
            m_damageTimerCurrent = 0;
            characterSprite.color = normalColor;
        }

        /// <summary>
        /// Move player
        /// </summary>
        private void Move_Player()
        {
            m_currentSpeed = m_isDodging ? dodgeSpeed : walkSpeed;


            rb.MovePosition(rb.position + new Vector2(Mathf.Clamp(vel.x, -m_currentSpeed, m_currentSpeed),
                    Mathf.Clamp(vel.y, -m_currentSpeed, m_currentSpeed)) * Time.deltaTime);
        }


        IEnumerator FlashingSprite()
        {
            characterSprite.color = damageColor;
            yield return new WaitUntil(() => !m_isDodging);
            StartDamageCooldown();
            while (m_isTakingDamage)
            {
                characterSprite.color = damageColor;
                yield return new WaitForSeconds(m_damageColorChangeSpeed);
                characterSprite.color = normalColor;
                yield return new WaitForSeconds(m_damageColorChangeSpeed);
            }
            yield return new WaitUntil(() => !m_isTakingDamage);
            StopCoroutine(FlashingSprite());
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Enemy") && !m_isTakingDamage && !m_isDodging)
                TakeDamage(1.5f, other.transform.position);
        }
    }

}