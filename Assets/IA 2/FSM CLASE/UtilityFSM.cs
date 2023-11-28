using UnityEngine;
//using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;


public static class UtilityFSM {

	public static T Log<T>(T param, string message = "") {
		Debug.Log(message +  param.ToString());
		return param;
	}


}
