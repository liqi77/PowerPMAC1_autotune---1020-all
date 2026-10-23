using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ODT.Common;
using ODT.PowerPmacComLib;
using System.ComponentModel;
using System.Text.RegularExpressions;


namespace Power_PMAC_COM
{
    public enum PMACComErrCode
    {
        /// <summary>
        /// 通信执行成功
        /// </summary>
        Success = 0,
        /// <summary>
        /// 同步Gpascii未连接
        /// </summary>
        SyncGpasciiNoConnected = 1,
        /// <summary>
        /// 异步Gpascii未连接
        /// </summary>
        AsyncGpasciiNoConnected = 2,
        /// <summary>
        /// 未成功获取正确指令文本
        /// </summary>
        NoCorrectText = 3,
        /// <summary>
        /// 同步Gpascii通信错误
        /// </summary>
        SyncGpasciiError = 4,
        /// <summary>
        /// 异步Gpascii通信错误
        /// </summary>
        AsyncGpassciiError = 5
    };

    public class PowerPamaCom
    {
        private ISyncGpasciiCommunicationInterface      PowerPmacComSyncGpascii;//PowerPMAC同步Gpascii
        private IAsyncGpasciiCommunicationInterface     PowerPmacComAsyncGpascii;//PowerPMAC异步Gpascii
        private ISyncTerminalCommunicationInterface     PowerPmacComSyncTerminal;//PowerPMAC同步Terminal
        private IAsyncTerminalCommunicationInterface    PowerPmacComAsyncTerminal;//PowerPMAC异步Terminal
        private IUnsolicitedEventInterface              PowerPmacComUnsolicitedEvent;//PowerPMAC主动传输Event
        private IGetErrorsEventInterface                PowerPmacComGetErrorsEvent;//PowerPMAC获取错误Event
        private IFTPClientInterface                     PowerPmacComFTPClient;//PowerPMAC的FTP传输

        private deviceProperties                        DeviceProperties;//PowerPMAC的设备属性

        private AsyncDataReceiveEvent                   AsyncGpasciiDataReceiveEventHandler;//异步Gpascii接受数据响应函数
        private AsyncDataReceiveEvent                   AsyncTerminalDataReceiveEventHandler;//异步Terminal接受数据响应函数
        private UnsolicitedResponse                     UnsolicitedResponseHandler;//PowerPMAC主动传输响应函数
        private GetErrorsResponse                       GetErrorsResponseHandler;//PowerPMAC主动发送错误响应函数

        public PowerPamaCom()
        {
            DeviceProperties = new deviceProperties();
            AsyncGpasciiDataReceiveEventHandler = null;
            AsyncTerminalDataReceiveEventHandler = null;
            UnsolicitedResponseHandler = null;
            GetErrorsResponseHandler = null;
        }

        /// <summary>
        /// PowerPMAC控制器通信接口初始化
        /// </summary>
        /// <returns></returns>
        public bool PowerPmacComInit(deviceProperties DevProp)
        {
            try
            {
                PowerPmacComSyncGpascii = Connect.CreateSyncGpascii(CommunicationGlobals.ConnectionTypes.SSH, null);
                PowerPmacComAsyncGpascii = Connect.CreateAsyncGpascii(CommunicationGlobals.ConnectionTypes.SSH, null);
                PowerPmacComSyncTerminal = Connect.CreateSyncTerminal(CommunicationGlobals.ConnectionTypes.SSH, null);
                PowerPmacComAsyncTerminal = Connect.CreateAsyncTerminal(CommunicationGlobals.ConnectionTypes.SSH, null);
                PowerPmacComUnsolicitedEvent = Connect.CreateUnsolicitedEvent(CommunicationGlobals.ConnectionTypes.SSH, null);
                PowerPmacComGetErrorsEvent = Connect.CreateGetErrorsEvent(CommunicationGlobals.ConnectionTypes.SSH, null);
                PowerPmacComFTPClient = Connect.CreateFTPClient(CommunicationGlobals.FTPConnectionTypes.FTP, null);
            }
            catch
            {
                return false;
            }
            DeviceProperties = DevProp;
            return true;
        }

