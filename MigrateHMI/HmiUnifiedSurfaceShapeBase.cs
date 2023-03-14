using System;
using MigrateHMI.Shapes;

namespace MigrateHMI
{
	internal class HmiUnifiedSurfaceShapeBase : HmiUnifiedShapeBase
	{
		public uint Height { get; protected set; }
		public int Left { get; protected set; }

		public int Top { get; protected set; }
		public uint Width { get; protected set; }
	}
}
