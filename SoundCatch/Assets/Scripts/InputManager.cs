using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    public UIFunctionEvent uiFunEvent;
    public UIFunctionEvent uiOutlineEvent;
    public AudioSource guidevoiceSC;

    public GameObject gamepause;

    [SerializeField] private int uiNum = -1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // setting â ������ ���� ó��
            if (SceneManager.GetActiveScene().name == "Setting") {
                Debug.Log("������ ȭ��ǥ");
                if (uiNum < 3) 
                {
                    uiNum = 2;
                }
                if(uiNum != 2)
                {
                    uiOutlineEvent?.Raise(uiNum + 6);
                }
                else
                {
                    uiOutlineEvent?.Raise(9);
                }
                uiNum += 1;
                if (uiNum > 5)
                {
                    uiNum = 3;
                }
                uiOutlineEvent?.Raise(uiNum + 3);
            }
            else {
                if(uiNum > 2)      // setting�� ����� �� ���󺹱�
                {
                    uiNum = -1;
                }
                if (uiNum != -1) // uiNum�� -1�� �ƴ� ���(���� �ٸ� UI�� �ƿ������� �ִ� ���)
                {
                    uiOutlineEvent?.Raise(uiNum + 3); // ���� �ƿ������� �ִ� UI�� �ƿ������� ����.
                }
                else // uiNum�� -1(InputManager�� ó�� ����Ǿ��� ��)�� ���
                {
                    uiOutlineEvent?.Raise(3); // ù ��° UI�� OffOutline ����(Outline�� ����.)
                }
                uiNum += 1; // ���� UI�� ����Ų��.
                if (uiNum > 2) // ���� UI�� 2���� Ŭ ���(uiNum�� 0 ~ 2������(3��) �����Ƿ�).
                {
                    uiNum = 0; // uiNum�� 0���� ù��° UI�� ����Ű���� ��
                }
                uiOutlineEvent?.Raise(uiNum); // �ش� UI�� �ƿ������� �Ҵ�.
            }
        } else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (SceneManager.GetActiveScene().name == "Setting")
            {
                Debug.Log("���� ȭ��ǥ");
                if (uiNum < 3)
                {
                    uiNum = 2;
                }
                if (uiNum != 2)
                {
                    uiOutlineEvent?.Raise(uiNum + 6);
                }
                else
                {
                    uiOutlineEvent?.Raise(11);
                }
                uiNum -= 1;
                if (uiNum < 3)
                {
                    uiNum = 5;
                }
                uiOutlineEvent?.Raise(uiNum + 3);
            }
            else
            {
                if (uiNum > 2)     // setting�� ����� �� ���󺹱�
                {
                    uiNum = -1;
                }
                if (uiNum != -1)// uiNum�� -1�� �ƴ� ���(���� �ٸ� UI�� �ƿ������� �ִ� ���)
                {
                    uiOutlineEvent?.Raise(uiNum + 3); // ���� �ƿ������� �ִ� UI�� �ƿ������� ����.
                }
                else // uiNum�� -1(InputManager�� ó�� ����Ǿ��� ��)�� ���
                {
                    uiOutlineEvent?.Raise(5); // ������ ��° UI�� OffOutline ����(Outline�� ����.)
                }
                uiNum -= 1;// ���� UI�� ����Ų��.
                if (uiNum < 0) // ���� UI�� 0���� ���� ���(uiNum�� 0 ~ 2������(3��) �����Ƿ�).
                {
                    uiNum = 2;// uiNum�� 2�� ������ UI�� ����Ű���� ��
                }
                uiOutlineEvent?.Raise(uiNum); // �ش� UI�� �ƿ������� �Ҵ�.
            }            
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!guidevoiceSC.isPlaying) // �ȳ������� ��µǰ� ���� ���� ��� 
            {
                if(SceneManager.GetActiveScene().name == "Setting")
                {
                    if(uiNum == 4)
                    {
                        gamepause.GetComponent<GamePause>().Resume();
                        gamepause.GetComponent<GamePause>().paused = false;
                    }
                    uiFunEvent?.Raise(uiNum - 3);
                }
                else
                {
                    uiFunEvent?.Raise(uiNum); // �ش� UI�� �Լ� ����
                }
            }
        }
    }



}
