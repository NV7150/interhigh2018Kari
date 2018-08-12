using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShootingSystem : MonoBehaviour {
    /// <summary>
    /// 射撃を行う地点のTransform
    /// </summary>
    public Transform shootFrom;
    
    
    /// <summary>
    /// 発射可能までの秒数
    /// </summary>
    private float burstTimer;
    
    /// <summary>
    /// 残弾数
    /// </summary>
    private int remainingAmmo;
    
    
    /// <summary>
    /// 弾のばらけ具合
    /// </summary>
    public abstract float ShootWide { get; }
    
    /// <summary>
    /// 射程
    /// </summary>
    public abstract float ShootRange { get; }
    
    /// <summary>
    /// 通常時の発射間隔
    /// 単位は秒
    /// </summary>
    protected abstract float BurstTime { get; }
    
    /// <summary>
    /// リロードの基礎時間
    /// </summary>
    protected abstract float ReloadTime { get; }
    
    /// <summary>
    /// 能力値などによるリロードボーナス
    /// 単位は割合
    /// 実リロード時間はReloadTime * ReloadRateとなる
    /// つまり{(1 - ReloadRate) * 100}%秒数が軽減される
    /// </summary>
    protected abstract float ReloadRate { get; }
    
    /// <summary>
    /// 装弾限界
    /// </summary>
    protected abstract int maxAmmo { get; }


    private void Start() {
        //最大弾数に設定
        remainingAmmo = maxAmmo;
    }


    /// <summary>
    /// 弾が飛ぶベクトルを計算します
    /// </summary>
    /// <returns>弾が飛ぶベクトル</returns>
    protected Vector3 getBulletVector(float forcusRate) {
        //半径1の円の円周上のある一点をランダムに設定
        //そこに0~shootWideをかけることでランダムな平面円の中にある一点を生成
        //この時、射程によってshootWideを変化させることでshootWideとshootRangeのバランスが変わらない様にする
        var randomCerclePoint = new Vector2(Random.Range(-1.0f,1.0f),Random.Range(-1.0f,1.0f)).normalized * Random.Range(0f,(ShootWide + ShootRange / 100) * forcusRate);
		
        //上を三次元化して回転
        //rockDistanceじゃなくてshootRange分射撃するのでz軸はshootRange
        var vector = shootFrom.rotation * new Vector3(randomCerclePoint.x,randomCerclePoint.y,ShootRange);

        return vector;
    }
    
    /// <summary>
    /// 射撃後に次の発射間隔を決定します
    /// ここで残弾数処理もします
    /// </summary>
    protected void updateBurstTime() {
        remainingAmmo--;
        if (remainingAmmo > 0) {
            //残弾があったら発射間隔分タイマーをセット
            burstTimer = BurstTime;
        } else {
            //なかったらリロード
            reload();
        }
    }
    
    /// <summary>
    /// リロードします
    /// デフォだと発射間隔をリロード時間にして残弾をフルにするだけです
    /// </summary>
    protected virtual void reload() {
        //発射までの時間をリロード分に設定
        burstTimer = ReloadTime * ReloadRate;
        //全弾リロード
        remainingAmmo = maxAmmo;
    }
    
    /// <summary>
    /// 発射間隔タイマーを進めます
    /// </summary>
    /// <returns>発射可能になった場合、trueを返す</returns>
    protected bool updateCanShoot() {
        //時間分タイマーを進める
        burstTimer -= Time.deltaTime;
        return burstTimer < 0;
    }
}
