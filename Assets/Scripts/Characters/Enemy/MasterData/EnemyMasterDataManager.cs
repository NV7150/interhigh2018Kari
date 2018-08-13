using System.Collections;
using System.Collections.Generic;
using Characters.Enemy;
using UnityEngine;

public class EnemyMasterDataManager : MasterDataManager {
    /// <summary>
    /// シングルトン
    /// </summary>
    public static readonly EnemyMasterDataManager INSTANCE = new EnemyMasterDataManager();

    /// <summary>
    /// 生成済みEnemyBuilderのリスト
    /// </summary>
    private List<EnemyBuilder> builders = new List<EnemyBuilder>();
    
    private EnemyMasterDataManager() {
        loadData("MasterDatas/EnemyMasterData");
        loadBuilder();
    }
    
    void loadBuilder(){
        for (int i = 0; i < MaxId; i++) {
            //ビルダーを作成
            var builder = new EnemyBuilder();
            builder.Id = int.Parse(getRawParam(i, "ID"));
            builder.Name = getRawParam(i, "Name");
            builder.Str = int.Parse(getRawParam(i, "Strength"));
            builder.Tech = int.Parse(getRawParam(i, "Technic"));
            builder.Agi = int.Parse(getRawParam(i, "Agility"));
            builder.Tough = int.Parse(getRawParam(i, "Toughness"));
            builder.WeaponId = int.Parse(getRawParam(i, "WeaponID"));
            builder.Prefab = Resources.Load<GameObject>("Prefabs/" + getRawParam(i, "PrefabName"));
            //リストに登録
            builders.Add(builder);
        }
    }

    public GameObject getEnemy(int id) {
        //ビルダーから敵を生成
        var builder = builders[id];
        var enemyObj = MonoBehaviour.Instantiate(builder.Prefab);
        enemyObj.GetComponent<EnemyAbilities>().setBuilder(builder);
        enemyObj.GetComponent<EnemyEquipmentManager>().setBuilder(builder);

        return enemyObj;
    }
}
