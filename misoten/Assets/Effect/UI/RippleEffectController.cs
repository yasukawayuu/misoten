using UnityEngine;

public class RippleEffectController : MonoBehaviour
{
    public GameObject ripplePrefab;  // ｲｨｼy･ｨ･ﾕ･ｧ･ｯ･ﾈ､ﾎ･ﾗ･・ﾏ･ﾖ､ﾎﾕﾕ

    void Update()
    {
        // ･ﾞ･ｦ･ｹﾗｯ･・ﾃ･ｯ ､ﾞ､ｿ､ﾏ ･ｿ･ﾃ･ﾁ､ﾊｳ・
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            // ･ｹ･ｯ･・`･ｻﾖﾃ｣ｨ･ﾞ･ｦ･ｹ､ﾞ､ｿ､ﾏ･ｿ･ﾃ･ﾁﾎｻﾖﾃ｣ｩ､・`･・ﾉﾗﾋ､ﾋ我轍
            Vector3 screenPos = Input.GetMouseButtonDown(0) ? Input.mousePosition : (Vector3)Input.GetTouch(0).position;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            worldPos.z = 0;  // 2Dｿﾕ馮ﾄﾚ､ﾎﾎｻﾖﾃ､ﾋﾔOｶｨ

            // ｲｨｼy･ｨ･ﾕ･ｧ･ｯ･ﾈ､､･ｹ･ｿ･ｹｻｯ
            GameObject rippleInstance = Instantiate(ripplePrefab, worldPos, Quaternion.identity);

            // ｲｨｼy･｢･ﾋ･皓`･ｷ･逾ﾙﾉ・
            Animator animator = rippleInstance.GetComponentInChildren<Animator>();
            if (animator != null)
            {
                animator.Play("RippleAnimation");  // ﾔOｶｨ､ｷ､ｿ･｢･ﾋ･皓`･ｷ･逾ﾙﾉ・            }
            }
            // ･｢･ﾋ･皓`･ｷ･逾Kﾁﾋ矣｡｢･､･ｹ･ｿ･ｹ､ﾆ莱
            Destroy(rippleInstance, 0.5f);  // ･｢･ﾋ･皓`･ｷ･逾ﾎ餃､ｵ､ｬ0.5ﾃ・ﾈ△ｶｨ
        }
    }
}
