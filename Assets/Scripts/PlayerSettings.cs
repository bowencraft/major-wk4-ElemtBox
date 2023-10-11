using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class PlayerSettings : MonoBehaviour
{
    public string Title;
    public string csvData;

    public int PlayerRemainSteps = 6;

    [Header("=== Data ===")]
    public TextAsset csvFile; // 指定的.csv文件

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        csvData = csvFile.text;
        Title = csvFile.name;
    }

    public void CopyText(string csvData)
    {
        GUIUtility.systemCopyBuffer = csvData;
        Debug.Log("Copied to clipboard!");
    }

    public void ReloadGame(string _text)
    {
        csvData = _text;
        //Debug.Log("Back to settings!");
        //SceneManager.LoadScene(1);

    }

}
