using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine.Windows;

public class FileUploader : MonoBehaviour
{
    // WebGL 平台的 JavaScript 函数声明
    #if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void uploadFile();
    #endif

    public void OnButtonClick()
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
        uploadFile();
        #else
        // 如果是桌面平台，使用 Unity 的 OpenFilePanel
        string path = UnityEditor.EditorUtility.OpenFilePanel("Select a CSV file", "", "csv");
        if (!string.IsNullOrEmpty(path))
        {
            string contents = System.IO.File.ReadAllText(path);
            string fileName = Path.GetFileName(path);
            OnFileUploaded(fileName + "|" + contents);
        }
        #endif
    }

    // 这个函数将在文件被选择并读取后被调用
    public void OnFileUploaded(string combinedString)
    {
        string[] parts = combinedString.Split('|');
        if (parts.Length >= 2)
        {
            string fileName = parts[0];
            string fileContents = parts[1];

            GetComponent<PlayerSettings>().Title = fileName;
            // 在这里处理文件名和文件内容
            string pattern = @"-s=(\d+)";
            Match match = Regex.Match(fileName, pattern);

            if (match.Success)
            {
                string numberString = match.Groups[1].Value;
                int number = int.Parse(numberString);
                GetComponent<PlayerSettings>().PlayerRemainSteps = number;
            } else
            {
                GetComponent<PlayerSettings>().PlayerRemainSteps = 6;
            }
            GetComponent<PlayerSettings>().ReloadGame(fileContents);
            SceneManager.LoadScene(1);
        }
    }

}
