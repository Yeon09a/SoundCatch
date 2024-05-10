using System.Collections.Generic;
using UnityEngine;

enum HandGesture
{
    paper, // �չٴ�
    rock // �ָ�
}


public class HandTracking : MonoBehaviour
{
    // event
    public UIFunctionEvent uiFunEvent;
    public GameObjectFunctionEvent gameObjectFunEvent;

    private bool isCameraOn = false; // ���̽� ������ ����Ǿ�����

    // �ν�
    private RaycastHit hit;
    public GameObject preHit;
    private bool isHandRock = false; // �� ����� �ָ��ΰ�
    // �� �ν� �߽� ��ǥ
    private float x;
    private float y;
    private float z;
    // �ν� Ÿ�̸�
    private float rockTime = 0.0f;

    // �����
    public AudioSource audioSource;
    public AudioSource subAscr;
    private Sound sound;
    private Sound subsound;

    // ���� Ÿ�̸�
    private float soundTimer = 0.0f;

    // layer
    private int bLayer = 1 << 3; // ���̾� 3 : Background(���)
    private int uLayer = 1 << 5; // ���̾� 5 : UI
    private int gLayer = 1 << 6; // ���̾� 6 : GameObject

    void Start()
    {
        
    }

    void Update()
    {
        string data = UDPReceive.instance.data; // �ν� ������ �ޱ�

        if (data != "")
        {
            if (!data.Equals("true")) // �ν� �����Ͱ� ��ǥ�� ���
            {
                // �ν� ������ ��ó��
                data = data.Remove(0, 1);
                data = data.Remove(data.Length - 1, 1);
                string[] points = data.Split(',');

                // �ν� ��ǥ�� �߰� ��ǥ ��������(���� �߻��� �κи� ��������)
                x = 7 - float.Parse(points[27]) / 100;
                y = float.Parse(points[28]) / 100;
                z = float.Parse(points[29]) / 100;

                // �� ��� ��������
                float handG = float.Parse(points[63]);
                HandGesture handGesture = (HandGesture)((int)handG);

                Vector3 handCenter = new Vector3(x, y, z);

                Debug.DrawRay(handCenter, Vector3.forward, Color.blue, 300.0f); // �ӽ� ���̾� ǥ��

                if (Physics.Raycast(handCenter, Vector3.forward, out hit, 300.0f, bLayer | uLayer | gLayer)) 
                {
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("UI")) // �ν��� ������Ʈ�� UI�� ���
                    {
                        // �Ҹ� ���
                        PlaySound(3.0f);
                        
                        // UI 3�� �ָ� �ν�
                        if (CognizeHandGesture(handGesture, 3.0f))
                        {
                            // UI ������Ʈ�� ��� �ش� ������Ʈ�� ���õǾ��� ��
                            uiFunEvent.Raise(sound.objectNum);
                        }
                    }
                    else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Background")) // �ν��� ������Ʈ�� Background�� ���
                    {
                        // �Ҹ� ���
                        PlayLoopSound();

                    }
                    else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("GameObject")) // �ν��� ������Ʈ�� GameObject�� ���
                    {

                        // ���� ����
                        switch (SceneLoader.Instance.mainGame)
                        {
                            case MainGame.hiddenSound: // ���� �Ҹ� ã���� ���� ������Ʈ �ν� �κ�

                                // �Ҹ� ���
                                // �ν��� ������Ʈ�� �Ҹ��� ��� �ݺ��ؼ� ����ϸ� PlayLoopSound()
                                // �ν��� ������Ʈ�� �Ҹ��� �� �ʰ� ������ ������ �ݺ��ؼ� ����ϸ� PlaySound(���� ��)
                                PlayLoopSound();

                                // ���� ������Ʈ �ָ� �ν�
                                if (CognizeHandGesture(handGesture, 3.0f)) // �Ű����� 3.0f �����ؼ� ���ϴ� �� ��ŭ �ָ��� ���� �Լ� ���� ����
                                {
                                    // ���� ������Ʈ�� ��� �ش� ������Ʈ�� ���õǾ��� ��
                                    uiFunEvent.Raise(sound.objectNum);
                                }

                                break;
                            case MainGame.setSound: // �� ���߱��� ���� ������Ʈ �ν� �κ�

                                // �Ҹ� ���
                                // �ν��� ������Ʈ�� �Ҹ��� ��� �ݺ��ؼ� ����ϸ� PlayLoopSound()
                                // �ν��� ������Ʈ�� �Ҹ��� �� �ʰ� ������ ������ �ݺ��ؼ� ����ϸ� PlaySound(���� ��)

                                // ���� ������Ʈ �ָ� �ν�
                                if (CognizeHandGesture(handGesture, 3.0f)) // �Ű����� 3.0f �����ؼ� ���ϴ� �� ��ŭ �ָ��� ���� �Լ� ���� ����
                                {
                                    // ���� ������Ʈ�� ��� �ش� ������Ʈ�� ���õǾ��� ��
                                    gameObjectFunEvent.Raise(sound.objectNum);
                                }

                                break;
                            case MainGame.causeSound: // �Ҹ��� ã���� ���� ������Ʈ �ν� �κ�

                                // �Ҹ� ���
                                PlayLoopSound();
                                gameObjectFunEvent.SSRaise(handCenter);

                                // ���� ������Ʈ �ָ� �ν�
                                if (CognizeHandGesture(handGesture, 3.0f)) // �Ű����� 3.0f �����ؼ� ���ϴ� �� ��ŭ �ָ��� ���� �Լ� ���� ����
                                {
                                    // ���� ������Ʈ�� ��� �ش� ������Ʈ�� ���õǾ��� ��
                                    gameObjectFunEvent.Raise(sound.objectNum);
                                }

                                break;
                        }
                    }

                    preHit = hit.transform.gameObject;
                }
            }
            else // �ν� �����Ͱ� true�� ��� => ���̽� ������ ����Ǿ��� ���� Ÿ�̹��� ���� ����
            {
                isCameraOn = true;
            }
        }
    }

    // �� ��� �ν�
    private bool CognizeHandGesture(HandGesture hand, float timer)
    {
        bool rockFinish = false;
        
        switch (hand)
        {
            case HandGesture.rock:
                isHandRock = true;

                // �� �ν�
                rockTime += Time.deltaTime;

                if (rockTime >= timer)
                {
                    rockFinish = true;
                    rockTime = 0.0f;
                }
                break;
            default:
                isHandRock = false;
                rockTime = 0.0f;
                break;
        }

        return rockFinish;
    }
    
    // �Ҹ� ���(GameObject, Background�� ���� ��� �ݺ��ؼ� ����ִ� �����)
    private void PlayLoopSound()
    {
        if (preHit == null || hit.collider.gameObject != preHit)
        {
            sound = hit.collider.GetComponent<Sound>();
            subAscr.Stop();
            if(sound.isSub)
            {
                subAscr.loop = true;
                subAscr.panStereo = -1;
                audioSource.panStereo = 1;
                subAscr.clip = sound.subSound;
                subAscr.volume = 0.8f;
                subAscr.Play();
            }
            else
            {
                audioSource.panStereo = 0;
                subAscr.panStereo = 0;
            }
            audioSource.Stop();
            audioSource.loop = true;
            audioSource.clip = sound.cubeSound;
            audioSource.volume = 0.8f;
            audioSource.Play();

            rockTime = 0.0f;
        } 
    }

    // �Ҹ� ���(UI�� ���� ������ �ְ� �ݺ��ؼ� ����ִ� �����)
    private void PlaySound(float timer) // �Ű����� : ����ִ� ������ �� �ʷ� �� ������.
    {

        if (preHit == null || hit.collider.gameObject != preHit)
        {
            sound = hit.collider.GetComponent<Sound>();

            audioSource.Stop();
            audioSource.volume = 0.8f;
            audioSource.PlayOneShot(sound.cubeSound);
            audioSource.clip = sound.cubeSound;

            rockTime = 0.0f;
            soundTimer = 0.0f;
        } else
        {
            if (!audioSource.isPlaying)
            {
                soundTimer += Time.deltaTime;

                if (soundTimer >= timer)
                {
                    audioSource.PlayOneShot(sound.cubeSound);
                    soundTimer = 0.0f;
                }
            }
        }
    }

    // ���� ���� ����
    /*public void SetCurScene(MainGame gameName)
    {
        mainGame = gameName;
    }
*/
    // ���� �� ���� ����
    public Vector3 getHandInfo()
    {
        Vector3 pos = new Vector3(x, y, z);

        return pos;
    }
    // ���� �� ��� ����
    public bool getGestureInfo()
    {
        return isHandRock;  
    }
}
