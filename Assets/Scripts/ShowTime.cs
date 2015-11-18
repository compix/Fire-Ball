using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShowTime : MonoBehaviour
{
    [SerializeField]
    Text m_timeText;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!m_timeText)
            return;

        m_timeText.text = "Time: " + Mathf.Floor(Time.timeSinceLevelLoad);
	}
}
