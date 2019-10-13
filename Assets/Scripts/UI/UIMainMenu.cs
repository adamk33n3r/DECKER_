using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIMainMenu : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public GameObject cover;

    private void Start()
    {
        StartCoroutine(CO_TitleText(0.3f));
    }

    public void StartGame()
    {
        BGMusic.Instance.SetVolume(0.5f);
        SceneManager.LoadScene("Game");
    }

    public IEnumerator CO_TitleText(float timeBetween)
    {
        this.titleText.text = "_";
        yield return new WaitForSeconds(timeBetween);
        this.titleText.text = "D_";
        yield return new WaitForSeconds(timeBetween);
        this.titleText.text = "DE_";
        yield return new WaitForSeconds(timeBetween);
        this.titleText.text = "DEC_";
        yield return new WaitForSeconds(timeBetween);
        this.titleText.text = "DECK_";
        yield return new WaitForSeconds(timeBetween);
        this.titleText.text = "DECKE_";
        yield return new WaitForSeconds(timeBetween);
        this.titleText.text = "DECKER_";

        StartCoroutine(CO_FlashCursor(1));
    }

    public IEnumerator CO_FlashCursor(float timeBetween)
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetween);
            this.cover.SetActive(true);
            yield return new WaitForSeconds(timeBetween);
            this.cover.SetActive(false);
        }
    }
}
