using System;

namespace MigrateHMI.Shapes
{
	// Token: 0x02000007 RID: 7
	internal class HmiUnifiedCentricShapeBase : HmiUnifiedShapeBase
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000049 RID: 73 RVA: 0x000035B0 File Offset: 0x000017B0
		// (set) Token: 0x0600004A RID: 74 RVA: 0x000035B8 File Offset: 0x000017B8
		public int CenterX { get; protected set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600004B RID: 75 RVA: 0x000035C1 File Offset: 0x000017C1
		// (set) Token: 0x0600004C RID: 76 RVA: 0x000035C9 File Offset: 0x000017C9
		public int CenterY { get; protected set; }
	}
}
