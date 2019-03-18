using System.Collections;
using System.Collections.Generic;

public partial class XmlData
{
	public Dictionary<string, XmlData> Childs;
	public Dictionary<string, string> Attributes;
	public string Node;

	public string GetAttribute(string key)
	{
		string value;
		if (Attributes.TryGetValue(key, out value))
			return value;

		return string.Empty;
	}
}