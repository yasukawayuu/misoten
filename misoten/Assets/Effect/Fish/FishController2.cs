using UnityEngine;

public class FishController2 : MonoBehaviour
{
    public float speed = 2f; // ､ﾎｻAﾒﾆ・ﾋﾙｶﾈ
    public float angleOffset = -90f; // ｽﾇｶﾈ･ｪ･ﾕ･ｻ･ﾃ･ﾈ
    public float destroyDistanceMultiplier = 3f; // ﾆﾆ牡ﾎｻﾖﾃ､ﾎｱｶﾂﾊ
    public Camera mainCamera; // ･ｫ･皈鬘｢ﾒ編ｰ､ﾐｶｨ､ｹ､・ｿ､皃ﾋﾊｹﾓﾃ

    [Header("･鬣ﾀ･狃ｯﾔOｶｨ")]
    public Vector2 sizeRange = new Vector2(0.5f, 2f); // ､ﾎ･ｵ･､･ｺｹ・
    public Vector2 speedRange = new Vector2(1f, 5f); // ､ﾎﾋﾙｶﾈｹ・
    public float largeFishSizeThreshold = 1.5f; // ｴｭ､ﾊ､ﾎ･ｵ･､･ｺ體ｎ

    private Vector3 targetPosition;

    private void Start()
    {
        // mainCamera､ｬﾔOｶｨ､ｵ､・ﾆ､､､ﾊ､､因ｺﾏ｡｢･ｷｩ`･ﾚ､ﾎ･皈､･ｫ･皈鬢｡ｵﾃ
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        // ､ﾎ･ｵ･､･ｺ､ﾈﾋﾙｶﾈ､鬣ﾀ･爨ﾋﾔOｶｨ
        RandomizeFish();

        // ｳﾚｷｽﾏﾎﾔOｶｨ
        //SetInitialDirection();
        SetTargetPosition(targetPosition); // ･ｿｩ`･ｲ･ﾃ･ﾈﾎｻﾖﾃ､ﾈｽﾇｶﾈ､・ﾂ
    }

    // ､ﾎﾒﾆ・ﾏﾈﾎｻﾖﾃ､Oｶｨ
    public void SetTargetPosition(Vector3 target)
    {
        targetPosition = target;

        // ﾄｿ侏ﾎｻﾖﾃ､ﾋﾏｫ､ｦｷｽﾏ桐网ｷ｡｢ｻﾘﾜ椄{ﾕ・
        Vector3 direction = target - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // ｽﾇｶﾈ､桐・
        transform.rotation = Quaternion.Euler(0, 0, angle + angleOffset); // ｽﾇｶﾈ･ｪ･ﾕ･ｻ･ﾃ･ﾈ､ﾓ､ｨ､・
    }

    // ､ﾎ･ｵ･､･ｺ､ﾈﾋﾙｶﾈ､鬣ﾀ･爨ﾋﾔOｶｨ
    private void RandomizeFish()
    {
        bool largeFishExists = false;
        FishController2[] fishes = FindObjectsOfType<FishController2>(); // ･ｷｩ`･ﾚ､ﾎ､ｹ､ﾙ､ﾆ､ﾎFishController2､｡ｵﾃ
        foreach (FishController2 fish in fishes)
        {
            // ｴｭ､ﾊ､ｬｴ贇ﾚ､ｹ､・ｫｴ_ﾕJ
            if (fish.transform.localScale.x >= largeFishSizeThreshold)
            {
                largeFishExists = true;
                break;
            }
        }

        // ､ﾎ･ｵ･､･ｺ､鬣ﾀ･爨ﾋﾔOｶｨ
        float randomSize = Random.Range(sizeRange.x, sizeRange.y);

        // ｴｭ､ﾊ､ｬｴ贇ﾚ､ｷ｡｢･鬣ﾀ･爨ﾋﾟx､ﾐ､・ｿ･ｵ･､･ｺ､ｬ體ｎﾒﾔﾉﾏ､ﾎ因ｺﾏ｡｢･ｵ･､･ｺ､｡､ｵ､ｯﾕ{ﾕ・
        if (largeFishExists && randomSize >= largeFishSizeThreshold)
        {
            randomSize = Random.Range(sizeRange.x, largeFishSizeThreshold);
        }

        // ､ﾎ･ｹ･ｱｩ`･・Oｶｨ
        transform.localScale = new Vector3(randomSize, randomSize, 1f);

        // ･ｵ･､･ｺ､ﾋｻﾅ､､､ﾆﾋﾙｶﾈ､{ﾕ・
        float sizeFactor = (randomSize - sizeRange.x) / (sizeRange.y - sizeRange.x);
        speed = Mathf.Lerp(speedRange.y, speedRange.x, sizeFactor); // ･ｵ･､･ｺ､ﾋ場､ｸ､ﾆﾋﾙｶﾈ､・ﾋ･｢ﾑa馮
    }

    private void Update()
    {
        // ､ｿｩ`･ｲ･ﾃ･ﾈﾎｻﾖﾃ､ﾋﾏｫ､ﾃ､ﾆﾒﾆ・
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // ﾄｿ侏ﾎｻﾖﾃ､ﾋｵｽﾟ_､ｷ､ｿ､ｫ､ﾉ､ｦ､ｫ､_ﾕJ
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            // ･ｿｩ`･ｲ･ﾃ･ﾈﾎｻﾖﾃ､ｹ･ｯ･・`･ﾋ､ﾋ我轍
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(targetPosition);
            screenPosition.z = 0f; // 2D･ｲｩ`･爨ﾊ､ﾎ､ﾇzｎ､ﾏ殪ﾒ・
            // ﾄｿ侏ﾎｻﾖﾃ､ｬｻｭﾃ貽ﾚ､ﾋ､｢､・ｫ､ﾐｶｨ
            if (screenPosition.x < 0 || screenPosition.x > Screen.width || screenPosition.y < 0 || screenPosition.y > Screen.height)
            {
                // ｻｭﾃ賚筅ﾋｳｿ因ｺﾏ｡｢､ﾆ牡
                Destroy(gameObject);
            }
            else
            {
                // ﾄｿ侏ﾎｻﾖﾃ､ｬｻｭﾃ貽ﾚ､ﾋ､｢､・ﾏ｡｢･ｿｩ`･ｲ･ﾃ･ﾈﾎｻﾖﾃ､・ﾂ
                // ﾄｿ侏ﾎｻﾖﾃ､ﾓ餃､ｹ､・ｿ､皃ﾎ･ｪ･ﾕ･ｻ･ﾃ･ﾈ､桐・
                Vector3 direction = targetPosition - transform.position;
                Vector3 offsetPosition = direction.normalized * destroyDistanceMultiplier;

                // ･ｿｩ`･ｲ･ﾃ･ﾈﾎｻﾖﾃ､ﾓ餃
                targetPosition = targetPosition + offsetPosition;
                SetTargetPosition(targetPosition); // ･ｿｩ`･ｲ･ﾃ･ﾈﾎｻﾖﾃ､ﾈｽﾇｶﾈ､・ﾂ
            }
        }
    }
}
