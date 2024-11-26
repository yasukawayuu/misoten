using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // 移动速度

    private void Update()
    {
        // 获取水平和垂直方向的输入
        float horizontal = Input.GetAxis("Horizontal"); // A/D 或 左/右键
        float vertical = Input.GetAxis("Vertical");     // W/S 或 上/下键

        // 计算移动方向
        Vector3 direction = new Vector3(horizontal, vertical, 0f).normalized;

        // 移动物体
        transform.position += direction * moveSpeed * Time.deltaTime;
    }
}
