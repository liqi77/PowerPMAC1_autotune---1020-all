using ODT.Common;
using ODT.Common.Properties;
using ODT.PowerPmacComLib;
using Power_PMAC_COM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace PMacLearn
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private PowerPamaCom ppCom = new PowerPamaCom();//PowerPMAC通信类
        private deviceProperties currentDevProp = new deviceProperties();//PowerPMAC控制器设备属性
        Image connectLargeImage;
        Image disconnectLargeImage;
        string rootFolder = System.Environment.CurrentDirectory;
        int rotateAxisIndex = 5;
        bool pmacConStatus = false;//PMAC连接状态
        private delegate void displayMessageDelegate();//委托
        double rotatePos = 0;
        double rotateSpeed = 0;
        double rotateFolError = 0;
        int iAxisHomeFlag = 0;
        int iAxisEnableFlag = 0;
        int iSpindleCMode = 0;
        int iAxisLockFlag = 0;

        private void Form1_Load(object sender, EventArgs e)
        {
            connectLargeImage = Image.FromFile(rootFolder + "\\Pictures\\green.png");
            disconnectLargeImage = Image.FromFile(rootFolder + "\\Pictures\\red.png");

            powerPamcConnectInit();

            pixConStatus.Image = disconnectLargeImage;
            pixHomeStatus.Image = disconnectLargeImage;

            pmacConStatus = false;
        }

        /// <summary>
        /// 连接PowerPMAC通信
        /// </summary>
        private void PowerPmacConnect()
        {
            //以Sync Gpascii方式连接PMAC
            tssStatus.Text = "正在以同步Gpascii方式连接PowerPMAC运动控制器...";
            Thread.Sleep(500);
            if (ppCom.SyncGpasciiConnect())
            {
                tssStatus.Text = "同步Gpascii连接成功！";
                Thread.Sleep(500);
            }
            else
            {
                tssStatus.Text = "同步Gpascii连接失败，请检查硬件和参数设置！";

                return;
            }
            //以Sync Terminal方式连接PMAC            
            tssStatus.Text = "正在以同步Terminal方式连接PowerPMAC运动控制器...";
            Thread.Sleep(500);
            if (ppCom.SyncTerminalConnect())
            {
                tssStatus.Text = "同步Terminal连接成功！";
                Thread.Sleep(500);
            }
            else
            {
                tssStatus.Text = "同步Terminal连接失败，请检查硬件和参数设置！";

                return;
            }
            //连接FTP接口
            tssStatus.Text = "正在连接PowerPMAC主动传输接口...";
            Thread.Sleep(500);
            if (ppCom.FtpClientConnect())
            {
                tssStatus.Text = "FTP连接成功！";
                Thread.Sleep(500);
            }
            else
            {
                tssStatus.Text = "FTP连接失败，请检查硬件和参数设置！";

                return;
            }
            //连接PMAC主动传输接口
            tssStatus.Text = "正在连接PowerPMAC主动传输接口...";
            Thread.Sleep(500);
            if (ppCom.UnsolicitedEventConnect())
            {
                tssStatus.Text = "PowerPMAC主动传输接口连接成功！";
                Thread.Sleep(500);
                if (ppCom.UnsolicitedEventLinkCom1(powerpmacUnsolicitedMessageResponse))
                {

                    tssStatus.Text = "PowerPMAC主动传输接口COM1通信开启！";
                }
                else
                {
                    tssStatus.Text = "PowerPMAC主动传输接口COM1通信超时！";

                    return;
                }
                Thread.Sleep(500);
            }
            else
            {
                tssStatus.Text = "PowerPMAC主动传输接口连接失败，请检查硬件和参数设置！";

            }


            tssStatus.Text = "PowerPMAC连接成功!";

            pixConStatus.Image = connectLargeImage;
            pmacConStatus = true;
            ppCom.SetPMACReg(ECommandReg.CommandReg, (int)ECommandCodes.Initialize, DisplayPowerPmacComError);
            //PowerPMACParameterInit();

            timerUpdateStatus.Enabled = true;
            timerUpdateStatus.Start();
        }

        /// <summary>
        /// Power PMAC主动传输数据响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void powerpmacUnsolicitedMessageResponse(object sender, UnsolicitedArgs e)
        {
            string powerpmacMessage = e.UnsolicitedMessage;
            string stickycomment = string.Empty;


            displayMessageDelegate displayMesDelegate3 = delegate ()
            {

                this.tssStatus.Text = stickycomment;

            };
            this.Invoke(displayMesDelegate3);
        }


        /// <summary>
        /// 断开PowerPMAC通信
        /// </summary>
        private void PowerPmacDisconnect()
        {

            //关闭同步Gpascii和Terminal通信
            ppCom.SyncGpasciiDisconnect();
            ppCom.SyncTerminalDisconnect();
            ppCom.FtpClientDisconnect();
            //关闭PowerPMAC主动上传Message通信
            ppCom.UnsolicitedEventStopCom1();
            ppCom.UnsolicitedEventDisconnect();


        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            PowerPmacConnect();

        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            PowerPmacDisconnect();
            pixConStatus.Image = disconnectLargeImage;
            pmacConStatus = false;
        }

        private void btnServerOn_Click(object sender, EventArgs e)
        {
            ppCom.SetPMACReg(ECommandReg.CommandReg, (int)ECommandCodes.Axis1Enable + rotateAxisIndex, DisplayPowerPmacComError);
        }

        private void btnServerOff_Click(object sender, EventArgs e)
        {
            ppCom.SetPMACReg(ECommandReg.CommandReg, (int)ECommandCodes.Xbreak + rotateAxisIndex, DisplayPowerPmacComError);
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            if (ppCom.SetPMACReg(ECommandReg.CommandReg, (int)ECommandCodes.Axis1Home + rotateAxisIndex, DisplayPowerPmacComError) == PMACComErrCode.Success)
            {
                pixHomeStatus.Image = connectLargeImage;
            }
            else
            {
                pixHomeStatus.Image = disconnectLargeImage;
            }

        }


        private void DisplayPowerPmacComError(PMACComErrCode ePmacComErrCode)
        {
            string status = "";
            switch (ePmacComErrCode)
            {
                case PMACComErrCode.SyncGpasciiError:
                    status = "同步Gpascii通信错误！";
                    break;
                case PMACComErrCode.AsyncGpassciiError:
                    status = "异步Gpascii通信错误！";
                    break;
                case PMACComErrCode.NoCorrectText:
                    status = "未成功获取命令文本！";
                    break;
                case PMACComErrCode.SyncGpasciiNoConnected:
                case PMACComErrCode.AsyncGpasciiNoConnected:
                    status = "未与PowerPMAC建立通信，请连接！";
                    break;
            }
            tssStatus.Text = status;
        }


        private void btnRead_Click(object sender, EventArgs e)
        {
            int iMotorNum = 9;//一次性获取9个轴数据
            string response;
            //读取Axis Position
            if (ppCom.GetPMACReg(EAxesPosReg.Axis1Positioin, iMotorNum, out response, DisplayPowerPmacComError) == PMACComErrCode.Success)
            {
                ppCom.ParsePMACBlockResponse(response, ParseCNCAxisData);

                txtFeedbackAngle.Text = rotatePos.ToString();
                txtRotateSpeed.Text = rotateSpeed.ToString();
            }
        }

        /// <summary>
        /// 解析CNC下位机轴数据
        /// </summary>
        /// <param name="TextLeft"></param>
        /// <param name="ValueRight"></param>
        private void ParseCNCAxisData(string TextLeft, string ValueRight)
        {
            switch (TextLeft)
            {
                //轴位置
                case "p105": try { rotatePos = Convert.ToDouble(ValueRight); } catch { } break;
                //轴DTG目标位置
                // case "p115": try { dAxisDTG[5] = Convert.ToDouble(ValueRight); } catch { } break;
                //轴扭矩
                // case "p125": try { dAxisTorque[5] = Convert.ToDouble(ValueRight); } catch { } break;
                //轴速度
                case "p135": try { rotateSpeed = Convert.ToDouble(ValueRight); } catch { } break;
                //轴FolErr
                case "p145": try { rotateFolError = Convert.ToDouble(ValueRight); } catch { } break;
                //轴回零标志
                case "p150": try { iAxisHomeFlag = Convert.ToInt32(ValueRight); } catch { } break;
                case "p151": try { iAxisEnableFlag = Convert.ToInt32(ValueRight); } catch { } break;
                case "p152": try { iSpindleCMode = Convert.ToInt32(ValueRight); } catch { } break;
                case "p159": try { iAxisLockFlag = Convert.ToInt32(ValueRight); } catch { } break;
            }
        }

        private void btnWrite_Click(object sender, EventArgs e)
        {
            int position = Convert.ToInt32(txtCmdAngle.Text);
            ppCom.SetPMACReg(EAxesPosReg.Axis6disttogo, position, DisplayPowerPmacComError);
        }

        /// <summary>
        /// PowerPmac控制器连接功能初始化
        /// </summary>
        public void powerPamcConnectInit()
        {
            currentDevProp.IPAddress = "172.20.0.200";  //172.20.0.200  192.168.0.200
            currentDevProp.PortNumber = Convert.ToInt16("23");
            currentDevProp.User = "root";
            currentDevProp.Password = "deltatau";
            currentDevProp.Protocol = CommunicationGlobals.ConnectionTypes.SSH;
            if (!ppCom.PowerPmacComInit(currentDevProp))
            {
                MessageBox.Show("检查PowerPMAC的PDK授权，请与设备生产商联系......");

            }
            else
            {
                pixConStatus.Image = connectLargeImage;
                pmacConStatus = true;
            }

        }

        private void timerUpdateStatus_Tick(object sender, EventArgs e)
        {
            string response;
            if (pmacConStatus == true)
            {
                //读取Axis 回零标志位
                if (ppCom.GetPMACReg(EAxesPosReg.AxisHomeFlag, 3, out response, DisplayPowerPmacComError) == PMACComErrCode.Success)
                {
                    ppCom.ParsePMACBlockResponse(response, ParseCNCAxisData);

                    txtFeedbackAngle.Text = rotatePos.ToString();
                    txtRotateSpeed.Text = rotateSpeed.ToString();

                }
            }
        }
    }
}
