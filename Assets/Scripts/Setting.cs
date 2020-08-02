using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using SFB;
using TMPro;

public class Setting : MonoBehaviour
{
    [Header("Auto Setting")]
    public bool autoSetting;

    [Header("UI")]
    public GameObject srcInput;
    public GameObject destInput;
    public GameObject mainController;
    public GameObject mainCanvas;
    public GameObject settingCanvas;
    public List<GameObject> categoryInputs;
    public GameObject shuffleToggle;

    [System.NonSerialized]
    public List<string> categoryNames;
    [System.NonSerialized]
    public string srcPath;
    [System.NonSerialized]
    public string destPath;
    [System.NonSerialized]
    public bool shuffleLoad;

    // Start is called before the first frame update
    void Start()
    {
        categoryNames = new List<string>();

#if UNITY_EDITOR
        if (autoSetting)
        {
            srcPath = "/Users/kazuki/Desktop/srcFolder";
            destPath = "/Users/kazuki/Desktop/destFolder";
            if (!Directory.Exists(srcPath))
            {
                Directory.CreateDirectory(srcPath);
            }
            if (!Directory.Exists(destPath))
            {
                Directory.CreateDirectory(destPath);
            }
            string samplePath = Path.Combine(Application.dataPath, "SamplePictures");
            var sampleDirs = Directory.GetFiles(samplePath);

            //copy sample
            for (int i = 0; i < sampleDirs.Length; i++)
            {
                File.Copy(sampleDirs[i], Path.Combine(srcPath, Path.GetFileName(sampleDirs[i])), true);
            }

            categoryNames.Add("a");
            categoryNames.Add("b");
            categoryNames.Add("c");

            mainCanvas.SetActive(true);
            settingCanvas.SetActive(false);
            mainController.GetComponent<Classfication>().Init();
        }
#endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectSource()
    {
        srcPath = StandaloneFileBrowser.OpenFolderPanel("Select Folder", "", true)[0];
        srcInput.GetComponentInChildren<TextMeshProUGUI>().text = srcPath;
    }

    public void SelectDest()
    {
        destPath = StandaloneFileBrowser.OpenFolderPanel("Select Folder", "", true)[0];
        destInput.GetComponentInChildren<TextMeshProUGUI>().text = destPath;
    }

    public void StartMain()
    {
        for (int i = 0; i < categoryInputs.Count; i++)
        {
            string txt = categoryInputs[i].GetComponent<TMP_InputField>().text;
            if (txt != "")
            {
                categoryNames.Add(txt);
            }
        }

        Debug.Log("src= " + srcPath);
        Debug.Log("dist= " + destPath);
        foreach (var category in categoryNames)
        {
            Debug.Log(category);
        }

        shuffleLoad = shuffleToggle.GetComponent<Toggle>().isOn;

        mainCanvas.SetActive(true);
        settingCanvas.SetActive(false);
        mainController.GetComponent<Classfication>().Init();
    }
}
