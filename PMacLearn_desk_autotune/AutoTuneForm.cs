using ODT.Common;
using ODT.PowerPmacComLib;
using Power_PMAC_COM;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace PMacLearn
{
    public class AutoTuneForm : Form
    {
        private readonly PowerPamaCom ppCom = new PowerPamaCom();
        private readonly deviceProperties device = new deviceProperties();
        private readonly Timer pollTimer = new Timer();
        private readonly Dictionary<int, double> pValues = new Dictionary<int, double>();
        private readonly List<TuneSample> samples = new List<TuneSample>();
        private readonly TextBox[] pidFields = new TextBox[5];
        private readonly TextBox[] testFields = new TextBox[4];
        private readonly Label stateValue = new Label();
        private readonly Label errorValue = new Label();
        private readonly Label connectionValue = new Label();
        private readonly Label[] metricValues = new Label[5];
        private readonly Chart sampleChart = new Chart();
        private readonly Button pauseButton = new Button();
        private bool connected;
        private bool polling;
        private double maxSpeed;
        private double maxAcceleration;
        private double maxDac;

        private static readonly string[] PidNames = { "Kp", "Kvfb", "Kvff", "Kaff", "Ki" };
        private static readonly string[] TestNames = { "测试位移 (counts)", "测试速度 (counts/s)", "测试加速度 (counts/s^2)", "最大 DAC (counts)" };
        private static readonly int[] PidRegisters = { 720, 721, 722, 723, 724 };
        private static readonly int[] TestRegisters = { 710, 711, 712, 713 };
        private static readonly string[] MetricNames = { "峰值误差", "RMS 误差", "稳态误差", "超调量", "稳定时间" };

        public AutoTuneForm()
        {
            Text = "PowerPMAC 自动调谐";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(980, 700);
            ClientSize = new Size(1180, 850);
            Font = new Font("Microsoft YaHei UI", 9F);
            BackColor = Color.FromArgb(245, 247, 248);
            BuildInterface();

            pollTimer.Interval = 500;
            pollTimer.Tick += PollTimer_Tick;
            FormClosing += AutoTuneForm_FormClosing;
            SetConnected(false);
        }

        private void BuildInterface()
        {
            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(14),
                ColumnCount = 1,
                RowCount = 5,
                AutoSize = false
            };
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 170));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 114));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 120));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            Controls.Add(root);

            var connectionBar = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false, FlowDirection = FlowDirection.LeftToRight, Padding = new Padding(0, 4, 0, 0) };
            connectionValue.AutoSize = true;
            connectionValue.Text = "通信断开";
            connectionValue.ForeColor = Color.Firebrick;
            connectionValue.Font = new Font(Font, FontStyle.Bold);
            connectionValue.Margin = new Padding(4, 8, 18, 0);
            connectionBar.Controls.Add(connectionValue);
            connectionBar.Controls.Add(MakeButton("连接", ConnectClicked, 88));
            connectionBar.Controls.Add(MakeButton("断开", DisconnectClicked, 88));
            connectionBar.Controls.Add(MakeButton("读取当前参数", ReadParametersClicked, 118));
            connectionBar.Controls.Add(MakeButton("读取采样", ReadSamplesClicked, 96));
            connectionBar.Controls.Add(MakeButton("导出 CSV", ExportCsvClicked, 96));
            root.Controls.Add(connectionBar, 0, 0);

            var top = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1 };
            top.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            top.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            root.Controls.Add(top, 0, 1);
            top.Controls.Add(BuildParameterGroup(), 0, 0);
            top.Controls.Add(BuildTestGroup(), 1, 0);

            var commands = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false, Padding = new Padding(0, 12, 0, 0) };
            commands.Controls.Add(MakeButton("开始调谐", StartClicked, 108));
            commands.Controls.Add(MakeButton("写入测试参数", WriteTestParametersClicked, 112));
            pauseButton.Text = "暂停";
            pauseButton.Width = 88;
            pauseButton.Height = 34;
            pauseButton.Click += PauseClicked;
            commands.Controls.Add(pauseButton);
            commands.Controls.Add(MakeButton("停止", StopClicked, 88));
            commands.Controls.Add(MakeButton("应用候选参数", ApplyCandidateClicked, 120));
            commands.Controls.Add(MakeButton("恢复备份参数", RestoreBackupClicked, 120));
            commands.Controls.Add(MakeButton("保存最终参数", SaveFinalClicked, 120));
            root.Controls.Add(commands, 0, 2);

            root.Controls.Add(BuildStatusGroup(), 0, 3);
            sampleChart.Dock = DockStyle.Fill;
            sampleChart.BackColor = Color.White;
            sampleChart.BorderlineColor = Color.FromArgb(215, 221, 224);
            sampleChart.BorderlineDashStyle = ChartDashStyle.Solid;
            sampleChart.BorderlineWidth = 1;
            sampleChart.ChartAreas.Add(new ChartArea("Samples")
            {
                BackColor = Color.White,
                AxisX = { Title = "采样点", MajorGrid = { LineColor = Color.FromArgb(232, 236, 238) } },
                AxisY = { MajorGrid = { LineColor = Color.FromArgb(232, 236, 238) } }
            });
            sampleChart.Legends.Add(new Legend("Legend") { Docking = Docking.Top, Alignment = StringAlignment.Near });
            AddSeries("指令位置", Color.FromArgb(33, 115, 99));
            AddSeries("实际位置", Color.FromArgb(54, 103, 160));
            AddSeries("跟随误差", Color.FromArgb(210, 118, 45));
            AddSeries("DAC", Color.FromArgb(147, 75, 75));
            var chartGroup = new GroupBox { Text = "采样曲线", Dock = DockStyle.Fill, Padding = new Padding(8) };
            chartGroup.Controls.Add(sampleChart);
            root.Controls.Add(chartGroup, 0, 4);
        }

        private Control BuildParameterGroup()
        {
            var group = new GroupBox { Text = "当前伺服参数", Dock = DockStyle.Fill, Padding = new Padding(10) };
            var grid = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 5, Padding = new Padding(4) };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 82));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            for (int i = 0; i < PidNames.Length; i++)
            {
                grid.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
                grid.Controls.Add(MakeLabel(PidNames[i]), 0, i);
                pidFields[i] = MakeInput();
                grid.Controls.Add(pidFields[i], 1, i);
            }
            group.Controls.Add(grid);
            return group;
        }

        private Control BuildTestGroup()
        {
            var group = new GroupBox { Text = "测试参数", Dock = DockStyle.Fill, Padding = new Padding(10) };
            var grid = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 4, Padding = new Padding(4) };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 170));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            for (int i = 0; i < TestNames.Length; i++)
            {
                grid.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
                grid.Controls.Add(MakeLabel(TestNames[i]), 0, i);
                testFields[i] = MakeInput();
                testFields[i].Text = i == 0 ? "5000" : i == 1 ? "1000" : i == 2 ? "50" : "16000";
                grid.Controls.Add(testFields[i], 1, i);
            }
            group.Controls.Add(grid);
            return group;
        }

        private Control BuildStatusGroup()
        {
            var group = new GroupBox { Text = "调谐状态与实时指标", Dock = DockStyle.Fill, Padding = new Padding(10) };
            var grid = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 7, RowCount = 2, Padding = new Padding(4) };
            for (int i = 0; i < 5; i++) grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12));
            grid.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            grid.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            for (int i = 0; i < MetricNames.Length; i++)
            {
                grid.Controls.Add(MakeLabel(MetricNames[i]), i, 0);
                metricValues[i] = MakeLabel("--");
                metricValues[i].Font = new Font(Font, FontStyle.Bold);
                grid.Controls.Add(metricValues[i], i, 1);
            }
            grid.Controls.Add(MakeLabel("状态"), 5, 0);
            stateValue.Text = "未连接";
            stateValue.AutoEllipsis = true;
            grid.Controls.Add(stateValue, 5, 1);
            grid.Controls.Add(MakeLabel("错误信息"), 6, 0);
            errorValue.Text = "--";
            errorValue.AutoEllipsis = true;
            errorValue.ForeColor = Color.Firebrick;
            grid.Controls.Add(errorValue, 6, 1);
            group.Controls.Add(grid);
            return group;
        }

        private void AddSeries(string name, Color color)
        {
            sampleChart.Series.Add(new Series(name)
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 2,
                Color = color,
                ChartArea = "Samples",
                XValueType = ChartValueType.Int32
            });
        }

        private static Label MakeLabel(string text)
        {
            return new Label { Text = text, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft, AutoEllipsis = true };
        }

        private static TextBox MakeInput()
        {
            return new TextBox { Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Right, Margin = new Padding(3, 4, 8, 4) };
        }

        private Button MakeButton(string text, EventHandler handler, int width)
        {
            var button = new Button { Text = text, Width = width, Height = 34, Margin = new Padding(4, 0, 8, 0), UseVisualStyleBackColor = true };
            button.Click += handler;
            return button;
        }

        private void ConnectClicked(object sender, EventArgs e)
        {
            try
            {
                device.IPAddress = "172.20.0.200"; // 192.168.0.200
                device.PortNumber = 22;
                device.User = "root";
                device.Password = "deltatau";
                device.Protocol = CommunicationGlobals.ConnectionTypes.SSH;
                if (!ppCom.PowerPmacComInit(device) || !ppCom.SyncGpasciiConnect())
                    throw new InvalidOperationException("同步 Gpascii 连接失败，请检查控制器地址和 PDK 配置。");
                SetConnected(true);
                ReadLimits();
                ReadParameters();
                PollController();
                pollTimer.Start();
            }
            catch (Exception ex)
            {
                SetConnected(false);
                ShowError(ex.Message);
            }
        }

        private void DisconnectClicked(object sender, EventArgs e)
        {
            pollTimer.Stop();
            try { if (ppCom.IsSyncGpasciiConnected()) ppCom.SyncGpasciiDisconnect(); }
            catch { }
            SetConnected(false);
        }

        private void SetConnected(bool value)
        {
            connected = value;
            connectionValue.Text = value ? "通信已连接" : "通信断开";
            connectionValue.ForeColor = value ? Color.FromArgb(33, 115, 99) : Color.Firebrick;
            if (!value) stateValue.Text = "未连接";
        }

        private void ReadParametersClicked(object sender, EventArgs e)
        {
            try { EnsureConnected(); ReadParameters(); }
            catch (Exception ex) { ShowError(ex.Message); }
        }

        private void ReadParameters()
        {
            for (int i = 0; i < PidFieldsLength; i++)
            {
                double value = ReadExpression("Motor[1].Servo." + PidNames[i]);
                pidFields[i].Text = value.ToString("G10", CultureInfo.InvariantCulture);
                pValues[720 + i] = value;
            }
        }

        private int PidFieldsLength { get { return pidFields.Length; } }

        private void ReadLimits()
        {
            maxSpeed = ReadExpression("Motor[1].MaxSpeed");
            maxAcceleration = Math.Abs(ReadExpression("Motor[1].InvAmax"));
            maxDac = Math.Abs(ReadExpression("Motor[1].MaxDac"));
        }

        private void StartClicked(object sender, EventArgs e)
        {
            try
            {
                EnsureConnected();
                if ((int)ReadExpression("P701") != 0) throw new InvalidOperationException("当前调谐状态不是空闲，请先停止并确认下位机回到空闲状态。");
                ReadLimits();
                double[] values = ReadAndValidateTestValues();
                for (int i = 0; i < values.Length; i++) WriteP(TestRegisters[i], values[i]);
                WriteP(714, Math.Abs(ReadExpression("Motor[1].FatalFeLimit")) * 0.8);
                WriteP(700, 1);
                errorValue.Text = "--";
                samples.Clear();
                ClearChart();
                SetStatus("启动请求已发送");
            }
            catch (Exception ex) { ShowError(ex.Message); }
        }

        private void WriteTestParametersClicked(object sender, EventArgs e)
        {
            try
            {
                EnsureConnected();
                if ((int)ReadExpression("P701") != 0) throw new InvalidOperationException("调谐运行期间不能修改测试参数。");
                ReadLimits();
                var values = ReadAndValidateTestValues();
                for (int i = 0; i < values.Length; i++) WriteP(TestRegisters[i], values[i]);
                WriteP(714, Math.Abs(ReadExpression("Motor[1].FatalFeLimit")) * 0.8);
                SetStatus("测试参数已写入下位机");
            }
            catch (Exception ex) { ShowError(ex.Message); }
        }

        private double[] ReadAndValidateTestValues()
        {
            var values = new double[testFields.Length];
            for (int i = 0; i < values.Length; i++)
            {
                if (!double.TryParse(testFields[i].Text, NumberStyles.Float, CultureInfo.CurrentCulture, out values[i]) ||
                    double.IsNaN(values[i]) || double.IsInfinity(values[i]))
                    throw new InvalidOperationException(TestNames[i] + "必须是有效数字。");
            }
            if (values[0] < 1000 || values[0] > 50000) throw new InvalidOperationException("测试位移范围为 1000 到 50000 counts。");
            if (values[1] <= 0 || values[1] > maxSpeed) throw new InvalidOperationException("测试速度必须大于 0 且不超过控制器 MaxSpeed (" + maxSpeed.ToString("G6", CultureInfo.InvariantCulture) + ")。");
            if (values[2] <= 0 || values[2] > maxAcceleration) throw new InvalidOperationException("测试加速度必须大于 0 且不超过控制器绝对最大加速度 (" + maxAcceleration.ToString("G6", CultureInfo.InvariantCulture) + ")。");
            if (values[3] <= 0 || values[3] > maxDac) throw new InvalidOperationException("最大 DAC 必须大于 0 且不超过 Motor[1].MaxDac (" + maxDac.ToString("G6", CultureInfo.InvariantCulture) + ")。");
            return values;
        }

        private void PauseClicked(object sender, EventArgs e)
        {
            try
            {
                EnsureConnected();
                int status = (int)ReadExpression("P701");
                if (status == 1)
                {
                    WriteP(700, 4);
                    pauseButton.Text = "恢复";
                }
                else if (status == 2)
                {
                    WriteP(700, 0);
                    pauseButton.Text = "暂停";
                }
                else throw new InvalidOperationException("只有运行中或已暂停时才能执行暂停/恢复。");
            }
            catch (Exception ex) { ShowError(ex.Message); }
        }

        private void StopClicked(object sender, EventArgs e)
        {
            try { EnsureConnected(); WriteP(700, 2); SetStatus("已发送停止请求"); }
            catch (Exception ex) { ShowError(ex.Message); }
        }

        private void ApplyCandidateClicked(object sender, EventArgs e)
        {
            try
            {
                EnsureConnected();
                int status = (int)ReadExpression("P701");
                if (status == 1 || status == 2 || status == 3) throw new InvalidOperationException("调谐运行或分析期间不能应用候选参数。");
                if (MessageBox.Show("应用 P740-P744 候选 PID 参数到 Motor[1].Servo？", "确认应用", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
                var candidate = ReadPBlock(740, 5);
                if (candidate[0] < 0.01 || candidate[0] > 50) throw new InvalidOperationException("候选参数无效，未检测到有效的 Kp 拟合结果。");
                WriteServoParameters(candidate);
                for (int i = 0; i < candidate.Length; i++) WriteP(720 + i, candidate[i]);
                ReadParameters();
                SetStatus("候选参数已应用");
            }
            catch (Exception ex) { ShowError(ex.Message); }
        }

        private void RestoreBackupClicked(object sender, EventArgs e)
        {
            try
            {
                EnsureConnected();
                int status = (int)ReadExpression("P701");
                if (status == 1 || status == 2 || status == 3) throw new InvalidOperationException("调谐运行或分析期间不能恢复参数。");
                if (MessageBox.Show("将 P730-P734 备份值恢复到伺服参数，继续？", "确认恢复", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
                var backup = ReadPBlock(730, 5);
                if (backup[0] <= 0) throw new InvalidOperationException("下位机没有有效的参数备份；至少需要完成一次调谐以生成备份。");
                WriteServoParameters(backup);
                for (int i = 0; i < backup.Length; i++) WriteP(720 + i, backup[i]);
                ReadParameters();
                SetStatus("备份参数已恢复");
            }
            catch (Exception ex) { ShowError(ex.Message); }
        }

        private void SaveFinalClicked(object sender, EventArgs e)
        {
            try
            {
                EnsureConnected();
                if (MessageBox.Show("PowerPMAC 将执行 save 命令，把当前控制器配置写入非易失存储。确认参数已验证并继续？", "确认保存", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
                SendCommand("save");
                SetStatus("已发送 save 命令");
            }
            catch (Exception ex) { ShowError(ex.Message); }
        }

        private void ReadSamplesClicked(object sender, EventArgs e)
        {
            try
            {
                EnsureConnected();
                var indices = ReadPBlock(760, 2);
                int count = Math.Max(0, Math.Min(256, (int)indices[1]));
                if (count == 0) throw new InvalidOperationException("下位机报告采样数为 0 (P761)。当前下位机程序尚未填充采样缓冲区。");
                double[] actual = ReadQChannel(0, count);
                double[] command = ReadQChannel(1, count);
                double[] dac = ReadQChannel(2, count);
                samples.Clear();
                for (int i = 0; i < count; i++) samples.Add(new TuneSample(i, command[i], actual[i], command[i] - actual[i], dac[i]));
                DrawSamples();
                SetStatus("已读取 " + count + " 个采样点");
            }
            catch (Exception ex) { ShowError(ex.Message); }
        }

        private double[] ReadQChannel(int channel, int count)
        {
            string response;
            string command = "Q" + (channel * 256).ToString(CultureInfo.InvariantCulture) + "," + count.ToString(CultureInfo.InvariantCulture) + "\r";
            if (ppCom.GetSyncGpasciiResponse(command, out response) != Status.Ok)
                throw new InvalidOperationException("读取 Q 缓冲区通道 " + channel + " 失败。");
            var values = new List<double>();
            foreach (Match match in Regex.Matches(response, @"(?im)\bq\d+\s*=\s*([-+]?(?:\d+\.?\d*|\.\d+)(?:[eE][-+]?\d+)?)"))
            {
                double value;
                if (double.TryParse(match.Groups[1].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out value)) values.Add(value);
            }
            if (values.Count == 0) values = ParseNumbers(response);
            if (values.Count < count) throw new InvalidOperationException("Q 缓冲区通道 " + channel + " 返回点数不足。");
            return values.Take(count).ToArray();
        }

        private void ExportCsvClicked(object sender, EventArgs e)
        {
            if (samples.Count == 0)
            {
                MessageBox.Show("请先读取有效采样数据。", "没有采样数据", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            using (var dialog = new SaveFileDialog { Filter = "CSV 文件 (*.csv)|*.csv", FileName = "PowerPMAC_Tune_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv" })
            {
                if (dialog.ShowDialog(this) != DialogResult.OK) return;
                try
                {
                    var csv = new StringBuilder();
                    csv.AppendLine("Timestamp," + DateTime.Now.ToString("o", CultureInfo.InvariantCulture));
                    csv.AppendLine("Kp,Kvfb,Kvff,Kaff,Ki,PeakErr,RMSerr,SSerr,Overshoot,SettleTime");
                    ReadParameters();
                    ReadPBlock(750, 5);
                    csv.AppendLine(string.Join(",", pidFields.Select(x => CsvNumber(ParseInput(x.Text))).Concat(Enumerable.Range(0, 5).Select(i => CsvNumber(GetP(750 + i))))));
                    csv.AppendLine("Index,CommandPosition,ActualPosition,FollowingError,DAC");
                    foreach (var sample in samples)
                        csv.AppendLine(string.Join(",", sample.Index, CsvNumber(sample.Command), CsvNumber(sample.Actual), CsvNumber(sample.Error), CsvNumber(sample.Dac)));
                    File.WriteAllText(dialog.FileName, csv.ToString(), new UTF8Encoding(true));
                    SetStatus("調諧記錄已导出: " + dialog.FileName);
                }
                catch (Exception ex) { ShowError(ex.Message); }
            }
        }

        private static double ParseInput(string text)
        {
            double value;
            return double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out value) ? value : 0;
        }

        private static string CsvNumber(double value) { return value.ToString("G17", CultureInfo.InvariantCulture); }

        private void PollTimer_Tick(object sender, EventArgs e)
        {
            if (polling || !connected) return;
            polling = true;
            try { PollController(); }
            catch (Exception ex)
            {
                SetConnected(false);
                pollTimer.Stop();
                ShowError("通信中断: " + ex.Message);
            }
            finally { polling = false; }
        }

        private void PollController()
        {
            ReadPBlock(700, 57);
            ReadParametersFromCache();
            int status = (int)GetP(701);
            int error = (int)GetP(702);
            stateValue.Text = StateText(status);
            errorValue.Text = error == 0 ? "--" : ErrorText(error);
            pauseButton.Text = status == 2 ? "恢复" : "暂停";
            for (int i = 0; i < metricValues.Length; i++)
                metricValues[i].Text = GetP(750 + i).ToString("G7", CultureInfo.InvariantCulture);
        }

        private void ReadParametersFromCache()
        {
            for (int i = 0; i < pidFields.Length; i++) pidFields[i].Text = GetP(720 + i).ToString("G10", CultureInfo.InvariantCulture);
        }

        private double[] ReadPBlock(int start, int count)
        {
            string response;
            if (ppCom.GetSyncGpasciiResponse("P" + start.ToString(CultureInfo.InvariantCulture) + "," + count.ToString(CultureInfo.InvariantCulture) + "\r", out response) != Status.Ok)
                throw new InvalidOperationException("读取 P" + start + " 参数区失败。");
            foreach (Match match in Regex.Matches(response, @"(?im)\bp(\d+)\s*=\s*([-+0-9.e]+)"))
            {
                int index;
                double value;
                if (int.TryParse(match.Groups[1].Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out index) &&
                    double.TryParse(match.Groups[2].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
                    pValues[index] = value;
            }
            var values = new double[count];
            for (int i = 0; i < count; i++)
            {
                if (!pValues.TryGetValue(start + i, out values[i]))
                {
                    if (count == 1)
                    {
                        values[i] = ReadExpression("P" + (start + i).ToString(CultureInfo.InvariantCulture));
                        pValues[start + i] = values[i];
                    }
                    else throw new InvalidOperationException("PowerPMAC 未返回 P" + (start + i) + "。");
                }
            }
            return values;
        }

        private double GetP(int index)
        {
            double value;
            if (!pValues.TryGetValue(index, out value)) throw new InvalidOperationException("缺少 P" + index + " 的读取值。");
            return value;
        }

        private void WriteP(int index, double value)
        {
            SendCommand("P" + index.ToString(CultureInfo.InvariantCulture) + "=" + value.ToString("G17", CultureInfo.InvariantCulture));
            pValues[index] = value;
        }

        private double ReadExpression(string expression)
        {
            string response;
            if (ppCom.GetSyncGpasciiResponse(expression + "\r", out response) != Status.Ok)
                throw new InvalidOperationException("读取 " + expression + " 失败。");
            var match = Regex.Match(response, @"([-+]?(?:\d+\.?\d*|\.\d+)(?:[eE][-+]?\d+)?)\s*(?:\x06)?\s*$");
            double value;
            if (!match.Success || !double.TryParse(match.Groups[1].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
            {
                match = Regex.Match(response, @"=\s*([-+]?(?:\d+\.?\d*|\.\d+)(?:[eE][-+]?\d+)?)");
                if (!match.Success || !double.TryParse(match.Groups[1].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
                    throw new InvalidOperationException("无法解析 " + expression + " 的响应: " + response.Trim());
            }
            return value;
        }

        private void SendCommand(string command)
        {
            string response;
            if (ppCom.GetSyncGpasciiResponse(command + "\r", out response) != Status.Ok)
                throw new InvalidOperationException("PowerPMAC 命令失败: " + command);
        }

        private void WriteServoParameters(double[] values)
        {
            if (values.Length != 5) throw new InvalidOperationException("伺服参数数量不正确。");
            string[] names = { "Kp", "Kvfb", "Kvff", "Kaff", "Ki" };
            for (int i = 0; i < names.Length; i++)
                SendCommand("Motor[1].Servo." + names[i] + "=" + values[i].ToString("G17", CultureInfo.InvariantCulture));
        }

        private static List<double> ParseNumbers(string response)
        {
            var values = new List<double>();
            foreach (Match match in Regex.Matches(response, @"[-+]?(?:\d+\.?\d*|\.\d+)(?:[eE][-+]?\d+)?"))
            {
                double value;
                if (double.TryParse(match.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out value)) values.Add(value);
            }
            return values;
        }

        private void DrawSamples()
        {
            ClearChart();
            foreach (var sample in samples)
            {
                sampleChart.Series["指令位置"].Points.AddXY(sample.Index, sample.Command);
                sampleChart.Series["实际位置"].Points.AddXY(sample.Index, sample.Actual);
                sampleChart.Series["跟随误差"].Points.AddXY(sample.Index, sample.Error);
                sampleChart.Series["DAC"].Points.AddXY(sample.Index, sample.Dac);
            }
        }

        private void ClearChart()
        {
            foreach (Series series in sampleChart.Series) series.Points.Clear();
        }

        private void EnsureConnected()
        {
            if (!connected || !ppCom.IsSyncGpasciiConnected()) throw new InvalidOperationException("请先连接 PowerPMAC。");
        }

        private void SetStatus(string text) { stateValue.Text = text; }
        private void ShowError(string text) { errorValue.Text = text; }

        private static string StateText(int status)
        {
            switch (status)
            {
                case 0: return "空闲";
                case 1: return "运行中";
                case 2: return "已暂停";
                case 3: return "分析中";
                case 4: return "完成";
                case 5: return "错误";
                default: return "未知状态 (" + status + ")";
            }
        }

        private static string ErrorText(int error)
        {
            switch (error)
            {
                case -1: return "用户取消";
                case 1: return "电机未使能";
                case 2: return "测试位移超范围";
                case 3: return "跟随误差超限";
                case 4: return "DAC 饱和";
                case 5: return "采样数据不足";
                case 6: return "拟合算法失败";
                case 7: return "运动程序未就绪";
                case 8: return "测试参数无效";
                default: return "错误码 " + error;
            }
        }

        private void AutoTuneForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            pollTimer.Stop();
            try { if (ppCom.IsSyncGpasciiConnected()) ppCom.SyncGpasciiDisconnect(); }
            catch { }
        }

        private sealed class TuneSample
        {
            public TuneSample(int index, double command, double actual, double error, double dac)
            {
                Index = index;
                Command = command;
                Actual = actual;
                Error = error;
                Dac = dac;
            }

            public int Index { get; private set; }
            public double Command { get; private set; }
            public double Actual { get; private set; }
            public double Error { get; private set; }
            public double Dac { get; private set; }
        }
    }
}