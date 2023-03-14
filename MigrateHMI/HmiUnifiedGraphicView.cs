using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using Siemens.Engineering.HmiUnified;
using Siemens.Engineering.HmiUnified.UI.Enum;
using Siemens.Engineering.HmiUnified.UI.Parts;
using Siemens.Engineering.HmiUnified.UI.Screens;
using Siemens.Engineering.HmiUnified.UI.Shapes;

namespace MigrateHMI
{
	internal class HmiUnifiedGraphicView : HmiUnifiedSurfaceShapeBase
	{
		public Color AlternateBackColor { get; private set; }
		public HmiFillDirection FillDirection { get; private set; }
		public byte FillLevel { get; private set; }
		public string Graphic { get; private set; }
		public HmiGraphicStretchMode GraphicStretchMode { get; private set; }
		public HmiPaddingPart Padding { get; private set; }
		public bool ShowFillLevel { get; private set; }
		public HmiUnifiedGraphicView(HmiSoftware target, string screenName, XElement graphicViewElement)
		{
			base.AttributeElementDictionary = new Dictionary<XElement, bool>();
			this.hmiUnifiedTarget = target;
			this.screeName = screenName;
			this.graphicViewElement = graphicViewElement;
			this.ImportGraphicViewAuthorization(graphicViewElement);
			base.ImportBackColor(graphicViewElement);
			this.ImportEnabled();
			this.ImportGraphBackFillPattern();
			this.ImportHeight();
			this.ImportLeft();
			this.ImportName();
			this.ImportGraphic();
			this.ImportTop();
			this.ImportVisible();
			this.ImportWidth();
		}
		private void ImportGraphicViewAuthorization(XElement graphicViewElement)
		{
			string text = base.ImportAuthorization(graphicViewElement);
			if (text != null)
			{
				base.Authorization = text;
				this.Attributes.Add("Authorization", base.Authorization);
			}
		}
		private void ImportEnabled()
		{
			XNode xnode = (from element in (from elements in this.graphicViewElement.Elements("AttributeList")
			select elements).Elements("Enabled")
			select element).Nodes<XElement>().FirstOrDefault<XNode>();
			string a = (xnode != null) ? xnode.ToString() : null;
			if (a == "false")
			{
				base.Enabled = false;
				this.Attributes.Add("Enabled", base.Enabled);
				return;
			}
			if (!(a == "true"))
			{
				return;
			}
			base.Enabled = true;
			this.Attributes.Add("Enabled", base.Enabled);
		}
		private void ImportGraphBackFillPattern()
		{
			if (base.ImportBackFillPattern(this.graphicViewElement, ref this.BackFillPattern))
			{
				this.Attributes.Add("BackFillPattern", this.BackFillPattern);
			}
		}
		private void ImportHeight()
		{
			XNode xnode = (from element in (from elements in this.graphicViewElement.Elements("AttributeList")
			select elements).Elements("Height")
			select element).Nodes<XElement>().FirstOrDefault<XNode>();
			int num;
			if (int.TryParse((xnode != null) ? xnode.ToString() : null, out num))
			{
				try
				{
					uint height = checked((uint)num);
					base.Height = height;
					this.Attributes.Add("Height", base.Height);
				}
				catch (OverflowException)
				{
				}
			}
		}
		private void ImportLeft()
		{
			XNode xnode = (from element in (from elements in this.graphicViewElement.Elements("AttributeList")
			select elements).Elements("Left")
			select element).Nodes<XElement>().FirstOrDefault<XNode>();
			int left;
			if (int.TryParse((xnode != null) ? xnode.ToString() : null, out left))
			{
				base.Left = left;
				this.Attributes.Add("Left", base.Left);
			}
		}
		private void ImportName()
		{
			IEnumerable<XElement> source = from element in (from elements in this.graphicViewElement.Elements("AttributeList")
			select elements).Elements("ObjectName")
			select element;
			XNode xnode = source.Nodes<XElement>().FirstOrDefault<XNode>();
			base.Name = ((xnode != null) ? xnode.ToString() : null);
			if (base.Name != null)
			{
				this.Attributes.Add("Name", base.Name);
			}
		}
		private void ImportGraphic()
		{
			IEnumerable<XElement> source = from element in (from element in (from elements in this.graphicViewElement.Elements("LinkList")
			select elements).Elements("Picture")
			select element).Elements("Name")
			select element;
			XNode xnode = source.Nodes<XElement>().FirstOrDefault<XNode>();
			this.Graphic = ((xnode != null) ? xnode.ToString() : null);
			if (this.Graphic != null)
			{
				this.Attributes.Add("Graphic", this.Graphic);
			}
		}
		private void ImportTop()
		{
			XNode xnode = (from elements in (from elements in this.graphicViewElement.Elements("AttributeList")
			select elements).Elements("Top")
			select elements).Nodes<XElement>().FirstOrDefault<XNode>();
			int top;
			if (int.TryParse((xnode != null) ? xnode.ToString() : null, out top))
			{
				base.Top = top;
				this.Attributes.Add("Top", base.Top);
			}
		}
		private void ImportVisible()
		{
			XNode xnode = (from elements in (from elements in this.graphicViewElement.Elements("AttributeList")
			select elements).Elements("Visible")
			select elements).Nodes<XElement>().FirstOrDefault<XNode>();
			if (xnode != null)
			{
				string a = xnode.ToString();
				if (a == "true")
				{
					base.Visible = true;
					this.Attributes.Add("Visible", base.Visible);
					return;
				}
				if (!(a == "false"))
				{
					return;
				}
				base.Visible = false;
				this.Attributes.Add("Visible", base.Visible);
			}
		}
		private void ImportWidth()
		{
			XNode xnode = (from element in (from elements in this.graphicViewElement.Elements("AttributeList")
			select elements).Elements("Height")
			select element).Nodes<XElement>().FirstOrDefault<XNode>();
			int num;
			if (int.TryParse((xnode != null) ? xnode.ToString() : null, out num))
			{
				try
				{
					uint width = checked((uint)num);
					base.Width = width;
					this.Attributes.Add("Width", base.Width);
				}
				catch (Exception value)
				{
					Console.WriteLine(value);
					throw;
				}
			}
		}
		public override void Create(HmiScreen screen)
		{
			screen.ScreenItems.Create<HmiGraphicView>(base.Name).SetAttributes(this.Attributes);
		}
		private HmiSoftware hmiUnifiedTarget;
		private string screeName;
		private XElement graphicViewElement;
		private string name;
		private HmiFillPattern BackFillPattern;
	}
}
