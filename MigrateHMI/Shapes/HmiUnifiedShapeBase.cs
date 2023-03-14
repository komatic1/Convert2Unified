using System;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using Siemens.Engineering.HmiUnified.UI.Enum;

namespace MigrateHMI.Shapes
{
	// Token: 0x02000009 RID: 9
	internal class HmiUnifiedShapeBase : HmiUnifiedSimpleScreenItemBase
	{
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000066 RID: 102 RVA: 0x0000394A File Offset: 0x00001B4A
		// (set) Token: 0x06000067 RID: 103 RVA: 0x00003952 File Offset: 0x00001B52
		public short RotationAngle { get; protected set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000068 RID: 104 RVA: 0x0000395B File Offset: 0x00001B5B
		// (set) Token: 0x06000069 RID: 105 RVA: 0x00003963 File Offset: 0x00001B63
		public HmiRotationCenterPlacement RotationCenterPlacement { get; protected set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600006A RID: 106 RVA: 0x0000396C File Offset: 0x00001B6C
		// (set) Token: 0x0600006B RID: 107 RVA: 0x00003974 File Offset: 0x00001B74
		public float RotationCenterX { get; protected set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600006C RID: 108 RVA: 0x0000397D File Offset: 0x00001B7D
		// (set) Token: 0x0600006D RID: 109 RVA: 0x00003985 File Offset: 0x00001B85
		public float RotationCenterY { get; protected set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600006E RID: 110 RVA: 0x0000398E File Offset: 0x00001B8E
		// (set) Token: 0x0600006F RID: 111 RVA: 0x00003996 File Offset: 0x00001B96
		public Color BackColor { get; protected set; }

		// Token: 0x06000070 RID: 112 RVA: 0x000039A0 File Offset: 0x00001BA0
		protected bool ImportBackFillPattern(XElement screenItemElement, ref HmiFillPattern backFillPattern)
		{
			bool result = true;
			XNode xnode = (from element in (from elements in screenItemElement.Elements("AttributeList")
			select elements).Elements("BackFillStyle")
			select element).Nodes<XElement>().FirstOrDefault<XNode>();
			string text = (xnode != null) ? xnode.ToString() : null;

			switch (text)
			{
				case "GradientHorizontalTricolor":
					backFillPattern = HmiFillPattern.GradientHorizontalTricolor;  //12;
                    return result;
				case "GradientForwardDiagonalTricolor":
					backFillPattern = HmiFillPattern.GradientForwardDiagonalTricolor; //14;
                    return result;
				case "Solid":
					backFillPattern = HmiFillPattern.Solid; // 0;
                    return result;
				case "GradientBackwardDiagonal":
					backFillPattern = HmiFillPattern.GradientBackwardDiagonal; // 11;
                    return result;
				case "Horizontal":
					backFillPattern = HmiFillPattern.ForwardDiagonal; // 6;
                    return result;
				case "GradientBackwardDiagonalTricolor":
					backFillPattern = HmiFillPattern.GradientForwardDiagonalTricolor; // 15;
                    return result;
				case "GradientVertical":
					backFillPattern = HmiFillPattern.GradientHorizontal; // 9;
                    return result;
				case "Vertical":
					backFillPattern = HmiFillPattern.GradientVertical; // 7;
                    return result;
				case "Transparent":
					backFillPattern = HmiFillPattern.Transparent; // 1;
                    return result;
				case "GradientVerticalTricolor":
					backFillPattern = HmiFillPattern.GradientVerticalTricolor; // 13;
                    return result;
				case "Cross":
					backFillPattern = HmiFillPattern.BackwardDiagonal; // 3;
                    return result;
				case "GradientForwardDiagonal":
					backFillPattern = HmiFillPattern.GradientForwardDiagonal; // 10;
                    return result;
				case "GradientHorizontal":
					backFillPattern = HmiFillPattern.GradientHorizontal; // 8;
                    return result;
				case "ForwardDiagonal":
					backFillPattern = HmiFillPattern.ForwardDiagonal; // 2;
                    return result;
				case "BackwardDiagonal":
					backFillPattern = HmiFillPattern.BackwardDiagonal; // 2;
                    return result;
				case "DiagonalCross":
					backFillPattern = HmiFillPattern.DiagonalCross; // 4;
                    return result;
				default:
                    result = false;
                    return result;
            }

		}

		// Token: 0x06000071 RID: 113 RVA: 0x00003CD0 File Offset: 0x00001ED0
		protected bool ImportBackColor(XElement screenItemElement)
		{
			bool result = false;
			XElement xelement = (from element in (from elements in screenItemElement.Elements("AttributeList")
			select elements).Elements("BackColor")
			select element).FirstOrDefault<XElement>();
			string value = xelement.Value;
			if (value != string.Empty)
			{
				string[] array = value.Split(new char[]
				{
					','
				});
				int red;
				bool flag = int.TryParse(array[0], out red);
				int green;
				bool flag2 = int.TryParse(array[1], out green);
				int blue;
				bool flag3 = int.TryParse(array[2], out blue);
				if (flag && flag2 && flag3)
				{
					this.BackColor = Color.FromArgb(red, green, blue);
					result = true;
					this.Attributes.Add("BackColor", this.BackColor);
					base.AttributeElementDictionary.Add(xelement, false);
				}
			}
			return result;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00003DDC File Offset: 0x00001FDC
		protected string ImportAuthorization(XElement screenItemElement)
		{
			XNode xnode = (from elements in (from elements in screenItemElement.Elements("AttributeList")
			select elements).Elements("Authorization")
			select elements).Nodes<XElement>().FirstOrDefault<XNode>();
			if (xnode == null)
			{
				return null;
			}
			return xnode.ToString();
		}
	}
}
