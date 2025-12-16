using AutoUpload.Shared;
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

namespace AutoUpload.Windows
{
    public partial class FormSettings : Form
    {
        public FormSettings()
        {
            InitializeComponent();
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            var settings = new Settings();
            try
            {
                var fileStream = new FileStream("settings.config", FileMode.Open, FileAccess.Read, FileShare.None);
                settings.Load(fileStream);
                fileStream.Close();

                NumericUpDownSunfrogDelayTime.Value = settings.SunfrogDelayTime;
                NumericUpDownTeespringDelayTime.Value = settings.TeespringDelayTime;
                NumericUpDownTeechipDelayTime.Value = settings.TeechipDelayTime;
                NumericUpDownViralstyleDelayTime.Value = settings.ViralstyleDelayTime;
                NumericUpDownTeezilyDelayTime.Value = settings.TeezilyDelayTime;

                CheckBoxSunfrogMultipleAccounts.Checked = settings.SunfrogDesignsPerAccount > 0;
                NumericUpDownSunfrogDesignsPerAccount.Value = settings.SunfrogDesignsPerAccount;
                CheckBoxTeespringMultipleAccounts.Checked = settings.TeespringDesignsPerAccount > 0;
                NumericUpDownTeespringDesignsPerAccount.Value = settings.TeespringDesignsPerAccount;
                CheckBoxTeechipMultipleAccounts.Checked = settings.TeechipDesignsPerAccount > 0;
                NumericUpDownTeechipDesignsPerAccount.Value = settings.TeechipDesignsPerAccount;
                CheckBoxViralstyleMultipleAccounts.Checked = settings.ViralstyleDesignsPerAccount > 0;
                NumericUpDownViralstyleDesignsPerAccount.Value = settings.ViralstyleDesignsPerAccount;
                CheckBoxTeezilyMultipleAccounts.Checked = settings.TeezilyDesignsPerAccount > 0;
                NumericUpDownTeezilyDesignsPerAccount.Value = settings.TeezilyDesignsPerAccount;

                TextBoxMockupDefaultLocation.Text = settings.MockupDefaultLocation;
            }
            catch (Exception exc)
            {
                Logger.WriteLog(exc);
                // Recommended values
                NumericUpDownSunfrogDelayTime.Value = 15;
                NumericUpDownTeespringDelayTime.Value = 15;
                NumericUpDownTeechipDelayTime.Value = 15;
                NumericUpDownViralstyleDelayTime.Value = 15;
                NumericUpDownTeezilyDelayTime.Value = 15;
                CheckBoxSunfrogMultipleAccounts.Checked = false;
                NumericUpDownSunfrogDesignsPerAccount.Value = 0;
                CheckBoxTeespringMultipleAccounts.Checked = false;
                NumericUpDownTeespringDesignsPerAccount.Value = 0;
                CheckBoxTeechipMultipleAccounts.Checked = false;
                NumericUpDownTeechipDesignsPerAccount.Value = 0;
                CheckBoxViralstyleMultipleAccounts.Checked = false;
                NumericUpDownViralstyleDesignsPerAccount.Value = 0;
                CheckBoxTeezilyMultipleAccounts.Checked = false;
                NumericUpDownTeezilyDesignsPerAccount.Value = 0;
                TextBoxMockupDefaultLocation.Text = "";
            }
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            // Save to file
            var settings = new Settings();
            try
            {
                settings.SunfrogDelayTime = (int)NumericUpDownSunfrogDelayTime.Value;
                settings.TeespringDelayTime = (int)NumericUpDownTeespringDelayTime.Value;
                settings.TeechipDelayTime = (int)NumericUpDownTeechipDelayTime.Value;
                settings.ViralstyleDelayTime = (int)NumericUpDownViralstyleDelayTime.Value;
                settings.TeezilyDelayTime = (int)NumericUpDownTeezilyDelayTime.Value;

                settings.SunfrogDesignsPerAccount = CheckBoxSunfrogMultipleAccounts.Checked ? (int)NumericUpDownSunfrogDesignsPerAccount.Value : 0;
                settings.TeespringDesignsPerAccount = CheckBoxTeespringMultipleAccounts.Checked ? (int)NumericUpDownTeespringDesignsPerAccount.Value : 0;
                settings.TeechipDesignsPerAccount = CheckBoxTeechipMultipleAccounts.Checked ? (int)NumericUpDownTeechipDesignsPerAccount.Value : 0;
                settings.ViralstyleDesignsPerAccount = CheckBoxViralstyleMultipleAccounts.Checked ? (int)NumericUpDownViralstyleDesignsPerAccount.Value : 0;
                settings.TeezilyDesignsPerAccount = CheckBoxTeezilyMultipleAccounts.Checked ? (int)NumericUpDownTeezilyDesignsPerAccount.Value : 0;

                settings.MockupDefaultLocation = TextBoxMockupDefaultLocation.Text;

                var fileStream = new FileStream("settings.config", FileMode.Create, FileAccess.Write, FileShare.None);
                settings.Save(fileStream);
                fileStream.Close();
                Close();
            }
            catch (Exception exc)
            {
                Logger.WriteLog(exc);
                MessageBox.Show("Cannot save settings file. Please check access rights to file and try again.", "Auto Upload", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CheckBoxSunfrogMultipleAccounts_CheckedChanged(object sender, EventArgs e)
        {
            NumericUpDownSunfrogDesignsPerAccount.Enabled = CheckBoxSunfrogMultipleAccounts.Checked;
        }

        private void CheckBoxTeespringMultipleAccounts_CheckedChanged(object sender, EventArgs e)
        {
            NumericUpDownTeespringDesignsPerAccount.Enabled = CheckBoxTeespringMultipleAccounts.Checked;
        }

        private void CheckBoxTeechipMultipleAccounts_CheckedChanged(object sender, EventArgs e)
        {
            NumericUpDownTeechipDesignsPerAccount.Enabled = CheckBoxTeechipMultipleAccounts.Checked;
        }

        private void CheckBoxViralstyleMultipleAccounts_CheckedChanged(object sender, EventArgs e)
        {
            NumericUpDownViralstyleDesignsPerAccount.Enabled = CheckBoxViralstyleMultipleAccounts.Checked;
        }

        private void CheckBoxTeezilyMultipleAccounts_CheckedChanged(object sender, EventArgs e)
        {
            NumericUpDownTeezilyDesignsPerAccount.Enabled = CheckBoxTeezilyMultipleAccounts.Checked;
        }

        private void ButtonBrowseMockupLocation_Click(object sender, EventArgs e)
        {
            if (FolderBrowserDialogMockupLocation.ShowDialog() == DialogResult.OK)
            {
                TextBoxMockupDefaultLocation.Text = FolderBrowserDialogMockupLocation.SelectedPath;
            }
        }
    }
}
