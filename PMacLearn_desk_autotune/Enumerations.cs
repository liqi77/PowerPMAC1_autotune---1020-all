using System;
using System.ComponentModel;

namespace PMacLearn
{

    public enum EPages
    {
        /// <summary>数控系统界面</summary>
        CncPage = 0,
    };

    public enum EAxisUnit
    {
        mm = 0,
        deg = 1,
        rpm = 2
    };

    public enum EBtAbortState
    {
        ///<summary>中止程序命令</summary>
        Abort = 0,
        ///<summary>倒带命令</summary>
        Rewind = 1
    };

    public enum EUnits
    {
        English = 0,
        Metric = 1,
    };

    /// <summary>机床状态</summary>
    public enum EMachineState
    {
        /// <summary>The controller is not OnLine and/or the GUI is not Running</summary>
        Offline = 0,
        /// <summary>The controller is not ready to run a program (not homed, manual mode, etc)</summary>
        NotReady = 1,
        /// <summary>The controller is ready to have a program loaded and run</summary>
        Ready = 2,
        /// <summary>The NC program has been loaded and the controller is ready to start</summary>
        ProgramLoaded = 3,
        /// <summary>The controller is running the NC program</summary>
        Running = 4,
        /// <summary>The controller is in Feed Hold (paused)</summary>
        InFeedHold = 5,
        ///// <summary>The NC program has completed successfully</summary>
        Completed = 6,
        /// <summary>The NC program has failed</summary>
        Failed = 7,
        /// <summary>The NC program has been aborted by the operator</summary>
        Aborted = 8,
        /// <summary>The hardware E-Stop button is pressed</summary>
        EStopped = 9,
        /// <summary>Error condition (lost communication, etc)</summary>
        Error = 10,
        /// <summary>The NC program is being downloaded to the controller</summary>
        Downloading = 11,
        /// <summary>适应性添加，未使用******The NC program is being ErrorAborted</summary>
        ErrorAborted = 12,
    };

    /// <summary>机床工作模式</summary>
    public enum EMachineMode
    {
        /// <summary>Auto Mode for running NC programs</summary>
        Auto = 0,
        /// <summary>Manual Mode for Jogging and Homing</summary>
        Manual = 1,
        /// <summary>MDI Mode for executing code from the MDI tab</summary>
        MDI = 2
    };

    /// <summary>机床下位机向上位机发送信息,20230731 </summary>
    public enum EMachineSendMessage
    {
        /// <summary>无信息</summary>
        MachineSendNone = 0,
        /// <summary>完成上下位机通信握手功能</summary>
        MachineSendInitialized = 1,
        /// <summary>完成下位机复位</summary>
        MachineSendResetCompleted = 2,
        /// <summary>完成机床整体回零</summary>
        MachineSendHomeCompleted = 3,
        /// <summary>机床取消</summary>
        MachineSendCanceled = 4,
        /// <summary>NC加工代码开始执行</summary>
        MachineSendCyclestarted = 5,
        /// <summary>进入到FeedHold模式</summary>
        MachineSendInfeedhold = 6,
        /// <summary>NC加工代码中止运行</summary>
        MachineSendProgramaborted = 7,
        /// <summary>NC加工代码运行出错</summary>
        MachineSendProgramfailed = 8,
        /// <summary>NC加工代码运行完成</summary>
        MachineSendProgramCompleted = 9,
        /// <summary>轴1回零完成</summary>
        MachineSendAxis1HomeCompleted = 101,
        /// <summary>轴2回零完成</summary>
        MachineSendAxis2HomeCompleted = 102,
        /// <summary>轴3回零完成</summary>
        MachineSendAxis3HomeCompleted = 103,
        /// <summary>轴4回零完成</summary>
        MachineSendAxis4HomeCompleted = 104,
        /// <summary>轴5回零完成</summary>
        MachineSendAxis5HomeCompleted = 105,
        /// <summary>轴6回零完成</summary>
        MachineSendAxis6HomeCompleted = 106,
        /// <summary>轴7回零完成</summary>
        MachineSendAxis7HomeCompleted = 107,
        /// <summary>轴8回零完成</summary>
        MachineSendAxis8HomeCompleted = 108,
        /// <summary>轴9回零完成</summary>
        MachineSendAxis9HomeCompleted = 109,

        /// <summary>轴1回零完成</summary>
        MachineSendAxis1HomeCanceled = 121,
        /// <summary>轴2回零完成</summary>
        MachineSendAxis2HomeCanceled = 122,
        /// <summary>轴3回零完成</summary>
        MachineSendAxis3HomeCanceled = 123,
        /// <summary>轴4回零完成</summary>
        MachineSendAxis4HomeCanceled = 124,
        /// <summary>轴5回零完成</summary>
        MachineSendAxis5HomeCanceled = 125,
        /// <summary>轴6回零完成</summary>
        MachineSendAxis6HomeCanceled = 126,
        /// <summary>轴7回零完成</summary>
        MachineSendAxis7HomeCanceled = 127,
        /// <summary>轴8回零完成</summary>
        MachineSendAxis8HomeCanceled = 128,
        /// <summary>轴9回零完成</summary>
        MachineSendAxis9HomeCanceled = 129    
            
    }

    /// <summary>命令码</summary>
    public enum ECommandCodes
    {
        // PC sets a command code, Controller clears the command code on receipt then performs the action.
        // PC waits for Controller to clear the command code before setting another.
        None = 0,



        Initialize = 100,  // use SetCommandCodeThenWait()
        Reset = 101,       // use SetCommandCodeThenWait()
        Home = 102,        // use SetCommandCodeThenWait()
        Cancel = 109,      // a lengthy operation in progress (Init, Reset, Home, Abort) was canceled by the user

        JogPositiveContinuous = 110,
        JogNegativeContinuous = 111,
        JogPositiveIncremental = 112,
        JogNegativeIncremental = 113,
        JogStop = 114,  // this code also causes servo loops to close

        CycleStart = 120,
        FeedHold = 121,
        Abort = 122,       // use SetCommandCodeThenWait()

        SetWorkOffsets = 130,
        SetToolOffsets = 131,
        ZeroRelPos = 132,
        SetWorkOffsetsManual = 133,
        SetWorkOffSetsXAxis = 134,
        SetWorkOffSetsYAxis = 135,
        SetWorkOffSetsZAxis = 136,

        Axis1Home = 170,// 20201118,LiuXiufeng
        Axis2Home = 171,
        Axis3Home = 172,
        Axis4Home = 173,
        Axis5Home = 174,
        Axis6Home = 175,
        Axis7Home = 176,
        Axis8Home = 177,
        Axis9Home = 178,

        //20240906,licky,未用
        UI_Yon = 300, //Y加入
        UI_Yoff = 301, //Y退出
        UI_Bon = 302, //B加入
        UI_Boff = 303, //B退出

        Axis1Enable = 180,// 20201124,LiuXiufeng
        Axis2Enable = 181,
        Axis3Enable = 182,
        Axis4Enable = 183,
        Axis5Enable = 184,
        Axis6Enable = 185,
        Axis7Enable = 186,
        Axis8Enable = 187,
        Axis9Enable = 188,
        Axis_XZC_Enable = 189,

        //licky 20240906
        Xbreak = 91,
        Ybreak = 92,
        Zbreak = 93,
        Abreak = 94,
        Bbreak = 95,
        Cbreak = 96,
        Ubreak = 97,
        Vbreak = 98,
        Wbreak = 99,

        SpindleModeChange = 190,
        UI_LaserOn = 191,
        UI_LaserOff = 192,

        MachineOutputToggle1 = 193,
        MachineOutputToggle2 = 194,
        MachineOutputToggle3 = 195,
        MachineOutputToggle4 = 196,
        MachineOutputToggle5 = 197,
        MachineOutputToggle6 = 198,
        MachineOutputToggle7 = 199,
        MachineOutputToggle8 = 200,
        MachineOutputToggle9 = 201,
        MachineOutputToggle10 = 202,
        MachineOutputToggle11 = 203,
        MachineOutputToggle12 = 204,
        GrindProtectionToggle = 205,
        DressProtectionToggle = 206,
    };

    /// <summary>软面板按钮命令码</summary>
    public enum ESoftPanelCommandCodes
    {
        // PC sets a command code, Controller clears the command code on receipt then performs the action.
        // PC waits for Controller to clear the command code before setting another.
        None = 0,

        FeedrateMinus = 140,
        FeedratePlus = 141,
        Feedrate0 = 142,
        Feedrate100 = 143,

        RapidMinus = 144,
        RapidPlus = 145,
        Rapid0 = 146,
        Rapid100 = 147,

        SpindleMinus = 150,
        SpindlePlus = 151,
        Spindle0 = 155,
        Spindle100 = 152,
        SpindleCW = 153,
        SpindleCCW = 154,

        ToolChange = 160,

        GrindingMinus = 161,
        GrindingPlus = 162,
        GrindingOff = 163,
        GrindingOn = 164,
        DressingMinus = 165,
        DressingPlus = 166,
        DressingOff = 167,
        DressingOn = 168,
    };

    /// <summary>PMAC系统状态位</summary>
    public enum ESysStatus
    {
        [Description("WDTFault0")]
        WDTFault0 = 0,
        [Description("WDTFault1")]
        WDTFault1 = 1,
        [Description("PwrOnFault")]
        PwrOnFault = 2,
        [Description("ProjectLoadErr")]
        ProjectLoadErr = 3,
        [Description("ConfigLoadErr")]
        ConfigLoadErr = 4,
        [Description("HWChangeErr")]
        HWChangeErr = 5,
        [Description("FileConfigErr")]
        FileConfigErr = 6,
        [Description("Default")]
        Default = 7,
        [Description("NoClocks")]
        NoClocks = 8,
        [Description("AbortAll")]
        AbortAll = 9,
        [Description("BufSizeErr")]
        BufSizeErr = 10,
        [Description("FlashSizeErr")]
        FlashSizeErr = 11
    }

