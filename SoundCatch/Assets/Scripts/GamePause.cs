using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePause : MonoBehaviour
{
    bool paused = false;
    bool check = false;

    string before;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
            check = true;
        }
        if (check) {
            if(paused)
            {
                Pause();
                check = false;
            }
            else if (!paused)
            {
                Resume();
                check = false;
            }
        }
    }

    public void Pause()     // ���� �Ͻ�����
    {
        // SceneManager.LoadScene("Setting", LoadSceneMode.Additive);
        before = SceneLoader.Instance.curSceneName;
        SceneLoader.Instance.ChangeScene("Setting");
        //Time.timeScale = 0;
    }

    public void Resume()    // ���� �̾��ϱ�
    {
        //Time.timeScale = 1;
        // SceneManager.UnloadSceneAsync("Setting");
        SceneLoader.Instance.ChangeScene(before);
    }
}
