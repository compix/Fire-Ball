using UnityEngine;
using System.Collections;
using UnityStandardAssets.Effects;

public class Burnable : MonoBehaviour
{
    public Transform m_fire;

    public float burnDuration = 3.0f;

    private SpriteRenderer sprite;
    private float burnProgress = 0.0f;
    private bool isBurning = false;

    // Use this for initialization
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isBurning)
        {
            burnProgress += Time.deltaTime / burnDuration;
            sprite.color = Color.Lerp(Color.white, new Color(0, 0, 0, 0), burnProgress);
        }
    }

    public void burn()
    {
        Transform fire = (Transform) Instantiate(m_fire, transform.position, transform.rotation);
        fire.GetComponent<ParticleSystemMultiplier>().multiplier = 1.8f;
        transform.parent = fire;
        gameObject.AddComponent<SelfDestruction>();
        GetComponent<SelfDestruction>().lifeDuration = burnDuration;
        fire.gameObject.AddComponent<SelfDestruction>();
        fire.GetComponent<SelfDestruction>().lifeDuration = burnDuration - 1.0f;
        isBurning = true;
    }
}
