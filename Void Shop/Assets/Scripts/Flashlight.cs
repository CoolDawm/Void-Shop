using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    private bool _flashlightOn;
    public GameObject FlashlightObj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            FlashlightObj.SetActive(!_flashlightOn);
            _flashlightOn = !_flashlightOn;
        }
    }
}
