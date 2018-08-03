using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Player;
using UnityEngine;
using UnityEngine.UI;

public class AimCursor : MonoBehaviour {
	/// <summary>
	/// 各カーソルのImageオブジェクト
	/// </summary>
	public Image cursorUp;
	public Image cursorDown;
	public Image cursorRight;
	public Image cursorLeft;

	/// <summary>
	/// プレイヤのGameObject
	/// </summary>
	public GameObject player;
	
	/// <summary>
	/// カメラ
	/// </summary>
	public Camera cam;

	public CameraAimer aimer;
	
	/// <summary>
	/// PlayerShootingSystemのコンポーネント
	/// </summary>
	private PlayerShootingSystem shootSys;
	
	/// <summary>
	/// エイムフォーカスコンポーネント
	/// </summary>
	private AimForcusSystem aimForcusSys;
	
	/// <summary>
	/// 通常時の画面命中半径
	/// </summary>
	private float radius;
	
	/// <summary>
	/// 通常時の最大射程命中円の半径
	/// </summary>
	private float shootRadius;

	/// <summary>
	/// 通常時の最大射程スクリーンの高さ
	/// </summary>
	private float hitScrHeight;
	
	/// <summary>
	/// エイム時の最大射程スクリーンの高さ
	/// </summary>
	private float aimIngHitScrHeight;
	
	/// <summary>
	/// エイム時の画面命中円の半径
	/// </summary>
	private float aimIngRadius;

	private RectTransform rect;

	private PlayerStateManager _stateMan;

	public PlayerRecoilManager recoilMan;

	// Use this for initialization
	void Start () {
		rect = GetComponent<RectTransform>();
		shootSys = player.GetComponent<PlayerShootingSystem>();
		aimForcusSys = player.GetComponent<AimForcusSystem>();
		_stateMan = player.GetComponent<PlayerStateManager>();
		
		//各種値を計算
		//通常時の最大射程スクリーンの高さ
		hitScrHeight = shootSys.shootRange * 2.0f * Mathf.Tan(aimer.NonAimZoomFov * 0.5f * Mathf.Deg2Rad);
		//通常時の最大射程命中円の半径
		shootRadius = shootSys.ShootWide + shootSys.ShootRange / 10;
		//通常時の画面命中円の半径
		radius = shootRadius / hitScrHeight * Screen.height;
		
		//エイム時の最大射程スクリーンの高さ
		aimIngHitScrHeight = shootSys.ShootRange * 2.0f * Mathf.Tan(aimer.AimZoomFov * 0.5f * Mathf.Deg2Rad);
		//エイム時の画面命中円半径
		aimIngRadius = shootRadius / aimIngHitScrHeight * Screen.height;
	}
	
	// Update is called once per frame
	void Update () {
		
		float screenRad;
		if (shootSys.IsAimCap) {
			//エイム制限にかかった場合はその分補正して半径を計算
			var aimCapShootRadius = shootRadius * shootSys.RealAimDistanceToObject / shootSys.RockDistance;
			var scrHeight = ((_stateMan.IsAiming) ? aimIngHitScrHeight : hitScrHeight);
			screenRad = aimCapShootRadius / scrHeight  * Screen.height * shootSys.CurrentAimCorrection + 6.25f;
		} else {
			screenRad = (_stateMan.IsAiming) ?  aimIngRadius : radius;
			//最終計算
			screenRad *= shootSys.CurrentAimCorrection;
			screenRad += 6.25f;
		}
		
		//各オブジェクトに反映
		cursorUp.rectTransform.localPosition = new Vector3(0,screenRad);
		cursorDown.rectTransform.localPosition = new Vector3(0,-screenRad);
		cursorRight.rectTransform.localPosition = new Vector3(screenRad,0);
		cursorLeft.rectTransform.localPosition = new  Vector3(-screenRad,0);

		if (_stateMan.IsRecoilEffecting) {
			rect.localPosition = new Vector3(0,computeRecoilOffset());
		} else {
			rect.localPosition = new Vector3(0,0);
		}
	}


	float computeRecoilOffset() {
		var worldSin = Mathf.Sin(recoilMan.Recoiled * Mathf.PI / 180) * shootSys.RockDistance;
		var fov = (_stateMan.IsAiming) ? aimer.AimZoomFov : aimer.NonAimZoomFov;
		var sinHeight = shootSys.RockDistance * 2.0f * Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad);
		return worldSin / sinHeight * Screen.height;
	}
}
