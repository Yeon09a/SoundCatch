using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameObjectEventListener : MonoBehaviour
{
    public GameObjectFunctionEvent Event;
    public UnityEvent<int> Response;

    public UnityEvent<Vector3> ssResponse;
    public UnityEvent tsResponse;

    public void OnEventRaised(int objectIndex)
    {
        Response.Invoke(objectIndex); 
    }

    private void OnEnable()
    { 
        Event.RegisterListener(this);
    }

    private void OnDisable()
    { 
        Event.UnregisterListener(); 
    }

    // �Ҹ��� ã�� ���ӿ��� �ʿ��ؼ� �߰�
    public void OnSSEventRaised(Vector3 handPos)
    {
        ssResponse.Invoke(handPos);
    }

    // �� ���߱� ���ӿ��� �ʿ��ؼ� �߰�
    public void OnTSEventRaised()
    {
        tsResponse.Invoke();
    }
}
