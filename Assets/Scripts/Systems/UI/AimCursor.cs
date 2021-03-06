﻿using System;
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
	/// カメラのエイムズームコンポーネント
	/// </summary>
	public CameraAimer camAimer;

	/// <summary>
	/// PlayerShootingSystemのコンポーネント
	/// </summary>
	private PlayerShootingSystem shootSys;

	private PlayerAimer plAimer;
	
	private PlayerAbilities abilities;

	private ShootWeapon weapon;
	
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

	/// <summary>
	/// このエイムカーソルのRectTransform
	/// </summary>
	private RectTransform rect;
	
	/// <summary>
	/// プレイヤーのステートマネージャ
	/// </summary>
	private PlayerStateManager stateMan;

	private EquipmentManager equipMan;

	/// <summary>
	/// リコイルマネージャ
	/// </summary>
	public AimObjectRecoiler recoilMan;
	
	/// <summary>
	/// 変更感知用に覚えておくshootWide
	/// </summary>
	private float shootWideRem;
	
	/// <summary>
	/// 変更感知用に覚えておくScreenHeight
	/// </summary>
	private float scrHeightRem;

	private void Awake() {
		rect = GetComponent<RectTransform>();
		stateMan = player.GetComponent<PlayerStateManager>();
		abilities = player.GetComponent<PlayerAbilities>();
		equipMan = player.GetComponent<EquipmentManager>();
		plAimer = player.GetComponent<PlayerAimer>();
		shootSys = player.GetComponent<PlayerShootingSystem>();
	}

	// Use this for initialization
	void Start () {
		weapon = equipMan.CurrentShootWeapon;
		
		//各種値を計算
		//通常時の最大射程スクリーンの高さ
		hitScrHeight = weapon.Range * 2.0f * Mathf.Tan(camAimer.NonAimZoomFov * 0.5f * Mathf.Deg2Rad);
		//通常時の最大射程命中円の半径
		shootRadius = abilities.ShootWide + weapon.Range / 100;
		//通常時の画面命中円の半径
		radius = shootRadius / hitScrHeight * Screen.height;
		
		//エイム時の最大射程スクリーンの高さ
		aimIngHitScrHeight = weapon.Range * 2.0f * Mathf.Tan(camAimer.AimZoomFov * 0.5f * Mathf.Deg2Rad);
		//エイム時の画面命中円半径
		aimIngRadius = shootRadius / aimIngHitScrHeight * Screen.height;
		
		//変更感知用に覚えておく
		shootWideRem = abilities.ShootWide;
		scrHeightRem = Screen.height;
	}
	
	// Update is called once per frame
	void Update () {
		//変更を見る
		checkChange();
		
		float screenRad;
		if (plAimer.IsAimCap) {
			//エイム制限にかかった場合はその分補正して半径を計算
			var aimCapShootRadius = shootRadius * plAimer.RealAimDistToObj / plAimer.RockDist;
			var scrHeight = ((stateMan.IsAiming) ? aimIngHitScrHeight : hitScrHeight);
			screenRad = aimCapShootRadius / scrHeight  * Screen.height * shootSys.CurrentAimCorrection + 6.25f;
		} else {
			//それ以外は事前計算分を使用
			screenRad = (stateMan.IsAiming) ?  aimIngRadius : radius;
			//最終計算
			screenRad *= shootSys.CurrentAimCorrection;
			screenRad += 6.25f;
		}
		
		//各オブジェクトに反映
		cursorUp.rectTransform.localPosition = new Vector3(0,screenRad);
		cursorDown.rectTransform.localPosition = new Vector3(0,-screenRad);
		cursorRight.rectTransform.localPosition = new Vector3(screenRad,0);
		cursorLeft.rectTransform.localPosition = new  Vector3(-screenRad,0);
		
		//反動分エイムカーソルを上下
		if (stateMan.IsRecoilEffecting) {
			rect.localPosition = new Vector3(0,computeRecoilOffset());
		} else {
			rect.localPosition = new Vector3(0,0);
		}
	}
	
	/// <summary>
	/// リコイルによって発生する高さ補正を計算
	/// </summary>
	/// <returns>発生した高さ</returns>
	float computeRecoilOffset() {
		var worldSin = Mathf.Sin(recoilMan.Recoiled * Mathf.PI / 180) * plAimer.RockDist;
		var fov = (stateMan.IsAiming) ? camAimer.AimZoomFov : camAimer.NonAimZoomFov;
		var sinHeight = plAimer.RockDist * 2.0f * Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad);
		return worldSin / sinHeight * Screen.height;
	}

	void checkChange() {
		//武器変更感知
		if(equipMan.CurrentShootWeapon.Id != weapon.Id){
			weapon = equipMan.CurrentShootWeapon;
			reDefRange();
		}
		
		//命中円変更感知
		if (Math.Abs(abilities.ShootWide - shootWideRem) > 0.016f) {
			reDefWide();
		}
		
		//画面の大きさの変更感知
		if (Math.Abs(scrHeightRem - Screen.height) > 0.1f) {
			reDefRad();
		}
	}
	
	/// <summary>
	/// shootWideが変更された時に各値再定義します
	/// </summary>
	void reDefWide() {
		//通常時の最大射程命中円の半径
		shootRadius = abilities.ShootWide + weapon.Range / 100;
		//通常時の画面命中円の半径
		radius = shootRadius / hitScrHeight * Screen.height;
		
		//エイム時の画面命中円半径
		aimIngRadius = shootRadius / aimIngHitScrHeight * Screen.height;
		
		//変更感知用に覚えておく
		shootWideRem = abilities.ShootWide;
	}

	/// <summary>
	/// shootRangeが変更された(武器が変更された)時に各値を再定義します
	/// っていうか全部変わるので全部再定義します
	/// </summary>
	void reDefRange() {
		//通常時の最大射程スクリーンの高さ
		hitScrHeight = weapon.Range * 2.0f * Mathf.Tan(camAimer.NonAimZoomFov * 0.5f * Mathf.Deg2Rad);
		//通常時の最大射程命中円の半径
		shootRadius = abilities.ShootWide + weapon.Range / 100;
		//通常時の画面命中円の半径
		radius = shootRadius / hitScrHeight * Screen.height;
		
		//エイム時の最大射程スクリーンの高さ
		aimIngHitScrHeight = weapon.Range * 2.0f * Mathf.Tan(camAimer.AimZoomFov * 0.5f * Mathf.Deg2Rad);
		//エイム時の画面命中円半径
		aimIngRadius = shootRadius / aimIngHitScrHeight * Screen.height;
		
	}
	
	/// <summary>
	/// 画面の大きさが変更（正確には高さが）された時に各値を再定義します
	/// </summary>
	void reDefRad() {
		//通常時の画面命中円の半径
		radius = shootRadius / hitScrHeight * Screen.height;
		//エイム時の画面命中円半径
		aimIngRadius = shootRadius / aimIngHitScrHeight * Screen.height;
		
		//再記憶
		scrHeightRem = Screen.height;
	}
}
