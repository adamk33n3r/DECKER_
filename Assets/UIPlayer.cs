using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIPlayer : MonoBehaviour
{
    public TextMeshProUGUI hpText;
    public IntegerVariable hp;

    private void Update()
    {
        this.hpText.text = string.Format("HP: {0}", this.hp.Value);
    }
}
