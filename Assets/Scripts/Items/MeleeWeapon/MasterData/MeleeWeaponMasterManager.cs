using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponMasterManager : MasterDataManager {
	public static readonly MeleeWeaponMasterManager INSTANCE = new MeleeWeaponMasterManager();
	
	/// <summary>
	/// 生成済みビルダー
	/// </summary>
	private List<MeleeWeaponBuilder> builders = new List<MeleeWeaponBuilder>();

	private MeleeWeaponMasterManager() {
		base.loadData("MasterDatas/MeleeWeaponMasterData");
		loadBuilder();
	}

	private void loadBuilder() {
		for (int i = 0; i < MaxId; i++) {
			var builder = new MeleeWeaponBuilder();
			
			builder.Id = int.Parse(getRawParam(i, "ID"));
			builder.Name = getRawParam(i, "Name");
			builder.Damage = float.Parse(getRawParam(i, "Damage"));
			builder.WeponPrefab = Resources.Load<GameObject>("Prefabs/" + getRawParam(i, "PrefabName"));
			builder.WeaponAnim =
				Object.Instantiate(Resources.Load<RuntimeAnimatorController>("Animators/" + getRawParam(i, "AnimatorName")));
			
			builders.Add(builder);
		}
	}
	
	/// <summary>
	/// 指定IDの近接武器を生成して返します
	/// </summary>
	/// <param name="id">生成したい武器のID</param>
	/// <returns>生成した武器</returns>
	public MeleeWeapon creatWeapon(int id) {
		return builders[id].getWeapon();
	}
}
