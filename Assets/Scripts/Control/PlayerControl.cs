using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

    public Rigidbody2D playerBody;
    public Transform groundCheckTransform;

    private float moveSpeed = 3.0f;
    private float moveForce = 15.0f;
    private float jumpForce = 300.0f;

    private bool grounded = true;
    private bool jump = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        grounded = Physics2D.Linecast(transform.position, groundCheckTransform.position, 1 << LayerMask.NameToLayer("Ground"));

        if (grounded && Input.GetButtonDown("Jump"))
            jump = true;
    }

    void FixedUpdate()
    {
        float horizontalAxis = Input.GetAxis("Horizontal");

        if(grounded)
            playerBody.velocity = new Vector2(horizontalAxis * moveSpeed, playerBody.velocity.y);
        else
            playerBody.AddForce(new Vector2(horizontalAxis * moveSpeed, 0.0f));
        //playerBody.velocity = new Vector2(horizontalAxis * moveSpeed, playerBody.velocity.y);

        if (jump)
        {
            playerBody.AddForce(new Vector2(0.0f, jumpForce));
            jump = false;
        } 
    }
}
