using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    protected virtual void Awake()
    {
        Instance = this;
    }

    public virtual void OnGameStart()
    {

    }
}