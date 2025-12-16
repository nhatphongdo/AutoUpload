using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoFacebook
{
    public partial class FormAddGroupPage : Form
    {
        public FormAddGroupPage()
        {
            InitializeComponent();
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextGroupPageLinks.Text))
            {
                MessageBox.Show("Please input at least 1 link", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var context = new LocalDatabaseEntities())
            {
                var wrongLinks = 0;
                var pageLinks = 0;
                var groupLinks = 0;
                foreach (var link in TextGroupPageLinks.Text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    // Get id in link
                    var parts = new List<string>(link.ToLower().Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries));
                    var index = parts.FindIndex(x => x.Contains("facebook.com"));
                    if (index >= 0 && index < parts.Count - 1)
                    {
                        if (parts[index + 1] == "groups" && index + 2 <= parts.Count - 1)
                        {
                            var url = $"http://www.facebook.com/groups/{parts[index + 2]}";
                            var group = context.Groups.FirstOrDefault(x => x.GroupLink == url);
                            if (group == null)
                            {
                                group = new Group()
                                {
                                    GroupLink = url,
                                };
                                context.Groups.Add(group);
                                ++groupLinks;
                            }
                        }
                        else
                        {
                            var url = $"http://www.facebook.com/{parts[index + 1]}";
                            var page = context.Pages.FirstOrDefault(x => x.PageLink == url);
                            if (page == null)
                            {
                                page = new Page()
                                {
                                    PageLink = url,
                                };
                                context.Pages.Add(page);
                                ++pageLinks;
                            }
                        }
                    }
                }

                context.SaveChanges();

                MessageBox.Show($"You already added {groupLinks} groups and {pageLinks} pages", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Close();
            }
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
