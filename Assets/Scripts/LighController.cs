using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LighController : MonoBehaviour
{

    Light gameLight = null;
    [SerializeField] private float nextLightToggle = 0f;
    [SerializeField] private bool IsDay = false;
    private float lightIntensity = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        gameLight = gameObject.GetComponent<Light>();
        nextLightToggle = Time.unscaledTime + 1;
        gameLight.intensity = 0.1f;

    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.controler.state == GameController.STATES.INTRO)
        {
            lightIntensity = 0.1f; 
        }

        else if (GameController.controler.state == GameController.STATES.RUNNING)
        {
            if (Time.unscaledTime > nextLightToggle)
            {
                if (!IsDay)
                {
                    lightIntensity = 0.8f;
                    IsDay = true;
                    nextLightToggle = Time.unscaledTime + 100f;
                }
                else
                {
                    lightIntensity = 0.1f;
                    IsDay = false;
                    nextLightToggle = Time.unscaledTime + 100f;
                }
               
            }
        }
        else if (GameController.controler.state == GameController.STATES.GAMEOVER)
        {
            lightIntensity = 0f;
            IsDay = false;
        }

        ControlLight(lightIntensity);

    }

    void ControlLight(float end_intensity)
    {
        if (gameLight.intensity < end_intensity)
        {
            gameLight.intensity += 0.001f;
        }
        else if (gameLight.intensity > end_intensity)
        {
            gameLight.intensity -= 0.001f;
        }
    }
}
