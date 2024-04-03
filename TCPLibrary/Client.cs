using UnityEngine;

public class Client : MonoBehaviour
{
    private ClientInterface _clientInterface;

    private void Start()
    {
        _clientInterface = new ClientInterface();
        _clientInterface.Start();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
            _clientInterface.SendInput(0);
        if (Input.GetKey(KeyCode.S))
            _clientInterface.SendInput(1);
        if (Input.GetKey(KeyCode.A))
            _clientInterface.SendInput(2);
        if (Input.GetKey(KeyCode.D))
            _clientInterface.SendInput(3);
    }
}