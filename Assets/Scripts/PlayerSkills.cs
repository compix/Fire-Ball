using UnityEngine;
using System.Collections;
using UnityAnalyticsHeatmap;

public class PlayerSkills : MonoBehaviour
{
    public class Charge
    {
        private Rigidbody2D m_entityBody;

        private float m_startSpeed;
        private float m_endSpeed;

        private float m_progress = 0.0f;
        private Vector2 m_destination;

        public Charge(Rigidbody2D entityBody, float startSpeed, float endSpeed, Vector2 destination)
        {
            m_entityBody = entityBody;
            m_startSpeed = startSpeed;
            m_endSpeed = endSpeed;
            m_destination = destination;
        }

        public bool update()
        {
            if (!m_entityBody)
                return false;

            Vector2 curPos = m_entityBody.transform.position;
            Vector2 delta = m_destination - curPos;
            m_entityBody.velocity = delta.normalized * FUtil.regress(m_startSpeed, m_endSpeed, Mathf.Clamp(m_progress, 0f, 1f));
            m_progress += Time.deltaTime;

            if(Physics2D.Linecast(curPos, curPos + delta.normalized * 0.5f, 1 << LayerMask.NameToLayer("Ground")))
            {
                return false;
            }

            if (delta.magnitude <= 0.4f)
            {
                //m_airCharge = false;
                //m_marker.removeMark();
                m_entityBody.velocity = delta.normalized * m_endSpeed;
                return false;
            }

            return true;
        }
    }

    [SerializeField]
    float m_startChargeSpeed = 10.0f;

    [SerializeField]
    float m_endChargeSpeed = 4.0f;

    [SerializeField]
    float m_groundStartChargeSpeed = 5.0f;

    [SerializeField]
    float m_groundEndChargeSpeed = 1.0f;

    private Rigidbody2D m_playerBody;

    private DestinationMarker m_marker;

    private Charge m_currentCharge;
    private Vector2 m_airChargeDestination;
    private Color m_chargeColor = new Color(0.0f / 255.0f, 255.0f / 255.0f, 0.0f, 1);
    private Color m_idleColor = Color.white;

    private const float m_boostEffectDuration = 0.5f;
    private float m_boostEffectTimeLeft = 0.5f;
    private bool m_boostEffectActive = false;

    private ParticleSystem m_boostEffect;
    private Transform m_cameraTarget;
    private float m_cameraTargetSpeedMultiplier = 0.002f;
    private Vector2 m_cameraTargetMaxPos = new Vector2(3.0f, 3.0f);

    // Use this for initialization
    void Start ()
    {
        m_playerBody = GetComponent<Rigidbody2D>();
        m_marker = GetComponent<DestinationMarker>();

        m_boostEffect = transform.Find("BoostEffect").GetComponent<ParticleSystem>();
        m_cameraTarget = transform.FindChild("CameraTarget");
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(m_boostEffectActive)
        {
            m_boostEffectTimeLeft -= Time.deltaTime;
            if (m_boostEffectTimeLeft <= 0.0f)
                boostEffect(false);
        }

        /*
        if(m_cameraTarget && m_playerBody)
        {
            float x = Mathf.Clamp(m_cameraTarget.localPosition.x + m_playerBody.velocity.x * m_cameraTargetSpeedMultiplier, -m_cameraTargetMaxPos.x, m_cameraTargetMaxPos.x);
            float y = Mathf.Clamp(m_cameraTarget.localPosition.y + m_playerBody.velocity.y * m_cameraTargetSpeedMultiplier, -m_cameraTargetMaxPos.y, m_cameraTargetMaxPos.y);

            m_cameraTarget.localPosition = new Vector2(x, 0.0f);
        }
        */
    }

    void FixedUpdate()
    {
        if (m_currentCharge != null)
        {
            if(!m_currentCharge.update())
            {
                m_currentCharge = null;
                m_marker.removeMark();
                //changeFireColor(m_idleColor);
            }
        }
    }

    public void initCharge(Vector2 start, Vector2 target, bool grounded)
    {
        m_airChargeDestination = target;
        m_marker.mark(target);

        if(grounded)
        {
            AudioSource chargeSound = m_boostEffect.GetComponent<AudioSource>();
            if (chargeSound && !chargeSound.isPlaying)
                chargeSound.Play();

            m_currentCharge = new Charge(m_playerBody, m_groundStartChargeSpeed, m_groundEndChargeSpeed, target);  
        }
        else
        {
            m_currentCharge = new Charge(m_playerBody, m_startChargeSpeed, m_endChargeSpeed, target);
            AudioSource chargeSound = GetComponent<AudioSource>();
            if (chargeSound)
                chargeSound.Play();
        }  

        //changeFireColor(m_chargeColor);
        boostEffect(!grounded);
    }

    private void changeFireColor(Color color)
    {
        var particleSystems = GetComponentsInChildren<ParticleSystem>();
        foreach(var ps in particleSystems)
        {
            if(ps.name == "Flames")
            {
                ps.startColor = color;
            }
        }
    }

    private void boostEffect(bool trigger)
    {
        m_boostEffectTimeLeft = m_boostEffectDuration;

        var particleSystems = GetComponentsInChildren<ParticleSystem>();
        m_boostEffectActive = trigger;

        //if (trigger)
            //Instantiate(m_boostEffect, transform.position, Quaternion.identity);

        m_boostEffect.enableEmission = trigger;
        
        /*
        foreach (var ps in particleSystems)
        {
            if (ps.name == "Fire")
            {
                ps.startColor = trigger ? m_chargeColor : m_idleColor;
                ps.emissionRate = trigger ? 30 : 15;
            }

            if (ps.name == "Flames")
            {
                ps.startColor = trigger ? m_chargeColor : m_idleColor;
                ps.maxParticles = trigger ? 50 : 25;
                ps.emissionRate = trigger ? 45 : 25;
            }
        }
        */
    }

    public void OnTriggerExit2D(Collider2D other)
    {

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        Burnable burnable = other.GetComponent<Burnable>();
        if (GetComponent<SelfDestruction>() == null && burnable && !burnable.IsBurning())
        {
            HeatmapEvent.Send("BurnedVegetation2", transform.position, Time.timeSinceLevelLoad);
            KHeatmap.Log("BurnedVegetation2", transform.position);
            burnable.Burn();
        }

        //if (other.GetComponent<ChargeRecharge>())
            //boostEffect(true);
    }

    public void OnCollisionExit2D(Collision2D collision)
    {

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        /*
        Vector2 curPos = transform.position;
        Vector2 delta = m_airChargeDestination - curPos;
        if(m_playerBody.velocity.magnitude > m_endChargeSpeed)
            m_playerBody.velocity = delta.normalized * m_endChargeSpeed;

        m_currentCharge = null;
        m_marker.removeMark();
        */
    }
}
