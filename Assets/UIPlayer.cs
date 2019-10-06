using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIPlayer : MonoBehaviour
{
    public TextMeshProUGUI hpText;
    public IntegerVariable hp;
    public TextMeshProUGUI levelText;
    public IntegerVariable level;

    public TextMeshProUGUI activeEffects;
    public TextMeshProUGUI str;
    public IntegerVariable playerDamageMod;

    private void Update()
    {
        this.hpText.text = string.Format("HP: {0}", this.hp.Value);
        this.levelText.text = string.Format("Level: {0}", this.level.Value);

        string activeEffects = "";
        foreach (var activeEffect in GameManager.Instance.activeEffects)
        {
            activeEffects += string.Format("{0} +{1}: {2}\n", activeEffect.effect.name, activeEffect.amount, activeEffect.timer);
        }
        this.activeEffects.text = activeEffects;

        this.str.text = string.Format("STR: {0}", this.playerDamageMod.Value);
    }
}
