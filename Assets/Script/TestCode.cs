using UnityEngine;
using UnityEngine.InputSystem;

public class TestCode : MonoBehaviour
{
	[SerializeField] private InputActionProperty select;

	public void OnClickButton()
	{
		Debug.Log("Button Click");
	}

	public void Update()
	{
		var ss = select.action.ReadValue<float>();

		if (ss != 0)
			Debug.Log("Click");
	}
}
