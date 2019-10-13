using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnableButtonOnTurn : MonoBehaviour
{
    public Turn turn;

    private Button button;

    private void Start()
    {
        this.button = GetComponent<Button>();
    }

    void Update()
    {
        this.button.interactable = GameManager.Instance.WhichTurn == this.turn;
    }
}
