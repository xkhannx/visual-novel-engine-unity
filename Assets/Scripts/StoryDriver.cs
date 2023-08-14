using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using XNode;

public class StoryDriver : MonoBehaviour
{
    public ConversationsSO convos;
    StoryGraph curConvo;
    [Header("References")]
    public Image background;
    public Image portrait;
    public Text speakerName;
    public Text dialogText;
    public Text statusUpdateText;
    [Header("Windows")]
    public RectTransform dialogBubble;
    public RectTransform dialogShiftedUp;
    public RectTransform dialogAuthor;
    [Header("Buttons")]
    public Button nextButton;
    public Button[] choiceButtons;

    Vector2 dialogPos;
    Vector2 authorPos;
    Vector2 authorSize;
    Vector2 dialogSize;
    Vector2 multipleChoicePos;

    string statUpdates = "";
    Coroutine statUpdatePopupCor;
    int curConvoIndex = 0;
    void Start()
    {
        dialogPos = dialogBubble.anchoredPosition;
        dialogSize = dialogBubble.sizeDelta;

        authorPos = dialogAuthor.anchoredPosition;
        authorSize = dialogAuthor.sizeDelta;

        multipleChoicePos = dialogShiftedUp.anchoredPosition;

        InitStory();
    }

    private void InitStory()
    {
        curConvo = convos.conversations[curConvoIndex];

        StoryStartNode startingNode = null;
        foreach (Node node in curConvo.nodes)
        {
            if (node.GetType() == typeof(StoryStartNode))
            {
                startingNode = node as StoryStartNode;
                break;
            }
        }

        if (startingNode != null)
        {
            background.sprite = startingNode.background;
        }

        curConvo.current = startingNode.GetOutputPort("exit").Connection.node;
        ShowDialog();
    }

    void ShowDialog()
    {
        if (curConvo.current.GetType() == typeof(StoryEndNode))
        {
            nextButton.gameObject.SetActive(false);
            curConvoIndex++;
            if (curConvoIndex < convos.conversations.Count && convos.conversations[curConvoIndex] != null)
            {
                InitStory();
            }
            return;
        }


        if (curConvo.current.GetType() == typeof(DialogNode))
        {
            NODE_RegularDialog();
            return;
        }

        if (curConvo.current.GetType() == typeof(UpdateStatNode))
        {
            NODE_UpdateStats();
            return;
        }

        if (curConvo.current.GetType() == typeof(CheckStatNode))
        {
            NODE_CheckStats();
            return;
        }

        if (curConvo.current.GetType() == typeof(UpdateMarkerNode))
        {
            NODE_UpdateMarker();
            return;
        }

        if (curConvo.current.GetType() == typeof(CheckMarkerNode))
        {
            NODE_CheckMarker();
            return;
        }
    }

    private void NODE_CheckMarker()
    {
        CheckMarkerNode node = (CheckMarkerNode)curConvo.current;

        if (node.marker.received)
        {
            curConvo.current = node.GetOutputPort("happened").Connection.node;
        }
        else
        {
            curConvo.current = node.GetOutputPort("didNotHappen").Connection.node;
        }

        ShowDialog();
    }

    private void NODE_UpdateMarker()
    {
        UpdateMarkerNode node = (UpdateMarkerNode)curConvo.current;
        node.marker.received = true;

        curConvo.current = curConvo.current.GetOutputPort("exit").Connection.node;
        ShowDialog();
    }

    private void NODE_CheckStats()
    {
        CheckStatNode node = (CheckStatNode)curConvo.current;

        if (node.stat.value >= node.threshold)
        {
            curConvo.current = node.GetOutputPort("greaterOrEqual").Connection.node;
        } else
        {
            curConvo.current = node.GetOutputPort("less").Connection.node;
        }

        ShowDialog();
    }

    private void NODE_UpdateStats()
    {
        UpdateStatNode node = (UpdateStatNode)curConvo.current;
        node.stat.value += node.change;
        if (statUpdates != "")
        {
            statUpdates += "\n";
        }
        statUpdates += node.stat.statName + ": " + node.change.ToString();

        curConvo.current = curConvo.current.GetOutputPort("exit").Connection.node;
        ShowDialog();
    }

    private void NODE_RegularDialog()
    {
        DialogNode node = (DialogNode)curConvo.current;
        dialogText.text = node.line;

        if (node.character != null)
        {
            portrait.sprite = node.character.portrait;
            speakerName.text = node.character.characterName;
        }

        UpdateDialogPosition();

        ShowStatUpdates();
    }

    private void ShowStatUpdates()
    {
        if (statUpdates != "")
        {
            if (statUpdatePopupCor != null)
            {
                StopCoroutine(statUpdatePopupCor);
            }

            statusUpdateText.text = statUpdates;
            statUpdates = "";
            statUpdatePopupCor = StartCoroutine(BlinkStatUpdate());
        }
    }

    void UpdateDialogPosition()
    {
        DialogNode node = (DialogNode)curConvo.current;

        if (node.answers.Length > 0)
        {
            dialogBubble.anchoredPosition = multipleChoicePos;
            dialogBubble.sizeDelta = dialogSize;

            nextButton.gameObject.SetActive(false);
            speakerName.transform.parent.gameObject.SetActive(true);
            portrait.gameObject.SetActive(true);

            PrepareButton(0, node.answers[0].text);
            PrepareButton(1, node.answers[1].text);
            choiceButtons[2].gameObject.SetActive(false);
            if (node.answers.Length > 2)
            {
                PrepareButton(2, node.answers[2].text);
            }
        }
        else
        {
            dialogBubble.anchoredPosition = node.character != null ? dialogPos : authorPos;
            dialogBubble.sizeDelta = node.character != null ? dialogSize : authorSize;

            nextButton.gameObject.SetActive(true);
            speakerName.transform.parent.gameObject.SetActive(node.character != null);
            portrait.gameObject.SetActive(node.character != null);

            foreach (var but in choiceButtons)
            {
                but.gameObject.SetActive(false);
            }
        }
    }

    void PrepareButton(int _num, string _text)
    {
        choiceButtons[_num].gameObject.SetActive(true);
        choiceButtons[_num].GetComponentInChildren<Text>().text = _text;
    }

    // Button functions
    public void Next()
    {
        if (curConvo.current.GetType() == typeof(StoryEndNode))
        {
            return;
        }

        curConvo.current = curConvo.current.GetOutputPort("exit").Connection.node;
        ShowDialog();
    }

    public void MakeChoice(int _choice)
    {
        Node node = curConvo.current.GetPort("answers " + _choice.ToString()).Connection.node;
        Debug.Log("you selected " + node.name);
        curConvo.current = node;
        ShowDialog();
    }

    // Coroutines
    IEnumerator BlinkStatUpdate()
    {
        statusUpdateText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        statusUpdateText.gameObject.SetActive(false);
    }
}
