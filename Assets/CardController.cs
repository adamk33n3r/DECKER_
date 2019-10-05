using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{

    public Card card;

    private void Start()
    {

    }

    private void Update()
    {

    }

    public void Activate()
    {
        GameManager.Instance.ProcessCard(this.card, null);
    }
}
