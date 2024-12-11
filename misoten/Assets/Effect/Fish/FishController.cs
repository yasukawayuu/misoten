using UnityEngine;

public class FishController : MonoBehaviour
{
<<<<<<< HEAD
    public float speed = 2f; // ô~¤ÎÒÆEËÙ¶È
    public float angleOffset = -90f; // ½Ç¶È¤Î¥ª¥Õ¥»¥Ã¥È¡£Unity¥¨¥Ç¥£¥¿¤ÇÕ{Õû¿ÉÄÜ
    public float destroyDistanceMultiplier = 3f; // ÆÆ‰²Î»ÖÃ¤Î±¶ÂÊ
    public Camera mainCamera; // ¥«¥á¥é¡¢Ò•Ò°¤òÅĞ¶¨¤¹¤E¿¤á¤ËÊ¹ÓÃ
=======
    public float speed = 2f; // ‹›‚ÌˆÚ“®‘¬“x
    public float angleOffset = -90f; // Šp“x‚ÌƒIƒtƒZƒbƒgBUnityƒGƒfƒBƒ^‚Å’²®‰Â”\
    public float destroyDistanceMultiplier = 3f; // ”j‰óˆÊ’u‚Ì”{—¦
    public Camera mainCamera; // ƒJƒƒ‰A‹–ì‚ğ”»’è‚·‚é‚½‚ß‚Ég—p
>>>>>>> ec0ffe0 (fish(æ–‡å­—BUGã§å†æå‡ºã—ã¾ã—ãŸ)ã¨ã‚¿ãƒƒãƒã®ã‚½ãƒ¼ã‚¹ã‚³ãƒ¼ãƒ‰)

    private Vector3 targetPosition;

