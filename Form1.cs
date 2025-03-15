using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using MetroSet_UI.Controls;
using System.IO;
using SharpCompress.Archives;
using SharpCompress.Archives.Rar;
using SharpCompress.Common;
using SharpCompress.Readers;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Threading;
using System.Linq.Expressions;

namespace Netro
{
    public partial class Form1 : MetroSet_UI.Forms.MetroSetForm
    {
        private static readonly Form2 form21 = new Form2();
#pragma warning disable IDE0044
        private Form2 form2 = form21;
#pragma warning restore IDE0044

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private const int WM_VSCROLL = 0x115;
        private const int SB_LINEUP = 0;
        private const int SB_LINEDOWN = 1;

        private HashSet<string> errorFiles = new HashSet<string>();

        public Form1()
        {
            InitializeComponent();
            this.StyleManager = styleManager1;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Text = "";
            this.BackColor = Color.White;
            this.Padding = new Padding(2);
            this.StartPosition = FormStartPosition.CenterScreen;

            //---------------------------------------

            FlowLayoutPanel panel = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                FlowDirection = FlowDirection.RightToLeft,
                AutoSize = true,
                WrapContents = false
            };

            //---------------------------------------

            Panel ustPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 40,
                BackColor = Color.FromArgb(30, 30, 30)
            };
            this.Controls.Add(ustPanel);

            Label titleLabel = new Label
            {
                Text = "RLog",
                ForeColor = Color.White,
                Font = new Font("Arial", 12, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(10, 10),
                TextAlign = ContentAlignment.MiddleLeft
            };
            ustPanel.Controls.Add(titleLabel);

            Button closeButton = new Button
            {
                Text = "✕",
                ForeColor = Color.White,
                BackColor = Color.FromArgb(220, 53, 69),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(40, 30),
                Location = new Point(this.Width - 50, 5),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            closeButton.FlatAppearance.BorderSize = 0;
            closeButton.Click += (s, args) => this.Close();
            ustPanel.Controls.Add(closeButton);

            Button yardimTusu = new Button
            {
                Text = "?",
                ForeColor = Color.White,
                BackColor = Color.FromArgb(209, 165, 9),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(40, 30),
                Location = new Point(this.Width - 150, 5),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            yardimTusu.FlatAppearance.BorderSize = 0;
            yardimTusu.Click += (s, args) => form2.ShowDialog();
            ustPanel.Controls.Add(yardimTusu);

            Button kucultmeTusu = new Button
            {
                Text = "—",
                ForeColor = Color.White,
                BackColor = Color.FromArgb(108, 117, 125),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(40, 30),
                Location = new Point(this.Width - 100, 5),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            kucultmeTusu.FlatAppearance.BorderSize = 0;
            kucultmeTusu.Click += (s, args) => this.WindowState = FormWindowState.Minimized;
            ustPanel.Controls.Add(kucultmeTusu);

            ustPanel.MouseDown += TopPanel_MouseDown;
            ustPanel.MouseMove += TopPanel_MouseMove;

            textBox1.MouseWheel += TextBox1_MouseWheel;


            this.Resize += (s, args) =>
            {
                closeButton.Location = new Point(this.Width - 50, 5);
                yardimTusu.Location = new Point(this.Width - 150, 5);
                kucultmeTusu.Location = new Point(this.Width - 100, 5);
            };

            listBox1.ContextMenuStrip = contextMenuStrip1;

            metroSetButton1.NormalColor = Color.FromArgb(68, 71, 210);
            metroSetButton1.HoverColor = Color.FromArgb(78, 81, 230);
            metroSetButton1.PressColor = Color.FromArgb(68, 71, 203);
            metroSetButton1.DisabledBackColor = Color.FromArgb(88, 89, 130);
            metroSetButton1.DisabledForeColor = Color.FromArgb(114, 121, 128);

            metroSetButton1.NormalBorderColor = Color.Black;
            metroSetButton1.HoverBorderColor = Color.Black;
            metroSetButton1.PressBorderColor = Color.Black;

            metroSetButton2.NormalColor = Color.FromArgb(68, 71, 210);
            metroSetButton2.HoverColor = Color.FromArgb(78, 81, 230);
            metroSetButton2.PressColor = Color.FromArgb(68, 71, 203);
            metroSetButton2.DisabledBackColor = Color.FromArgb(88, 89, 130);
            metroSetButton2.DisabledForeColor = Color.FromArgb(114, 121, 128);

            metroSetButton2.NormalBorderColor = Color.Black;
            metroSetButton2.HoverBorderColor = Color.Black;
            metroSetButton2.PressBorderColor = Color.Black;
        }
        private Point mouseDownLocation;
        protected override void WndProc(ref Message m)
        {
            const int WM_NCLBUTTONDBLCLK = 0x00A3;
            const int WM_LBUTTONDBLCLK = 0x0203;

            if (m.Msg == WM_NCLBUTTONDBLCLK || m.Msg == WM_LBUTTONDBLCLK)
                return;

            base.WndProc(ref m);
        }
        private void TopPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                mouseDownLocation = e.Location;
        }
        private void TextBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                SendMessage(textBox1.Handle, WM_VSCROLL, SB_LINEUP, 0);
            }
            else
            {
                SendMessage(textBox1.Handle, WM_VSCROLL, SB_LINEDOWN, 0);
            }
        }
        private void TopPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - mouseDownLocation.X;
                this.Top += e.Y - mouseDownLocation.Y;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            listBox1.DrawMode = DrawMode.OwnerDrawFixed;
            listBox1.DrawItem += new DrawItemEventHandler(listBox1_DrawItem);
            listBox1.SelectionMode = SelectionMode.MultiExtended;
            listBox1.KeyUp += listBox1_KeyUp;
            this.Text = "RLog - Coded by Radox";
        }
