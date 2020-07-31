using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SFB;
using TMPro;

public class Setting : MonoBehaviour
{
    public GameObject srcInput;
    public GameObject destInput;
    public GameObject mainController;
    public GameObject mainCanvas;
    public GameObject settingCanvas;
    public List<GameObject> categoryInputs;

    [System.NonSerialized]
    public List<string> categoryNames;
    [System.NonSerialized]
    public string srcPath;
    [System.NonSerialized]
    public string destPath;

    // Start is called before the first frame update
    void Start()
    {
        categoryNames = new List<string>();
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

        mainCanvas.SetActive(true);
        settingCanvas.SetActive(false);
        mainController.GetComponent<Classfication>().Init();
    }
}
