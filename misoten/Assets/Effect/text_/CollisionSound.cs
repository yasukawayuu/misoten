using UnityEngine;

public class CollisionSound : MonoBehaviour
{
    public AudioClip collisionSound; // 撞击音效
    private AudioSource audioSource; // 音频源

    private void Start()
    {
        // 获取或添加 AudioSource 组件
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; // 确保不会自动播放
        audioSource.clip = collisionSound; // 设置音效
    }

    // 碰撞检测
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 播放音效
        if (collisionSound != null)
        {
            audioSource.Play();
        }
    }

    // 如果使用触发器
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 播放音效
        if (collisionSound != null)
        {
            audioSource.Play();
        }
    }
}
