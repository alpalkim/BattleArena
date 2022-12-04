using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] private Text _gameResultText;
    [SerializeField] private Button _mainMenuButton;

    private void Awake()
    {
        _mainMenuButton.onClick.AddListener(OpenMainMenu);
    }

    public void SetPanel(bool isWin)
    {
        gameObject.SetActive(true);
        _gameResultText.text = isWin ? "YOU WIN! \n\n Your alive heroes got experience point" : "You lose...\n\n Choose new heroes and try again.";
    }

    private void OpenMainMenu() => SceneManager.LoadSceneAsync(0);
}