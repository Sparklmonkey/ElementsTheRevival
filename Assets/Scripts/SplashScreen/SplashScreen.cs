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

    private List<Vector3> finalPositions = new List<Vector3>();

    private List<GameObject> imageObjects = new List<GameObject>();

    private bool isLoadingNextScene = false, isNewGame = false;

    private IEnumerator MoveImageAroundToFinalPosition(GameObject imageToMove, int finalIndex, int imageIndex)
    {
        for (int i = 0; i < finalPositions.Count; i++)
        {
            while (imageToMove.transform.position != finalPositions[i])
            {
                imageToMove.transform.position = Vector3.MoveTowards(imageToMove.transform.position, finalPositions[i], 1500f * Time.deltaTime);
                yield return null;
            }
            if (i == 0)
            {
                if (imageIndex == 0)
                {
                    break;
                }
                StartCoroutine(MoveImageAroundToFinalPosition(imageObjects[imageIndex - 1], finalIndex - 1, imageIndex - 1));
            }
            if (finalIndex == i)
            {
                break;
            }
        }
    }

    private void Start()
    {
        if(PlayerPrefs.GetInt("HasKeysStored") != 1)
        {
            PlayerPrefs.SetInt("HasKeysStored", 1);

            PlayerPrefs.SetInt("QuickPlay", 1);
            PlayerPrefs.SetFloat("AnimSpeed", 0.2f);
            PlayerPrefs.SetFloat("BGMVolume", 100f);
            PlayerPrefs.SetFloat("SFXVolume", 100f);
        }

        PlayerPrefs.SetString("IsOnline", "False");
        Transform[] componentsInChildren = finalImagePositionsParent.GetComponentsInChildren<Transform>();
        foreach (Transform transform in componentsInChildren)
        {
            finalPositions.Add(transform.position);
        }
        finalPositions.RemoveAt(0);
        Image[] componentsInChildren2 = imageObjectsParent.GetComponentsInChildren<Image>();
        foreach (Image image in componentsInChildren2)
        {
            imageObjects.Add(image.gameObject);
        }
        StartCoroutine(MoveImageAroundToFinalPosition(imageObjects[11], finalPositions.Count - 1, 11));
        StartCoroutine(StartTitleAnimation());
    }

    public void SkipSplashAnimation()
    {
        if (PlayerPrefs.GetFloat("HasSeenSplash") == 1f && !isLoadingNextScene)
        {
        
            StopAllCoroutines();
            for (int i = 0; i < imageObjects.Count; i++)
            {
                imageObjects[i].transform.position = finalPositions[i];
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
            //shader.SetColor("_EdgeColour", ElementColours.GetRandomColour());
            yield return null;
        }
        shader.SetFloat("_Fade", 1f);
        StartCoroutine(LoadNextScene());
    }
}
