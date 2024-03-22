using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Input;

namespace SeagullClicker
{
	[StructLayout(LayoutKind.Sequential)]
	public struct win32Point
	{
		public int X;
		public int Y;
		public win32Point(int x, int y)
		{
			this.X = x;
			this.Y = y;
		}
	}

	class Mouse
	{
		[DllImport("user32.dll")]
		private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

		[DllImport("user32.dll")]
		static extern int SetCursorPos(int x, int y);

		[DllImport("user32.dll")]
		public static extern bool GetCursorPos(out win32Point lpPoint);

		private const int MOUSEEVENTF_LEFTDOWN = 0x02;
		private const int MOUSEEVENTF_LEFTUP = 0x04;
		private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
		private const int MOUSEEVENTF_RIGHTUP = 0x10;

		public static void MouseEvent(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo)
		{
			mouse_event(dwFlags, dx, dy, cButtons, dwExtraInfo);
		}

		public static void LeftClick(int x, int y)
		{
			SetCursorPos(x, y);
			MouseEvent(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
		}

		public static void RightClick(int x, int y)
		{
			SetCursorPos(x, y);
			MouseEvent(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
		}
	}
}
