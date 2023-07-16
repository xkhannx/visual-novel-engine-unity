using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class CheckMarkerNode : Node {
	[Input(ShowBackingValue.Never)] public bool enter;

    public MarkerSO marker;

    [Output(ShowBackingValue.Never, ConnectionType.Override)] public bool happened;
    [Output(ShowBackingValue.Never, ConnectionType.Override)] public bool didNotHappen;


    // Use this for initialization
    protected override void Init() {
		base.Init();
		
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		return null; // Replace this
	}
}