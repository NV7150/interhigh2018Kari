using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConfigs : MonoBehaviour {
	public float mouseRotationSensability = 300f;

	public float MouseRotationSensability {
		get { return mouseRotationSensability; }
		set { mouseRotationSensability = value; }
	}
}