    private void Start()
    {
<<<<<<< HEAD
        // Inspector¤Ç¥«¥á¥é¤¬¸ûÀE±¤Æ¤é¤EÆ¤¤¤Ê¤¤ˆöºÏ¡¢¥·©`¥óÄÚ¤ÎMain Camera¤ò×ÔE¤ÇÈ¡µÃ
=======
        // Inspector‚ÅƒJƒƒ‰‚ªŠ„‚è“–‚Ä‚ç‚ê‚Ä‚¢‚È‚¢ê‡AƒV[ƒ““à‚ÌMain Camera‚ğ©“®‚Åæ“¾
>>>>>>> ec0ffe0 (fish(æ–‡å­—BUGã§å†æå‡ºã—ã¾ã—ãŸ)ã¨ã‚¿ãƒƒãƒã®ã‚½ãƒ¼ã‚¹ã‚³ãƒ¼ãƒ‰)
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // åƒJƒƒ‰‚ğæ“¾
        }
    }

    public void SetTargetPosition(Vector3 target)
    {
        targetPosition = target;

<<<<<<< HEAD
        // ·½Ïò¤òÓ‹Ëã¤·¤Æ»ØÜ¤òÕ{ÕE
        Vector3 direction = target - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // ½Ç¶È¤òÓ‹ËE
        transform.rotation = Quaternion.Euler(0, 0, angle + angleOffset); // ½Ç¶È¥ª¥Õ¥»¥Ã¥È¤ò¼Ó¤¨¤E
=======
        // •ûŒü‚ğŒvZ‚µ‚Ä‰ñ“]‚ğ’²®
        Vector3 direction = target - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Šp“x‚ğŒvZ
        transform.rotation = Quaternion.Euler(0, 0, angle + angleOffset); // Šp“xƒIƒtƒZƒbƒg‚ğ‰Á‚¦‚é
>>>>>>> ec0ffe0 (fish(æ–‡å­—BUGã§å†æå‡ºã—ã¾ã—ãŸ)ã¨ã‚¿ãƒƒãƒã®ã‚½ãƒ¼ã‚¹ã‚³ãƒ¼ãƒ‰)
    }

    private void Update()
    {
<<<<<<< HEAD
        // ô~¤ò¥¿©`¥²¥Ã¥ÈÎ»ÖÃ¤ËÏò¤±¤ÆÒÆE
=======
        // ‹›‚ğƒ^[ƒQƒbƒgˆÊ’u‚ÉŒü‚¯‚ÄˆÚ“®
>>>>>>> ec0ffe0 (fish(æ–‡å­—BUGã§å†æå‡ºã—ã¾ã—ãŸ)ã¨ã‚¿ãƒƒãƒã®ã‚½ãƒ¼ã‚¹ã‚³ãƒ¼ãƒ‰)
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // ‹›‚ªƒ^[ƒQƒbƒgˆÊ’u‚É“’B‚µ‚½‚©ƒ`ƒFƒbƒN
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
<<<<<<< HEAD
            // ¥«¥á¥é¤ÎÒ•Ò°¹ E¤òÈ¡µÃ
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(targetPosition);

            // 2D¥²©`¥à¤Ç¤Ï¥«¥á¥é¤ÎZ‚¤Ï¹Ì¶¨¤µ¤EEÙ¤­
            screenPosition.z = 0f; // z‚¤E¤ËÔO¶¨¡£x¤ÈyİS¤Î¤ß¤¬ÖØÒª

            // Ä¿˜ËÎ»ÖÃ¤¬»­ÃæÄÚ¤Ë¤¢¤E«¤òÅĞ¶¨
=======
            // ƒJƒƒ‰‚Ì‹–ì”ÍˆÍ‚ğæ“¾
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(targetPosition);

            // 2DƒQ[ƒ€‚Å‚ÍƒJƒƒ‰‚ÌZ’l‚ÍŒÅ’è‚³‚ê‚é‚×‚«
            screenPosition.z = 0f; // z’l‚ğ0‚Éİ’èBx‚Æy²‚Ì‚İ‚ªd—v

            // –Ú•WˆÊ’u‚ª‰æ–Ê“à‚É‚ ‚é‚©‚ğ”»’è
>>>>>>> ec0ffe0 (fish(æ–‡å­—BUGã§å†æå‡ºã—ã¾ã—ãŸ)ã¨ã‚¿ãƒƒãƒã®ã‚½ãƒ¼ã‚¹ã‚³ãƒ¼ãƒ‰)
            if (screenPosition.x < 0 || screenPosition.x > Screen.width || screenPosition.y < 0 || screenPosition.y > Screen.height)
            {
                // ‰æ–ÊŠO‚Ìê‡A‹›‚ğ”j‰ó
                Destroy(gameObject);
            }
            else
            {
<<<<<<< HEAD
                // Ä¿˜ËÎ»ÖÃ¤¬Ò•Ò°ÄÚ¤Ë¤¢¤EĞ¡¢¥¿©`¥²¥Ã¥ÈÎ»ÖÃ¤ò¸EÂ
                // ÆÆ‰²Î»ÖÃ¤Î¥ª¥Õ¥»¥Ã¥È¤òÓ‹ËE
=======
                // –Ú•WˆÊ’u‚ª‹–ì“à‚É‚ ‚ê‚ÎAƒ^[ƒQƒbƒgˆÊ’u‚ğXV
                // ”j‰óˆÊ’u‚ÌƒIƒtƒZƒbƒg‚ğŒvZ
>>>>>>> ec0ffe0 (fish(æ–‡å­—BUGã§å†æå‡ºã—ã¾ã—ãŸ)ã¨ã‚¿ãƒƒãƒã®ã‚½ãƒ¼ã‚¹ã‚³ãƒ¼ãƒ‰)
                Vector3 direction = targetPosition - transform.position;
                Vector3 offsetPosition = direction.normalized * destroyDistanceMultiplier;

                // ƒ^[ƒQƒbƒgˆÊ’u‚ğ‰„’·
                targetPosition = targetPosition + offsetPosition;
<<<<<<< HEAD
                SetTargetPosition(targetPosition); // ¥¿©`¥²¥Ã¥ÈÎ»ÖÃ¤È½Ç¶È¤ò¸EÂ
=======
                SetTargetPosition(targetPosition); // ƒ^[ƒQƒbƒgˆÊ’u‚ÆŠp“x‚ğXV
>>>>>>> ec0ffe0 (fish(æ–‡å­—BUGã§å†æå‡ºã—ã¾ã—ãŸ)ã¨ã‚¿ãƒƒãƒã®ã‚½ãƒ¼ã‚¹ã‚³ãƒ¼ãƒ‰)
            }
        }
    }
}
