using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour
{
	public Transform ball;

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButtonDown (0)) {
			Vector3 pos = Input.mousePosition;
			pos.z = 5f;
			pos = camera.ScreenToWorldPoint (pos);
			Instantiate (ball, pos, Quaternion.identity);
		}
	}
}
