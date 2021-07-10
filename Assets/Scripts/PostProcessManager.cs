using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessManager : MonoBehaviour
{
    public static PostProcessManager instance = null;
    private Volume volume;
    private Vignette vignette;
    public float minVignette;
    public float maxVignette;
    // Start is called before the first frame update

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        volume = GetComponent<Volume>();
        VolumeProfile profile = volume.sharedProfile;
        profile.TryGet(out vignette);
    }

    public void AdjustVignette(float value)
    {
        vignette.intensity.value = value;
    }
}