    /// <summary>PMAC坐标系状态0</summary>
    public enum ECoordStatus0
    {
        [Description("ErrorStatus0")]
        ErrorStatus0 = 0,
        [Description("ErrorStatus1")]
        ErrorStatus1 = 1,
        [Description("ErrorStatus2")]
        ErrorStatus2 = 2,
        [Description("ErrorStatus3")]
        ErrorStatus3 = 3,
        [Description("ErrorStatus4")]
        ErrorStatus4 = 4,
        [Description("ErrorStatus5")]
        ErrorStatus5 = 5,
        [Description("ErrorStatus6")]
        ErrorStatus6 = 6,
        [Description("ErrorStatus7")]
        ErrorStatus7 = 7,
        [Description("TimersEnabled")]
        TimersEnabled = 8,
        [Description("BlockRequest")]
        BlockRequest = 9,
        [Description("Reserve")]
        Reserve = 10,
        [Description("InPos")]
        InPos = 11,
        [Description("AmpEna")]
        AmpEna = 12,
        [Description("ClosedLoop")]
        ClosedLoop = 13,
        [Description("DesVelZero")]
        DesVelZero = 14,
        [Description("HomeComplete")]
        HomeComplete = 15,
        [Description("TimerEnabled")]
        TimerEnabled = 16,
        [Description("AuxFault")]
        AuxFault = 17,
        [Description("EncLoss")]
        EncLoss = 18,
        [Description("AmpWarn")]
        AmpWarn = 19,
        [Description("TriggerNotFound")]
        TriggerNotFound = 20,
        [Description("I2tFault")]
        I2tFault = 21,
        [Description("SoftPlusLimit")]
        SoftPlusLimit = 22,
        [Description("SoftMinusLimit")]
        SoftMinusLimit = 23,
        [Description("AmpFault")]
        AmpFault = 24,
        [Description("LimitStop")]
        LimitStop = 25,
        [Description("FeFatal")]
        FeFatal = 26,
        [Description("FeWarn")]
        FeWarn = 27,
        [Description("PlusLimit")]
        PlusLimit = 28,
        [Description("MinusLimit")]
        MinusLimit = 29,
        [Description("HomeInProgress")]
        HomeInProgress = 30,
        [Description("TriggerMove")]
        TriggerMove = 31,
    }

    /// <summary>PMAC坐标系状态1</summary>
    public enum ECoordStatus1
    {
        [Description("AddedDwellDis")]
        AddedDwellDis = 0,
        [Description("SharpCornerStop")]
        SharpCornerStop = 1,
        [Description("CC3Active")]
        CC3Active = 2,
        [Description("EndDelayActive")]
        EndDelayActive = 3,
        [Description("CCMoveType")]
        CCMoveType0 = 4,
        [Description("CCMoveType")]
        CCMoveType1 = 5,
        [Description("CCOffReq")]
        CCOffReq = 6,
        [Description("CCAddedArc")]
        CCAddedArc = 7,
        [Description("LookAheadActive")]
        LookAheadActive = 8,
        [Description("LookAheadFlush")]
        LookAheadFlush = 9,
        [Description("LookAheadReCalc")]
        LookAheadReCalc = 10,
        [Description("LookAheadChange")]
        LookAheadChange = 11,
        [Description("LookAheadStop")]
        LookAheadStop = 12,
        [Description("LookAheadDir")]
        LookAheadDir = 13,
        [Description("LookAheadLookBack")]
        LookAheadLookBack = 14,
        [Description("LookAheadWrap")]
        LookAheadWrap = 15,
        [Description("SegStopReq")]
        SegStopReq = 16,
        [Description("SegEnabled")]
        SegEnabled = 17,
        [Description("SegMoveDecel")]
        SegMoveDecel = 18,
        [Description("SegMoveAccel")]
        SegMoveAccel = 19,
        [Description("SegMove0")]
        SegMove0 = 20,
        [Description("SegMove1")]
        SegMove1 = 21,
        [Description("MoveMode0")]
        MoveMode0 = 22,
        [Description("MoveMode1")]
        MoveMode1 = 23,
        [Description("CCMode0")]
        CCMode0 = 24,
        [Description("CCMode1")]
        CCMode1 = 25,
        [Description("ContMotion")]
        ContMotion = 26, 
        [Description("BlockActive")]
        BlockActive = 27,
        [Description("FeedHold0")]
        FeedHold0 = 28,
        [Description("FeedHold1")]
        FeedHold1 = 29,
        [Description("LinToPvtBuf")]
        LinToPvtBuf = 30,
        [Description("Csolve")]
        Csolve = 31,
    }

    /// <summary>PMAC电机状态0</summary>
    public enum EMotorStatus0
    {
        [Description("Reserve0")]
        Reserve0 = 0,
        [Description("Reserve1")]
        Reserve1 = 1,
        [Description("Reserve2")]
        Reserve2 = 2,
        [Description("Reserve3")]
        Reserve3 = 3,
        [Description("SpindleMotor0")]
        SpindleMotor0 = 4,
        [Description("SpindleMotor1")]
        SpindleMotor1 = 5,
        [Description("GantryHomed")]
        GantryHomed = 6,
        [Description("TriggerSpeedSel")]
        TriggerSpeedSel = 7,
        [Description("PhaseFound")]
        PhaseFound = 8,
        [Description("BlockRequest")]
        BlockRequest = 9,
        [Description("Reserve4")]
        Reserve4 = 10,
        [Description("InPos")]
        InPos = 11,
        [Description("AmpEna")]
        AmpEna = 12,
        [Description("ClosedLoop")]
        ClosedLoop = 13,
        [Description("DesVelZero")]
        DesVelZero = 14,
        [Description("HomeComplete")]
        HomeComplete = 15,
        [Description("Reserve5")]
        Reserve5 = 16,
        [Description("AuxFault")]
        AuxFault = 17,
        [Description("EncLoss")]
        EncLoss = 18,
        [Description("AmpWarn")]
        AmpWarn = 19,
        [Description("TriggerNotFound")]
        TriggerNotFound = 20,
        [Description("I2tFault")]
        I2tFault = 21,
        [Description("SoftPlusLimit")]
        SoftPlusLimit = 22,
        [Description("SoftMinusLimit")]
        SoftMinusLimit = 23,
        [Description("AmpFault")]
        AmpFault = 24,
        [Description("LimitStop")]
        LimitStop = 25,
        [Description("FeFatal")]
        FeFatal = 26,
        [Description("FeWarn")]
        FeWarn = 27,
        [Description("PlusLimit")]
        PlusLimit = 28,
        [Description("MinusLimit")]
        MinusLimit = 29,
        [Description("HomeInProgress")]
        HomeInProgress = 30,
        [Description("TriggerMove")]
        TriggerMove = 31,
    }


    /// <summary>PMAC电机状态1</summary>
    public enum EMotorStatus1
    {
        [Description("Reserve6")]
        Reserve6 = 0,
        [Description("Reserve7")]
        Reserve7 = 1,
        [Description("Reserve8")]
        Reserve8 = 2,
        [Description("Reserve9")]
        Reserve9 = 3,
        [Description("Reserve10")]
        Reserve10 = 4,
        [Description("Reserve11")]
        Reserve11 = 5,
        [Description("Reserve12")]
        Reserve12 = 6,
        [Description("Reserve13")]
        Reserve13 = 7,
        [Description("Reserve14")]
        Reserve14 = 8,
        [Description("Reserve15")]
        Reserve15 = 9,
        [Description("Reserve16")]
        Reserve16 = 10,
        [Description("Reserve17")]
        Reserve18 = 11,
        [Description("Reserve19")]
        Reserve19 = 12,
        [Description("Reserve20")]
        Reserve20 = 13,
        [Description("Reserve21")]
        Reserve21 = 14,
        [Description("Reserve22")]
        Reserve22 = 15,
        [Description("Reserve23")]
        Reserve23 = 16,
        [Description("Reserve24")]
        Reserve24 = 17,
        [Description("Reserve25")]
        Reserve25 = 18,
        [Description("Reserve26")]
        Reserve26 = 19,
        [Description("Reserve27")]
        Reserve27 = 20,
        [Description("Reserve28")]
        Reserve28 = 21,
        [Description("Reserve29")]
        Reserve29 = 22,
        [Description("Reserve30")]
        Reserve30 = 23,
        [Description("Reserve31")]
        Reserve31 = 24,
        [Description("Reserve32")]
        Reserve32 = 25,
        [Description("Reserve33")]
        Reserve33 = 26,
        [Description("SoftLimitDir")]
        SoftLimitDir = 27,
        [Description("BlDir")]
        BlDir = 28,
        [Description("DacLimit")]
        DacLimit = 29,
        [Description("SoftLimit")]
        SoftLimit = 30,
        [Description("Csolve")]
        Csolve = 31,
    }

    /// <summary>软按钮状态</summary>
    [Flags]
    public enum ESoftPanelStatusBits
    {
        None = 0x0000,

        FeedrateMinus = 0x0001,
        FeedratePlus = 0x0002,
        Feedrate0 = 0x0004,
        Feedrate100 = 0x0008,

