using UnityEngine;
using System.Collections;

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

    // Use this for initialization
    void Start ()
    {
        m_playerBody = GetComponent<Rigidbody2D>();
        m_marker = GetComponent<DestinationMarker>();
    }
	
	// Update is called once per frame
	void Update ()
    {

    }

    void FixedUpdate()
    {
        if (m_currentCharge != null)
        {
            if(!m_currentCharge.update())
            {
                m_currentCharge = null;
                m_marker.removeMark();
            }
        }
    }

    public void initCharge(Vector2 start, Vector2 target, bool grounded)
    {
        m_airChargeDestination = target;
        m_marker.mark(target);

        if(grounded)
            m_currentCharge = new Charge(m_playerBody, m_groundStartChargeSpeed, m_groundEndChargeSpeed, target);
        else
            m_currentCharge = new Charge(m_playerBody, m_startChargeSpeed, m_endChargeSpeed, target);
    }

    public void OnTriggerExit2D(Collider2D other)
    {

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (GetComponent<SelfDestruction>() == null && other.tag == "Burnable")
        {
            other.GetComponent<Burnable>().burn();
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 curPos = transform.position;
        Vector2 delta = m_airChargeDestination - curPos;
        if(m_playerBody.velocity.magnitude > m_endChargeSpeed)
            m_playerBody.velocity = delta.normalized * m_endChargeSpeed;

        m_currentCharge = null;
        m_marker.removeMark();
    }
}