        /// <summary>
        /// 连接同步Gpascii通信
        /// </summary>
        /// <returns></returns>
        public bool SyncGpasciiConnect()
        {
            PowerPmacComSyncGpascii = Connect.CreateSyncGpascii(DeviceProperties.Protocol, PowerPmacComSyncGpascii);
            var bSuccess = PowerPmacComSyncGpascii.ConnectGpAscii(DeviceProperties.IPAddress, DeviceProperties.PortNumber, DeviceProperties.User, DeviceProperties.Password);
            return bSuccess;
        }

        /// <summary>
        /// 检测是否已经连接同步Gpascii通信
        /// </summary>
        /// <returns></returns>
        public bool IsSyncGpasciiConnected()
        {
            return PowerPmacComSyncGpascii.GpAsciiConnected;
        }

        /// <summary>
        /// 以同步Gpascii通信方式发送Command，接受回应数据Response
        /// </summary>
        /// <param name="cmd">发送Command</param>
        /// <param name="response">回应数据Response</param>
        /// <returns></returns>
        public Status GetSyncGpasciiResponse(string cmd, out string response)
        {
            return PowerPmacComSyncGpascii.GetResponse(cmd, out response);
        }

        /// <summary>
        /// 断开同步Gpascii通信
        /// </summary>
        /// <returns></returns>
        public bool SyncGpasciiDisconnect()
        {
            var bSuccess = PowerPmacComSyncGpascii.DisconnectGpascii();
            return bSuccess;
        }

        /// <summary>
        /// 连接异步Gpascii通信
        /// </summary>
        /// <returns></returns>
        public bool AsyncGpasciiConnect()
        {
            PowerPmacComAsyncGpascii = Connect.CreateAsyncGpascii(DeviceProperties.Protocol, PowerPmacComAsyncGpascii);
            var bSuccess = PowerPmacComAsyncGpascii.ConnectGPAscii(DeviceProperties.IPAddress, DeviceProperties.PortNumber, DeviceProperties.User, DeviceProperties.Password);
            return bSuccess;
        }

        /// <summary>
        /// 检测是否已经连接异步Gpascii通信
        /// </summary>
        /// <returns></returns>
        public bool IsAsyncGpasciiConnected()
        {
            return PowerPmacComAsyncGpascii.GpAsciiConnected;
        }

        /// <summary>
        /// 以异步Gpascii通信方式发送Command，接受回应数据Response
        /// </summary>
        /// <param name="cmd">发送Command</param>
        /// <param name="response">回应数据Response</param>
        /// <returns></returns>
        public Status GetAsyncGpasciiResponse(string cmd)
        {
            return PowerPmacComAsyncGpascii.AsyncGetResponse(cmd);
        }

        public bool AddAsyncGpasciiReceiveHandler(AsyncDataReceiveEvent AsyncDataReceiveHandler)
        {
            if (AsyncGpasciiDataReceiveEventHandler != null)
                return false;
            AsyncGpasciiDataReceiveEventHandler = AsyncDataReceiveHandler;
            PowerPmacComAsyncGpascii.AsyncDataAvailable += AsyncGpasciiDataReceiveEventHandler;
            return true;
        }

        public bool RemoveAsyncGpasciiReceiveHandler()
        {
            if (AsyncGpasciiDataReceiveEventHandler == null)
                return false;
            PowerPmacComAsyncGpascii.AsyncDataAvailable -= AsyncGpasciiDataReceiveEventHandler;
            return true;
        }

        /// <summary>
        /// 断开异步Gpascii通信
        /// </summary>
        /// <returns></returns>
        public bool AsyncGpasciiDisconnect()
        {
            var bSuccess = PowerPmacComAsyncGpascii.DisconnectGpascii();
            return bSuccess;
        }

