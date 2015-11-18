using UnityEngine;
using System.Collections;

public class WinTrigger : MonoBehaviour
{
    [SerializeField]
    int m_winLevel = 2;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerPrefs.SetFloat("Player Time", Time.timeSinceLevelLoad);
        PlayerPrefs.Save();
        Application.LoadLevel(m_winLevel);
    }
}
