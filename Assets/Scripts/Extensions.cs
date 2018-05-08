using UnityEngine;
using System.Collections;

public static class Extensions
{

	public static int ToInt(this string val)
	{
		int output;
		if (int.TryParse (val, out output)) {
			return output;
		}

		return -1;
	}
}

