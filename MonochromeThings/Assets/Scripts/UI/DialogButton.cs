using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogButton : MonoBehaviour 
{
    [SerializeField] private TextMeshProUGUI textUI;
    [SerializeField] int dialogId;

    private void Awake()
    {
        textUI = GetComponentInChildren<TextMeshProUGUI>();
        Button button = GetComponent<Button>();
        button.onClick.AddListener(this.PushButton);

        Init();
    }

    private void Init()
    {
        dialogId = 0;
        textUI.text = "";
    }

    public void SetUI(DialogData data)
    {
        if (data == null)
        {
            return;
        }
        textUI.text = data.question;
        dialogId = data.dialogId;
    }

    public void SetActive(bool value)
    {
        this.gameObject.SetActive(value);
    }

    public void PushButton()
    {
        DialogManager.instance.ShowAnswer(this.dialogId);
    }
    
}
