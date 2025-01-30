using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectsDetector : MonoBehaviour
{
    Bounds playerColliderSize;
    private float itemsCount = 0;
    private int maxCount = 50;
    public float scaleSize;
    public Animator playerAnim, sizeUpAnim;
    public ParticleSystem sizeUpfx;
    public TextMeshProUGUI eatItemsCouTxt, maxItemsCouText;
    public Image fillImg;
    public Transform itemsCountBox, scaleObjTra;
    private Camera cam;
    public GameManager gameManager;
    Vector3 playerScale;

    private void Start()
    {
        cam = Camera.main;
        SetMaxItemsCountText();

        if (PlayerPrefs.HasKey("Size"))
        {
            float playerScaleVal = PlayerPrefs.GetFloat("Size");

            Vector3 scale = new Vector3(playerScaleVal, playerScaleVal, playerScaleVal);
            print(scale + " Scale");
            scaleObjTra.localScale = scale;
        }
    }

    private void LateUpdate()
    {
        itemsCountBox.LookAt(itemsCountBox.position + cam.transform.forward);
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(Eat(other));
    }

    IEnumerator Eat(Collider other)
    {
        playerColliderSize = GetComponent<Collider>().bounds;

        // Get the size of the collider
        Bounds otherColliderSize = other.bounds;

        float playerSize = playerColliderSize.size.magnitude;
        float itemSize = otherColliderSize.size.magnitude;

        // Compare the sizes
        if (playerSize > itemSize && other.transform.parent)
        {
            if (!gameManager.gameStarted)
            {
                Destroy(other.transform.parent.gameObject);
                yield break;
            }

            float count = itemSize * 100;

            if (count > 6)
                count = 6;

            itemsCount += count;
            UpdateHealthBar();
            SetEatItemsCountText();
            StartCoroutine(CheckScalePlayerSize());

            playerAnim.Play("Eat");

            if (!AudioManager.Instance.audioSource.isPlaying)
                AudioManager.Instance.Play("Eat");

            ItemsManager.Instance.collectedItemsName.Add(other.transform.parent.gameObject.name);
            Destroy(other.transform.parent.gameObject);
        }

        yield return null;
    }

    IEnumerator CheckScalePlayerSize()
    {
        if (itemsCount > maxCount)
        {
            itemsCount = 0;
            SetEatItemsCountText();


            if (gameManager.currentLevelno < 4)
            {
                maxCount += 400;

                if (scaleObjTra.localScale.x < 4)
                    scaleSize = .5f;
                else if (scaleObjTra.localScale.x < 15)
                    scaleSize = 1;
                else if (scaleObjTra.localScale.x < 35)
                    scaleSize = 1.5f;
                else if (scaleObjTra.localScale.x < 50)
                    scaleSize = 2;
                else if (scaleObjTra.localScale.x < 100)
                    scaleSize = 2.5f;
                else if (scaleObjTra.localScale.x > 100)
                    scaleSize = 3;
            }
            else
            {
                maxCount += 200;

                if (scaleObjTra.localScale.x < 4)
                    scaleSize = 2;
                else if (scaleObjTra.localScale.x < 15)
                    scaleSize = 3;
                else if (scaleObjTra.localScale.x < 35)
                    scaleSize = 4;
                else if (scaleObjTra.localScale.x < 50)
                    scaleSize = 5;
                else if (scaleObjTra.localScale.x < 100)
                    scaleSize = 6;
                else if (scaleObjTra.localScale.x > 100)
                    scaleSize = 7;
            }

            if (GameObject.Find("BonusLevel"))
                maxCount += 200;

            SetMaxItemsCountText();
            StartCoroutine(SizeUp(true, scaleSize));
        }

        yield return null;
    }

    private void UpdateHealthBar()
    {
        float fillAmount = (float)itemsCount / maxCount;
        fillImg.fillAmount = fillAmount;
    }

    private void SetEatItemsCountText()
    {
        eatItemsCouTxt.text = Mathf.RoundToInt(itemsCount).ToString();
    }

    private void SetMaxItemsCountText()
    {
        maxItemsCouText.text = maxCount.ToString();
    }

    public IEnumerator SizeUp(bool playSizeAnim, float _ScaleSize)
    {
        playerScale = scaleObjTra.localScale;

        playerScale.x += _ScaleSize;
        playerScale.y += _ScaleSize;
        playerScale.z += _ScaleSize;

        scaleObjTra.localScale = playerScale;

        if(playSizeAnim)
            PlaySizeUpAnim();

        yield return null;
    }

    public void SaveSize()
    {
        PlayerPrefs.SetFloat("Size", playerScale.x);
    }

    private void PlaySizeUpAnim()
    {
        sizeUpAnim.GetComponent<AudioSource>().Play();
        sizeUpAnim.Play("SizeUp");
        sizeUpfx.Play();
    }
}