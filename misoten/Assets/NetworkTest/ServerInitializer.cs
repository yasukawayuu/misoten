using UnityEngine;
using Unity.Netcode;

public class ServerInitializer : MonoBehaviour
{
    void Start()
    {
        NetworkManager.Singleton.StartServer();
    }
}