using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioSource subAudioSource;

    public AudioInfoSO _ClickRightBlock;
    public AudioEventChannelSO _ClickRightBlockEC;

    public AudioClip[] clips;

    //-----------------------------

    public GameObject a;
    public GameObject b;
    public GameObject c;

    public Vector2 ssPos;
    public Vector2 snPos;
    public Vector2 sdPos;

    //----------------------------------

    public Sound playgroundS;
    
    public bool[] arrBool = new bool[4] { false, false, false, false};

    public float maxSize = 5f;
    public float maxVolume = 0.8f;

    public Vector2 handPos;

    public AudioClip noise;

    private bool inSN = false;


    void Start()
    {
        audioSource = GameObject.FindGameObjectWithTag("HTManager").GetComponent<AudioSource>();
        subAudioSource = audioSource.GetComponentsInChildren<AudioSource>()[1];
        subAudioSource.clip = noise;
        int index = Random.Range(0, 4);
        ssPos = SetPos(index);

        snPos = SetObstacle();
        sdPos = SetObstacle();

        playgroundS.cubeSound = clips[Random.Range(0, 11)];

        //------------------------------
        a.transform.position = ssPos;
        b.transform.position = snPos;
        c.transform.position = sdPos;
        //--------------------------------
    }

    public void CheckSound(Vector3 handPos)
    {
        this.handPos = new Vector2(handPos.x, handPos.y);

        switch (CheckPosArea())
        {
            case 0:
                subAudioSource.Stop();
                audioSource.volume = CalDis(handPos);
                inSN = false;
                break;
            case 1: // 노이즈
                if (!inSN)
                {
                    subAudioSource.Play();
                }
                inSN = true;
                break;
            case 2: // 볼륨 줄이기
                subAudioSource.Stop();
                float vol = audioSource.volume >= 0.04 ? audioSource.volume - 0.03f : 0.01f;
                audioSource.volume = vol;
                inSN = false;
                break;
        };
    }

    public void CheckAnswer(int objectNum)
    {
        if (handPos.x >= ssPos.x - 0.4f && handPos.x <= ssPos.x + 0.4f && handPos.y >= ssPos.y - 0.4f && handPos.y <= ssPos.y + 0.4f)
        {
            _ClickRightBlockEC.RaisePlayAudio(_ClickRightBlock);
            SceneLoader.Instance.ChangeScene("GameClear");

            //Debug.Log("Clear"); // 임의 작성
        } else
        {
            audioSource.Play();
        }
    }

    private float CalDis(Vector3 handPos)
    {
        float dis = Vector2.Distance(this.handPos, ssPos);

        if (dis > maxSize)
        {
            return 0.01f;
        }
        else if (dis > 0.4f)
        {
            return (maxVolume - 0.3f) * (1f - Mathf.Clamp01(dis / maxSize));
        }
        else
        {
            return maxVolume;
        }
    }

    private int CheckPosArea()
    {
        if (handPos.x >= snPos.x - 0.2f && handPos.x <= snPos.x + 0.2f && handPos.y >= snPos.y - 0.2f && handPos.y <= snPos.y + 0.2f) // 장애물 구역(노이즈)
        {
            return 1;
        } else if(handPos.x >= sdPos.x - 0.2f && handPos.x <= sdPos.x + 0.2f && handPos.y >= sdPos.y - 0.2f && handPos.y <= sdPos.y + 0.2f) // 장애물 구역(소리 감소)
        {
            return 2;
        } else // 일반 구역
        {
            return 0;
        }
    }

    private Vector2 SetPos(int i)
    {
        arrBool[i] = true;
        switch (i)
        {
            case 0:
                return new Vector2(Random.Range(-2.7f, -0.1f), Random.Range(3.7f, 4.7f));
            case 1:
                return new Vector2(Random.Range(0.7f, 3.3f), Random.Range(3.7f, 4.7f));
            case 2:
                return new Vector2(Random.Range(-2.7f, -0.1f), Random.Range(1.8f, 2.8f));
            default:
                return new Vector2(Random.Range(0.7f, 3.3f), Random.Range(1.8f, 2.8f));

        }
    }

    private Vector2 SetObstacle()
    {
        int i;
        do
        {
            i = Random.Range(0, 4);
        } while (arrBool[i]);

        return SetPos(i);
    }
}
