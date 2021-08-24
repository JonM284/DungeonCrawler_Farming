using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

[RequireComponent(typeof(Rigidbody2D))]
public class Player_Movement : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed;

    [SerializeField]
    private float dodgeSpeed;

    [SerializeField]
    private float m_dodgeTimerMax;

    [Space]

    [Header("Temporary Variables")]
    [SerializeField]
    private Color normalColor, dashingColor;
    [SerializeField]
    private SpriteRenderer characterSprite;



    private Player m_player;

    private float m_dodgeTimerCurrent = 0;
    [SerializeField]
    private float m_currentSpeed;
    private Vector2 vel;
    [SerializeField]
    private Vector2 dashDir;
    private Vector2 tempDir;
    private float m_horizontalInput, m_verticalInput;
    private bool m_isDodging = false;

    private Rigidbody2D rb;

    private void Awake()
    {
        m_player = ReInput.players.GetPlayer(0);
        rb = GetComponent<Rigidbody2D>();
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
        if (!m_isDodging) {
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
            Do_Dodge_Action();
        }

        if (m_player.GetButtonDown("Attack"))
        {
            Debug.Log("<color=red>Player Attacked.</color>");
        }
    }

    void Check_Cooldown()
    {
        if (m_isDodging && m_dodgeTimerCurrent < m_dodgeTimerMax)
        {
            m_dodgeTimerCurrent += Time.deltaTime;
        }

        if (m_isDodging && m_dodgeTimerCurrent >= m_dodgeTimerMax)
        {
            Reset_Dodge_Variables();
        }

    }


    /// <summary>
    /// Start Dash Actions
    /// </summary>
    void Do_Dodge_Action()
    {
        m_isDodging = true;
        characterSprite.color = dashingColor;
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

    /// <summary>
    /// Move player
    /// </summary>
    private void Move_Player()
    {
        m_currentSpeed = m_isDodging ? dodgeSpeed : walkSpeed;


        rb.MovePosition(rb.position + new Vector2(Mathf.Clamp(vel.x, -m_currentSpeed, m_currentSpeed),
                Mathf.Clamp(vel.y, -m_currentSpeed, m_currentSpeed)) * Time.deltaTime);
    }
}
