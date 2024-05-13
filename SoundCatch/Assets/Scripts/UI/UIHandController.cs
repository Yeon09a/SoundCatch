using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// �� ��ġ�� ����ٴϸ� ���� ������� ����� �˷��ִ� ui ������ ���� ��ũ��Ʈ
public class UIHandController : MonoBehaviour
{
    // ��������Ʈ ���
    public Sprite handClose;
    public Sprite handOpen;
    public Sprite handV;

    // �̹���
    Image handImg;

    // �� �������� ���󰡱� ����
    GameObject ht;
    RectTransform rectTransform;
    Vector3 handPos;
    int gesture;

    // Start is called before the first frame update
    void Start()
    {
        handImg = GetComponent<Image>();
        ht = GameObject.FindGameObjectWithTag("HTManager");
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        // �� ��ġ�� �°� �� UI �̵�
        handPos = ht.GetComponent<HandTracking>().getHandPos();
        rectTransform.position = handPos;

        // �� ��翡 �°� �� UI ��� ����
        gesture = ht.GetComponent<HandTracking>().getGestureInfo();
        if (gesture == 0 )
        {
            handImg.sprite = handOpen;
        } else if (gesture == 1 )
        {
            handImg.sprite = handClose;
        }
        else if(gesture == 2 )
        {
            handImg.sprite = handV;
        }
    }
}
