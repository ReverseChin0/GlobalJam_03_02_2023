using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatforms : MonoBehaviour
{
    private PlatformEffector2D m_Effector;
    private PlayerGroundCheck m_GroundCheck;
    public PlayerFallThrough playerFall;

    private void Awake()
    {
        m_Effector = GetComponent<PlatformEffector2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            playerFall = collision.transform.GetComponent<PlayerFallThrough>();
            m_GroundCheck = collision.transform.GetComponent<PlayerGroundCheck>();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (playerFall == null)
        {
            return;
        }
        if (playerFall.fallThrough)
        {
            m_Effector.rotationalOffset = 180;
            playerFall = null;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        playerFall = null;
        m_Effector.rotationalOffset = 0;
    }
}