        RapidMinus = 0x10000,
        RapidPlus = 0x20000,
        Rapid0 = 0x40000,
        Rapid100 = 0x80000,

        SpindleMinus = 0x0010,
        SpindlePlus = 0x0020,
        Spindle0 = 0x0400,
        Spindle100 = 0x0040,
        SpindleCW = 0x0080,
        SpindleCCW = 0x0100,

        ToolChange = 0x0200,
    };

    public enum ERunOptions
    {
        None = 0x0000,
        OptionStop = 0x0001,
        SingleBlock = 0x0002,
        BlockSkip = 0x0004,
        DryRun = 0x0008,

        RotaryBuffer = 0x0010,
        DownloadSel = 0x0020,
        MidProgStart = 0x0040,
        AutoMidStart = 0x0080
    };

    public enum EMachineAlarms
    {
        AlarmNone = 0,//无报警
        AlarmEStop = 0x0001,//急停
        AlarmHydroPress = 0x0002,//液压站
        AlarmXAxisPress = 0x0004,//X轴液压
        AlarmZAxisPress = 0x0008,//Z轴液压
        AlarmCAxisPress = 0x0010,//C轴气压
        AlarmAirPress = 0x0020,//总气压
        AlarmVacuum = 0x0040,//负压
        AlarmGrindPump = 0x0080,//磨削主轴润滑泵
        AlarmGrindMotor = 0x0100,//磨削主轴变频器报警
        AlarmDressMotor = 0x0200,//修整主轴变频器报警
        AlarmSpindleOff = 0x0400,//工件主轴意外掉使能
    };

        /// <summary>Jog Options enumeration</summary>
        [Flags]
    public enum EJogOptions
    {
        //UI_JogXMsk和Speed Mask同地址；licky
        [Description("None")]
        None = 0x00000,
        [Description("Speed1 (Slow)")]
        Speed1 = 0x00001,
        [Description("Speed3 (Medium)")]
        Speed3 = 0x00002,
        [Description("Speed5 (Fast)")]
        Speed5 = 0x00004,
        [Description("Speed Mask")]
        SpeedMask = 0xF000F,
        //和UI_Axis_X同地址,Axis Mask=UI_JogXMsk；licky
        [Description("Axis 1")]
        Axis1 = 0x00010,
        [Description("Axis 2")]
        Axis2 = 0x00020,
        [Description("Axis 3")]
        Axis3 = 0x00040,
        [Description("Axis 4")]
        Axis4 = 0x00080,
        [Description("Axis 5")]
        Axis5 = 0x00100,
        [Description("Axis 6")]
        Axis6 = 0x00200,
        [Description("Axis 7")]
        Axis7 = 0x00400,
        [Description("Axis 8")]
        Axis8 = 0x00800,
        [Description("Axis 9")]
        Axis9 = 0x01000,
        [Description("Axis 10")]
        Axis10 = 0x02000,
        [Description("Axis Mask")]
        AxisMask = 0x0FFF0,

        [Description("Speed2")]
        Speed2 = 0x10000,
        [Description("Speed4")]
        Speed4 = 0x20000
    };
    /// <summary>信息标志位</summary>
    [Flags]
    public enum EMessageBits
    {
        // Controller sets the message bit(s).  PC clears them on receipt and performs the pre-programmed action.
        None = 0x00000000,
        Bit0 = 0x00000001,
        Bit1 = 0x00000002,
        Bit2 = 0x00000004,
        Bit3 = 0x00000008,
        Bit4 = 0x00000010,
        Bit5 = 0x00000020,
        Bit6 = 0x00000040,
        Bit7 = 0x00000080,
        Bit8 = 0x00000100,
        Bit9 = 0x00000200,
        Bit10 = 0x00000400,
        Bit11 = 0x00000800,
        Bit12 = 0x00001000,
        Bit13 = 0x00002000,
        Bit14 = 0x00004000,
        Bit15 = 0x00008000,
        Bit16 = 0x00010000,
        Bit17 = 0x00020000,
        Bit18 = 0x00040000,
        Bit19 = 0x00080000,
        Bit20 = 0x00100000,
        Bit21 = 0x00200000,
        Bit22 = 0x00400000,
        Bit23 = 0x00800000,
        Bit24 = 0x01000000,
        Bit25 = 0x02000000,
        Bit26 = 0x04000000,
        Bit27 = 0x08000000,
        Bit28 = 0x10000000,
        Bit29 = 0x20000000,
        Bit30 = 0x40000000,
        // Bit31 = 0x80000000
    };

    /// <summary>G-code Group 0 enumeration (unmodal).</summary>
    [Flags]
    public enum EGroup0
    {
        /// <summary>No Group 0 G-codes set</summary>
        None = 0x00000000,
        /// <summary>Dwell</summary>
        [Description("G04 - Dwell")]
        G04 = 0x00000001,
        /// <summary>Exact Stop Check</summary>
        [Description("G09 - Exact Stop Check")]
        G09 = 0x00000002,
        /// <summary>Programmable Data Input</summary>
        [Description("G10 - Programmable Data Input")]
        G10 = 0x00000004,
        /// <summary>PMAC Data Input</summary>
        [Description("G10.1 - PMAC Data Input")]
        G10_1 = 0x00000008,
        /// <summary>Tool Offset Increase</summary>
        [Description("G45 - Tool Offset Increase")]
        G45 = 0x00000010,
        /// <summary>Tool Offset Decrease</summary>
        [Description("G46 - Tool Offset Decrease")]
        G46 = 0x00000020,
        /// <summary>Tool Offset Double Increase</summary>
        [Description("G47 - Tool Offset Double Increase")]
        G47 = 0x00000040,
        /// <summary>Tool Offset Double Decrease</summary>
        [Description("G48 - Tool Offset Double Decrease")]
        G48 = 0x00000080,
        /// <summary>Absolute Zero Point Programming</summary>
        [Description("G92 - Absolute Zero Point Programming")]
        G92 = 0x00000100,
        /// <summary>Absolute Zero Point Programming Cancel</summary>
        [Description("G92.1 - Absolute Zero Point Programming Cancel")]
        G92_1 = 0x00000200,
        /// <summary>Lookahead Segment Control</summary>
        [Description("G105 - Lookahead Segment Control")]
        G105 = 0x00000400,
        /// <summary>Local Coordinate System</summary>
        [Description("G52 - Local Coordinate System")]
        G52 = 0x00000800,
        /// <summary>Machine Coordinate System</summary>
        [Description("G53 - Machine Coordinate System")]
        G53 = 0x00001000,
    };

    /// <summary>G-code Group 1 enumeration.</summary>
    public enum EGroup1
    {
        /// <summary>Rapid Traverse</summary>
        [Description("G00 - Rapid Traverse")]
        G00,
        /// <summary>Linear Interpolation</summary>
        [Description("G01 - Linear Interpolation")]
        G01,
        /// <summary>Spline Interpolation</summary>
        [Description("G01.1 - Spline Interpolation")]
        G01_1,
        /// <summary>Circular Interpolation (CW)</summary>
        [Description("G02 - Circular Interpolation (CW)")]
        G02,
        /// <summary>Circular Interpolation (CCW)</summary>
        [Description("G03 - Circular Interpolation (CCW)")]
        G03
    };

    /// <summary>G-code Group 2 enumeration.</summary>
    public enum EGroup2
    {
        /// <summary>XY Plane Selection</summary>
        [Description("G17 - XY Plane Selection")]
        G17,
        /// <summary>ZX Plane Selection</summary>
        [Description("G18 - ZX Plane Selection")]
        G18,
        /// <summary>YZ Plane Selection</summary>
        [Description("G19 - YZ Plane Selection")]
        G19,
    };

    /// <summary>G-code Group 3 enumeration.</summary>
    public enum EGroup3
    {
        /// <summary>Absolute Command Mode</summary>
        [Description("G90 - Absolute Command Mode")]
        G90,
        /// <summary>Incremental Command Mode</summary>
        [Description("G91 - Incremental Command Mode")]
        G91,
        /// <summary>Arc Radius Absolute Mode</summary>
        [Description("G90.1 - Arc Radius Absolute Mode")]
        G90_1,
        /// <summary>Arc Radius Incremental Mode</summary>
        [Description("G91.1 - Arc Radius Incremental Mode")]
        G91_1,
    };


    /// <summary>G-code Group 5 enumeration.</summary>
    public enum EGroup5
    {
        /// <summary>Inverse Time Feedrate</summary>
        [Description("G93 - Inverse Time Feedrate")]
        G93,
        /// <summary>Feedrate Per Minute</summary>
        [Description("G94 - Feedrate Per Minute")]
        G94,
        /// <summary>Feedrate Per Revolution</summary>
        [Description("G95 - Feedrate Per Revolution")]
        G95,
    };

    /// <summary>G-code Group 6 enumeration.</summary>
    public enum EGroup6
    {
        /// <summary>Inch Mode</summary>
        [Description("G20 - Inch Mode")]
        G20,
        /// <summary>Metric Mode</summary>
        [Description("G21 - Metric Mode")]
        G21,
    };

    /// <summary>G-code Group 19 enumeration.</summary>
    public enum EGroup19
    {
        /// <summary>Spindle Speed Detect ON</summary>
        [Description("G25 - Spindle Speed Detect OFF")]
        G25,
        /// <summary>Spindle Speed Detect OFF</summary>
        [Description("G26 - Spindle Speed Detect ON")]
        G26,
    };

