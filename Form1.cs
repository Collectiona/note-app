using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Drawing.Text;
namespace WinFormsApp4
{
    public partial class Form1 : Form
    {
        private string currentFile = "";
        private bool isNewDocument = true;

        public Form1()
        {
            InitializeComponent();
            LoadFonts();
            LoadFontSizes();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            rtbContent.Font = new Font("Tahoma", 14);
            cmbFont.Text = "Tahoma";
            cmbSize.Text = "14";
            UpdateWordCount();
        }

        private void LoadFonts()
        {
            using (InstalledFontCollection fontCollection = new InstalledFontCollection())
            {
                foreach (FontFamily font in fontCollection.Families)
                {
                    cmbFont.Items.Add(font.Name);
                }
            }
        }

        private void LoadFontSizes()
        {
            int[] sizes = { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 32, 36, 40, 44, 48, 54, 60, 66, 72 };
            foreach (int size in sizes)
            {
                cmbSize.Items.Add(size.ToString());
            }
        }

        private void hệThốngToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            mnuNew_Click(sender, e);
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void mnuNew_Click(object sender, EventArgs e)
        {
            rtbContent.Clear();

            currentFile = "";
            isNewDocument = true;

            rtbContent.Font = new Font("Tahoma", 14);

            cmbFont.Text = "Tahoma";
            cmbSize.Text = "14";

            btnBold.Checked = false;
            btnItalic.Checked = false;
            btnUnderline.Checked = false;

            UpdateWordCount();
        }

        private void mnuOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDlg = new OpenFileDialog();

            openDlg.Filter =
                "Rich Text Format (*.rtf)|*.rtf|" +
                "Text files (*.txt)|*.txt";

            if (openDlg.ShowDialog() == DialogResult.OK)
            {
                currentFile = openDlg.FileName;
                isNewDocument = false;

                string extension = Path.GetExtension(currentFile).ToLower();

                if (extension == ".rtf")
                {
                    rtbContent.LoadFile(currentFile, RichTextBoxStreamType.RichText);
                }
                else
                {
                    rtbContent.LoadFile(currentFile, RichTextBoxStreamType.PlainText);
                }

                UpdateWordCount();
            }
        }

        private void mnuSave_Click(object sender, EventArgs e)
        {
            SaveDocument();
        }
        private void SaveDocument()
        {
            if (isNewDocument || string.IsNullOrEmpty(currentFile))
            {
                SaveFileDialog saveDlg = new SaveFileDialog();

                saveDlg.Filter =
                    "Rich Text Format (*.rtf)|*.rtf";

                saveDlg.DefaultExt = "rtf";

                if (saveDlg.ShowDialog() == DialogResult.OK)
                {
                    currentFile = saveDlg.FileName;

                    rtbContent.SaveFile(
                        currentFile,
                        RichTextBoxStreamType.RichText
                    );

                    isNewDocument = false;

                    MessageBox.Show(
                        "Lưu văn bản thành công!",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
            else
            {
                string extension = Path.GetExtension(currentFile).ToLower();

                if (extension == ".rtf")
                {
                    rtbContent.SaveFile(
                        currentFile,
                        RichTextBoxStreamType.RichText
                    );
                }
                else
                {
                    rtbContent.SaveFile(
                        currentFile,
                        RichTextBoxStreamType.PlainText
                    );
                }

                MessageBox.Show(
                    "Lưu văn bản thành công!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveDocument();
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void cmbFont_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFont.SelectedItem == null)
                return;

            string fontName = cmbFont.SelectedItem.ToString();

            if (rtbContent.SelectionLength > 0)
            {
                Font oldFont = rtbContent.SelectionFont;

                if (oldFont != null)
                {
                    rtbContent.SelectionFont =
                        new Font(fontName, oldFont.Size, oldFont.Style);
                }
                else
                {
                    rtbContent.SelectionFont =
                        new Font(fontName, 14);
                }
            }
            else
            {
                Font oldFont = rtbContent.Font;

                rtbContent.Font =
                    new Font(fontName, oldFont.Size, oldFont.Style);
            }
        }

        private void cmbFont_Click(object sender, EventArgs e)
        {
            cmbFont_SelectedIndexChanged(sender, e);
        }

        private void cmbSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSize.SelectedItem == null)
                return;

            float size = float.Parse(cmbSize.SelectedItem.ToString());

            if (rtbContent.SelectionLength > 0)
            {
                Font oldFont = rtbContent.SelectionFont;

                if (oldFont != null)
                {
                    rtbContent.SelectionFont =
                        new Font(oldFont.FontFamily, size, oldFont.Style);
                }
                else
                {
                    rtbContent.SelectionFont =
                        new Font("Tahoma", size);
                }
            }
            else
            {
                Font oldFont = rtbContent.Font;

                rtbContent.Font =
                    new Font(oldFont.FontFamily, size, oldFont.Style);
            }
        }

        private void cmbSize_Click(object sender, EventArgs e)
        {
            cmbSize_SelectedIndexChanged(sender, e);
        }

        private void btnBold_Click(object sender, EventArgs e)
        {
            ApplyFontStyle(FontStyle.Bold, btnBold.Checked);
        }
        private void ApplyFontStyle(FontStyle style, bool enabled)
        {
            if (rtbContent.SelectionLength == 0)
                return;

            Font currentFont = rtbContent.SelectionFont;

            if (currentFont == null)
                return;

            FontStyle newStyle;

            if (enabled)
            {
                newStyle = currentFont.Style | style;
            }
            else
            {
                newStyle = currentFont.Style & ~style;
            }

            rtbContent.SelectionFont =
                new Font(
                    currentFont.FontFamily,
                    currentFont.Size,
                    newStyle
                );
        }

        private void btnItalic_Click(object sender, EventArgs e)
        {
            ApplyFontStyle(FontStyle.Italic, btnItalic.Checked);
        }

        private void btnUnderline_Click(object sender, EventArgs e)
        {
            ApplyFontStyle(FontStyle.Underline, btnUnderline.Checked);
        }

        private void lblWordCount_Click(object sender, EventArgs e)
        {
            UpdateWordCount();
        }

        private void UpdateWordCount()
        {
            string text = rtbContent.Text.Trim();

            if (string.IsNullOrEmpty(text))
            {
                lblWordCount.Text = "Tổng số từ: 0";
                return;
            }

            string[] words = Regex.Split(text, @"\s+");

            lblWordCount.Text = "Tổng số từ: " + words.Length;
        }
    }
}
