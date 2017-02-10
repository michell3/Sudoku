using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
																			//BUTTONS
	public static bool AButton()
	{
		return Input.GetButtonDown ("A_Button");
	}

	public static bool BButton()
	{
		return Input.GetButtonDown ("B_Button");
	}

	public static bool XButton()
	{
		return Input.GetButtonDown ("X_Button");
	}

	public static bool YButton()
	{
		return Input.GetButtonDown ("Y_Button");
	}


	public static bool LeftTrigger()									//TRIGGERS
	{
		return Input.GetButtonDown ("Left_Trigger");
	}

	public static bool RightTrigger()
	{
		return Input.GetButtonDown ("Right_Trigger");
	}


	public static float MainHorizontal()								//JOYSTICKS AXIS
	{
		float range = 0.0f;
		range += Input.GetAxis ("J_MainHorizontal");
		return Mathf.Clamp (range, -1.0f, 1.0f);
	}

	public static float MainVertical()
	{
		float range = 0.0f;
		range += Input.GetAxis ("J_MainVertical");
		return Mathf.Clamp (range, -1.0f, 1.0f);
	}

	public static Vector3 MainJoystick()
	{
		return new Vector3 (MainHorizontal(), MainVertical(), 0f);
	}
}
