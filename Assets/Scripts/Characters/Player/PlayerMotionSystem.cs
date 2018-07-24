using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Player {
	/// <summary>
	/// プレイヤーのアニメーション系統を司るコンポーネント
	/// </summary>
    public abstract class PlayerMotionSystem : MonoBehaviour {
		
		
		
        // Update is called once per frame
        protected void FixedUpdate () {
            moveMotionUpdate();
        }
	
        /// <summary>
        /// update毎に行われる移動・回転などの処理
        /// turn()を毎回呼ぶので、したくない時はオーバーライドしてください
        /// </summary>
        protected void moveMotionUpdate() {
            positionUpdate();
            turn();
        }
	
        /// <summary>
        /// update毎に行われる移動処理の内、位置に関わる処理
        /// getKey(マップされているキー)として該当メソッドを呼ぶので、キーが押された時の処理などは実装側で分岐してください
        /// </summary>
        protected void positionUpdate() {
	        float x = Input.GetAxis("Horizontal");
	        float y = Input.GetAxis("Vertical");
        }
	
        /// <summary>
        /// 前進の処理
        /// </summary>
        protected abstract void moveForward();
	
        /// <summary>
        /// 後退の処理
        /// </summary>
        protected abstract void moveRight();
	
        /// <summary>
        /// 右に進む処理
        /// </summary>
        protected abstract void moveLeft();
	
        /// <summary>
        /// 左に進む処理
        /// </summary>
        protected abstract void moveBack();
	
        /// <summary>
        /// 回転処理
        /// moveMotionUpdateがオーバーライドされない限り毎フレーム呼ばれます
        /// </summary>
        protected abstract void turn();
    }
}

