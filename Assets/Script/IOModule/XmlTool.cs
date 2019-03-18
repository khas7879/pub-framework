using UnityEngine;
using System.Xml;
using System.Collections.Generic;

namespace CJC.Framework.IO
{
	public class XmlTool
	{
		public static XmlData LoadFile(string fileURL)
		{
			string url = Application.dataPath + "/" + fileURL;

			XmlDocument doc = new XmlDocument();
			doc.Load(url);

			XmlNode node = doc.SelectSingleNode("root");
			XmlData data = LoadData(node);
			return data;
		}

		private static XmlData LoadData(XmlNode node)
		{
			XmlData data = new XmlData();
			data.Node = node.Name;

			var attributes = node.Attributes;
			int attr_count = attributes.Count;
			data.Attributes = new Dictionary<string, string>();
			for (int idx = 0; idx < attr_count; ++idx)
			{
				var attribute = attributes[idx];
				data.Attributes.Add(attribute.Name, attribute.Value);
			}

			var nodes = node.ChildNodes;
			int node_count = nodes.Count;
			data.Childs = new Dictionary<string, XmlData>();
			for(int idx = 0; idx < node_count; ++idx)
			{
				var child = nodes.Item(idx);
				XmlData newData = LoadData(child);
				data.Childs.Add(child.Name, newData);
			}
			return data;
		}

#if UNITY_EDITOR
		private static bool ExportFile(string url, XmlData data)
		{
			return ExportData(data);
		}

		private static bool ExportData(XmlData data)
		{
			return false;
		}
#endif
	}
}