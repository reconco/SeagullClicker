using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeagullClicker
{
	public partial class MainWindow
	{
		public static readonly string SAVE_FILE_PATH = "save.save";
		public static SaveFile saveFile = new SaveFile(SAVE_FILE_PATH);

		private bool LoadSaveData()
		{
			bool loaded = saveFile.OpenSaveFile();
			if (loaded)
			{
				_clickTargetPoint.X = (int)saveFile.GetData("pointX", 0);
				_clickTargetPoint.Y = (int)saveFile.GetData("pointY", 0);
				_settedClickTime = (int)saveFile.GetData("delayClickTime", 5);
			}
			//else
			//	SaveSettingData();

			return loaded;
		}

		public void SaveSettingData()
		{
			saveFile.SetData("pointX", _clickTargetPoint.X);
			saveFile.SetData("pointY", _clickTargetPoint.Y);
			saveFile.SetData("delayClickTime", _settedClickTime);

			saveFile.SaveThisFile();
		}
	}
}
