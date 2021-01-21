using System;
using UnityEngine;
using UnityEngine.UI;

public class GameOverView : MonoBehaviour
{
    [SerializeField] private Button _restartGame;

    public void Init(Action endGame)
    {
        _restartGame.onClick.AddListener(() => endGame?.Invoke());
    }

    public void Disable()
    {
        _restartGame.onClick.RemoveAllListeners();
    }
}