#pragma warning disable IDE1006
        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
#pragma warning restore IDE1006
        {
            if (e.Index < 0) return;

            e.DrawBackground();
            string itemText = listBox1.Items[e.Index].ToString();

            string[] parts = itemText.Split(new string[] { "-->" }, StringSplitOptions.None);
            if (parts.Length < 2) return;

            string fileName = parts[0].Trim();
            string filePath = parts[1].Trim();

            Font font = new Font("Tahoma", 10, FontStyle.Bold);

            Brush fileNameBrush = errorFiles.Contains(filePath) ? Brushes.Red : Brushes.LavenderBlush;
            Brush arrowBrush = Brushes.LightSteelBlue;
            Brush filePathBrush = errorFiles.Contains(filePath) ? Brushes.Red : Brushes.LemonChiffon;

            SizeF fileNameSize = e.Graphics.MeasureString(fileName, font);
            SizeF arrowSize = e.Graphics.MeasureString("-->", font);

            int startX = e.Bounds.Left + 5;
            int arrowX = startX + (int)fileNameSize.Width + 10;
            int filePathX = arrowX + (int)arrowSize.Width + 10;

            e.Graphics.DrawString(fileName, font, fileNameBrush, startX, e.Bounds.Top);
            e.Graphics.DrawString("-->", font, arrowBrush, arrowX, e.Bounds.Top);
            e.Graphics.DrawString(filePath, font, filePathBrush, filePathX, e.Bounds.Top);

            e.DrawFocusRectangle();
        }
