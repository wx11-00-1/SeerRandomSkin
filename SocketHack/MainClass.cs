using EasyHook;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Seer;
using System.Drawing;
using System.Net.Sockets;
using System.Windows.Forms;

namespace SocketHack
{
    public class MainClass : EasyHook.IEntryPoint
    {
        private static LocalHook lhRecv, lhWSASend;
        public static IntPtr hGameWnd;
        public static FormScreenShot childFormScreenShot;

        public MainClass(EasyHook.RemoteHooking.IContext context, IntPtr handle)
        {
            hGameWnd = handle;
        }

        #region WSASend

        public unsafe struct WSABUF
        {
            public Int32 len;
            public IntPtr buf;
        }

        [DllImport("ws2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        private unsafe static extern Int32 WSASend(Int32 Socket, IntPtr lpBuffers, UInt32 dwBufferCount, IntPtr lpNumberOfBytesSent, UInt32 dwFlags, IntPtr lpOverlapped, IntPtr lpCompletionRoutine);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        unsafe delegate Int32 DWSASend(Int32 Socket, IntPtr lpBuffers, UInt32 dwBufferCount, IntPtr lpNumberOfBytesSent, UInt32 dwFlags, IntPtr lpOverlapped, IntPtr lpCompletionRoutine);

        private static unsafe Int32 WSASend_Hook(Int32 Socket, IntPtr lpBuffers, UInt32 dwBufferCount, IntPtr lpNumberOfBytesSent, UInt32 dwFlags, IntPtr lpOverlapped, IntPtr lpCompletionRoutine)
        {
            Int32 res = WSASend(Socket, lpBuffers, dwBufferCount, lpNumberOfBytesSent, dwFlags, lpOverlapped, lpCompletionRoutine);

            WSABUF wsBuffer;
            wsBuffer = (WSABUF)Marshal.PtrToStructure(lpBuffers, typeof(WSABUF));

            // wsBuffer 的长度是不正确的，要读封包头
            byte[] packHead = new byte[4];
            Marshal.Copy(wsBuffer.buf, packHead, 0, 4);
            if (packHead[0] != 0 || packHead[1] != 0) return res;
            int len = (packHead[0] << 24) + (packHead[1] << 16) + (packHead[2] << 8) + packHead[3];

            byte[] temp = new byte[len];
            Marshal.Copy(wsBuffer.buf, temp, 0, len);
            Packet.ProcessingSendPacket(Socket, temp, len);

            return res;
        }

        #endregion

        #region recv

        [DllImport("WS2_32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        private unsafe static extern Int32 recv(Int32 socket, IntPtr buffer, Int32 length, Int32 flags);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        unsafe delegate Int32 Drecv(Int32 socket, IntPtr buffer, Int32 length, Int32 flags);

        private static unsafe Int32 Recv_Hook(Int32 socket, IntPtr buffer, Int32 length, Int32 flags)
        {
            Int32 res = recv(socket, buffer, length, flags);

            if (res <= 0 || socket != Packet.Socket) return res;

            byte[] temp = new byte[res];
            Marshal.Copy(buffer, temp, 0, res);
            Packet.ProcessingRecvPacket(socket, temp, res);

            return res;
        }

        #endregion

        #region 开始拦截
        public static void StartHook()
        {
            try
            {
                lhRecv = LocalHook.Create(LocalHook.GetProcAddress("WS2_32.dll", "recv"), new Drecv(Recv_Hook), null);
                lhRecv.ThreadACL.SetExclusiveACL(new Int32[] { 0 });

                lhWSASend = LocalHook.Create(LocalHook.GetProcAddress("WS2_32.dll", "WSASend"), new DWSASend(WSASend_Hook), null);
                lhWSASend.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        public void Run(RemoteHooking.IContext context, IntPtr handle)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            childFormScreenShot = new FormScreenShot();
            Application.Run(childFormScreenShot);
        }
    }
}
