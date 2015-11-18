using UnityEngine;
using System.Collections;

public class CameraFitter : MonoBehaviour
{
    [SerializeField]
    bool landscape = true;
    [SerializeField]
    float preferredAspect = 16f / 9f;

    private Camera m_camera;
    private float preferredSize;
    private float currentAspect;

	// Use this for initialization
	void Start ()
    {
        m_camera = GetComponent<Camera>();
        Debug.Assert(m_camera);

        preferredSize = m_camera.orthographicSize;
    }

    private void adjustAspect()
    {
        m_camera = GetComponent<Camera>();
        Debug.Assert(m_camera);
        float aspect = m_camera.aspect;
        float invAspect = 1f / aspect;

        if (landscape)
            m_camera.orthographicSize = invAspect * preferredAspect * preferredSize;
        else
            m_camera.orthographicSize = preferredSize;

        currentAspect = aspect;
    }

	// Update is called once per frame
	void Update ()
    {
        if (currentAspect != m_camera.aspect)
            adjustAspect();
    }
}