#pragma warning disable IDE1006
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
#pragma warning restore IDE1006
        {
            while (listBox1.SelectedItems.Count > 0)
            {
                listBox1.Items.Remove(listBox1.SelectedItem);
            }
        }
        private async void metroSetButton1_Click(object sender, EventArgs e)
        {
            metroSetButton1.Enabled = false;
            metroSetButton2.Enabled = false;

            metroSetButton1.NormalTextColor = Color.FromArgb(114, 121, 128);
            metroSetButton1.NormalColor = Color.FromArgb(88, 89, 130);
            metroSetButton1.DisabledBackColor = Color.FromArgb(88, 89, 130);
            metroSetButton1.DisabledForeColor = Color.FromArgb(114, 121, 128);

            label2.Text = "Progress...";
            progressBar1.Value = 5;
            progressBar1.Visible = true;

            listBox1.ContextMenuStrip = null;
            bool isSuccess = false;

            try
            {
                if (listBox1.Items.Count == 0 || string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    MessageBox.Show("\r\nPlease select file first and enter word(s) for filtering.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    progressBar1.Value = 0;
                    label2.Text = "Error: File or filter word missing!";

                    metroSetButton1.NormalColor = Color.FromArgb(200, 50, 50);
                    metroSetButton1.HoverColor = Color.FromArgb(220, 60, 60);
                    metroSetButton1.PressColor = Color.FromArgb(180, 40, 40);
                    metroSetButton1.NormalTextColor = Color.White;
                    metroSetButton1.DisabledBackColor = Color.FromArgb(88, 89, 130);
                    metroSetButton1.DisabledForeColor = Color.FromArgb(114, 121, 128);

                    metroSetButton2.NormalColor = Color.FromArgb(200, 50, 50);
                    metroSetButton2.HoverColor = Color.FromArgb(220, 60, 60);
                    metroSetButton2.PressColor = Color.FromArgb(180, 40, 40);
                    metroSetButton2.NormalTextColor = Color.White;
                    metroSetButton2.DisabledBackColor = Color.FromArgb(88, 89, 130);
                    metroSetButton2.DisabledForeColor = Color.FromArgb(114, 121, 128);

                    metroSetButton1.Enabled = true;
                    metroSetButton2.Enabled = true;

                    return;
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Text File|*.txt",
                    FileName = "RLog.txt"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(saveFileDialog.FileName))
                {
                    string savePath = saveFileDialog.FileName;
                    List<string> keywords = textBox1.Lines
                        .SelectMany(line => Regex.Split(line, @"[\s,;]+"))
                        .Where(k => !string.IsNullOrWhiteSpace(k))
                        .ToList();

                    List<string> matchedLines = new List<string>();
                    int totalFiles = listBox1.Items.Count;
                    int processedFiles = 0;

                    await Task.Run(() =>
                    {
                        List<object> itemsToRemove = new List<object>();
                        List<object> itemsCopy = new List<object>();

                        this.Invoke(new Action(() =>
                        {
                            itemsCopy = listBox1.Items.Cast<object>().ToList();
                        }));

                        foreach (var item in itemsCopy)

                        {
                            try
                            {
                                string filePath = item.ToString().Split(new string[] { "-->" }, StringSplitOptions.None).Last().Trim();
                                string extension = Path.GetExtension(filePath).ToLower();

                                if (extension == ".txt" || extension == ".json" || extension == ".log" || extension == ".xml" || extension == ".csv")
                                {
                                    ProcessTextFile(filePath, keywords, matchedLines);
                                }
                                else if (extension == ".zip")
                                {
                                    ExtractAndProcessZip(filePath, keywords, matchedLines);
                                }
                                else if (extension == ".rar")
                                {
                                    ExtractAndProcessRar(filePath, keywords, matchedLines, listBox1);
                                }
                                else if (extension == ".7z" || extension == ".tar.gz")
                                {
                                    ExtractAndProcess7zOrTarGz(filePath, keywords, matchedLines);
                                }
                                if (metroSetCheckBox1.Checked)
                                {
                                    try
                                    {
                                        File.Delete(filePath);
                                        itemsToRemove.Add(item);
                                    }
                                    catch (Exception ex)
                                    {
                                        matchedLines.Add($"Could not delete file: {filePath} - Error: {ex.Message}");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                matchedLines.Add($"An Unexpected Error Occurred: {ex.Message}");
                            }

                            Interlocked.Increment(ref processedFiles);
                            int progressPercentage = (totalFiles > 0) ? (processedFiles * 100) / totalFiles : 100;

                            this.Invoke(new Action(() =>
                            {
                                progressBar1.Value = progressPercentage;
                                label2.Text = $"Processing... ({processedFiles}/{totalFiles})";
                                metroSetButton1.NormalTextColor = Color.FromArgb(114, 121, 128);
                                metroSetButton1.NormalColor = Color.FromArgb(88, 89, 130);
                                metroSetButton1.DisabledBackColor = Color.FromArgb(88, 89, 130);
                                metroSetButton1.DisabledForeColor = Color.FromArgb(114, 121, 128);
                                metroSetButton1.Enabled = false;
                                metroSetButton2.Enabled = false;
                            }));
                        }

                        this.Invoke(new Action(() =>
                        {
                            foreach (var item in itemsToRemove)
                            {
                                listBox1.Items.Remove(item);
                            }
                        }));
                    });
                    File.WriteAllLines(savePath, matchedLines);
                    MessageBox.Show($"The filtered data was saved as {savePath}", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                progressBar1.Value = 100;
                progressBar1.ForeColor = Color.Red;
                label2.Text = "Error: An unexpected error occurred during the operation!";

                metroSetButton1.NormalTextColor = Color.FromArgb(114, 121, 128);
                metroSetButton1.NormalColor = Color.FromArgb(88, 89, 130);
                metroSetButton1.DisabledBackColor = Color.FromArgb(88, 89, 130);
                metroSetButton1.DisabledForeColor = Color.FromArgb(114, 121, 128);
            }
            finally
            {
                listBox1.ContextMenuStrip = contextMenuStrip1;
                metroSetButton1.Enabled = true;
                metroSetButton2.Enabled = true;
                progressBar1.Value = 0;
                progressBar1.Visible = true;

                if (isSuccess)
                {
                    label2.Text = "Done!";
                    progressBar1.Value = 0;
                    listBox1.ContextMenuStrip = contextMenuStrip1;

                    metroSetButton1.NormalColor = Color.FromArgb(68, 71, 210);
                    metroSetButton1.HoverColor = Color.FromArgb(78, 81, 230);
                    metroSetButton1.PressColor = Color.FromArgb(68, 71, 203);
                    metroSetButton1.NormalTextColor = Color.White;
                    metroSetButton1.DisabledBackColor = Color.FromArgb(88, 89, 130);
                    metroSetButton1.DisabledForeColor = Color.FromArgb(114, 121, 128);

                    metroSetButton2.NormalColor = Color.FromArgb(68, 71, 210);
                    metroSetButton2.HoverColor = Color.FromArgb(78, 81, 230);
                    metroSetButton2.PressColor = Color.FromArgb(68, 71, 203);
                    metroSetButton2.NormalTextColor = Color.White;
                    metroSetButton2.DisabledBackColor = Color.FromArgb(88, 89, 130);
                    metroSetButton2.DisabledForeColor = Color.FromArgb(114, 121, 128);
                }
            }
        }
        private void ExtractAndProcess7zOrTarGz(string archivePath, List<string> keywords, List<string> matchedLines)
        {
            using (var archive = SharpCompress.Archives.SevenZip.SevenZipArchive.Open(archivePath))
            {
                foreach (var entry in archive.Entries.Where(e => !e.IsDirectory &&
                                                                 (e.Key.EndsWith(".txt") || e.Key.EndsWith(".json") ||
                                                                  e.Key.EndsWith(".log") || e.Key.EndsWith(".xml") ||
                                                                  e.Key.EndsWith(".csv"))))
                {
                    using (var stream = entry.OpenEntryStream())
                    using (var reader = new StreamReader(stream))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (keywords.Any(k => line.IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0))
                            {
                                matchedLines.Add(line.Trim());
                            }
                        }
                    }
                }
            }
        }
        private void ProcessTextFile(string filePath, List<string> keywords, List<string> matchedLines)
        {
            string extension = Path.GetExtension(filePath).ToLower();

            if (extension == ".xml")
            {
                ProcessXmlFile(filePath, keywords, matchedLines);
                return;
            }

            foreach (var line in File.ReadLines(filePath))
            {
                if (keywords.Any(k => line.IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0))
                {
                    matchedLines.Add(line);
                }
            }
        }
        private void ProcessXmlFile(string filePath, List<string> keywords, List<string> matchedLines)
        {
            var doc = new System.Xml.XmlDocument();
            doc.Load(filePath);

            foreach (System.Xml.XmlNode node in doc.DocumentElement.ChildNodes)
            {
                string nodeText = node.InnerText;

                if (keywords.Any(k => nodeText.IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0))
                {
                    matchedLines.Add(nodeText);
                }
            }
        }
        private void ExtractAndProcessZip(string zipPath, List<string> keywords, List<string> matchedLines)
        {
            using (ZipArchive archive = ZipFile.OpenRead(zipPath))
            {
                foreach (var entry in archive.Entries)
                {
                    if (entry.Name.EndsWith(".txt") || entry.Name.EndsWith(".json"))
                    {
                        using (StreamReader reader = new StreamReader(entry.Open()))
                        {
                            string content = reader.ReadToEnd();
                            matchedLines.AddRange(content.Split('\n').Where(line => keywords.Any(k => line.IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0)));
                        }
                    }
                }
            }
        }
        private void ExtractAndProcessRar(string rarPath, List<string> keywords, List<string> matchedLines, ListBox listBox)
        {
            try
            {
                using (var archive = RarArchive.Open(rarPath))
                {
                    foreach (var entry in archive.Entries.Where(e => !e.IsDirectory &&
                                                                     (e.Key.EndsWith(".txt") ||
                                                                      e.Key.EndsWith(".json") ||
                                                                      e.Key.EndsWith(".log") ||
                                                                      e.Key.EndsWith(".xml") ||
                                                                      e.Key.EndsWith(".csv"))))
                    {
                        using (var stream = entry.OpenEntryStream())
                        using (var reader = new StreamReader(stream))
                        {
                            string line;
                            while ((line = reader.ReadLine()) != null)
                            {
                                if (keywords.Any(k => line.IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0))
                                {
                                    matchedLines.Add(line.Trim());
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                errorFiles.Add(rarPath);
                listBox.Invoke((MethodInvoker)delegate
                {
                    listBox.Invalidate();
                });
            }
        }
        private void listBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                listBox1.BeginUpdate();

                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    listBox1.SetSelected(i, true);
                }
                listBox1.EndUpdate();
                e.Handled = true;
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void metroSetButton2_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Supported Files|*.zip;*.rar;*.7z;*.tar.gz;*.txt;*.json;*.log;*.xml;*.csv|All Files|*.*"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var selectedFiles = openFileDialog.FileNames;
                foreach (var file in selectedFiles)
                {
                    string extension = Path.GetExtension(file).ToLower();
                    if (extension == ".zip" || extension == ".rar" || extension == ".7z" || extension == ".tar.gz" ||
    extension == ".txt" || extension == ".json" || extension == ".log" || extension == ".xml" || extension == ".csv")

                    {
                        string fileName = Path.GetFileName(file);
                        string combinedInfo = $"{fileName} --> {file}";

                        if (!listBox1.Items.Cast<string>().Any(item => item.Contains(file)))
                        {
                            listBox1.Items.Add(combinedInfo);
                        }
                    }
                }
            }

        }
        private void metroSetCheckBox1_CheckedChanged(object sender)
        {

        }
    }
}