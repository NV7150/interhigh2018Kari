using System.Collections;
using System.Collections.Generic;
using Characters.Player;
using UnityEngine;

public class ShootWeapon : MonoBehaviour {
	private int id;
	private string name;
	private float recoil;
	private float range;
	private float damage;
	private int ammo;
	private float zoomRate;
	private int needStr;
	private float weight;
	private float fireSec;
	private float reloadSec;
	private bool isAutomatic;

	/// <summary>
	/// この射撃武器のID
	/// </summary>
	public int Id {
		get { return id; }
	}

	/// <summary>
	/// この射撃武器の名前
	/// </summary>
	public string Name {
		get { return name; }
	}

	/// <summary>
	/// この射撃武器の反動
	/// 目安は50f ~ 150f
	/// </summary>
	public float Recoil {
		get { return recoil; }
	}

	/// <summary>
	/// 射撃武器の射程
	/// 単位はunity距離
	/// </summary>
	public float Range {
		get { return range; }
	}

	/// <summary>
	/// この射撃武器の威力
	/// 単位はHP(割合)
	/// </summary>
	public float Damage {
		get { return damage; }
	}

	/// <summary>
	/// この射撃武器の装弾数
	/// 単位は個数
	/// </summary>
	public int Ammo {
		get { return ammo; }
	}

	/// <summary>
	/// この射撃武器のズーム率
	/// 単位は割合(乗算なので1-ZoomRate縮まる)
	/// </summary>
	public float ZoomRate {
		get { return zoomRate; }
	}

	/// <summary>
	/// この射撃武器を装備するのに必要な筋力値
	/// 単位は筋力
	/// </summary>
	public int NeedStr {
		get { return needStr; }
	}

	/// <summary>
	/// この射撃武器の重さ
	/// 未定
	/// </summary>
	public float Weight {
		get { return weight; }
	}

	/// <summary>
	/// この射撃武器の連続発射性能
	/// 単位は秒
	/// </summary>
	public float FireSec {
		get { return fireSec; }
	}
	
	/// <summary>
	/// この射撃武器が連射式か否か
	/// </summary>
	public bool IsAutomatic {
		get { return isAutomatic; }
	}

	public float ReloadSec {
		get { return reloadSec; }
	}

	/// <summary>
	/// データを入れます
	/// </summary>
	public void setBuilder(ShootWeaponBuilder builder) {
		id = builder.Id;
		name = builder.Name;
		recoil = builder.Recoil;
		range = builder.Range;
		damage = builder.Damage;
		ammo = builder.Ammo;
		zoomRate = builder.ZoomRate;
		needStr = builder.NeedStr;
		weight = builder.Weight;
		fireSec = builder.FireRate;
		reloadSec = builder.ReloadSec;
		isAutomatic = builder.IsAutomatic;
	}
}