        /// <summary>
        /// 连接同步Terminal通信
        /// </summary>
        /// <returns></returns>
        public bool SyncTerminalConnect()
        {
            PowerPmacComSyncTerminal = Connect.CreateSyncTerminal(DeviceProperties.Protocol, PowerPmacComSyncTerminal);
            var bSuccess = PowerPmacComSyncTerminal.ConnectTerminal(DeviceProperties.IPAddress, DeviceProperties.PortNumber, DeviceProperties.User, DeviceProperties.Password);
            return bSuccess;
        }

        /// <summary>
        /// 检测是否已经连接同步Terminal通信
        /// </summary>
        /// <returns></returns>
        public bool IsSyncTerminalConnected()
        {
            return PowerPmacComSyncTerminal.SocketConnected;
        }

        /// <summary>
        /// 同步Terminal发送命令行
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public Status SyncTerminalSendCommand(string cmd, out string response)
        {
            return PowerPmacComSyncTerminal.SendCommand(cmd, out response);
        }

        /// <summary>
        /// 断开同步Terminal通信
        /// </summary>
        /// <returns></returns>
        public bool SyncTerminalDisconnect()
        {
            var bSuccess = PowerPmacComSyncTerminal.DisconnectTerminal();
            return bSuccess;
        }

        /// <summary>
        /// 连接异步Terminal通信
        /// </summary>
        /// <returns></returns>
        public bool AsyncTerminalConnect()
        {
            PowerPmacComAsyncTerminal = Connect.CreateAsyncTerminal(DeviceProperties.Protocol, PowerPmacComAsyncTerminal);
            var bSuccess = PowerPmacComAsyncTerminal.ConnectTerminal(DeviceProperties.IPAddress, DeviceProperties.PortNumber, DeviceProperties.User, DeviceProperties.Password);
            return bSuccess;
        }

        /// <summary>
        /// 检测是否已经连接异步Terminal通信
        /// </summary>
        /// <returns></returns>
        public bool IsAsyncTerminalConnected()
        {
            return PowerPmacComAsyncTerminal.SocketConnected;
        }
        
        /// <summary>
        /// 异步Terminal发送命令
        /// </summary>
        /// <param name="cmd">命令行</param>
        /// <returns></returns>
        public Status AsyncTerminalSendCommand(string cmd)
        {
            return PowerPmacComAsyncTerminal.AsyncSendCommand(cmd);
        }

        public bool AddAsyncTerminalReceiveHandler(AsyncDataReceiveEvent AsyncDataReceiveHandler)
        {
            if (AsyncTerminalDataReceiveEventHandler != null)
                return false;
            AsyncTerminalDataReceiveEventHandler = AsyncDataReceiveHandler;
            PowerPmacComAsyncTerminal.AsyncDataAvailable += AsyncTerminalDataReceiveEventHandler;
            return true;
        }

        public bool RemoveAsyncTerminalReceiveHandler()
        {
            if (AsyncTerminalDataReceiveEventHandler == null)
                return false;
            PowerPmacComAsyncTerminal.AsyncDataAvailable -= AsyncTerminalDataReceiveEventHandler;
            return true;
        }

        /// <summary>
        /// 断开异步Terminal通信
        /// </summary>
        /// <returns></returns>
        public bool AsyncTerminalDisconnect()
        {
            var bSuccess = PowerPmacComAsyncTerminal.DisconnectTerminal();
            return bSuccess;
        }

        /// <summary>
        /// 连接PowerPMAC主动上传Message通信
        /// </summary>
        /// <returns></returns>
        public bool UnsolicitedEventConnect()
        {
            PowerPmacComUnsolicitedEvent = Connect.CreateUnsolicitedEvent(DeviceProperties.Protocol, PowerPmacComUnsolicitedEvent);
            var bSuccess = PowerPmacComUnsolicitedEvent.ConnectEvent(DeviceProperties.IPAddress, DeviceProperties.PortNumber, DeviceProperties.User, DeviceProperties.Password);
            return bSuccess;
        }


