using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UICard : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI level;
    public TextMeshProUGUI description;
    public Image mask;

    private CardController controller;
    private CanvasGroup canvasGroup;

    private Material materialCache;

    void Start()
    {
        SetUp();
    }

    // Update is called once per frame
    void Update()
    {
        //Turn turn = GameManager.Instance.WhichTurn;
        //if (turn == this.cachedTurn)
        //    return;

        ////this.gameObject.SetActive(turn == Turn.Player);
        //this.canvasGroup.alpha = turn == Turn.Player ? 1 : 0.5f;
        //this.cachedTurn = turn;

#if UNITY_EDITOR
        if (!Application.isPlaying)
            SetUp();
#endif

    }

    private void SetUp()
    {
        this.controller = GetComponentInParent<CardController>();
        this.canvasGroup = GetComponent<CanvasGroup>();
        this.title.text = this.controller.card.name;
        this.level.text = string.Format("Level {0}", this.controller.card.level);
        string effects = "";
        if (this.controller.card.physicalDamage > 0)
            effects += string.Format("DMG: {0}\n", this.controller.card.physicalDamage);
        foreach (var cardEffect in this.controller.card.effects)
        {
            string target = cardEffect.target == Target.Self ? "Self" : "Enemy";
            // Is instant. e.g. Reboot or Heal
            if (cardEffect.time == 0)
                effects += string.Format("{0} ({1}) +{2}\n", cardEffect.effect.name, target, cardEffect.amount);
            else
                effects += string.Format("{0} ({3}) +{1}: {2}\n", cardEffect.effect.name, cardEffect.amount, cardEffect.time, target);
        }
        this.description.text = effects;
        if (this.controller.card.type != null)
        {

            if (this.materialCache == null)
                this.materialCache = new Material(this.mask.material);
            this.mask.material = this.materialCache;
            //renderer.SetMaterial(this.materialCache, 0);
            //renderer.GetMaterial().SetColor("_MaskColor", this.controller.card.type.color);
            //this.mask.materialForRendering.SetColor("_MaskColor", this.controller.card.type.color);
            this.mask.material.SetColor("_MaskColor", this.controller.card.type.color);
        }
    }
}
