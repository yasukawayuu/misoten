using Unity.Netcode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCon : NetworkBehaviour
{
    private float speed = 0.02f;
    private Vector2 moveInput = Vector2.zero;
    private Color color;

    void Start()
    {
        color = new Color(Random.value, Random.value, Random.value, 1.0f);

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.material.color = color;
        SetSpriteColorServerRpc(color);
    }

    void Update()
    {

        //ownerの場合
        if (IsOwner)
        {
            float xs = 0.0f; float ys = 0.0f;

            // 移動入力を設定
            if (Input.GetKey("left"))
            {
                xs = -speed;
            }
            if (Input.GetKey("right"))
            {
                xs = speed;
            }
            if (Input.GetKey("up"))
            {
                ys = speed;
            }
            if (Input.GetKey("down"))
            {
                ys = -speed;
            }

            SetMoveInputServerRpc(xs, ys);
        }


        //サーバー（ホスト）の場合
        if (IsServer)
        {
            //移動処理
            Vector2 vec = transform.position;
            vec += moveInput;
            transform.position = vec;

            moveInput = Vector2.zero;
        }
    }

    // カラーをセットするRPC
    [ServerRpc]
    private void SetSpriteColorServerRpc(Color c)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.material.color = c;
    }

    // 移動入力をセットするRPC
    [ServerRpc]
    private void SetMoveInputServerRpc(float x, float y)
    {
        moveInput = new Vector2(x, y);
    }
}