    /// <summary>G-code Group 7 enumeration.</summary>
    public enum EGroup7
    {
        /// <summary>Cutter Compensation Cancel</summary>
        [Description("G40 - Cutter Compensation Cancel")]
        G40,
        /// <summary>Cutter Compensation Left</summary>
        [Description("G41 - Cutter Compensation Left")]
        G41,
        /// <summary>Cutter Compensation Right</summary>
        [Description("G42 - Cutter Compensation Right")]
        G42,
    };

    /// <summary>G-code Group 8 enumeration.</summary>
    public enum EGroup8
    {
        /// <summary>Tool Length Compensation, Positive</summary>
        [Description("G43 - Tool Length Compensation, Positive")]
        G43,
        /// <summary>Tool Length Compensation, Negative</summary>
        [Description("G44 - Tool Length Compensation, Negative")]
        G44,
        /// <summary>Tool Length Compensation Cancel</summary>
        [Description("G49 - Tool Length Compensation Cancel")]
        G49,
    };

    /// <summary>G-code Group 9 enumeration.</summary>
    public enum EGroup9
    {
        /// <summary>Bolt Circle, Secondary Canned Cycle</summary>
        [Description("G70 - Bolt Circle")]
        G70,
        /// <summary>Bolt Arc, Secondary Canned Cycle</summary>
        [Description("G71 - Bolt Arc")]
        G71,
        /// <summary>Line at Angle, Secondary Canned Cycel</summary>
        [Description("G72 - Line at Angle")]
        G72,
        /// <summary>Peck Drilling Canned-Cycle With Short Retract</summary>
        [Description("G73 - Peck Drilling Cycle-Short Retract")]
        G73,
        /// <summary>Left Hand Float Tapping Cnned-Cycle</summary>
        [Description("G74 - Left Hand Float Tapping")]
        G74,
        /// <summary>Fine Boring Canned-Cycle</summary>
        [Description("G76 - Fine Boring")]
        G76,
        /// <summary>Canned-Cycle Cancel</summary>
        [Description("G80 - Canned-Cycle Cancel")]
        G80,
        /// <summary>Simple Drilling Canned-Cycle</summary>
        [Description("G81 - Simple Drilling Canned-Cycle")]
        G81,
        /// <summary>Peck Drilling Canned-Cycle With Long Retract</summary>
        [Description("G83 - Peck Drilling Cycle-Long Retract")]
        G83,
        /// <summary>Right Hand Float Tapping Canned-Cycle</summary>
        [Description("G84 - Right Hand Float Tapping")]
        G84,
        /// <summary>Right Hand Rigid Tapping Canned-Cycle </summary>
        [Description("G84.2 - Right Hand Rigid Tapping")]
        G84_2,
        /// <summary>Left Hand Rigid Tapping Canned-Cycle</summary>
        [Description("G84.3 - Left Hand Rigid Tapping")]
        G84_3,
        /// <summary>Peck Right Hand Rigid Tapping Canned-Cycle</summary>
        [Description("G84.4 - Peck Right Hand Rigid Tapping")]
        G84_4,
        /// <summary>Peck Left Hand Rigid Tapping Canned-Cycle</summary>
        [Description("G84.5 - Peck Left Hand Rigid Tapping")]
        G84_5,
        /// <summary>Boring Canned-Cycle No Dwell</summary>
        [Description("G85 - Boring Without Dwell")]
        G85,
        /// <summary>Boring Canned-Cycle With Feed-In(Spindle-ON) Rapid-Out(Spindle-OFF)</summary>
        [Description("G86 - Boring Canned-Cycle With Feed-In(Spindle-ON) Rapid-Out(Spindle-OFF)")]
        G86,
        /// <summary>Back Boring Canned-Cycle</summary>
        [Description("G87 - Back Boring Canned-Cycle")]
        G87,
        /// <summary>Boring Canned-Cycle That Allows Manual Operation</summary>
        [Description("G88 - Boring Cycle That Allows Manual Operation")]
        G88,
        /// <summary>Boring Canned-Cycle With Dwell</summary>
        [Description("G89 - Boring Cycle With Dwell")]
        G89,
    };

    /// <summary>G-code Group 11 enumeration.</summary>
    public enum EGroup11
    {
        /// <summary>Scaling Cancel</summary>
        [Description("G50 - Scaling Cancel")]
        G50,
        /// <summary>Scaling</summary>
        [Description("G51 - Scaling")]
        G51,
    };


