using UnityEngine;
using System.Xml;
using System.Collections.Generic;
using System.IO;

namespace CJC.Framework.IO
{
	public class XmlTool
	{
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
		private const string Root = "root";
		private const string Node = "node";

		private static string GetRealPath(string url) { return Application.dataPath + "/" + url; }

		public static XmlData GetXmlData(string url)
		{
			url = GetRealPath(url);
			XmlDocument doc = new XmlDocument();
			doc.Load(url);
			if (null == doc)
				return null;

			XmlData data = new XmlData();
			XmlNode node = doc.DocumentElement;
			ImportXmlData(node, ref data);
			return data;
		}


		private static void ImportXmlData(XmlNode node, ref XmlData data)
		{
			data.SetNodeName(node.Name);
			var attributes = node.Attributes;
			if(null != attributes)
			{
				int attriCount = attributes.Count;
				for (int idx = 0; idx < attriCount; ++idx)
				{
					var attribute = attributes[idx];
					data.AddAttribute(attribute.Name, attribute.Value);
				}
			}

			var childNodes = node.ChildNodes;
			if (null == childNodes)
				return;

			int nodeCount = childNodes.Count;
			for (int idx = 0; idx < nodeCount; ++idx)
			{
				var childNode = childNodes.Item(idx);
				XmlData childData = new XmlData();
				ImportXmlData(childNode, ref childData);
				data.AddChild(childData);
			}
		}

		public static void ExportFile(string url, XmlData data)
		{
			url = GetRealPath(url);

			XmlDocument doc = new XmlDocument();
			doc.AppendChild(doc.CreateXmlDeclaration("1.0", "utf-8", ""));

			if (string.IsNullOrEmpty(data.Node))
				data.SetNodeName(Root);

			XmlElement root = AddXmlData(doc, data);
			doc.AppendChild(root);
			doc.Save(url);
		}

		/// <summary>
		/// 添加一条Xmldata
		/// </summary>
		/// <param name="doc"></param>
		/// <param name="data"></param>
		/// <returns>添加的那条Element</returns>
		private static XmlElement AddXmlData(XmlDocument doc, XmlData data)
		{
			string nodeName = string.IsNullOrEmpty(data.Node) ? Node : data.Node;
			XmlElement element = doc.CreateElement(nodeName);
			ExportXmlData(doc, element, data);
			return element;
		}

		public static bool ReplaceXmlData(string url, IXmlDataSerilizer serilizer, string xpath = null)
		{
			XmlData data = new XmlData();
			if (!serilizer.XmlSerilize(ref data))
				return false;

			return ReplaceXmlData(url, data, xpath);
		}

		/// <summary>
		/// 仅支持在root节点下修改一条数据
		/// </summary>
		/// <param name="url"></param>
		/// <param name="data"></param>
		/// <param name="xpath"></param>
		/// <returns></returns>
		public static bool ReplaceXmlData(string url, XmlData data, string xpath = null)
		{
			url = GetRealPath(url);

			XmlDocument doc = new XmlDocument();
			doc.Load(url);

			if (null == doc)
				return false;

			XmlNode root = doc.DocumentElement;
			XmlNode replacedNode = string.IsNullOrEmpty(xpath) ? null : doc.SelectSingleNode(xpath);
			XmlElement element = AddXmlData(doc, data);

			if(null != replacedNode)
				root.ReplaceChild(element, replacedNode);
			else
				root.AppendChild(element);

			doc.Save(url);
			return true;
		}

		/// <summary>
		/// 在已有一条XmlElement上导出一条XmlData
		/// </summary>
		/// <param name="element"></param>
		/// <param name="data"></param>
		private static void ExportXmlData(XmlDocument doc, XmlElement element, XmlData data)
		{
			if (null != data.Attributes)
			{
				foreach (var attributePair in data.Attributes)
					element.SetAttribute(attributePair.Key, attributePair.Value);
			}

			if (null != data.Childs)
			{
				foreach (var childData in data.Childs)
				{
					XmlElement childElement = AddXmlData(doc, childData);
					element.AppendChild(childElement);
				}
			}
		}
#endif
	}

	public interface IXmlDataSerilizer
	{
		bool XmlSerilize(ref XmlData data);
		bool XmlDeserilize(XmlData data);
	}
}