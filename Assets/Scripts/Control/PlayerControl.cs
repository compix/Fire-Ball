using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PlayerControl : MonoBehaviour
{
    [SerializeField]
    RectTransform rectTransform;

    [SerializeField]
    EventSystem eventSystem;

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

            var target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            KHeatmap.Log("TouchPoint", target);

            if (airChargeReady)
            {
                bool inRect = rectTransform.rect.Contains(rectTransform.worldToLocalMatrix.MultiplyPoint(target));

                if(inRect)
                {
                    airChargeReady = grounded;
                    playerSkills.initCharge(transform.position, target, grounded);
                }
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
