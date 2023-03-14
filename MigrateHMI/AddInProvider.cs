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
            m_TiaPortal = tiaPortal ?? throw new ArgumentNullException("tiaPortal");
		}

		protected override IEnumerable<ContextMenuAddIn> GetContextMenuAddIns()
		{
			yield return new MigrationAddin(this.m_TiaPortal);
			yield break;
		}

		private readonly TiaPortal m_TiaPortal;
	}
}
