using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIEnemy : MonoBehaviour
{
    public TextMeshProUGUI enemyName;
    public TextMeshProUGUI activeEffects;
    public TextMeshProUGUI hp;
    public TextMeshProUGUI att;

    private EnemyController controller;

    private void Start()
    {
        this.controller = GetComponentInParent<EnemyController>();
        this.controller.OnUpdate += UpdateText;
        UpdateText();
    }

    private void OnDestroy()
    {
        this.controller.OnUpdate -= UpdateText;
    }

    public void UpdateText()
    {
        this.enemyName.text = this.controller.name;
        string activeEffects = "";
        foreach (var activeEffect in this.controller.activeEffects)
        {
            activeEffects += string.Format("{0} +{1}: {2}\n", activeEffect.effect.name, activeEffect.effect.amount, activeEffect.timer);
        }
        this.activeEffects.text = activeEffects;
        this.hp.text = string.Format("HP: {0}", this.controller.health);
        this.att.text = string.Format("ATT: {0}+{1}", this.controller.damage, this.controller.damageMod);
    }
}
