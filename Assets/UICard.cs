using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UICard : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;

    private CardController controller;
    private CanvasGroup canvasGroup;

    private Turn cachedTurn;

    void Start()
    {
        this.cachedTurn = GameManager.Instance.WhichTurn;
        this.controller = GetComponentInParent<CardController>();
        this.canvasGroup = GetComponent<CanvasGroup>();
        this.title.text = this.controller.card.name;
        string effects = this.controller.card.effects.Count > 0 ? "" : "No Effects";
        foreach (var effect in this.controller.card.effects)
        {
            effects += string.Format("{0} +{1}: {2}\n", effect.name, effect.amount, effect.time);
        }
        this.description.text = effects;
    }

    // Update is called once per frame
    void Update()
    {
        Turn turn = GameManager.Instance.WhichTurn;
        if (turn == this.cachedTurn)
            return;

        //this.gameObject.SetActive(turn == Turn.Player);
        this.canvasGroup.alpha = turn == Turn.Player ? 1 : 0.5f;
        this.cachedTurn = turn;
    }
}
