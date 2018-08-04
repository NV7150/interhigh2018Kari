using System.Collections;
using System.Collections.Generic;
using Characters.Player;
using UnityEngine;

public class CameraAimer : MonoBehaviour {
	/// <summary>
	/// エイムズームする速度
	/// </summary>
	public float aimZoomSpeed = 10.0f;
	
	/// <summary>
	/// エイムズームによる高さ補正の速さ
	/// </summary>
	public float aimHeightCorrectSpeed = 0.001f;
	
	/// <summary>
	/// エイムズーム時の最高到達field of view
	/// </summary>
	public float aimZoomFov = 30f;
	
	public float AimZoomFov {
		get { return aimZoomFov; }
	}
	
	/// <summary>
	/// エイムズームしていない時の最高到達field of view
	/// </summary>
	public float nonAimZoomFov = 60f;

	public float NonAimZoomFov {
		get { return nonAimZoomFov; }
	}

	/// <summary>
	/// エイムズーム時のカメラの高さ補正最大値
	/// </summary>
	public float aimHeightCorrection = 0.2f;

	/// <summary>
	/// カメラコンポーネント
	/// </summary>
	private Camera cam;
	
	/// <summary>
	/// カメラのTPVCameraコンポーネント
	/// </summary>
	private TPVCamera tpvCam;

	/// <summary>
	/// プレイヤーのステートマネージャ
	/// </summary>
	public PlayerStateManager StateMan;
	
	// Use this for initialization
	void Start () {
		cam = GetComponent<Camera>();
		tpvCam = GetComponent<TPVCamera>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (StateMan.IsAiming) {
			aimZoom();
		} else {
			releaseAimZoom();
		}
	}
	
	/// <summary>
	/// エイムズームします
	/// </summary>
	void aimZoom() {
		//ズーム
		if (cam.fieldOfView > aimZoomFov) {
			cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, aimZoomFov, aimZoomSpeed);
		}
		
		//高さ補正
		if (tpvCam.HeightCorrectVal < aimHeightCorrection) {
			tpvCam.HeightCorrectVal = Mathf.Lerp(tpvCam.HeightCorrectVal, aimHeightCorrection, aimHeightCorrectSpeed);
		}
	}

	/// <summary>
	/// エイムズームを解除します
	/// </summary>
	void releaseAimZoom() {
		//ズームの解除
		if (cam.fieldOfView < nonAimZoomFov) {
			cam.fieldOfView = Mathf.Lerp(cam.fieldOfView,nonAimZoomFov,aimZoomSpeed);
		}
		
		//高さ補正の解除
		if (tpvCam.HeightCorrectVal > 0f) {
			tpvCam.HeightCorrectVal = Mathf.Lerp(tpvCam.HeightCorrectVal, 0f, aimHeightCorrectSpeed);
		}
	}
}
