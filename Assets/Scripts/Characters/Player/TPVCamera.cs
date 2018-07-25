// BEGIN MIT LICENSE BLOCK //
//
// Copyright (c) 2016 dskjal
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//
// END MIT LICENSE BLOCK   //

using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class TPVCamera : MonoBehaviour {
	public Transform Target;
	public float DistanceToPlayerM = 2f;    // カメラとプレイヤーとの距離[m]
	public float SlideDistanceM = 0f;       // カメラを横にスライドさせる；プラスの時右へ，マイナスの時左へ[m]
	public float HeightM = 1.2f;            // 注視点の高さ[m]
	public float RotationSensitivity = 300f;// 感度
	
	void Start () {
		if(Target == null) {
			Debug.LogError("ターゲットが設定されていない");
			Application.Quit();
		}
	}

	void FixedUpdate () {
		var rotY = Input.GetAxis("Mouse Y") * Time.deltaTime * RotationSensitivity;

		var lookAt = Target.position + Vector3.up * HeightM;
		
		// カメラがプレイヤーの真上や真下にあるときにそれ以上回転させないようにする
		if(transform.forward.y > 0.5f && rotY < 0) {
			rotY = 0;
		}
		if(transform.forward.y < -0.5f && rotY > 0) {
			rotY = 0;
		}
		transform.RotateAround(lookAt, transform.right, rotY);
		
		//なんかの拍子にカメラが反対側行ったら戻す
		if (Math.Abs(Target.forward.normalized.z - transform.forward.normalized.z) >= 1) {
			transform.RotateAround(lookAt,Target.up,180);
		}

		// カメラとプレイヤーとの間の距離を調整
		transform.position = lookAt - transform.forward * DistanceToPlayerM;

		// 注視点の設定
		transform.LookAt(lookAt);

		// カメラを横にずらして中央を開ける
		transform.position = transform.position + transform.right * SlideDistanceM;
	}
}