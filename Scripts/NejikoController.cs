using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NejikoController : MonoBehaviour
{
    CharacterController controller;
    Animator animator;

    Vector3 moveDirection = Vector3.zero;

    public float gravity;   //重力の強さ
    public float speedZ;    //スピード
    public float speedJump; //ジャンプ力

    // Start is called before the first frame update
    void Start()
    {
        //必要なコンポーネントを自動取得
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //CharacterControllerコンポーネントの能力で地面判定ができる(isGrounded)
        if (controller.isGrounded)  //地面にいる時
        {
            //上下キーの入力があれば
            if (Input.GetAxis("Vertical") != 0.0f)
            {
                //キャラクター主観における奥行き(z軸)を前か後ろ方向の数値を設定
                moveDirection.z = Input.GetAxis("Vertical") * speedZ;
            }
            else   //上下キーの入力がなければ
            {
                //キャラクター主観における奥行き(z軸)の数字は0
                moveDirection.z = 0;
            }

            //方向の回転
            //左右キーの入れ具合に応じて即座にキャラクターを回転させる
            transform.Rotate(0, Input.GetAxis("Horizontal") * 3, 0);

            //ジャンプキーが押されたら
            if (Input.GetButton("Jump"))
            {
                //Y方向に変数speedJumpに設定した数字を加える
                moveDirection.y = speedJump;
                //アニメーターのTriggerパラメータ"jump"をオンにしてジャンプアニメに移行
                //※Triggerパラメータは一度発動したら自動ですぐオフになり元のアニメに戻る
                animator.SetTrigger("jump");
            }
        }

        //常に重力の力がY軸にかかっている
        moveDirection.y -= gravity * Time.deltaTime;

        //プレイヤー主観における動きのデータ(変数moveDirection)がグローバル座標でいうとどうなるのか？という数字に変換
        //※特にプレイヤーが回転してしまう分、前後の動きが主観とグローバルで異なるため
        //transform.TransformDirectionがプレイヤーの向きを考慮し移動させてくれる
        Vector3 globalDirection = transform.TransformDirection(moveDirection);
        //グローバル座標におきかえたプレイヤーの動きのデータをもとに実際に動作させる
        //CharacterControllerコンポーネントのMoveメソッドの引数に動かしたい方向を指定
        controller.Move(globalDirection * Time.deltaTime);

        //もしプレイヤーが地面に触れていたらY軸にかかる力を0にする
        if(controller.isGrounded) moveDirection.y = 0;

        //動きが0以外なら走ってるアニメにする(前後に動いてる)
        //アニメーターのBoolパラメータ"run"をオンか、オフにする
        //プレイヤーが前後に走っている時つまり(moveDirection.z != 0)がtrueの時→オン
        //プレイヤーが止まっている時つまり(moveDirection.z != 0)がfalseの時→オフ
        //※オフという事はIdleアニメになっている
        animator.SetBool("run", moveDirection.z != 0.0f);
    }
}
