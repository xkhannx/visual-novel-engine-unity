using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using XNode;

public class StoryDriver : MonoBehaviour
{
    public StoryGraph graph;
    [Header("References")]
    public Image background;
    public Image portrait;
    public Text speakerName;
    public Text dialogText;
    public Text statusUpdateText;
    [Header("Windows")]
    public RectTransform dialogBubble;
    public RectTransform dialogShiftedUp;
    [Header("Buttons")]
    public Button nextButton;
    public Button[] choiceButtons;

    Vector2 dialogPos;
    Vector2 multipleChoicePos;

    string statUpdates = "";
    Coroutine statUpdatePopupCor;
    void Start()
    {
        dialogPos = dialogBubble.anchoredPosition;
        multipleChoicePos = dialogShiftedUp.anchoredPosition;

        StoryStartNode startingNode = null;
        foreach (Node node in graph.nodes)
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

        graph.current = startingNode.GetOutputPort("exit").Connection.node;
        ShowDialog();
    }

    void ShowDialog()
    {
        if (graph.current.GetType() == typeof(StoryEndNode))
        {
            nextButton.gameObject.SetActive(false);
            return;
        }

        if (graph.current.GetType() == typeof(DialogNode))
        {
            NODE_RegularDialog();
            return;
        }

        if (graph.current.GetType() == typeof(UpdateStatNode))
        {
            NODE_UpdateStats();
            return;
        }

        if (graph.current.GetType() == typeof(CheckStatNode))
        {
            NODE_CheckStats();
            return;
        }

        if (graph.current.GetType() == typeof(UpdateMarkerNode))
        {
            NODE_UpdateMarker();
            return;
        }

        if (graph.current.GetType() == typeof(CheckMarkerNode))
        {
            NODE_CheckMarker();
            return;
        }
    }

    private void NODE_CheckMarker()
    {
        CheckMarkerNode node = (CheckMarkerNode)graph.current;

        if (node.marker.received)
        {
            graph.current = node.GetOutputPort("happened").Connection.node;
        }
        else
        {
            graph.current = node.GetOutputPort("didNotHappen").Connection.node;
        }

        ShowDialog();
    }

    private void NODE_UpdateMarker()
    {
        UpdateMarkerNode node = (UpdateMarkerNode)graph.current;
        node.marker.received = true;

        graph.current = graph.current.GetOutputPort("exit").Connection.node;
        ShowDialog();
    }

    private void NODE_CheckStats()
    {
        CheckStatNode node = (CheckStatNode)graph.current;

        if (node.stat.value >= node.threshold)
        {
            graph.current = node.GetOutputPort("greaterOrEqual").Connection.node;
        } else
        {
            graph.current = node.GetOutputPort("less").Connection.node;
        }

        ShowDialog();
    }

    private void NODE_UpdateStats()
    {
        UpdateStatNode node = (UpdateStatNode)graph.current;
        node.stat.value += node.change;
        if (statUpdates != "")
        {
            statUpdates += "\n";
        }
        statUpdates += node.stat.statName + ": " + node.change.ToString();

        graph.current = graph.current.GetOutputPort("exit").Connection.node;
        ShowDialog();
    }

    private void NODE_RegularDialog()
    {
        DialogNode node = (DialogNode)graph.current;

        portrait.sprite = node.character.portrait;
        speakerName.text = node.character.characterName;
        dialogText.text = node.line;

        UpdateDialogPosition(node.answers.Length > 0);

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

    void UpdateDialogPosition(bool multiple)
    {
        DialogNode node = (DialogNode)graph.current;

        if (multiple)
        {
            dialogBubble.anchoredPosition = multipleChoicePos;
            nextButton.gameObject.SetActive(false);

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
            dialogBubble.anchoredPosition = dialogPos;
            nextButton.gameObject.SetActive(true);
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
        if (graph.current.GetType() == typeof(StoryEndNode))
        {
            return;
        }

        graph.current = graph.current.GetOutputPort("exit").Connection.node;
        ShowDialog();
    }

    public void MakeChoice(int _choice)
    {
        Node node = graph.current.GetPort("answers " + _choice.ToString()).Connection.node;
        Debug.Log("you selected " + node.name);
        graph.current = node;
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
