using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonInput : MonoBehaviour
{

    public FixedJoystick LeftJoystick;
    protected PlayerController Control;

    // Start is called before the first frame update
    void Start()
    {
        Control = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        Control.Hinput = LeftJoystick.Horizontal;
        Control.Vinput = LeftJoystick.Vertical;

    }
}
