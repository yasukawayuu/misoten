using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    // 切换到指定场景
    public void SwitchScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
       
    }

}
