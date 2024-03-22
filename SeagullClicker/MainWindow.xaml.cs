using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Diagnostics;

namespace SeagullClicker
{
	/// <summary>
	/// MainWindow.xaml에 대한 상호 작용 논리
	/// </summary>
	/// 
	public partial class MainWindow : Window
	{
		public enum ClickerMode { Waiting, Timer}

		ClickerMode _currentMode = ClickerMode.Waiting;

		KeyInput _keyInput = new KeyInput();
		Stopwatch _stopwatch = new Stopwatch();
		DispatcherTimer _timer = null;
		const int TimerFrame = 12;

		win32Point _clickTargetPoint = new win32Point(0, 0);

		int _settedClickTime = 5;

		//bool _isRetargeting = false;

		public ClickerMode CurrentMode
		{
			get { return _currentMode; }
		}

		public MainWindow()
		{
			InitializeComponent();

			_keyInput.SetMainWindow(this);
			_timer = new DispatcherTimer();
			_timer.Interval = TimeSpan.FromMilliseconds(1000 / TimerFrame);
			_timer.Tick += OnTimerTick;
		}

		private void Window_SourceInitialized(object sender, EventArgs e)
		{
			timerNumberTextBox.OnTextChanged += OnTimerNumberTextChanged;

			LoadSaveData();
			pointX.Text = _clickTargetPoint.X.ToString();
			pointY.Text = _clickTargetPoint.Y.ToString();
			timerNumberTextBox.Text = _settedClickTime.ToString();

			_keyInput.RegisterHotkey(new WindowInteropHelper(this).Handle);

		}

		private void Window_Closed(object sender, EventArgs e)
		{
			_keyInput.UnregisterHotkey();
		}

		//private void retargetingButton_Click(object sender, RoutedEventArgs e)
		//{
		//	_isRetargeting = !_isRetargeting;

		//	if(_isRetargeting)
		//	{
		//		retargetingButton.Content = "좌표 지정 완료";
		//	}
		//	else
		//	{
		//		retargetingButton.Content = "좌표 재지정";
		//	}
		//}

		public void SetClickTargetPoint(int x, int y)
		{
			_clickTargetPoint.X = x;
			_clickTargetPoint.Y = y;

			pointX.Text = x.ToString();
			pointY.Text = y.ToString();
		}
		private void OnTimerNumberTextChanged(object sender, EventArgs e)
		{
			string text = timerNumberTextBox.Text;
			int temp = 0;
			if (int.TryParse(text, out temp))
			{
				_settedClickTime = temp;
				leftTimeText.Text = temp.ToString();
			}
		}

		public void saveButton_Click(object sender, RoutedEventArgs e)
		{
			SaveSettingData();
		}

		public void startButton_Click(object sender, RoutedEventArgs e)
		{
			switch(_currentMode)
			{
				case ClickerMode.Waiting:
					_currentMode = ClickerMode.Timer;
					timerNumberTextBox.IsEnabled = false;
					startButton.Content = "타이머 중지";
					leftTimeText.Foreground = Brushes.Firebrick;
					_stopwatch.Restart();
					_timer.Start();
					break;

				case ClickerMode.Timer:
					_currentMode = ClickerMode.Waiting;
					timerNumberTextBox.IsEnabled = true;
					startButton.Content = "타이머 시작";
					leftTimeText.Foreground = Brushes.Black;
					_stopwatch.Stop();
					_timer.Stop();
					break;

				default:
					break;
			}
		}


		private void OnTimerTick(object sender, EventArgs e)
		{
			const double Correction = 0.5;
			double leftTime = (double)_settedClickTime - (_stopwatch.Elapsed.TotalSeconds - Correction);
			double leftTimeForShow = leftTime + Correction;

			bool timeOver = false;
			if (leftTime <= 0)
			{
				timeOver = true;
				leftTime = 0;
				leftTimeForShow = 0;
			}

			leftTimeText.Text = ((int)leftTimeForShow).ToString();

			if(timeOver)
			{
				startButton_Click(this, null);
				Mouse.LeftClick(_clickTargetPoint.X, _clickTargetPoint.Y);
			}
		}

		private void topMostButton_Click(object sender, RoutedEventArgs e)
		{
			this.Topmost = !this.Topmost;
			if (this.Topmost)
			{
				topMostButton.Content = "프로그램 최상단 표시 끄기";
			}
			else if (!this.Topmost)
			{
				topMostButton.Content = "프로그램 최상단 표시";
			}
		}
	}
}
