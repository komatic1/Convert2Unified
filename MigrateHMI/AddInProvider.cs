using System;
using System.Collections.Generic;
using Siemens.Engineering;
using Siemens.Engineering.AddIn;
using Siemens.Engineering.AddIn.Menu;

namespace MigrateHMI
{
	public class AddInProvider : ProjectTreeAddInProvider
	{
		public AddInProvider(TiaPortal tiaPortal)
		{
			if (tiaPortal == null)
			{
				throw new ArgumentNullException("tiaPortal");
			}
			this.m_TiaPortal = tiaPortal;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002065 File Offset: 0x00000265
		protected override IEnumerable<ContextMenuAddIn> GetContextMenuAddIns()
		{
			yield return new MigrationAddin(this.m_TiaPortal);
			yield break;
		}

		// Token: 0x04000001 RID: 1
		private readonly TiaPortal m_TiaPortal;
	}
}
