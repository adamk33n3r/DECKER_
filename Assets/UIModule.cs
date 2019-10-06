using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIModule : MonoBehaviour
{
    public IntegerVariable playerHackerTokens;
    public Module module;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI costText;
    public Button panelButton;
    public Image costImage;

    private void Update()
    {
        this.nameText.text = module.name;
        this.costText.text = string.Format("§{0}", module.price);
        bool tooExpensive = module.price > this.playerHackerTokens;
        this.panelButton.interactable = !tooExpensive;
        this.costImage.color = tooExpensive ? this.panelButton.colors.disabledColor : Color.white;
    }
}
