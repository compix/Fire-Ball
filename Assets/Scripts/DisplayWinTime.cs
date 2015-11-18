using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisplayWinTime : MonoBehaviour
{
    [SerializeField]
    Text m_text;

    // Use this for initialization
    void Start ()
    {
        m_text.text = "Time: " + PlayerPrefs.GetFloat("Player Time");
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
