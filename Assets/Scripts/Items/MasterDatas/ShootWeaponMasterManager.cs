using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootWeaponMasterManager : MasterDataManager{

	private List<ShootWeaponBuilder> builders = new List<ShootWeaponBuilder>();

	public static readonly ShootWeaponMasterManager INSTANCE = new ShootWeaponMasterManager();

	private ShootWeaponMasterManager() {
		base.loadData("MasterDatas/WeaponMasterData");
		loadBuilder();
	}
	
	/// <summary>
	/// データを読み込んでbuilderとしてListに組み込みます
	/// </summary>
	void loadBuilder() {
		var max = MaxId;
		for (int i = 0; i < max; i++) {
			ShootWeaponBuilder builder = new ShootWeaponBuilder();
			
			//各値を読み込み
			builder.Id = int.Parse(getRawParam(i, "ID"));
			builder.Name = getRawParam(i, "Name");
			builder.Ammo = int.Parse(getRawParam(i, "Ammo"));
			builder.Damage = float.Parse(getRawParam(i, "Damage"));
			builder.FireRate = float.Parse(getRawParam(i, "FireRate"));
			builder.NeedStr = int.Parse(getRawParam(i, "NeedSTR"));
			builder.Range = float.Parse(getRawParam(i, "Range"));
			builder.Recoil = float.Parse(getRawParam(i, "Recoil"));
			builder.Weight = float.Parse(getRawParam(i, "Weight"));
			builder.ZoomRate = float.Parse(getRawParam(i, "ZoomRate"));
			builder.IsAutomatic = (getRawParam(i,"isAutomatic") == "TRUE");
			builder.ReloadSec = float.Parse(getRawParam(i, "ReloadSec"));
			builder.WeaponPrefab = (GameObject) Resources.Load(getRawParam(i, "ObjectPath"));
			
			builders.Add(builder);
		}
	}
	
	/// <summary>
	/// idから武器を生成します
	/// </summary>
	/// <param name="id">生成したい武器のID</param>
	/// <returns>生成した武器のGameObject</returns>
	public GameObject creatWeapon(int id) {
		var builder = builders[id];
		GameObject weapon = MonoBehaviour.Instantiate(builder.WeaponPrefab);
		weapon.GetComponent<ShootWeapon>().setBuilder(builder);
		return weapon;
	}
	
	/// <summary>
	/// idから武器を生成します
	/// </summary>
	/// <param name="id">生成したい武器のID</param>
	/// <param name="pos">生成した武器の希望するPosition</param>
	/// <param name="rotation">生成した武器の希望するRotation</param>
	/// <returns>生成した武器のGameObject</returns>
	public GameObject creatWeapon(int id, Vector3 pos, Quaternion rotation) {
		var builder = builders[id];
		GameObject weapon = MonoBehaviour.Instantiate(builder.WeaponPrefab,pos,rotation);
		weapon.GetComponent<ShootWeapon>().setBuilder(builder);
		return weapon;
	}
}
