using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    [SerializeField]
    Transform innerFlame;

    [SerializeField]
    int hp = 2;

    [SerializeField]
    float sizeReduction = 0.2f;

    [SerializeField]
    float minSize = 0.1f;

    [SerializeField]
    Transform m_loseSound;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerExit2D(Collider2D collision)
    {

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Burnable" && collision.GetComponent<Damager>() != null)
        {
            hp--;
            //reduceSize();
            removeInnerFlame();

            Destroy(collision.GetComponent<Damager>());

            if(hp == 0)
            {
                gameObject.AddComponent<SelfDestruction>();
                gameObject.GetComponent<SelfDestruction>().duration = 0.0f;

                Destroy(GetComponent<PlayerControl>());
                Destroy(GetComponent<CircleCollider2D>());
                Destroy(GetComponent<Rigidbody2D>());

                if(m_loseSound)
                {
                    AudioSource sound = m_loseSound.GetComponent<AudioSource>();
                    if(sound && sound.enabled)
                        sound.Play();
                }
            }
        }
    }

    private void reduceSize()
    {
        var systems = GetComponentsInChildren<ParticleSystem>();

        foreach(var system in systems)
        {
            system.startSize = Mathf.Max(system.startSize - sizeReduction, minSize);
        }
    }

    private void removeInnerFlame()
    {
        if(innerFlame)
        {
            innerFlame.gameObject.AddComponent<SelfDestruction>();
            innerFlame.gameObject.GetComponent<SelfDestruction>().duration = 0.0f;
        }
    }
}
