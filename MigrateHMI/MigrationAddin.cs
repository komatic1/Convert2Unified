using System;
using System.Collections.Generic;
using System.IO;
using Siemens.Engineering;
using Siemens.Engineering.AddIn;
using Siemens.Engineering.AddIn.Menu;
using Siemens.Engineering.AddIn.Utilities;
using Siemens.Engineering.Hmi;
using Siemens.Engineering.HmiUnified;
using Siemens.Engineering.HW;
using Siemens.Engineering.HW.Features;

namespace MigrateHMI
{
	public class MigrationAddin : ContextMenuAddIn
    {	
		// 
		public MigrationAddin(TiaPortal tiaPortal) : base("Migration2Unified")
		{
            // 
            if (tiaPortal == null)
			{
				throw new ArgumentNullException("tiaPortal");
			}
			m_TiaPortal = tiaPortal;
		}
        
		protected override void BuildContextMenuItems(ContextMenuAddInRoot addInRootSubmenu)
		{
			addInRootSubmenu.Items.AddActionItem<Device>("RunIt", new ActionItem<Device>.OnClickDelegate(this.OnClick_LaunchUI));
            addInRootSubmenu.Items.AddActionItem<Device>("RunIt", new ActionItem<Device>.OnClickDelegate(this.OnClick_LaunchUI));

        }
        private void OnClick_LaunchUI(MenuSelectionProvider<Device> selectionProvider)
		{
			
            string text = this.WriteApplicationToTempFolder(this.exeName, this.dllNames);
			IEnumerable<Device> selection = selectionProvider.GetSelection<Device>();
			string text2 = this.GetHmiTarget(selection);
			text2 = text2 + " -P \"" + this.m_TiaPortal.GetCurrentProcess().Id.ToString() + "\"";
			text2 = text2 + " -A \"" + this.m_TiaPortal.GetCurrentProcess().Path.ToString() + "\"";
			Process.Start(text, text2);
		}

		private string WriteApplicationToTempFolder(string exeResource, string[] referenceResources = null)
		{
			string temporaryDirectory = this.GetTemporaryDirectory();
			File.WriteAllBytes(Path.Combine(temporaryDirectory, this.GetFileNameFromResource(exeResource)), this.GetResourceStream(exeResource));
			if (referenceResources != null)
			{
				foreach (string text in referenceResources)
				{
					File.WriteAllBytes(Path.Combine(temporaryDirectory, this.GetFileNameFromResource(text)), this.GetResourceStream(text));
				}
			}
			return Path.Combine(temporaryDirectory, this.GetFileNameFromResource(exeResource));
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00003462 File Offset: 0x00001662
		private byte[] GetResourceStream(string name)
		{
			BinaryReader binaryReader = new BinaryReader(base.GetType().Assembly.GetManifestResourceStream(name));
			return binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x0000348B File Offset: 0x0000168B
		private string GetTemporaryDirectory()
		{
			string text = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
			Directory.CreateDirectory(text);
			return text;
		}

		// Token: 0x06000047 RID: 71 RVA: 0x000034A4 File Offset: 0x000016A4
		private string GetFileNameFromResource(string resourceName)
		{
			string text = "MigrateHMI.OpennessExe.";
			return resourceName.Substring(text.Length);
		}

		// Token: 0x06000048 RID: 72 RVA: 0x000034C4 File Offset: 0x000016C4
		private string GetHmiTarget(IEnumerable<Device> devices)
		{
			string text = string.Empty;
			foreach (Device device in devices)
			{
				DeviceItemComposition deviceItems = device.DeviceItems;
				if (deviceItems != null)
				{
					foreach (DeviceItem deviceItem in deviceItems)
					{
						SoftwareContainer service = deviceItem.GetService<SoftwareContainer>();
						if (service != null)
						{
							HmiSoftware hmiSoftware = service.Software as HmiSoftware;
							if (service.Software is HmiTarget)
							{
								text = text + " -s \"" + device.Name + "\"";
							}
							else if (hmiSoftware != null)
							{
								text = text + " -t \"" + device.Name + "\"";
							}
						}
					}
				}
			}
			return text;
		}

		// Token: 0x0400001E RID: 30
		private readonly TiaPortal m_TiaPortal;

		// Token: 0x0400001F RID: 31
		private readonly string exeName = "MigrateHMI.OpennessExe.Data2UnifiedTool.exe";

		// Token: 0x04000020 RID: 32
		private readonly string[] dllNames = new string[]
		{
			"MigrateHMI.OpennessExe.BusinessLogic.dll",
			"MigrateHMI.OpennessExe.ClientLogic.dll",
			"MigrateHMI.OpennessExe.Logger.dll",
			"MigrateHMI.OpennessExe.UserInterface.dll",
			"MigrateHMI.OpennessExe.ViewModel.dll",
			"MigrateHMI.OpennessExe.Progress.dll"
		};
	}
}
