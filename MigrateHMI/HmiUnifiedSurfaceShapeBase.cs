using System;
using MigrateHMI.Shapes;

namespace MigrateHMI
{
	// Token: 0x02000005 RID: 5
	internal class HmiUnifiedSurfaceShapeBase : HmiUnifiedShapeBase
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000038 RID: 56 RVA: 0x00003294 File Offset: 0x00001494
		// (set) Token: 0x06000039 RID: 57 RVA: 0x0000329C File Offset: 0x0000149C
		public uint Height { get; protected set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600003A RID: 58 RVA: 0x000032A5 File Offset: 0x000014A5
		// (set) Token: 0x0600003B RID: 59 RVA: 0x000032AD File Offset: 0x000014AD
		public int Left { get; protected set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600003C RID: 60 RVA: 0x000032B6 File Offset: 0x000014B6
		// (set) Token: 0x0600003D RID: 61 RVA: 0x000032BE File Offset: 0x000014BE
		public int Top { get; protected set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600003E RID: 62 RVA: 0x000032C7 File Offset: 0x000014C7
		// (set) Token: 0x0600003F RID: 63 RVA: 0x000032CF File Offset: 0x000014CF
		public uint Width { get; protected set; }
	}
}
