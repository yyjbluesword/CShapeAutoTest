using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SDKContext;
using SDKServoService;
using SDKParamService;
using SDKKinematicsService;

namespace SDKMovementService
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
    class MovementService
    {
        //关节运动
        [DllImport("eltrobot.dll")]
        public static extern int elt_joint_move(IntPtr ctx, double [] target_pos_array, double speed, ref elt_error err);
        //直线运动
        [DllImport("eltrobot.dll")]
        public static extern int elt_line_move(IntPtr ctx, double [] target_pos_array, double speed, ref elt_error err);
        //圆弧运动
        [DllImport("eltrobot.dll")]
        public static extern int elt_arc_move(IntPtr ctx, double [] middle_target_pos_array,
            double [] target_pos_array, double speed, ref elt_error err);
        //旋转运动
        [DllImport("eltrobot.dll")]
        public static extern int elt_rotate_move(IntPtr ctx, double [] target_pos_array, double speed, ref elt_error err);
        //设置路点运动时最大关节速度
        [DllImport("eltrobot.dll")]
        public static extern int elt_set_waypoint_max_joint_speed(IntPtr ctx, double speed, ref elt_error err);
        //设置路点运动时最大直线速度
        [DllImport("eltrobot.dll")]
        public static extern int elt_set_waypoint_max_line_speed(IntPtr ctx, double speed, ref elt_error err);
        //设置路点运动时最大旋转速度
        [DllImport("eltrobot.dll")]
        public static extern int elt_set_waypoint_max_rotate_speed(IntPtr ctx, double speed, ref elt_error err);
        //添加路点信息
        [DllImport("eltrobot.dll")]
        public static extern int elt_add_waypoint(IntPtr ctx, double [] waypoint_array, ref elt_error err);
        //清除路点信息
        [DllImport("eltrobot.dll")]
        public static extern int elt_clear_waypoint(IntPtr ctx, ref elt_error err);
        //轨迹运动
        [DllImport("eltrobot.dll")]
        public static extern int elt_track_move(IntPtr ctx, int move_type, int pl, ref elt_error err);
        //停止机器人运行
        [DllImport("eltrobot.dll")]
        public static extern int elt_stop(IntPtr ctx, ref elt_error err);
        //机器人自动运行
        [DllImport("eltrobot.dll")]
        public static extern int elt_run(IntPtr ctx, ref elt_error err);
        //机器人暂停
        [DllImport("eltrobot.dll")]
        public static extern int elt_pause(IntPtr ctx, ref elt_error err);
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
         * 关节运动
         */ 
         public bool jointMove(IntPtr ctx, double [] target_pos_array, double speed)
        {
            elt_error err = new elt_error();
            int ret = elt_joint_move(ctx, target_pos_array, speed, ref err);
            return outputRet(ret, err, "elt_joint_move");
        }
        /*
         * 直线运动
         */ 
         public bool lineMove(IntPtr ctx, double [] target_pos_array, double speed)
        {
            elt_error err = new elt_error();
            int ret = elt_line_move(ctx, target_pos_array, speed, ref err);
            return outputRet(ret, err, "elt_line_move");
        }
        /*
         * 圆弧运动
         */ 
         public bool arcMove(IntPtr ctx, double [] middle_target_pos_array, double [] target_pos_array, double speed)
        {
            elt_error err = new elt_error();
            int ret = elt_arc_move(ctx, middle_target_pos_array, target_pos_array, speed, ref err);
            return outputRet(ret, err, "elt_arc_move");
        }
        /*
         * 旋转运动
         */ 
         bool rotateMove(IntPtr ctx, double [] target_pos_array, double speed)
        {
            elt_error err = new elt_error();
            int ret = elt_rotate_move(ctx, target_pos_array, speed, ref err);
            return outputRet(ret, err, "elt_rotate_move");
        }
        /*
         * 设置路点运动时最大关节速度
         */ 
         bool setWaypointMaxJointSpeed(IntPtr ctx, double speed)
        {
            elt_error err = new elt_error();
            int ret = elt_set_waypoint_max_joint_speed(ctx, speed, ref err);
            return outputRet(ret, err, "elt_set_waypoint_max_joint_speed");
        }
        /*
         * 设置路点运动时最大直线速度
         */ 
         bool setWaypointMaxLineSpeed(IntPtr ctx, double speed)
        {
            elt_error err = new elt_error();
            int ret = elt_set_waypoint_max_line_speed(ctx, speed, ref err);
            return outputRet(ret, err, "elt_set_waypoint_max_line_speed");
        }
        /*
         * 设置路点运动时最大旋转速度
         */
         bool setWaypointMaxRotateSpeed(IntPtr ctx, double speed)
        {
            elt_error err = new elt_error();
            int ret = elt_set_waypoint_max_rotate_speed(ctx, speed, ref err);
            return outputRet(ret, err, "elt_set_waypoint_max_rotate_speed");
        }
        /*
         * 添加路点信息
         */ 
         bool addWaypoint(IntPtr ctx, double [] waypoint_array)
        {
            elt_error err = new elt_error();
            int ret = elt_add_waypoint(ctx, waypoint_array, ref err);
            return outputRet(ret, err, "elt_add_waypoint");
        }
        /*
         * 清除路点信息
         */ 
         bool clearWaypoint(IntPtr ctx)
        {
            elt_error err = new elt_error();
            int ret = elt_clear_waypoint(ctx, ref err);
            return outputRet(ret, err, "elt_clear_waypoint");
        }
        /*
         * 轨迹运动
         */ 
         bool trackMove(IntPtr ctx, int move_type, int pl)
        {
            elt_error err = new elt_error();
            int ret = elt_track_move(ctx, move_type, pl, ref err);
            return outputRet(ret, err, "elt_track_move");
        }
        /*
         * 停止机器人运动
         */ 
         bool stopRobot(IntPtr ctx)
        {
            elt_error err = new elt_error();
            int ret = elt_stop(ctx, ref err);
            return outputRet(ret, err, "elt_stop");
        }
        /*
         * 机器人自动运行
         */ 
         bool runRobot(IntPtr ctx)
        {
            elt_error err = new elt_error();
            int ret = elt_run(ctx, ref err);
            return outputRet(ret, err, "elt_run");
        }
        /*
         * 机器人暂停
         */ 
         bool pauseRobot(IntPtr ctx)
        {
            elt_error err = new elt_error();
            int ret = elt_pause(ctx, ref err);
            return outputRet(ret, err, "elt_pause");
        }
        /*
         * 关节运动测试
         */ 
         public bool jointMoveTest()
        {
            Context context = new Context();
            ServoService servoService = new ServoService();
            ParamService paramService = new ParamService();
            IntPtr ctx = context.SDKCreate();
            Random rd = new Random();
            if (null == ctx)
                goto errlabel;
            if (!context.SDKLogin(ctx))
                goto errlabel;
            double[] start_pos_array = {0,-90,0,0,90,0,0,0};
            // 同步编码器
            int status = servoService.getMotorStatus(ctx);
            if (-1 == status)
                goto errlabel;
            if(0 == status)
            {
                if (!servoService.syncMotorStatus(ctx))
                    goto errlabel;
                Thread.Sleep(1500);
            }
            // 打开伺服抱闸
            status = servoService.getServoStatus(ctx);
            if (-1 == status)
                goto errlabel;
            if(0 == status)
            {
                if (!servoService.setServoStatus(ctx, 1))
                    goto errlabel;
                Thread.Sleep(1000);
            }
            //速度为-1测试
            if (jointMove(ctx, start_pos_array, -1))
            {
                Console.WriteLine("speed = -1 test error.");
                goto errlabel;
            }
            //速度为101测试
            if (jointMove(ctx, start_pos_array, 101))
            {
                Console.WriteLine("speed = 101 test error.");
                goto errlabel;
            }
            // 前进到起始点
            if (!jointMove(ctx, start_pos_array, 20))
                goto errlabel;
            while (true)
            {
                status = paramService.getRobotState(ctx);
                if (-1 == status)
                    goto errlabel;
                if (0 == status)
                    break;
                Thread.Sleep(300);
            }
            // 关节运动到新位置
            start_pos_array[0] += 90;
            if (!jointMove(ctx, start_pos_array, 20))
                goto errlabel;
            while (true)
            {
                status = paramService.getRobotState(ctx);
                if (-1 == status)
                    goto errlabel;
                if (0 == status)
                    break;
                Thread.Sleep(300);
            }
            //运动回起始点
            start_pos_array[0] -= 90;
            if (!jointMove(ctx, start_pos_array, 20))
                goto errlabel;
            while (true)
            {
                status = paramService.getRobotState(ctx);
                if (-1 == status)
                    goto errlabel;
                if (0 == status)
                    break;
                Thread.Sleep(300);
            }
            if (!context.SDKLogout(ctx))
            {
                goto errlabel;
            }
            if (!context.SDKDestroy(ctx))
                goto errlabel;
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test joint move success.");
            Console.WriteLine("=====================================================================");
            return true;
        errlabel:
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test joint move failed.");
            Console.WriteLine("=====================================================================");
            return false;
        }
        /*
         * 直线运动测试
         */
        public bool lineMoveTest()
        {
            Context context = new Context();
            ServoService servoService = new ServoService();
            ParamService paramService = new ParamService();
            IntPtr ctx = context.SDKCreate();
            KinematicsService kinematicService = new KinematicsService();
            Random rd = new Random();
            if (null == ctx)
                goto errlabel;
            if (!context.SDKLogin(ctx))
                goto errlabel;
            double[] start_pos_array = { 0, -90, 0, 0, 90, 0, 0, 0 };
            double[] target_pos_array = new double[8];
            double[] pose_array = new double[6];
            // 同步编码器
            int status = servoService.getMotorStatus(ctx);
            if (-1 == status)
                goto errlabel;
            if (0 == status)
            {
                if (!servoService.syncMotorStatus(ctx))
                    goto errlabel;
                Thread.Sleep(1500);
            }
            // 打开伺服抱闸
            status = servoService.getServoStatus(ctx);
            if (-1 == status)
                goto errlabel;
            if (0 == status)
            {
                if (!servoService.setServoStatus(ctx, 1))
                    goto errlabel;
                Thread.Sleep(1000);
            }
            // 关节前进到起始点
            if (!jointMove(ctx, start_pos_array, 20))
                goto errlabel;
            while (true)
            {
                status = paramService.getRobotState(ctx);
                if (-1 == status)
                    goto errlabel;
                if (0 == status)
                    break;
                Thread.Sleep(300);
            }
            if (!paramService.getRobotPose(ctx, pose_array))
            {
                Console.WriteLine("get robot pose error.");
                goto errlabel;
            }
            pose_array[0] -= 100;
            pose_array[1] -= 100;
            pose_array[2] -= 100;
            if(!kinematicService.inverseKinematic(ctx, pose_array, target_pos_array))
            {
                Console.WriteLine("inverseKinematic error.");
                goto errlabel;
            }
            //速度为-1测试
            if (lineMove(ctx, target_pos_array, -1))
            {
                Console.WriteLine("speed = -1 test error.");
                goto errlabel;
            }
            //速度为101测试
            /*if (lineMove(ctx, start_pos_array, 101))
            {
                Console.WriteLine("speed = 101 test error.");
                goto errlabel;
            }*/
            // 关节运动到新位置
            if (!lineMove(ctx, target_pos_array, 100))
                goto errlabel;
            while (true)
            {
                status = paramService.getRobotState(ctx);
                if (-1 == status)
                    goto errlabel;
                if (0 == status)
                    break;
                Thread.Sleep(300);
            }
            //运动回起始点
            if (!jointMove(ctx, start_pos_array, 20))
                goto errlabel;
            while (true)
            {
                status = paramService.getRobotState(ctx);
                if (-1 == status)
                    goto errlabel;
                if (0 == status)
                    break;
                Thread.Sleep(300);
            }
            if (!context.SDKLogout(ctx))
            {
                goto errlabel;
            }
            if (!context.SDKDestroy(ctx))
                goto errlabel;
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test line move success.");
            Console.WriteLine("=====================================================================");
            return true;
        errlabel:
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test line move failed.");
            Console.WriteLine("=====================================================================");
            return false;
        }
        /*
         * 圆弧运动测试
         */
        public bool arcMoveTest()
        {
            Context context = new Context();
            ServoService servoService = new ServoService();
            ParamService paramService = new ParamService();
            IntPtr ctx = context.SDKCreate();
            KinematicsService kinematicService = new KinematicsService();
            Random rd = new Random();
            if (null == ctx)
                goto errlabel;
            if (!context.SDKLogin(ctx))
                goto errlabel;
            double[] start_pos_array = { 0, -90, 24, 0, 62, 0, 0, 0 };
            double[] middle_pos_array = new double[8];
            double[] target_pos_array = new double[8];
            double[] pose_array = new double[6];
            // 同步编码器
            int status = servoService.getMotorStatus(ctx);
            if (-1 == status)
                goto errlabel;
            if (0 == status)
            {
                if (!servoService.syncMotorStatus(ctx))
                    goto errlabel;
                Thread.Sleep(1500);
            }
            // 打开伺服抱闸
            status = servoService.getServoStatus(ctx);
            if (-1 == status)
                goto errlabel;
            if (0 == status)
            {
                if (!servoService.setServoStatus(ctx, 1))
                    goto errlabel;
                Thread.Sleep(1000);
            }
            // 关节前进到起始点
            if (!jointMove(ctx, start_pos_array, 20))
                goto errlabel;
            while (true)
            {
                status = paramService.getRobotState(ctx);
                if (-1 == status)
                    goto errlabel;
                if (0 == status)
                    break;
                Thread.Sleep(300);
            }
            if (!paramService.getRobotPose(ctx, pose_array))
            {
                Console.WriteLine("get robot pose error.");
                goto errlabel;
            }
            pose_array[0] += 50;
            pose_array[1] += 100;
            if (!kinematicService.inverseKinematic(ctx, pose_array, middle_pos_array))
            {
                Console.WriteLine("inverseKinematic error.");
                goto errlabel;
            }
            pose_array[0] += 50;
            pose_array[1] -= 100;
            if (!kinematicService.inverseKinematic(ctx, pose_array, target_pos_array))
            {
                Console.WriteLine("inverseKinematic error.");
                goto errlabel;
            }
            //速度为-1测试
            if (arcMove(ctx,middle_pos_array, target_pos_array, -1))
            {
                Console.WriteLine("speed = -1 test error.");
                goto errlabel;
            }
            //速度为101测试
            /*if (lineMove(ctx, start_pos_array, 101))
            {
                Console.WriteLine("speed = 101 test error.");
                goto errlabel;
            }*/
            // 圆弧运动到新位置
            if (!arcMove(ctx, middle_pos_array, target_pos_array, 100))
                goto errlabel;
            while (true)
            {
                status = paramService.getRobotState(ctx);
                if (-1 == status)
                    goto errlabel;
                if (0 == status)
                    break;
                Thread.Sleep(300);
            }
            start_pos_array[2] = 0; start_pos_array[4] = 90;
            //运动回起始点
            if (!jointMove(ctx, start_pos_array, 20))
                goto errlabel;
            while (true)
            {
                status = paramService.getRobotState(ctx);
                if (-1 == status)
                    goto errlabel;
                if (0 == status)
                    break;
                Thread.Sleep(300);
            }
            if (!context.SDKLogout(ctx))
            {
                goto errlabel;
            }
            if (!context.SDKDestroy(ctx))
                goto errlabel;
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test arc move success.");
            Console.WriteLine("=====================================================================");
            return true;
        errlabel:
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test arc move failed.");
            Console.WriteLine("=====================================================================");
            return false;
        }
        /*
         * 旋转运动测试
         */
        public bool rotateMoveTest()
        {
            Context context = new Context();
            ServoService servoService = new ServoService();
            ParamService paramService = new ParamService();
            IntPtr ctx = context.SDKCreate();
            KinematicsService kinematicService = new KinematicsService();
            Random rd = new Random();
            if (null == ctx)
                goto errlabel;
            if (!context.SDKLogin(ctx))
                goto errlabel;
            double[] start_pos_array = { 0, -90, 0, 0, 90, 0, 0, 0 };
            double[] target_pos_array = new double[8];
            double[] pose_array = new double[6];
            // 同步编码器
            int status = servoService.getMotorStatus(ctx);
            if (-1 == status)
                goto errlabel;
            if (0 == status)
            {
                if (!servoService.syncMotorStatus(ctx))
                    goto errlabel;
                Thread.Sleep(1500);
            }
            // 打开伺服抱闸
            status = servoService.getServoStatus(ctx);
            if (-1 == status)
                goto errlabel;
            if (0 == status)
            {
                if (!servoService.setServoStatus(ctx, 1))
                    goto errlabel;
                Thread.Sleep(1000);
            }
            // 关节前进到起始点
            if (!jointMove(ctx, start_pos_array, 20))
                goto errlabel;
            while (true)
            {
                status = paramService.getRobotState(ctx);
                if (-1 == status)
                    goto errlabel;
                if (0 == status)
                    break;
                Thread.Sleep(300);
            }
            if (!paramService.getRobotPose(ctx, pose_array))
            {
                Console.WriteLine("get robot pose error.");
                goto errlabel;
            }
            pose_array[3] += 50;
            pose_array[4] += 50;
            pose_array[5] += 50;
            if (!kinematicService.inverseKinematic(ctx, pose_array, target_pos_array))
            {
                Console.WriteLine("inverseKinematic error.");
                goto errlabel;
            }
            //速度为-1测试
            if (rotateMove(ctx, target_pos_array, -1))
            {
                Console.WriteLine("speed = -1 test error.");
                goto errlabel;
            }
            //速度为101测试
            /*if (lineMove(ctx, start_pos_array, 101))
            {
                Console.WriteLine("speed = 101 test error.");
                goto errlabel;
            }*/
            // 旋转运动到新位置
            if (!rotateMove(ctx, target_pos_array, 30))
                goto errlabel;
            while (true)
            {
                status = paramService.getRobotState(ctx);
                if (-1 == status)
                    goto errlabel;
                if (0 == status)
                    break;
                Thread.Sleep(300);
            }
            //关节运动回起始点
            if (!jointMove(ctx, start_pos_array, 20))
                goto errlabel;
            while (true)
            {
                status = paramService.getRobotState(ctx);
                if (-1 == status)
                    goto errlabel;
                if (0 == status)
                    break;
                Thread.Sleep(300);
            }
            if (!context.SDKLogout(ctx))
            {
                goto errlabel;
            }
            if (!context.SDKDestroy(ctx))
                goto errlabel;
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test rotate move success.");
            Console.WriteLine("=====================================================================");
            return true;
        errlabel:
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test rotate move failed.");
            Console.WriteLine("=====================================================================");
            return false;
        }
        /*
         * 关节路点运动测试
         */
        public bool jointTrackMoveTest()
        {
            if (!jointTrackMovegenericTest())
            {
                Console.WriteLine("joint track move test failed.");
                goto errlabel;
            }
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test joint track move success.");
            Console.WriteLine("=====================================================================");
            return true;
        errlabel:
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test joint track move failed.");
            Console.WriteLine("=====================================================================");
            return false;
        }
        /*
         * 关节路点运行测试。常规测试
         */
        public bool jointTrackMovegenericTest()
        {
            Context context = new Context();
            ServoService servoService = new ServoService();
            ParamService paramService = new ParamService();
            IntPtr ctx = context.SDKCreate();
            KinematicsService kinematicService = new KinematicsService();
            Random rd = new Random();
            if (null == ctx)
                goto errlabel;
            if (!context.SDKLogin(ctx))
                goto errlabel;
            double[] start_pos_array = { 0, -90, 0, 0, 90, 0, 0, 0 };
            double[] target_pos_array = new double[8];
            double[] pose_array = new double[6];
            // 同步编码器
            int status = servoService.getMotorStatus(ctx);
            if (-1 == status)
                goto errlabel;
            if (0 == status)
            {
                if (!servoService.syncMotorStatus(ctx))
                    goto errlabel;
                Thread.Sleep(1500);
            }
            // 打开伺服抱闸
            status = servoService.getServoStatus(ctx);
            if (-1 == status)
                goto errlabel;
            if (0 == status)
            {
                if (!servoService.setServoStatus(ctx, 1))
                    goto errlabel;
                Thread.Sleep(1000);
            }
            // 关节前进到起始点
            if (!jointMove(ctx, start_pos_array, 20))
                goto errlabel;
            while (true)
            {
                status = paramService.getRobotState(ctx);
                if (-1 == status)
                    goto errlabel;
                if (0 == status)
                    break;
                Thread.Sleep(300);
            }
            //设置路点运动时最大关节运行速度
            if(setWaypointMaxJointSpeed(ctx, -1))
            {
                Console.WriteLine("set way point max joint speed value = -1 test error");
                goto errlabel;
            }
            if(setWaypointMaxJointSpeed(ctx, 101))
            {
                Console.WriteLine("set way point max joint speed value = 101 test error");
                goto errlabel;
            }
            if(!setWaypointMaxJointSpeed(ctx, 30))
            {
                Console.WriteLine("set way point max joint speed value = 30 test error");
                goto errlabel;
            }
            //清空路点
            if (!clearWaypoint(ctx))
            {
                Console.WriteLine("clear way point error.");
                goto errlabel;
            }
            //添加路点
            if(!addWaypoint(ctx, start_pos_array))
                goto errlabel;
            start_pos_array[0] = 90;
            if (!addWaypoint(ctx, start_pos_array))
                goto errlabel;
            start_pos_array[0] = -90;
            if (!addWaypoint(ctx, start_pos_array))
                goto errlabel;
            start_pos_array[0] = 0;
            if (!addWaypoint(ctx, start_pos_array))
                goto errlabel;
            //路点运动类型为-1测试
            if(trackMove(ctx, -1, 0))
            {
                Console.WriteLine("track move type = -1 test error.");
                goto errlabel;
            }
            /*//路点运动类型为-1测试
            if (trackMove(ctx, 3, 0))
            {
                Console.WriteLine("track move type = 3 test error.");
                goto errlabel;
            }
            //路点运动pl为-1测试
            if(trackMove(ctx,0,-1))
            {
                Console.WriteLine("track move pl = -1 test error.");
                goto errlabel;
            }
            //路点运动pl为8测试
            if (trackMove(ctx, 0, 8))
            {
                Console.WriteLine("track move pl = 8 test error.");
                goto errlabel;
            }*/
            //常规路点运动
            if(!trackMove(ctx, 0, 7))
            {
                Console.WriteLine("track move generic test error.");
                goto errlabel;
            }
            while (true)
            {
                Thread.Sleep(1000);
                status = paramService.getRobotState(ctx);
                if (-1 == status)
                    goto errlabel;
                if (0 == status)
                    break;
            }
            //关节运动回起始点
            if (!jointMove(ctx, start_pos_array, 20))
                goto errlabel;
            while (true)
            {
                status = paramService.getRobotState(ctx);
                if (-1 == status)
                    goto errlabel;
                if (0 == status)
                    break;
                Thread.Sleep(300);
            }
            if (!context.SDKLogout(ctx))
            {
                goto errlabel;
            }
            if (!context.SDKDestroy(ctx))
                goto errlabel;
            return true;
        errlabel:
            return false;
        }
        //关节路点运行测试。未设置速度，直接运行测试
        //关节路点运行测试。未添加路点，直接运行测试
        /*
         * 直线路点运动测试
         */
        public bool lineTrackMoveTest()
        {
            if (!lineTrackMovegenericTest())
            {
                Console.WriteLine("line track move test failed.");
                goto errlabel;
            }
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test line track move success.");
            Console.WriteLine("=====================================================================");
            return true;
        errlabel:
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test line track move failed.");
            Console.WriteLine("=====================================================================");
            return false;
        }
        /*
         * 直线路点运行测试。常规测试
         */
        public bool lineTrackMovegenericTest()
        {
            Context context = new Context();
            ServoService servoService = new ServoService();
            ParamService paramService = new ParamService();
            IntPtr ctx = context.SDKCreate();
            KinematicsService kinematicService = new KinematicsService();
            Random rd = new Random();
            if (null == ctx)
                goto errlabel;
            if (!context.SDKLogin(ctx))
                goto errlabel;
            double[] start_pos_array = { 0, -90, 0, 0, 90, 0, 0, 0 };
            double[] target_pos_array = new double[8];
            double[] pose_array = new double[6];
            double[] response_pos_array = new double[8];
            // 同步编码器
            int status = servoService.getMotorStatus(ctx);
            if (-1 == status)
                goto errlabel;
            if (0 == status)
            {
                if (!servoService.syncMotorStatus(ctx))
                    goto errlabel;
                Thread.Sleep(1500);
            }
            // 打开伺服抱闸
            status = servoService.getServoStatus(ctx);
            if (-1 == status)
                goto errlabel;
            if (0 == status)
            {
                if (!servoService.setServoStatus(ctx, 1))
                    goto errlabel;
                Thread.Sleep(1000);
            }
            // 关节前进到起始点
            if (!jointMove(ctx, start_pos_array, 20))
                goto errlabel;
            while (true)
            {
                status = paramService.getRobotState(ctx);
                if (-1 == status)
                    goto errlabel;
                if (0 == status)
                    break;
                Thread.Sleep(300);
            }
            //设置路点运动时最大直线运行速度
            if (setWaypointMaxLineSpeed(ctx, -1))
            {
                Console.WriteLine("set way point max joint speed value = -1 test error");
                goto errlabel;
            }
            if (!setWaypointMaxLineSpeed(ctx, 100))
            {
                Console.WriteLine("set way point max joint speed value = 30 test error");
                goto errlabel;
            }
            //清空路点
            if (!clearWaypoint(ctx))
            {
                Console.WriteLine("clear way point error.");
                goto errlabel;
            }
            //添加路点
            if (!paramService.getRobotPose(ctx, pose_array))
            {
                Console.WriteLine("get robot pose error.");
                goto errlabel;
            }
            if (!kinematicService.inverseKinematic(ctx, pose_array, response_pos_array))
            {
                Console.WriteLine("inverseKinematic error.");
                goto errlabel;
            }
            if (!addWaypoint(ctx, response_pos_array))
                goto errlabel;
            pose_array[0] += 100;
            pose_array[1] -= 100;
            pose_array[2] -= 100;
            if (!kinematicService.inverseKinematic(ctx, pose_array, response_pos_array))
            {
                Console.WriteLine("inverseKinematic error.");
                goto errlabel;
            }
            if (!addWaypoint(ctx, response_pos_array))
                goto errlabel;
            //路点运动类型为-1测试
            if (trackMove(ctx, -1, 0))
            {
                Console.WriteLine("track move type = -1 test error.");
                goto errlabel;
            }
            //常规路点运动
            if (!trackMove(ctx, 1, 7))
            {
                Console.WriteLine("track move generic test error.");
                goto errlabel;
            }
            while (true)
            {
                Thread.Sleep(1000);
                status = paramService.getRobotState(ctx);
                if (-1 == status)
                    goto errlabel;
                if (0 == status)
                    break;
            }
            //关节运动回起始点
            if (!jointMove(ctx, start_pos_array, 20))
                goto errlabel;
            while (true)
            {
                status = paramService.getRobotState(ctx);
                if (-1 == status)
                    goto errlabel;
                if (0 == status)
                    break;
                Thread.Sleep(300);
            }
            if (!context.SDKLogout(ctx))
            {
                goto errlabel;
            }
            if (!context.SDKDestroy(ctx))
                goto errlabel;
            return true;
        errlabel:
            return false;
        }
        //直线路点运行测试。未设置速度，直接运行测试
        //直线路点运行测试。未添加路点，直接运行测试
        /*
         * 旋转路点运动测试
         */
        public bool rotateTrackMoveTest()
        {
            if (!rotateTrackMovegenericTest())
            {
                Console.WriteLine("line track move test failed.");
                goto errlabel;
            }
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test rotate track move success.");
            Console.WriteLine("=====================================================================");
            return true;
        errlabel:
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test rotate track move failed.");
            Console.WriteLine("=====================================================================");
            return false;
        }
        /*
         * 旋转路点运行测试。常规测试
         */
        public bool rotateTrackMovegenericTest()
        {
            Context context = new Context();
            ServoService servoService = new ServoService();
            ParamService paramService = new ParamService();
            IntPtr ctx = context.SDKCreate();
            KinematicsService kinematicService = new KinematicsService();
            Random rd = new Random();
            if (null == ctx)
                goto errlabel;
            if (!context.SDKLogin(ctx))
                goto errlabel;
            double[] start_pos_array = { 0, -90, 0, 0, 90, 0, 0, 0 };
            double[] target_pos_array = new double[8];
            double[] pose_array = new double[6];
            double[] response_pos_array = new double[8];
            // 同步编码器
            int status = servoService.getMotorStatus(ctx);
            if (-1 == status)
                goto errlabel;
            if (0 == status)
            {
                if (!servoService.syncMotorStatus(ctx))
                    goto errlabel;
                Thread.Sleep(1500);
            }
            // 打开伺服抱闸
            status = servoService.getServoStatus(ctx);
            if (-1 == status)
                goto errlabel;
            if (0 == status)
            {
                if (!servoService.setServoStatus(ctx, 1))
                    goto errlabel;
                Thread.Sleep(1000);
            }
            // 关节前进到起始点
            if (!jointMove(ctx, start_pos_array, 20))
                goto errlabel;
            while (true)
            {
                status = paramService.getRobotState(ctx);
                if (-1 == status)
                    goto errlabel;
                if (0 == status)
                    break;
                Thread.Sleep(300);
            }
            //设置路点运动时最大旋转运行速度
            if (setWaypointMaxRotateSpeed(ctx, -1))
            {
                Console.WriteLine("set way point max joint speed value = -1 test error");
                goto errlabel;
            }
            if (!setWaypointMaxRotateSpeed(ctx, 30))
            {
                Console.WriteLine("set way point max joint speed value = 30 test error");
                goto errlabel;
            }
            //清空路点
            if (!clearWaypoint(ctx))
            {
                Console.WriteLine("clear way point error.");
                goto errlabel;
            }
            //添加路点
            if (!paramService.getRobotPose(ctx, pose_array))
            {
                Console.WriteLine("get robot pose error.");
                goto errlabel;
            }
            if (!kinematicService.inverseKinematic(ctx, pose_array, response_pos_array))
            {
                Console.WriteLine("inverseKinematic error.");
                goto errlabel;
            }
            if (!addWaypoint(ctx, response_pos_array))
                goto errlabel;
            pose_array[3] += 30*3.14/180.0;
            pose_array[5] -= 30*3.14/180.0;
            pose_array[2] -= 30*3.14/180.0;
            if (!kinematicService.inverseKinematic(ctx, pose_array, response_pos_array))
            {
                Console.WriteLine("inverseKinematic error.");
                goto errlabel;
            }
            if (!addWaypoint(ctx, response_pos_array))
                goto errlabel;
            //路点运动类型为-1测试
            if (trackMove(ctx, -1, 0))
            {
                Console.WriteLine("track move type = -1 test error.");
                goto errlabel;
            }
            //常规路点运动
            if (!trackMove(ctx, 2, 7))
            {
                Console.WriteLine("track move generic test error.");
                goto errlabel;
            }
            while (true)
            {
                Thread.Sleep(1000);
                status = paramService.getRobotState(ctx);
                if (-1 == status)
                    goto errlabel;
                if (0 == status)
                    break;
            }
            //关节运动回起始点
            if (!jointMove(ctx, start_pos_array, 20))
                goto errlabel;
            while (true)
            {
                status = paramService.getRobotState(ctx);
                if (-1 == status)
                    goto errlabel;
                if (0 == status)
                    break;
                Thread.Sleep(300);
            }
            if (!context.SDKLogout(ctx))
            {
                goto errlabel;
            }
            if (!context.SDKDestroy(ctx))
                goto errlabel;
            return true;
        errlabel:
            return false;
        }
        //旋转路点运行测试。未设置速度，直接运行测试
        //旋转路点运行测试。未添加路点，直接运行测试
        /*
         * 圆弧路点运动测试
         */
        public bool arcTrackMoveTest()
        {
            if (!arcTrackMovegenericTest())
            {
                Console.WriteLine("arc track move test failed.");
                goto errlabel;
            }
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test arc track move success.");
            Console.WriteLine("=====================================================================");
            return true;
        errlabel:
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test arc track move failed.");
            Console.WriteLine("=====================================================================");
            return false;
        }
        /*
         * 圆弧路点运行测试。常规测试
         */
        public bool arcTrackMovegenericTest()
        {
            Context context = new Context();
            ServoService servoService = new ServoService();
            ParamService paramService = new ParamService();
            IntPtr ctx = context.SDKCreate();
            KinematicsService kinematicService = new KinematicsService();
            Random rd = new Random();
            if (null == ctx)
                goto errlabel;
            if (!context.SDKLogin(ctx))
                goto errlabel;
            double[] start_pos_array = { 0, -90, 0, 0, 90, 0, 0, 0 };
            double[] target_pos_array = new double[8];
            double[] pose_array = new double[6];
            double[] response_pos_array = new double[8];
            // 同步编码器
            int status = servoService.getMotorStatus(ctx);
            if (-1 == status)
                goto errlabel;
            if (0 == status)
            {
                if (!servoService.syncMotorStatus(ctx))
                    goto errlabel;
                Thread.Sleep(1500);
            }
            // 打开伺服抱闸
            status = servoService.getServoStatus(ctx);
            if (-1 == status)
                goto errlabel;
            if (0 == status)
            {
                if (!servoService.setServoStatus(ctx, 1))
                    goto errlabel;
                Thread.Sleep(1000);
            }
            // 关节前进到起始点
            if (!jointMove(ctx, start_pos_array, 20))
                goto errlabel;
            while (true)
            {
                status = paramService.getRobotState(ctx);
                if (-1 == status)
                    goto errlabel;
                if (0 == status)
                    break;
                Thread.Sleep(300);
            }
            //设置路点运动时最大直线运行速度
            if (setWaypointMaxLineSpeed(ctx, -1))
            {
                Console.WriteLine("set way point max line speed value = -1 test error");
                goto errlabel;
            }
            if (!setWaypointMaxLineSpeed(ctx, 100))
            {
                Console.WriteLine("set way point max line speed value = 100 test error");
                goto errlabel;
            }
            //清空路点
            if (!clearWaypoint(ctx))
            {
                Console.WriteLine("clear way point error.");
                goto errlabel;
            }
            //添加路点
            if (!paramService.getRobotPose(ctx, pose_array))
            {
                Console.WriteLine("get robot pose error.");
                goto errlabel;
            }
            Console.WriteLine("{0},{0},{0},{0},{0},{0}", pose_array[0], pose_array[1], pose_array[2], pose_array[3], pose_array[4], pose_array[5]);
            if (!kinematicService.inverseKinematic(ctx, pose_array, response_pos_array))
            {
                Console.WriteLine("inverseKinematic error.");
                goto errlabel;
            }
            if (!addWaypoint(ctx, response_pos_array))
                goto errlabel;
            pose_array[2] -= 100;
            if (!kinematicService.inverseKinematic(ctx, pose_array, response_pos_array))
            {
                Console.WriteLine("inverseKinematic error.");
                goto errlabel;
            }
            if (!addWaypoint(ctx, response_pos_array))
                goto errlabel;
            pose_array[0] += 50;
            pose_array[1] += 100;
            if (!kinematicService.inverseKinematic(ctx, pose_array, response_pos_array))
            {
                Console.WriteLine("inverseKinematic error.");
                goto errlabel;
            }
            if (!addWaypoint(ctx, response_pos_array))
                goto errlabel;
            pose_array[0] += 50;
            pose_array[1] -= 100;
            if (!kinematicService.inverseKinematic(ctx, pose_array, response_pos_array))
            {
                Console.WriteLine("inverseKinematic error.");
                goto errlabel;
            }
            pose_array[0] -= 100;
            if (!addWaypoint(ctx, response_pos_array))
                goto errlabel;
            //路点运动类型为-1测试
            if (trackMove(ctx, -1, 0))
            {
                Console.WriteLine("track move type = -1 test error.");
                goto errlabel;
            }
            //常规路点运动
            if (!trackMove(ctx, 2, 7))
            {
                Console.WriteLine("track move generic test error.");
                goto errlabel;
            }
            while (true)
            {
                Thread.Sleep(1000);
                status = paramService.getRobotState(ctx);
                if (-1 == status)
                    goto errlabel;
                if (0 == status)
                    break;
            }
            //关节运动回起始点
            if (!jointMove(ctx, start_pos_array, 20))
                goto errlabel;
            while (true)
            {
                status = paramService.getRobotState(ctx);
                if (-1 == status)
                    goto errlabel;
                if (0 == status)
                    break;
                Thread.Sleep(300);
            }
            if (!context.SDKLogout(ctx))
            {
                goto errlabel;
            }
            if (!context.SDKDestroy(ctx))
                goto errlabel;
            return true;
        errlabel:
            return false;
        }
        //圆弧路点运行测试。未设置速度，直接运行测试
        //圆弧路点运行测试。未添加路点，直接运行测试
        /*
         * 路点运行过程中启动暂停测试
         */ 
        public bool runstopMoveTest()
        {
            Context context = new Context();
            ServoService servoService = new ServoService();
            ParamService paramService = new ParamService();
            IntPtr ctx = context.SDKCreate();
            KinematicsService kinematicService = new KinematicsService();
            Random rd = new Random();
            if (null == ctx)
                goto errlabel;
            if (!context.SDKLogin(ctx))
                goto errlabel;
            double[] start_pos_array = { 0, -90, 0, 0, 90, 0, 0, 0 };
            double[] target_pos_array = new double[8];
            double[] pose_array = new double[6];
            // 同步编码器
            int status = servoService.getMotorStatus(ctx);
            if (-1 == status)
                goto errlabel;
            if (0 == status)
            {
                if (!servoService.syncMotorStatus(ctx))
                    goto errlabel;
                Thread.Sleep(1500);
            }
            // 打开伺服抱闸
            status = servoService.getServoStatus(ctx);
            if (-1 == status)
                goto errlabel;
            if (0 == status)
            {
                if (!servoService.setServoStatus(ctx, 1))
                    goto errlabel;
                Thread.Sleep(1000);
            }
            // 关节前进到起始点
            if (!jointMove(ctx, start_pos_array, 20))
                goto errlabel;
            while (true)
            {
                status = paramService.getRobotState(ctx);
                if (-1 == status)
                    goto errlabel;
                if (0 == status)
                    break;
                Thread.Sleep(300);
            }
            //设置路点运动时最大关节运行速度
            if (!setWaypointMaxJointSpeed(ctx, 30))
            {
                Console.WriteLine("set way point max joint speed value = 30 test error");
                goto errlabel;
            }
            //清空路点
            if (!clearWaypoint(ctx))
            {
                Console.WriteLine("clear way point error.");
                goto errlabel;
            }
            //添加路点
            if (!addWaypoint(ctx, start_pos_array))
                goto errlabel;
            start_pos_array[0] = 90;
            if (!addWaypoint(ctx, start_pos_array))
                goto errlabel;
            start_pos_array[0] = -90;
            if (!addWaypoint(ctx, start_pos_array))
                goto errlabel;
            start_pos_array[0] = 0;
            if (!addWaypoint(ctx, start_pos_array))
                goto errlabel;
            start_pos_array[1] = -141;
            if (!addWaypoint(ctx, start_pos_array))
                goto errlabel;
            start_pos_array[1] = -36;
            if (!addWaypoint(ctx, start_pos_array))
                goto errlabel;
            start_pos_array[1] = -90;
            if (!addWaypoint(ctx, start_pos_array))
                goto errlabel;
            //三轴
            start_pos_array[2] = 39;
            if (!addWaypoint(ctx, start_pos_array))
                goto errlabel;
            start_pos_array[2] = -55;
            if (!addWaypoint(ctx, start_pos_array))
                goto errlabel;
            start_pos_array[2] = 0;
            if (!addWaypoint(ctx, start_pos_array))
                goto errlabel;
            //四轴
            start_pos_array[3] = 108;
            if (!addWaypoint(ctx, start_pos_array))
                goto errlabel;
            start_pos_array[3] = -92;
            if (!addWaypoint(ctx, start_pos_array))
                goto errlabel;
            start_pos_array[3] = 0;
            if (!addWaypoint(ctx, start_pos_array))
                goto errlabel;
            //五轴
            start_pos_array[4] = 101;
            if (!addWaypoint(ctx, start_pos_array))
                goto errlabel;
            start_pos_array[4] = 6;
            if (!addWaypoint(ctx, start_pos_array))
                goto errlabel;
            start_pos_array[4] = 90;
            if (!addWaypoint(ctx, start_pos_array))
                goto errlabel;
            //六轴
            start_pos_array[5] = -180;
            if (!addWaypoint(ctx, start_pos_array))
                goto errlabel;
            start_pos_array[5] = 180;
            if (!addWaypoint(ctx, start_pos_array))
                goto errlabel;
            start_pos_array[5] = 0;
            if (!addWaypoint(ctx, start_pos_array))
                goto errlabel;
            //常规路点运动
            if (!trackMove(ctx, 0, 7))
            {
                Console.WriteLine("track move generic test error.");
                goto errlabel;
            }
            //测试启动停止
            Thread.Sleep(2000);
            if (!pauseRobot(ctx))
            {
                Console.WriteLine("pause robot error.");
                goto errlabel;
            }
            Thread.Sleep(2000);
            if(!runRobot(ctx))
            {
                Console.WriteLine("run robot error.");
                goto errlabel;
            }
            Thread.Sleep(2000);
            if(!stopRobot(ctx))
            {
                Console.WriteLine("stop robot error.");
                goto errlabel;
            }
            Thread.Sleep(3000);
            //关节运动回起始点
            if (!jointMove(ctx, start_pos_array, 20))
                goto errlabel;
            while (true)
            {
                status = paramService.getRobotState(ctx);
                if (-1 == status)
                    goto errlabel;
                if (0 == status)
                    break;
                Thread.Sleep(300);
            }
            if (!context.SDKLogout(ctx))
            {
                goto errlabel;
            }
            if (!context.SDKDestroy(ctx))
                goto errlabel;
            return true;
        errlabel:
            return false;
        }
    }
}
