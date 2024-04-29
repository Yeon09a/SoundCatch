using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TuningSoundManager : MonoBehaviour
{
    int stagePitch;
    int pPitch;
    bool gesture;
    Vector3 handPos;
    HandTracking ht = new HandTracking();
    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        // ������ �� 1~8 ���� ���� �ȿ��� �����ϰ� �������� ��ġ�� ����
        stagePitch = UnityEngine.Random.Range(1, 9);
        // ���� �÷��̾� ��ġ�� 1('��'��)���� ����
        pPitch = 1;

    }

    // Update is called once per frame
    void Update()
    {
        // �� ���� �޾ƿ���
        handPos = ht.getHandInfo();
        // �� ��ǥ�� viewport point�� ��ȯ
        handPos = cam.WorldToViewportPoint(handPos);
        // viewport point�� y���� ���� ��ġ ����
        pPitch = (int)System.Math.Truncate((double)(handPos[1] * 10 / 8));

        // �ָ��� ������� ���� ��������. ������� true, ������ false
        gesture = ht.getGestureInfo();

        // �ָ��� ����� ��
        if (gesture)
        {
            // ��ġ�� ����ٸ�
            if (pPitch == stagePitch)
            {
                // ���� �¸�
                Debug.Log("Win");
            }
        }
    }
}
