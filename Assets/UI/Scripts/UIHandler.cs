using UnityEngine;
using System.Collections;

public class UIHandler : MonoBehaviour
{
    [SerializeField]
    PlayerControl m_playerControl;

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void LoadLevel(int level)
    {
        Application.LoadLevel(level);
        ResumeGame();
    }

    public void PauseGame()
    {
        if (m_playerControl)
            m_playerControl.enabled = false;

        Time.timeScale = 0.0f;
    }

    public void ResumeGame()
    {
        if (m_playerControl)
            m_playerControl.enabled = true;

        Time.timeScale = 1.0f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
