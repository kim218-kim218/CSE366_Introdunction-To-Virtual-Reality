using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Action : MonoBehaviour
{
    public Text text;
    //public GameObject gameObject;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("STARTtttt");
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position += gameObject.transform.rotation * new Vector3(Input.GetAxis("Horizontal") * 0.1f, 0, Input.GetAxis("Vertical") * 0.1f);

        if (Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            text.text = 1.ToString();
            Debug.Log("Button 1");
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            text.text = 2.ToString();
            Debug.Log("Button 2");
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton3))
        {
            text.text = 3.ToString();
            Debug.Log("Button 3");
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            text.text = 0.ToString();
            Debug.Log("Button 0");
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log("Space key was pressed.");
        }

    }
}
