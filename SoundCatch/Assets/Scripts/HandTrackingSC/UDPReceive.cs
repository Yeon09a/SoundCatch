using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPReceive : Singleton<UDPReceive>
{
    public bool startRecieving = true;
    public bool printToConsole = false;
    Thread receiveThread;
    UdpClient client;
    public int port = 5052;
    public string data;

    public RunPython runPython = new RunPython();

    public static UDPReceive instance = null;

    private void Awake()
    {
        // �̱���
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        runPython.RunExe();

        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    private void ReceiveData()
    {

        client = new UdpClient(port);
        while (startRecieving)
        {

            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] dataByte = client.Receive(ref anyIP);
                data = Encoding.UTF8.GetString(dataByte);

                if (printToConsole) { print(data); }
            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }

    public void ExitHandTracking()
    {
        startRecieving = false;
        receiveThread.Abort();
        client.Close();
        runPython.StopPythonExe();
    }



    void Update()
    {
        // �ӽ� �ڵ�Ʈ��ŷ ����
        /*if (Input.GetKeyDown(KeyCode.X))
        {
            ExitHandTracking();
        }*/
    }
}
