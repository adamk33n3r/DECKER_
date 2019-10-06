using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CardPoolController : MonoBehaviour
{
    public IntegerVariable playerLevel;
    public CardPool availableCards;
    public CardController cardPrefab;
    public GameObject poolLocation;

    private void Awake()
    {
        //this.gameObject.SetActive(false);
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
    private void OnEnable()
    {
        var selectedCards = this.availableCards.GetCards(this.playerLevel);
        if (selectedCards.Count() == 0)
        {
            Debug.Log("no cards");
            GameManager.Instance.CodedProgram(null);
            this.gameObject.SetActive(false);
            return;
        }

        int pos = 0;
        float width = 3f;
        float padding = 0.25f;
        float startingPos = ((width + padding) * (selectedCards.Count() - 1)) / 2;
        foreach (var card in selectedCards)
        {
            var cardController = Instantiate(this.cardPrefab, this.poolLocation.transform);
            cardController.card = card;

            cardController.gameObject.transform.Translate(new Vector3((width + padding) * pos - startingPos, 0, 0));

            // Clear existing listener
            var button = cardController.gameObject.GetComponentInChildren<Button>();
            button.onClick.AddListener(() => { AddCardToDeck(card); });
            pos++;
        }
    }

    private void AddCardToDeck(Card card) {
        GameManager.Instance.CodedProgram(card);
        for (int child = 0; child < this.poolLocation.transform.childCount; child++)
        {
            Destroy(this.poolLocation.transform.GetChild(child).gameObject);
        }
        this.gameObject.SetActive(false);
    }

}
