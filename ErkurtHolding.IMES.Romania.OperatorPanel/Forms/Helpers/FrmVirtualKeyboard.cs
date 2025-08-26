using ErkurtHolding.IMES.Romania.OperatorPanel.Localization;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Forms.Helpers
{
    public partial class FrmVirtualKeyboard : DevExpress.XtraEditors.XtraForm
    {
        public string InputText
        {
            get
            {
                return memoEdit1.Text;
            }
        }

        public FrmVirtualKeyboard(string text)
        {
            InitializeComponent();

            LanguageHelper.InitializeLanguage(this);

            memoEdit1.Text = text;
        }

        #region BASE METHODS
        private void addChar(string key)
        {
            cbShiftL.Checked = false;
            cbShiftR.Checked = false;
            deleteSelected();
            int index = memoEdit1.SelectionStart;
            memoEdit1.Text = memoEdit1.Text.Insert(index, key);
            refocus(index + key.Length);
        }

        private void refocus(int index)
        {
            memoEdit1.Focus();
            memoEdit1.SelectionStart = index;
            memoEdit1.SelectionLength = 0;
            memoEdit1.ScrollToCaret();
        }

        private void addChar(string big, string small)
        {
            if (cbCapsLock.Checked || cbShiftL.Checked || cbShiftR.Checked)
                addChar(big);
            else
                addChar(small);
        }

        private void deleteSelected()
        {
            int index = memoEdit1.SelectionStart;
            int length = memoEdit1.SelectionLength;
            if (length > 0)
            {
                memoEdit1.Text = memoEdit1.Text.Remove(index, length);
                refocus(index);
            }
        }
        #endregion

        #region KEYS
        private void keyApos_Click(object sender, EventArgs e)
        {
            addChar("\"");
        }

        private void key1_Click(object sender, EventArgs e)
        {
            addChar("1");
        }

        private void key2_Click(object sender, EventArgs e)
        {
            addChar("2");
        }

        private void key3_Click(object sender, EventArgs e)
        {
            addChar("3");
        }

        private void key4_Click(object sender, EventArgs e)
        {
            addChar("4");
        }

        private void key5_Click(object sender, EventArgs e)
        {
            addChar("5");
        }

        private void key6_Click(object sender, EventArgs e)
        {
            addChar("6");
        }

        private void key7_Click(object sender, EventArgs e)
        {
            addChar("7");
        }

        private void key8_Click(object sender, EventArgs e)
        {
            addChar("8");
        }

        private void key9_Click(object sender, EventArgs e)
        {
            addChar("9");
        }

        private void key0_Click(object sender, EventArgs e)
        {
            addChar("0");
        }

        private void keyMinus_Click(object sender, EventArgs e)
        {
            addChar("-");
        }

        private void keyEqual_Click(object sender, EventArgs e)
        {
            addChar("=");
        }

        private void keyBack_Click(object sender, EventArgs e)
        {
            int length = memoEdit1.SelectionLength;
            if (length > 0)
            {
                deleteSelected();
                return;
            }
            int index = memoEdit1.SelectionStart;
            if (index > 0)
            {
                memoEdit1.Text = memoEdit1.Text.Remove(index - 1, 1);
                refocus(index - 1);
            }
        }

        private void keyTab_Click(object sender, EventArgs e)
        {
            addChar("\t");
        }

        private void keyQ_Click(object sender, EventArgs e)
        {
            addChar("Q", "q");
        }

        private void keyW_Click(object sender, EventArgs e)
        {
            addChar("W", "w");
        }

        private void keyE_Click(object sender, EventArgs e)
        {
            addChar("E", "e");
        }

        private void keyR_Click(object sender, EventArgs e)
        {
            addChar("R", "r");
        }

        private void keyT_Click(object sender, EventArgs e)
        {
            addChar("T", "t");
        }

        private void keyY_Click(object sender, EventArgs e)
        {
            addChar("Y", "y");
        }

        private void keyU_Click(object sender, EventArgs e)
        {
            addChar("U", "u");
        }

        private void keyI_Click(object sender, EventArgs e)
        {
            addChar("I", "ı");
        }

        private void keyO_Click(object sender, EventArgs e)
        {
            addChar("O", "o");
        }

        private void keyP_Click(object sender, EventArgs e)
        {
            addChar("P", "p");
        }

        private void keyGG_Click(object sender, EventArgs e)
        {
            addChar("Ğ", "ğ");
        }

        private void keyUU_Click(object sender, EventArgs e)
        {
            addChar("Ü", "ü");
        }

        private void keyClampOpen_Click(object sender, EventArgs e)
        {
            addChar("[");
        }

        private void keyClampClose_Click(object sender, EventArgs e)
        {
            addChar("]");
        }

        private void keyA_Click(object sender, EventArgs e)
        {
            addChar("A", "a");
        }

        private void keyS_Click(object sender, EventArgs e)
        {
            addChar("S", "s");
        }

        private void keyD_Click(object sender, EventArgs e)
        {
            addChar("D", "d");
        }

        private void keyF_Click(object sender, EventArgs e)
        {
            addChar("F", "f");
        }

        private void keyG_Click(object sender, EventArgs e)
        {
            addChar("G", "g");
        }

        private void keyH_Click(object sender, EventArgs e)
        {
            addChar("H", "h");
        }

        private void keyJ_Click(object sender, EventArgs e)
        {
            addChar("J", "j");
        }

        private void keyK_Click(object sender, EventArgs e)
        {
            addChar("K", "k");
        }

        private void keyL_Click(object sender, EventArgs e)
        {
            addChar("L", "l");
        }

        private void keySS_Click(object sender, EventArgs e)
        {
            addChar("Ş", "ş");
        }

        private void keyII_Click(object sender, EventArgs e)
        {
            addChar("İ", "i");
        }

        private void keyComma_Click(object sender, EventArgs e)
        {
            addChar(",");
        }

        private void keyEnter_Click(object sender, EventArgs e)
        {
            addChar("\r\n");
        }

        private void keyLT_Click(object sender, EventArgs e)
        {
            addChar(">", "<");
        }

        private void keyZ_Click(object sender, EventArgs e)
        {
            addChar("Z", "z");
        }

        private void keyX_Click(object sender, EventArgs e)
        {
            addChar("X", "x");
        }

        private void keyC_Click(object sender, EventArgs e)
        {
            addChar("C", "c");
        }

        private void keyV_Click(object sender, EventArgs e)
        {
            addChar("V", "v");
        }

        private void keyB_Click(object sender, EventArgs e)
        {
            addChar("B", "b");
        }

        private void keyN_Click(object sender, EventArgs e)
        {
            addChar("N", "n");
        }

        private void keyM_Click(object sender, EventArgs e)
        {
            addChar("M", "m");
        }

        private void keyOO_Click(object sender, EventArgs e)
        {
            addChar("Ö", "ö");
        }

        private void keyCC_Click(object sender, EventArgs e)
        {
            addChar("Ç", "ç");
        }

        private void keyDot_Click(object sender, EventArgs e)
        {
            addChar(".");
        }

        private void keySpace_Click(object sender, EventArgs e)
        {
            addChar(" ");
        }
        #endregion

        #region CHECKBUTTON CHECKED EVENTS
        private void cbShiftL_CheckedChanged(object sender, EventArgs e)
        {
            if (cbShiftL.Checked)
                cbShiftL.Appearance.BackColor = Color.LightGray;
            else
                cbShiftL.Appearance.BackColor = SystemColors.ButtonHighlight;
        }

        private void cbShiftR_CheckedChanged(object sender, EventArgs e)
        {
            if (cbShiftR.Checked)
                cbShiftR.Appearance.BackColor = Color.LightGray;
            else
                cbShiftR.Appearance.BackColor = SystemColors.ButtonHighlight;
        }

        private void cbCapsLock_CheckedChanged(object sender, EventArgs e)
        {
            if (cbCapsLock.Checked)
                cbCapsLock.Appearance.BackColor = Color.LightGray;
            else
                cbCapsLock.Appearance.BackColor = SystemColors.ButtonHighlight;
        }
        #endregion

        private void btnSafe_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }

}