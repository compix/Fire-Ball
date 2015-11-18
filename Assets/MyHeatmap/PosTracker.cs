using UnityEngine;
using System.Collections;

public class PosTracker : MonoBehaviour
{
    [SerializeField]
    float m_logTime = 0.5f;

    private float m_timer = 0.0f;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        m_timer += Time.deltaTime;
        
        if(m_timer >= m_logTime)
        {
            m_timer = 0.0f;
            KHeatmap.Log(name + "Pos", transform.position);
        }   
	}
}
