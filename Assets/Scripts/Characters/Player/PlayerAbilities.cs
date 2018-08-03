using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour {
	private int MAX_ABILITY = 50;
	
	/// <summary>
	/// 筋力：リロード速度、反動制御、近接攻撃力に関係
	/// 一部武器の装備条件
	/// </summary>
	private int strength;
	
	/// <summary>
	/// 技巧：命中円の大きさと収縮速度、足音の消音性に関係
	/// </summary>
	private int technich;
	
	/// <summary>
	/// 敏捷性：足の早さ、気づかれる確率、近接武器の振りの速さに関係
	/// </summary>
	private int agility;
	
	/// <summary>
	/// 体力：HP、重量に関係
	/// </summary>
	private int toughness;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	/// <summary>
	/// リロード速度の補正
	/// 単位は割合
	/// </summary>
	/// <returns>補正値(0f~1f)</returns>
	float getReloadRate() {
		//リロード補正は1倍~最大で0.5倍なので-0.01x+1.01
		return -0.01f * strength + 1.01f;
	}

	float getRecoilControll() {
		return 0f;
	}
}
