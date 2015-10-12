using UnityEngine;
using System.Collections;

public class Magnet : MonoBehaviour
{
    public Vector2 direction;

    private float force = 5.0f;

    private const float forceTime = 0.05f;
    private float forceTimer = 0.05f;
    private const float hitTime = 0.5f;
    private float hitTimer = 0.1f;

    // Use this for initialization
    void Start()
    {
        direction.x = Mathf.Cos(transform.rotation.z);
        direction.y = Mathf.Sin(transform.rotation.z);
    }

    // Update is called once per frame
    void Update()
    {
        forceTimer -= Time.deltaTime;
    }

    public void FixedUpdate()
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 3.0f); // 1 << LayerMask.NameToLayer("Ground")
        if (hit.collider != null)
        {
            hitTimer -= Time.deltaTime;
            if(hitTimer <= 0.0f)
            {
                hit.rigidbody.velocity = direction * force;
                hitTimer = hitTime;
            }
            

            //hit.rigidbody.AddForce(direction * force);
        }
    }
}
