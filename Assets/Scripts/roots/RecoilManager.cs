using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using Characters.Player;
using UnityEngine;

public abstract class RecoilManager : MonoBehaviour {
    /// <summary>
    /// 反動によって体が跳ね挙げられている状況にあるか
    /// </summary>
    private bool isRecoiling = false;

    protected bool IsRecoiling {
        get { return isRecoiling; }
    }
    /// <summary>
    /// 現在のリコイル速度
    /// 単位は角度/秒
    /// </summary>
    private float recoilCurrentVelocity;

    /// <summary>
    /// 現在のリコイルによる補正合計角度
    /// 単位は角度
    /// </summary>
    protected float recoiled;

    public float Recoiled {
        get { return recoiled; }
    }

    /// <summary>
    /// リコイルに対抗する加速度
    /// 単位は角度/秒^2
    /// </summary>
    protected abstract float RecoilControll { get; }


    protected virtual void FixedUpdate() {
        updateCurrentRecoilAngle();
    }

    /// <summary>
    /// このフレームでのリコイル補正角度を計算します
    /// </summary>
    /// <returns>リコイル補正角度</returns>
    public virtual void updateCurrentRecoilAngle() {
        if (isRecoiling) {
            recoiled += recoiling();
            if (recoiled > 45) {
                recoilCurrentVelocity = 0f;
                recoiled = 45;
            }
            if (recoiled < 0f) {
                //リコイル値が0になったら終了
                isRecoiling = false;
                recoilCurrentVelocity = 0;
                recoiled = 0f;
            }
        }
    }
    
    /// <summary>
    /// 反動の早さを計算します
    /// </summary>
    /// <returns>反動の量</returns>
    float recoiling() {
        //recoilVelocity(recoil()の引数)が初速度、recoilControllが加速度の加速度運動として処理
        //反動仕切ったところから戻る時は2倍速
        if (recoilCurrentVelocity > 0) {
            recoilCurrentVelocity -= RecoilControll * Time.deltaTime;
        } else {
            recoilCurrentVelocity -= RecoilControll * 2 * Time.deltaTime;
        }
        
        return recoilCurrentVelocity * Time.deltaTime;
    }
    
    /// <summary>
    /// 反動を設定します
    /// </summary>
    /// <param name="maxVelocity">リコイル開始速度、単位は角度/秒</param>
    public virtual void recoil(float recoilVelocity) {
        recoilCurrentVelocity = recoilVelocity;
        isRecoiling = true;
    }
}
