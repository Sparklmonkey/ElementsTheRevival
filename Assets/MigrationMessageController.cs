using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MigrationMessageController : MonoBehaviour
{
    private void Awake()
    {
        if (PlayerPrefs.HasKey("SeenMigr"))
        {
            gameObject.SetActive(false);
        }

        PlayerPrefs.SetString("SeenMigr", "SeenMigr");
    }
}
