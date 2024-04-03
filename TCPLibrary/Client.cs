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
        var inputed = false;
        if (Input.GetKey(KeyCode.W))
        {
            inputed = true;
            _clientInterface.SendInput(0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputed = true;
            _clientInterface.SendInput(1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputed = true;
            _clientInterface.SendInput(2);
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputed = true;
            _clientInterface.SendInput(3);
        }
        if (inputed == false)
        {
            _clientInterface.SendInput(4);
        }
    }
}