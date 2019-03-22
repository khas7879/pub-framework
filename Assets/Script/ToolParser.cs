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

	public static string StringParser(Vector3 vector)
	{
		return string.Format("{0},{1},{2}", vector.x, vector.y, vector.z);
	}

	public static Vector3 VectorParser(string str)
	{
		var strArray = str.Split(',');
		switch (strArray.Length)
		{
			case 2:
				{
					float x = FloatParse(strArray[0]);
					float y = FloatParse(strArray[1]);
					return new Vector3(x, y, 0);
				}
			case 3:
				{
					float x = FloatParse(strArray[0]);
					float y = FloatParse(strArray[1]);
					float z = FloatParse(strArray[2]);
					return new Vector3(x, y, z);
				}
			default:
				return Vector3.zero;
		}
	}
}
