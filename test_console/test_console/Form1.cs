using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test_console
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);


        string[] aar = null;
        public Form1()
        {
            InitializeComponent();
            LoadingMenu();

        }



        public void LoadingMenu()
        {
            string key1 = ConfigurationManager.AppSettings["key1"];
            aar = key1.Split(',');
            foreach (var item in aar)
            {
                ToolStripMenuItem menu_item = new ToolStripMenuItem();
                menu_item.Name = item;
                menu_item.Text = "显示 " + item;
                contextMenuStrip1.Items.Add(menu_item);
                WindowHide(item, 0);//初始化全部隐藏
            }
            for (int i = 0; i < 2; i++)
            {
                ToolStripMenuItem menu_item = new ToolStripMenuItem();
                menu_item.Name = i == 0 ? "全部显示" : "退出";
                menu_item.Text = i == 0 ? "全部显示" : "退出";
                contextMenuStrip1.Items.Add(menu_item);
            }

        }

        public static void WindowHide(string consoleTitle, int lpWindowName)
        {
            try
            {
                IntPtr a = FindWindow(null, consoleTitle);
                if (a != IntPtr.Zero)
                {
                    var _a = ShowWindow(a, (uint)lpWindowName);//隐藏窗⼝
                }
                else
                {
                    MessageBox.Show($"{consoleTitle}未启用");
                }
            }
            catch (Exception)
            {
                MessageBox.Show($"操作失败");
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            WindowHide("pltcyer", 0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            WindowHide("pltcyer", 5);
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                //还原窗体显示
                WindowState = FormWindowState.Normal;
                //激活窗体并给予它焦点
                this.Activate();
                //任务栏区显示图标
                this.ShowInTaskbar = true;
                //托盘区图标隐藏
                notifyIcon1.Visible = false;
            }
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            //判断是否选择的是最小化按钮
            if (WindowState == FormWindowState.Minimized)
            {
                //隐藏任务栏区图标
                this.ShowInTaskbar = false;
                //图标显示在托盘区
                notifyIcon1.Visible = true;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("是否确认退出程序？确认退出后，全部隐藏窗口将显示！", "退出", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                foreach (var item in aar)
                {
                    WindowHide(item, 5);
                }
                // 关闭所有的线程
                System.Environment.Exit(0);
            }
        }


        private void contextMenuStrip1_MouseClick(object sender, MouseEventArgs e)
        {
            foreach (ToolStripItem items in contextMenuStrip1.Items)
            {
                if (items.Selected == true)
                {
                    string name = items.Text;
                    if (name.Contains("全部"))
                    {
                        foreach (var item in aar)
                        {
                            if (name == "全部显示")
                            {
                                WindowHide(item, 5);
                            }
                            else
                            {
                                WindowHide(item, 0);
                            }
                        }
                        items.Text = items.Text == "全部显示" ? "全部隐藏" : "全部显示";
                    }
                    else if (name == "退出")
                    {
                        Form1_FormClosing(sender, null);
                    }
                    else
                    {
                        string _name = items.Name;
                        if (name.Contains("显示"))
                        {
                            WindowHide(_name, 5);
                        }
                        else
                        {
                            WindowHide(_name, 0);
                        }
                        items.Text = items.Text.Contains("显示") ? $"隐藏 {_name} " : $"显示 {_name}";
                    }
                }
            }
        }
    }
}
