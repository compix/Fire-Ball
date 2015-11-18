using UnityEngine;
using System.Collections;

public class StartSoundDelayed : MonoBehaviour
{
    [SerializeField]
    ulong m_delay = 1;

	// Use this for initialization
	void Start ()
    {
        GetComponent<AudioSource>().Play(m_delay);
    }
	
	// Update is called once per frame
	void Update ()
    {
	}
}
