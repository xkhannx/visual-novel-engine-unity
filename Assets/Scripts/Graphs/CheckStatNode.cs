﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class CheckStatNode : Node {
	[Input(ShowBackingValue.Never)] public bool enter;

    public int threshold;
    public StatSO stat;

    [Output(ShowBackingValue.Never, ConnectionType.Override)] public bool greaterOrEqual;
    [Output(ShowBackingValue.Never, ConnectionType.Override)] public bool less;


    // Use this for initialization
    protected override void Init() {
		base.Init();
		
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		return null; // Replace this
	}
}