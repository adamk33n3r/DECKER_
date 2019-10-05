using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEnemyTurnButton : MonoBehaviour
{
    private Button button;

    private void Start()
    {
        this.button = GetComponent<Button>();
    }

    void Update()
    {
        this.button.interactable = GameManager.Instance.WhichTurn == Turn.Enemies;
    }
}
