using UnityEngine;
using System.Collections;
using UnityStandardAssets.Effects;
using UnityAnalyticsHeatmap;

public class Burnable : MonoBehaviour
{
    public Transform m_fire;

    public float m_burnDuration = 3.0f;

    private SpriteRenderer m_sprite;
    private float m_burnProgress = 0.0f;
    private bool m_isBurning = false;

    // Use this for initialization
    void Start()
    {
        m_sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(m_isBurning)
        {
            m_burnProgress += Time.deltaTime / m_burnDuration;
            m_sprite.color = Color.Lerp(Color.white, new Color(0, 0, 0, 0), m_burnProgress);
        }
    }

    public void Burn()
    {
        if (m_isBurning)
            return;

        Transform fire = (Transform) Instantiate(m_fire, transform.position, transform.rotation);
        fire.GetComponent<ParticleSystemMultiplier>().multiplier = 1.8f;
        transform.parent = fire;
        gameObject.AddComponent<SelfDestruction>();
        GetComponent<SelfDestruction>().duration = m_burnDuration;
        fire.gameObject.AddComponent<SelfDestruction>();
        fire.GetComponent<SelfDestruction>().duration = m_burnDuration - 1.0f;

        AudioSource sound = GetComponent<AudioSource>();
        if (sound)
            sound.Play();

        m_isBurning = true;
    }

    public bool IsBurning()
    {
        return m_isBurning;
    }
}
