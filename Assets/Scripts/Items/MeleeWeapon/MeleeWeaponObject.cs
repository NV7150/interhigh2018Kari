using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponObject : MonoBehaviour {

	public Animator animator;
	
	/// <summary>
	/// 武器の持ち主が攻撃中かどうか
	/// </summary>
	private bool isAttacking;
	
	/// <summary>
	/// 武器の持ち主にとって敵と判断するオブジェクトのタグ
	/// </summary>
	private string enemytag;

	private MeleeSystem meleeSys;
	
	
	public MeleeSystem MeleeSys {
		set { meleeSys = value; }
	}

	public string Enemytag {
		set { enemytag = value; }
	}

	// Update is called once per frame
	void FixedUpdate () {
		//攻撃状況を更新
		isAttacking = animator.GetBool("Attacking");
	}	
	
	private void OnTriggerEnter(Collider other) {
		//当たって攻撃中かつ敵だったら着弾処理
		if (isAttacking) {
			if (other.gameObject.CompareTag(enemytag)) {
				meleeSys.hit(other.gameObject);
			}
		}
	}
}
