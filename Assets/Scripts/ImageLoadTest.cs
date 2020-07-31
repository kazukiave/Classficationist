using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageLoadTest : MonoBehaviour
{

    public Image image;

    private string distPath = @"/Users/kazuki/Desktop/ArchillectSelectMini";
    private List<string> imagePaths;
    private Texture2D imageTexture;
    private int heightMaxSize;
    private int widthMaxSize;

    // Start is called before the first frame update
    void Start()
    {
        heightMaxSize = (int)image.rectTransform.rect.height;
        widthMaxSize = (int)image.rectTransform.rect.width;

        imagePaths = new List<string>();
        imagePaths.AddRange(System.IO.Directory.GetFiles(distPath, "*.jpg"));
        imagePaths.AddRange(System.IO.Directory.GetFiles(distPath, "*.png"));
        Debug.Log("Image num = " + imagePaths.Count);

        string imagePath = imagePaths[0];
        byte[] imageByte = System.IO.File.ReadAllBytes(imagePath);

        imageTexture = new Texture2D(2, 2);
        imageTexture.LoadImage(imageByte);

        Debug.Log("widthMaxSize" + widthMaxSize+" heightMaxSize"+heightMaxSize);

        //change image size
       
        if(imageTexture.width > widthMaxSize)
        {
            Debug.Log("aaa");
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
        if(imageTexture.height > heightMaxSize)
        {
            Debug.Log("bbb");
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
        Debug.Log(image.rectTransform.rect.height + " " + image.rectTransform.rect.width);
    }

    // Update is called once per frame
    void Update()
    {
        
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
 
}
