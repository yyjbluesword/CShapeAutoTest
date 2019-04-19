using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using SDKContext;

namespace SDKServoService
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
    
    class ServoService
    {
        //获取机械臂伺服状态
        [DllImport("eltrobot.dll")]
        public static extern int elt_get_servo_status(IntPtr ctx, ref int status, ref elt_error err);
        //获取机械臂上下电状态
        [DllImport("eltrobot.dll")]
        public static extern int elt_get_motor_status(IntPtr ctx, ref int status, ref elt_error err);
        //设置机械臂伺服状态
        [DllImport("eltrobot.dll")]
        public static extern int elt_set_servo_status(IntPtr ctx, int status, ref elt_error err);
        //同步伺服编码器数据
        [DllImport("eltrobot.dll")]
        public static extern int elt_sync_motor_status(IntPtr ctx, ref elt_error err);
        //清除报警
        [DllImport("eltrobot.dll")]
        public static extern int elt_clear_alarm(IntPtr ctx, int status, ref elt_error err);
        public static string get_error_msg(char[] msg)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in msg)
            {
                sb.Append(c);
            }
            return sb.ToString();
        }
        bool outputRet(int type,elt_error err, string msg)
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
         *获取伺服状态测试
         * ctx:上下文
         * return:伺服状态或-1（失败）
         */
        public int getServoStatus(IntPtr ctx)
        {
            int status = new int();
            status = -1;
            elt_error err = new elt_error();
            int ret = elt_get_servo_status(ctx, ref status, ref err);
            outputRet(ret, err,"elt_get_servo_status");
            return status;
        }
        /*
         *  获取机械臂上下电状态
         *  ctx: 上下文
         *  return 上下电状态或-1（失败）
         */ 
        public int getMotorStatus(IntPtr ctx)
        {
            int status = new int();
            status = -1;
            elt_error err = new elt_error();
            int ret = elt_get_motor_status(ctx, ref status, ref err);
            outputRet(ret, err,"elt_get_motot_status");
            return status;
        }
        /*
         * 设置机械臂伺服状态
         * ctx: 上下文
         * status: 状态（0，1）
         * return:成功或失败
         */ 
        public bool setServoStatus(IntPtr ctx, int status)
        {
            elt_error err = new elt_error();
            int ret = elt_set_servo_status(ctx, status, ref err);
            return outputRet(ret,err, "elt_set_servo_status");
        }
        /*
         * 同步伺服编码器数据
         * ctx:上下文
         * return：成功或失败
         */ 
        public bool syncMotorStatus(IntPtr ctx)
        {
            elt_error err = new elt_error();
            int ret = elt_sync_motor_status(ctx, ref err);
            return outputRet(ret,err, "elt_sync_motor_status");
        }
        /*
         * 清报警功能
         * ctx
         */
         bool clearAlarm(IntPtr ctx, int status)
        {
            elt_error err = new elt_error();
            int ret = elt_clear_alarm(ctx, status ,ref err);
            return outputRet(ret, err, "elt_clear_alarm");
        }
        public bool SyncGetMotorStatusTest()
        {
            Context context = new Context();
            ServoService servoService = new ServoService();
            IntPtr ctx = context.SDKCreate();
            int status;
            if (null == ctx)
                goto errlabel;
            if (!context.SDKLogin(ctx))
                goto errlabel;
            for (int i = 0; i < 10; i++)
            {
                status = servoService.getMotorStatus(ctx);
                if (-1 == status)
                    goto errlabel;
                Console.WriteLine("motorStatus = {0}", status);
                if (!servoService.syncMotorStatus(ctx))
                    goto errlabel;
                Thread.Sleep(1500);
            }
            if (!context.SDKLogout(ctx))
            {
                goto errlabel;
            }
            if (!context.SDKDestroy(ctx))
                goto errlabel;
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test SyncGetMotorStatus success.");
            Console.WriteLine("=====================================================================");
            return true;
        errlabel:
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test GetMotorStatus failed.");
            Console.WriteLine("=====================================================================");
            return false;
        }
        public bool SetGetServoStatusTest()
        {
            Context context = new Context();
            ServoService servoService = new ServoService();
            IntPtr ctx = context.SDKCreate();
            int status = 0;
            if (null == ctx)
                goto errlabel;
            if (!context.SDKLogin(ctx))
                goto errlabel;
            for (int i = 0; i < 10; i++)
            {
                status = (status == 0) ? 1 : 0;
                if (!servoService.setServoStatus(ctx, status))
                    goto errlabel;
                Thread.Sleep(1000);
                status = servoService.getServoStatus(ctx);
                if (-1 == status)
                    goto errlabel;
                Console.WriteLine("servoStatus = {0}", status);
                Thread.Sleep(1000);
            }
            if (!context.SDKLogout(ctx))
            {
                goto errlabel;
            }
            if (!context.SDKDestroy(ctx))
                goto errlabel;
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test SetServoStatus success.");
            Console.WriteLine("=====================================================================");
            return true;
        errlabel:
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test SetServoStatus failed.");
            Console.WriteLine("=====================================================================");
            return false;
        }
        /*
         * 清报警功能测试 
         */
        public bool ClearAlarmTest()
        {
            Context context = new Context();
            ServoService servoService = new ServoService();
            IntPtr ctx = context.SDKCreate();
            if (null == ctx)
                goto errlabel;
            if (!context.SDKLogin(ctx))
                goto errlabel;
            //边界-1测试
            if (clearAlarm(ctx, -1))
                goto errlabel;
            Thread.Sleep(1000);
            //边界2测试
            if (clearAlarm(ctx, 2))
                goto errlabel;
            Thread.Sleep(1000);
            //正常数据1测试
            if (!clearAlarm(ctx, 0))
                goto errlabel;
            Thread.Sleep(1000);
            //正常数据2测试
            if (!clearAlarm(ctx, 1))
                goto errlabel;  
            if (!context.SDKLogout(ctx))
            {
                goto errlabel;
            }
            if (!context.SDKDestroy(ctx))
                goto errlabel;
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test clear alarm success.");
            Console.WriteLine("=====================================================================");
            return true;
        errlabel:
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test clear alarm failed.");
            Console.WriteLine("=====================================================================");
            return false;
        }

    }
}
