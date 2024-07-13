using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuningSoundManagerNew : MonoBehaviour
{
    int stagePitch;

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

        // ������ �� 0~6 ���� ���� �ȿ��� �����ϰ� �������� ��ġ�� ����
        stagePitch = UnityEngine.Random.Range(0, 6);
        asTime = -3.5f;
        audioSource.clip = sounds[stagePitch];
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
