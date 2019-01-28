using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class init_SO : MonoBehaviour {

	public bool_var bool_Var;
	public bool val;

	private void Awake() {
		bool_Var.val = val;
	}

}
