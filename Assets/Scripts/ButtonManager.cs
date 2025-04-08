using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public NetworkManager m_NetworkManager;
    // Start is called before the first frame update

    public void StartHost() 
    {
        m_NetworkManager.StartHost();
    }
    public void StartServer()
    {
        m_NetworkManager.StartServer();
    }
    public void StartClient()
    {
        m_NetworkManager.StartClient();
    }
}
