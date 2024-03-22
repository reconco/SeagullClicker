using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Interop;

namespace SeagullClicker
{
	class KeyInput
	{
		private const int HOTKEY_ID = 9000;

		//Modifiers:
		private const uint MOD_NONE = 0x0000; //(none)
		private const uint MOD_ALT = 0x0001; //ALT
		private const uint MOD_CONTROL = 0x0002; //CTRL
		private const uint MOD_SHIFT = 0x0004; //SHIFT
		private const uint MOD_WIN = 0x0008; //WINDOWS

		//CAPS LOCK:
		private const uint VK_CAPITAL = 0x14;

		[DllImport("user32.dll")]
		private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

		[DllImport("user32.dll")]
		private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

		private MainWindow _mainWindow;
		private IntPtr _windowHandle;
		private HwndSource _source;

		public void SetMainWindow(MainWindow mainWindow)
		{
			_mainWindow = mainWindow;
		}

		public void RegisterHotkey(IntPtr _windowHandle)
		{
			this._windowHandle = _windowHandle;
			_source = HwndSource.FromHwnd(_windowHandle);
			_source.AddHook(HwndHook);

			RegisterHotKey(_windowHandle, HOTKEY_ID, MOD_CONTROL | MOD_SHIFT, (uint)VirtualKeys.VK_1);
			RegisterHotKey(_windowHandle, HOTKEY_ID, MOD_CONTROL | MOD_SHIFT, (uint)VirtualKeys.VK_2);
			RegisterHotKey(_windowHandle, HOTKEY_ID, MOD_CONTROL | MOD_SHIFT, (uint)VirtualKeys.VK_3);
		}

		private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			const int WM_HOTKEY = 0x0312;
			switch (msg)
			{
				case WM_HOTKEY:
					switch (wParam.ToInt32())
					{
						case HOTKEY_ID:
							int vkey = (((int)lParam >> 16) & 0xFFFF);

							if (vkey == (uint)VirtualKeys.VK_1)
							{
								if(_mainWindow.CurrentMode == MainWindow.ClickerMode.Timer)
									return IntPtr.Zero;

								//Set position
								win32Point point = new win32Point();
								Mouse.GetCursorPos(out point);

								_mainWindow.SetClickTargetPoint(point.X, point.Y);
							}
							else if (vkey == (uint)VirtualKeys.VK_2)
							{
								//Save
								_mainWindow.saveButton_Click(this, null);
							}
							else if (vkey == (uint)VirtualKeys.VK_3)
							{
								//Start or Stop
								_mainWindow.startButton_Click(this, null);
							}

							handled = true;
							break;
					}
					break;
			}
			return IntPtr.Zero;
		}

		public void UnregisterHotkey()
		{
			_source.RemoveHook(HwndHook);
			UnregisterHotKey(_windowHandle, HOTKEY_ID);
		}
	}
}
