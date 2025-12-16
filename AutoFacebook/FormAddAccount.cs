using System;
using System.Linq;
using System.Windows.Forms;

namespace AutoFacebook
{
    public partial class FormAddAccount : Form
    {
        public bool IsEditing { get; set; }

        public string AccountName { get; set; }

        public FormAddAccount()
        {
            InitializeComponent();
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextAccount.Text))
            {
                MessageBox.Show("Please input Account", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(TextPassword.Text))
            {
                MessageBox.Show("Please input Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Add account to database
            using (var context = new LocalDatabaseEntities())
            {
                var account = context.Accounts.FirstOrDefault(x => x.AccountName.Equals(TextAccount.Text.Trim(), StringComparison.OrdinalIgnoreCase) && x.Type == (int)AccountType.Facebook);

                if (account == null)
                {
                    account = new Account()
                    {
                        AccountName = TextAccount.Text.Trim().ToLower(),
                        Type = (int)AccountType.Facebook
                    };
                    context.Accounts.Add(account);
                }
                account.Password = TextPassword.Text;

                context.SaveChanges();
            }

            Close();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FormAddAccount_Shown(object sender, EventArgs e)
        {
            if (IsEditing)
            {
                TextAccount.Enabled = false;
                TextAccount.Text = AccountName;
            }
        }
    }
}
