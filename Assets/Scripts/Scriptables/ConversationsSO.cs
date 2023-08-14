using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Conversations List", menuName = "Game/Conversations List")]
public class ConversationsSO : ScriptableObject
{
    public List<StoryGraph> conversations;
}