    /// <summary>G-code Group 12 enumeration.</summary>
    public enum EGroup12
    {
        /// <summary>Work Coordinate Offsets Cancelled</summary>
        [Description("Gxx - Work Coordinate Offsets Cancelled")]
        None,
        /// <summary>Work Coordinate Offset 1</summary>
        [Description("G54 - Work Coordinate Offset 1")]
        G54,
        /// <summary>Work Coordinate Offset 2</summary>
        [Description("G55 - Work Coordinate Offset 2")]
        G55,
        /// <summary>Work Coordinate Offset 3</summary>
        [Description("G56 - Work Coordinate Offset 3")]
        G56,
        /// <summary>Work Coordinate Offset 4</summary>
        [Description("G57 - Work Coordinate Offset 4")]
        G57,
        /// <summary>Work Coordinate Offset 5</summary>
        [Description("G58 - Work Coordinate Offset 5")]
        G58,
        /// <summary>Work Coordinate Offset 6</summary>
        [Description("G59 - Work Coordinate Offset 6")]
        G59,
        /// <summary>Work Coordinate Offset 7</summary>
        [Description("G54.1 P1 - Work Coordinate Offset 7")]
        G541,
        /// <summary>Work Coordinate Offset 8</summary>
        [Description("G54.1 P2 - Work Coordinate Offset 8")]
        G542,
        /// <summary>Work Coordinate Offset 9</summary>
        [Description("G54.1 P3 - Work Coordinate Offset 9")]
        G543,
        /// <summary>Work Coordinate Offset 10</summary>
        [Description("G54.1 P4 - Work Coordinate Offset 10")]
        G544,
        /// <summary>Work Coordinate Offset 11</summary>
        [Description("G54.1 P5 - Work Coordinate Offset 11")]
        G545,
        /// <summary>Work Coordinate Offset 12</summary>
        [Description("G54.1 P6 - Work Coordinate Offset 12")]
        G546,
        /// <summary>Work Coordinate Offset 13</summary>
        [Description("G54.1 P7 - Work Coordinate Offset 13")]
        G547,
        /// <summary>Work Coordinate Offset 14</summary>
        [Description("G54.1 P8 - Work Coordinate Offset 14")]
        G548,
        /// <summary>Work Coordinate Offset 15</summary>
        [Description("G54.1 P9 - Work Coordinate Offset 15")]
        G549,
        /// <summary>Work Coordinate Offset 16</summary>
        [Description("G54.1 P10 - Work Coordinate Offset 16")]
        G5410,
        /// <summary>Work Coordinate Offset 17</summary>
        [Description("G54.1 P11 - Work Coordinate Offset 17")]
        G5411,
        /// <summary>Work Coordinate Offset 18</summary>
        [Description("G54.1 P12 - Work Coordinate Offset 18")]
        G5412,
        /// <summary>Work Coordinate Offset 19</summary>
        [Description("G54.1 P13 - Work Coordinate Offset 19")]
        G5413,
        /// <summary>Work Coordinate Offset 20</summary>
        [Description("G54.1 P14 - Work Coordinate Offset 20")]
        G5414,
        /// <summary>Work Coordinate Offset 21</summary>
        [Description("G54.1 P15 - Work Coordinate Offset 21")]
        G5415,
        /// <summary>Work Coordinate Offset 22</summary>
        [Description("G54.1 P16 - Work Coordinate Offset 22")]
        G5416,
        /// <summary>Work Coordinate Offset 23</summary>
        [Description("G54.1 P17 - Work Coordinate Offset 23")]
        G5417,
        /// <summary>Work Coordinate Offset 24</summary>
        [Description("G54.1 P18 - Work Coordinate Offset 24")]
        G5418,
        /// <summary>Work Coordinate Offset 25</summary>
        [Description("G54.1 P19 - Work Coordinate Offset 25")]
        G5419,
        /// <summary>Work Coordinate Offset 26</summary>
        [Description("G54.1 P20 - Work Coordinate Offset 26")]
        G5420,
        /// <summary>Work Coordinate Offset 27</summary>
        [Description("G54.1 P21 - Work Coordinate Offset 27")]
        G5421,
        /// <summary>Work Coordinate Offset 28</summary>
        [Description("G54.1 P22 - Work Coordinate Offset 28")]
        G5422,
        /// <summary>Work Coordinate Offset 29</summary>
        [Description("G54.1 P23 - Work Coordinate Offset 29")]
        G5423,
        /// <summary>Work Coordinate Offset 30</summary>
        [Description("G54.1 P24 - Work Coordinate Offset 30")]
        G5424,
        /// <summary>Work Coordinate Offset 31</summary>
        [Description("G54.1 P25 - Work Coordinate Offset 31")]
        G5425,
        /// <summary>Work Coordinate Offset 32</summary>
        [Description("G54.1 P26 - Work Coordinate Offset 32")]
        G5426,
        /// <summary>Work Coordinate Offset 33</summary>
        [Description("G54.1 P27 - Work Coordinate Offset 33")]
        G5427,
        /// <summary>Work Coordinate Offset 34</summary>
        [Description("G54.1 P28 - Work Coordinate Offset 34")]
        G5428,
        /// <summary>Work Coordinate Offset 35</summary>
        [Description("G54.1 P29 - Work Coordinate Offset 35")]
        G5429,
        /// <summary>Work Coordinate Offset 36</summary>
        [Description("G54.1 P30 - Work Coordinate Offset 36")]
        G5430,
        /// <summary>Work Coordinate Offset 37</summary>
        [Description("G54.1 P31 - Work Coordinate Offset 37")]
        G5431,
        /// <summary>Work Coordinate Offset 38</summary>
        [Description("G54.1 P32 - Work Coordinate Offset 38")]
        G5432,
        /// <summary>Work Coordinate Offset 39</summary>
        [Description("G54.1 P33 - Work Coordinate Offset 39")]
        G5433,
        /// <summary>Work Coordinate Offset 40</summary>
        [Description("G54.1 P34 - Work Coordinate Offset 40")]
        G5434,
        /// <summary>Work Coordinate Offset 41</summary>
        [Description("G54.1 P35 - Work Coordinate Offset 41")]
        G5435,
        /// <summary>Work Coordinate Offset 42</summary>
        [Description("G54.1 P36 - Work Coordinate Offset 42")]
        G5436,
        /// <summary>Work Coordinate Offset 43</summary>
        [Description("G54.1 P37 - Work Coordinate Offset 43")]
        G5437,
        /// <summary>Work Coordinate Offset 44</summary>
        [Description("G54.1 P38 - Work Coordinate Offset 44")]
        G5438,
        /// <summary>Work Coordinate Offset 45</summary>
        [Description("G54.1 P39 - Work Coordinate Offset 45")]
        G5439,
        /// <summary>Work Coordinate Offset 46</summary>
        [Description("G54.1 P40 - Work Coordinate Offset 46")]
        G5440,
        /// <summary>Work Coordinate Offset 47</summary>
        [Description("G54.1 P41 - Work Coordinate Offset 47")]
        G5441,
        /// <summary>Work Coordinate Offset 48</summary>
        [Description("G54.1 P42 - Work Coordinate Offset 48")]
        G5442,
        /// <summary>Work Coordinate Offset 49</summary>
        [Description("G54.1 P43 - Work Coordinate Offset 49")]
        G5443,
        /// <summary>Work Coordinate Offset 50</summary>
        [Description("G54.1 P44 - Work Coordinate Offset 50")]
        G5444,
        /// <summary>Work Coordinate Offset 51</summary>
        [Description("G54.1 P45 - Work Coordinate Offset 51")]
        G5445,
        /// <summary>Work Coordinate Offset 52</summary>
        [Description("G54.1 P46 - Work Coordinate Offset 52")]
        G5446,
        /// <summary>Work Coordinate Offset 53</summary>
        [Description("G54.1 P47 - Work Coordinate Offset 53")]
        G5447,
        /// <summary>Work Coordinate Offset 54</summary>
        [Description("G54.1 P48 - Work Coordinate Offset 54")]
        G5448,
        /// <summary>Work Coordinate Offset 55</summary>
        [Description("G54.1 P49 - Work Coordinate Offset 55")]
        G5449,
        /// <summary>Work Coordinate Offset 56</summary>
        [Description("G54.1 P50 - Work Coordinate Offset 56")]
        G5450,
        /// <summary>Work Coordinate Offset 57</summary>
        [Description("G54.1 P51 - Work Coordinate Offset 57")]
        G5451,
        /// <summary>Work Coordinate Offset 58</summary>
        [Description("G54.1 P52 - Work Coordinate Offset 58")]
        G5452,
        /// <summary>Work Coordinate Offset 59</summary>
        [Description("G54.1 P53 - Work Coordinate Offset 59")]
        G5453,
        /// <summary>Work Coordinate Offset 60</summary>
        [Description("G54.1 P54 - Work Coordinate Offset 60")]
        G5454,
        /// <summary>Work Coordinate Offset 61</summary>
        [Description("G54.1 P55 - Work Coordinate Offset 61")]
        G5455,
        /// <summary>Work Coordinate Offset 62</summary>
        [Description("G54.1 P56 - Work Coordinate Offset 62")]
        G5456,
        /// <summary>Work Coordinate Offset 63</summary>
        [Description("G54.1 P57 - Work Coordinate Offset 63")]
        G5457,
        /// <summary>Work Coordinate Offset 64</summary>
        [Description("G54.1 P58 - Work Coordinate Offset 64")]
        G5458,
        /// <summary>Work Coordinate Offset 65</summary>
        [Description("G54.1 P59 - Work Coordinate Offset 65")]
        G5459,
        /// <summary>Work Coordinate Offset 66</summary>
        [Description("G54.1 P60 - Work Coordinate Offset 66")]
        G5460,
        /// <summary>Work Coordinate Offset 67</summary>
        [Description("G54.1 P61 - Work Coordinate Offset 67")]
        G5461,
        /// <summary>Work Coordinate Offset 68</summary>
        [Description("G54.1 P62 - Work Coordinate Offset 68")]
        G5462,
        /// <summary>Work Coordinate Offset 69</summary>
        [Description("G54.1 P63 - Work Coordinate Offset 69")]
        G5463,
        /// <summary>Work Coordinate Offset 70</summary>
        [Description("G54.1 P64 - Work Coordinate Offset 70")]
        G5464,
        /// <summary>Work Coordinate Offset 71</summary>
        [Description("G54.1 P65 - Work Coordinate Offset 71")]
        G5465,
        /// <summary>Work Coordinate Offset 72</summary>
        [Description("G54.1 P66 - Work Coordinate Offset 72")]
        G5466,
        /// <summary>Work Coordinate Offset 73</summary>
        [Description("G54.1 P67 - Work Coordinate Offset 73")]
        G5467,
        /// <summary>Work Coordinate Offset 74</summary>
        [Description("G54.1 P68 - Work Coordinate Offset 74")]
        G5468,
        /// <summary>Work Coordinate Offset 75</summary>
        [Description("G54.1 P69 - Work Coordinate Offset 75")]
        G5469,
        /// <summary>Work Coordinate Offset 76</summary>
        [Description("G54.1 P70 - Work Coordinate Offset 76")]
        G5470,
        /// <summary>Work Coordinate Offset 77</summary>
        [Description("G54.1 P71 - Work Coordinate Offset 77")]
        G5471,
        /// <summary>Work Coordinate Offset 78</summary>
        [Description("G54.1 P72 - Work Coordinate Offset 78")]
        G5472,
        /// <summary>Work Coordinate Offset 79</summary>
        [Description("G54.1 P73 - Work Coordinate Offset 79")]
        G5473,
        /// <summary>Work Coordinate Offset 80</summary>
        [Description("G54.1 P74 - Work Coordinate Offset 80")]
        G5474,
        /// <summary>Work Coordinate Offset 81</summary>
        [Description("G54.1 P75 - Work Coordinate Offset 81")]
        G5475,
        /// <summary>Work Coordinate Offset 82</summary>
        [Description("G54.1 P76 - Work Coordinate Offset 82")]
        G5476,
        /// <summary>Work Coordinate Offset 83</summary>
        [Description("G54.1 P77 - Work Coordinate Offset 83")]
        G5477,
        /// <summary>Work Coordinate Offset 84</summary>
        [Description("G54.1 P78 - Work Coordinate Offset 84")]
        G5478,
        /// <summary>Work Coordinate Offset 85</summary>
        [Description("G54.1 P79 - Work Coordinate Offset 85")]
        G5479,
        /// <summary>Work Coordinate Offset 86</summary>
        [Description("G54.1 P80 - Work Coordinate Offset 86")]
        G5480,
        /// <summary>Work Coordinate Offset 87</summary>
        [Description("G54.1 P81 - Work Coordinate Offset 87")]
        G5481,
        /// <summary>Work Coordinate Offset 88</summary>
        [Description("G54.1 P82 - Work Coordinate Offset 88")]
        G5482,
        /// <summary>Work Coordinate Offset 89</summary>
        [Description("G54.1 P83 - Work Coordinate Offset 89")]
        G5483,
        /// <summary>Work Coordinate Offset 90</summary>
        [Description("G54.1 P84 - Work Coordinate Offset 90")]
        G5484,
        /// <summary>Work Coordinate Offset 91</summary>
        [Description("G54.1 P85 - Work Coordinate Offset 91")]
        G5485,
        /// <summary>Work Coordinate Offset 92</summary>
        [Description("G54.1 P86 - Work Coordinate Offset 92")]
        G5486,
        /// <summary>Work Coordinate Offset 93</summary>
        [Description("G54.1 P87 - Work Coordinate Offset 93")]
        G5487,
        /// <summary>Work Coordinate Offset 94</summary>
        [Description("G54.1 P88 - Work Coordinate Offset 94")]
        G5488,
        /// <summary>Work Coordinate Offset 95</summary>
        [Description("G54.1 P89 - Work Coordinate Offset 95")]
        G5489,
        /// <summary>Work Coordinate Offset 96</summary>
        [Description("G54.1 P90 - Work Coordinate Offset 96")]
        G5490,
        /// <summary>Work Coordinate Offset 97</summary>
        [Description("G54.1 P91 - Work Coordinate Offset 97")]
        G5491,
        /// <summary>Work Coordinate Offset 98</summary>
        [Description("G54.1 P92 - Work Coordinate Offset 98")]
        G5492,
        /// <summary>Work Coordinate Offset 99</summary>
        [Description("G54.1 P93 - Work Coordinate Offset 99")]
        G5493,
        /// <summary>Work Coordinate Offset 100</summary>
        [Description("G54.1 P94 - Work Coordinate Offset 100")]
        G5494,
        /// <summary>Work Coordinate Offset 101</summary>
        [Description("G54.1 P95 - Work Coordinate Offset 101")]
        G5495,
        /// <summary>Work Coordinate Offset 102</summary>
        [Description("G54.1 P96 - Work Coordinate Offset 102")]
        G5496,
        /// <summary>Work Coordinate Offset 103</summary>
        [Description("G54.1 P97 - Work Coordinate Offset 103")]
        G5497,
        /// <summary>Work Coordinate Offset 104</summary>
        [Description("G54.1 P98 - Work Coordinate Offset 104")]
        G5498,
        /// <summary>Work Coordinate Offset 105</summary>
        [Description("G54.1 P99 - Work Coordinate Offset 105")]
        G5499,
        /// <summary>Work Coordinate Offset 106</summary>
        [Description("G54.1 P100 - Work Coordinate Offset 106")]
        G54100,
    };

