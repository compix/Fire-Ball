using UnityEngine;
using System.Collections;

public class ChargeRecharge : MonoBehaviour
{
    private ParticleSystem m_particleSystem;
    private const float boostEffectDuration = 0.5f;
    private float boostEffectTimeLeft = 0.5f;
    private bool boostEffect = false;

    // Use this for initialization
    void Start()
    {
        m_particleSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if(boostEffect)
        {
            boostEffectTimeLeft -= Time.deltaTime;

            if (boostEffectTimeLeft <= 0.0f)
            {
                boostEffectTimeLeft = boostEffectDuration;
                m_particleSystem.emissionRate = 10.0f;
                boostEffect = false;
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.GetComponent<PlayerControl>() != null)
        {
            collision.GetComponent<PlayerControl>().recharge();
            m_particleSystem.emissionRate = 100.0f;
            AudioSource sound = GetComponent<AudioSource>();
            if (sound && sound.enabled)
                sound.Play();
            boostEffect = true;
        }
    }
}
