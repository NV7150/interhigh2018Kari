using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	private Animator _animator;
	private CharacterController _characterController;

	private readonly int SPEED = 500;
	private int HASH_WALK = Animator.StringToHash("speed");
	private float SPEED_DAMP = 0.1f;
	
	// Use this for initialization
	void Start () {
		_characterController = GetComponent<CharacterController>();
		_animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float h  = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");

		var hSpeed = transform.right * h * SPEED * Time.deltaTime;
		var vSpeed = transform.forward * v * SPEED * Time.deltaTime;
		
		Move(hSpeed,vSpeed);
		Animate(hSpeed.magnitude,vSpeed.magnitude);
		Turn();
	}

	void Move(Vector3 hSpeed,Vector3 vSpeed) {
		_characterController.SimpleMove(hSpeed);
		_characterController.SimpleMove(vSpeed);
	}

	void Animate(float h,float v) {
		_animator.SetFloat(HASH_WALK,v,SPEED_DAMP,Time.deltaTime);
	}
	
	void Turn() {
		//マウスから回転
		var mouseAngle = Input.GetAxis("Mouse X") * Time.deltaTime * SPEED;
		transform.Rotate(0,mouseAngle,0);
	}
}
