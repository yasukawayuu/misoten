using UnityEngine;

public class FishController : MonoBehaviour
{
    public float speed = 2f; // ､ﾎﾒﾆ・ﾋﾙｶﾈ
    public float angleOffset = -90f; // ｽﾇｶﾈ､ﾎ･ｪ･ﾕ･ｻ･ﾃ･ﾈ｡｣Unity･ｨ･ﾇ･｣･ｿ､ﾇﾕ{ﾕ釤ﾉﾄﾜ
    public float destroyDistanceMultiplier = 3f; // ﾆﾆ牡ﾎｻﾖﾃ､ﾎｱｶﾂﾊ
    public Camera mainCamera; // ･ｫ･皈鬘｢ﾒ編ｰ､ﾐｶｨ､ｹ､・ｿ､皃ﾋﾊｹﾓﾃ

    private Vector3 targetPosition;

    private void Start()
    {
        // Inspector､ﾇ･ｫ･皈鬢ｬｸ釥・ｱ､ﾆ､鬢・ﾆ､､､ﾊ､､因ｺﾏ｡｢･ｷｩ`･ﾚ､ﾎMain Camera､ﾔ・､ﾇﾈ｡ｵﾃ
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // ﾖｫ･皈鬢｡ｵﾃ
        }
    }

    public void SetTargetPosition(Vector3 target)
    {
        targetPosition = target;

        // ｷｽﾏ桐网ｷ､ﾆｻﾘﾜ椄{ﾕ・
        Vector3 direction = target - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // ｽﾇｶﾈ､桐・
        transform.rotation = Quaternion.Euler(0, 0, angle + angleOffset); // ｽﾇｶﾈ･ｪ･ﾕ･ｻ･ﾃ･ﾈ､ﾓ､ｨ､・
    }

    private void Update()
    {
        // ､ｿｩ`･ｲ･ﾃ･ﾈﾎｻﾖﾃ､ﾋﾏｱ､ﾆﾒﾆ・
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // ､ｬ･ｿｩ`･ｲ･ﾃ･ﾈﾎｻﾖﾃ､ﾋｵｽﾟ_､ｷ､ｿ､ｫ･ﾁ･ｧ･ﾃ･ｯ
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            // ･ｫ･皈鬢ﾎﾒ編ｰｹ・､｡ｵﾃ
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(targetPosition);

            // 2D･ｲｩ`･爨ﾇ､ﾏ･ｫ･皈鬢ﾎZｎ､ﾏｹﾌｶｨ､ｵ､・・ﾙ､ｭ
            screenPosition.z = 0f; // zｎ､・､ﾋﾔOｶｨ｡｣x､ﾈyﾝS､ﾎ､ﾟ､ｬﾖﾘﾒｪ

            // ﾄｿ侏ﾎｻﾖﾃ､ｬｻｭﾃ貽ﾚ､ﾋ､｢､・ｫ､ﾐｶｨ
            if (screenPosition.x < 0 || screenPosition.x > Screen.width || screenPosition.y < 0 || screenPosition.y > Screen.height)
            {
                // ｻｭﾃ賚筅ﾎ因ｺﾏ｡｢､ﾆ牡
                Destroy(gameObject);
            }
            else
            {
                // ﾄｿ侏ﾎｻﾖﾃ､ｬﾒ編ｰﾄﾚ､ﾋ､｢､・ﾐ｡｢･ｿｩ`･ｲ･ﾃ･ﾈﾎｻﾖﾃ､・ﾂ
                // ﾆﾆ牡ﾎｻﾖﾃ､ﾎ･ｪ･ﾕ･ｻ･ﾃ･ﾈ､桐・
                Vector3 direction = targetPosition - transform.position;
                Vector3 offsetPosition = direction.normalized * destroyDistanceMultiplier;

                // ･ｿｩ`･ｲ･ﾃ･ﾈﾎｻﾖﾃ､ﾓ餃
                targetPosition = targetPosition + offsetPosition;
                SetTargetPosition(targetPosition); // ･ｿｩ`･ｲ･ﾃ･ﾈﾎｻﾖﾃ､ﾈｽﾇｶﾈ､・ﾂ
            }
        }
    }
}
