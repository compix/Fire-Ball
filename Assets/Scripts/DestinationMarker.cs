using UnityEngine;
using System.Collections;

public class DestinationMarker : MonoBehaviour
{
    public Transform marker;
    //private Transform currentMarker;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void mark(Vector2 pos)
    {
        marker.gameObject.SetActive(true);
        marker.GetComponent<Transform>().position = pos;
        //Transform markerInstance = (Transform) Instantiate(marker, pos, Quaternion.identity);
    }

    public void removeMark()
    {
        marker.gameObject.SetActive(false);
    }
}
