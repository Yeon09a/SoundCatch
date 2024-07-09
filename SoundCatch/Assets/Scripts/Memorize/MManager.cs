using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memorize : MonoBehaviour
{
    private HandTracking hT;
    private AudioSource audioSource;

    public AudioClip[] clips; // 0 : open 1: close 2 : scissor
    public AudioClip[] resultClips; // 0 : ���� 1 : ����
    public AudioClip testStart; // ����ϼ��� ���� ����
    public AudioClip answerStart; // ����!
    public AudioClip completeClip; // Ŭ����
    public AudioClip bingoClip; // ���� 
    public AudioClip failClip; // ����
    public AudioClip[] countdownClip;

    private int[] level_1 = new int[3];
    private int[] level_2 = new int[5];
    private int[] level_3 = new int[7];
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject htManager = GameObject.FindGameObjectWithTag("HTManager");
        hT = htManager.GetComponent<HandTracking>();
        audioSource = htManager.GetComponent<AudioSource>();
        audioSource.panStereo = 0;
        audioSource.volume = 0.8f;
        audioSource.loop = false;

        for (int i = 0; i < 7; i++)
        {
            if (i < 3)
            {
                level_1[i] = Random.Range(0, 3);
                level_2[i] = Random.Range(0, 3);
                level_3[i] = Random.Range(0, 3);
            } else if (i < 5)
            {
                level_2[i] = Random.Range(0, 3);
                level_3[i] = Random.Range(0, 3);
            } else if (i < 7)
            {
                level_3[i] = Random.Range(0, 3);
            }
        }

        Invoke("StartGameCor", 8.5f);
    }

    public void StartGameCor()
    {
        StartCoroutine(PlayGame());
    }

    IEnumerator GameStart(int[] soundArr)
    {
        yield return StartCoroutine(PlayTest(soundArr));
        PlayAudio(answerStart);
        for (int i = 0; i < soundArr.Length; i++)
        {
            yield return StartCoroutine(Countdown()); // ī��Ʈ�ٿ�
            if (hT.getGestureInfo() == soundArr[i]) // ����
            {
                PlayAudio(bingoClip);
                yield return new WaitForSeconds(1.0f);
            } else // ������ �ƴ� ���
            {
                PlayAudio(failClip);
                // ���� ������ ���� Ŭ���� ������ �̵�
                yield return new WaitForSeconds(1.0f);
                SceneLoader.Instance.ChangeScene("GameClear");
            }
        }

        yield break;
    }


    IEnumerator PlayTest(int[] soundArr) // �ܿ��� �� ���� ���
    {
        for (int i = 0; i < soundArr.Length; i++)
        {
            PlayAudio(clips[soundArr[i]]);
            yield return new WaitForSeconds(2.0f);
        }
        yield break;
    }

    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(1.0f); // ���� ���� ������ ���� �Ǵ�
        PlayAudio(countdownClip[0]);
        yield return new WaitForSeconds(1.0f);
        PlayAudio(countdownClip[0]);
        yield return new WaitForSeconds(1.0f);
        PlayAudio(countdownClip[1]);
        yield return new WaitForSeconds(1.0f); 
    }

    IEnumerator PlayGame()
    {
        for (int i = 0; i < 4; i++)
        {
            switch (i)
            {
                case 0:
                    yield return StartCoroutine(GameStart(level_1));
                    break;
                case 1:
                    yield return StartCoroutine(GameStart(level_2));
                    break;
                case 2:
                    yield return StartCoroutine(GameStart(level_3));
                    break;
                case 3:
                    SceneLoader.Instance.ChangeScene("GameClear");
                    break;
            }
        }

        // ���� Ŭ���� ������ �̵�.
    }

    private void PlayAudio(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
