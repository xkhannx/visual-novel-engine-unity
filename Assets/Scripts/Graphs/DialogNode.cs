using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using UnityEngine.UI;

public class DialogNode : Node {
    [Input(ShowBackingValue.Never)] public bool enter;

    public CharacterSO character;
    public Emotions emotion;
    [TextArea(1, 20)] public string line;

    [Output(ShowBackingValue.Never, ConnectionType.Override)] public bool exit;
    [Output(dynamicPortList = true)] public Choice[] answers;

    // Use this for initialization
    protected override void Init() {
		base.Init();
    }

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		return null; // Replace this
	}
}
