using System.Collections;
using System.Collections.Generic;
using Characters.Player;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	/// <summary>
	/// 走る速度
	/// </summary>
	public int SPEED = 250;
	
	private Animator _animator;
	private CharacterController _characterController;
	
	/// <summary>
	/// アニメータの速度決定float値
	/// </summary>
	private int HASH_WALK = Animator.StringToHash("speed");
	/// <summary>
	/// 速度の変更時間
	/// </summary>
	private float SPEED_DAMP = 0.1f;
	
	private PlayerAbilities abilities;

	private PlayerStateManager _stateMan;
	
	// Use this for initialization
	void Start () {
		_characterController = GetComponent<CharacterController>();
		_animator = GetComponent<Animator>();
		abilities = GetComponent<PlayerAbilities>();

		_stateMan = GetComponent<PlayerStateManager>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//各方向のキー入力状況を確認して速度を算出
		float h  = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");

		var hSpeed = transform.right * h * SPEED * Time.deltaTime;
		var vSpeed = transform.forward * v * SPEED * Time.deltaTime;
		
		//スニーク等による速度補正を追加
		float rate = speedCorrectionRate();
		hSpeed *= rate;
		vSpeed *= rate;
		
		//移動
		Move(hSpeed,vSpeed);
		Animate(hSpeed.magnitude,vSpeed.magnitude);
		Turn();
	}
	
	/// <summary>
	/// 歩行の早さ補正値を計算します
	/// </summary>
	/// <returns>補正値　単位は割合(~1.0f)</returns>
	float speedCorrectionRate() {
		float rate = 1.0f;
		rate *= (_stateMan.IsAiming) ? abilities.AimingSpeedRate : 1.0f;
		rate *= (_stateMan.IsSneaking) ? abilities.SneakSpeedRate : 1.0f;
		return rate;
	}
	
	/// <summary>
	/// スピード分移動します
	/// </summary>
	/// <param name="hSpeed">水平方向（横）のスピード</param>
	/// <param name="vSpeed">垂直方向（前後）のスピード</param>
	void Move(Vector3 hSpeed,Vector3 vSpeed) {
		_characterController.SimpleMove(hSpeed);
		_characterController.SimpleMove(vSpeed);
		
		//hspeedかvspeedが0.1を超えているなら動いていると判断
		_stateMan.IsMoving = hSpeed.magnitude > 0.1f || vSpeed.magnitude > 0.1f;
	}
	
	/// <summary>
	/// 歩行アニメーションを再生します
	/// </summary>
	/// <param name="hSpeed">水平方向（横）のスピード</param>
	/// <param name="vSpeed">垂直方向（前後）のスピード</param>
	void Animate(float h,float v) {
		_animator.SetFloat(HASH_WALK,v,SPEED_DAMP,Time.deltaTime);
	}
	
	/// <summary>
	/// 回転します
	/// </summary>
	void Turn() {
		//マウスから回転
		var mouseAngle = Input.GetAxis("Mouse X") * Time.deltaTime * SPEED;
		transform.Rotate(0,mouseAngle,0);
	}
}
