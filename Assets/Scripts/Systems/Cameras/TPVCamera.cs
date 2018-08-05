// BEGIN MIT LICENSE BLOCK //
//
// Copyright (c) 2016 dskjal
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//
// END MIT LICENSE BLOCK   //

using System;
using Characters.Player;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class TPVCamera : MonoBehaviour {
	public Transform Target;
	public float DistanceToPlayerM = 2f;    // カメラとプレイヤーとの距離[m]
	public float SlideDistanceM = 0f;       // カメラを横にスライドさせる；プラスの時右へ，マイナスの時左へ[m]
	public float HeightM = 1.2f;            // 注視点の高さ[m]
	public float RotationSensability = 300f;
	public bool isFPS = false;

	private bool isBacking = false;

	/// <summary>
	/// 背景のレイヤーマスク
	/// </summary>
	private int bgLayerMask;
	
	/// <summary>
	/// 注目しているプレイヤーのステートマネージャ
	/// </summary>
	public PlayerStateManager StateMan;

	/// <summary>
	/// rotYの補正値
	/// </summary>
	private float correctYRotVal;

	public float CorrectYRotVal {
		get { return correctYRotVal; }
		set { correctYRotVal = value; }
	}
	
	/// <summary>
	/// マウス入力を有効にするか
	/// </summary>
	private bool mouseActivated = true;

	public bool MouseActivated {
		get { return mouseActivated; }
		set { mouseActivated = value; }
	}
	
	/// <summary>
	/// 高さ補正値
	/// </summary>
	private float heightCorrectVal;

	public float HeightCorrectVal {
		get { return heightCorrectVal; }
		set { heightCorrectVal = value; }
	}

	void Start () {
		if(Target == null) {
			Debug.LogError("ターゲットが設定されていない");
			Application.Quit();
		}

		bgLayerMask = LayerMask.GetMask("BackGround");
	}
	
	void FixedUpdate () {
		var lookAt = Target.position + Vector3.up * (HeightM - heightCorrectVal);

		tpvCamChanging(lookAt);

		if (Input.GetButtonDown("ChangeViewpoint")) {
			//FPS視点化
			isFPS = !isFPS;
		}
		
	}
	
	/// <summary>
	/// マウス操作や反動によってtpvカメラを回転させます
	/// </summary>
	/// <param name="correctionVal">角度補正値</param>
	/// <param name="mouseActivated">マウスによる回転を適用するならtrue</param>
	/// <returns>注視点</returns>
	void tpvCamChanging(Vector3 lookAt) {
		float mouseMoveVal = (StateMan.isMouseActivated) ? Input.GetAxis("Mouse Y")  * RotationSensability : 0;
		
		//correctionValはangle指定することがあるので自前でdeltaTimeをかける
		var rotY = mouseMoveVal * Time.deltaTime - CorrectYRotVal;
		
		// カメラがプレイヤーの真上や真下にあるときにそれ以上回転させないようにする
		if(transform.forward.y > 0.5f && (rotY < 0 || rotY > 90)) {
			rotY = 0;
		}
		if(transform.forward.y < -0.5f && (rotY > 0 || rotY < -90)) {
			rotY = 0;
		}

		transform.RotateAround(lookAt, transform.right, rotY);
		
		//なんかの拍子にカメラが反対側行ったら戻す
		if (Math.Abs(Target.forward.normalized.z - transform.forward.normalized.z) >= 1) {
			transform.RotateAround(lookAt,Target.up,180);
		}
		
		// カメラを横にずらして中央を開ける値を計算
		var slide = transform.right * SlideDistanceM;

		if (!isFPS) {
			// カメラとプレイヤーとの間の距離を調整
			transform.position = lookAt - transform.forward * DistanceToPlayerM;
			
			// 注視点の設定
			transform.LookAt(lookAt);
			
			
			//距離の調整
			adjustToSeePL(slide);
		}else{
			//FPS視点ならFPS視点に変更
			transform.position = Target.position + new Vector3(0, 1.5f, 0);
		}
		
		transform.position += slide;
	}
	
	/// <summary>
	/// PLとの間に物があってPLが見えなくなる時、カメラを近づけます
	/// </summary>
	/// <param name="lookAt">PL</param>
	void adjustToSeePL(Vector3 slide) {
		//カメラからプレイヤーにレイを飛ばす
		RaycastHit hit;
		if (Physics.Linecast(Target.position + new Vector3(0, 1.5f, 0), transform.position + slide, out hit, bgLayerMask)) {
			transform.position = Target.position + new Vector3(0, 1.5f, 0);
		}
	}
}