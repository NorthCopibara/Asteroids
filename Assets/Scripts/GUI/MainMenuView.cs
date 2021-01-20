using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : MonoBehaviour
{
    [SerializeField] private Button _startGame;

    public void Init(Action startGame) 
    {
        _startGame.onClick.AddListener(() => startGame?.Invoke());
    }

    public void Disable() 
    {
        _startGame.onClick.RemoveAllListeners();
    }
}
