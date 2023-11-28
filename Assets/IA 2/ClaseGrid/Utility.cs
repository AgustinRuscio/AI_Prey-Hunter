using UnityEngine;
//using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;

public static class Utility {
    // Transformaciones

    public static int Clampi(int v, int min, int max)
    {
        return v < min ? min : (v > max ? max : v);
    }

	public static T Log<T>(T param, string message = "") {
		Debug.Log(message +  param.ToString());
		return param;
	}

	public static IEnumerable<Src> Generate<Src>(Src seed, Func<Src, Src> generator) {
		while (true) {
			yield return seed;
			seed = generator(seed);
		}
	}
}
