using UnityEngine;
using System.Collections;

public class HighlightAppearance : MonoBehaviour
{
    private SpriteRenderer m_sprite;
    private Color m_oldColor;

    private float m_lerpTimeMultiplier = 0.1f;
    private float m_fadeTimer = 0.0f;
    private bool m_appeared = false;

    // Use this for initialization
    void Start()
    {
        m_sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_appeared || !m_sprite)
            return;

        m_fadeTimer += Time.deltaTime * m_lerpTimeMultiplier;

        m_sprite.color = Color.Lerp(m_sprite.color, m_oldColor, m_fadeTimer);

        if (m_fadeTimer >= 1.0f)
        {
            m_fadeTimer = 0.0f;
            m_appeared = false;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (m_sprite && collision.GetComponent<Camera>() != null)
        {
            m_oldColor = m_sprite.color;
            m_sprite.color = Color.white;
            m_appeared = true;
        }
    }
}
