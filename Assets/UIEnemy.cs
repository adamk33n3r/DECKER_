using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIEnemy : MonoBehaviour
{
    public TextMeshProUGUI enemyName;
    public TextMeshProUGUI activeEffects;
    public TextMeshProUGUI hp;
    public TextMeshProUGUI str;

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
        this.enemyName.text = this.controller.enemyData.name;
        string activeEffects = "";
        //if (this.controller.stunned)
        //    activeEffects += "Stunned\n";
        foreach (var activeEffect in this.controller.activeEffects)
        {
            activeEffects += string.Format("{0} +{1}: {2}\n", activeEffect.effect.name, activeEffect.amount, activeEffect.timer);
        }
        this.activeEffects.text = activeEffects;
        this.hp.text = string.Format("HP: {0}", this.controller.health);
        this.str.text = string.Format("STR: {0}", this.controller.damageMod);
    }
}
