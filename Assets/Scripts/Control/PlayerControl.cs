using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    private bool grounded = true;
    private bool jump = false;
    private bool airCharge = false;
    private bool airChargeReady = false;

    
    private PlayerSkills playerSkills;
    public Transform groundCheckTransform;

    // Use this for initialization
    void Start()
    {
        playerSkills = GetComponent<PlayerSkills>();
        
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
                var target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                playerSkills.initCharge(transform.position, target);  
            }
        }
    }

    void FixedUpdate()
    {
        float horizontalAxis = Input.GetAxis("Horizontal");

        if (jump)
            airChargeReady = true;

        playerSkills.move(horizontalAxis, jump);

        jump = false;
    }

    public void recharge()
    {
        airChargeReady = true;
    }
}
