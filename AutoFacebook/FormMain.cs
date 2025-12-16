using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;

namespace AutoFacebook
{
    public partial class FormMain : Form
    {
        private IList<IWebDriver> _webDrivers;
        private string _selectedMenuItem;
        private List<string> _groupsAndPages;
        private bool _isPosting;
        private decimal _timeCounter;
        private decimal _postCounter;
        private readonly string NewLine = char.ConvertFromUtf32(13) + char.ConvertFromUtf32(10);

        public FormMain()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Load data
            _webDrivers = new List<IWebDriver>();
            _groupsAndPages = new List<string>();
            ReloadAccounts();
            ReloadGroupsPages();

            RadioRepeat.Checked = true;

            FacebookApi.method_14("sunfrogshirts_new_anhdung");
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_webDrivers != null)
            {
                foreach (var webDriver in _webDrivers)
                {
                    webDriver.Quit();
                }
            }
        }

        private void ButtonAddNewAccount_Click(object sender, EventArgs e)
        {
            var form = new FormAddAccount();
            form.ShowDialog();
            ReloadAccounts();
        }

        private void ReloadAccounts()
        {
            using (var context = new LocalDatabaseEntities())
            {
                foreach (var account in context.Accounts)
                {
                    var type = account.Type == (int)AccountType.Facebook ? "Facebook" : "";
                    var text = $"{account.AccountName} ({account.Password})";
                    if (!CheckedListFacebookAccounts.Items.Contains(text))
                    {
                        CheckedListFacebookAccounts.Items.Add(text);
                    }
                }
            }
        }

        private void ReloadGroupsPages()
        {
            using (var context = new LocalDatabaseEntities())
            {
                foreach (var group in context.Groups)
                {
                    var type = group.Type == (int)GroupType.Open
                        ? "Open"
                        : (group.Type == (int)GroupType.Secret ? "Secret" : "Closed");
                    var text = $"Group {group.GroupName} ({type}) - {group.GroupLink}";
                    if (!_groupsAndPages.Contains(text))
                    {
                        CheckedListGroupPage.Items.Add(text);
                        _groupsAndPages.Add(text);
                    }
                }
                foreach (var page in context.Pages)
                {
                    var text = $"Page {page.PageName} - {page.PageLink}";
                    if (!_groupsAndPages.Contains(text))
                    {
                        CheckedListGroupPage.Items.Add(text);
                        _groupsAndPages.Add(text);
                    }
                }
            }
        }

        private void MenuDeleteAccount_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedMenuItem))
            {
                return;
            }

            CheckedListFacebookAccounts.Items.Remove(_selectedMenuItem);

            using (var context = new LocalDatabaseEntities())
            {
                var account = context.Accounts.FirstOrDefault(x => _selectedMenuItem.Contains(x.AccountName + " ("));
                if (account != null)
                {
                    context.Accounts.Remove(account);
                    context.SaveChanges();
                }
            }

            _selectedMenuItem = null;
        }

        private void CheckedListFacebookAccounts_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
            {
                return;
            }

            var index = CheckedListFacebookAccounts.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                _selectedMenuItem = CheckedListFacebookAccounts.Items[index].ToString();
            }
        }

        private void MenuEditAccount_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedMenuItem))
            {
                return;
            }

            using (var context = new LocalDatabaseEntities())
            {
                var account = context.Accounts.FirstOrDefault(x => _selectedMenuItem.Contains(x.AccountName + " ("));
                if (account != null)
                {
                    var form = new FormAddAccount
                    {
                        AccountName = account.AccountName,
                        IsEditing = true
                    };
                    form.ShowDialog();
                }
            }

            _selectedMenuItem = null;
        }

        private void TextSearchGroupPage_TextChanged(object sender, EventArgs e)
        {
            CheckedListGroupPage.Items.Clear();
            foreach (var item in _groupsAndPages)
            {
                if (string.IsNullOrEmpty(TextSearchGroupPage.Text) || item.ToLower().Contains(TextSearchGroupPage.Text.ToLower()))
                {
                    CheckedListGroupPage.Items.Add(item);
                }
            }
        }

        private void RadioRepeat_CheckedChanged(object sender, EventArgs e)
        {
            NumericRepeatMinutes.Enabled = RadioRepeat.Checked;
            NumericRepeatTotalPosts.Enabled = RadioRepeat.Checked;
            label4.Enabled = RadioRepeat.Checked;
            label6.Enabled = RadioRepeat.Checked;
            label7.Enabled = RadioRepeat.Checked;
        }

        private void RadioSpecificTimes_CheckedChanged(object sender, EventArgs e)
        {
            TimePickerSpecificTime.Enabled = RadioSpecificTimes.Checked;
            ButtonAddTime.Enabled = RadioSpecificTimes.Checked;
            ListSpecificTimes.Enabled = RadioSpecificTimes.Checked;
        }

        private void ButtonStartPosting_Click(object sender, EventArgs e)
        {
            // Check conditions
            var accountCount = 0;
            for (var i = 0; i < CheckedListFacebookAccounts.Items.Count; i++)
            {
                if (CheckedListFacebookAccounts.GetItemChecked(i))
                {
                    ++accountCount;
                }
            }
            if (accountCount == 0)
            {
                MessageBox.Show("Please select at least 1 account", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var groupCount = 0;
            for (var i = 0; i < CheckedListGroupPage.Items.Count; i++)
            {
                if (CheckedListGroupPage.GetItemChecked(i))
                {
                    ++groupCount;
                }
            }
            if (groupCount == 0)
            {
                MessageBox.Show("Please select at least 1 group or page", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(TextContent.Text))
            {
                MessageBox.Show("Please input content", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (RadioRepeat.Checked)
            {
                if (NumericRepeatMinutes.Value < 1)
                {
                    MessageBox.Show("Please input at least 1 minute for repeat time", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (NumericRepeatTotalPosts.Value < 1)
                {
                    MessageBox.Show("Please input at least 1 post for limit posts", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else if (RadioSpecificTimes.Checked)
            {
                if (ListSpecificTimes.Items.Count == 0)
                {
                    MessageBox.Show("Please input at least 1 specific time", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            ButtonStartPosting.Enabled = false;
            _isPosting = false;
            _timeCounter = NumericRepeatMinutes.Value;
            _postCounter = 0;
            TimerCounter.Start();
        }

        private void ButtonAddTime_Click(object sender, EventArgs e)
        {
            if (!ListSpecificTimes.Items.Contains(TimePickerSpecificTime.Value.ToString("HH:mm")))
            {
                ListSpecificTimes.Items.Add(TimePickerSpecificTime.Value.ToString("HH:mm"));
            }
        }

        private void LinkLabelAddImage_Click(object sender, EventArgs e)
        {
            if (OpenFileDialogImages.ShowDialog() == DialogResult.OK)
            {
                foreach (var file in OpenFileDialogImages.FileNames)
                {
                    if (ListImages.Items.Find(file.ToLower(), true).Length > 0)
                    {
                        continue;
                    }

                    var image = Image.FromFile(file);
                    var newImage = image.GetThumbnailImage(64, 64, () => false, IntPtr.Zero);
                    ImageListPosting.Images.Add(newImage);
                    ListImages.Items.Add(file.ToLower(), Path.GetFileNameWithoutExtension(file), ImageListPosting.Images.Count - 1);
                }
            }
        }

        private void ButtonAddNewGroupPage_Click(object sender, EventArgs e)
        {
            var form = new FormAddGroupPage();
            form.ShowDialog();
            ReloadGroupsPages();
        }

        private void CheckedListGroupPage_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
            {
                return;
            }

            var index = CheckedListGroupPage.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                _selectedMenuItem = CheckedListGroupPage.Items[index].ToString();
            }
        }

        private void MenuDeleteGroupPage_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedMenuItem))
            {
                return;
            }

            _groupsAndPages.Remove(_selectedMenuItem);
            CheckedListGroupPage.Items.Remove(_selectedMenuItem);

            using (var context = new LocalDatabaseEntities())
            {
                if (_selectedMenuItem.StartsWith("Group"))
                {
                    _selectedMenuItem = _selectedMenuItem + "+";
                    var group = context.Groups.FirstOrDefault(x => _selectedMenuItem.Contains(x.GroupLink + "+"));
                    if (group != null)
                    {
                        context.Groups.Remove(group);
                    }
                }
                else if (_selectedMenuItem.StartsWith("Page"))
                {
                    _selectedMenuItem = _selectedMenuItem + "+";
                    var page = context.Pages.FirstOrDefault(x => _selectedMenuItem.Contains(x.PageLink + "+"));
                    if (page != null)
                    {
                        context.Pages.Remove(page);
                    }
                }

                context.SaveChanges();
            }

            _selectedMenuItem = null;
        }

        private void MenuSelectAllDisplayGroupPage_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < CheckedListGroupPage.Items.Count; i++)
            {
                CheckedListGroupPage.SetItemChecked(i, true);
            }
        }

        private void CheckedListGroupPage_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            LabelInformation.Text = $"You will post on {CheckedListGroupPage.CheckedItems.Count + (e.NewValue == CheckState.Checked ? 1 : -1)} groups or pages by using {CheckedListFacebookAccounts.CheckedItems.Count} accounts.";
        }

        private bool Post(FacebookApi facebookApi, string account, string link, bool isGroup, string content, string[] images)
        {
            bool result;

            if (isGroup)
            {
                result = facebookApi.PostOnGroup(link, content, images);
                if (!result)
                {
                    TextMessageLog.Text =
                        $"*** ERROR ****{NewLine}Post to group {link} failed with account {account}: {facebookApi.ErrorMessage}{NewLine}{NewLine}" +
                        TextMessageLog.Text;
                }
                else
                {
                    TextMessageLog.Text = $"Post to group {link} with account {account} sucessfully.{NewLine}{NewLine}" + TextMessageLog.Text;
                }
            }
            else
            {
                result = facebookApi.PostOnPage(link, content);
                if (!result)
                {
                    TextMessageLog.Text =
                        $"*** ERROR ****{NewLine}Post to page {link} failed with account {account}: {facebookApi.ErrorMessage}{NewLine}{NewLine}" +
                        TextMessageLog.Text;
                }
                else
                {
                    TextMessageLog.Text = $"Post to page {link} with account {account} sucessfully.{NewLine}{NewLine}" + TextMessageLog.Text;
                }
            }

            return result;
        }

        private bool PostAll()
        {
            var allResults = true;
            _isPosting = true;
            using (var context = new LocalDatabaseEntities())
            {
                var images = new List<string>();
                foreach (ListViewItem imageItem in ListImages.Items)
                {
                    if (!string.IsNullOrEmpty(imageItem.Name))
                    {
                        images.Add(imageItem.Name);
                    }
                }
                foreach (string selectedAccount in CheckedListFacebookAccounts.CheckedItems)
                {
                    var account = context.Accounts.FirstOrDefault(x => selectedAccount.Contains(x.AccountName + " ("));
                    if (account != null)
                    {
                        var webdriver = FacebookApi.CreateWebDriver();
                        _webDrivers.Add(webdriver);
                        var facebookApi = new FacebookApi(webdriver);
                        var result = facebookApi.Login(account.AccountName, account.Password);

                        if (!result)
                        {
                            TextMessageLog.Text = $"*** ERROR ****{NewLine}Login failed with account {account}: {facebookApi.ErrorMessage}{NewLine}{NewLine}" + TextMessageLog.Text;
                            webdriver.Quit();
                            return false;
                        }

                        foreach (string selectedGroupPage in CheckedListGroupPage.CheckedItems)
                        {
                            var link = "";
                            var isGroup = false;
                            if (selectedGroupPage.StartsWith("Group"))
                            {
                                var group = context.Groups.FirstOrDefault(x => (selectedGroupPage + "+").Contains(x.GroupLink + "+"));
                                if (group != null)
                                {
                                    link = group.GroupLink;
                                    isGroup = true;
                                }
                            }
                            else if (selectedGroupPage.StartsWith("Page"))
                            {
                                var page = context.Pages.FirstOrDefault(x => (selectedGroupPage + "+").Contains(x.PageLink + "+"));
                                if (page != null)
                                {
                                    link = page.PageLink;
                                    isGroup = false;
                                }
                            }

                            if (!string.IsNullOrEmpty(link))
                            {
                                allResults &= Post(facebookApi, account.AccountName, link, isGroup, TextContent.Text, images.ToArray());
                            }
                        }

                        webdriver.Quit();
                        _webDrivers.Remove(webdriver);
                    }
                }
            }
            _isPosting = false;
            return allResults;
        }

        private void TimerCounter_Tick(object sender, EventArgs e)
        {
            if (_isPosting)
            {
                return;
            }

            if (RadioSpecificTimes.Checked)
            {
                foreach (var timeStr in ListSpecificTimes.Items)
                {
                    var time = DateTime.ParseExact(timeStr.ToString(), "HH:mm", CultureInfo.CurrentUICulture);
                    if (time.Hour == DateTime.Now.Hour && time.Minute == DateTime.Now.Minute)
                    {
                        if (!PostAll())
                        {
                            TimerCounter.Stop();
                            ButtonStartPosting.Enabled = true;
                        }
                        break;
                    }
                }
            }
            else if (RadioRepeat.Checked)
            {
                ++_timeCounter;
                if (_timeCounter >= NumericRepeatMinutes.Value)
                {
                    if (!PostAll())
                    {
                        TimerCounter.Stop();
                        ButtonStartPosting.Enabled = true;
                    }
                    _timeCounter = 0;
                    ++_postCounter;

                    if (_postCounter >= NumericRepeatTotalPosts.Value)
                    {
                        TimerCounter.Stop();
                        ButtonStartPosting.Enabled = true;
                    }
                }
            }
        }

        private void CheckedListFacebookAccounts_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            LabelInformation.Text = $"You will post on {CheckedListGroupPage.CheckedItems.Count} groups or pages by using {CheckedListFacebookAccounts.CheckedItems.Count + (e.NewValue == CheckState.Checked ? 1 : -1)} accounts.";
        }
    }
}
