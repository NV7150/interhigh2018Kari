using System.Collections;
using System.Collections.Generic;
using RootMotion.Demos;
using UnityEngine;

public abstract class MeleeSystem : MonoBehaviour{
	/// <summary>
	/// アニメータ
	/// </summary>
	private Animator animator;
	
	protected abstract float attackSpeedRate { get; }

	protected virtual void Awake() {
		animator = GetComponent<Animator>();
		Debug.Log(animator);
	}

	/// <summary>
	/// 攻撃が命中した時の処理
	/// </summary>
	/// <param name="hitObj">命中した対象のGameObject。敵であることは保証されている</param>
	public abstract void hit(GameObject hitObj);
	
	/// <summary>
	/// 攻撃処理
	/// </summary>
	public virtual void attack() {
		//攻撃速度を調整
		animator.speed *= attackSpeedRate;
		//攻撃アニメーション再生
		animator.SetTrigger("Attack");
	}
	
}
