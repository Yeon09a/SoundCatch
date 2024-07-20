using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TuningSoundManagerNew : MonoBehaviour
{
    int stagePitch;
    int stageLv;
    Scene scene;

    public AudioSource audioSource;
    public AudioSource subAudioSource;
    public AudioClip[] sounds;

    public AudioInfoSO _ClickRightBlock;
    public AudioEventChannelSO _ClickRightBlockEC;

    float asTime = 1.0f;    // �������� �� �ݺ������

    GameObject ht;
    // private GameObjectEventListener listener;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        // listener = GetComponent<GameObjectEventListener>();

        ht = GameObject.FindGameObjectWithTag("HTManager");
        audioSource = ht.GetComponent<AudioSource>();
        subAudioSource = ht.GetComponentInChildren<AudioSource>();

        // ���� ���̵��� ���� �������� ��ġ ����
        scene = SceneManager.GetActiveScene();
        if (SceneLoader.Instance.mainGame == MainGame.tuningSoundNew1)
        {
            stageLv = 1;
            stagePitch = UnityEngine.Random.Range(0, 4);
        }
        else if (SceneLoader.Instance.mainGame == MainGame.tuningSoundNew2)
        {
            stageLv = 2;
            stagePitch = UnityEngine.Random.Range(0, 6);
        }
        else if (SceneLoader.Instance.mainGame == MainGame.tuningSoundNew3)
        {
            stageLv = 3;
            stagePitch = UnityEngine.Random.Range(0, 11);
        }
        else
        {
            Debug.Log("Error");
            stagePitch = 0;
        }

        asTime = -3.5f;
        // ���� �÷��̾� ��ġ�� 0('��'��)���� ����
        // pPitch = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // �������� �� �ݺ� ���
        asTime += Time.deltaTime;
        if (asTime >= 5.0f)
        {
            audioSource.PlayOneShot(sounds[stagePitch]);
            asTime = 0.0f;
        }
    }

    public void checkAnswer(int obNum)
    {
        Debug.Log("Check");
        if(obNum == stagePitch)
        {
            Debug.Log("����");
            // ���� Ŭ���� ������ ��ȯ
            audioSource.loop = false;
            _ClickRightBlockEC.RaisePlayAudio(_ClickRightBlock);
            SceneLoader.Instance.ChangeScene("GameClear");
        }
        else
        {
            Debug.Log("����");
        }
    }
}
