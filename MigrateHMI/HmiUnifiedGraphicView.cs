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
	// Token: 0x02000003 RID: 3
	internal class HmiUnifiedGraphicView : HmiUnifiedSurfaceShapeBase
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000003 RID: 3 RVA: 0x00002075 File Offset: 0x00000275
		// (set) Token: 0x06000004 RID: 4 RVA: 0x0000207D File Offset: 0x0000027D
		public Color AlternateBackColor { get; private set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000005 RID: 5 RVA: 0x00002086 File Offset: 0x00000286
		// (set) Token: 0x06000006 RID: 6 RVA: 0x0000208E File Offset: 0x0000028E
		public HmiFillDirection FillDirection { get; private set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000007 RID: 7 RVA: 0x00002097 File Offset: 0x00000297
		// (set) Token: 0x06000008 RID: 8 RVA: 0x0000209F File Offset: 0x0000029F
		public byte FillLevel { get; private set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000009 RID: 9 RVA: 0x000020A8 File Offset: 0x000002A8
		// (set) Token: 0x0600000A RID: 10 RVA: 0x000020B0 File Offset: 0x000002B0
		public string Graphic { get; private set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000B RID: 11 RVA: 0x000020B9 File Offset: 0x000002B9
		// (set) Token: 0x0600000C RID: 12 RVA: 0x000020C1 File Offset: 0x000002C1
		public HmiGraphicStretchMode GraphicStretchMode { get; private set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000D RID: 13 RVA: 0x000020CA File Offset: 0x000002CA
		// (set) Token: 0x0600000E RID: 14 RVA: 0x000020D2 File Offset: 0x000002D2
		public HmiPaddingPart Padding { get; private set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600000F RID: 15 RVA: 0x000020DB File Offset: 0x000002DB
		// (set) Token: 0x06000010 RID: 16 RVA: 0x000020E3 File Offset: 0x000002E3
		public bool ShowFillLevel { get; private set; }

		// Token: 0x06000011 RID: 17 RVA: 0x000020EC File Offset: 0x000002EC
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

		// Token: 0x06000012 RID: 18 RVA: 0x00002164 File Offset: 0x00000364
		private void ImportGraphicViewAuthorization(XElement graphicViewElement)
		{
			string text = base.ImportAuthorization(graphicViewElement);
			if (text != null)
			{
				base.Authorization = text;
				this.Attributes.Add("Authorization", base.Authorization);
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000219C File Offset: 0x0000039C
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

		// Token: 0x06000014 RID: 20 RVA: 0x0000228C File Offset: 0x0000048C
		private void ImportGraphBackFillPattern()
		{
			if (base.ImportBackFillPattern(this.graphicViewElement, ref this.BackFillPattern))
			{
				this.Attributes.Add("BackFillPattern", this.BackFillPattern);
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000022C0 File Offset: 0x000004C0
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

		// Token: 0x06000016 RID: 22 RVA: 0x00002394 File Offset: 0x00000594
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

		// Token: 0x06000017 RID: 23 RVA: 0x00002450 File Offset: 0x00000650
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

		// Token: 0x06000018 RID: 24 RVA: 0x00002508 File Offset: 0x00000708
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

		// Token: 0x06000019 RID: 25 RVA: 0x000025F0 File Offset: 0x000007F0
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

		// Token: 0x0600001A RID: 26 RVA: 0x000026AC File Offset: 0x000008AC
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

		// Token: 0x0600001B RID: 27 RVA: 0x0000279C File Offset: 0x0000099C
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

		// Token: 0x0600001C RID: 28 RVA: 0x00002874 File Offset: 0x00000A74
		public override void Create(HmiScreen screen)
		{
			screen.ScreenItems.Create<HmiGraphicView>(base.Name).SetAttributes(this.Attributes);
		}

		// Token: 0x04000002 RID: 2
		private HmiSoftware hmiUnifiedTarget;

		// Token: 0x04000003 RID: 3
		private string screeName;

		// Token: 0x04000004 RID: 4
		private XElement graphicViewElement;

		// Token: 0x04000005 RID: 5
		private string name;

		// Token: 0x04000007 RID: 7
		private HmiFillPattern BackFillPattern;
	}
}
