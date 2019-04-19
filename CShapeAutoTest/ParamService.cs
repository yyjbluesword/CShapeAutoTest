using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SDKContext;
using SDKServoService;

namespace SDKParamService
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
    class ParamService
    {
        //获取机器人状态
        [DllImport("eltrobot.dll")]
        public static extern int elt_get_robot_state(IntPtr ctx, ref int status, ref elt_error err);
        //获取机器人模式
        [DllImport("eltrobot.dll")]
        public static extern int elt_get_robot_mode(IntPtr ctx, ref int mode, ref elt_error err);
        //获取机器人当前位置信息
        [DllImport("eltrobot.dll")]
        unsafe public static extern int elt_get_robot_pos(IntPtr ctx,  double [] pos_array, ref elt_error err);
        //获取机器人当前位姿信息
        [DllImport("eltrobot.dll")]
        unsafe public static extern int elt_get_robot_pose(IntPtr ctx, double [] pose_array, ref elt_error err);
        //获取机器人马达速度
        [DllImport("eltrobot.dll")]
        unsafe public static extern int elt_get_motor_speed(IntPtr ctx, double[] speed_array, ref elt_error err);
        //获取机器人当前坐标
        [DllImport("eltrobot.dll")]
        public static extern int elt_get_current_coord(IntPtr ctx, ref int coord, ref elt_error err);
        //获取机器人循环模式
        [DllImport("eltrobot.dll")]
        public static extern int elt_get_cycle_mode(IntPtr ctx, ref int mode, ref elt_error err);
        //获取机器人当前作业行号
        [DllImport("eltrobot.dll")]
        public static extern int elt_get_current_job_line(IntPtr ctx, ref int line_no, ref elt_error err);
        //获取机器人当前编码器值列表
        [DllImport("eltrobot.dll")]
        unsafe public static extern int elt_get_current_encode(IntPtr ctx, double[] encode_array, ref elt_error err);
        //获取机器人当前工具号
        [DllImport("eltrobot.dll")]
        public static extern int elt_get_tool_number(IntPtr ctx, ref int tool_num, ref elt_error err);
        //获取机器人当前用户号
        [DllImport("eltrobot.dll")]
        public static extern int elt_get_user_number(IntPtr ctx, ref int user_num, ref elt_error err);
        //获取机器人当前力矩信息
        [DllImport("eltrobot.dll")]
        unsafe public static extern int elt_get_robot_torques(IntPtr ctx, double[] torques, ref elt_error err);
        //获取模拟量输入
        [DllImport("eltrobot.dll")]
        public static extern int elt_get_analog_input(IntPtr ctx, int addr, ref double value, ref elt_error err);
        //设置模拟量输出
        [DllImport("eltrobot.dll")]
        public static extern int elt_set_analog_output(IntPtr ctx, int addr, double value, ref elt_error err);
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
         * 获取机器人状态
         */ 
        public int getRobotState(IntPtr ctx)
        {
            int state = new int();
            state = -1;
            elt_error err = new elt_error();
            int ret = elt_get_robot_state(ctx, ref state, ref err);
            outputRet(ret, err, "elt_get_robot_state");
            return state;
        }
        /*
         * 获取机器人模式
         */ 
         public int getRobotMode(IntPtr ctx)
        {
            int mode = new int();
            mode = -1;
            elt_error err = new elt_error();
            int ret = elt_get_robot_state(ctx, ref mode, ref err);
            outputRet(ret, err, "elt_get_robot_state");
            return mode;
        }
        /*
         * 获取机器人当前位置信息
         */ 
         unsafe public bool getRobotPos(IntPtr ctx,  double [] pos_array)
        {
            elt_error err = new elt_error();
            int ret = elt_get_robot_pos(ctx, pos_array, ref err);
            return outputRet(ret, err, "elt_get_robot_pos");
        }
        /*
         * 获取机器人当前位姿信息
         */ 
         unsafe public bool getRobotPose(IntPtr ctx, double [] pose_array)
        {
            elt_error err = new elt_error();
            int ret = elt_get_robot_pose(ctx, pose_array, ref err);
            return outputRet(ret, err, "elt_get_robot_pose");
        }
        /*
         * 获取机器人马达速度 
         */
         unsafe public bool getMotorSpeed(IntPtr ctx, double [] speed_array)
        {
            elt_error err = new elt_error();
            int ret = elt_get_motor_speed(ctx, speed_array, ref err);
            return outputRet(ret, err, "elt_get_motor_speed");
        }
        /*
         * 获取机器人当前坐标
         */
         public int getCurrentCoord(IntPtr ctx)
        {
            int coord = new int();
            coord = -1;
            elt_error err = new elt_error();
            int ret = elt_get_current_coord(ctx, ref coord, ref err);
            outputRet(ret, err, "elt_get_current_coord");
            return coord;
        }
        /**
         * 获取机器人循环模式
         */ 
         public int getCycleMode(IntPtr ctx)
        {
            int mode = new int();
            mode = -1;
            elt_error err = new elt_error();
            int ret = elt_get_cycle_mode(ctx, ref mode, ref err);
            outputRet(ret, err, "elt_get_cycle_mode");
            return mode;
        }
        /**
         * 获取机器人当前作业运行行号 
         */
         public int getCurrentJobLine(IntPtr ctx)
        {
            int line_no = new int();
            line_no = -1;
            elt_error err = new elt_error();
            int ret = elt_get_current_job_line(ctx, ref line_no, ref err);
            outputRet(ret, err, "elt_get_current_job_line");
            return line_no;
        }
        /*
         * 获取机器人当前编码器值列表
         */ 
         unsafe public bool getCurrentEncode(IntPtr ctx, double [] encode_array)
        {
            elt_error err = new elt_error();
            int ret = elt_get_current_encode(ctx, encode_array, ref err);
            return outputRet(ret, err, "elt_get_current_encode");
        }
        /*
         * 获取机器人当前工具号
         */ 
         public int getToolNumber(IntPtr ctx)
        {
            int tool_num = new int();
            tool_num = -1;
            elt_error err = new elt_error();
            int ret = elt_get_tool_number(ctx, ref tool_num, ref err);
            outputRet(ret, err, "elt_get_tool_number");
            return tool_num;
        }
        /*
         * 获取机器人当前用户号
         */ 
         public int getUserNumber(IntPtr ctx)
        {
            int user_num = new int();
            user_num = -1;
            elt_error err = new elt_error();
            int ret = elt_get_tool_number(ctx, ref user_num, ref err);
            outputRet(ret, err, "elt_get_user_number");
            return user_num;
        }
        /**
         * 获取机器人当前力矩信息
         */
         unsafe public bool getRobotTorques(IntPtr ctx, double [] torques)
        {
            elt_error err = new elt_error();
            int ret = elt_get_robot_torques(ctx, torques, ref err);
            return outputRet(ret, err, "elt_get_robot_torques");
        }
        /**
         * 获取模拟量输入
         */ 
         public bool getAnalogInput(IntPtr ctx, int addr, ref double value)
        {
            elt_error err = new elt_error();
            int ret = elt_get_analog_input(ctx, addr, ref value, ref err);
            return outputRet(ret, err, "elt_get_analog_input");
        }
        /**
         * 设置模拟量输出
         */ 
         public bool setAnalogOutput(IntPtr ctx, int addr, double value)
        {
            elt_error err = new elt_error();
            int ret = elt_set_analog_output(ctx, addr, value, ref err);
            return outputRet(ret, err, "elt_set_analog_output");
        }
        /**
         * 获取机器人状态测试
         */ 
         public bool getRobotStateTest()
        {
            Context context = new Context();
            IntPtr ctx = context.SDKCreate();
            if (null == ctx)
                goto errlabel;
            if (!context.SDKLogin(ctx))
                goto errlabel;
            if(-1 == getRobotState(ctx))
            {
                goto errlabel;
            }
            if (!context.SDKLogout(ctx))
            {
                goto errlabel;
            }
            if (!context.SDKDestroy(ctx))
                goto errlabel;
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test get robot state success.");
            Console.WriteLine("=====================================================================");
            return true;
        errlabel:
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test get robot state failed.");
            Console.WriteLine("=====================================================================");
            return false;
        }
     /**
     * 获取机器人模式测试
     */
        public bool getRobotModeTest()
        {
            Context context = new Context();
            IntPtr ctx = context.SDKCreate();
            if (null == ctx)
                goto errlabel;
            if (!context.SDKLogin(ctx))
                goto errlabel;
            if (-1 == getRobotMode(ctx))
            {
                goto errlabel;
            }
            if (!context.SDKLogout(ctx))
            {
                goto errlabel;
            }
            if (!context.SDKDestroy(ctx))
                goto errlabel;
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test get robot mode success.");
            Console.WriteLine("=====================================================================");
            return true;
        errlabel:
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test get robot mode failed.");

            Console.WriteLine("=====================================================================");
            return false;
        }
        /**
        * 获取机器人当前位置信息测试
        */
        public bool getRobotPosTest()
        {
            Context context = new Context();
            IntPtr ctx = context.SDKCreate();
            double [] pos_array = new double[8];
            if (null == ctx)
                goto errlabel;
            if (!context.SDKLogin(ctx))
                goto errlabel;
            if (!getRobotPos(ctx, pos_array))
                goto errlabel;
            Console.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7}", 
                pos_array[0], pos_array[1], pos_array[2], pos_array[3],
                pos_array[4], pos_array[5], pos_array[6], pos_array[7]);
            if (!context.SDKLogout(ctx))
            {
                goto errlabel;
            }
            if (!context.SDKDestroy(ctx))
                goto errlabel;
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test get robot pos success.");
            Console.WriteLine("=====================================================================");
            return true;
        errlabel:
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test get robot pos failed.");
            Console.WriteLine("=====================================================================");
            return false;
        }
        /**
        * 获取机器人当前位姿信息测试
        */
        public bool getRobotPoseTest()
        {
            Context context = new Context();
            IntPtr ctx = context.SDKCreate();
            double[] pose_array = new double[6];
            if (null == ctx)
                goto errlabel;
            if (!context.SDKLogin(ctx))
                goto errlabel;
            if (!getRobotPose(ctx, pose_array))
                goto errlabel;
            Console.WriteLine("{0},{1},{2},{3},{4},{5}",
                pose_array[0], pose_array[1], pose_array[2],
                pose_array[3], pose_array[4], pose_array[5]);
            if (!context.SDKLogout(ctx))
            {
                goto errlabel;
            }
            if (!context.SDKDestroy(ctx))
                goto errlabel;
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test get robot pose success.");
            Console.WriteLine("=====================================================================");
            return true;
        errlabel:
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test get robot pose failed.");
            Console.WriteLine("=====================================================================");
            return false;
        }
        /**
        * 获取机器人马达速度测试
        */
        unsafe public bool getMotorSpeedTest()
        {
            Context context = new Context();
            IntPtr ctx = context.SDKCreate();
            double[] speed_array = new double[8];
            if (null == ctx)
                goto errlabel;
            if (!context.SDKLogin(ctx))
                goto errlabel;
            if (!getMotorSpeed(ctx, speed_array))
                goto errlabel;
            Console.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7}",
                speed_array[0], speed_array[1], speed_array[2], speed_array[3],
                speed_array[4], speed_array[5], speed_array[6], speed_array[7]);
            if (!context.SDKLogout(ctx))
            {
                goto errlabel;
            }
            if (!context.SDKDestroy(ctx))
                goto errlabel;
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test get motor speed success.");
            Console.WriteLine("=====================================================================");
            return true;
        errlabel:
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test get motor speed failed.");
            Console.WriteLine("=====================================================================");
            return false;
        }
        /**
        * 获取机器人当前坐标测试
        */
        public bool getCurrentCoordTest()
        {
            Context context = new Context();
            IntPtr ctx = context.SDKCreate();
            int coord;
            if (null == ctx)
                goto errlabel;
            if (!context.SDKLogin(ctx))
                goto errlabel;
            coord = getCurrentCoord(ctx);
            if (-1 == coord)
                goto errlabel;
            Console.WriteLine("current coord = {0}.", coord);
            if (!context.SDKLogout(ctx))
            {
                goto errlabel;
            }
            if (!context.SDKDestroy(ctx))
                goto errlabel;
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test get current coord success.");
            Console.WriteLine("=====================================================================");
            return true;
        errlabel:
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test get current coord failed.");
            Console.WriteLine("=====================================================================");
            return false;
        }
        /**
        * 获取机器人当前循环模式测试
        */
        public bool getCycleModeTest()
        {
            Context context = new Context();
            IntPtr ctx = context.SDKCreate();
            int mode;
            if (null == ctx)
                goto errlabel;
            if (!context.SDKLogin(ctx))
                goto errlabel;
            mode = getCycleMode(ctx);
            if (-1 == mode)
                goto errlabel;
            Console.WriteLine("cycle mode = {0}.", mode);
            if (!context.SDKLogout(ctx))
            {
                goto errlabel;
            }
            if (!context.SDKDestroy(ctx))
                goto errlabel;
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test get cycle mode success.");
            Console.WriteLine("=====================================================================");
            return true;
        errlabel:
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test get cycle mode failed.");
            Console.WriteLine("=====================================================================");
            return false;
        }
        /**
       * 获取机器人当前作业运行行号测试
       */
        public bool getCurrentJobLineTest()
        {
            Context context = new Context();
            IntPtr ctx = context.SDKCreate();
            int line_no;
            if (null == ctx)
                goto errlabel;
            if (!context.SDKLogin(ctx))
                goto errlabel;
            line_no = getCurrentJobLine(ctx);
            if (-1 == line_no)
                goto errlabel;
            Console.WriteLine("line no = {0}.", line_no);
            if (!context.SDKLogout(ctx))
            {
                goto errlabel;
            }
            if (!context.SDKDestroy(ctx))
                goto errlabel;
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test get current job line success.");
            Console.WriteLine("=====================================================================");
            return true;
        errlabel:
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test get current job line failed.");
            Console.WriteLine("=====================================================================");
            return false;
        }
        /**
        * 获取机器人当前编码器列表测试
        */
        public bool getCurrentEncodeTest()
        {
            Context context = new Context();
            IntPtr ctx = context.SDKCreate();
            double[] encode_array = new double[8];
            if (null == ctx)
                goto errlabel;
            if (!context.SDKLogin(ctx))
                goto errlabel;
            if (!getCurrentEncode(ctx, encode_array))
                goto errlabel;
            Console.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7}",
                encode_array[0], encode_array[1], encode_array[2], encode_array[3],
                encode_array[4], encode_array[5], encode_array[6], encode_array[7]);
            if (!context.SDKLogout(ctx))
            {
                goto errlabel;
            }
            if (!context.SDKDestroy(ctx))
                goto errlabel;
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test get current encode success.");
            Console.WriteLine("=====================================================================");
            return true;
        errlabel:
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test get current encode failed.");
            Console.WriteLine("=====================================================================");
            return false;
        }
        /**
       * 获取机器人当前工具号测试
       */
        public bool getToolNumberTest()
        {
            Context context = new Context();
            IntPtr ctx = context.SDKCreate();
            int tool_num;
            if (null == ctx)
                goto errlabel;
            if (!context.SDKLogin(ctx))
                goto errlabel;
            tool_num = getToolNumber(ctx);
            if (-1 == tool_num)
                goto errlabel;
            Console.WriteLine("tool_num = {0}.", tool_num);
            if (!context.SDKLogout(ctx))
            {
                goto errlabel;
            }
            if (!context.SDKDestroy(ctx))
                goto errlabel;
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test get tool number success.");
            Console.WriteLine("=====================================================================");
            return true;
        errlabel:
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test get tool number line failed.");
            Console.WriteLine("=====================================================================");
            return false;
        }
        /**
       * 获取机器人当前用户工具号测试
       */
        public bool getUserNumberTest()
        {
            Context context = new Context();
            IntPtr ctx = context.SDKCreate();
            int user_num;
            if (null == ctx)
                goto errlabel;
            if (!context.SDKLogin(ctx))
                goto errlabel;
            user_num = getUserNumber(ctx);
            if (-1 == user_num)
                goto errlabel;
            Console.WriteLine("user_num = {0}.", user_num);
            if (!context.SDKLogout(ctx))
            {
                goto errlabel;
            }
            if (!context.SDKDestroy(ctx))
                goto errlabel;
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test get user number success.");
            Console.WriteLine("=====================================================================");
            return true;
        errlabel:
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test get user number failed.");
            Console.WriteLine("=====================================================================");
            return false;
        }
        /**
        * 获取机器人当前力矩信息测试
        */
        public bool getRobotTorquesTest()
        {
            Context context = new Context();
            IntPtr ctx = context.SDKCreate();
            double[] torques_array = new double[8];
            if (null == ctx)
                goto errlabel;
            if (!context.SDKLogin(ctx))
                goto errlabel;
            if (!getRobotTorques(ctx, torques_array))
                goto errlabel;
            Console.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7}",
                torques_array[0], torques_array[1], torques_array[2], torques_array[3],
                torques_array[4], torques_array[5], torques_array[6], torques_array[7]);
            if (!context.SDKLogout(ctx))
            {
                goto errlabel;
            }
            if (!context.SDKDestroy(ctx))
                goto errlabel;
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test get robot torques success.");
            Console.WriteLine("=====================================================================");
            return true;
        errlabel:
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test get robot torques failed.");
            Console.WriteLine("=====================================================================");
            return false;
        }
        /**
       * 获取模拟量输入测试
       */
        public bool getAnalogInputTest()
        {
            Context context = new Context();
            IntPtr ctx = context.SDKCreate();
            double value = new double();
            if (null == ctx)
                goto errlabel;
            if (!context.SDKLogin(ctx))
                goto errlabel;
            // analog -1边界测试
            if (getAnalogInput(ctx, -1, ref value))
                goto errlabel;
            // analog 2边界测试
            if (getAnalogInput(ctx, 2, ref value))
                goto errlabel;
            // analog 0 测试
            if (!getAnalogInput(ctx, 0, ref value))
                goto errlabel;
            Console.WriteLine("analog 0 = {0}", value);
            // analog 1测试
            if (!getAnalogInput(ctx, 1, ref value))
                goto errlabel;
            Console.WriteLine("analog 1 = {0}", value);
            if (!context.SDKLogout(ctx))
            {
                goto errlabel;
            }
            if (!context.SDKDestroy(ctx))
                goto errlabel;
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test get ananlog intput success.");
            Console.WriteLine("=====================================================================");
            return true;
        errlabel:
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test get analog input failed.");
            Console.WriteLine("=====================================================================");
            return false;
        }
        /**
       * 设置模拟量输出测试
       */
        public bool setAnalogOutputTest()
        {
            Context context = new Context();
            IntPtr ctx = context.SDKCreate();
            Random rd = new Random();
            double value;
            if (null == ctx)
                goto errlabel;
            if (!context.SDKLogin(ctx))
                goto errlabel;
            // analog output index -1边界测试
            if (setAnalogOutput(ctx, -1, 5.0))
                goto errlabel;
            Thread.Sleep(1000);
            // analog output index 4边界测试
            if (setAnalogOutput(ctx, 4, 5.0))
                goto errlabel;
            Thread.Sleep(1000);
            // analog output value -10.1边界测试
            if (setAnalogOutput(ctx, 1, -10.1))
                goto errlabel;
            Thread.Sleep(1000);
            // analog output value 10.1边界测试
            if (setAnalogOutput(ctx, 1, 10.1))
                goto errlabel;
            Thread.Sleep(1000);
            // analog output 1——4 测试
            for (int i = 0; i < 4; i++)
            {
                value = rd.Next(0, 100) / 10.0 * ((i%2 == 0) ? 1 : -1);
                if (!setAnalogOutput(ctx, i, value))
                    goto errlabel;
            }
            if (!context.SDKLogout(ctx))
            {
                goto errlabel;
            }
            if (!context.SDKDestroy(ctx))
                goto errlabel;
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test get ananlog intput success.");
            Console.WriteLine("=====================================================================");
            return true;
        errlabel:
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test get analog input failed.");
            Console.WriteLine("=====================================================================");
            return false;
        }
    }
}
