using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace i2cdotnet
{
    public class I2c
    {
        protected const Int32 I2C_RETRIES = 0x0701;
        protected const Int32 I2C_TIMEOUT = 0x0702;
        protected const Int32 OPEN_READ_WRITE = 2;

        [DllImport("libc.so.6", EntryPoint = "open", SetLastError = true)]
        static extern int _Open(string fd, int mode);
        public static int Open(string device, int mode)
        {
            int fd = _Open(device, mode);
            if (fd == 0) fd = -1;
            CheckFunction(fd);

            return fd;
        }

        [DllImport("libc.so.6", EntryPoint = "close", SetLastError = true)]
        static extern int _Close(int fd);
        public static void Close(int fd)
        {
            CheckFunction(_Close(fd));
        }

        [DllImport("i2c-dotnet.so", EntryPoint = "reg_read_byte", SetLastError = true)]
        private static extern int _RegReadByte(int fd, int devaddr, int regaddr, ref int content);
        public static void RegReadByte(int fd, int devaddr, int regaddr, ref int content)
        {
            CheckFunction(_RegReadByte(fd, devaddr, regaddr, ref content));
        }

        [DllImport("i2c-dotnet.so", EntryPoint = "reg_write_byte", SetLastError = true)]
        static extern Int32 _RegWriteByte(int fd, sbyte addr, sbyte cmd, sbyte val);
        public static void RegWriteByte(int fd, Int16 addr, Int16 cmd, Int16 val)
        {
            sbyte _addr = (sbyte)addr;
            sbyte _cmd = (sbyte)cmd;
            sbyte _val = (sbyte)val;

            CheckFunction(_RegWriteByte(fd, _addr, _cmd, _val));
        }

        [DllImport("i2c-dotnet.so", EntryPoint = "reg_read_byte", SetLastError = true)]
        static extern Int32 _RegReadByte(int fd, sbyte addr, sbyte cmd, ref sbyte val);
        public static void RegReadByte(int fd, sbyte addr, sbyte cmd, ref sbyte val)
        {
            CheckFunction(_RegReadByte(fd, addr, cmd, ref val));
        }

        [DllImport("i2c-dotnet.so", EntryPoint = "reg_read_short", SetLastError = true)]
        static extern Int32 _RegReadShort(int fd, sbyte addr, sbyte cmd, ref Int16 val);
        public static void RegReadShort(int fd, Int16 addr, Int16 cmd, ref Int16 val)
        {
            sbyte _addr = (sbyte)addr;
            sbyte _cmd = (sbyte)cmd;

            CheckFunction(_RegReadShort(fd, _addr, _cmd, ref val));
        }

        [DllImport("i2c-dotnet.so", EntryPoint = "reg_read_short", SetLastError = true)]
        static extern Int32 _RegReadShort(int fd, sbyte addr, sbyte cmd, ref UInt16 val);
        public static void RegReadShort(int fd, Int16 addr, UInt16 cmd, ref UInt16 val)
        {
            sbyte _addr = (sbyte)addr;
            sbyte _cmd = (sbyte)cmd;

            CheckFunction(_RegReadShort(fd, _addr, _cmd, ref val));
        }

        [DllImport("i2c-dotnet.so", EntryPoint = "reg_read_24", SetLastError = true)]
        static extern Int32 _RegRead24(int fd, sbyte addr, sbyte cmd, ref Int32 val);
        public static void RegRead24(int fd, sbyte addr, Int16 cmd, ref Int16 val)
        {
            sbyte _addr = (sbyte)addr;
            sbyte _cmd = (sbyte)cmd;
            Int32 _val = (Int32)val;

            CheckFunction(_RegRead24(fd, _addr, _cmd, ref _val));
        }

        [DllImport("i2c-dotnet.so", EntryPoint = "reg_write_bytes", SetLastError = true)]
        static extern Int32 _RegWriteBytes(int fd, sbyte addr, Int16[] content, int size);
        public static void RegWriteBytes(int fd, sbyte addr, Int16 cmd, Int16[] content)
        {
            sbyte _addr = (sbyte)addr;
            Int16[] buf = new Int16[content.Length + 1];

            buf[0] = cmd;
            for (Int16 i = 0; i < content.Length; i++)
            {
                buf[i + 1] = content[i];
            }

            CheckFunction(_RegWriteBytes(fd, _addr, buf, (Int16)buf.Length));
        }

        static void CheckFunction(int rc)
        {
            StackTrace stackTrace = new StackTrace();
            var FuncName = stackTrace.GetFrame(1).GetMethod().Name;

            if (rc < 0)
            {
                var name = System.Reflection.MethodBase.GetCurrentMethod().Name;
                var error = String.Format("Error:{0} {1}", FuncName, rc);
                throw new Exception(error);
            }
        }
    }
}