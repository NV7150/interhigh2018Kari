using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputInfo {
	private bool buttonPressDown;
	private bool buttonPressing;
	private bool buttonPressUp;

	public bool ButtonPressDown {
		get { return buttonPressDown; }
		set { buttonPressDown = value; }
	}

	public bool ButtonPressing {
		get { return buttonPressing; }
		set { buttonPressing = value; }
	}

	public bool ButtonPressUp {
		get { return buttonPressUp; }
		set { buttonPressUp = value; }
	}
}
