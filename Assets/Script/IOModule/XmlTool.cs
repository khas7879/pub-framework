using UnityEngine;
using System.Xml;
using System.Collections.Generic;
using System.IO;

namespace CJC.Framework.IO
{
	public class XmlTool
	{
		private const string Root = "root";
		public static XmlData LoadFile(string fileURL)
		{
			string url = Application.dataPath + "/" + fileURL;

			XmlDocument doc = new XmlDocument();
			doc.Load(url);

			XmlNode node = doc.SelectSingleNode(Root);
			XmlData data = LoadData(node);
			return data;
		}

		private static XmlData LoadData(XmlNode node)
		{
			XmlData data = new XmlData();
			data.SetNodeName(node.Name);

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
			data.Childs = new List<XmlData>();
			for(int idx = 0; idx < node_count; ++idx)
			{
				var child = nodes.Item(idx);
				XmlData newData = LoadData(child);
				data.Childs.Add(newData);
			}
			return data;
		}

#if UNITY_EDITOR
		public static void ExportFile(string url, XmlData data)
		{
			url = Application.dataPath + "/" + url;

			XmlDocument doc = new XmlDocument();
			doc.AppendChild(doc.CreateXmlDeclaration("1.0", "utf-8", ""));

			if (string.IsNullOrEmpty(data.Node))
				data.SetNodeName(Root);

			XmlElement root = ExportData(doc, data);
			doc.AppendChild(root);
			doc.Save(url);
		}

		private static XmlElement ExportData(XmlDocument doc, XmlData data)
		{
			string nodeName = string.IsNullOrEmpty(data.Node) ? "node" : data.Node;
			XmlElement element = doc.CreateElement(nodeName);
			if(null != data.Attributes)
			{
				foreach (var attributePair in data.Attributes)
					element.SetAttribute(attributePair.Key, attributePair.Value);
			}

			if(null != data.Childs)
			{
				foreach (var childData in data.Childs)
				{
					XmlElement childElement = ExportData(doc, childData);
					element.AppendChild(childElement);
				}
			}
			return element;
		}
#endif
	}
}