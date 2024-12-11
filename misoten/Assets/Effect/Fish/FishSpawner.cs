using System.Collections;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
<<<<<<< HEAD
    public GameObject[] fishPrefabs; // Ñ}Êı¤Îô~¤Î¥×¥EÏ¥Ö¤ò¸ñ¼{
    public Camera mainCamera; // ¥á¥¤¥ó¥«¥á¥E
    public float spawnInterval = 3f; // ô~¤ÎÉú³Éég¸E
    private GameObject lastSpawnedFishPrefab = null; // ×ü@á¤ËÉú³É¤µ¤E¿ô~¤Î¥×¥EÏ¥Ö£¨×ûÏõ¤Î¥×¥EÏ¥Ö¤«¤É¤¦¤«¤òÅĞ¶Ï£©

    private void Start()
    {
        // ô~¤ò¶¨ÆÚµÄ¤ËÉú³É¤¹¤E³¥E`¥Á¥ó¤òé_Ê¼
=======
    public GameObject[] fishPrefabs; // •¡”‚Ì‹›‚ÌƒvƒŒƒnƒu‚ğŠi”[
    public Camera mainCamera; // ƒƒCƒ“ƒJƒƒ‰
    public float spawnInterval = 3f; // ‹›‚Ì¶¬ŠÔŠu
    private GameObject lastSpawnedFishPrefab = null; // ÅŒã‚É¶¬‚³‚ê‚½‹›‚ÌƒvƒŒƒnƒuiÅ‰‚ÌƒvƒŒƒnƒu‚©‚Ç‚¤‚©‚ğ”»’fj

    private void Start()
    {
        // ‹›‚ğ’èŠú“I‚É¶¬‚·‚éƒRƒ‹[ƒ`ƒ“‚ğŠJn
>>>>>>> ec0ffe0 (fish(æ–‡å­—BUGã§å†æå‡ºã—ã¾ã—ãŸ)ã¨ã‚¿ãƒƒãƒã®ã‚½ãƒ¼ã‚¹ã‚³ãƒ¼ãƒ‰)
        StartCoroutine(SpawnFishCoroutine());
    }

    private IEnumerator SpawnFishCoroutine()
    {
        while (true)
        {
            // ‹›‚ğ¶¬
            SpawnFish();
            // Ÿ‚Ì¶¬‚Ü‚Å‘Ò‹@
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnFish()
    {
<<<<<<< HEAD
        // ¥«¥á¥é¤ÎËÄÓç¤Î¥¹¥¯¥E`¥ó×ù˜Ë¤òÈ¡µÃ
        Vector3 screenBottomLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        Vector3 screenTopRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.nearClipPlane));

        // ¥é¥ó¥À¥à¤ËÎ»ÖÃ¤ò›Q¶¨£º»­ÃæÍâ¤Î¤É¤ÎŞx¤«¤éÉú³É¤¹¤E«£¨×ó¡¢ÓÒ¡¢ÉÏ¡¢ÏÂ£©
        Vector3 spawnPosition = Vector3.zero;
        Vector3 targetPosition = Vector3.zero;
        int side = Random.Range(0, 4); // 0:×E 1:ÓÒ, 2:ÉÏ, 3:ÏÂ

        switch (side)
        {
            case 0: // ×ó‚È¤«¤éÉú³É¡¢ÓÒ‚È¤ËÏû¤¨¤E
                spawnPosition = new Vector3(screenBottomLeft.x - 1, Random.Range(screenBottomLeft.y, screenTopRight.y), 0);
                targetPosition = new Vector3(screenTopRight.x + 1, Random.Range(screenBottomLeft.y, screenTopRight.y), 0);
                break;
            case 1: // ÓÒ‚È¤«¤éÉú³É¡¢×ó‚È¤ËÏû¤¨¤E
                spawnPosition = new Vector3(screenTopRight.x + 1, Random.Range(screenBottomLeft.y, screenTopRight.y), 0);
                targetPosition = new Vector3(screenBottomLeft.x - 1, Random.Range(screenBottomLeft.y, screenTopRight.y), 0);
                break;
            case 2: // ÉÏ‚È¤«¤éÉú³É¡¢ÏÂ‚È¤ËÏû¤¨¤E
                spawnPosition = new Vector3(Random.Range(screenBottomLeft.x, screenTopRight.x), screenTopRight.y + 1, 0);
                targetPosition = new Vector3(Random.Range(screenBottomLeft.x, screenTopRight.x), screenBottomLeft.y - 1, 0);
                break;
            case 3: // ÏÂ‚È¤«¤éÉú³É¡¢ÉÏ‚È¤ËÏû¤¨¤E
=======
        // ƒJƒƒ‰‚Ìl‹÷‚ÌƒXƒNƒŠ[ƒ“À•W‚ğæ“¾
        Vector3 screenBottomLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        Vector3 screenTopRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.nearClipPlane));

        // ƒ‰ƒ“ƒ_ƒ€‚ÉˆÊ’u‚ğŒˆ’èF‰æ–ÊŠO‚Ì‚Ç‚Ì•Ó‚©‚ç¶¬‚·‚é‚©i¶A‰EAãA‰ºj
        Vector3 spawnPosition = Vector3.zero;
        Vector3 targetPosition = Vector3.zero;
        int side = Random.Range(0, 4); // 0:¶, 1:‰E, 2:ã, 3:‰º

        switch (side)
        {
            case 0: // ¶‘¤‚©‚ç¶¬A‰E‘¤‚ÉÁ‚¦‚é
                spawnPosition = new Vector3(screenBottomLeft.x - 1, Random.Range(screenBottomLeft.y, screenTopRight.y), 0);
                targetPosition = new Vector3(screenTopRight.x + 1, Random.Range(screenBottomLeft.y, screenTopRight.y), 0);
                break;
            case 1: // ‰E‘¤‚©‚ç¶¬A¶‘¤‚ÉÁ‚¦‚é
                spawnPosition = new Vector3(screenTopRight.x + 1, Random.Range(screenBottomLeft.y, screenTopRight.y), 0);
                targetPosition = new Vector3(screenBottomLeft.x - 1, Random.Range(screenBottomLeft.y, screenTopRight.y), 0);
                break;
            case 2: // ã‘¤‚©‚ç¶¬A‰º‘¤‚ÉÁ‚¦‚é
                spawnPosition = new Vector3(Random.Range(screenBottomLeft.x, screenTopRight.x), screenTopRight.y + 1, 0);
                targetPosition = new Vector3(Random.Range(screenBottomLeft.x, screenTopRight.x), screenBottomLeft.y - 1, 0);
                break;
            case 3: // ‰º‘¤‚©‚ç¶¬Aã‘¤‚ÉÁ‚¦‚é
>>>>>>> ec0ffe0 (fish(æ–‡å­—BUGã§å†æå‡ºã—ã¾ã—ãŸ)ã¨ã‚¿ãƒƒãƒã®ã‚½ãƒ¼ã‚¹ã‚³ãƒ¼ãƒ‰)
                spawnPosition = new Vector3(Random.Range(screenBottomLeft.x, screenTopRight.x), screenBottomLeft.y - 1, 0);
                targetPosition = new Vector3(Random.Range(screenBottomLeft.x, screenTopRight.x), screenTopRight.y + 1, 0);
                break;
        }

<<<<<<< HEAD
        // ô~¤Î¥×¥EÏ¥Ö¤ò¥é¥ó¥À¥à¤Ëßx’k
        GameObject selectedFishPrefab = fishPrefabs[Random.Range(0, fishPrefabs.Length)];

        // ×ûÏõ¤Î¥×¥EÏ¥Ö¤¬Éú³É¤µ¤EEöºÏ¡¢10¤«¤E0Æ¥¤Îô~¤òÉú³É
        if (selectedFishPrefab == fishPrefabs[0] && lastSpawnedFishPrefab != selectedFishPrefab)
        {
            int numberOfFishToSpawn = Random.Range(10, 20); // 10¤«¤E0Æ¥¤Îô~¤ò¥é¥ó¥À¥à¤ËÉú³É
            int maxColumns = 3; // 1ĞĞ¤¢¤¿¤EûĞEÆ¥¤Îô~
            int rows = Mathf.CeilToInt(numberOfFishToSpawn / (float)maxColumns); // ±ØÒª¤ÊĞĞÊı¤òÓ‹ËE
            // Ñ}Êı¤Îô~¤òÉú³É
            for (int row = 0; row < rows; row++)
            {
                // ¸÷ĞĞ¤Îé_Ê¼Î»ÖÃ¤òÓ‹ËE
                Vector3 rowStartPosition = spawnPosition + new Vector3(row * 1.5f, 0, 0); // ¸÷ĞĞ¤Îég¸ô£¨1.5f¤ÏÀıÊ¾‚¡¢Õ{Õû¿ÉÄÜ£©

                for (int col = 0; col < maxColumns; col++)
                {
                    if ((row * maxColumns + col) >= numberOfFishToSpawn) break; // ±ØÒª¤ÊÊı¤ò³¬¤¨¤Ê¤¤¤è¤¦¤Ë¤¹¤E
                    // ÖØ¤Ê¤ê¤ò±Ü¤±¤E¿¤á¤Ë¥é¥ó¥À¥à¤ËÎ»ÖÃ¤ò¥ª¥Õ¥»¥Ã¥È
=======
        // ‹›‚ÌƒvƒŒƒnƒu‚ğƒ‰ƒ“ƒ_ƒ€‚É‘I‘ğ
        GameObject selectedFishPrefab = fishPrefabs[Random.Range(0, fishPrefabs.Length)];

        // Å‰‚ÌƒvƒŒƒnƒu‚ª¶¬‚³‚ê‚éê‡A10‚©‚ç20•C‚Ì‹›‚ğ¶¬
        if (selectedFishPrefab == fishPrefabs[0] && lastSpawnedFishPrefab != selectedFishPrefab)
        {
            int numberOfFishToSpawn = Random.Range(10, 20); // 10‚©‚ç20•C‚Ì‹›‚ğƒ‰ƒ“ƒ_ƒ€‚É¶¬
            int maxColumns = 3; // 1s‚ ‚½‚èÅ‘å3•C‚Ì‹›
            int rows = Mathf.CeilToInt(numberOfFishToSpawn / (float)maxColumns); // •K—v‚Ès”‚ğŒvZ

            // •¡”‚Ì‹›‚ğ¶¬
            for (int row = 0; row < rows; row++)
            {
                // Šes‚ÌŠJnˆÊ’u‚ğŒvZ
                Vector3 rowStartPosition = spawnPosition + new Vector3(row * 1.5f, 0, 0); // Šes‚ÌŠÔŠui1.5f‚Í—á¦’lA’²®‰Â”\j

                for (int col = 0; col < maxColumns; col++)
                {
                    if ((row * maxColumns + col) >= numberOfFishToSpawn) break; // •K—v‚È”‚ğ’´‚¦‚È‚¢‚æ‚¤‚É‚·‚é

                    // d‚È‚è‚ğ”ğ‚¯‚é‚½‚ß‚Éƒ‰ƒ“ƒ_ƒ€‚ÉˆÊ’u‚ğƒIƒtƒZƒbƒg
>>>>>>> ec0ffe0 (fish(æ–‡å­—BUGã§å†æå‡ºã—ã¾ã—ãŸ)ã¨ã‚¿ãƒƒãƒã®ã‚½ãƒ¼ã‚¹ã‚³ãƒ¼ãƒ‰)
                    Vector3 offset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
                    Vector3 fishPosition = rowStartPosition + new Vector3(col * 1.5f, 0, 0) + offset; // Še‹›‚ÌˆÊ’u
                    GameObject fish = Instantiate(selectedFishPrefab, fishPosition, Quaternion.identity);
                    FishController fishController = fish.GetComponent<FishController>();
                    if (fishController != null)
                    {
                        // –Ú•WˆÊ’u‚ğİ’è
                        fishController.SetTargetPosition(targetPosition);
                    }
                }
            }
        }
        else
        {
            // 1•C‚¾‚¯‚Ì‹›‚ğ¶¬
            GameObject fish = Instantiate(selectedFishPrefab, spawnPosition, Quaternion.identity);
            FishController fishController = fish.GetComponent<FishController>();
            if (fishController != null)
            {
                // –Ú•WˆÊ’u‚ğİ’è
                fishController.SetTargetPosition(targetPosition);
            }
        }

<<<<<<< HEAD
        // ×ü@á¤ËÉú³É¤µ¤E¿ô~¤Î¥×¥EÏ¥Ö¤ò¸EÂ
=======
        // ÅŒã‚É¶¬‚³‚ê‚½‹›‚ÌƒvƒŒƒnƒu‚ğXV
>>>>>>> ec0ffe0 (fish(æ–‡å­—BUGã§å†æå‡ºã—ã¾ã—ãŸ)ã¨ã‚¿ãƒƒãƒã®ã‚½ãƒ¼ã‚¹ã‚³ãƒ¼ãƒ‰)
        lastSpawnedFishPrefab = selectedFishPrefab;
    }
}
