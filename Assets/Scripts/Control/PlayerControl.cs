using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    private bool grounded = true;
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
                airChargeReady = true;

            if (airChargeReady)
            {
                airChargeReady = grounded;
                var target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                playerSkills.initCharge(transform.position, target, grounded);  
            }
        }
    }

    void FixedUpdate()
    {
    }

    public void recharge()
    {
        airChargeReady = true;
    }
}