    /// <summary>G-code Group 13 enumeration.</summary>
    public enum EGroup13 //G96,G97
    {
        /// <summary>Constant Surface Speed</summary>
        [Description("G96 - Constant Surface Speed")]
        G96,
        /// <summary>Work Coordinate Offset 1</summary>
        [Description("G97 - Constant Surface Speed Cancel")]
        G97,
    };

    /// <summary>G-code Group 15 enumeration.</summary>
    public enum EGroup15 //G61,G64,
    {
        /// <summary>Exact Stop Mode</summary>
        [Description("G61 - Exact Stop Mode")]
        G61,
        /// <summary>Cutting Mode (Cancel Exact Stop Mode)</summary>
        [Description("G64 - Cutting Mode (Cancel Exact Stop Mode)")]
        G64,
    };

    /// <summary>G-code Group 16 enumeration.</summary>
    public enum EGroup16
    {
        /// <summary>Coordinate Rotation</summary>
        [Description("G68 - Coordinate Rotation")]
        G68,
        /// <summary>Coordinate Rotation Cancel</summary>
        [Description("G69 - Coordinate Rotation Cancel")]
        G69,
    };

    /// <summary>G-code Group 22 enumeration.</summary>
    public enum EGroup22
    {
        /// <summary>Mirror Image Cancel</summary>
        [Description("G50.1 - Mirror Image Cancel")]
        G50_1,
        /// <summary>Mirror Image</summary>
        [Description("G51.1 - Mirror Image")]
        G51_1,
    };


    public enum ECommandReg//机床各种寄存位置,,内部参数，下位可读到
    {
        /// <summary>
        /// 机床State寄存器
        /// </summary>
        [Description("m1")]
        MachineState = 1,
        /// <summary>
        /// 命令寄存器
        /// </summary>
        [Description("m2")]
        CommandReg = 2,
        /// <summary>
        /// 机床Status寄存器
        /// </summary>
        [Description("m3")]
        StatusReg = 3,
        /// <summary>
        /// Jog选项
        /// </summary>
        [Description("m4")]
        JogOptions = 4,
        /// <summary>
        /// Run选项
        /// </summary>
        [Description("m5")]
        RunOptions = 5,
        /// <summary>
        /// 机床控制模式
        /// </summary>
        [Description("m6")]
        MachineMode = 6,
        /// <summary>
        /// Hmi计数器
        /// </summary>
        [Description("m7")]
        HmiCounter = 7,
        /// <summary>
        /// 对话框响应
        /// </summary>
        [Description("m8")]
        DialogResponse = 8,
        /// <summary>
        /// 下位机向上位机发送信号,20230731
        /// </summary>
        [Description("m9")]
        MachineSendMessage = 9,
        /// <summary>
        /// 软MCP命令
        /// </summary>
        [Description("m20")]
        SoftPanelCommand = 20,
        /// <summary>
        /// 软MCP状态
        /// </summary>
        [Description("m21")]
        SoftPanelStatus = 21,
        /// <summary>
        /// 致命Msg
        /// </summary>
        [Description("m100")]
        MsgFatal = 100,
        /// <summary>
        /// 警告Msg
        /// </summary>
        [Description("m120")]
        MsgWarning = 120,
        /// <summary>
        /// 提示Msg
        /// </summary>
        [Description("m140")]
        MsgInfo = 140
    };

    public enum ESysStatusReg //这个状态是用啦干嘛的
    {
        [Description("m50")]
        SysStatus = 50,//读取Sys Coord Motor状态
        [Description("m51")]
        Coord0Status0 = 51,
        [Description("m52")]
        Coord0Status1 = 52,
        [Description("m53")]
        Coord1Status0 = 53,
        [Description("m54")]
        Coord1Status1 = 54,
        [Description("m55")]
        Coord2Status0 = 55,
        [Description("m56")]
        Coord2Status1 = 56,
        [Description("m57")]
        Motor0Status0 = 57,
        [Description("m58")]
        Motor0Status1 = 58,
        [Description("m59")]
        Motor1Status0 = 59,
        [Description("m60")]
        Motor1Status1 = 60,
        [Description("m61")]
        Motor2Status0 = 61,
        [Description("m62")]
        Motor2Status1 = 62,
        [Description("m63")]
        Motor3Status0 = 63,
        [Description("m64")]
        Motor3Status1 = 64,
        [Description("m65")]
        Motor4Status0 = 65,
        [Description("m66")]
        Motor4Status1 = 66,
        [Description("m67")]
        Motor5Status0 = 67,
        [Description("m68")]
        Motor5Status1 = 68,
        [Description("m69")]
        Motor6Status0 = 69,
        [Description("m70")]
        Motor6Status1 = 70,
        [Description("m71")]
        Motor7Status0 = 71,
        [Description("m72")]
        Motor7Status1 = 72,
        [Description("m73")]
        Motor8Status0 = 73,
        [Description("m74")]
        Motor8Status1 = 74,
    };

    public enum EAxesPosReg
    {
        /// <summary>
        /// Axis1位置
        /// </summary>
        [Description("p100")]
        Axis1Positioin = 100,
        /// <summary>
        /// Axis2位置
        /// </summary>
        [Description("p101")]
        Axis2Position = 101,
        /// <summary>
        /// Axis3位置
        /// </summary>
        [Description("p102")]
        Axis3Position = 102,
        /// <summary>
        /// Axis4位置
        /// </summary>
        [Description("p103")]
        Axis4Position = 103,
        /// <summary>
        /// Axis5位置
        /// </summary>
        [Description("p104")]
        Axis5Postion = 104,
        /// <summary>
        /// Axis6位置
        /// </summary>
        [Description("p105")]
        Axis6Positioin = 105,
        /// <summary>
        /// Axis7位置
        /// </summary>
        [Description("p106")]
        Axis7Position = 106,
        /// <summary>
        /// Axis8位置
        /// </summary>
        [Description("p107")]
        Axis8Position = 107,
        /// <summary>
        /// Axis9位置
        /// </summary>
        [Description("p108")]
        Axis9Position = 108,
        /// <summary>
        /// Axis9位置
        /// </summary>
        [Description("p109")]
        Axis10Postion = 109,


        /// <summary>
        /// Axis1目标位置
        /// </summary>
        [Description("p110")]
        Axis1disttogo = 110,
        /// <summary>
        /// Axis2轴目标位置
        /// </summary>
        [Description("p111")]
        Axis2disttogo = 111,
        /// <summary>
        /// Axis3轴目标位置
        /// </summary>
        [Description("p112")]
        Axis3disttogo = 112,
        /// <summary>
        /// Axis4目标位置
        /// </summary>
        [Description("p113")]
        Axis4disttogo = 113,
        /// <summary>
        /// Axis5轴目标位置
        /// </summary>
        [Description("p114")]
        Axis5disttogo = 114,
        /// <summary>
        /// Axis6轴目标位置
        /// </summary>
        [Description("p115")]
        Axis6disttogo = 115,
        /// <summary>
        /// Axis7轴目标位置
        /// </summary>
        [Description("p116")]
        Axis7disttogo = 116,
        /// <summary>
        /// Axis8目标位置
        /// </summary>
        [Description("p117")]
        Axis8disttogo = 117,
        /// <summary>
        /// Axis9轴目标位置
        /// </summary>
        [Description("p118")]
        Axis9disttogo = 118,
        /// <summary>
        /// Axis10轴目标位置
        /// </summary>
        [Description("p119")]
        Axis10disttogo = 119,
        /// <summary>
        /// Axis1轴扭矩
        /// </summary>
        [Description("p120")]
        Axis1torque = 120,
        /// <summary>
        /// Axis2轴扭矩
        /// </summary>
        [Description("p121")]
        Axis2torque = 121,
        /// <summary>
        /// Axis3轴扭矩
        /// </summary>
        [Description("p122")]
        Axis3torque = 122,
        /// <summary>
        /// Axis4轴扭矩
        /// </summary>
        [Description("p123")]
        Axis4torque = 123,
        /// <summary>
        /// Axis5轴扭矩
        /// </summary>
        [Description("p124")]
        Axis5torque = 124,
        /// <summary>
        /// Axis6轴扭矩
        /// </summary>
        [Description("p125")]
        Axis6torque = 125,
        /// <summary>
        /// Axis7轴扭矩
        /// </summary>
        [Description("p126")]
        Axis7torque = 126,
        /// <summary>
        /// Axis8轴扭矩
        /// </summary>
        [Description("p127")]
        Axis8torque = 127,
        /// <summary>
        /// Axis9轴扭矩
        /// </summary>
        [Description("p128")]
        Axis9torque = 128,
        /// <summary>
        /// Axis10轴扭矩
        /// </summary>
        [Description("p129")]
        Axis10torque = 129,


