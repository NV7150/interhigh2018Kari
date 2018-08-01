using System.Collections;
using System.Collections.Generic;
using Characters.Player;
using UnityEngine;

public class PlayerSneakSystem : MonoBehaviour {
	/// <summary>
	/// 音が出ている範囲を示すSphereCollider
	/// </summary>
	private SphereCollider soundCol;
	
	/// <summary>
	/// 隠密状態でない時かつ静止時のsoundColの半径
	/// </summary>
	public float normalStaySoundRange = 3.0f;
	
	/// <summary>
	/// 隠密状態でない時、移動した場合にsoundColが何倍されるか
	/// </summary>
	public float normalMoveSoundMag = 2.0f;
	
	/// <summary>
	/// 隠密状態時、範囲が何倍されるか((1 - sneakMag) * 100% 分縮小する)
	/// </summary>
	public float sneakMag = 0.8f;

	private PlayerStateManager stateMan;
	
	// Use this for initialization
	void Start () {
		soundCol = GetComponent<SphereCollider>();
		soundCol.radius = normalStaySoundRange;
		stateMan = GetComponent<PlayerStateManager>();
	}
	
	// Update is called once per frame
	void Update () {
		float radius = normalStaySoundRange;
		
		if (Input.GetButton("Sneak")) {
			//隠密状態にする
			radius *= sneakMag;
			stateMan.IsSneaking = true;
		} else {
			stateMan.IsSneaking = false;
		}

		if (stateMan.IsMoving) {
			radius *= normalMoveSoundMag;
		}

		soundCol.radius = radius;
	}
}
