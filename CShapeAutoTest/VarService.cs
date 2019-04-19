using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SDKContext;

namespace SDKVarService
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
    class VarService
    {
        //获取系统B变量值
        [DllImport("eltrobot.dll")]
        public static extern int elt_get_sysvar_b(IntPtr ctx, int addr, ref int value, ref elt_error err);
        //设置系统B变量值
        [DllImport("eltrobot.dll")]
        public static extern int elt_set_sysvar_b(IntPtr ctx, int addr, int value, ref elt_error err);
        //获取系统I变量值
        [DllImport("eltrobot.dll")]
        public static extern int elt_get_sysvar_i(IntPtr ctx, int addr, ref int value, ref elt_error err);
        //设置系统I变量值
        [DllImport("eltrobot.dll")]
        public static extern int elt_set_sysvar_i(IntPtr ctx, int addr, int value, ref elt_error err);
        //获取系统D变量值
        [DllImport("eltrobot.dll")]
        public static extern int elt_get_sysvar_d(IntPtr ctx, int addr, ref double value, ref elt_error err);
        //设置系统D变量值
        [DllImport("eltrobot.dll")]
        public static extern int elt_set_sysvar_d(IntPtr ctx, int addr, double value, ref elt_error err);
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
        /*
         * 获取系统B变量值
         */ 
         public bool getSysvarB(IntPtr ctx, int addr, ref int value)
        {
            elt_error err = new elt_error();
            int ret = elt_get_sysvar_b(ctx, addr, ref value, ref err);
            return outputRet(ret, err, "elg_get_sysvar_b");
        }
        /*
         * 设置系统B变量值
         */ 
         public bool setSysvarB(IntPtr ctx, int addr, int value)
        {
            elt_error err = new elt_error();
            int ret = elt_set_sysvar_b(ctx, addr, value, ref err);
            return outputRet(ret, err, "elt_set_sysvar_b");
        }
        /*
         * 获取系统I变量值
         */ 
         bool getSysvarI(IntPtr ctx, int addr, ref int value)
        {
            elt_error err = new elt_error();
            int ret = elt_get_sysvar_i(ctx, addr, ref value, ref err);
            return outputRet(ret, err, "elt_get_sysvar_i");
        }
        /*
         * 设置系统I变量值
         */ 
         bool setSysvarI(IntPtr ctx, int addr, int value)
        {
            elt_error err = new elt_error();
            int ret = elt_set_sysvar_i(ctx, addr, value, ref err);
            return outputRet(ret, err, "elt_set_sysvar_i");
        }
        /*
         * 获取系统D变量值
         */ 
         bool getSysvarD(IntPtr ctx, int addr, ref double value)
        {
            elt_error err = new elt_error();
            int ret = elt_get_sysvar_d(ctx, addr, ref value, ref err);
            return outputRet(ret, err, "elt_get_sysvar_d");
        }
        /*
         * 设置系统D变量值
         */ 
         bool setSysvarD(IntPtr ctx, int addr, double value)
        {
            elt_error err = new elt_error();
            int ret = elt_set_sysvar_d(ctx, addr, value, ref err);
            return outputRet(ret, err, "elt_set_sysvar_d");
        }
        /*
         * 获取B变量值测试
         */ 
         public bool getSysvarBTest()
        {
            Context context = new Context();
            IntPtr ctx = context.SDKCreate();
            int value = new int();
            if (null == ctx)
                goto errlabel;
            if (!context.SDKLogin(ctx))
                goto errlabel;
            // system var B index -1边界测试
            if (getSysvarB(ctx, -1, ref value))
                goto errlabel;
            Thread.Sleep(100);
            // system var B index 256边界测试
            if (getSysvarB(ctx, 256, ref value))
                goto errlabel;
            Thread.Sleep(100);
            // system var B index 0——255 测试
            for (int i = 0; i < 256; i++)
            {
                if (!getSysvarB(ctx, i, ref value))
                    goto errlabel;
                Console.WriteLine("B {0}, value = {1}", i, value);
                Thread.Sleep(100);
            }
            if (!context.SDKLogout(ctx))
            {
                goto errlabel;
            }
            if (!context.SDKDestroy(ctx))
                goto errlabel;
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test get system var B success.");
            Console.WriteLine("=====================================================================");
            return true;
        errlabel:
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test get system var B failed.");
            Console.WriteLine("=====================================================================");
            return false;
        }
        /*
         * 设置B变量测试
         */ 
         public bool setSysvarBTest()
        {
            Context context = new Context();
            IntPtr ctx = context.SDKCreate();
            Random rd = new Random();
            int value;
            int retvalue = new int();
            if (null == ctx)
                goto errlabel;
            if (!context.SDKLogin(ctx))
                goto errlabel;
            // system var B index -1边界测试
            if (setSysvarB(ctx, -1, 0))
                goto errlabel;
            Thread.Sleep(100);
            // system var B index 256边界测试
            if (setSysvarB(ctx, 256, 0))
                goto errlabel;
            // system var B value -1边界测试
            if (setSysvarB(ctx, 0, -1))
                goto errlabel;
            // system var B value 2^16边界测试
            if (setSysvarB(ctx, 0, (int)Math.Pow(2, 16)))
                goto errlabel;
            Thread.Sleep(100);
            // system var B index 0——255 测试
            for (int i = 0; i < 256; i++)
            {
                value = rd.Next(0, (int)Math.Pow(2, 16));
                if (!setSysvarB(ctx, i, value))
                    goto errlabel;
                Thread.Sleep(100);
                if (!getSysvarB(ctx, i, ref retvalue))
                    goto errlabel;
                if(value != retvalue)
                {
                    Console.WriteLine("set B{0}, value = {0}, but ret value = {1}", i, value, retvalue);
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
            Console.WriteLine("Test set system var B success.");
            Console.WriteLine("=====================================================================");
            return true;
        errlabel:
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test set system var B failed.");
            Console.WriteLine("=====================================================================");
            return false;
        }
        /*
         * 获取I变量值测试
         */
        public bool getSysvarITest()
        {
            Context context = new Context();
            IntPtr ctx = context.SDKCreate();
            int value = new int();
            if (null == ctx)
                goto errlabel;
            if (!context.SDKLogin(ctx))
                goto errlabel;
            // system var I index -1边界测试
            if (getSysvarI(ctx, -1, ref value))
                goto errlabel;
            Thread.Sleep(100);
            // system var I index 256边界测试
            if (getSysvarI(ctx, 256, ref value))
                goto errlabel;
            Thread.Sleep(100);
            // system var I index 0——255 测试
            for (int i = 0; i < 256; i++)
            {
                if (!getSysvarI(ctx, i, ref value))
                    goto errlabel;
                Console.WriteLine("I {0}, value = {1}", i, value);
                Thread.Sleep(100);
            }
            if (!context.SDKLogout(ctx))
            {
                goto errlabel;
            }
            if (!context.SDKDestroy(ctx))
                goto errlabel;
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test get system var I success.");
            Console.WriteLine("=====================================================================");
            return true;
        errlabel:
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test get system var I failed.");
            Console.WriteLine("=====================================================================");
            return false;
        }
        /*
         * 设置I变量测试
         */
        public bool setSysvarITest()
        {
            Context context = new Context();
            IntPtr ctx = context.SDKCreate();
            Random rd = new Random();
            int value;
            int retvalue = new int();
            if (null == ctx)
                goto errlabel;
            if (!context.SDKLogin(ctx))
                goto errlabel;
            // system var I index -1边界测试
            if (setSysvarI(ctx, -1, 0))
                goto errlabel;
            Thread.Sleep(100);
            // system var I index 256边界测试
            if (setSysvarI(ctx, 256, 0))
                goto errlabel;
            // system var I value -2^31-1边界测试
            if (setSysvarI(ctx, 0, (int)Math.Pow(2,31)*-1-1))
                goto errlabel;
            // system var I value 2^31边界测试
            if (setSysvarI(ctx, 0, (int)Math.Pow(2, 31)))
                goto errlabel;
            Thread.Sleep(100);
            // system var I index 0——255 测试
            for (int i = 0; i < 256; i++)
            {
                value = rd.Next(0, (int)Math.Pow(2, 31))*(i%2==0?1:-1);
                if (!setSysvarI(ctx, i, value))
                    goto errlabel;
                Thread.Sleep(100);
                if (!getSysvarI(ctx, i, ref retvalue))
                    goto errlabel;
                if (value != retvalue)
                {
                    Console.WriteLine("set I{0}, value = {0}, but ret value = {1}", i, value, retvalue);
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
            Console.WriteLine("Test set system var I success.");
            Console.WriteLine("=====================================================================");
            return true;
        errlabel:
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test set system var I failed.");
            Console.WriteLine("=====================================================================");
            return false;
        }
        /*
         * 获取D变量值测试
         */
        public bool getSysvarDTest()
        {
            Context context = new Context();
            IntPtr ctx = context.SDKCreate();
            double value = new double();
            if (null == ctx)
                goto errlabel;
            if (!context.SDKLogin(ctx))
                goto errlabel;
            // system var D index -1边界测试
            if (getSysvarD(ctx, -1, ref value))
                goto errlabel;
            Thread.Sleep(100);
            // system var D index 256边界测试
            if (getSysvarD(ctx, 256, ref value))
                goto errlabel;
            Thread.Sleep(100);
            // system var D index 0——255 测试
            for (int i = 0; i < 256; i++)
            {
                if (!getSysvarD(ctx, i, ref value))
                    goto errlabel;
                Console.WriteLine("D {0}, value = {1}", i, value);
                Thread.Sleep(100);
            }
            if (!context.SDKLogout(ctx))
            {
                goto errlabel;
            }
            if (!context.SDKDestroy(ctx))
                goto errlabel;
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test get system var D success.");
            Console.WriteLine("=====================================================================");
            return true;
        errlabel:
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test get system var D failed.");
            Console.WriteLine("=====================================================================");
            return false;
        }
        /*
         * 设置D变量测试
         */
        public bool setSysvarDTest()
        {
            Context context = new Context();
            IntPtr ctx = context.SDKCreate();
            Random rd = new Random();
            double value;
            double retvalue = new double();
            if (null == ctx)
                goto errlabel;
            if (!context.SDKLogin(ctx))
                goto errlabel;
            // system var D index -1边界测试
            if (setSysvarD(ctx, -1, 0))
                goto errlabel;
            // system var D index 256边界测试
            if (setSysvarD(ctx, 256, 0))
                goto errlabel;
            /*
            // system var D value -3.4E+38-1边界测试
            if (setSysvarD(ctx, 0, (int)Math.Pow(2, 31) * -1 - 1))
                goto errlabel;
            // system var D value 3.4E+38+1边界测试
            if (setSysvarD(ctx, 0, (int)Math.Pow(2, 31)))
                goto errlabel;
            Thread.Sleep(100);
            */
            // system var D index 0——255 测试
            for (int i = 0; i < 256; i++)
            {
                value = rd.Next(0, (int)Math.Pow(2, 16)) * (i%2 == 0 ? 1.0 : -1.0);
                if (!setSysvarD(ctx, i, value))
                    goto errlabel;
                if (!getSysvarD(ctx, i, ref retvalue))
                    goto errlabel;
                if (Math.Abs(value - retvalue) > 0.001)
                {
                    Console.WriteLine("set D{0}, value = {0}, but ret value = {1}", i, value, retvalue);
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
            Console.WriteLine("Test set system var D success.");
            Console.WriteLine("=====================================================================");
            return true;
        errlabel:
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test set system var D failed.");
            Console.WriteLine("=====================================================================");
            return false;
        }
    }
}
