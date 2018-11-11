using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
	public LayerMask blockLayer; //ブロックレイヤー
	private Rigidbody2D rbody; //プレイヤー制御用Rigidbody2D

	private const float MOVE_SPEED = 3; //移動速度
	private float moveSpeed; //移動速度
	private float jumpPower = 200; //ジャンプの力
	private bool goJump = false; //ジャンプしたか否か
	private bool canJump = false; //ブロックに設置しているか否か

	public enum MOVE_DIR{ //移動方向定義
		STOP,
		LEFT,
		RIGHT,
	};

	private MOVE_DIR moveDirection = MOVE_DIR.STOP; //移動方向

	void Start () {
		rbody = GetComponent<Rigidbody2D>();
	}
	
	void FixedUpdate(){
		//移動方向で処理を分岐
		switch(moveDirection){
		case MOVE_DIR.STOP:
			moveSpeed = 0;
			break;
		case MOVE_DIR.LEFT:
			moveSpeed = MOVE_SPEED * -1;
			transform.localScale = new Vector2(-4,4);
			break;
		case MOVE_DIR.RIGHT:
			moveSpeed = MOVE_SPEED;
			transform.localScale = new Vector2(4,4);
			break;
		}

		rbody.velocity = new Vector2 (moveSpeed, rbody.velocity.y);

		//ジャンプ処理
		if(goJump){
			rbody.AddForce(Vector2.up * jumpPower);
			goJump = false;
		}
	}

	private void Update(){
		//canJumpの更新
		canJump = Physics2D.Linecast(transform.position - (transform.right * 0.3f),
					transform.position - (transform.up * 0.05f),blockLayer) ||
				  Physics2D.Linecast(transform.position + (transform.right * 0.3f),
					transform.position - (transform.up * 0.05f),blockLayer);
		
		//キー入力検知
		moveDirection = MOVE_DIR.STOP;
		if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && canJump) goJump = true;
		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) moveDirection = MOVE_DIR.LEFT;
		if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) moveDirection = MOVE_DIR.RIGHT;

	}
	
}
