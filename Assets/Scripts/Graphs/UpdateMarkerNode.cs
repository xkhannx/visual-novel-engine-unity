﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class UpdateMarkerNode : Node {
    [Input(ShowBackingValue.Never)] public bool enter;
    [Output(ShowBackingValue.Never, ConnectionType.Override)] public bool exit;

	public MarkerSO marker;
    // Use this for initialization
    protected override void Init() {
		base.Init();
		
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		return null; // Replace this
	}
}