﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class EquipmentManager : MonoBehaviour {
	/// <summary>
	/// キャラクターの手の部分のGameObject
	/// </summary>
	public GameObject hand;

	private Animator anim;
	
	/// <summary>
	/// 現在装備中の武器
	/// </summary>
	private Weapon currentWeapon;
	
	/// <summary>
	/// 現在装備中の武器がShootWeaponであったらその武器が入る
	/// </summary>
	private ShootWeapon currentShootWeapon;
	
	/// <summary>
	/// 現在装備中の武器がMeleeWeaponだったらその武器が入る
	/// </summary>
	private MeleeWeapon currentMeleeWeapon;
	
	/// <summary>
	/// 生成済みオブジェクトのプール
	/// 基本的にまた装備する可能性があるもの（インベントリにまだ入っているもの）がプールされる
	/// </summary>
	private Dictionary<int, GameObject> createdObjects = new Dictionary<int, GameObject>();
	
	
	public Weapon CurrentWeapon {
		get { return currentWeapon; }
	}

	public WeaponType CurrentWeaponType {
		get { return currentWeapon.WeaponType; }
	}

	public ShootWeapon CurrentShootWeapon {
		get {
			var shootWeapon = (CurrentWeaponType == WeaponType.SHOOT) ? currentShootWeapon : null;
			return shootWeapon;
		}
	}

	public MeleeWeapon CurrentMeleeWeapon {
		get {
			var meleeWeapon = (CurrentWeaponType == WeaponType.MELEE) ? currentMeleeWeapon : null;
			return meleeWeapon;
		}
	}

	protected abstract WeaponSwitcher Switcher { get; }
	protected abstract MeleeSystem MeleeSys { get; }
	/// <summary>
	/// 敵と判断するゲームオブジェクトのタグ
	/// </summary>
	protected abstract string EnemyTag { get; }
	
	protected virtual void Awake() {
		anim = GetComponent<Animator>();
	}

	/// <summary>
	/// 該当武器を装備します
	/// </summary>
	/// <param name="weapon">装備したい武器</param>
	public virtual void equip(Weapon weapon) {
		//一度装備解除
		disarm();
		
		currentWeapon = weapon;
		
		registerWeapon(weapon);
		activateObject(weapon.UniqueId);
		
		if (CurrentWeaponType == WeaponType.SHOOT) {
			currentShootWeapon = (ShootWeapon) weapon;
			var obj = weapon.WeaponObj.GetComponent<ShootWeaponObject>();
			//パラメータの設定
			Switcher.switchShoot(obj.aimTransform,obj.shootFrom);
			
			//位置設定
			rotateShootWeapon(obj);
			moveToGrip(obj.gameObject,obj.girp.position);
		} else {
			currentMeleeWeapon = (MeleeWeapon) weapon;
			var obj = weapon.WeaponObj.GetComponent<MeleeWeaponObject>();
			
			//パラメータの設定
			obj.animator = anim;
			obj.Enemytag = EnemyTag;
			obj.MeleeSys = MeleeSys;
 			Switcher.switchMelee();
			
			//位置設定
			rotateMeleeWeapon(obj);
			moveToGrip(obj.gameObject,obj.grip.position);
		}
		
		//アニメータを編集
		anim.runtimeAnimatorController = Instantiate(weapon.WeaponAnim);
	}
	
	/// <summary>
	/// 装備を解除し、素手になります
	/// </summary>
	public void disarm() {
		if (currentWeapon != null) {
			deactivateObject(currentWeapon.UniqueId);
			currentWeapon = null;
		}
	}
	
	/// <summary>
	/// 武器を生成済みのディクショナリに保存します
	/// </summary>
	/// <param name="weapon">保存したいWeaponオブジェクト</param>
	/// <returns>武器が生成済みでなかった時true、生成済みであった場合はfalseを返す</returns>
	void registerWeapon(Weapon weapon) {
		//ユニークIDを元にすでにオブジェクトが生成済みかどうかを検索
		if (!createdObjects.ContainsKey(weapon.UniqueId)) {
			//生成してなかったら生成してプールに加える
			weapon.creatObject();
			var weaponObject = weapon.WeaponObj;
			//一応activeはfalseに
			weaponObject.SetActive(false);
			weaponObject.transform.parent = hand.transform;
			
			createdObjects.Add(weapon.UniqueId,weaponObject);
		}
	}
	
	/// <summary>
	/// 対象の武器オブジェクトをactive化します
	/// </summary>
	/// <param name="uniqueId">active化したい武器のユニークID</param>
	void activateObject(int uniqueId) {
		createdObjects[uniqueId].SetActive(true);
	}
	
	/// <summary>
	/// 対象の武器オブジェクトを非active化します
	/// </summary>
	/// <param name="uniqueId">非active化したい武器のユニークID</param>
	void deactivateObject(int uniqueId) {
		createdObjects[uniqueId].SetActive(false);
	}
	
	/// <summary>
	/// 対象のオブジェクトを生成済みDictionaryから削除します
	/// </summary>
	/// <param name="uniqueId">削除したい武器オブジェクトのユニークID</param>
	/// <returns>削除が成功したか</returns>
	public bool removeWeapon(int uniqueId) {
		return createdObjects.Remove(uniqueId);
	}

	void rotateMeleeWeapon(MeleeWeaponObject meleeWeapon) {
		meleeWeapon.transform.rotation = Quaternion.FromToRotation(meleeWeapon.tipForward, hand.transform.up);
	}

	void rotateShootWeapon(ShootWeaponObject shootWeapon) {
		shootWeapon.transform.rotation = Quaternion.LookRotation(hand.transform.forward,hand.transform.up);
	}

	void moveToGrip(GameObject weapon,Vector3 gripPos) {
		var difference = hand.transform.position - gripPos;
		weapon.transform.position += difference;
	}
	
}

