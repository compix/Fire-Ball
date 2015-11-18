using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class AtmosphereEffect : MonoBehaviour
{

    private Material material;

    void Awake()
    {
        material = new Material(Shader.Find("Hidden/Atmosphere"));
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //material.SetFloat("_bwBlend", intensity);
        Graphics.Blit(source, destination, material);
    }
}
