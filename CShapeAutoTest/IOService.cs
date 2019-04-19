using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SDKContext;

namespace SDKIOService
{
    [StructLayout(LayoutKind.Sequential)]
    unsafe public struct elt_error
    {
        public int code;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public char[] err_msg;
    }
    enum errType
    {
        ELT_ERROR = -1,
        ELT_FAILURE,
        ELT_SUCCESS,
    }
    class IOService
    {
        //获取输入IO状态
        [DllImport("eltrobot.dll")]
        public static extern int elt_get_input(IntPtr ctx, int addr,ref int status, ref elt_error err);
        //获取输出IO状态
        [DllImport("eltrobot.dll")]
        public static extern int elt_get_output(IntPtr ctx, int addr, ref int status, ref elt_error err);
        //设置输出IO状态
        [DllImport("eltrobot.dll")]
        public static extern int elt_set_output(IntPtr ctx, int addr, int status, ref elt_error err);
        //获取虚拟输入IO状态
        [DllImport("eltrobot.dll")]
        public static extern int elt_get_virtual_input(IntPtr ctx, int addr, ref int status, ref elt_error err);
        //获取虚拟输出IO状态
        [DllImport("eltrobot.dll")]
        public static extern int elt_get_virtual_output(IntPtr ctx, int addr, ref int status, ref elt_error err);
        //设置虚拟输出IO状态
        [DllImport("eltrobot.dll")]
        public static extern int elt_set_virtual_output(IntPtr ctx, int addr, int status, ref elt_error err);
        public static string get_error_msg(char[] msg)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in msg)
            {
                sb.Append(c);
            }
            return sb.ToString();
        }
        bool outputRet(int type, elt_error err, string msg)
        {
            if ((errType)type == errType.ELT_ERROR)
            {
                Console.WriteLine("{0} error.", msg);
                Console.WriteLine("err.code = {0}, err.err_msg = {1}.", err.code, err.err_msg);
            }
            else if ((errType)type == errType.ELT_FAILURE)
            {
                Console.WriteLine("{0} failed.", msg);
                Console.WriteLine("err.code = {0}, err.err_msg = {1}.", err.code, err.err_msg);
            }
            else if ((errType)type == errType.ELT_SUCCESS)
            {
                return true;
            }
            else
            {
                Console.WriteLine("{0} other status.", msg);
                Console.WriteLine("err.code = {0}, err.err_msg = {1}.", err.code, err.err_msg);
            }
            return false;
        }
        /**
         * 获取输入IO状态
         */ 
         public bool getInput(IntPtr ctx, int addr, ref int status)
        {
            elt_error err = new elt_error();
            int ret = elt_get_input(ctx, addr, ref status, ref err);
            return outputRet(ret, err, "elt_get_input");
        }
        /**
         * 获取输出IO状态
         */ 
         public bool getOutput(IntPtr ctx, int addr, ref int status)
        {
            elt_error err = new elt_error();
            int ret = elt_get_output(ctx, addr, ref status, ref err);
            return outputRet(ret, err, "elt_get_output");
        }
        /**
         * 设置输出IO状态
         */ 
         public bool setOutput(IntPtr ctx, int addr, int value)
        {
            elt_error err = new elt_error();
            int ret = elt_set_output(ctx, addr, value, ref err);
            return outputRet(ret, err, "elt_set_output");
        }
        /**
         * 获取虚拟输入IO状态
         */ 
         public bool getVirtualInput(IntPtr ctx, int addr, ref int status)
        {
            elt_error err = new elt_error();
            int ret = elt_get_virtual_input(ctx, addr, ref status, ref err);
            return outputRet(ret, err, "elt_get_virtual_input");
        }
        /**
         * 获取虚拟输出IO状态
         */ 
         public bool getVirtualOutput(IntPtr ctx, int addr, ref int status)
        {
            elt_error err = new elt_error();
            int ret = elt_get_virtual_output(ctx, addr, ref status, ref err);
            return outputRet(ret, err, "elt_get_virtual_output");
        }
        /**
         * 设置虚拟输出IO状态
         */ 
         public bool setVirtualOutput(IntPtr ctx, int addr, int status)
        {
            elt_error err = new elt_error();
            int ret = elt_set_virtual_output(ctx, addr, status, ref err);
            return outputRet(ret, err, "elt_set_virtual_output");
        }
        /**
         * 获取输入IO状态测试
         */ 
         public bool getInputTest()
        {
            Context context = new Context();
            IntPtr ctx = context.SDKCreate();
            int status = new int();
            if (null == ctx)
                goto errlabel;
            if (!context.SDKLogin(ctx))
                goto errlabel;
            // digital input index -1边界测试
            if (getInput(ctx, -1, ref status))
                goto errlabel;
            Thread.Sleep(100);
            // digital input index 128边界测试
            if (getInput(ctx, 128, ref status))
                goto errlabel;
            Thread.Sleep(100);
            // analog output 1——4 测试
            for (int i = 0; i < 128; i++)
            {
                if (!getInput(ctx, i, ref status))
                    goto errlabel;
                Console.WriteLine("intput {0}, status = {1}", i, status);
                Thread.Sleep(100);
            }
            if (!context.SDKLogout(ctx))
            {
                goto errlabel;
            }
            if (!context.SDKDestroy(ctx))
                goto errlabel;
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test get digital intput success.");
            Console.WriteLine("=====================================================================");
            return true;
        errlabel:
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test get digital input failed.");
            Console.WriteLine("=====================================================================");
            return false;
        }
        /**
         * 获取输出IO状态测试
         */
        public bool getOutputTest()
        {
            Context context = new Context();
            IntPtr ctx = context.SDKCreate();
            int status = new int();
            if (null == ctx)
                goto errlabel;
            if (!context.SDKLogin(ctx))
                goto errlabel;
            // digital output index -1边界测试
            if (getOutput(ctx, -1, ref status))
                goto errlabel;
            Thread.Sleep(100);
            // digital output index 128边界测试
            if (getOutput(ctx, 128, ref status))
                goto errlabel;
            Thread.Sleep(100);
            // analog output 0——127 测试
            for (int i = 0; i < 128; i++)
            {
                if (!getOutput(ctx, i, ref status))
                    goto errlabel;
                Console.WriteLine("output {0}, status = {1}", i, status);
                Thread.Sleep(100);
            }
            if (!context.SDKLogout(ctx))
            {
                goto errlabel;
            }
            if (!context.SDKDestroy(ctx))
                goto errlabel;
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test get digital output success.");
            Console.WriteLine("=====================================================================");
            return true;
        errlabel:
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test get digital output failed.");
            Console.WriteLine("=====================================================================");
            return false;
        }
        /**
         * 设置输出IO状态测试
         */
        public bool setOutputTest()
        {
            Context context = new Context();
            IntPtr ctx = context.SDKCreate();
            int status = 0;
            int retstatus = new int();
            if (null == ctx)
                goto errlabel;
            if (!context.SDKLogin(ctx))
                goto errlabel;
            // digital output index -1边界测试
            if (setOutput(ctx, -1, status))
                goto errlabel;
            Thread.Sleep(100);
            // digital output index 128边界测试
            if (setOutput(ctx, 128, status))
                goto errlabel;
            Thread.Sleep(100);
            // analog output 0——127 测试
            for (int i = 0; i < 128; i++)
            {
                if (!setOutput(ctx, i, status))
                    goto errlabel;
                Thread.Sleep(100);
                if (!getOutput(ctx, i, ref retstatus))
                    goto errlabel;
                if (status != retstatus)
                {
                    Console.WriteLine("set status = {0}, but ret status = {1}", status, retstatus);
                    goto errlabel;
                }
                status = (status == 0) ? 1 : 0;
            }
            if (!context.SDKLogout(ctx))
            {
                goto errlabel;
            }
            if (!context.SDKDestroy(ctx))
                goto errlabel;
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test set digital output success.");
            Console.WriteLine("=====================================================================");
            return true;
        errlabel:
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test set digital output failed.");
            Console.WriteLine("=====================================================================");
            return false;
        }
        /**
         * 获取虚拟输入IO状态测试
         */ 
         public bool getVirtualInputTest()
        {
            Context context = new Context();
            IntPtr ctx = context.SDKCreate();
            int status = new int();
            if (null == ctx)
                goto errlabel;
            if (!context.SDKLogin(ctx))
                goto errlabel;
            // virtual input index -1边界测试
            if (getVirtualInput(ctx, -1, ref status))
                goto errlabel;
            Thread.Sleep(100);
            // virtual input index 128边界测试
            if (getVirtualInput(ctx, 128, ref status))
                goto errlabel;
            Thread.Sleep(100);
            // virtual input index 0——127 测试
            for (int i = 0; i < 128; i++)
            {
                if (!getVirtualInput(ctx, i, ref status))
                    goto errlabel;
                Console.WriteLine("get virtual input {0}, status = {1}.", i, status);
                Thread.Sleep(100);
            }
            if (!context.SDKLogout(ctx))
            {
                goto errlabel;
            }
            if (!context.SDKDestroy(ctx))
                goto errlabel;
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test set digital output success.");
            Console.WriteLine("=====================================================================");
            return true;
        errlabel:
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test set digital output failed.");
            Console.WriteLine("=====================================================================");
            return false;
        }
        /**
         * 获取虚拟输出IO状态测试
         */
        public bool getVirtualOutputTest()
        {
            Context context = new Context();
            IntPtr ctx = context.SDKCreate();
            int status = new int();
            if (null == ctx)
                goto errlabel;
            if (!context.SDKLogin(ctx))
                goto errlabel;
            // virtual input index 399边界测试
            if (getVirtualOutput(ctx, 399, ref status))
                goto errlabel;
            Thread.Sleep(100);
            // virtual input index 1537边界测试
            if (getVirtualOutput(ctx, 1537, ref status))
                goto errlabel;
            Thread.Sleep(100);
            // virtual input index 400——1536 测试
            for (int i = 400; i < 1537; i++)
            {
                if (!getVirtualOutput(ctx, i, ref status))
                    goto errlabel;
                Console.WriteLine("get virtual output {0}, status = {1}.", i, status);
                Thread.Sleep(100);
            }
            if (!context.SDKLogout(ctx))
            {
                goto errlabel;
            }
            if (!context.SDKDestroy(ctx))
                goto errlabel;
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test get virtual output success.");
            Console.WriteLine("=====================================================================");
            return true;
        errlabel:
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test get virtual output failed.");
            Console.WriteLine("=====================================================================");
            return false;
        }
        /**
         * 设置虚拟输出IO状态测试
         */
        public bool setVirtualOutputTest()
        {
            Context context = new Context();
            IntPtr ctx = context.SDKCreate();
            int status = 0;
            int retstatus = new int();
            if (null == ctx)
                goto errlabel;
            if (!context.SDKLogin(ctx))
                goto errlabel;
            // virtual output index 399边界测试
            if (setVirtualOutput(ctx, 399, 0))
                goto errlabel;
            Thread.Sleep(100);
            // virtual input index 1537边界测试
            if (setVirtualOutput(ctx, 1537, 0))
                goto errlabel;
            Thread.Sleep(100);
            // virtual input index 400——1536 测试
            for (int i = 400; i < 1537; i++)
            {
                if (!setVirtualOutput(ctx, i, status))
                    goto errlabel;
                Console.WriteLine("get virtual output {0}, status = {1}.", i, status);
                Thread.Sleep(100);
                if (!getVirtualOutput(ctx, i, ref retstatus))
                    goto errlabel;
                if(status != retstatus)
                {
                    Console.WriteLine("set virtual ouptut status = {0}, but ret status = {1}", status, retstatus);
                    goto errlabel;
                }
            }
            if (!context.SDKLogout(ctx))
            {
                goto errlabel;
            }
            if (!context.SDKDestroy(ctx))
                goto errlabel;
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test set virtual output success.");
            Console.WriteLine("=====================================================================");
            return true;
        errlabel:
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test set virtual output failed.");
            Console.WriteLine("=====================================================================");
            return false;
        }
    }
}
