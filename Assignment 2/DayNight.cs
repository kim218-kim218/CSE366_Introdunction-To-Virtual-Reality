using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNight : MonoBehaviour
{
    [SerializeField] private float timeDuration;
    public GameObject moon;
    public GameObject sun;
    public Light sunLight;
    public Light Bld1Light1;
    public Light Bld1Light2;
    public Light Bld2Light1;
    public Light Bld2Light2;
    public Light Bld3Light1;
    public Light Bld3Light2;
    public Light carlight1;
    public Light carlight2;
    private bool isNight = false;
    public Color dayFog;
    public Color nightFog;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        sunLight.transform.Rotate(Vector3.right, (180f / timeDuration) * Time.deltaTime);

        float t = 0.8f-Mathf.PingPong(Time.time / timeDuration, 1f);
        float exposureValue = Mathf.Lerp(0f, 1f, t);

        RenderSettings.skybox.SetFloat("_Exposure", exposureValue);


        if (sunLight.transform.eulerAngles.x >= 170)
        {
            isNight = true;
        }
        else if (sunLight.transform.eulerAngles.x <= 10)
        {
            isNight = false;
        }

        On_Off_light();
        
        
    }

    public void On_Off_light()
    {
        if (isNight)
        {
            carlight1.enabled = true;
            carlight2.enabled = true;
            Bld1Light1.enabled = true;
            Bld1Light2.enabled = true;
            Bld2Light1.enabled = true;
            Bld2Light2.enabled = true;
            Bld3Light1.enabled = true;
            Bld3Light2.enabled = true;
        }
        else
        {
            carlight1.enabled = false;
            carlight2.enabled = false;
            Bld1Light1.enabled = false;
            Bld1Light2.enabled = false;
            Bld2Light1.enabled = false;
            Bld2Light2.enabled = false;
            Bld3Light1.enabled = false;
            Bld3Light2.enabled = false;
        }
    }
}
