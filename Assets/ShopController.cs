using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopController : MonoBehaviour
{
    public ModuleList masterModuleList;
    public ModuleList shopList;
    public ModuleList playerModules;
    public IntegerVariable playerHackerTokens;
    public UIModule modulePrefab;
    public GameObject moduleLocation;
    public TextMeshProUGUI tokensText;

    private void OnEnable()
    {
        this.tokensText.text = string.Format("Hacker Tokens: {0}", this.playerHackerTokens.Value);
        // Get modules not already owned by player
        var availableModules = this.masterModuleList.modules.Except(this.playerModules.modules);

        // Pick 1 module randomly for each price
        var groupedByPrice = availableModules.GroupBy((module) => module.price).OrderBy((group) => group.Key);
        this.shopList.modules.Clear();
        foreach (var group in groupedByPrice)
        {
            int price = group.Key;
            int idx = Random.Range(0, group.Count());
            this.shopList.modules.Add(group.ElementAt(idx));
        }

        int pos = 0;
        float width = 4f;
        float padding = 0.25f;
        float startingPos = ((width + padding) * (this.shopList.modules.Count() - 1)) / 2;
        Debug.Log(string.Join(", ", this.shopList.modules));
        foreach (var module in this.shopList.modules)
        {
            var uiModule = Instantiate(this.modulePrefab, this.moduleLocation.transform);
            uiModule.module = module;

            uiModule.gameObject.transform.Translate(new Vector3((width + padding) * pos - startingPos, 0, 0));

            var button = uiModule.gameObject.GetComponentInChildren<Button>();
            button.onClick.AddListener(() => { PurchaseModule(module); });
            pos++;
        }
    }

    public void Pass()
    {
        // Remove module game objects
        for (int child = 0; child < this.moduleLocation.transform.childCount; child++)
        {
            Destroy(this.moduleLocation.transform.GetChild(child).gameObject);
        }
        this.gameObject.SetActive(false);
        GameManager.Instance.StartNewRound();
    }

    private void PurchaseModule(Module module)
    {
        this.playerHackerTokens.Value -= module.price;
        this.playerModules.modules.Add(module);

        // Remove module game objects
        for (int child = 0; child < this.moduleLocation.transform.childCount; child++)
        {
            Destroy(this.moduleLocation.transform.GetChild(child).gameObject);
        }
        this.gameObject.SetActive(false);
        GameManager.Instance.StartNewRound();
    }
}
