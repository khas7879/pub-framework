using System.Collections;
using System.Collections.Generic;

public partial class XmlData
{
	public string Node { get; private set; }

	public List<XmlData> Childs
	{
		get {
			if (null == _childs)
				_childs = new List<XmlData>();
			return _childs;
		}
		set { _childs = value; }
	}

	public Dictionary<string, string> Attributes
	{
		get
		{
			if (null == _attributes)
				_attributes = new Dictionary<string, string>();
			return _attributes;	
		}
		set { _attributes = value; }
	}

	public string GetAttribute(string key)
	{
		string value;
		if (Attributes.TryGetValue(key, out value))
			return value;

		return string.Empty;
	}

	public void SetAttribute(string key, object value)
	{
		if (Attributes.ContainsKey(key))
			Attributes[key] = value.ToString();

		else
			Attributes.Add(key, value.ToString());
	}

	public void AddAttribute(string key, object value)
	{
		SetAttribute(key, value);
	}

	public void AddChild(XmlData data)
	{
		Childs.Add(data);
	}

	public void SetNodeName(object name)
	{
		Node = name.ToString();
	}

	public XmlData SearchChild(System.Predicate<XmlData> func) { return Childs.Find(func); }

	public List<XmlData> SearchChildAll(System.Predicate<XmlData> func) { return Childs.FindAll(func); }

	private List<XmlData> _childs;
	private Dictionary<string, string> _attributes;
}