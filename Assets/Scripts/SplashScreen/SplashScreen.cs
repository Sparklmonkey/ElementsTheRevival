using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{
    public Transform finalImagePositionsParent;

    public Transform imageObjectsParent;

    public Image titleImage;

    public Material dissolveMat;

    public Sprite titleSprite;

    [SerializeField]
    private List<Transform> finalPositions;

    [SerializeField]
    private List<GameObject> imageObjects;

    private bool isLoadingNextScene = false;

    [SerializeField]
    private GameObject appUpdatePopUp;
    private IEnumerator MoveImageAround(GameObject imageToMove, int finalIndex)
    {
        if(finalIndex == 0)
        {
            while (imageToMove.transform.position != finalPositions[0].position)
            {
                imageToMove.transform.position = Vector3.MoveTowards(imageToMove.transform.position, finalPositions[0].position, 1500f * Time.deltaTime);
                yield return null;
            }
            yield break;
        }
        for (int i = 0; i < finalPositions.Count; i++)
        {
            int whileLoopBreak = 0;
            while (imageToMove.transform.position != finalPositions[i].position && whileLoopBreak < 16)
            {
                imageToMove.transform.position = Vector3.MoveTowards(imageToMove.transform.position, finalPositions[i].position, 1500f * Time.deltaTime);
                whileLoopBreak++;
                if(whileLoopBreak == 16)
                {
                    imageToMove.transform.position = finalPositions[i].position;
                }
                yield return null;
            }
            whileLoopBreak = 0;
            if(i == 0 && finalIndex > 0)
            {
                StartCoroutine(MoveImageAround(imageObjects[finalIndex - 1], finalIndex - 1));
            }
            if(i == finalIndex)
            {
                yield break;
            }
        }
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("HasKeysStored") != 1)
        {
            PlayerPrefs.SetInt("HasKeysStored", 1);

            PlayerPrefs.SetInt("QuickPlay", 1);
            PlayerPrefs.SetFloat("AnimSpeed", 0.2f);
            PlayerPrefs.SetFloat("BGMVolume", 100f);
            PlayerPrefs.SetFloat("SFXVolume", 100f);
        }

        StartCoroutine(MoveImageAround(imageObjects[11], 12));
        StartCoroutine(StartTitleAnimation());
    }

    public void SkipSplashAnimation()
    {
        if (PlayerPrefs.GetFloat("HasSeenSplash") == 1f && !isLoadingNextScene)
        {
            StopAllCoroutines();
            for (int i = 0; i < imageObjects.Count; i++)
            {
                imageObjects[i].transform.position = finalPositions[i].position;
            }
            titleImage.material.SetFloat("_Fade", 1f);
            StartCoroutine(LoadNextScene());
        }
    }

    private IEnumerator LoadNextScene()
    {
        isLoadingNextScene = true;
        yield return new WaitForSeconds(1f);
        PlayerPrefs.SetFloat("HasSeenSplash", 1f);

        SceneManager.LoadScene("LoginScreen");
    }

    private IEnumerator StartTitleAnimation()
    {
        Material shader = titleImage.material;
        shader.SetTexture("_MainTex", titleSprite.texture);
        shader.SetFloat("_Fade", 0f);
        shader.SetFloat("_Scale", 150f);
        float currentTime = 0f;
        while (currentTime < 6f)
        {
            float value = currentTime / 6f;
            currentTime += Time.deltaTime;
            shader.SetFloat("_Fade", value);
            yield return null;
        }
        shader.SetFloat("_Fade", 1f);
        StartCoroutine(LoadNextScene());
    }
}