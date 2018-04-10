using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tsSleepBlock001
{
    public partial class Form1 : Form
    {
        private PerformanceCounter _pfcounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        private int _par = -1;
        public Form1()
        {
            InitializeComponent();
            timer1.Tick += CheckCpuPar;
            float cpu = _pfcounter.NextValue();
        }

        private void CheckCpuPar(object sender, EventArgs e)
        {
            var cpu = (int)_pfcounter.NextValue();
            if(cpu > _par)
            {
                SetThreadExecutionState(ExecutionState.SystemRequired);
            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkBox = (CheckBox)sender;

            if (chkBox.Checked)
            {
                var flg = true;
                int min = -1;
                int par = -1;
                if (flg && !int.TryParse(textBox1.Text, out min)) flg = false;
                if (flg && !int.TryParse(textBox2.Text, out par)) flg = false;
                if (flg && !((min > 0) && (min < 120))) flg = false;
                if (flg && !((par > 0) && (par < 101))) flg = false;
                if(flg == false)
                {
                    MessageBox.Show("値が変です");
                    chkBox.Checked = false;
                    return;
                }
                _par = par;
                timer1.Interval = min * 60 * 1000;
                timer1.Start();
                chkBox.Text = "STOP";
            }
            else
            {
                timer1.Stop();
                chkBox.Text = "START";
            }
        }
        [DllImport("kernel32.dll")]
        extern static ExecutionState SetThreadExecutionState(ExecutionState esFlags);
        [FlagsAttribute]
        public enum ExecutionState : uint
        {
            // 関数が失敗した時の戻り値
            Null = 0,
            // スタンバイを抑止
            SystemRequired = 1,
            // 画面OFFを抑止
            DisplayRequired = 2,
            // 効果を永続させる。ほかオプションと併用する。
            Continuous = 0x80000000,
        }
    }
}
