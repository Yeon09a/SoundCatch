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
    // ����
    AudioSource audioSource;
    public AudioClip[] handClips;

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

        audioSource = ht.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // �� ��ġ�� �°� �� UI �̵�
        handPos = ht.GetComponent<HandTracking>().getHandPos();
        rectTransform.position = handPos;

        int oldGesture = gesture;
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
        
        // �� ��� ����� �Ҹ� ���
        if (oldGesture !=  gesture)
        {
            if (gesture == 0)
            {
                audioSource.PlayOneShot(handClips[0]);
            }
            else if (gesture == 1)
            {
                audioSource.PlayOneShot(handClips[1]);
            }
            else if (gesture == 2)
            {
                audioSource.PlayOneShot(handClips[2]);
            }
        }
    }
}
