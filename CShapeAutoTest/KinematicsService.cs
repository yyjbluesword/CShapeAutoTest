using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SDKContext;
using SDKParamService;

namespace SDKKinematicsService
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
    class KinematicsService
    {
        //逆解函数
        [DllImport("eltrobot.dll")]
        unsafe public static extern int elt_inverse_kinematic(IntPtr ctx, double[] target_pose_array, double [] response_pos_array, ref elt_error err);
        //正解函数
        [DllImport("eltrobot.dll")]
        public static extern int elt_positive_kinematic(IntPtr ctx, double[] target_pos_array, double [] response_pose_array, ref elt_error err);
        //基坐标到用户坐标位姿转化
        [DllImport("eltrobot.dll")]
        public static extern int elt_base2user(IntPtr ctx, double[] base_pose_array, int user_no, double[] user_pose_array, ref elt_error err);
        //用户坐标到基坐标位姿转化
        [DllImport("eltrobot.dll")]
        public static extern int elt_user2base(IntPtr ctx, double[] user_pose_array, int user_no, double [] base_pose_array, ref elt_error err);
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
         * 逆解函数
         */ 
         unsafe public bool inverseKinematic(IntPtr ctx, double [] target_pose_array, double [] response_pos_array)
        {
            elt_error err = new elt_error();
            int ret = elt_inverse_kinematic(ctx, target_pose_array, response_pos_array, ref err);
            return outputRet(ret, err, "elt_inverse_kinematic");
        }
        /*
         * 正解函数
         */ 
         bool positiveKinematic(IntPtr ctx, double [] target_pos_array, double [] response_pose_array)
        {
            elt_error err = new elt_error();
            int ret = elt_positive_kinematic(ctx, target_pos_array, response_pose_array, ref err);
            return outputRet(ret, err, "elt_positive_kinematic");
        }
        /*
         * 基坐标到用户坐标位姿转化
         */ 
         bool base2User(IntPtr ctx, double [] base_pose_array, int user_no, double [] user_pose_array)
        {
            elt_error err = new elt_error();
            int ret = elt_base2user(ctx, base_pose_array, user_no, user_pose_array, ref err);
            return outputRet(ret, err, "elt_base2user");
        }
        /*
         * 用户坐标到基坐标位姿转化
         */
        bool user2Base(IntPtr ctx, double[] user_pose_array, int user_no, double[] base_pose_array)
        {
            elt_error err = new elt_error();
            int ret = elt_user2base(ctx, user_pose_array, user_no, base_pose_array, ref err);
            return outputRet(ret, err, "elt_user2base");
        }
        /*
         * 逆解函数测试
         */
        public bool inverseKinematicTest()
        {
            Context context = new Context();
            ParamService paramService = new ParamService();
            IntPtr ctx = context.SDKCreate();
            double[] pos_array = new double[8];
            double[] pose_array = new double[6];
            double[] response_pos_array = new double[8];
            if (null == ctx)
                goto errlabel;
            if (!context.SDKLogin(ctx))
                goto errlabel;
            if(!paramService.getRobotPos(ctx, pos_array))
            {
                Console.WriteLine("get robot pos error.");
                goto errlabel;
            }
            if(!paramService.getRobotPose(ctx, pose_array))
            {
                Console.WriteLine("get robot pose error.");
                goto errlabel;
            }
            if (!inverseKinematic(ctx, pose_array, response_pos_array))
            {
                Console.WriteLine("inverse kinematic error.");

            }
            for (int i = 0; i < 8; i++)
            {
                if(Math.Abs(pos_array[i] - response_pos_array[i])>0.01)
                {
                    Console.WriteLine("inverse kinematic value error.");
                    Console.WriteLine("pose_array = {0},{1},{2},{3},{4},{5}",
                        pose_array[0], pose_array[1], pose_array[2], pose_array[3],
                        pose_array[4], pose_array[5]);
                    Console.WriteLine("pos_array = {0},{1},{2},{3},{4},{5},{6},{7}",
                        pos_array[0], pos_array[1], pos_array[2], pos_array[3],
                        pos_array[4], pos_array[5], pos_array[6], pos_array[7]);
                    Console.WriteLine("res_array = {0},{1},{2},{3},{4},{5},{6},{7}",
                        response_pos_array[0], response_pos_array[1], response_pos_array[2], response_pos_array[3],
                        response_pos_array[4], response_pos_array[5], response_pos_array[6], response_pos_array[7]);
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
            Console.WriteLine("Test inverse kinematic success.");
            Console.WriteLine("=====================================================================");
            return true;
        errlabel:
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test inverse kinematic failed.");
            Console.WriteLine("=====================================================================");
            return false;
        }
        /*
         * 正解函数测试
         */
        public bool positiveKinematicTest()
        {
            Context context = new Context();
            ParamService paramService = new ParamService();
            IntPtr ctx = context.SDKCreate();
            double[] pos_array = new double[8];
            double[] pose_array = new double[6];
            double[] response_pose_array = new double[6];
            if (null == ctx)
                goto errlabel;
            if (!context.SDKLogin(ctx))
                goto errlabel;
            if (!paramService.getRobotPos(ctx, pos_array))
            {
                Console.WriteLine("get robot pos error.");
                goto errlabel;
            }
            if (!paramService.getRobotPose(ctx, pose_array))
            {
                Console.WriteLine("get robot pose error.");
                goto errlabel;
            }
            if (!positiveKinematic(ctx, pos_array, response_pose_array))
            {
                Console.WriteLine("positive kinematic error.");

            }
            for (int i = 0; i < 6; i++)
            {
                if (Math.Abs(pose_array[i] - response_pose_array[i]) > 0.01)
                {
                    Console.WriteLine("positive kinematic value error.");
                    Console.WriteLine("pose_array = {0},{1},{2},{3},{4},{5}",
                        pose_array[0], pose_array[1], pose_array[2], pose_array[3],
                        pose_array[4], pose_array[5]);
                    Console.WriteLine("pos_array = {0},{1},{2},{3},{4},{5},{6},{7}",
                        pos_array[0], pos_array[1], pos_array[2], pos_array[3],
                        pos_array[4], pos_array[5], pos_array[6], pos_array[7]);
                    Console.WriteLine("res_array = {0},{1},{2},{3},{4},{5},{6},{7}",
                        response_pose_array[0], response_pose_array[1], response_pose_array[2], response_pose_array[3],
                        response_pose_array[4], response_pose_array[5], response_pose_array[6], response_pose_array[7]);
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
            Console.WriteLine("Test positive kinematic success.");
            Console.WriteLine("=====================================================================");
            return true;
        errlabel:
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test positive kinematic failed.");
            Console.WriteLine("=====================================================================");
            return false;
        }
        /*
         * 用户坐标到基坐标变换函数测试
         */
        public bool baseUserConvertTest()
        {
            Context context = new Context();
            ParamService paramService = new ParamService();
            IntPtr ctx = context.SDKCreate();            
            double[] response_pose_array = new double[6];
            double[] base_pose_array = new double[6];
            double[] user_pose_array = new double[6];
            if (null == ctx)
                goto errlabel;
            if (!context.SDKLogin(ctx))
                goto errlabel;
            if (!paramService.getRobotPose(ctx, base_pose_array))
            {
                Console.WriteLine("get robot pos error.");
                goto errlabel;
            }
            //base2User,用户坐标号为-1测试
            if(base2User(ctx,base_pose_array, -1, user_pose_array))
            {
                Console.WriteLine("base to user convert error.");
                goto errlabel;
            }
            //base2User,用户坐标号为8测试
            if (base2User(ctx, base_pose_array, 8, user_pose_array))
            {
                Console.WriteLine("base to user convert error.");
                goto errlabel;
            }
            //base2User,正常测试
            if (!base2User(ctx, base_pose_array, 7, user_pose_array))
            {
                Console.WriteLine("base to user convert error.");
                goto errlabel;
            }
            //user2Base，用户坐标号-1测试
            if(user2Base(ctx, user_pose_array, -1, response_pose_array))
            {
                Console.WriteLine("user to base convert error.");
                goto errlabel;
            }
            //user2Base，用户坐标号8测试
            if (user2Base(ctx, user_pose_array, 8, response_pose_array))
            {
                Console.WriteLine("user to base convert error.");
                goto errlabel;
            }
            //user2Base,正常测试
            if (!user2Base(ctx, user_pose_array, 7, response_pose_array))
            {
                Console.WriteLine("user to base convert error.");
                goto errlabel;
            }
            for (int i = 0; i < 6; i++)
            {
                if (Math.Abs(base_pose_array[i] - response_pose_array[i]) > 0.01)
                {
                    Console.WriteLine("user and base convert error.");
                    Console.WriteLine("base_pose_array = {0},{1},{2},{3},{4},{5}",
                        base_pose_array[0], base_pose_array[1], base_pose_array[2], base_pose_array[3],
                        base_pose_array[4], base_pose_array[5]);
                    Console.WriteLine("user_pose_array = {0},{1},{2},{3},{4},{5}",
                        user_pose_array[0], user_pose_array[1], user_pose_array[2], user_pose_array[3],
                        user_pose_array[4], user_pose_array[5]);
                    Console.WriteLine("resp_pose_array = {0},{1},{2},{3},{4},{5}",
                        response_pose_array[0], response_pose_array[1], response_pose_array[2], response_pose_array[3],
                        response_pose_array[4], response_pose_array[5]);
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
            Console.WriteLine("Test user base convert success.");
            Console.WriteLine("=====================================================================");
            return true;
        errlabel:
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Test user base convert failed.");
            Console.WriteLine("=====================================================================");
            return false;
        }
    }
}
