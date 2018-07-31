using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShootingSystem : MonoBehaviour {
    /// <summary>
    /// 弾のばらけ具合
    /// </summary>
    public abstract float ShootWide { get; }
    
    /// <summary>
    /// 射程
    /// </summary>
    public abstract float ShootRange { get; }

    /// <summary>
    /// 射撃を行う地点のTransform
    /// </summary>
    public Transform shootFrom;
    
    /// <summary>
    /// 弾が飛ぶベクトルを計算します
    /// </summary>
    /// <returns>弾が飛ぶベクトル</returns>
    protected Vector3 getBulletVector(float forcusRate) {
        //半径1の円の円周上のある一点をランダムに設定
        //そこに0~shootWideをかけることでランダムな平面円の中にある一点を生成
        //この時、射程によってshootWideを変化させることでshootWideとshootRangeのバランスが変わらない様にする
        var randomCerclePoint = new Vector2(Random.Range(-1.0f,1.0f),Random.Range(-1.0f,1.0f)).normalized * Random.Range(0f,ShootWide + ShootRange / 10 * forcusRate);
		
        //上を三次元化して回転
        //rockDistanceじゃなくてshootRange分射撃するのでz軸はshootRange
        var vector = shootFrom.rotation * new Vector3(randomCerclePoint.x,randomCerclePoint.y,ShootRange);

        return vector;
    }
}
