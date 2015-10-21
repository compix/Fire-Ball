using UnityEngine;
using System.Collections;

public class SelfDestruction : MonoBehaviour
{
    public float lifeDuration = 5.0f;

    private float maxParticleLifetime = 0.0f;

    // Use this for initialization
    private IEnumerator Start ()
    {
        var systems = GetComponentsInChildren<ParticleSystem>();

        foreach (var system in systems)
            maxParticleLifetime = Mathf.Max(system.startLifetime, maxParticleLifetime);

        float destructionTime = Time.time + lifeDuration;

        while (Time.time < destructionTime)
            yield return null;

        // Turn off emission
        foreach (var system in systems)
            system.enableEmission = false;

        // Wait for any remaining particles to expire
        yield return new WaitForSeconds(maxParticleLifetime);

        Destroy(gameObject);
    }
	
	// Update is called once per frame
	void Update ()
    {
	    
	}
}
