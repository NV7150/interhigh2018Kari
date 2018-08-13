using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon {
	/// <summary>
	/// 武器１個体が固有に持つID
	/// </summary>
	private int uniqueId;

	/// <summary>
	/// 武器のID
	/// </summary>
	private int id;
	
	/// <summary>
	/// 武器の名前
	/// </summary>
	private string name;
	
	/// <summary>
	/// 武器の基礎ダメージ
	/// </summary>
	private float damage;
	
	/// <summary>
	/// 武器を使うときに使うアニメーター
	/// </summary>
	private RuntimeAnimatorController weaponAnim;
	
	/// <summary>
	/// 武器のプレファブ
	/// </summary>
	private GameObject objPrefab;
	
	/// <summary>
	/// 武器をオブジェクト化した場合、そのオブジェクト
	/// </summary>
	private GameObject currentObj;
	
	public WeaponType WeaponType {
		get { return WeaponType.MELEE; }
	}

	public int UniqueId {
		get { return uniqueId; }
	}
	public GameObject WeaponObj {
		get { return currentObj; }
	}

	public RuntimeAnimatorController WeaponAnim {
		get { return weaponAnim; }
	}

	public int Id {
		get { return id; }
	}

	public string Name {
		get { return name; }
	}

	public float Damage {
		get { return damage; }
	}

	public MeleeWeapon(MeleeWeaponBuilder builder) {
		uniqueId = WeaponHelper.NewestId;
		id = builder.Id;
		damage = builder.Damage;
		name = builder.Name;
		weaponAnim = builder.WeaponAnim;
		objPrefab = builder.WeponPrefab;
	}
	
	public void creatObject(Vector3 handPos) {
		currentObj = MonoBehaviour.Instantiate(objPrefab);
		currentObj.transform.position += handPos;
	}
}
