using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private int id;
    private string playerName = "Anonymous";

    // プレイヤー名を表示するためのUI Text
    public Text nameText;

    // プレイヤーのスプライトレンダラー
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        UpdateNameDisplay();
    }

    void Update()
    {
        // 名前の位置をプレイヤーの上に表示
        if (nameText != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 1.0f);
            nameText.transform.position = screenPos;
        }
    }

    // IDを設定
    public void SetID(int playerID)
    {
        id = playerID;
    }

    // 名前を設定
    public void SetName(string name)
    {
        playerName = name;
        UpdateNameDisplay();
    }

    // 名前を更新
    public void UpdateName(string newName)
    {
        playerName = newName;
        UpdateNameDisplay();
    }

    // 名前表示を更新
    void UpdateNameDisplay()
    {
        if (nameText != null)
        {
            nameText.text = playerName;
        }
    }

    // 位置を更新
    public void UpdatePosition(Vector2 newPos)
    {
        transform.position = new Vector3(newPos.x, newPos.y, 0);
    }

    // 移動処理
    public void Move(Vector2 delta)
    {
        transform.position += (Vector3)delta;
    }
}
