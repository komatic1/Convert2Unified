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
	// Token: 0x02000004 RID: 4
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

		// Token: 0x0600002E RID: 46 RVA: 0x00002A60 File Offset: 0x00000C60
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

		// Token: 0x0600002F RID: 47 RVA: 0x00002AD8 File Offset: 0x00000CD8
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

		// Token: 0x06000030 RID: 48 RVA: 0x00002BC8 File Offset: 0x00000DC8
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

		// Token: 0x06000031 RID: 49 RVA: 0x00002D14 File Offset: 0x00000F14
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

		// Token: 0x06000032 RID: 50 RVA: 0x00002E30 File Offset: 0x00001030
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

		// Token: 0x06000033 RID: 51 RVA: 0x00002FD4 File Offset: 0x000011D4
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

		// Token: 0x06000034 RID: 52 RVA: 0x00003138 File Offset: 0x00001338
		public HmiUnifiedScreen(HmiSoftware target, string screenName)
		{
			this.hmiUnifiedTarget = target;
			this.AttributeElementDictionary = new Dictionary<XElement, bool>();
			this.screenItems = new List<HmiUnifiedScreenItemBase>();
			this.CreateScreen(screenName);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00003170 File Offset: 0x00001370
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

		// Token: 0x06000036 RID: 54 RVA: 0x000031F8 File Offset: 0x000013F8
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

		// Token: 0x06000037 RID: 55 RVA: 0x0000324C File Offset: 0x0000144C
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

		// Token: 0x0400000F RID: 15
		private HmiSoftware hmiUnifiedTarget;

		// Token: 0x04000010 RID: 16
		private XElement screenElement;

		// Token: 0x04000015 RID: 21
		private Dictionary<string, object> attributes = new Dictionary<string, object>();

		// Token: 0x04000019 RID: 25
		private HmiScreen screen;
	}
}
