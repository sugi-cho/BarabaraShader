using UnityEngine;
using System.Collections;

public class BarabaraBall : MonoBehaviour
{
	Material m;

	void Start ()
	{
		m = renderer.material;
		m.SetFloat ("_T", Time.timeSinceLevelLoad);
		Destroy (gameObject, 10f);
	}

	void OnDestroy ()
	{
		if (m != null)
			Destroy (m);
	}
}
