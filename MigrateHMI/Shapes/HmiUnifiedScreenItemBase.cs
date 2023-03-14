using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Siemens.Engineering.HmiUnified.UI.Enum;
using Siemens.Engineering.HmiUnified.UI.Screens;

namespace MigrateHMI.Shapes
{
	// Token: 0x0200000B RID: 11
	public class HmiUnifiedScreenItemBase
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000077 RID: 119 RVA: 0x00003E86 File Offset: 0x00002086
		// (set) Token: 0x06000078 RID: 120 RVA: 0x00003E8E File Offset: 0x0000208E
		public string Authorization { get; protected set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000079 RID: 121 RVA: 0x00003E97 File Offset: 0x00002097
		// (set) Token: 0x0600007A RID: 122 RVA: 0x00003E9F File Offset: 0x0000209F
		public HmiQuality CurrentQuality { get; protected set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600007B RID: 123 RVA: 0x00003EA8 File Offset: 0x000020A8
		// (set) Token: 0x0600007C RID: 124 RVA: 0x00003EB0 File Offset: 0x000020B0
		public bool Enabled { get; protected set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600007D RID: 125 RVA: 0x00003EB9 File Offset: 0x000020B9
		// (set) Token: 0x0600007E RID: 126 RVA: 0x00003EC1 File Offset: 0x000020C1
		public string Name { get; protected set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600007F RID: 127 RVA: 0x00003ECA File Offset: 0x000020CA
		// (set) Token: 0x06000080 RID: 128 RVA: 0x00003ED2 File Offset: 0x000020D2
		public bool RequiredExplicitRelease { get; protected set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000081 RID: 129 RVA: 0x00003EDB File Offset: 0x000020DB
		// (set) Token: 0x06000082 RID: 130 RVA: 0x00003EE3 File Offset: 0x000020E3
		public ushort TabIndex { get; protected set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000083 RID: 131 RVA: 0x00003EEC File Offset: 0x000020EC
		// (set) Token: 0x06000084 RID: 132 RVA: 0x00003EF4 File Offset: 0x000020F4
		public bool Visible { get; protected set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000085 RID: 133 RVA: 0x00003EFD File Offset: 0x000020FD
		// (set) Token: 0x06000086 RID: 134 RVA: 0x00003F05 File Offset: 0x00002105
		public Dictionary<XElement, bool> AttributeElementDictionary { get; protected set; }

		// Token: 0x06000087 RID: 135 RVA: 0x00003F0E File Offset: 0x0000210E
		public virtual void Create(HmiScreen screen)
		{
		}

		// Token: 0x0400003F RID: 63
		protected Dictionary<string, object> Attributes = new Dictionary<string, object>();
	}
}
