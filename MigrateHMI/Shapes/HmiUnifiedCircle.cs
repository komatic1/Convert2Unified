using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using Siemens.Engineering.HmiUnified;
using Siemens.Engineering.HmiUnified.UI.Enum;
using Siemens.Engineering.HmiUnified.UI.Screens;
using Siemens.Engineering.HmiUnified.UI.Shapes;

namespace MigrateHMI.Shapes
{
	// Token: 0x02000008 RID: 8
	internal class HmiUnifiedCircle : HmiUnifiedCentricShapeBase
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600004E RID: 78 RVA: 0x000035D2 File Offset: 0x000017D2
		// (set) Token: 0x0600004F RID: 79 RVA: 0x000035DA File Offset: 0x000017DA
		public Color AlternateBackColor { get; private set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000050 RID: 80 RVA: 0x000035E3 File Offset: 0x000017E3
		// (set) Token: 0x06000051 RID: 81 RVA: 0x000035EB File Offset: 0x000017EB
		public Color AlternateBorderColor { get; private set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000052 RID: 82 RVA: 0x000035F4 File Offset: 0x000017F4
		// (set) Token: 0x06000053 RID: 83 RVA: 0x000035FC File Offset: 0x000017FC
		public Color BorderColor { get; private set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000054 RID: 84 RVA: 0x00003605 File Offset: 0x00001805
		// (set) Token: 0x06000055 RID: 85 RVA: 0x0000360D File Offset: 0x0000180D
		public byte BorderWidth { get; private set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000056 RID: 86 RVA: 0x00003616 File Offset: 0x00001816
		// (set) Token: 0x06000057 RID: 87 RVA: 0x0000361E File Offset: 0x0000181E
		public HmiDashType DashType { get; private set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000058 RID: 88 RVA: 0x00003627 File Offset: 0x00001827
		// (set) Token: 0x06000059 RID: 89 RVA: 0x0000362F File Offset: 0x0000182F
		public HmiFillDirection FillDirection { get; private set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00003638 File Offset: 0x00001838
		// (set) Token: 0x0600005B RID: 91 RVA: 0x00003640 File Offset: 0x00001840
		public byte FillLevel { get; private set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600005C RID: 92 RVA: 0x00003649 File Offset: 0x00001849
		// (set) Token: 0x0600005D RID: 93 RVA: 0x00003651 File Offset: 0x00001851
		public bool ShowFillLevel { get; private set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600005E RID: 94 RVA: 0x0000365A File Offset: 0x0000185A
		// (set) Token: 0x0600005F RID: 95 RVA: 0x00003662 File Offset: 0x00001862
		public Dictionary<string, object> AttributeErrorsDictionary { get; private set; }

		// Token: 0x06000060 RID: 96 RVA: 0x0000366C File Offset: 0x0000186C
		public HmiUnifiedCircle(HmiSoftware target, string screenName, XElement screenItemElement)
		{
			base.AttributeElementDictionary = new Dictionary<XElement, bool>();
			this.AttributeErrorsDictionary = new Dictionary<string, object>();
			this.Attributes = new Dictionary<string, object>();
			this.hmiUnifiedTarget = target;
			this.screenName = screenName;
			this.screenItemElement = screenItemElement;
			base.ImportBackColor(screenItemElement);
			this.ImportCircleBackFillPattern();
			this.ImportCircleAuthorization(screenItemElement);
			this.ImportEnabled();
			this.ImportVisible();
		}

		// Token: 0x06000061 RID: 97 RVA: 0x000036D6 File Offset: 0x000018D6
		private void ImportCircleBackFillPattern()
		{
			if (base.ImportBackFillPattern(this.screenItemElement, ref this.BackFillPattern))
			{
				this.Attributes.Add("BackFillPattern", this.BackFillPattern);
			}
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00003707 File Offset: 0x00001907
		public override void Create(HmiScreen screen)
		{
			screen.ScreenItems.Create<HmiCircle>(base.Name).SetAttributes(this.Attributes);
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00003728 File Offset: 0x00001928
		private void ImportCircleAuthorization(XElement graphicViewElement)
		{
			string text = base.ImportAuthorization(graphicViewElement);
			if (text != null)
			{
				base.Authorization = text;
				this.Attributes.Add("Authorization", base.Authorization);
				base.AttributeElementDictionary.Add(graphicViewElement, false);
			}
		}

		// Token: 0x06000064 RID: 100 RVA: 0x0000376C File Offset: 0x0000196C
		private void ImportEnabled()
		{
			XNode xnode = (from element in (from elements in this.screenItemElement.Elements("AttributeList")
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

		// Token: 0x06000065 RID: 101 RVA: 0x0000385C File Offset: 0x00001A5C
		private void ImportVisible()
		{
			XNode xnode = (from elements in (from elements in this.screenItemElement.Elements("AttributeList")
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

		// Token: 0x04000025 RID: 37
		private new Color BackColor;

		// Token: 0x04000026 RID: 38
		public HmiFillPattern BackFillPattern;

		// Token: 0x0400002D RID: 45
		private HmiSoftware hmiUnifiedTarget;

		// Token: 0x0400002E RID: 46
		private string screenName;

		// Token: 0x0400002F RID: 47
		private XElement screenItemElement;
	}
}
