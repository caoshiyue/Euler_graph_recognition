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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using generate_graph;
using MathWorks.MATLAB.NET.Arrays;
using System.Diagnostics;

namespace discrete_mathematics3
{
    /// <summary>
    /// Windows operate API
    /// </summary>
    public class Win32API
    {
        #region Win32 API
        [DllImport("user32.dll", EntryPoint = "GetWindowThreadProcessId", SetLastError = true,
             CharSet = CharSet.Unicode, ExactSpelling = true,
             CallingConvention = CallingConvention.StdCall)]
        public static extern long GetWindowThreadProcessId(long hWnd, long lpdwProcessId);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongA", SetLastError = true)]
        public static extern long GetWindowLong(IntPtr hwnd, int nIndex);

        public static IntPtr SetWindowLong(HandleRef hWnd, int nIndex, int dwNewLong)
        {
            if (IntPtr.Size == 4)
            {
                return SetWindowLongPtr32(hWnd, nIndex, dwNewLong);
            }
            return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLong", CharSet = CharSet.Auto)]
        public static extern IntPtr SetWindowLongPtr32(HandleRef hWnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", CharSet = CharSet.Auto)]
        public static extern IntPtr SetWindowLongPtr64(HandleRef hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern long SetWindowPos(IntPtr hwnd, long hWndInsertAfter, long x, long y, long cx, long cy, long wFlags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool MoveWindow(IntPtr hwnd, int x, int y, int cx, int cy, bool repaint);

        [DllImport("user32.dll", EntryPoint = "PostMessageA", SetLastError = true)]
        public static extern bool PostMessage(IntPtr hwnd, uint Msg, uint wParam, uint lParam);

        /// <summary>
        /// 获取系统错误信息描述
        /// </summary>
        /// <param name="errCode">系统错误码</param>
        /// <returns></returns>
        public static string GetLastError()
        {
            var errCode = Marshal.GetLastWin32Error();
            IntPtr tempptr = IntPtr.Zero;
            string msg = null;
            FormatMessage(0x1300, ref tempptr, errCode, 0, ref msg, 255, ref tempptr);
            return msg;
        }
        /// <summary>
        /// 获取系统错误信息描述
        /// </summary>
        /// <param name="errCode">系统错误码</param>
        /// <returns></returns>
        public static string GetLastErrorString(int errCode)
        {
            IntPtr tempptr = IntPtr.Zero;
            string msg = null;
            FormatMessage(0x1300, ref tempptr, errCode, 0, ref msg, 255, ref tempptr);
            return msg;
        }

        [System.Runtime.InteropServices.DllImport("Kernel32.dll")]
        public extern static int FormatMessage(int flag, ref IntPtr source, int msgid, int langid, ref string buf, int size, ref IntPtr args);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetParent(IntPtr hwnd);

        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        public const int SWP_NOOWNERZORDER = 0x200;
        public const int SWP_NOREDRAW = 0x8;
        public const int SWP_NOZORDER = 0x4;
        public const int SWP_SHOWWINDOW = 0x0040;
        public const int WS_EX_MDICHILD = 0x40;
        public const int SWP_NOACTIVATE = 0x10;
        public const int SWP_ASYNCWINDOWPOS = 0x4000;
        public const int SWP_NOMOVE = 0x2;
        public const int SWP_NOSIZE = 0x1;
        public const int WM_CLOSE = 0x10;

        public const int SW_HIDE = 0; //{隐藏, 并且任务栏也没有最小化图标}
        public const int SW_SHOWNORMAL = 1; //{用最近的大小和位置显示, 激活}
        public const int SW_NORMAL = 1; //{同 SW_SHOWNORMAL}
        public const int SW_SHOWMINIMIZED = 2; //{最小化, 激活}
        public const int SW_SHOWMAXIMIZED = 3; //{最大化, 激活}
        public const int SW_MAXIMIZE = 3; //{同 SW_SHOWMAXIMIZED}
        public const int SW_SHOWNOACTIVATE = 4; //{用最近的大小和位置显示, 不激活}
        public const int SW_SHOW = 5; //{同 SW_SHOWNORMAL}
        public const int SW_MINIMIZE = 6; //{最小化, 不激活}
        public const int SW_SHOWMINNOACTIVE = 7; //{同 SW_MINIMIZE}
        public const int SW_SHOWNA = 8; //{同 SW_SHOWNOACTIVATE}
        public const int SW_RESTORE = 9; //{同 SW_SHOWNORMAL}
        public const int SW_SHOWDEFAULT = 10; //{同 SW_SHOWNORMAL}
        public const int SW_MAX = 10; //{同 SW_SHOWNORMAL}

        //const int PROCESS_ALL_ACCESS = 0x1F0FFF;
        //const int PROCESS_VM_READ = 0x0010;
        //const int PROCESS_VM_WRITE = 0x0020;     

        internal const int
            GWL_WNDPROC = (-4),
            GWL_HINSTANCE = (-6),
            GWL_HWNDPARENT = (-8),
            GWL_STYLE = (-16),
            GWL_EXSTYLE = (-20),
            GWL_USERDATA = (-21),
            GWL_ID = (-12);
        internal const int
              WS_CHILD = 0x40000000,
              WS_VISIBLE = 0x10000000,
              LBS_NOTIFY = 0x00000001,
              HOST_ID = 0x00000002,
              LISTBOX_ID = 0x00000001,
              WS_VSCROLL = 0x00200000,
              WS_BORDER = 0x00800000;
        private const int HWND_TOP = 0x0;
        private const int WM_COMMAND = 0x0112;
        private const int WM_QT_PAINT = 0xC2DC;
        private const int WM_PAINT = 0x000F;
        private const int WM_SIZE = 0x0005;
        private const int SWP_FRAMECHANGED = 0x0020;
        #endregion Win32 API
    }

    /// <summary>
    /// Window embed functions
    /// </summary>
    public partial class PluginContainer : ContentControl
    {
        public PluginContainer(string appFileName, IntPtr hostHandle)
        {
            _appFilename = appFileName;
            _hostWinHandle = hostHandle;
        }
        private bool getwindows()
        {
            try
            {
                tarwinhandle = IntPtr.Zero;
                while (tarwinhandle == IntPtr.Zero)
                {
                    System.Threading.Thread.Sleep(5);
                    tarwinhandle = Win32API.FindWindow(null, _appFilename);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool EmbedProcess(int width, int height, Point p)
        {
            getwindows();
            try
            {
                var pluginWinHandle = tarwinhandle;//Get the handle of main window.
                embedResult = Win32API.SetParent(pluginWinHandle, _hostWinHandle);//set parent window
                Win32API.SetWindowLong(new HandleRef(this, pluginWinHandle), Win32API.GWL_STYLE, Win32API.WS_VISIBLE);//Set window style to "None".
                var moveResult = Win32API.MoveWindow(pluginWinHandle, (int)p.X, (int)p.Y, width, height, true);//Move window to fixed position(up-left is (0,0), and low-right is (width, height)).
                if (!moveResult || embedResult == 0)
                {

                    if (MAXCOUNT-- > 0)
                    {
                        EmbedProcess(width, height, p);
                    }
                }
                else
                {
                    Win32API.ShowWindow(pluginWinHandle, (short)Win32API.SW_NORMAL);
                }
            }
            catch (Exception ex)
            {
                var errorString = Win32API.GetLastError();
                MessageBox.Show(errorString + ex.Message);
            }
            return (embedResult != 0);
        }
        #region
        public int embedResult = 0;
        private IntPtr _hostWinHandle { get; set; }
        public IntPtr tarwinhandle;
        private string _appFilename = "";
        private int MAXCOUNT = 10;
        #endregion
    }
    /// <summary>
    /// C++ Fleury functions
    /// </summary>
    public class Eulardll
    {
        [DllImport("EularPath.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void initadj(int v, ref int a);

        [DllImport("EularPath.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern Int32 getpath(ref IntPtr s);
    }
}
