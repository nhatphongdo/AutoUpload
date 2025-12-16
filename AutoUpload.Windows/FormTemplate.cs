using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace AutoUpload.Windows
{
    public partial class FormTemplate : Form
    {
        public FormTemplate()
        {
            InitializeComponent();
        }

        private void FormTemplate_Load(object sender, EventArgs e)
        {
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ComboBoxFrontHAlign.SelectedItem as string))
            {
                MessageBox.Show(@"You must choose a value of Front Horizontal Align", @"Auto Upload", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ComboBoxFrontHAlign.Focus();
                return;
            }

            if (string.IsNullOrEmpty(ComboBoxFrontVAlign.SelectedItem as string))
            {
                MessageBox.Show(@"You must choose a value of Front Vertical Align", @"Auto Upload", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ComboBoxFrontVAlign.Focus();
                return;
            }

            if (string.IsNullOrEmpty(ComboBoxBackHAlign.SelectedItem as string))
            {
                MessageBox.Show(@"You must choose a value of Back Horizontal Align", @"Auto Upload", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ComboBoxBackHAlign.Focus();
                return;
            }

            if (string.IsNullOrEmpty(ComboBoxBackVAlign.SelectedItem as string))
            {
                MessageBox.Show(@"You must choose a value of Back Vertical Align", @"Auto Upload", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ComboBoxBackVAlign.Focus();
                return;
            }

            if (string.IsNullOrEmpty(TextBoxTitle.Text))
            {
                MessageBox.Show(@"You must choose a value of Title", @"Auto Upload", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TextBoxTitle.Focus();
                return;
            }

            if (string.IsNullOrEmpty(TextBoxDescription.Text))
            {
                MessageBox.Show(@"You must choose a value of Description", @"Auto Upload", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TextBoxDescription.Focus();
                return;
            }

            if (string.IsNullOrEmpty(TextBoxKeywords.Text))
            {
                MessageBox.Show(@"You must choose a value of Keywords", @"Auto Upload", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TextBoxKeywords.Focus();
                return;
            }

            if (string.IsNullOrEmpty(TextBoxUrl.Text))
            {
                MessageBox.Show(@"You must choose a value of Url", @"Auto Upload", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TextBoxUrl.Focus();
                return;
            }

            if (SaveFileDialogTemplate.ShowDialog() == DialogResult.OK)
            {
                var json = new JObject
                {
                    ["FrontHAlign"] = (string)ComboBoxFrontHAlign.SelectedItem,
                    ["FrontVAlign"] = (string)ComboBoxFrontVAlign.SelectedItem,
                    ["BackHAlign"] = (string)ComboBoxBackHAlign.SelectedItem,
                    ["BackVAlign"] = (string)ComboBoxBackVAlign.SelectedItem,
                    ["Title"] = TextBoxTitle.Text,
                    ["Description"] = TextBoxDescription.Text,
                    ["SunfrogCategory"] = ComboBoxSunfrogCategory.SelectedItem == null ? "" : ((KeyValuePair<string, string>)ComboBoxSunfrogCategory.SelectedItem).Key,
                    ["TeespringCategory"] = ComboBoxTeespringCategory.SelectedItem == null ? "" : ((KeyValuePair<string, string>)ComboBoxTeespringCategory.SelectedItem).Key,
                    ["ViralstyleCategory"] = ComboBoxViralstyleCategory.SelectedItem == null ? "" : ((KeyValuePair<string, string>)ComboBoxViralstyleCategory.SelectedItem).Key,
                    ["Keywords"] = TextBoxKeywords.Text,
                    ["Duration"] = NumericUpDownDuration.Value,
                    ["Url"] = TextBoxUrl.Text,
                    ["Goal"] = NumericUpDownGoal.Value,
                    ["AutoRestart"] = CheckBoxAutoRestart.Checked,
                    ["IsPrivate"] = CheckBoxIsPrivate.Checked,
                };
                File.WriteAllText(SaveFileDialogTemplate.FileName, json.ToString());
                MessageBox.Show(@"Template file is saved successfully.", @"Auto Upload", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
