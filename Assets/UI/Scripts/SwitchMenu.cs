using UnityEngine;
using System.Collections;

public class SwitchMenu : MonoBehaviour
{
    [SerializeField]
    bool m_visibleAtStart;

    [SerializeField]
    Canvas m_nextMenu;

    Canvas m_prevMenu;

	// Use this for initialization
	void Start ()
    {
        Debug.Assert(GetComponent<Canvas>());

        if(!m_visibleAtStart)
        {
            GetComponent<Canvas>().enabled = false;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void Next()
    {
        if(m_nextMenu)
        {
            SwitchMenu next = m_nextMenu.GetComponent<SwitchMenu>();
            if(next)
            {
                next.SetPrevMenu(GetComponent<Canvas>());
            }

            GetComponent<Canvas>().enabled = false;
            m_nextMenu.enabled = true;
        }
    }

    public void Back()
    {
        if (m_prevMenu)
        {
            GetComponent<Canvas>().enabled = false;
            m_prevMenu.enabled = true;
        }
    }

    public void SetNextMenu(Canvas nextMenu)
    {
        m_nextMenu = nextMenu;
    }

    public void SetPrevMenu(Canvas prevMenu)
    {
        m_prevMenu = prevMenu;
    }
}
