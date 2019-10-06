using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class CardPool : ScriptableObject
{
    public List<Card> pool;

    private static int[] probabilities = new int[] {
        100,
        30,
        20,
        10,
        5,
    };

    public IEnumerable<Card> GetCards(int hackerLevel)
    {
        List<Card> selectedCards = new List<Card>(hackerLevel);
        // Higher level more cards to choose from
        int rank = -1;
        for (int level = 1; level <= hackerLevel; level++)
        {
            for (int l = hackerLevel; l > 0; l--)
            {
                if (l > probabilities.Length)
                    continue;
                int chance = Random.Range(0, 100);
                int prob = probabilities[l - 1];
                if (chance <= prob)
                {
                    bool cardsOfThisLevel = this.pool.Any((card) => card.level == l);
                    if (!cardsOfThisLevel)
                        continue;
                    rank = l;
                    break;
                }
            }

            var levelCards = this.pool.Where((card) => card.level == rank);
            if (levelCards.Count() == 0)
            {
                continue;
            }
            int randomCard = Random.Range(0, levelCards.Count());
            Debug.Log(string.Format("randomCardIdx: " + randomCard));
            Debug.Log(string.Format("levelCardsCount: " + levelCards.Count()));
            Card selectedCard = levelCards.ElementAt(randomCard);
            selectedCards.Add(selectedCard);
        }

        Debug.Log(selectedCards.Count());

        return selectedCards;
    }
}