        /// <summary>
        /// Axis1轴速度
        /// </summary>
        [Description("p130")]
        Axis1velocity = 130,
        /// <summary>
        /// Axis2轴速度
        /// </summary>
        [Description("p131")]
        Axis2velocity = 131,
        /// <summary>
        /// Axis3轴速度
        /// </summary>
        [Description("p132")]
        Axis3velocity = 132,
        /// <summary>
        /// Axis4轴速度
        /// </summary>
        [Description("p133")]
        Axis4velocity = 133,
        /// <summary>
        /// Axis5轴速度
        /// </summary>
        [Description("p134")]
        Axis5velocity = 134,
        /// <summary>
        /// Axis6轴速度
        /// </summary>
        [Description("p135")]
        Axis6velocity = 135,
        /// <summary>
        /// Axis7轴速度
        /// </summary>
        [Description("p136")]
        Axis7velocity = 136,
        /// <summary>
        /// Axis8轴速度
        /// </summary>
        [Description("p137")]
        Axis8velocity = 137,
        /// <summary>
        /// Axis9轴速度
        /// </summary>
        [Description("p138")]
        Axis9velocity = 138,
        /// <summary>
        /// Axis10轴速度
        /// </summary>
        [Description("p139")]
        Axis10velocity = 139,



        /// <summary>
        /// Axis1跟踪误差
        /// </summary>
        [Description("p140")]
        Axis1FolErr = 140,
        /// <summary>
        /// Axis2跟踪误差
        /// </summary>
        [Description("p141")]
        Axis2FolErr = 141,
        /// <summary>
        /// Axis3跟踪误差
        /// </summary>
        [Description("p142")]
        Axis3FolErr = 142,
        /// <summary>
        /// Axis4跟踪误差
        /// </summary>
        [Description("p143")]
        Axis4FolErr = 143,
        /// <summary>
        /// Axis5跟踪误差
        /// </summary>
        [Description("p144")]
        Axis5FolErr = 144,
        /// <summary>
        /// Axis6跟踪误差
        /// </summary>
        [Description("p145")]
        Axis6FolErr = 145,
        /// <summary>
        /// Axis7跟踪误差
        /// </summary>
        [Description("p146")]
        Axis7FolErr = 146,
        /// <summary>
        /// Axis8跟踪误差
        /// </summary>
        [Description("p147")]
        Axis8FolErr = 147,
        /// <summary>
        /// Axis9跟踪误差
        /// </summary>
        [Description("p148")]
        Axis9FolErr = 148,
        /// <summary>
        /// Axis10跟踪误差
        /// </summary>
        [Description("p149")]
        Axis10FolErr = 149,
        /// <summary>
        /// 轴回零标志位，位标志 1-已经回过零 0-未回零
        /// </summary>
        [Description("p150")]
        AxisHomeFlag = 150,
        /// <summary>
        /// 轴使能标志位，位标志 1-Enable 0-Disable
        /// </summary>
        [Description("p151")]
        AxisEnableFlag = 151,
        /// <summary>
        /// 主/C轴模式，位标志 1-C轴模式 0-主轴模式
        /// </summary>
        [Description("p152")]
        SpindleCMode = 152,
        /// <summary>
        /// 磨削主轴指令
        /// </summary>
        [Description("p153")]
        GrindSpeedCmd = 153,
        /// <summary>
        /// 修整主轴指令
        /// </summary>
        [Description("p154")]
        DressSpeedCmd = 154,

        /// <summary>
        /// 轴刹车标志位，位标志 1-松开 0-刹车
        /// </summary>
        [Description("p159")]
        AxisLockFlag = 159,

    };

    public enum EGCodesGroup
    {
        [Description("m200")]
        GCodesGroup0 = 200,
        [Description("p201")]
        GCodesGroup1 = 201,
        [Description("p202")]
        GCodesGroup2 = 202,
        [Description("p203")]
        GCodesGroup3 = 203,
        [Description("p204")]
        GCodesGroup4 = 204,
        [Description("p205")]
        GCodesGroup5 = 205,
        [Description("p206")]
        GCodesGroup6 = 206,
        [Description("p207")]
        GCodesGroup7 = 207,
        [Description("p208")]
        GCodesGroup8 = 208,
        [Description("p209")]
        GCodesGroup9 = 209,
        [Description("p210")]
        GCodesGroup10 = 210,
        [Description("p211")]
        GCodesGroup11 = 211,
        [Description("p212")]
        GCodesGroup12 = 212,
        [Description("p213")]
        GCodesGroup13 = 213,
        [Description("p214")]
        GCodesGroup14 = 214,
        [Description("p215")]
        GCodesGroup15 = 215,
        [Description("p216")]
        GCodesGroup16 = 216,
        [Description("p217")]
        GCodesGroup17 = 217,
        [Description("p218")]
        GCodesGroup18 = 218,
        [Description("p219")]
        GCodesGroup19 = 219,
        [Description("p220")]
        GCodesGroup20 = 220,
        [Description("p221")]
        GCodesGroup21 = 221,
        [Description("p222")]
        GCodesGroup22 = 222,
    }

    public enum EMacroGCodesGroup//没有启用
    {
        [Description("p225")]
        MacroGCodesGroup1 = 225,
        [Description("p226")]
        MacroGCodesGroup2 = 226,
        [Description("p227")]
        MacroGCodesGroup3 = 227,
        [Description("p228")]
        MacroGCodesGroup4 = 228,
        [Description("p229")]
        MacroGCodesGroup5 = 229,
        [Description("p230")]
        MacroGCodesGroup6 = 230,
        [Description("p231")]
        MacroGCodesGroup7 = 231,
        [Description("p232")]
        MacroGCodesGroup8 = 232,
        [Description("p233")]
        MacroGCodesGroup9 = 233,
        [Description("p234")]
        MacroGCodesGroup10 = 234,
        [Description("p235")]
        MacroGCodesGroup11 = 235,
        [Description("p236")]
        MacroGCodesGroup12 = 236,
        [Description("p237")]
        MacroGCodesGroup13 = 237,
        [Description("p238")]
        MacroGCodesGroup14 = 238,
        [Description("p239")]
        MacroGCodesGroup15 = 239,
        [Description("p240")]
        MacroGCodesGroup16 = 240,
        [Description("p241")]
        MacroGCodesGroup17 = 241,
        [Description("p242")]
        MacroGCodesGroup18 = 242,
        [Description("p243")]
        MacroGCodesGroup19 = 243,
        [Description("p244")]
        MacroGCodesGroup20 = 244,
        [Description("p245")]
        MacroGCodesGroup21 = 245,
        [Description("p246")]
        MacroGCodesGroup22 = 246,
    }

    public enum EMCodesGroup//没有启用
    {
        [Description("p300")]
        MCodesProgramGroup = 300,
        [Description("p301")]
        MCodesSpindleGroup = 301,
        [Description("p302")]
        MCodeCoolantGroup = 302,
        [Description("p303")]
        MCodesChuckGroup = 303,
        [Description("p304")]
        MCodesThreadingGroup = 304,
        [Description("p305")]
        MCodesGearRangeGroup = 305,
        [Description("p306")]
        MCodesFeedrateOverrideGroup = 306,
        [Description("p307")]
        MCodesBAxisGroup = 307,
        [Description("p308")]
        MCodesSubprogramGroup = 308,
        [Description("p309")]
        MCodesProbGroup = 309
    }

    public enum EDisplayReg
    {
        /// <summary>
        /// 实际进给速率
        /// </summary>
        [Description("p500")]
        DispFeedAct = 500,

        /// <summary>
        /// 指令进给速率
        /// </summary>
        [Description("p501")]
        DispFeedCmd = 501,

        /// <summary>
        /// 进给倍率
        /// </summary>
        [Description("p502")]
        DispFeedOverride = 502,
        /// <summary>
        /// 进给模式
        /// </summary>
        [Description("p503")]
        DispFeedMode = 503,
        /// <summary>
        /// 轴显示模式
        /// </summary>
        [Description("p504")]
        AxesDisplayMode = 504,
        /// <summary>
        /// 快移倍率
        /// </summary>
        [Description("p505")]
        DispRapidOverride = 505,

        /// <summary>
        /// 主轴实际速率
        /// </summary>
        [Description("p510")]
        DispSpindleAct = 510,
        /// <summary>
        /// 主轴指令速率
        /// </summary>
        [Description("p511")]
        DispSpindleCmd = 511,
        /// <summary>
        /// 主轴倍率
        /// </summary>
        [Description("p512")]
        DispSpindleOverride = 512,
        /// <summary>
        /// 主轴模式
        /// </summary>
        [Description("p513")]
        DispSpindleMode = 513,        
        /// <summary>
        /// 磨削主轴倍率
        /// </summary>
        [Description("p514")]
        DispGrindOverride = 514,

        /// <summary>
        /// 磨削主轴速度指令
        /// </summary>
        [Description("p515")]
        DispGrindCmd = 515,
        /// <summary>
        /// 磨削主轴实际速度
        /// </summary>
        [Description("p516")]
        DispGrindAct = 516,
        /// <summary>
        /// 修整主轴倍率
        /// </summary>
        [Description("p517")]
        DispDressOverride = 517,
        /// <summary>
        /// 修整主轴速度指令
        /// </summary>
        [Description("p518")]
        DispDressCmd = 518,
        /// <summary>
        /// 修整主轴实际速度
        /// </summary>
        [Description("p519")]
        DispDressAct = 519,
    };

