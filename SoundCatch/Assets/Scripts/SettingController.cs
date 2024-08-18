using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingController : MonoBehaviour
{
    TMP_Text visualText;

    private void Awake()
    {
        visualText = GameObject.Find("VisualSignText").GetComponent<TMP_Text>();
    }

    public void ClickButton0() // �ð��� ǥ�� ���
    {
        Debug.Log("�ð��� ǥ��:���");
        if (UIVisualController.Instance.visual == true)
        {
            Debug.Log("�ð��� ǥ��:����");
            UIVisualController.Instance.visual = false;
            UIVisualController.Instance.visualChange();
            visualText.text = "�ð���\r\nǥ��\r\n\r\nOff";


        } else if (UIVisualController.Instance.visual == false)
        {
            Debug.Log("�ð��� ǥ��:�ѱ�");
            UIVisualController.Instance.visual = true;
            UIVisualController.Instance.visualChange();
            visualText.text = "�ð���\r\nǥ��\r\n\r\nOn";
        }
    }

    public void ClickButton1() // ���Ӽ��� �޴���
    {
        SceneLoader.Instance.ChangeScene("SelectGame");
    }
    public void ClickButton2() // ��������
    {
        SceneLoader.Instance.ChangeScene("GameExit");
    }
}