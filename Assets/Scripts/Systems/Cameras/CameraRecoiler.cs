using System.Collections;
using System.Collections.Generic;
using Characters.Player;
using UnityEngine;

public class CameraRecoiler : MonoBehaviour {
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
	
	/// <summary>
	/// ステートマネージャ
	/// </summary>
	public PlayerStateManager stateMan;

	private TPVCamera tpvCam;

	private void Start() {
		tpvCam = GetComponent<TPVCamera>();
	}

	// Update is called once per frame
	void FixedUpdate () {
		tpvCam.CorrectYRotVal = updateRecoilVal();
	}
	
	float updateRecoilVal() {
		float recoilval = 0;
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
				tpvCam.MouseActivated = false;
				return recoilval;
			}
		}
		tpvCam.MouseActivated = true;

		return recoilval;
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
	
	/// <summary>
	/// 反動の早さを計算します
	/// </summary>
	/// <returns>反動の早さ</returns>
	float recoiling() {
		recoilCurrentVelocity = Mathf.Lerp(recoilCurrentVelocity, recoilMax, recoilAcceleration);
		return recoilCurrentVelocity * Time.deltaTime;
	}
}
