using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour
{
    [SerializeField]
    Transform target;

    [SerializeField]
    float minX;
    [SerializeField]
    float minY;
    [SerializeField]
    float maxX;
    [SerializeField]
    float maxY;

    [SerializeField]
    bool bounded = false;

    private float followSpeed = 4.0f;

    // Use this for initialization
    void Start ()
    {
        var camera = GetComponent<Camera>();
        var vertExtent = camera.orthographicSize;
        var horzExtent = vertExtent * Screen.width / Screen.height;

        /*
        minX += horzExtent;
        minY += vertExtent;
        maxX -= horzExtent;
        maxY -= vertExtent;
        */
    }
	
	// Update is called once per frame
	void Update ()
	{
    }

    void LateUpdate()
    {
        if(target != null)
        {
            float x = Mathf.Lerp(transform.position.x, target.position.x, Time.deltaTime * followSpeed);
            float y = Mathf.Lerp(transform.position.y, target.position.y, Time.deltaTime * followSpeed);

            transform.position = new Vector3(Mathf.Clamp(x, minX, maxX), Mathf.Clamp(y, minY, maxY), transform.position.z);
        }
    }
}
