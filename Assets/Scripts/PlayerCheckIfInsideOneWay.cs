using UnityEngine;

public class PlayerCheckIfInsideOneWay : MonoBehaviour
{
    public bool m_BehindPlatform;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("BehindPlatform"))
            m_BehindPlatform = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("BehindPlatform"))
            m_BehindPlatform = false;
    }
}
