using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightScript : MonoBehaviour
{

    Light phoneLight;
    float current;
    float last;
    void Start()
    {
        phoneLight = this.gameObject.GetComponent<Light>();
        last = phoneLight.range;
        phoneLight.range = 0;
        current = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            phoneLight.range = last;
            last = current;
            current = phoneLight.range;
        }
    }
}
