using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour
{

    public Transform playerTransform;

    private float followSpeed = 4.0f;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update ()
	{

	}

    void FixedUpdate()
    {
        float x = Mathf.Lerp(transform.position.x, playerTransform.position.x, Time.deltaTime * followSpeed);
        float y = Mathf.Lerp(transform.position.y, playerTransform.position.y, Time.deltaTime * followSpeed);

        transform.position = new Vector3(x, y, transform.position.z);
    }
}
