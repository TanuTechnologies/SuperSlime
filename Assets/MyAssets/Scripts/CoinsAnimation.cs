using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoinsAnimation : MonoBehaviour
{
    [SerializeField] private GameObject pileOfCoins;
    public int coinsAmount;
    public Transform targetPos;
    public Vector3 offset;
    private int coinsAmountToAdd;

    void OnEnable()
    {
        if (SceneManager.GetActiveScene().name == "Boss")
        {
            if (400 > ItemsManager.Instance.collectedItemsName.Count)
                coinsAmount = 400;
            else
                coinsAmount = ItemsManager.Instance.collectedItemsName.Count;
        }

        if (GameObject.Find("BonusLevel"))
        {
            if (1000 > ItemsManager.Instance.collectedItemsName.Count)
                coinsAmount = 1000;
            else
                coinsAmount = ItemsManager.Instance.collectedItemsName.Count;
        }

        CountCoins();
    }

    //public void SetCoinsAmount(int amount)
    //{
    //    coinsAmount = amount;
    //}

    public void CountCoins()
    {
        pileOfCoins.SetActive(true);
        var delay = 0f;

        for (int i = 0; i < pileOfCoins.transform.childCount; i++)
        {
            pileOfCoins.transform.GetChild(i).DOScale(1f, 0.3f).SetDelay(delay).SetEase(Ease.OutBack);

            pileOfCoins.transform.GetChild(i).GetComponent<RectTransform>().DOAnchorPos(targetPos.localPosition + offset, 0.8f)
                .SetDelay(delay + 0.5f).SetEase(Ease.InBack);

            pileOfCoins.transform.GetChild(i).DORotate(Vector3.zero, 0.5f).SetDelay(delay + 0.5f).SetEase(Ease.Flash);

            pileOfCoins.transform.GetChild(i).DOScale(0f, 0.3f).SetDelay(delay + 1.5f).SetEase(Ease.OutBack);

            delay += 0.1f;
        }

        Invoke("PlayCoinSound", .5f);
        InvokeRepeating("GiveCoins", .1f, .1f);
        Invoke("StopCoinAnimation", 2.0f); // Adjust the duration as needed
    }


    private void PlayCoinSound()
    {
        AudioManager.Instance.Play("Coins");
    }

    int count = 0;
    void GiveCoins()
    {
        count++;

        if (count == 25)
            CancelInvoke("GiveCoins");

        coinsAmountToAdd = coinsAmount / 25; // Divide total coins by the number of steps
        FindObjectOfType<CoinsManager>().AddMoney(coinsAmountToAdd);
    }

    void StopCoinAnimation()
    {
        CancelInvoke("GiveCoins");
    }
}
