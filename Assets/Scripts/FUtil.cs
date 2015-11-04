using UnityEngine;
using System.Collections;

public class FUtil
{
    public static float regress(float start, float end, float t)
    {
        return start + (Mathf.Sqrt(t) * 2.0f - t) * (end - start);
    }
}
