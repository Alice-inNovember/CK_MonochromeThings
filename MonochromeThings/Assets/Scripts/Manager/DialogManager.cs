using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DialogManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI AnswerContext;
    [SerializeField] private List<DialogButton> Questions;

    public List<DialogData> dialogs;
    public List<int> clearedEvents;

    public static DialogManager instance;

    private  void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

       
        clearedEvents = new List<int>();
        clearedEvents.Add(0);



        
    }

    private void Start()
    {
        if (Questions.Count == 0)
        {


            DialogButton[] buttons = GameObject.FindObjectsByType<DialogButton>(FindObjectsSortMode.None);


            foreach (var item in buttons)
            {
                Questions.Add(item);
            }
        }

        ShowQuestions();
    }

    public void ShowQuestions()
    {
        for (int i = 0; i < Questions.Count; i++)
        {
            if (i < dialogs.Count && clearedEvents.Contains(dialogs[i].eventId))
            {
                Questions[i].SetActive(true);
                Questions[i].SetUI(dialogs[i]);
            }
            else
                Questions[i].SetActive(false);
           
        }


    }

    public void ShowAnswer(int dialogID)
    {
        foreach (var item in dialogs)
        {
            if (item.dialogId == dialogID)
            {
                AnswerContext.text = item.answer;
                clearedEvents.Add((int)dialogID);
                
                //dialogs.Remove(item);

                ShowQuestions();
                
            }
        }
        
    }



}

[CreateAssetMenu(menuName = "ScriptableObjects/DialogTree",fileName = "DialogTree_")]
public class DialogTree : ScriptableObject
{
    
}

