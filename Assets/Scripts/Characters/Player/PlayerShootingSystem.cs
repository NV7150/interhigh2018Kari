using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEngine;

public class PlayerShootingSystem : MonoBehaviour {
	/// <summary>
	/// 射撃の触れる幅
	/// 単位は割合（1でQuatanion1の範囲でブレる）
	/// </summary>
	public float shootWide;
	/// <summary>
	/// 射程
	/// 単位はベクトルと同じ
	/// </summary>
	public float shootRange;
	
	/// <summary>
	/// 銃口のオブジェクト
	/// </summary>
	public Transform shootFrom; 
	
	/// <summary>
	/// 銃弾オブジェクトのプレファブ
	/// </summary>
	public GameObject bulletPrefab;
	
	/// <summary>
	/// TPSカメラのオブジェクト
	/// </summary>
	public Camera cam;
	
	/// <summary>
	/// AimIK
	/// </summary>
	private AimIK ik;
	
	/// <summary>
	/// ロック中の物体への距離
	/// </summary>
	private float rockDistance;

	public float ShootWide {
		get { return shootWide; }
	}

	public float ShootRange {
		get { return shootRange; }
	}

	public float RockDistance {
		get { return rockDistance; }
	}

	// Use this for initialization
	void Start () {
		ik = GetComponent<AimIK>();
		rockDistance = shootRange;
	}
	
	// Update is called once per frame
	void Update () {
		searchAim();

		float atk = Input.GetAxis("Fire1");
		if(atk == 1)
			shoot();
		
	}
	
	/// <summary>
	/// 銃口の向き（AimIKの位置）を修正します。
	/// update毎に呼ばれます。
	/// </summary>
	void searchAim() {
		//カメラからRayを発信
		var ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
		RaycastHit hit;
		//とりあえずPlayer以外なら命中判定
		if (Physics.Raycast(ray, out hit,shootRange) && !hit.collider.gameObject.CompareTag("Player")) {
			//物体に当たればそっちを向く
			ik.solver.IKPosition = hit.point;
			//距離を更新
			rockDistance = Vector3.Distance(transform.position, hit.point);
		} else {
			//Rayの終端を計算//
			//元となるベクトル（射程）
			var vector = new Vector3(0,0,shootRange);
			//射程をカメラの回転度分回転
			vector = cam.transform.rotation * vector;
			//カメラのpositionを加算
			vector = cam.transform.position + vector;
			
			//Rayの終端を向く
			ik.solver.IKPosition = vector;
			//距離は最大距離
			rockDistance = shootRange;
		}
	}

	/// <summary>
	/// 射撃します
	/// </summary>
	void shoot() {
		//ランダムなベクトルを生成（弾はtransform.upで前進が前提）→正規化（最大ぶれ） * ランダム値（ぶれ率）
		var vector = new Vector3(Random.Range(-1f,1f),0,Random.Range(-1f,1f));
		vector = vector.normalized * Random.Range(0f,shootWide);
		//仮
		var obj = Instantiate(bulletPrefab,shootFrom.position,Quaternion.Euler(ik.solver.transform.rotation.eulerAngles + new Vector3(0,0,-90)));
		obj.transform.Rotate(vector);
	}
}
