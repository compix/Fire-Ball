using UnityEngine;
using System.Collections;

public class ResetVelocity : MonoBehaviour {

    public Transform targetTransform;

    private const float waitTime = 5f;
    private float waitTimer = 0f;

	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
        waitTimer -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (waitTimer <= 0f)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetTransform.position - transform.position, 3.0f, 1 << LayerMask.NameToLayer("Magnet")); // 1 << LayerMask.NameToLayer("Ground")
            if (hit.collider != null && hit.rigidbody != null)
            {
                hit.rigidbody.velocity = new Vector2(0f, 0f);
                waitTimer = waitTime;
            }
        }
    }
}
