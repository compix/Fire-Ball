using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;

public class KHeatmap : MonoBehaviour
{
    //[SerializeField]
    //Transform m_visMesh;

    [SerializeField]
    List<string> m_eventsToVisualize = new List<string>();

    [SerializeField]
    List<Transform> m_eventVisualizationMeshes = new List<Transform>();

    [SerializeField]
    bool m_visualize = false;

    [SerializeField]
    Text m_heatpmapPathText;

    [SerializeField]
    bool m_showHeatmapLogPath = false;

    [SerializeField]
    Camera m_camera;

    [SerializeField]
    bool m_makeScreenshotsAtStart = false;

    private float m_waitTimeBeforeScreenshot = 2.0f;
    private const int m_totalScreenShotNum = 10;
    private int m_curScreenShotNum = 0;

    GameObject m_parent;

    static string m_basePath;

    // Use this for initialization
    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
            m_basePath = "/sdcard/Dot_Games/Fire/Log"; // Application.persistentDataPath + "/Log";//
        else
            m_basePath = "Log";

        if(m_showHeatmapLogPath)
            m_heatpmapPathText.text = "Heatmap Log Path: " + m_basePath;

        makeDir(m_basePath);

        if(m_visualize)
        {
            if (m_parent)
                foreach (Transform child in m_parent.GetComponent<Transform>())
                    Destroy(child);

            foreach (string eventName in m_eventsToVisualize)
                Visualize(eventName);
        }

        m_camera.transform.position = new Vector3(3.5f, 2.66f, -9.0f);
    }

    static void makeDir(string dir)
    {
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);
    }

    // Update is called once per frame
    void Update()
    {
        if(m_makeScreenshotsAtStart)
        {
            m_waitTimeBeforeScreenshot -= Time.deltaTime;
            if (m_waitTimeBeforeScreenshot > 0.0f)
                return;

            if (m_curScreenShotNum == 0)
                Time.timeScale = 0.0f;

            if (m_curScreenShotNum < m_totalScreenShotNum)
            {
                Application.CaptureScreenshot("Heatmap/S" + m_curScreenShotNum + ".png");
                Vector3 camPos = m_camera.transform.position;
                m_camera.transform.position = new Vector3(m_curScreenShotNum * m_camera.orthographicSize * 2.0f * m_camera.aspect, camPos.y, camPos.z);
                m_curScreenShotNum++;

                if (m_curScreenShotNum == m_totalScreenShotNum)
                    Time.timeScale = 1.0f;
            }
        }
    }

    public static void Log(string eventName, Vector3 pos)
    {
        var stream = File.AppendText(makePath(eventName));

        if(stream == null)
        {
            Debug.Log("Could not log " + eventName + " because the stream for the file could not be created: " + makePath(eventName));
            return;
        }

        stream.WriteLine(pos);
        stream.Close();
    }

    public static void clear(string eventName)
    {
        File.WriteAllText(makePath(eventName), string.Empty);
    }

    private static string makePath(string eventName)
    {
        return m_basePath + "/" + eventName + ".log";
    }

    private static Vector3 ToVector3(string vec3AsString)
    {
        string subStr = vec3AsString.Substring(1, vec3AsString.Length - 2);
        string[] split = subStr.Split(',');
        return new Vector3(float.Parse(split[0]), float.Parse(split[1]), float.Parse(split[2]));
    }

    public void Visualize(string eventName)
    {
        string path = makePath(eventName);

        Transform visMesh = m_eventVisualizationMeshes[m_eventsToVisualize.IndexOf(eventName)];
        if (!visMesh)
            return;

        if (!File.Exists(path))
            return;

        string[] strVectors = File.ReadAllLines(path);

        if (!m_parent)
        {
            m_parent = new GameObject();
            m_parent.name = "HeatmapVisualization";
        }

        foreach (string strVec in strVectors)
            CreateVisMesh(visMesh, ToVector3(strVec));
    }

    private void CreateVisMesh(Transform visMesh, Vector3 pos)
    {
        Transform mesh = (Transform)Instantiate(visMesh, pos, Quaternion.identity);
        mesh.parent = m_parent.GetComponent<Transform>();
    }
}
