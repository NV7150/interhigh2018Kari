using System;
using System.Collections;
using System.Collections.Generic;
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
	/// <summary>
	/// 弾の最大射程で、カメラに映っている四角形の高さ
	/// </summary>
	private float hitSclHeight;
	/// <summary>
	/// 弾の最大射程での命中範囲の半径
	/// </summary>
	private float hitSclRad;
	
	
	// Use this for initialization
	void Start () {
		shootSys = player.GetComponent<PlayerShootingSystem>();
		//各種値を計算
		hitSclHeight = shootSys.ShootRange * 2.0f * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
		hitSclRad = Mathf.Sin(shootSys.shootWide * Mathf.PI / 180f) * shootSys.ShootRange;
	}
	
	// Update is called once per frame
	void Update () {
		//最大射程でのカメラに映る範囲の高さ / 命中円の半径 * UIスクリーン高さ ＝ UIスクリーンの高さに対する命中円の半径の大きさ
		var radius = hitSclRad / hitSclHeight * Screen.height;
		
		//最大射程に対してどれくらいの距離の物体にロックしてるか計算
		float distRate = shootSys.RockDistance / shootSys.ShootRange;
		//命中円の半径を計算
		float screenRad = distRate * radius + 6.25f;
		//各オブジェクトに反映
		cursorUp.rectTransform.localPosition = new Vector3(0,screenRad);
		cursorDown.rectTransform.localPosition = new Vector3(0,-screenRad);
		cursorRight.rectTransform.localPosition = new Vector3(screenRad,0);
		cursorLeft.rectTransform.localPosition = new  Vector3(-screenRad,0);
	}
	
}