        /// <summary>
        /// 检测是否已经PowerPMAC主动上传的通信
        /// </summary>
        /// <returns></returns>
        public bool IsUnsolicitedEventConnected()
        {
            return PowerPmacComUnsolicitedEvent.SocketConnected;
        }

        /// <summary>
        /// 检测是否已经存在PowerPMAC主动上传Com1的响应功能
        /// </summary>
        /// <returns></returns>
        public bool IsUnsolicitedEventRunning()
        {
            return PowerPmacComUnsolicitedEvent.IsRunning;
        }

        /// <summary>
        /// 连接PowerPMAC主动上传Message的Buffer1
        /// </summary>
        /// <param name="ResponseHandler"></param>
        /// <returns></returns>
        public bool UnsolicitedEventLinkCom1(UnsolicitedResponse ResponseHandler)
        {
            if (!IsUnsolicitedEventConnected())//判定UnsolicitedEvent通信是否连接
                return false;
            if (UnsolicitedResponseHandler != null)//判断是否已经连接Com1
                return false;
            PowerPmacComUnsolicitedEvent.UResponse += ResponseHandler;
            PowerPmacComUnsolicitedEvent.Buffer1 = true;
            var st = PowerPmacComUnsolicitedEvent.StartUnsolicitedEvent();
            switch(st)
            {
                case UnsolicitedStatus.Ok:
                    UnsolicitedResponseHandler = ResponseHandler;//成功才能记录UnsolicitedResponse的响应函数
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 关闭PowerPMAC主动上传Message的Buffer1
        /// </summary>
        /// <returns></returns>
        public bool UnsolicitedEventStopCom1()
        {
            if (UnsolicitedResponseHandler == null)
                return false;
            PowerPmacComUnsolicitedEvent.UResponse -= UnsolicitedResponseHandler;
            UnsolicitedResponseHandler = null;
            PowerPmacComUnsolicitedEvent.StopUnsolicitedEvent();
            return true;
        }

        /// <summary>
        /// 断开PowerPMAC主动上传Message通信
        /// </summary>
        /// <returns></returns>
        public bool UnsolicitedEventDisconnect()
        {
            var bSuccess = PowerPmacComUnsolicitedEvent.DisconnectEvent();
            return bSuccess;
        }

        /// <summary>
        /// 连接PowerPMAC主动报错事件
        /// </summary>
        /// <returns></returns>
        public bool GetErrorsEventConnect()
        {
            PowerPmacComGetErrorsEvent = Connect.CreateGetErrorsEvent(DeviceProperties.Protocol, PowerPmacComGetErrorsEvent);
            var bSuccess = PowerPmacComGetErrorsEvent.ConnectEvent(DeviceProperties.IPAddress, DeviceProperties.PortNumber, DeviceProperties.User, DeviceProperties.Password);
            return bSuccess;
        }

        /// <summary>
        /// 检测是否已经连接PowerPMAC主动报错的通信
        /// </summary>
        /// <returns></returns>
        public bool IsGetErrorsEventConnected()
        {
            return PowerPmacComGetErrorsEvent.SocketConnected;
        }

        /// <summary>
        /// 检测是否已经存在PowerPMAC主动报错的响应功能
        /// </summary>
        /// <returns></returns>
        public bool IsGetErrorsEventRunning()
        {
            return PowerPmacComGetErrorsEvent.IsRunning;
        }

        /// <summary>
        /// 开启PowerPMAC主动报错响应功能
        /// </summary>
        /// <param name="ResponseHandler"></param>
        /// <returns></returns>
        public bool StartGetErrorsEvent(GetErrorsResponse ResponseHandler)
        {
            if (!IsGetErrorsEventConnected())
                return false;
            if (GetErrorsResponseHandler != null)
                return false;
            PowerPmacComGetErrorsEvent.ErrorsResponse += ResponseHandler;
            var st = PowerPmacComGetErrorsEvent.StartGetErrorsEvent();
            switch(st)
            {
                case Status.Ok:
                    GetErrorsResponseHandler = ResponseHandler;
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 关闭PowerPMAC主动报错响应功能
        /// </summary>
        /// <returns></returns>
        public bool StopGetErrorsEvent()
        {
            if (GetErrorsResponseHandler == null)
                return false;
            PowerPmacComGetErrorsEvent.ErrorsResponse -= GetErrorsResponseHandler;
            GetErrorsResponseHandler = null;
            PowerPmacComGetErrorsEvent.StopGetErrorsEvent();
            return true;
        }

        /// <summary>
        /// 断开PowerPMAC主动上传报错事件
        /// </summary>
        /// <returns></returns>
        public bool GetErrorsEventDisconnect()
        {
            var bSuccess = PowerPmacComGetErrorsEvent.DisconnectEvent();
            return bSuccess;
        }

        /// <summary>
        /// 连接PowerPMAC的FTP客户端
        /// </summary>
        /// <returns></returns>
        public bool FtpClientConnect()
        {
            PowerPmacComFTPClient = Connect.CreateFTPClient(CommunicationGlobals.FTPConnectionTypes.FTP, PowerPmacComFTPClient);
            var bSuccess = PowerPmacComFTPClient.ConnectFTP(DeviceProperties.IPAddress, DeviceProperties.User, DeviceProperties.Password);
            return bSuccess;
        }

        /// <summary>
        /// 检测是否已经连接FTP客户端
        /// </summary>
        /// <returns></returns>
        public bool IsFTPClientConnected()
        {
            return PowerPmacComFTPClient.FTPConnected;
        }

        /// <summary>
        /// FTP客户端删除File
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public bool FTPClientRemoveFile(string filename)
        {
            if (!IsFTPClientConnected())
                return false;
            PowerPmacComFTPClient.RemoveFile(filename);
            return true;
        }

        /// <summary>
        /// FTP客户端获取文件大小
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public int FTPClientGetFileSize(string filename)
        {
            return PowerPmacComFTPClient.GetFileSize(filename);
        }

        /// <summary>
        /// FTP客户端下载文件，从上位机到PMAC
        /// </summary>
        /// <param name="FilePath">源文件路径</param>
        /// <param name="targetfilePath">目标文件路径</param>
        /// <returns></returns>
        public bool FTPClientDownloadFile(string FilePath, string targetfilePath)
        {
            if (!IsFTPClientConnected())
                return false;
            return PowerPmacComFTPClient.DownloadFile(FilePath, targetfilePath);
        }

        /// <summary>
        /// FTP客户端上载文件，从PMAC到上位机
        /// </summary>
        /// <param name="filename">源文件路径</param>
        /// <param name="targetfilePath">目标文件路径</param>
        /// <returns></returns>
        public bool FTPClientUploadFile(string filename, string targetfilePath)
        {
            if (!IsFTPClientConnected())
                return false;
            return PowerPmacComFTPClient.UploadFile(filename, targetfilePath);
        }

        /// <summary>
        /// 断开PowerPMAC的FTP客户端
        /// </summary>
        /// <returns></returns>
        public bool FtpClientDisconnect()
        {
            var bSuccess = PowerPmacComFTPClient.DisconnectFTP();
            return bSuccess;
        }

        /// <summary>
        /// PowerPMAC指令回应包分隔符号
        /// </summary>
        private const char Ack = '\x06';

        /// <summary>
        /// 获取枚举类型的描述
        /// </summary>
        /// <param name="enumValue"></param>
        /// <param name="enumValueText"></param>
        /// <returns></returns>
        public bool GetEnumDesp(Enum enumValue, out string enumValueText)
        {
            string str = enumValue.ToString();
            System.Reflection.FieldInfo field = enumValue.GetType().GetField(str);
            object[] obj = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (obj == null || obj.Length == 0)
            {
                enumValueText = str;
                return false;
            }
            else
            {
                DescriptionAttribute da = (DescriptionAttribute)obj[0];
                enumValueText = da.Description;
                return true;
            }
        }

        /// <summary>
        /// 生成一条Power PMAC命令
        /// </summary>
        /// <param name="eCommand">命令左半边的枚举类型</param>
        /// <param name="Val">命令右半边的数据</param>
        /// <param name="strCommand"></param>
        /// <returns></returns>
        public bool GetCommandText(Enum eCommand, int Val, out string strCommand)
        {
            string CommandLeft, CommandRight;
            strCommand = string.Empty;
            if (!GetEnumDesp(eCommand, out CommandLeft))
            {
                return false;
            }
            CommandRight = Val.ToString();
            strCommand = CommandLeft + "=" + CommandRight + "\r";
            return true;
        }


        /// <summary>
        /// 生成赋值命令文本
        /// </summary>
        /// <param name="eCommand">命令左半边的枚举类型</param>
        /// <param name="Val">命令右半边的数据</param>
        /// <param name="strCommand"></param>
        /// <returns></returns>
        public bool GetCommandText(Enum eCommand, double Val, out string strCommand)
        {
            string CommandLeft, CommandRight;
            strCommand = string.Empty;
            if (!GetEnumDesp(eCommand, out CommandLeft))
            {
                return false;
            }
            CommandRight = Val.ToString();
            strCommand = CommandLeft + "=" + CommandRight + "\r";
            return true;
        }

        /// <summary>
        /// 生成查询命令文本
        /// </summary>
        /// <param name="eInquire"></param>
        /// <param name="strInquire">查询命令</param>
        /// <param name="iNum">连续读取参数数量</param>
        /// <returns></returns>
        public bool GetInquireText(Enum eInquire, out string strInquire, int iNum = 1)
        {
            string Inquire;
            Inquire = string.Empty;
            if (!GetEnumDesp(eInquire, out Inquire))
            {
                strInquire = Inquire;
                return false;
            }
            if (iNum > 1)
            {
                strInquire = Inquire + "," + iNum.ToString() + " \r";
            }
            else
            {
                strInquire = Inquire + " \r";
            }
            return true;
        }

        /// <summary>
        /// 将Power PMAC的多行回应包数据进行拆分、分割和去除空元素
        /// </summary>
        /// <param name="strResponse">Gpascii多行回应包数据</param>
        /// <param name="strArrayResponse">拆分出回应包数据元素</param>
        /// <returns></returns>
        public bool ParseResponsePack(string strResponse, out string[] strArrayResponse)
        {
            string pmacResponse = strResponse.ToLower();//将Power PMAC返回字符串，转换成小写
            pmacResponse = pmacResponse.Replace(Ack.ToString(), string.Empty);//将"\x06"转换成空String
            strArrayResponse = Regex.Split(pmacResponse, "\n", RegexOptions.IgnoreCase);//用"\n"分割成String数组
            strArrayResponse = strArrayResponse.Where(s => !string.IsNullOrEmpty(s)).ToArray();//清理出String数组的空元素
            if (strArrayResponse.Length == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 解析Power PMAC的单条回应包数据
        /// </summary>
        /// <param name="strResponse">Gpascii回应包数据</param>
        /// <param name="strLeft">回应包数据的左半边String</param>
        /// <param name="strRight">回应包数据的右半边String</param>
        /// <returns></returns>
        public bool ParseResponseText(string strResponse, out string strLeft, out string strRight)
        {
            strLeft = string.Empty;
            strRight = string.Empty;
            if (!strResponse.Contains("="))//不包含“=”，无法进行解析
            {
                return false;
            }
            string[] ResponseArray = Regex.Split(strResponse, "=");
            if (ResponseArray.Length != 2)
            {
                return false;
            }
            else
            {
                strLeft = ResponseArray[0].Trim();
                strRight = ResponseArray[1].Trim();
                return true;
            }
        }

        public delegate void DisplayPmacErrFunc(PMACComErrCode ePmacComErrCode);//委托函数，解析不同类型数据

        /// <summary>
        /// 设置PMAC寄存器
        /// </summary>
        /// <param name="eCmd">寄存器</param>
        /// <param name="iVal">设置值</param>
        /// <param name="Func">委托函数</param>
        /// <returns></returns>
        public PMACComErrCode SetPMACReg(Enum eCmd, int iVal, DisplayPmacErrFunc Func = null)
        {
            PMACComErrCode ePmacComErrCode = PMACComErrCode.Success;
            if (IsSyncGpasciiConnected())
            {
                string cmd, response;
                if (GetCommandText(eCmd, iVal, out cmd))
                {
                    var st = GetSyncGpasciiResponse(cmd, out response);
                    if (st != Status.Ok)
                    {
                        ePmacComErrCode = PMACComErrCode.SyncGpasciiError;
                    }
                    else
                    {
                        ePmacComErrCode = PMACComErrCode.Success;
                    }
                }
                else
                {
                    ePmacComErrCode = PMACComErrCode.NoCorrectText;
                }
            }
            else
            {
                ePmacComErrCode = PMACComErrCode.SyncGpasciiNoConnected;
            }

            Func?.Invoke(ePmacComErrCode);

            return ePmacComErrCode;
        }

        /// <summary>
        /// 设置PMAC寄存器
        /// </summary>
        /// <param name="eCmd">寄存器</param>
        /// <param name="dVal">设置值</param>
        /// <param name="Func">委托函数</param>
        /// <returns></returns>
        public PMACComErrCode SetPMACReg(Enum eCmd, double dVal, DisplayPmacErrFunc Func = null)
        {
            PMACComErrCode ePmacComErrCode = PMACComErrCode.Success;
            if (IsSyncGpasciiConnected())
            {
                string cmd, response;
                if (GetCommandText(eCmd, dVal, out cmd))
                {
                    var st = GetSyncGpasciiResponse(cmd, out response);
                    if (st != Status.Ok)
                    {
                        ePmacComErrCode = PMACComErrCode.SyncGpasciiError;
                    }
                    else
                    {
                        ePmacComErrCode = PMACComErrCode.Success;
                    }
                }
                else
                {
                    ePmacComErrCode =  PMACComErrCode.NoCorrectText;
                }
            }
            else
            {
                ePmacComErrCode = PMACComErrCode.SyncGpasciiNoConnected;
            }

            Func?.Invoke(ePmacComErrCode);

            return ePmacComErrCode;
        }

        /// <summary>
        /// 获取PMAC寄存器值
        /// </summary>
        /// <param name="eInq">寄存器</param>
        /// <param name="iNum">获取寄存器数量</param>
        /// <param name="response">返回的查询Text文本</param>
        /// <param name="Func">委托函数</param>
        /// <returns></returns>
        public PMACComErrCode GetPMACReg(Enum eInq, int iNum, out string response, DisplayPmacErrFunc Func = null)
        {
            PMACComErrCode ePmacComErrCode = PMACComErrCode.Success;
            response = string.Empty;
            if (IsSyncGpasciiConnected())
            {
                string inq;
                if (GetInquireText(eInq, out inq, iNum))
                 {
                    var st = GetSyncGpasciiResponse(inq, out response);
                    if (st != Status.Ok)
                    {
                        ePmacComErrCode = PMACComErrCode.SyncGpasciiError;
                    }
                    else
                    {
                        ePmacComErrCode = PMACComErrCode.Success;
                    }
                }
                else
                {
                    ePmacComErrCode = PMACComErrCode.NoCorrectText;
                }
            }
            else
            {
                ePmacComErrCode = PMACComErrCode.SyncGpasciiNoConnected;
            }

            Func?.Invoke(ePmacComErrCode);

            return ePmacComErrCode;
        }

        public delegate void ParseFunc(string textLeft, string textRight);//委托函数，解析不同类型数据
        public void ParsePMACBlockResponse(string response, ParseFunc parseFunc)
        {
            string[] responseArray;
            if (ParseResponsePack(response, out responseArray))
            {
                for (int i = 0; i < responseArray.Length; i++)
                {
                    string responseleft, responseright;
                    ParseResponseText(responseArray[i], out responseleft, out responseright);
                    parseFunc(responseleft, responseright);
                }
            }
        }
    }
}
