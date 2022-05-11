using Backtrace.Unity;
using Backtrace.Unity.Model;
using System;
using System.Collections.Generic;
using UnityEngine;

public class StartUpScript : MonoBehaviour
{
    // Backtrace client instance
    private BacktraceClient _backtraceClient;

    void Start()
    {
        var serverUrl = "https://submit.backtrace.io/elementrevival/fc7f9d87a2821103cc6c77b84bcbaffd9f935c2a17917194349fe526d4d1f233/json";
        var gameObjectName = "Backtrace";
        var databasePath = "${Application.persistentDataPath}/sample/backtrace/path";
        var attributes = new Dictionary<string, string>() { { "my-super-cool-attribute-name", "attribute-value" } };

        // use game object to initialize Backtrace integration
        _backtraceClient = GameObject.Find(gameObjectName).GetComponent<BacktraceClient>();
        //Read from manager BacktraceClient instance
        var database = GameObject.Find(gameObjectName).GetComponent<BacktraceDatabase>();

        // or initialize Backtrace integration directly in your source code
        _backtraceClient = BacktraceClient.Initialize(
                url: serverUrl,
                databasePath: databasePath,
                gameObjectName: gameObjectName,
                attributes: attributes);
    }
}