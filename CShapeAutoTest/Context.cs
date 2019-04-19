using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SDKContext
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
    public class Context
    {
        //导入创建连接
        [DllImport("eltrobot.dll")]
        public static extern IntPtr elt_create_ctx(string addr, int port);
        //导入登录
        [DllImport("eltrobot.dll")]
        public static extern int elt_login(IntPtr ctx);
        //导入登出
        [DllImport("eltrobot.dll")]
        public static extern int elt_logout(IntPtr ctx);
        //导入销毁连接
        [DllImport("eltrobot.dll")]
        public static extern int elt_destroy_ctx(IntPtr ctx);
        public string get_error_msg(char[] msg)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in msg)
            {
                sb.Append(c);
            }
            return sb.ToString();
        }
        static bool outputRet(int type, string msg)
        {
            if ((errType)type == errType.ELT_ERROR)
            {
                Console.WriteLine("{0} error.", msg);
            }
            else if ((errType)type == errType.ELT_FAILURE)
            {
                Console.WriteLine("{0} failed.", msg);
            }
            else if ((errType)type == errType.ELT_SUCCESS)
            {
                //Console.WriteLine("{0} success.",msg);
                return true;
            }
            else
            {
                Console.WriteLine("{0} other status.", msg);
            }
            return false;
        }
        //创建连接
        public IntPtr SDKCreate()
        {
            IntPtr ctx = elt_create_ctx("192.168.1.200", 8055);
            if (null == ctx)
            {
                Console.WriteLine("elt_create_ctx failed.\n");
            }
            return ctx;
        }
        //销毁连接
        public bool SDKDestroy(IntPtr ctx)
        {
            int ret = elt_destroy_ctx(ctx);
            return outputRet(ret, "elt_destroy_ctx");
        }
        //登录
        public bool SDKLogin(IntPtr ctx)
        {
            int ret = elt_login(ctx);
            return outputRet(ret, "elt_login");
        }
        //退出
        public bool SDKLogout(IntPtr ctx)
        {
            int ret = elt_logout(ctx);
            return outputRet(ret, "elt_logout");
        }
        /*
         * 创建连接测试
         */
        public bool CreateConnectTest()
        {
            Context context = new Context();
            IntPtr ctx = context.SDKCreate();
            if (null == ctx)
            {
                goto errlabel;
            }
            if (!context.SDKDestroy(ctx))
            {
                goto errlabel;
            }
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test CreateConnnect success.");
            Console.WriteLine("=====================================================================");
            return true;
        errlabel:
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test CreateConnnect failed.");
            Console.WriteLine("=====================================================================");
            return false;
        }
        /*
         *销毁连接测试 
         */
        public bool DestroyConnectTest()
        {
            Context context = new Context();
            IntPtr ctx = context.SDKCreate();
            if (null == ctx)
                return false;
            if (!context.SDKDestroy(ctx))
                goto errlabel;
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test DestroyConnnect success.");
            Console.WriteLine("=====================================================================");
            return true;
        errlabel:
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test DestroyConnnect failed.");
            Console.WriteLine("=====================================================================");
            return false;
        }
        /*
         * 登录测试
         */
        public bool LoginTest()
        {
            Context context = new Context();
            IntPtr ctx = context.SDKCreate();
            if (null == ctx)
                goto errlabel;
            if (!context.SDKLogin(ctx))
            {
                goto errlabel;
            }
            if (!context.SDKLogout(ctx))
                goto errlabel;
            if (!context.SDKDestroy(ctx))
                goto errlabel;
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test LoginTest success.");
            Console.WriteLine("=====================================================================");
            return true;
        errlabel:
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test LoginTest failed.");
            Console.WriteLine("=====================================================================");
            return false;
        }
        /*
         * 退出测试
         */
        public bool LogoutTest()
        {
            Context context = new Context();
            IntPtr ctx = context.SDKCreate();
            if (null == ctx)
                goto errlabel;
            if (!context.SDKLogin(ctx))
                goto errlabel;
            if (!context.SDKLogout(ctx))
            {
                goto errlabel;
            }
            if (!context.SDKDestroy(ctx))
                goto errlabel;
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test LogoutTest success.");
            Console.WriteLine("=====================================================================");
            return true ;
        errlabel:
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test LogoutTest failed.");
            Console.WriteLine("=====================================================================");
            return false;
        }
    }
}