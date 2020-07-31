using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UITest : MonoBehaviour
{
    public int folderNum = 3;

    public List<GameObject> buttons;
    public List<string> categorys;
    // Start is called before the first frame update
    void Start()
    {
      for(int i = 0; i < buttons.Count; i++)
        {
            if(i < folderNum)
            {
                buttons[i].SetActive(true);

                var button = buttons[i].GetComponent<Button>();
                int num = i;
                button.onClick.AddListener(() => CopyImage(num));

                var textMesh = buttons[i].GetComponentInChildren<TextMeshProUGUI>();
                textMesh.text = categorys[i];
            }
            else
            {
                buttons[i].SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CopyImage(int num)
    {
        Debug.Log(num);
    }
}
