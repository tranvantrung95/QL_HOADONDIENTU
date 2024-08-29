using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HoaDonDienTu
{
//    public class InputBox
//    {
//        public static string Show(string prompt)
//        {
//            Form form = new Form();
//            Label label = new Label();
//            TextBox textBox = new TextBox();
//            Button buttonOk = new Button();
//            Button buttonCancel = new Button();

//            form.Text = "Enter Filter Value";
//            label.Text = prompt;
//            buttonOk.Text = "OK";
//            buttonCancel.Text = "Cancel";
//            buttonOk.DialogResult = DialogResult.OK;
//            buttonCancel.DialogResult = DialogResult.Cancel;

//            label.SetBounds(9, 20, 372, 13);
//            textBox.SetBounds(12, 36, 372, 20);
//            buttonOk.SetBounds(228, 72, 75, 23);
//            buttonCancel.SetBounds(309, 72, 75, 23);

//            label.AutoSize = true;
//            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
//            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
//            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

//            form.ClientSize = new System.Drawing.Size(396, 107);
//            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
//            form.ClientSize = new System.Drawing.Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
//            form.FormBorderStyle = FormBorderStyle.FixedDialog;
//            form.StartPosition = FormStartPosition.CenterScreen;
//            form.MinimizeBox = false;
//            form.MaximizeBox = false;
//            form.AcceptButton = buttonOk;
//            form.CancelButton = buttonCancel;

//            DialogResult dialogResult = form.ShowDialog();
//            if (dialogResult == DialogResult.OK)
//            {
//                return textBox.Text;
//            }
//            else
//            {
//                return null;
//            }
//        }

//}
    public partial class InputBox : Form
    {
        public event Action<string> ApplyFilter;

        private System.Windows.Forms.Label label;
        private TextBox textBox;
        private Button okButton;
        private Button cancelButton;


        public InputBox()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            label = new System.Windows.Forms.Label();
            label.Text = "OK";
            label.AutoSize = true;
            label.Location = new Point(10, 10);

            textBox = new TextBox();
            textBox.Location = new Point(10, 30);
            textBox.Size = new Size(250, 20);

            okButton = new Button();
            okButton.Text = "OK";
            okButton.DialogResult = DialogResult.OK;
            okButton.Location = new Point(60, 60);
            okButton.Size = new Size(75, 23);
            okButton.Click += OkButton_Click;

            cancelButton = new Button();
            cancelButton.Text = "Cancel";
            cancelButton.DialogResult = DialogResult.Cancel;
            cancelButton.Location = new Point(140, 60);
            cancelButton.Size = new Size(75, 23);
            cancelButton.Click += CancelButton_Click;

            Controls.Add(label);
            Controls.Add(textBox);
            Controls.Add(okButton);
            Controls.Add(cancelButton);

            Text = "Enter Filter Value";
            ClientSize = new Size(270, 110);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterScreen;
            MaximizeBox = false;
            MinimizeBox = false;
            AcceptButton = okButton;
            CancelButton = cancelButton;
        }

        private  void OkButton_Click(object sender, EventArgs e)
        {
            ApplyFilter?.Invoke(textBox.Text);
            Close();
        }

        private  void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        //public void Show(string prompt)
        //{
        //             Form form = new Form();
        // Label label = new Label();
        // TextBox textBox = new TextBox();
        // Button buttonOk = new Button();
        // Button buttonCancel = new Button();

        //form.Text = "Enter Filter Value";
        //    label.Text = prompt;
        //    buttonOk.Text = "OK";
        //    buttonCancel.Text = "Cancel";
        //    buttonOk.DialogResult = DialogResult.OK;
        //    buttonCancel.DialogResult = DialogResult.Cancel;

        //    label.SetBounds(9, 20, 372, 13);
        //    textBox.SetBounds(12, 36, 372, 20);
        //    buttonOk.SetBounds(228, 72, 75, 23);
        //    buttonCancel.SetBounds(309, 72, 75, 23);

        //    label.AutoSize = true;
        //    textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
        //    buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        //    buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

        //    form.ClientSize = new System.Drawing.Size(396, 107);
        //    form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
        //    form.ClientSize = new System.Drawing.Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
        //    form.FormBorderStyle = FormBorderStyle.FixedDialog;
        //    form.StartPosition = FormStartPosition.CenterScreen;
        //    form.MinimizeBox = false;
        //    form.MaximizeBox = false;
        //    form.AcceptButton = buttonOk;
        //    form.CancelButton = buttonCancel;

        //    DialogResult dialogResult = form.ShowDialog();
        //    if (dialogResult == DialogResult.OK)
        //    {
        //        buttonOk.Click += OkButton_Click;
        //    }
        //    else
        //    {
        //        Close();
        //    }
        //}
    }
}
