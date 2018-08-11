using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public interface Weapon{
    /// <summary>
    /// 武器の種別
    /// </summary>
    WeaponType WeaponType{ get; }
    
    /// <summary>
    /// ユニークID
    /// </summary>
    int UniqueId { get; }
    
    /// <summary>
    /// 武器オブジェクトで生成されたオブジェクト
    /// </summary>
    GameObject WeaponObj { get; }
    
    /// <summary>
    /// 武器装備時に使うアニメーター
    /// </summary>
    RuntimeAnimatorController WeaponAnim { get; }

    /// <summary>
    /// GameObjectを生成します
    /// </summary>
    void creatObject(Vector3 handPos);
}

/// <summary>
/// ユニークIDを提供するヘルパクラス
/// </summary>
public static class WeaponHelper {
    private static int newestId = 0;

    public static int NewestId {
        get {
            newestId++;
            return newestId;
        }
    }
}