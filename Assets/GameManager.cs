using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum Turn
{
    Player,
    Enemies,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    //public event Action<EnemyController> OnEnemyAction = (enemy) => { };
    public event System.Action OnActionOnPlayer = () => { };

    public IntegerVariable maxPlayerHP;
    public IntegerVariable playerHP;
    public Inventory startingPlayerInventory;
    public Inventory playerInventory;

    public IntegerVariable playerGuard;
    public IntegerVariable playerLevel;
    public IntegerVariable playerXP;
    public IntegerVariable playerDamageMod;
    public IntegerVariable playerHackerTokens;
    public IntegerVariable defaultHandSize;

    public GameObject cardLocation;
    public CardController cardPrefab;
    public CardPoolController cardPool;

    public ShopController shop;

    public ModuleList playerModules;

    public List<ActiveEffect> activeEffects = new List<ActiveEffect>();

    public Turn WhichTurn = Turn.Player;

    private bool pierce = false;
    private bool stunned = false;
    //private bool playerIsFocused = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetUp();
        DrawCards();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            Application.Quit();
    }

    public void ProcessCard(Card card, EnemyController enemy)
    {
        if (WhichTurn == Turn.Enemies)
            return;

        // Self
        Debug.Log("Player playing card: " + card);

        this.pierce = false;
        foreach (var cardEffect in card.effects.Where((effect) => effect.target == Target.Self))
        {
            switch (cardEffect.effect.name)
            {
                case "Heal":
                    this.playerHP.Value += cardEffect.amount;
                    break;
                case "Pierce":
                    this.pierce = true;
                    break;
                case "Focused":
                    //this.playerIsFocused = true;
                    break;
                case "Guard":
                    this.playerGuard = cardEffect.amount;
                    this.activeEffects.Add(new ActiveEffect { effect = cardEffect.effect, amount = cardEffect.amount, timer = cardEffect.time - 1 });
                    break;
                case "Reboot":
                    this.activeEffects.Clear();
                    this.playerInventory.deck.Clear();
                    break;
                default:
                    this.activeEffects.Add(new ActiveEffect { effect = cardEffect.effect, amount = cardEffect.amount, timer = cardEffect.time });
                    break;
            }
        }

        // Enemy

        if (enemy == null)
        {
            enemy = FindObjectOfType<EnemyController>();
        }
        enemy.AddEffects(card.effects.Where((effect) => effect.target == Target.Enemy));

        if (card.physicalDamage != 0)
            enemy.TakeDamage(card.physicalDamage + this.playerDamageMod, this.pierce);

        SwitchToTurn(Turn.Enemies);
    }

    public void ProcessEnemyTurn()
    {
        if (WhichTurn == Turn.Player)
            return;

        var enemies = FindObjectsOfType<EnemyController>();
        foreach (var enemy in enemies)
        {
            enemy.DoAction();
            //OnEnemyAction(enemy);
        }

        SwitchToTurn(Turn.Player);
    }

    private void ProcessEnemyEffects()
    {
        var enemies = FindObjectsOfType<EnemyController>();
        foreach (var enemy in enemies)
        {
            //Debug.Log("process effects");
            enemy.ProcessEffects();
            if (enemy.health <= 0)
            {
                // YOU WIN
                this.shop.gameObject.SetActive(true);
                Destroy(enemy.gameObject);
            }
        }
    }

    public void PlayEnemyCard(EnemyController enemy, Card card)
    {

        foreach (var cardEffect in card.effects.Where((effect) => effect.target == Target.Self))
        {
            switch (cardEffect.effect.name)
            {
                case "Heal":
                    enemy.health += cardEffect.amount;
                    break;
                case "Pierce":
                    enemy.pierce = true;
                    break;
                default:
                    enemy.activeEffects.Add(new ActiveEffect { effect = cardEffect.effect, amount = cardEffect.amount, timer = cardEffect.time });
                    break;
            }
        }

        foreach (var cardEffect in card.effects.Where((effect) => effect.target == Target.Enemy))
        {
            this.activeEffects.Add(new ActiveEffect { effect = cardEffect.effect, amount = cardEffect.amount, timer = cardEffect.time });
        }

        Debug.Log(card.physicalDamage + enemy.damageMod);
        Debug.Log(this.playerGuard.Value);
        int modDamage = (card.physicalDamage + enemy.damageMod) - (pierce ? 0 : this.playerGuard.Value);
        Debug.Log(modDamage);
        int totalDamage = Mathf.Clamp(modDamage, 0, modDamage);
        Debug.Log(totalDamage);
        this.playerHP.Value -= totalDamage;

        OnActionOnPlayer();
    }

    public void CodeProgram()
    {
        Debug.Log("code program");
        this.cardPool.gameObject.SetActive(true);
        // Will call CodedProgram
    }

    public void CodedProgram(Card card)
    {
        if (card != null)
        {
            this.playerInventory.deck.Add(card);
            this.playerXP++;
            if (this.playerXP == this.playerLevel)
            {
                this.playerLevel++;
                this.playerXP = 0;
            }
        }
        SwitchToTurn(Turn.Enemies);
    }

    private void SetUp()
    {
        this.cardPool.gameObject.SetActive(false);
        this.playerModules.modules.Clear();
        // Disable cards in editor
        for (int i = 0; i < this.cardLocation.transform.childCount; i++)
        {
            this.cardLocation.transform.GetChild(i).gameObject.SetActive(false);
        }

        this.playerGuard.Value = 0;
        this.playerLevel.Value = 1;
        this.playerXP.Value = 0;
        this.playerHP.Value = this.maxPlayerHP.Value;
        this.playerHackerTokens.Value = 0;
        this.playerInventory.deck.Clear();
        foreach (var card in this.startingPlayerInventory.deck) {
            this.playerInventory.deck.Add(card);
        }
    }

    private void ProcessActiveEffects()
    {
        this.playerGuard.Value = 0;
        this.playerDamageMod = 0;
        foreach (var activeEffect in this.activeEffects)
        {
            switch (activeEffect.effect.name)
            {
                case "DOT":
                    this.playerHP.Value -= activeEffect.amount;
                    break;
                case "HOT":
                    this.playerHP.Value += activeEffect.amount;
                    break;
                case "Guard":
                    this.playerGuard.Value += activeEffect.amount;
                    Debug.Log("setting guard");
                    Debug.Log(this.playerGuard.Value);
                    break;
                case "Strengthen":
                    this.playerDamageMod += activeEffect.amount;
                    break;
                case "Weaken":
                    this.playerDamageMod -= activeEffect.amount;
                    break;
                case "DOT+":
                    this.playerHP.Value -= activeEffect.amount;
                    activeEffect.amount++;
                    break;
                case "Stun":
                    this.stunned = true;
                    break;
            }
            activeEffect.timer--;
        }
        this.activeEffects.RemoveAll((effect) => effect.timer <= 0);
    }

    private void SwitchToTurn(Turn turn)
    {
        if (turn == Turn.Player)
        {
            ProcessActiveEffects();
            if (!this.stunned)
                DrawCards();
        } else
        {
            DiscardCards();
            ProcessEnemyEffects();
        }
        WhichTurn = turn;
    }

    private void DiscardCards()
    {
        for (int i = 0; i < this.cardLocation.transform.childCount; i++)
        {
            Destroy(this.cardLocation.transform.GetChild(i).gameObject);
        }
    }

    private void DrawCards() {
        this.shop.gameObject.SetActive(true);

        int pos = 0;
        float width = 3f;
        float padding = 0.25f;

        var copy = new List<Card>(this.playerInventory.deck);

        for (int i = 0; i < this.defaultHandSize; i++)
        {
            int rand = Random.Range(0, copy.Count);
            var cardToDraw = copy[rand];

            var cardCont = Instantiate(this.cardPrefab, this.cardLocation.transform);
            cardCont.card = cardToDraw;
            cardCont.GetComponentInChildren<Button>().onClick.AddListener(() => { cardCont.Activate(); });
            cardCont.gameObject.transform.Translate(new Vector3((width + padding) * pos, 0, 0));
            pos++;
        }

    }

}
