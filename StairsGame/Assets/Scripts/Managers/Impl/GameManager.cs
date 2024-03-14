using RobbieWagnerGames.Managers;
using UnityEngine;

public class GameManager : MonoBehaviour, IGameManager
{
    public static string persistentDataPath;
    public GameState CurrentGameState()
    {
        throw new System.NotImplementedException();
    }

    protected void Awake()
    {
        persistentDataPath = Application.persistentDataPath;
    }
}