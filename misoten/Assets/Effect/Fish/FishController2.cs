using UnityEngine;

public class FishController2 : MonoBehaviour
{
<<<<<<< HEAD
    public float speed = 2f; // ô~¤Î»ùµAÒÆEËÙ¶È
    public float angleOffset = -90f; // ½Ç¶È¥ª¥Õ¥»¥Ã¥È
    public float destroyDistanceMultiplier = 3f; // ÆÆ‰²Î»ÖÃ¤Î±¶ÂÊ
    public Camera mainCamera; // ¥«¥á¥é¡¢Ò•Ò°¤òÅĞ¶¨¤¹¤E¿¤á¤ËÊ¹ÓÃ

    [Header("¥é¥ó¥À¥à»¯ÔO¶¨")]
    public Vector2 sizeRange = new Vector2(0.5f, 2f); // ô~¤Î¥µ¥¤¥º¹ E
    public Vector2 speedRange = new Vector2(1f, 5f); // ô~¤ÎËÙ¶È¹ E
    public float largeFishSizeThreshold = 1.5f; // ´ó¤­¤Êô~¤Î¥µ¥¤¥ºé“‚
=======
    public float speed = 2f; // ‹›‚ÌŠî‘bˆÚ“®‘¬“x
    public float angleOffset = -90f; // Šp“xƒIƒtƒZƒbƒg
    public float destroyDistanceMultiplier = 3f; // ”j‰óˆÊ’u‚Ì”{—¦
    public Camera mainCamera; // ƒJƒƒ‰A‹–ì‚ğ”»’è‚·‚é‚½‚ß‚Ég—p

    [Header("ƒ‰ƒ“ƒ_ƒ€‰»İ’è")]
    public Vector2 sizeRange = new Vector2(0.5f, 2f); // ‹›‚ÌƒTƒCƒY”ÍˆÍ
    public Vector2 speedRange = new Vector2(1f, 5f); // ‹›‚Ì‘¬“x”ÍˆÍ
    public float largeFishSizeThreshold = 1.5f; // ‘å‚«‚È‹›‚ÌƒTƒCƒYè‡’l
>>>>>>> ec0ffe0 (fish(æ–‡å­—BUGã§å†æå‡ºã—ã¾ã—ãŸ)ã¨ã‚¿ãƒƒãƒã®ã‚½ãƒ¼ã‚¹ã‚³ãƒ¼ãƒ‰)

    private Vector3 targetPosition;

    private void Start()
    {
<<<<<<< HEAD
        // mainCamera¤¬ÔO¶¨¤µ¤EÆ¤¤¤Ê¤¤ˆöºÏ¡¢¥·©`¥óÄÚ¤Î¥á¥¤¥ó¥«¥á¥é¤òÈ¡µÃ
=======
        // mainCamera‚ªİ’è‚³‚ê‚Ä‚¢‚È‚¢ê‡AƒV[ƒ““à‚ÌƒƒCƒ“ƒJƒƒ‰‚ğæ“¾
>>>>>>> ec0ffe0 (fish(æ–‡å­—BUGã§å†æå‡ºã—ã¾ã—ãŸ)ã¨ã‚¿ãƒƒãƒã®ã‚½ãƒ¼ã‚¹ã‚³ãƒ¼ãƒ‰)
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        // ‹›‚ÌƒTƒCƒY‚Æ‘¬“x‚ğƒ‰ƒ“ƒ_ƒ€‚Éİ’è
        RandomizeFish();

<<<<<<< HEAD
        // ³õÆÚ·½Ïò¤ÎÔO¶¨
        //SetInitialDirection();
        SetTargetPosition(targetPosition); // ¥¿©`¥²¥Ã¥ÈÎ»ÖÃ¤È½Ç¶È¤ò¸EÂ
    }

    // ô~¤ÎÒÆEÏÈÎ»ÖÃ¤òÔO¶¨
=======
        // ‰Šú•ûŒü‚Ìİ’è
        // SetInitialDirection();
        SetTargetPosition(targetPosition); // ƒ^[ƒQƒbƒgˆÊ’u‚ÆŠp“x‚ğXV
    }

    // ‹›‚ÌˆÚ“®æˆÊ’u‚ğİ’è
