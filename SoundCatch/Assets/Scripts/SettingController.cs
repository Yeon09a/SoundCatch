using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingController : MonoBehaviour
{
    public void ClickButton0() // �ð��� ǥ�� ���
    {
        Debug.Log("�ð��� ǥ��:���");
        if (UIVisualController.Instance.visual == true)
        {
            Debug.Log("�ð��� ǥ��:����");
            UIVisualController.Instance.visual = false;
            UIVisualController.Instance.visualChange();
        } else if (UIVisualController.Instance.visual == false)
        {
            Debug.Log("�ð��� ǥ��:�ѱ�");
            UIVisualController.Instance.visual = true;
            UIVisualController.Instance.visualChange();
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