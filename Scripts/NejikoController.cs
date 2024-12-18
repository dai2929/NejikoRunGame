using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NejikoController : MonoBehaviour
{
    CharacterController controller;
    Animator animator;

    Vector3 moveDirection = Vector3.zero;

    public float gravity;
    public float speedZ;
    public float speedJump;

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
        if (controller.isGrounded)   //接地しているかの判定
        {
            //全身ベロシティの設定
            if (Input.GetAxis("Vertical") > 0.0f)
            {
                moveDirection.z = Input.GetAxis("Vertical") * speedZ;
            }
            else
            {
                moveDirection.z = 0;
            }

            //方向の回転
            transform.Rotate(0, Input.GetAxis("Horizontal") * 3, 0);

            //ジャンプ処理
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = speedJump;
                animator.SetTrigger("jump");
            }
        }

        //重力分の力を毎フレーム追加
        //常に重力の力がY軸にかかっている
        moveDirection.y -= gravity * Time.deltaTime;

        //移動実行
        //プレイヤー主観における動きのデータ(変数moveDirection)がグローバル座標でいうとどうなるのか？という数字に変換
        //※特にプレイヤーが回転してしまう分、前後の動きが主観とグローバルで異なるため
        Vector3 globalDirection = transform.TransformDirection(moveDirection);
        //グローバル座標におきかえたプレイヤーの動きのデータをもとに実際に動作させる
        //CharacterControllerコンポーネントのMoveメソッドの引数に動かしたい方向を指定
        controller.Move(globalDirection * Time.deltaTime);

        //移動後接地してたらY方向の速度はリセットする
        //もしプレイヤーが地面に触れていたら
        if(controller.isGrounded) moveDirection.y = 0;

        //速度が0以上なら走っているフラグをtrueにする
        animator.SetBool("run", moveDirection.z > 0.0f);
    }
}
