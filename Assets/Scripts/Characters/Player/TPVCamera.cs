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
	public float RotationSensitivity = 300f;// 感度

	/// <summary>
	/// 背景のレイヤーマスク
	/// </summary>
	private int bgLayerMask;
	
	/// <summary>
	/// 反動する加速度
	/// </summary>
	private float recoilAcceleration = 0f;
	
	/// <summary>
	/// 銃撃によって反動する最高の速さ
	/// </summary>
	private float recoilMax = 0f;

	/// <summary>
	/// 反動の現在の速さ
	/// </summary>
	private float recoilCurrentVelocity = 0f;

	/// <summary>
	/// 銃撃する前の角度(x-y平面)
	/// </summary>
	private float originalAngle;
	
	/// <summary>
	/// 銃撃後と銃撃前の角度差
	/// </summary>
	private float deltaAngle;
	
	/// <summary>
	/// 銃撃後と前の角度差が計算されているか
	/// </summary>
	private bool isDeltaAngleSetted = false;
	
	/// <summary>
	/// リコイル持続時間の残り
	/// 単位は秒
	/// </summary>
	private float recoilTime = 0f;

	/// <summary>
	/// 一回あたりのリコイル持続時間
	/// </summary>
	public float recoilTimeBase = 0.05f;

	/// <summary>
	/// 反動制御値
	/// </summary>
	public float recoilControll = 10f;

	public PlayerStateManager stateMan;
	
	void Start () {
		if(Target == null) {
			Debug.LogError("ターゲットが設定されていない");
			Application.Quit();
		}

		bgLayerMask = LayerMask.GetMask("BackGround");
	}

	void FixedUpdate () {
		float recoilval = 0;
		bool willMouseMove = true;
		//反動影響下ならば
		if (stateMan.IsRecoiling) {
			//反動値を計算
			if (stateMan.IsShooting || recoilTime > 0f) {
				//反動適用時間か射撃中なら反動適用
				recoilval = recoiling();
				recoilTime -= Time.deltaTime;
			} else {
				//そうでないなら反動制御
				recoilval = recoilControlling();
				willMouseMove = false;
			}
		}
		
		float mouseMoveVal = (willMouseMove) ? Input.GetAxis("Mouse Y")  * RotationSensitivity : 0;
		
		//recoilvalはangle指定することがあるので自前でdeltaTimeをかける
		var rotY = mouseMoveVal * Time.deltaTime  - recoilval;
		var lookAt = Target.position + Vector3.up * HeightM;
		
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

		// カメラとプレイヤーとの間の距離を調整
		transform.position = lookAt - transform.forward * DistanceToPlayerM;

		// 注視点の設定
		transform.LookAt(lookAt);

		// カメラを横にずらして中央を開ける
		transform.position = transform.position + transform.right * SlideDistanceM;
		
		//カメラとPLの間を調整
		adjustToSeePL(lookAt);
		
		
	}
	
	/// <summary>
	/// PLとの間に物があってPLが見えなくなる時、カメラを近づけます
	/// </summary>
	/// <param name="lookAt">PL</param>
	void adjustToSeePL(Vector3 lookAt) {
		//カメラからプレイヤーにレイを飛ばす
		Ray camToPlRay = new Ray(transform.position,Target.position + new Vector3(0,1.25f,0) - transform.position);
		RaycastHit hit;
		if (Physics.Raycast(camToPlRay,out hit)) {
			Debug.DrawLine(transform.position,hit.point);
			//プレイヤーに当たらなかったら
			if (!hit.collider.CompareTag("Player")) {
				Ray plTocamRay = new Ray(Target.position,transform.position - Target.position + new Vector3(0,1.25f,0) );
				RaycastHit nearestHit;
				if (Physics.Raycast(plTocamRay,out nearestHit,DistanceToPlayerM,bgLayerMask)) {
					//距離を調整
					transform.position = lookAt - transform.forward * nearestHit.distance;
				}
			}
		}
	}
	
	/// <summary>
	/// カメラを上に傾けて銃の反動状態にします
	/// </summary>
	/// <param name="recoil">反動値</param>
	public void recoilCemera(float recoilMax) {
		//反動制御フラグが初めてなら
		if (!stateMan.IsRecoiling) {
			//xz平面での向きを取得
			var horizontalVector = new Vector3(transform.forward.x,0,transform.forward.z);
			//y軸を含めた上下のみの角度を取得
			originalAngle = Vector3.Angle(horizontalVector, transform.forward);
			originalAngle *= (transform.forward.y > 0) ? 1 : -1;
			//反動関係制御フラグをつける
			stateMan.IsRecoiling = true;
		}
		//角度差計算済みフラグをリセット
		isDeltaAngleSetted = false;
		//反動の加速度を設定
		this.recoilAcceleration = recoilMax / 10;
		this.recoilMax = recoilMax;

		recoilTime = recoilTimeBase;
	}
	
	/// <summary>
	/// 反動状態から元に戻そうとします
	/// </summary>
	/// <returns>残っている反動値</returns>
	float recoilControlling() {
		recoilCurrentVelocity = 0;
		
		//角度差計算がまだなら
		if (!isDeltaAngleSetted) {
			//xz平面での向きを取得
			var horizontalVector = new Vector3(transform.forward.x, 0, transform.forward.z);
			//y軸を含めた上下のみの角度を取得
			var currntAngle = Vector3.Angle(horizontalVector, transform.forward);
			currntAngle *= (transform.forward.y > 0) ? 1 : -1;

			deltaAngle = currntAngle - originalAngle;
			isDeltaAngleSetted = true;
		}

		var recoilBack = recoilControll * Time.deltaTime;
		if (deltaAngle > recoilBack) {
			//反動制御力が反動角度差を下回ったら反動制御力分戻す
			deltaAngle -= recoilBack;
			return -recoilBack;
		} else {
			//上回ったら各種リセットをかけて完全に戻す
			stateMan.IsRecoiling = false;
			var angle = deltaAngle;
			deltaAngle = 0;
			return -angle;
		}
	}

	float recoiling() {
		recoilCurrentVelocity = Mathf.Lerp(recoilCurrentVelocity, recoilMax, recoilAcceleration);
		return recoilCurrentVelocity * Time.deltaTime;
	}
}