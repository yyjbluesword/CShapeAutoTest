using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Threading;
using SDKContext;
using SDKServoService;
using SDKParamService;
using SDKIOService;
using SDKVarService;
using SDKMovementService;
using SDKKinematicsService;

namespace CShapeAutoTest
{
    
    class Program
    {
        static void Main(string[] args)
        {
            int testNum = 1;
            int successNum = 0;
            Context context = new Context();
            ServoService servoService = new ServoService();
            ParamService paramService = new ParamService();
            IOService ioService = new IOService();
            VarService varService = new VarService();
            MovementService movementService = new MovementService();
            KinematicsService kinematicService = new KinematicsService();
            Console.WriteLine("{0}", System.Environment.CurrentDirectory);
            //上下文测试
            Console.WriteLine("{0}, Start to test elt_create function",testNum++);
            if (context.CreateConnectTest()) successNum++;
            Console.WriteLine("{0}, Start to test elt_destroy function",testNum++);
            if (context.DestroyConnectTest()) successNum++;
            Console.WriteLine("{0}, Start to test elt_login function", testNum++);
            if (context.LoginTest()) successNum++;
            Console.WriteLine("{0}, Start to test elt_out function", testNum++);
            if (context.LogoutTest()) successNum++;
            //伺服服务测试
            Console.WriteLine("{0}, Start to test sync and get Motor Status function", testNum++);
            if (servoService.SyncGetMotorStatusTest()) successNum++;
            Console.WriteLine("{0}, Start to test set and get ServoStatus function", testNum++);
            if(servoService.SetGetServoStatusTest()) successNum++;
            Console.WriteLine("{0}, Start to test clear alarm function", testNum++);
            if(servoService.ClearAlarmTest()) successNum++;
            //参数服务测试
            Console.WriteLine("{0}, Start to test get robot state function", testNum++);
            if(paramService.getRobotStateTest()) successNum++;
            Console.WriteLine("{0}, Start to test get robot mode function", testNum++);
            if(paramService.getRobotModeTest()) successNum++;
            Console.WriteLine("{0}, Start to test get robot pos function", testNum++);
            if(paramService.getRobotPosTest()) successNum++;
            Console.WriteLine("{0}, Start to test get robot pose function", testNum++);
            if(paramService.getRobotPoseTest()) successNum++;
            Console.WriteLine("{0}, Start to test get motor speed function", testNum++);
            if(paramService.getMotorSpeedTest()) successNum++;
            Console.WriteLine("{0}, Start to test get current coord function", testNum++);
            if(paramService.getCurrentCoordTest()) successNum++;
            Console.WriteLine("{0}, Start to test get cycle mode function", testNum++);
            if(paramService.getCycleModeTest()) successNum++;
            Console.WriteLine("{0}, Start to test get current job line function", testNum++);
            if(paramService.getCurrentJobLineTest()) successNum++;
            Console.WriteLine("{0}, Start to test get current encode function", testNum++);
            if(paramService.getCurrentEncodeTest()) successNum++;
            Console.WriteLine("{0}, Start to test get tool number function", testNum++);
            if(paramService.getToolNumberTest()) successNum++;
            Console.WriteLine("{0}, Start to test get user number function", testNum++);
            if(paramService.getUserNumberTest()) successNum++;
            Console.WriteLine("{0}, Start to test get robot torques function", testNum++);
            if(paramService.getRobotTorquesTest()) successNum++;
            Console.WriteLine("{0}, Start to test get analog input function", testNum++);
            if(paramService.getAnalogInputTest()) successNum++;
            Console.WriteLine("{0}, Start to test set analog output function", testNum++);
            if(paramService.setAnalogOutputTest()) successNum++;
            //IO服务测试
            Console.WriteLine("{0}, Start to test get digital input function", testNum++);
            if(ioService.getInputTest()) successNum++;
            Console.WriteLine("{0}, Start to test get digital output function", testNum++);
            if(ioService.getOutputTest()) successNum++;
            Console.WriteLine("{0}, Start to test set digital output function", testNum++);
            if(ioService.setOutputTest()) successNum++;
            Console.WriteLine("{0}, Start to test get virtual input function", testNum++);
            if(ioService.getVirtualInputTest()) successNum++;
            Console.WriteLine("{0}, Start to test get virtual output function", testNum++);
            if(ioService.getVirtualOutputTest()) successNum++;
            Console.WriteLine("{0}, Start to test set virtual output function", testNum++);
            if(ioService.setVirtualOutputTest()) successNum++;
            //变量服务测试
            Console.WriteLine("{0}, Start to test set system var b function", testNum++);
            if(varService.setSysvarBTest()) successNum++;
            Console.WriteLine("{0}, Start to test get system var b function", testNum++);
            if(varService.getSysvarBTest()) successNum++;
            Console.WriteLine("{0}, Start to test set system var i function", testNum++);
            if(varService.setSysvarITest()) successNum++;
            Console.WriteLine("{0}, Start to test get system var i function", testNum++);
            if(varService.getSysvarITest()) successNum++;
            Console.WriteLine("{0}, Start to test set system var d function", testNum++);
            if(varService.setSysvarDTest()) successNum++;
            Console.WriteLine("{0}, Start to test get system var d function", testNum++);
            if(varService.getSysvarDTest()) successNum++;
            //运动服务测试
            Console.WriteLine("{0}, Start to test joint move function", testNum++);
            if (movementService.jointMoveTest()) successNum++;
            Console.WriteLine("{0}, Start to test line move function", testNum++);
            if (movementService.lineMoveTest()) successNum++;
            Console.WriteLine("{0}, Start to test arc move function", testNum++);
            if (movementService.arcMoveTest()) successNum++;
            Console.WriteLine("{0}, Start to test rotate move function", testNum++);
            if (movementService.rotateMoveTest()) successNum++;
            Console.WriteLine("{0}, Start to test movj track move function", testNum++);
            if (movementService.jointTrackMoveTest()) successNum++;
            Console.WriteLine("{0}, Start to test movl track move function", testNum++);
            if (movementService.lineTrackMoveTest()) successNum++;
            Console.WriteLine("{0}, Start to test movl rotate track move function", testNum++);
            if (movementService.rotateTrackMoveTest()) successNum++;
            Console.WriteLine("{0}, Start to test movc track move function", testNum++);
            if (movementService.arcTrackMoveTest()) successNum++;
            Console.WriteLine("{0}, Start to test run stop track move function", testNum++);
            if (movementService.runstopMoveTest()) successNum++;
            //运动学服务测试
            Console.WriteLine("{0}, Start to test inverse kinematic function", testNum++);
            if (kinematicService.inverseKinematicTest()) successNum++;
            Console.WriteLine("{0}, Start to test positive kinematic function", testNum++);
            if (kinematicService.positiveKinematicTest()) successNum++;
            Console.WriteLine("{0}, Start to test base user coord convert function", testNum++);
            if (kinematicService.baseUserConvertTest()) successNum++;
            Console.WriteLine("\n\n\n===================================================");
            Console.WriteLine("total test case are {0} , success case are {1}", testNum - 1 , successNum);
            Console.ReadKey();
        }
    }
}
