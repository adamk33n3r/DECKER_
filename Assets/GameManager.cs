using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public IntegerVariable startingPlayerLevel;
    public IntegerVariable playerXP;
    public IntegerVariable playerDamageMod;
    public IntegerVariable playerHackerTokens;
    public IntegerVariable defaultHandSize;

    public GameObject cardLocation;
    public CardController cardPrefab;
    public CardPoolController cardPool;

    public LogController logController;

    public ShopController shop;

    public ModuleList playerModules;

    public List<ActiveEffect> activeEffects = new List<ActiveEffect>();
    public List<Enemy> enemies;
    public EnemyController enemyPrefab;

    public Turn WhichTurn = Turn.Player;

    private bool pierce = false;
    private bool stunned = false;
    //private bool playerIsFocused = false;
    private int cardsPlayedThisTurn = 0;
    private int winCount = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetUp();
        StartNewRound();
        DrawCards();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            Application.Quit();
    }

    public void Log(string text)
    {
        this.logController.Log(text);
    }

    public void ProcessCard(Card card, EnemyController enemy)
    {
        if (WhichTurn == Turn.Enemies)
            return;

        // Self
        Debug.Log("Player playing card: " + card);
        this.cardsPlayedThisTurn++;

        this.pierce = false;
        foreach (var cardEffect in card.effects.Where((effect) => effect.target == Target.Self))
        {
            switch (cardEffect.effect.name)
            {
                case "Heal":
                    this.playerHP.Value += cardEffect.amount;
                    if (this.playerHP.Value > this.maxPlayerHP.Value)
                        this.playerHP.Value = this.maxPlayerHP.Value;
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
                    int amount = cardEffect.amount;
                    bool hasTrojan = this.playerModules.modules.Any((module) => module.name == "Trojan");
                    if (hasTrojan)
                        amount++;
                    this.activeEffects.Add(new ActiveEffect { effect = cardEffect.effect, amount = amount, timer = cardEffect.time });
                    break;
            }
        }

        // Enemy

        if (enemy == null)
        {
            enemy = FindObjectOfType<EnemyController>();
        }
        enemy.AddEffects(card.effects.Where((effect) => effect.target == Target.Enemy));

        int totalDamage = card.physicalDamage + this.playerDamageMod;
        bool hasHalfAdder = this.playerModules.modules.Any((module) => module.name == "Half Adder");
        if (hasHalfAdder)
            totalDamage++;

        if (card.physicalDamage != 0)
            enemy.TakeDamage(totalDamage, this.pierce);

        if (this.cardsPlayedThisTurn == 1)
        {
            bool hasFloppyDrive = this.playerModules.modules.Any((module) => module.name == "Floppy Drive");
            if (hasFloppyDrive)
                return;
        }

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
                Destroy(enemy.gameObject);
                WinGame();
            }
        }
    }

    public void PlayEnemyCard(EnemyController enemy, Card card)
    {

        Debug.Log("enemy is playing card");
        this.logController.Log(string.Format("{0}   played   {1}", enemy.name, card.name));

        foreach (var cardEffect in card.effects.Where((effect) => effect.target == Target.Self))
        {
            switch (cardEffect.effect.name)
            {
                case "Heal":
                    enemy.health += cardEffect.amount;
                    if (enemy.health > enemy.enemyData.startingHealth)
                        enemy.health = enemy.enemyData.startingHealth;
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
        int guardModule = this.playerModules.modules.Any((module) => module.name == "Hard Target") ? 1 : 0;
        int guard = pierce ? 0 : (this.playerGuard.Value + guardModule);
        int modDamage = (card.physicalDamage + enemy.damageMod) - guard;
        Debug.Log(modDamage);
        int totalDamage = Mathf.Clamp(modDamage, 0, modDamage);
        Debug.Log(totalDamage);
        this.playerHP.Value -= totalDamage;

        if (this.playerHP.Value <= 0)
        {
            LoseGame();
        }

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
        this.playerHackerTokens.Value = 0;
        // Disable cards in editor
        for (int i = 0; i < this.cardLocation.transform.childCount; i++)
        {
            this.cardLocation.transform.GetChild(i).gameObject.SetActive(false);
        }

    }

    private void ProcessActiveEffects()
    {
        this.playerGuard.Value = 0;
        this.playerDamageMod = 0;
        bool hasPassiveScanner = this.playerModules.modules.Any((module) => module.name == "Passive Scanner");
        if (hasPassiveScanner)
            this.playerHP.Value++;
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
            this.cardsPlayedThisTurn = 0;
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
        int pos = 0;
        float width = 3f;
        float padding = 0.25f;

        var copy = new List<Card>(this.playerInventory.deck);

        int handSize = this.defaultHandSize;
        bool hasGripGloves = this.playerModules.modules.Any((module) => module.name == "Grip Gloves");
        if (hasGripGloves)
            handSize++;

        for (int i = 0; i < handSize; i++)
        {
            if (copy.Count == 0)
                break;
            int rand = Random.Range(0, copy.Count);
            var cardToDraw = copy[rand];
            copy.RemoveAt(rand);

            var cardCont = Instantiate(this.cardPrefab, this.cardLocation.transform);
            cardCont.card = cardToDraw;
            cardCont.GetComponentInChildren<Button>().onClick.AddListener(() => { cardCont.Activate(); });
            cardCont.gameObject.transform.Translate(new Vector3((width + padding) * pos, 0, 0));
            pos++;
        }

    }

    public void StartNewRound()
    {
        var newEnemy = this.enemies[this.winCount];
        var enemy = Instantiate(this.enemyPrefab);
        enemy.enemyData = newEnemy;

        this.playerGuard.Value = 0;
        this.playerLevel.Value = this.startingPlayerLevel.Value;
        this.playerXP.Value = 0;
        this.playerHP.Value = this.maxPlayerHP.Value;
        this.playerInventory.deck.Clear();
        foreach (var card in this.startingPlayerInventory.deck) {
            this.playerInventory.deck.Add(card);
        }

        WhichTurn = Turn.Player;
    }

    private void WinGame() {
        this.winCount++;
        if (this.winCount == 5)
        {
            SceneManager.LoadScene("Win");
        }
        this.playerHackerTokens++;
        this.shop.gameObject.SetActive(true);
    }

    private void LoseGame() {
        SceneManager.LoadScene("Lose");
    }

}
