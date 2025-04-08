using Fusion;
using UnityEngine;
using TMPro;

public class GameplayManager : SimulationBehaviour, IPlayerJoined
{
    public GameObject PlayerPrefab;

    private NetworkObject _player;

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            _player = Runner.Spawn(PlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            //_player.transform.GetChild(0).GetComponent<TMP_Text>().text = Runner.LocalPlayer.ToString();
        }
    }
}
