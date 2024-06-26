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
        public static FormPack childFormScreenShot;

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

        // CEF 部分版本的特色，发送封包有概率一个包分开两截发。处理方式，例如：
        //        【原封包】断成3截发送，总长度为50：                     | 12 | 24 | 14 |
        // 用来【占位的包】长度为18，发送的方式如下：
        //          第一截长度12，将1个占位包分开两次发送：| 12 | | 6 |
        //          第二截长度24，将两个占位包拼接起来发送：| 24 | | 12 |
        //          最后一截：| 14 |
        //        发送的时候将原封包内容存放到另一个缓冲区，解析后单独发送：| 50 |
        private static string pack_4047_str = "00 00 00 11 31 00 00 0f cf 00 00 00 00 00 00 01 ff";
        private static byte[] pack_4047 = Misc.HexString2ByteArray(pack_4047_str);
        private static int pack_4047_after_encrypt_len = 18;
        private static byte[] sendBufferFragment;
        private static byte[] sendBufferMock;
        private const int NO_FRAGMENT_PACK = -1;
        private const int PACK_FIRST_FRAGMENT_LESS_THAN_4 = -2;
        private static int fragmentLen = NO_FRAGMENT_PACK;

        private static void FillMockPackBuffer(int originPackLen)
        {
            int repeatTimes = (originPackLen - 1) / pack_4047_after_encrypt_len + 1;
            sendBufferMock = new byte[repeatTimes * pack_4047_after_encrypt_len];
            for(int i = 0, j = 0; i < repeatTimes; ++i, j += pack_4047_after_encrypt_len)
            {
                Array.Copy(Packet.ProcessingSendPacket(Packet.Socket, Packet.encrypt(pack_4047)), 0, sendBufferMock, j, pack_4047_after_encrypt_len);
            }
            // FormPack.ActionShowMsg($"占位包已就绪，长度为 {sendBufferMock.Length}");
        }

        /// <summary>
        /// 调用 WSASend 发包（只能在登录后使用）
        /// </summary>
        /// <param name="pack">密文包</param>
        public static unsafe void SendByteArr(byte[] pack)
        {
            IntPtr pBuffer = Marshal.AllocHGlobal(sizeof(WSABUF));
            WSABUF wsBuffer;
            wsBuffer.buf = Misc.BytesToIntptr(pack);
            wsBuffer.len = pack.Length;
            Marshal.StructureToPtr(wsBuffer, pBuffer, false);
            WSASend(Packet.Socket, pBuffer, 1, IntPtr.Zero, 0, IntPtr.Zero, IntPtr.Zero);
            Marshal.FreeHGlobal(wsBuffer.buf);
            Marshal.FreeHGlobal(pBuffer);
        }

        private static unsafe Int32 WSASend_Hook(Int32 Socket, IntPtr lpBuffers, UInt32 dwBufferCount, IntPtr lpNumberOfBytesSent, UInt32 dwFlags, IntPtr lpOverlapped, IntPtr lpCompletionRoutine)
        {
            WSABUF wsBuffer;
            wsBuffer = (WSABUF)Marshal.PtrToStructure(lpBuffers, typeof(WSABUF));

            byte[] packHead = new byte[4];
            Marshal.Copy(wsBuffer.buf, packHead, 0, 4);

            if (fragmentLen != NO_FRAGMENT_PACK)
            {
                // 断包
                // 与缓冲区中现有的包拼接
                byte[] tmpPack = new byte[wsBuffer.len];
                Marshal.Copy(wsBuffer.buf, tmpPack, 0, wsBuffer.len);
                sendBufferFragment = Misc.ArrayMerge(sendBufferFragment, tmpPack);
                if (fragmentLen == PACK_FIRST_FRAGMENT_LESS_THAN_4)
                {
                    // 封包断在代表封包长度的4个字节
                    // 拼接上这次发送的封包，应该足够了，总不至于就针对这4个字节断1次以上
                    fragmentLen = (sendBufferFragment[0] << 24) + (sendBufferFragment[1] << 16) + (sendBufferFragment[2] << 8) + sendBufferFragment[3];
                }
                tmpPack = new byte[wsBuffer.len];
                FillMockPackBuffer(wsBuffer.len);
                Array.Copy(sendBufferMock, tmpPack, wsBuffer.len);
                wsBuffer.buf = Misc.BytesToIntptr(tmpPack);
                Marshal.StructureToPtr(wsBuffer, lpBuffers, false);
                int result = WSASend(Socket, lpBuffers, dwBufferCount, lpNumberOfBytesSent, dwFlags, lpOverlapped, lpCompletionRoutine);
                FormPack.ActionShowMsg($"发送断包，长度为 {wsBuffer.len}");
                // 如有剩余的填充包，就先发完那一部分
                int remainsLen = sendBufferMock.Length - wsBuffer.len;
                if (remainsLen > 0)
                {
                    tmpPack = new byte[remainsLen];
                    Array.Copy(sendBufferMock, wsBuffer.len, tmpPack, 0, remainsLen);
                    SendByteArr(tmpPack);
                    // FormPack.ActionShowMsg($"发送剩余占位包，长度为 {remainsLen}");
                }
                if (sendBufferFragment.Length == fragmentLen)
                {
                    // 原封包保存完整了
                    // 发送原封包
                    SendByteArr(Packet.ProcessingSendPacket(Socket, sendBufferFragment));
                    fragmentLen = NO_FRAGMENT_PACK;
                    FormPack.ActionShowMsg("封包已补发");
                }
                return result;
            }
            else
            {
                if (packHead[0] != 0 || packHead[1] != 0) return WSASend(Socket, lpBuffers, dwBufferCount, lpNumberOfBytesSent, dwFlags, lpOverlapped, lpCompletionRoutine);
            }

            int len = (packHead[0] << 24) + (packHead[1] << 16) + (packHead[2] << 8) + packHead[3];
            if (len != wsBuffer.len)
            {
                FormPack.ActionShowMsg($"出现断包【{wsBuffer.len} / {len}】");
                // 保存原封包内容
                sendBufferFragment = new byte[wsBuffer.len];
                Marshal.Copy(wsBuffer.buf, sendBufferFragment, 0, wsBuffer.len);
                // 用无关紧要的包代替
                // 等到解析完整原本要发的包，再重新发送
                byte[] tmpPack = new byte[wsBuffer.len];
                if (wsBuffer.len >= 4)
                {
                    fragmentLen = len;
                    FillMockPackBuffer(wsBuffer.len);
                    Array.Copy(sendBufferMock, tmpPack, wsBuffer.len);
                }
                else
                {
                    fragmentLen = PACK_FIRST_FRAGMENT_LESS_THAN_4;
                    FillMockPackBuffer(1);
                    Array.Copy(pack_4047, tmpPack, wsBuffer.len);
                }
                wsBuffer.buf = Misc.BytesToIntptr(tmpPack);
                Marshal.StructureToPtr(wsBuffer, lpBuffers, false);
                var result = WSASend(Socket, lpBuffers, dwBufferCount, lpNumberOfBytesSent, dwFlags, lpOverlapped, lpCompletionRoutine);
                Marshal.FreeHGlobal(wsBuffer.buf);
                // 发送占位包的剩余字节
                int remainsLen = sendBufferMock.Length - wsBuffer.len;
                if (remainsLen > 0)
                {
                    var remainsPack = new byte[remainsLen];
                    Array.Copy(sendBufferMock, wsBuffer.len, remainsPack, 0, remainsLen);
                    SendByteArr(remainsPack);// 00 00 00 45 31 00 00 08 35 37 A0 10 94 00 00 02 60 00 00 00 00 00 00 02 88 00 00 01 A6 00 00 00 24 09 05 01 0A 23 01 03 78 03 79 05 40 89 A9 99 99 99 99 9A 05 40 77 B2 66 66 66 66 66 0A 01 04 85 00 04 83 24 
                }
                return result;
            }
            else
            {
                byte[] tmpPack = new byte[len];
                Marshal.Copy(wsBuffer.buf, tmpPack, 0, len);
                wsBuffer.buf = Misc.BytesToIntptr(Packet.ProcessingSendPacket(Socket, tmpPack));
                Marshal.StructureToPtr(wsBuffer, lpBuffers, false);
                var result = WSASend(Socket, lpBuffers, dwBufferCount, lpNumberOfBytesSent, dwFlags, lpOverlapped, lpCompletionRoutine);
                Marshal.FreeHGlobal(wsBuffer.buf);
                return result;
            }
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

            childFormScreenShot = new FormPack();
            Application.Run(childFormScreenShot);
        }
    }
}
