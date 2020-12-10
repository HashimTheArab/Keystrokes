using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.ComponentModel;

namespace Utilities
{
	/// A class that manages a global low level keyboard hook
	class globalKeyboardHook
	{
		/// defines the callback type for the hook
		public delegate int keyboardHookProc(int code, int wParam, ref keyboardHookStruct lParam);

		public struct keyboardHookStruct
		{
			public int vkCode;
			public int scanCode;
			public int flags;
			public int time;
			public int dwExtraInfo;
		}
		private static keyboardHookProc callbackDelegate;
		const int WH_KEYBOARD_LL = 13;
		const int WM_KEYDOWN = 0x100;
		const int WM_KEYUP = 0x101;
		const int WM_SYSKEYDOWN = 0x104;
		const int WM_SYSKEYUP = 0x105;

		/// The collections of keys to watch for
		public List<Keys> HookedKeys = new List<Keys>();

		/// Handle to the hook, need this to unhook and call the next hook
		IntPtr hhook = IntPtr.Zero;

		/// Occurs when one of the hooked keys is pressed
		public event KeyEventHandler KeyDown;

		/// Occurs when one of the hooked keys is released
		public event KeyEventHandler KeyUp;

		/// Initializes a new instance of the <see cref="globalKeyboardHook"/> class and installs the keyboard hook.
		public globalKeyboardHook()
		{
			hook();
		}

		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="globalKeyboardHook"/> is reclaimed by garbage collection and uninstalls the keyboard hook.
		~globalKeyboardHook()
		{
			unhook();
		}

		public void hook()
		{
			if (callbackDelegate != null) throw new InvalidOperationException("Can't hook more than once");
			IntPtr hInstance = LoadLibrary("User32");
			callbackDelegate = new keyboardHookProc(hookProc);
			hhook = SetWindowsHookEx(WH_KEYBOARD_LL, callbackDelegate, hInstance, 0);
			if (hhook == IntPtr.Zero) throw new Win32Exception();
		}

		/// Uninstalls the global hook
		public void unhook()
		{
			if (callbackDelegate == null) return;
			bool ok = UnhookWindowsHookEx(hhook);
			if (!ok) throw new Win32Exception();
			callbackDelegate = null;
		}

		/// The callback for the keyboard hook
		public int hookProc(int code, int wParam, ref keyboardHookStruct lParam)
		{
			if (code >= 0)
			{
				Keys key = (Keys)lParam.vkCode;
				if (HookedKeys.Contains(key))
				{
					KeyEventArgs kea = new KeyEventArgs(key);
					if ((wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN) && (KeyDown != null))
					{
						KeyDown(this, kea);
					}
					else if ((wParam == WM_KEYUP || wParam == WM_SYSKEYUP) && (KeyUp != null))
					{
						KeyUp(this, kea);
					}
					if (kea.Handled)
						return 1;
				}
			}
			return CallNextHookEx(hhook, code, wParam, ref lParam);
		}

		/// Sets the windows hook, do the desired event, one of hInstance or threadId must be non-null
		[DllImport("user32.dll")]
		static extern IntPtr SetWindowsHookEx(int idHook, keyboardHookProc callback, IntPtr hInstance, uint threadId);

		/// Unhooks the windows hook.
		[DllImport("user32.dll")]
		static extern bool UnhookWindowsHookEx(IntPtr hInstance);

		/// Calls the next hook.
		[DllImport("user32.dll")]
		static extern int CallNextHookEx(IntPtr idHook, int nCode, int wParam, ref keyboardHookStruct lParam);

		/// Loads the library.
		[DllImport("kernel32.dll")]
		static extern IntPtr LoadLibrary(string lpFileName);
	}
}
