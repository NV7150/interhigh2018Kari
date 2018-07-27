using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShootingSystem : MonoBehaviour {
    public Transform shootFrom;

    protected float shootWide;

    public float ShootWide {
        get { return shootWide; }
        set { shootWide = value; }
    }

    protected float shootRange;
    
    public float ShootRange {
        get { return shootRange; }
        set { shootRange = value; }
    }
    
    protected Vector3 getBulletVector() {
        //ワールド軸から見たマズルの角度
        var forwardDirection = Quaternion.FromToRotation(Vector3.forward, shootFrom.forward);
		
        //(半径１の円周常にあるランダムなある一点の一般化)　* (0~shootRange)によって中心〜円周までの割合を決定) 方向だけなのでrockdistanceは関係なし
        var randomCerclePoint = new Vector2(Random.Range(-1.0f,1.0f),Random.Range(-1.0f,1.0f)).normalized * Random.Range(0f,shootWide);
        //上を三次元化：zには射程
        var vector = forwardDirection * new Vector3(randomCerclePoint.x,randomCerclePoint.y,shootRange);

        return vector;
    }
}
