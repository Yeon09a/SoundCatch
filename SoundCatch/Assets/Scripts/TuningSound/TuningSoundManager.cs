using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class TuningSoundManager : MonoBehaviour
{
    int stagePitch;
    int pPitch;

    int gesture;
    Vector3 handPos;
    Vector3 handPosOld;
    bool pitchChange;

    public AudioSource audioSource;
    public AudioSource subAudioSource;
    public AudioClip clip;
    public AudioClip[] sounds;
    public AudioInfoSO _ClickRightBlock;
    public AudioEventChannelSO _ClickRightBlockEC;

    GameObject ht;

    bool check = false;

    bool set = false;       // ���� ���ÿ�
    float startTime = 0.0f; // ���� Ÿ�̸ӿ�
    float time = 0.0f;      // ���� Ÿ�̸ӿ�
    float asTime = 0.0f;    // �������� �� �ݺ������

    private GameObjectEventListener listener;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;

        ht = GameObject.FindGameObjectWithTag("HTManager");
        audioSource = ht.GetComponent<AudioSource>();
        subAudioSource = ht.GetComponentInChildren<AudioSource>();
        listener = GetComponent<GameObjectEventListener>();

        // ������ �� 0~6 ���� ���� �ȿ��� �����ϰ� �������� ��ġ�� ����
        stagePitch = UnityEngine.Random.Range(0, 6);
        asTime = 0.0f;
        audioSource.clip = sounds[stagePitch];

        // ���� �÷��̾� ��ġ�� 0('��'��)���� ����
        pPitch = 0;
        subAudioSource.clip = sounds[pPitch];

        // �� ��ġ ���� ��������
        handPos = ht.GetComponent<HandTracking>().getViewportPoint();
        handPosOld = handPos;
    }

    void Update()
    {
        // ���� �ȳ� �ð� ���� ���ߵ���
        if(startTime <= 8.26f)
        {
            startTime += Time.deltaTime;
        } else
        {
            if(set == false)
            {
                audioSource.PlayOneShot(sounds[stagePitch]);
                set = true;
            }

            asTime += Time.deltaTime;
            if (asTime >= 5.0f)
            {
                audioSource.PlayOneShot(sounds[stagePitch]);
                asTime = 0.0f;
            }

            // �� ��ġ ���� ����
            handPosOld = handPos;
            handPos = ht.GetComponent<HandTracking>().getViewportPoint();
            // UnityEngine.Debug.Log("Update. handPos : " + handPos);

            // �� ��ġ�� ����Ǹ� �̺�Ʈ �߻�
            if (handPosOld != handPos)
            {
                listener.OnTSEventRaised();
                // UnityEngine.Debug.Log("Event. handPos : " + handPos);
            }

            // �� ����� �ָ��̸� ī��Ʈ
            if (gesture == 1)
            {
                time += Time.deltaTime;
            }
        }        
    }

    public void checkAnswer()
    {
        // viewport point�� y���� ���� ��ġ ����
        int pPitchOld = pPitch;
        setpPitch();

        if (pPitch != pPitchOld)    // ���� �� �Ҹ� ���
        {
            subAudioSource.Stop();
            subAudioSource.clip = sounds[pPitch];
            subAudioSource.Play();
            pitchChange = true;
        }

        // ����ó ���� üũ��
        int oldGesture = gesture;

        // �ָ��� ������� ���� ��������. ������� 1, ������ 0
        gesture = ht.GetComponent<HandTracking>().getGestureInfo();
        if(oldGesture == 0 && gesture == 1)
        {
            check = true;
        }

        if ((gesture == 1) && (time >= 1.0f) && (pitchChange) && check)
        {
            // ��ġ�� ����ٸ�
            if (pPitch == stagePitch)
            {
                // ���� �¸�
                if (subAudioSource.isPlaying)
                {
                    subAudioSource.Stop();
                }
                subAudioSource.clip = sounds[7];
                subAudioSource.Play();

                check = false;
                
                // ���� Ŭ���� ������ ��ȯ
                _ClickRightBlockEC.RaisePlayAudio(_ClickRightBlock);
                SceneLoader.Instance.ChangeScene("GameClear");
            }
            else
            {
                // Ʋ���� ǥ���ϱ�
                if (subAudioSource.isPlaying)
                {
                    subAudioSource.Stop();
                }
                subAudioSource.clip = sounds[8];
                subAudioSource.Play();

                check = false;
            }

            // Ÿ�̸� �ʱ�ȭ
            time = 0.0f;
            // ������ ��ġ�� ����ɶ����� ���� �� ��
            pitchChange = false;
        }
    }

    // �� ��ġ�� ���� pPitch ������. �� ���� ��
    void setpPitch()
    {
        if (handPos[1] < 0.25)
        {
            pPitch = 0;
        }
        else if (handPos[1] < 0.325)
        {
            pPitch = 1;
        }
        else if (handPos[1] < 0.4)
        {
            pPitch = 2;
        }
        else if (handPos[1] < 0.475)
        {
            pPitch = 3;
        }
        else if (handPos[1] < 0.55)
        {
            pPitch = 4;
        }
        else if (handPos[1] < 0.625)
        {
            pPitch = 5;
        }
        else
        {
            pPitch = 6;
        }
    }
}
