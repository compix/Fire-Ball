using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour
{
    [SerializeField]
    float radius = 2.0f;

    [SerializeField]
    float rotationSpeed = Mathf.PI * 0.1f;

    private float rotation;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        //rotation += rotationSpeed * Time.deltaTime;
        //transform.position = new Vector2(Mathf.Cos(rotation) * radius, Mathf.Sin(rotation) * radius);
        transform.Rotate(0.0f, 0.0f, rotationSpeed);
	}
}
