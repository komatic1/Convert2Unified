using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using MigrateHMI.Shapes;
using Siemens.Engineering.HmiUnified;
using Siemens.Engineering.HmiUnified.UI.Screens;

namespace MigrateHMI
{
	public class HmiUnifiedScreen
	{
		public FileInfo fileInfo { get; private set; }
		public string Name { get; private set; }
		public bool Enabled { get; private set; }
		public byte ScreenNumber { get; private set; }
		public Color hmiUniBackColor { get; private set; }
		public Dictionary<XElement, bool> AttributeElementDictionary { get; private set; }
		public Dictionary<string, string> AttributeErrorsDictionary { get; private set; }
		public List<HmiUnifiedScreenItemBase> screenItems { get; private set; }
		public HmiUnifiedScreen(FileInfo file, HmiSoftware target)
		{
			this.AttributeElementDictionary = new Dictionary<XElement, bool>();
			this.screenItems = new List<HmiUnifiedScreenItemBase>();
			this.fileInfo = file;
			this.hmiUnifiedTarget = target;
			this.AttributeErrorsDictionary = new Dictionary<string, string>();
            this.screenElement = XElement.Load(this.fileInfo.FullName);
			IEnumerable<XElement> source = from elements in (from elements in (from elements in this.screenElement.Elements("Hmi.Screen.Screen")
			select elements).Elements("AttributeList")
			select elements).Elements("Name")
			select elements;
			this.Name = source.Nodes<XElement>().FirstOrDefault<XNode>().ToString();
			this.Name = this.GetScreenName(target, this.Name);
			this.ImportHmiUnifiedBackColor();
			this.ImportHmiUnifiedScreenNumber();
			this.ImportGraphicViews();
			this.ImportCircle();
		}
		private string GetScreenName(HmiSoftware target, string screen)
		{
			int num = 1;
			string text = screen;
			HmiScreen hmiScreen = target.Screens.Find(screen);
			if (hmiScreen != null)
			{
				text = screen + "_" + num.ToString();
				for (hmiScreen = target.Screens.Find(text); hmiScreen != null; hmiScreen = target.Screens.Find(text))
				{
					num++;
					text = text.Substring(0, text.LastIndexOf('_') + 1);
					text += num.ToString();
				}
			}
			return text;
		}
		public HmiScreen CreateScreen()
		{
			HmiScreen hmiScreen = null;
			if (this.hmiUnifiedTarget.Screens.Find(this.Name) == null)
			{
				hmiScreen = this.hmiUnifiedTarget.Screens.Create(this.Name);
				foreach (KeyValuePair<string, object> keyValuePair in this.attributes)
				{
					Dictionary<string, object> dictionary = this.attributes;
					try
					{
						hmiScreen.SetAttributes(dictionary);
					}
					catch (Exception)
					{
					}
				}
				foreach (HmiUnifiedScreenItemBase hmiUnifiedScreenItemBase in this.screenItems)
				{
					hmiUnifiedScreenItemBase.Create(hmiScreen);
				}
			}
			if (hmiScreen == null)
			{
				throw new ArgumentException(this.Name);
			}
			return hmiScreen;
		}
		private void ImportHmiUnifiedBackColor()
		{
			XElement xelement = (from element in (from elements in (from elements in this.screenElement.Elements("Hmi.Screen.Screen")
			select elements).Elements("AttributeList")
			select elements).Elements("BackColor")
			select element).FirstOrDefault<XElement>();
			string value = xelement.Value;
			string[] array = value.Split(new char[]
			{
				','
			});
			int red;
			int.TryParse(array[0], out red);
			int green;
			bool flag = int.TryParse(array[1], out green);
			int blue;
			bool flag2 = int.TryParse(array[2], out blue);
			if (false && flag && flag2)
			{
				this.hmiUniBackColor = Color.FromArgb(red, green, blue);
				this.attributes.Add("BackColor", this.hmiUniBackColor);
				this.AttributeElementDictionary.Add(xelement, false);
				return;
			}
			this.AttributeErrorsDictionary.Add("BackColor", value);
			this.AttributeElementDictionary.Add(xelement, true);
		}
		private void ImportHmiUnifiedScreenNumber()
		{
			XElement xelement = (from element in (from elements in (from elements in this.screenElement.Elements("Hmi.Screen.Screen")
			select elements).Elements("AttributeList")
			select elements).Elements("Number")
			select element).FirstOrDefault<XElement>();
			byte screenNumber;
			if (byte.TryParse(xelement.Value, out screenNumber))
			{
				this.ScreenNumber = screenNumber;
				Dictionary<string, object> dictionary = this.attributes;
				if (dictionary != null)
				{
					dictionary.Add("ScreenNumber", this.ScreenNumber);
				}
				this.AttributeElementDictionary.Add(xelement, false);
				return;
			}
			this.AttributeErrorsDictionary.Add("ScreenNumber", xelement.Value);
			this.AttributeElementDictionary.Add(xelement, true);
		}
		private void ImportGraphicViews()
		{
			IEnumerable<XElement> enumerable = from elements in (from elements in (from elements in (from elements in this.screenElement.Elements("Hmi.Screen.Screen")
			select elements).Elements("ObjectList")
			select elements).Elements("Hmi.Screen.ScreenLayer")
			select elements).Elements("ObjectList")
			select elements;
			foreach (XElement xelement in enumerable)
			{
				xelement.Value = "Hmi.Screen.GraphicView";
			}
			foreach (XElement graphicViewElement in from elements in enumerable.Elements("Hmi.Screen.GraphicView")
			select elements)
			{
				this.screenItems.Add(new HmiUnifiedGraphicView(this.hmiUnifiedTarget, this.Name, graphicViewElement));
			}
		}
		private void ImportCircle()
		{
			foreach (XElement screenItemElement in from elements in (from elements in (from elements in (from elements in (from elements in this.screenElement.Elements("Hmi.Screen.Screen")
			select elements).Elements("ObjectList")
			select elements).Elements("Hmi.Screen.ScreenLayer")
			select elements).Elements("ObjectList")
			select elements).Elements("Hmi.Screen.Circle")
			select elements)
			{
				this.screenItems.Add(new HmiUnifiedCircle(this.hmiUnifiedTarget, this.Name, screenItemElement));
			}
		}
		public HmiUnifiedScreen(HmiSoftware target, string screenName)
		{
			this.hmiUnifiedTarget = target;
			this.AttributeElementDictionary = new Dictionary<XElement, bool>();
			this.screenItems = new List<HmiUnifiedScreenItemBase>();
			this.CreateScreen(screenName);
		}
		private string GetScreenName(string screen)
		{
			int num = 1;
			string text = screen;
			HmiScreen hmiScreen = this.hmiUnifiedTarget.Screens.Find(screen);
			if (hmiScreen != null)
			{
				text = screen + "_" + num.ToString();
				for (hmiScreen = this.hmiUnifiedTarget.Screens.Find(text); hmiScreen != null; hmiScreen = this.hmiUnifiedTarget.Screens.Find(text))
				{
					num++;
					text = text.Substring(0, text.LastIndexOf('_') + 1);
					text += num.ToString();
				}
			}
			return text;
		}
		private HmiScreen CreateScreen(string name)
		{
			string screenName = this.GetScreenName(name);
			if (this.hmiUnifiedTarget.Screens.Find(screenName) == null)
			{
				this.screen = this.hmiUnifiedTarget.Screens.Create(screenName);
			}
			if (this.screen == null)
			{
				throw new ArgumentException(screenName);
			}
			return this.screen;
		}
		public bool setName(string attributeValue)
		{
			bool result = false;
			try
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				dictionary.Add("Name", attributeValue);
				this.hmiUnifiedTarget.SetAttributes(dictionary);
				result = true;
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}
		private HmiSoftware hmiUnifiedTarget;
		private XElement screenElement;
		private Dictionary<string, object> attributes = new Dictionary<string, object>();
		private HmiScreen screen;
	}
}
