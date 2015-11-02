using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;

    private float followSpeed = 4.0f;
    private Camera m_camera;

    // Use this for initialization
    void Start ()
    {
        m_camera = GetComponent<Camera>();
    }
	
	// Update is called once per frame
	void Update ()
	{
        //if (target)
        //{
        //    Vector3 point = m_camera.WorldToViewportPoint(target.position);
        //    Vector3 delta = target.position - m_camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
        //    Vector3 destination = transform.position + delta;
        //    transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        //}


    }

    void LateUpdate()
    {
        float x = Mathf.Lerp(transform.position.x, target.position.x, Time.deltaTime * followSpeed);
        float y = Mathf.Lerp(transform.position.y, target.position.y, Time.deltaTime * followSpeed);

        transform.position = new Vector3(x, y, transform.position.z);
    }
}
