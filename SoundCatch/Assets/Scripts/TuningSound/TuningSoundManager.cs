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

    public AudioSource audioSource;
    public AudioSource subAudioSource;
    public AudioClip clip;
    public AudioClip[] sounds;

    GameObject ht;
    float time = 0.0f;
    float asTime = 0.0f;

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
        UnityEngine.Debug.Log("stagePitch : " + stagePitch);
        asTime = 0.0f;
        audioSource.Stop();
        audioSource.PlayOneShot(sounds[stagePitch]);

        audioSource.clip = sounds[stagePitch];
        audioSource.Play();

        // ���� �÷��̾� ��ġ�� 0('��'��)���� ����
        pPitch = 0;
        subAudioSource.clip = sounds[pPitch];

        // �� ��ġ ���� ��������
        handPos = ht.GetComponent<HandTracking>().getViewportPoint();
        handPosOld = handPos;

        UnityEngine.Debug.Log("Start. handPos : " + handPos);
    }

    private void FixedUpdate()
    {
        // �������� �� �ݺ� ���
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
            UnityEngine.Debug.Log("Event. handPos : " + handPos);
        }

        // �� ����� �ָ��̸� ī��Ʈ
        time += Time.deltaTime;
    }

    public void checkAnswer()
    {
        // viewport point�� y���� ���� ��ġ ����
        int pPitchOld = pPitch;
        if (handPos[1] < 0.55)
        {
            pPitch = 0;
        }
        else if (handPos[1] < 0.65)
        {
            pPitch = 1;
        }
        else if (handPos[1] < 0.75)
        {
            pPitch = 2;
        }
        else if (handPos[1] < 0.85)
        {
            pPitch = 3;
        }
        else if (handPos[1] < 0.95)
        {
            pPitch = 4;
        }
        else if (handPos[1] < 1.05)
        {
            pPitch = 5;
        }
        else
        {
            pPitch = 6;
        }

        UnityEngine.Debug.Log("pPitch : " + pPitch);

        if (pPitch != pPitchOld)    // ���� �� �Ҹ� ���
        {
            subAudioSource.Stop();
            subAudioSource.clip = sounds[pPitch];
            subAudioSource.Play();
        }


        // �ָ��� ������� ���� ��������. ������� 1, ������ 0
        gesture = ht.GetComponent<HandTracking>().getGestureInfo();
        UnityEngine.Debug.Log("Gesture: " + gesture);

        if ((gesture == 1) && (time >= 2.0f))
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
                // ���� �Ͻ�����
                // Time.timeScale = 0.0f;
                // UnityEngine.Debug.Log("Win");
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
                UnityEngine.Debug.Log("Try Again");

            }

            // Ÿ�̸� �ʱ�ȭ
            time = 0.0f;
        }
    }
}
