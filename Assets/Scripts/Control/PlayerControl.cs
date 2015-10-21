using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    [SerializeField]
    float startChargeVelocity = 10.0f;

    [SerializeField]
    float endChargeVelocity = 4.0f;

    private Rigidbody2D playerBody;
    public Transform groundCheckTransform;

    private float moveSpeed = 3.0f;
    private float jumpForce = 300.0f;

    private bool grounded = true;
    private bool jump = false;
    private bool airCharge = false;
    private bool airChargeReady = false;
    private Vector2 airChargeDestination = new Vector2();
    private Vector2 airChargeStart = new Vector2();
    private float airChargeProgress = 0.0f;

    // Use this for initialization
    void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics2D.Linecast(transform.position, groundCheckTransform.position, 1 << LayerMask.NameToLayer("Ground"));

        if (Input.GetButtonDown("Jump"))
        {
            if (grounded)
                jump = true;
            else if (airChargeReady)
            {
                airChargeReady = false;
                airCharge = true;
                airChargeDestination = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                airChargeStart = transform.position;
                airChargeProgress = 0.0f;
            }
        }
    }

    void FixedUpdate()
    {
        float horizontalAxis = Input.GetAxis("Horizontal");

        if (!airCharge && !Mathf.Approximately(horizontalAxis, 0.0f))
            playerBody.velocity = new Vector2(horizontalAxis * moveSpeed, playerBody.velocity.y);

        if (jump)
        {
            playerBody.AddForce(new Vector2(0.0f, jumpForce));
            jump = false;
            airChargeReady = true;
        }

        if (airCharge)
        {
            Vector2 curPos = transform.position;
            Vector2 delta = airChargeDestination - curPos;
            playerBody.velocity = delta.normalized * regress(startChargeVelocity, endChargeVelocity, Mathf.Clamp(airChargeProgress, 0f, 1f));
            airChargeProgress += Time.deltaTime;

            if (delta.magnitude <= 0.4f)
            {
                airCharge = false;
            }
        }
    }

    float regress(float start, float end, float t)
    {
        return start + (Mathf.Sqrt(t) * 2.0f - t) * (end - start);
    }

    public void OnTriggerExit2D(Collider2D other)
    {

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Burnable")
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
    }

    public void recharge()
    {
        airChargeReady = true;
    }
}
