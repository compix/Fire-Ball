using UnityEngine;
using System.Collections;

public class PlayerSkills : MonoBehaviour
{
    [SerializeField]
    float startChargeVelocity = 10.0f;

    [SerializeField]
    float endChargeVelocity = 4.0f;

    private Rigidbody2D playerBody;

    private float moveSpeed = 3.0f;
    private float jumpForce = 300.0f;

    private bool airCharge = false;
    private Vector2 airChargeDestination = new Vector2();
    private Vector2 airChargeStart = new Vector2();
    private float airChargeProgress = 0.0f;

    private DestinationMarker marker;

    // Use this for initialization
    void Start ()
    {
        playerBody = GetComponent<Rigidbody2D>();
        marker = GetComponent<DestinationMarker>();
    }
	
	// Update is called once per frame
	void Update ()
    {

    }

    void FixedUpdate()
    {
        if (airCharge)
            charge();
    }

    float regress(float start, float end, float t)
    {
        return start + (Mathf.Sqrt(t) * 2.0f - t) * (end - start);
    }

    public void move(float horizontalMovement, bool jump)
    {
        if (!airCharge && !Mathf.Approximately(horizontalMovement, 0.0f))
            playerBody.velocity = new Vector2(horizontalMovement * moveSpeed, playerBody.velocity.y);

        if (jump)
            playerBody.AddForce(new Vector2(0.0f, jumpForce));
    }

    private void charge()
    {
        Vector2 curPos = transform.position;
        Vector2 delta = airChargeDestination - curPos;
        playerBody.velocity = delta.normalized * regress(startChargeVelocity, endChargeVelocity, Mathf.Clamp(airChargeProgress, 0f, 1f));
        airChargeProgress += Time.deltaTime;

        if (delta.magnitude <= 0.4f)
        {
            airCharge = false;
            marker.removeMark();
        }
    }

    public void initCharge(Vector2 start, Vector2 target)
    {
        airCharge = true;
        airChargeStart = start;
        airChargeDestination = target;
        airChargeProgress = 0.0f;
        marker.mark(target);
    }

    public void OnTriggerExit2D(Collider2D other)
    {

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Burnable")
        {
            other.GetComponent<Burnable>().burn();
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        airCharge = false;
        marker.removeMark();
    }
}
