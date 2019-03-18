using UnityEngine;
using System.Collections;

public class ToolParser
{
	public static int IntParse(string value)
	{
		int result;
		if (int.TryParse(value, out result))
			return result;

		return 0;
	}

	public static float FloatParse(string value)
	{
		float result;
		if (float.TryParse(value, out result))
			return result;

		return 0;
	}
}
