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
	
	/// <summary>
	/// PlayerShootingSystemのコンポーネント
	/// </summary>
	private PlayerShootingSystem shootSys;

	private AimForcusSystem aimForcusSys;
	
	/// <summary>
	/// 弾の最大射程で、カメラに映っている四角形の高さ
	/// </summary>
	private float hitSclHeight;

	private float radius;
	
	
	// Use this for initialization
	void Start () {
		shootSys = player.GetComponent<PlayerShootingSystem>();
		aimForcusSys = player.GetComponent<AimForcusSystem>();
		
		//各種値を計算
		hitSclHeight = shootSys.ShootRange * 2.0f * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
		var shootRadius = shootSys.ShootWide * shootSys.ShootRange / 10;
		radius = shootRadius / hitSclHeight * Screen.height;
	}
	
	// Update is called once per frame
	void Update () {
		//最大射程時の命中円の半径 / 最大射程でのカメラに映る範囲の高さ * UIスクリーンの高さ ＝ UIスクリーンの高さに対する命中円の半径の大きさ
//		var shootRadius = shootSys.ShootWide * shootSys.ShootRange / 10 * 1.0f/*aimForcusSys.CurrentForcusRate*/;
//		var radius = (shootRadius) / hitSclHeight * Screen.height;
		
		
		//ロックしてる部分による補正値
//		var distRate = shootSys.RockDistance / shootSys.ShootRange
		
		//最終計算
		float screenRad = radius  * aimForcusSys.CurrentForcusRate + 6.25f;
		
		//各オブジェクトに反映
		cursorUp.rectTransform.localPosition = new Vector3(0,screenRad);
		cursorDown.rectTransform.localPosition = new Vector3(0,-screenRad);
		cursorRight.rectTransform.localPosition = new Vector3(screenRad,0);
		cursorLeft.rectTransform.localPosition = new  Vector3(-screenRad,0);
	}
	
}
