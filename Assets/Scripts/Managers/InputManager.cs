using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;



public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public PlayerInputController inputControl;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }

        inputControl = new PlayerInputController();
    }

    private void OnEnable()
    {
        inputControl.Enable();
    }

    public void SetUIInput(bool _vis)
    {
        if(_vis)
        {
            Debug.Log("Active UI_Input");
            inputControl.UI.Enable();
        }
        else
        {
            Debug.Log("Ban UI_Input");
            inputControl.UI.Disable();
        }
    }

    public void SetMotorGamepad(float _duration,float lowFrequency,float highFrequency)
    {
        if(Gamepad.current==null)
        {
            return;
        }
        Gamepad.current.SetMotorSpeeds(lowFrequency, highFrequency);
        StartCoroutine(StopMotor(_duration));
    }

    IEnumerator StopMotor(float _duration)
    {
        yield return new WaitForSeconds(_duration);
        Gamepad.current.SetMotorSpeeds(0f, 0f);
    }
}