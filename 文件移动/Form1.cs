using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 文件移动
{
    public partial class Form1 : Form
    {
        string srcPath;
        string objPath;
        public Form1()
        {
            InitializeComponent();
        }

        //选择源文件夹
        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if(folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog.SelectedPath;
            }
            folderBrowserDialog.Dispose();

        }

        //选择目标文件夹
        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = folderBrowserDialog.SelectedPath;
            }
            folderBrowserDialog.Dispose();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            srcPath = textBox1.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            objPath = textBox2.Text;
        }

        //开始转移
        private void button1_Click(object sender, EventArgs e)
        {
            CopyFile(srcPath, objPath, false);
        }

        //true为将原来的文件夹包括子文件夹和子文件复制到新文件夹
        //false 为将选定文件夹的所有子文件，和子文件夹的子文件全部复制到另一个文件夹
        private static void CopyFile(string sourcePath, string destPath, bool b)
        {
            if(Directory.Exists(sourcePath))
            {
                if(!Directory.Exists(destPath))
                {
                    try
                    {
                        Directory.CreateDirectory(destPath);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("创建目标目录失败：" + ex.Message);
                    }
                }
                List<string> filesList = new List<string>(Directory.GetFiles(sourcePath));
                filesList.ForEach(c =>
                {
                    string destFile = Path.Combine(new string[] { destPath, Path.GetFileName(c) });
                    File.Copy(c, destFile, true);
                });
                List<string> folders = new List<string>(Directory.GetDirectories(sourcePath));
                folders.ForEach(f =>
                {
                    if (b)
                    {
                        string destDir = Path.Combine(new string[] { destPath, Path.GetFileName(f) });
                        CopyFile(f, destDir, b);
                    }
                    else
                    {
                        CopyFile(f, destPath, b);
                    }
                });
            }
            else
            {
                throw new DirectoryNotFoundException("源目录不存在！");
            }
        }

        //检查两个目录是否选择完成
        private void timer1_Tick(object sender, EventArgs e)
        {
            if(textBox1.Text != "" && textBox2.Text != "")
            {
                button1.Enabled = true;
                button4.Enabled = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            CopyFile(srcPath, objPath, true);
        }
    }
}
