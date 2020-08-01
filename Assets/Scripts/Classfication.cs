using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Classfication : MonoBehaviour
{
    [Header("set")]
    public GameObject settingObject;
    public Image image;
    private Setting setting;
    public TextMeshProUGUI currentCount;
    public TextMeshProUGUI totalCount;
    public TextMeshProUGUI minuteCount;
    public TextMeshProUGUI secondsCount;

    private string srcPath;
    private string destPath;
    private List<string> imagePaths;
    private Texture2D imageTexture;
    private int heightMaxSize;
    private int widthMaxSize;

    private bool isInit = false;
    private int count = 0;

    [Header("UI")]
    public List<GameObject> buttons;
    public GameObject mainCanvas;
    public GameObject finishCanvas;
    private List<string> categorys;

    private float elapsedTime = 0;
    private int minute = 0;


    enum State
    {
        Setting,
        Selecting,
        Selected,
        Finish
    }

    State currentState;
    private void Start()
    {
        currentState = State.Setting;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.Application.Quit();
        }

        if (currentState == State.Selected || currentState == State.Selecting)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= 60f)
            {
                minute++;
                elapsedTime -= 60f;
                minuteCount.GetComponentInChildren<TextMeshProUGUI>().text = minute.ToString() + "m";
            }
            secondsCount.GetComponentInChildren<TextMeshProUGUI>().text = Mathf.FloorToInt(elapsedTime).ToString() + "s";
        }

        switch (currentState)
        {
            case State.Setting:
                return;

            case State.Selecting:
                break;

            case State.Selected:
                count++;
                if (count == imagePaths.Count)
                {
                    Debug.Log("Finish");
                    currentState = State.Finish;
                    return;
                }
                currentCount.text = count.ToString();

                ShowImage();
                currentState = State.Selecting;
                break;

            case State.Finish:
                mainCanvas.SetActive(false);
                finishCanvas.SetActive(true);
                if(Input.GetKeyDown(KeyCode.Space))
                {
                    UnityEngine.Application.Quit();  
                }
                break;
        }
    }

    public void Init()
    {
        Debug.Log("Init Classfication");

        heightMaxSize = (int)image.rectTransform.rect.height;
        widthMaxSize = (int)image.rectTransform.rect.width;

        //copy field value
        setting = settingObject.GetComponent<Setting>();
        srcPath = setting.srcPath;
        destPath = setting.destPath;
        categorys = setting.categoryNames;

        var key = new float[buttons.Count];

        //set ui active or inactive
        for (int i = 0; i < buttons.Count; i++)
        {
            key[i] = buttons[i].transform.position.x;
            if (i < categorys.Count)
            {
                buttons[i].SetActive(true);

            }
            else
            {
                buttons[i].SetActive(false);
            }
        }

        //sort ui button
        var buttonsArr = buttons.ToArray();
        System.Array.Sort(key, buttonsArr);
        buttons = buttonsArr.ToList();

        //change button name
        int idx = 0;
        for (int i = 0; i < buttons.Count; i++)
        {
            if (buttons[i].activeSelf)
            {
                //add listenr
                var button = buttons[i].GetComponent<Button>();
                string fileName = categorys[idx];
                idx++;
                button.onClick.AddListener(() => CopyImage(fileName));

                //text
                var textMesh = buttons[i].GetComponentInChildren<TextMeshProUGUI>();
                textMesh.text = fileName;

                //create dire
                MakeDirectory(Path.Combine(destPath, fileName));
            }
        }

        //make dest folder


        //set src image paths
        imagePaths = new List<string>();
        imagePaths.AddRange(Directory.GetFiles(srcPath, "*.jpg"));
        imagePaths.AddRange(Directory.GetFiles(srcPath, "*.png"));
        Debug.Log("Image num = " + imagePaths.Count);
        totalCount.text = imagePaths.Count.ToString();

        ShowImage();
        currentState = State.Selecting;
    }

    private void ShowImage()
    {
        string imagePath = imagePaths[count];
        Debug.Log(imagePath);
        byte[] imageByte = System.IO.File.ReadAllBytes(imagePath);

        imageTexture = new Texture2D(2, 2);
        imageTexture.LoadImage(imageByte);

        if (imageTexture.width > widthMaxSize)
        {
            Vector2 rectSize = ResizeImageWidth();
            if (rectSize.y > heightMaxSize)
            {
                image.rectTransform.sizeDelta = ResizeImageHeight();
            }
            else
            {
                image.rectTransform.sizeDelta = rectSize;
            }
        }
        if (imageTexture.height > heightMaxSize)
        {
            Vector2 rectSize = ResizeImageHeight();
            if (rectSize.x > widthMaxSize)
            {
                image.rectTransform.sizeDelta = ResizeImageWidth();
            }
            else
            {
                image.rectTransform.sizeDelta = rectSize;
            }
        }


        Sprite imageSprit = Sprite.Create(imageTexture, new Rect(0f, 0f, imageTexture.width, imageTexture.height), new Vector2(0.5f, 0.5f));
        image.sprite = imageSprit;
    }

    private Vector2 ResizeImageWidth()
    {
        int imageWidth = widthMaxSize;
        int imageHeight = (imageTexture.height * imageWidth) / imageTexture.width;
        return new Vector2(imageWidth, imageHeight);
    }
    private Vector2 ResizeImageHeight()
    {
        int imageHeight = heightMaxSize;
        int imageWidth = (imageTexture.width * imageHeight) / imageTexture.height;
        return new Vector2(imageWidth, imageHeight);
    }

    void CopyImage(string dirName)
    {
        string dir = Path.Combine(destPath, dirName);
        string fineName = Path.GetFileName(imagePaths[count]);

        File.Copy(imagePaths[count], Path.Combine(dir, fineName), true);
        currentState = State.Selected;
    }

    private void MakeDirectory(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
}