    public enum EActiveTHDCode //没有启用
    {
        [Description("p520")]
        CS1ActiveTCode = 520,
        [Description("p521")]
        CS1ActiveHCode = 521,
        [Description("p522")]
        CS1ActiveDCode = 522,
        [Description("p600")]
        WorkCoordNum = 600,
        [Description("p601")]
        WorkOfsXPosMan = 601,
        [Description("p602")]
        WorkOfsYPosMan = 602,
        [Description("p603")]
        WorkOfsZPosMan = 603,
        [Description("p604")]
        ToolParamNum = 604,
        [Description("p605")]
        ToolRadius = 605,
        [Description("p606")]
        ToolWear = 606,

       
        [Description("p603")]
        WorkOfsBPosMan = 607,
        [Description("p604")]
        WorkOfsCPosMan = 608,

    }

    public enum EMisCellaneousParameters
    {
        /// <summary>
        /// 当前运行行号
        /// </summary>
        [Description("p692")]
        RunningLineNum = 692,
        /// <summary>
        /// 工件主轴状态 0-关闭 1-正转 2-反转
        /// </summary>
        [Description("p693")]
        SpindleState = 693,
        /// <summary>
        /// 磨削主轴状态 0-关闭 1-打开
        /// </summary>
        [Description("p694")]
        GrindState = 694,
        /// <summary>
        /// 修整主轴状态 0-关闭 1-打开
        /// </summary>
        [Description("p695")]
        DressState = 695,
        /// <summary>
        /// 磨削主轴和修整主轴保护组建状态 位状态 第0位-磨削主轴 第1位-修整主轴，0-关 1-开
        /// </summary>
        [Description("p696")]
        GrindProtection = 696,
        /// <summary>
        /// MachineInput机床床身输入状态
        /// </summary>
        [Description("p697")]
        MachineInput = 697,
        /// <summary>
        /// MachineOutput机床床身输出状态
        /// </summary>
        [Description("p698")]
        MachineOutput = 698,
        /// <summary>
        /// MachineAlarm机床报警
        /// </summary>
        [Description("p699")]
        MachineAlarm = 699,
        /// <summary>
        /// X1JOG速率
        /// </summary>
        [Description("p700")]
        JogX1 = 700,
        /// <summary>
        /// X10JOG速率
        /// </summary>
        [Description("p701")]
        JogX10 = 701,
        /// <summary>
        /// X100JOG速率
        /// </summary>
        [Description("p702")]
        JogX100 = 702,
        /// <summary>
        /// X1000JOG速率
        /// </summary>
        [Description("p703")]
        JogX1000 = 703,
        /// <summary>
        /// X10000JOG速率
        /// </summary>
        [Description("p704")]
        JogX10000 = 704,
        /// <summary>
        /// 增量JOG长度
        /// </summary>
        [Description("p705")]
        JogIncDist = 705,
        /// <summary>
        /// PowerPMAC下位机版本号
        /// </summary>
        [Description("p706")]
        PMACSourceVer = 706,
        /// <summary>
        /// 最大进给速率
        /// </summary>
        [Description("p707")]
        MaxFeedrate = 707,
        /// <summary>
        /// 回零取消
        /// </summary>
        [Description("p711")]
        HomeCancel = 711,
        /// <summary>
        /// MCP请求回零
        /// </summary>
        [Description("p712")]
        McpHomeFlag = 712,
        /// MCP请求切换MachineMode 1-Auto 2-Manual 3-Mdi
        /// </summary>
        [Description("p713")]
        McpSetMode = 713,
        /// MCP请求切换主/C轴
        /// </summary>
        [Description("p714")]
        McpCUMode = 714,
        /// MCP请求加工处理 循环开始-1 循环停止-2 倒带-3 进给保持-4
        /// </summary>
        [Description("p715")]
        McpProcessing = 715,
        /// MCP请求使能XZC三轴
        /// </summary>
        [Description("p716")]
        McpEnableXZC = 716,
        /// 下位机捕获到触发信号
        /// </summary>
        [Description("p717")]
        G32_Triggered = 717,
    };

    public enum ESkipPos
    {
        /// <summary>
        /// XskipPos
        /// </summary>
        [Description("p818")]
        XskipPos = 818,
        /// <summary>
        /// YskipPos
        /// </summary>
        [Description("p819")]
        YskipPos = 819,
        /// <summary>
        /// ZskipPos
        /// </summary>
        [Description("p820")]
        ZskipPos = 820,
        /// <summary>
        /// AskipPos
        /// </summary>
        [Description("p821")]
        AskipPos = 821,
        /// <summary>
        /// BskipPos
        /// </summary>
        [Description("p822")]
        BskipPos = 822,
        /// <summary>
        /// CskipPos
        /// </summary>
        [Description("p823")]
        CskipPos = 823,
        /// <summary>
        /// UskipPos
        /// </summary>
        [Description("p824")]
        UskipPos = 824,
        /// <summary>
        /// VskipPos
        /// </summary>
        [Description("p825")]
        VskipPos = 825,
        /// <summary>
        /// WskipPos
        /// </summary>
        [Description("p826")]
        WskipPos = 826,
    };

    public enum EWorkOffset
    {
        /// <summary>
        /// G54X
        /// </summary>
        [Description("p900")]
        CS1WorkG54X = 900,
        /// <summary>
        /// G54Y
        /// </summary>
        [Description("p901")]
        CS1WorkG54Y = 901,
        /// <summary>
        /// G54Z
        /// </summary>
        [Description("p902")]
        CS1WorkG54Z = 902,
        /// <summary>
        /// G55X
        /// </summary>
        [Description("p910")]
        CS1WorkG55X = 910,
        /// <summary>
        /// G55Y
        /// </summary>
        [Description("p911")]
        CS1WorkG55Y = 911,
        /// <summary>
        /// G55Z
        /// </summary>
        [Description("p912")]
        CS1WorkG55Z = 912,
        /// <summary>
        /// G56X
        /// </summary>
        [Description("p920")]
        CS1WorkG56X = 920,
        /// <summary>
        /// G56Y
        /// </summary>
        [Description("p921")]
        CS1WorkG56Y = 921,
        /// <summary>
        /// G56Z
        /// </summary>
        [Description("p922")]
        CS1WorkG56Z = 922,
        /// <summary>
        /// G57X
        /// </summary>
        [Description("p930")]
        CS1WorkG57X = 930,
        /// <summary>
        /// G57Y
        /// </summary>
        [Description("p931")]
        CS1WorkG57Y = 931,
        /// <summary>
        /// G57Z
        /// </summary>
        [Description("p932")]
        CS1WorkG57Z = 932,
        /// <summary>
        /// G58X
        /// </summary>
        [Description("p940")]
        CS1WorkG58X = 940,
        /// <summary>
        /// G58Y
        /// </summary>
        [Description("p941")]
        CS1WorkG58Y = 941,
        /// <summary>
        /// G58Z
        /// </summary>
        [Description("p942")]
        CS1WorkG58Z = 942,
        /// <summary>
        /// G59X
        /// </summary>
        [Description("p950")]
        CS1WorkG59X = 950,
        /// <summary>
        /// G59Y
        /// </summary>
        [Description("p951")]
        CS1WorkG59Y = 951,
        /// <summary>
        /// G59Z
        /// </summary>
        [Description("p952")]
        CS1WorkG59Z = 952,
    }


    public enum EToolOffset
    {
        /// <summary>
        /// Tool1CCGeom
        /// </summary>
        [Description("p3002")]
        CS1Tool1CCGeom = 3002,
        /// <summary>
        /// Tool1CCWear
        /// </summary>
        [Description("p3003")]
        CS1Tool1CCWear = 3003,
        /// <summary>
        /// Tool2CCGeom
        /// </summary>
        [Description("p3012")]
        CS1Tool2CCGeom = 3012,
        /// <summary>
        /// Tool2CCWear
        /// </summary>
        [Description("p3013")]
        CS1Tool2CCWear = 3013,
        /// <summary>
        /// Tool3CCGeom
        /// </summary>
        [Description("p3022")]
        CS1Tool3CCGeom = 3022,
        /// <summary>
        /// Tool3CCWear
        /// </summary>
        [Description("p3023")]
        CS1Tool3CCWear = 3023,
        /// <summary>
        /// Tool4CCGeom
        /// </summary>
        [Description("p3032")]
        CS1Tool4CCGeom = 3032,
        /// <summary>
        /// Tool4CCWear
        /// </summary>
        [Description("p3033")]
        CS1Tool4CCWear = 3033,
        /// <summary>
        /// Tool5CCGeom
        /// </summary>
        [Description("p3042")]
        CS1Tool5CCGeom = 3042,
        /// <summary>
        /// Tool5CCWear
        /// </summary>
        [Description("p3043")]
        CS1Tool5CCWear = 3043,
        /// <summary>
        /// Tool6CCGeom
        /// </summary>
        [Description("p3052")]
        CS1Tool6CCGeom = 3052,
        /// <summary>
        /// Tool6CCWear
        /// </summary>
        [Description("p3053")]
        CS1Tool6CCWear = 3053,
    }

    /// <summary>
    /// 轴显示模式
    /// </summary>
    public enum EAxesDisplayMode
    {
        ProgramPosition = 0,//程序位置
        RelativePosition = 1,//相对位置
        MachinePosition = 2,//机床位置
        CommandedPosition = 3//指令位置
    };

    /// <summary>
    /// 主轴模式
    /// </summary>
    public enum ESpindleMod
    {
        SpindleCut = 1,//切削模式
        SpindleCSS = 2//Constant Surface Speed(恒线速模式)
    };
}