>>>>>>> ec0ffe0 (fish(æ–‡å­—BUGã§å†æå‡ºã—ã¾ã—ãŸ)ã¨ã‚¿ãƒƒãƒã®ã‚½ãƒ¼ã‚¹ã‚³ãƒ¼ãƒ‰)
    public void SetTargetPosition(Vector3 target)
    {
        targetPosition = target;

<<<<<<< HEAD
        // Ä¿˜ËÎ»ÖÃ¤ËÏò¤«¤¦·½Ïò¤òÓ‹Ëã¤·¡¢»ØÜ¤òÕ{ÕE
        Vector3 direction = target - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // ½Ç¶È¤òÓ‹ËE
        transform.rotation = Quaternion.Euler(0, 0, angle + angleOffset); // ½Ç¶È¥ª¥Õ¥»¥Ã¥È¤ò¼Ó¤¨¤E
=======
        // –Ú•WˆÊ’u‚ÉŒü‚©‚¤•ûŒü‚ğŒvZ‚µA‰ñ“]‚ğ’²®
        Vector3 direction = target - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Šp“x‚ğŒvZ
        transform.rotation = Quaternion.Euler(0, 0, angle + angleOffset); // Šp“xƒIƒtƒZƒbƒg‚ğ‰Á‚¦‚é
>>>>>>> ec0ffe0 (fish(æ–‡å­—BUGã§å†æå‡ºã—ã¾ã—ãŸ)ã¨ã‚¿ãƒƒãƒã®ã‚½ãƒ¼ã‚¹ã‚³ãƒ¼ãƒ‰)
    }

    // ‹›‚ÌƒTƒCƒY‚Æ‘¬“x‚ğƒ‰ƒ“ƒ_ƒ€‚Éİ’è
    private void RandomizeFish()
    {
        bool largeFishExists = false;
        FishController2[] fishes = FindObjectsOfType<FishController2>(); // ƒV[ƒ““à‚Ì‚·‚×‚Ä‚ÌFishController2‚ğæ“¾
        foreach (FishController2 fish in fishes)
        {
<<<<<<< HEAD
            // ´ó¤­¤Êô~¤¬´æÔÚ¤¹¤E«´_ÕJ
=======
            // ‘å‚«‚È‹›‚ª‘¶İ‚·‚é‚©Šm”F
>>>>>>> ec0ffe0 (fish(æ–‡å­—BUGã§å†æå‡ºã—ã¾ã—ãŸ)ã¨ã‚¿ãƒƒãƒã®ã‚½ãƒ¼ã‚¹ã‚³ãƒ¼ãƒ‰)
            if (fish.transform.localScale.x >= largeFishSizeThreshold)
            {
                largeFishExists = true;
                break;
            }
        }

        // ‹›‚ÌƒTƒCƒY‚ğƒ‰ƒ“ƒ_ƒ€‚Éİ’è
        float randomSize = Random.Range(sizeRange.x, sizeRange.y);

<<<<<<< HEAD
        // ´ó¤­¤Êô~¤¬´æÔÚ¤·¡¢¥é¥ó¥À¥à¤Ëßx¤Ğ¤E¿¥µ¥¤¥º¤¬é“‚ÒÔÉÏ¤ÎˆöºÏ¡¢¥µ¥¤¥º¤òĞ¡¤µ¤¯Õ{ÕE
=======
        // ‘å‚«‚È‹›‚ª‘¶İ‚µAƒ‰ƒ“ƒ_ƒ€‚É‘I‚Î‚ê‚½ƒTƒCƒY‚ªè‡’lˆÈã‚Ìê‡AƒTƒCƒY‚ğ¬‚³‚­’²®
>>>>>>> ec0ffe0 (fish(æ–‡å­—BUGã§å†æå‡ºã—ã¾ã—ãŸ)ã¨ã‚¿ãƒƒãƒã®ã‚½ãƒ¼ã‚¹ã‚³ãƒ¼ãƒ‰)
        if (largeFishExists && randomSize >= largeFishSizeThreshold)
        {
            randomSize = Random.Range(sizeRange.x, largeFishSizeThreshold);
        }

<<<<<<< HEAD
        // ô~¤Î¥¹¥±©`¥EòÔO¶¨
        transform.localScale = new Vector3(randomSize, randomSize, 1f);

        // ¥µ¥¤¥º¤Ë»ù¤Å¤¤¤ÆËÙ¶È¤òÕ{ÕE
        float sizeFactor = (randomSize - sizeRange.x) / (sizeRange.y - sizeRange.x);
        speed = Mathf.Lerp(speedRange.y, speedRange.x, sizeFactor); // ¥µ¥¤¥º¤Ëê¤¸¤ÆËÙ¶È¤ò¥EË¥¢Ñaég
=======
        // ‹›‚ÌƒXƒP[ƒ‹‚ğİ’è
        transform.localScale = new Vector3(randomSize, randomSize, 1f);

        // ƒTƒCƒY‚ÉŠî‚Ã‚¢‚Ä‘¬“x‚ğ’²®
        float sizeFactor = (randomSize - sizeRange.x) / (sizeRange.y - sizeRange.x);
        speed = Mathf.Lerp(speedRange.y, speedRange.x, sizeFactor); // ƒTƒCƒY‚É‰‚¶‚Ä‘¬“x‚ğƒŠƒjƒA•âŠÔ
>>>>>>> ec0ffe0 (fish(æ–‡å­—BUGã§å†æå‡ºã—ã¾ã—ãŸ)ã¨ã‚¿ãƒƒãƒã®ã‚½ãƒ¼ã‚¹ã‚³ãƒ¼ãƒ‰)
    }

    private void Update()
    {
<<<<<<< HEAD
        // ô~¤ò¥¿©`¥²¥Ã¥ÈÎ»ÖÃ¤ËÏò¤«¤Ã¤ÆÒÆE
=======
        // ‹›‚ğƒ^[ƒQƒbƒgˆÊ’u‚ÉŒü‚©‚Á‚ÄˆÚ“®
>>>>>>> ec0ffe0 (fish(æ–‡å­—BUGã§å†æå‡ºã—ã¾ã—ãŸ)ã¨ã‚¿ãƒƒãƒã®ã‚½ãƒ¼ã‚¹ã‚³ãƒ¼ãƒ‰)
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // –Ú•WˆÊ’u‚É“’B‚µ‚½‚©‚Ç‚¤‚©‚ğŠm”F
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
<<<<<<< HEAD
            // ¥¿©`¥²¥Ã¥ÈÎ»ÖÃ¤ò¥¹¥¯¥E`¥ó×ù˜Ë¤Ë‰ä“Q
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(targetPosition);
            screenPosition.z = 0f; // 2D¥²©`¥à¤Ê¤Î¤Çz‚¤ÏŸoÒE
            // Ä¿˜ËÎ»ÖÃ¤¬»­ÃæÄÚ¤Ë¤¢¤E«¤òÅĞ¶¨
=======
            // ƒ^[ƒQƒbƒgˆÊ’u‚ğƒXƒNƒŠ[ƒ“À•W‚É•ÏŠ·
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(targetPosition);
            screenPosition.z = 0f; // 2DƒQ[ƒ€‚È‚Ì‚Åz’l‚Í–³‹

            // –Ú•WˆÊ’u‚ª‰æ–Ê“à‚É‚ ‚é‚©‚ğ”»’è
>>>>>>> ec0ffe0 (fish(æ–‡å­—BUGã§å†æå‡ºã—ã¾ã—ãŸ)ã¨ã‚¿ãƒƒãƒã®ã‚½ãƒ¼ã‚¹ã‚³ãƒ¼ãƒ‰)
            if (screenPosition.x < 0 || screenPosition.x > Screen.width || screenPosition.y < 0 || screenPosition.y > Screen.height)
            {
                // ‰æ–ÊŠO‚Éo‚½ê‡A‹›‚ğ”j‰ó
                Destroy(gameObject);
            }
            else
            {
<<<<<<< HEAD
                // Ä¿˜ËÎ»ÖÃ¤¬»­ÃæÄÚ¤Ë¤¢¤EöºÏ¡¢¥¿©`¥²¥Ã¥ÈÎ»ÖÃ¤ò¸EÂ
                // Ä¿˜ËÎ»ÖÃ¤òÑÓéL¤¹¤E¿¤á¤Î¥ª¥Õ¥»¥Ã¥È¤òÓ‹ËE
=======
                // –Ú•WˆÊ’u‚ª‰æ–Ê“à‚É‚ ‚éê‡Aƒ^[ƒQƒbƒgˆÊ’u‚ğXV
                // –Ú•WˆÊ’u‚ğ‰„’·‚·‚é‚½‚ß‚ÌƒIƒtƒZƒbƒg‚ğŒvZ
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
