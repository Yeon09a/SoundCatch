using UnityEngine;

enum HandGesture
{
    paper, // �չٴ�
    rock // �ָ�
}

public enum MainGame
{
    hiddenSound, // ���� �Ҹ� ã��
    setSound, // �� ���߱�
    causeSound // �Ҹ��� ã��
}

public class HandTracking : MonoBehaviour
{
    public MainGame mainGame = MainGame.causeSound;

    // event
    public UIFunctionEvent uiFunEvent;
    public GameObjectFunctionEvent gameObjectFunEvent;

    private bool isCameraOn = false; // ���̽� ������ ����Ǿ�����

    // �ν�
    private RaycastHit hit;
    private RaycastHit preHit;
    private bool isHandRock = false; // �� ����� �ָ��ΰ�
    // �ν� Ÿ�̸�
    private float rockTime = 0.0f;

    // �����
    public AudioSource audioSource;
    public AudioSource subAscr;
    private Sound sound;
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
                float x = 7 - float.Parse(points[27]) / 100;
                float y = float.Parse(points[28]) / 100;
                float z = float.Parse(points[29]) / 100;

                // �� ��� ��������
                float handG = float.Parse(points[63]);
                HandGesture handGesture = (HandGesture)((int)handG);

                Vector3 handCenter = new Vector3(x, y, z);

                Debug.DrawRay(handCenter, Vector3.forward, Color.blue, 300.0f); // �ӽ� ���̾� ǥ��

                if (Physics.Raycast(handCenter, Vector3.forward, out hit, 300.0f, bLayer | uLayer | gLayer)) 
                {
                    //Debug.Log(hit.collider.name);

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
                        switch (mainGame)
                        {
                            case MainGame.hiddenSound: // ���� �Ҹ� ã���� ���� ������Ʈ �ν� �κ�

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
                                gameObjectFunEvent.SSRaise(sound.objectNum, handCenter);

                                // ���� ������Ʈ �ָ� �ν�
                                if (CognizeHandGesture(handGesture, 3.0f)) // �Ű����� 3.0f �����ؼ� ���ϴ� �� ��ŭ �ָ��� ���� �Լ� ���� ����
                                {
                                    // ���� ������Ʈ�� ��� �ش� ������Ʈ�� ���õǾ��� ��
                                    gameObjectFunEvent.Raise(sound.objectNum);
                                }

                                break;
                        }
                    }

                    preHit = hit;
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
        if (!audioSource.isPlaying || hit.collider.gameObject != preHit.collider.gameObject)
        {
            sound = hit.collider.GetComponent<Sound>();

            audioSource.Stop();
            audioSource.loop = true;
            audioSource.clip = sound.cubeSound;
            audioSource.volume = 0.1f;
            audioSource.Play();

            rockTime = 0.0f;
        } 
    }

    // �Ҹ� ���(UI�� ���� ������ �ְ� �ݺ��ؼ� ����ִ� �����)
    private void PlaySound(float timer) // �Ű����� : ����ִ� ������ �� �ʷ� �� ������.
    {

        if (audioSource.clip == null || hit.collider.gameObject != preHit.collider.gameObject)
        {
            sound = hit.collider.GetComponent<Sound>();

            audioSource.Stop();
            audioSource.volume = 0.1f;
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
    public void SetCurScene(MainGame gameName)
    {
        mainGame = gameName;
    }
}
