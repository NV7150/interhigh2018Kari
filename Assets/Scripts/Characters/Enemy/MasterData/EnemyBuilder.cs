using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBuilder : MonoBehaviour {
    /// <summary>
    /// ID
    /// </summary>
    private int id;
    
    /// <summary>
    /// 名前
    /// </summary>
    private string name;
    
    /// <summary>
    /// 筋力
    /// </summary>
    private int str;
    
    /// <summary>
    /// 敏捷
    /// </summary>
    private int agi;
    
    /// <summary>
    /// 技巧
    /// </summary>
    private int tech;
    
    /// <summary>
    /// 体力
    /// </summary>
    private int tough;

    /// <summary>
    /// 使用武器のID
    /// </summary>
    private int weaponId;

    /// <summary>
    /// プレファブの名前
    /// </summary>
    private GameObject prefab;

    public int Id {
        get { return id; }
        set { id = value; }
    }

    public string Name {
        get { return name; }
        set { name = value; }
    }

    public int Str {
        get { return str; }
        set { str = value; }
    }

    public int Agi {
        get { return agi; }
        set { agi = value; }
    }

    public int Tech {
        get { return tech; }
        set { tech = value; }
    }

    public int Tough {
        get { return tough; }
        set { tough = value; }
    }

    public int WeaponId {
        get { return weaponId; }
        set { weaponId = value; }
    }

    public GameObject Prefab {
        get { return prefab; }
        set { prefab= value; }
    }
}
