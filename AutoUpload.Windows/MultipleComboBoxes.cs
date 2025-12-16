using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoUpload.Windows
{
    public partial class MultipleComboBoxes : UserControl
    {
        private string _value;
        private List<KeyValuePair<string, string>> items;

        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                var values = _value.Split(';');
                if (values.Length > 0)
                {
                    ComboBox01.SelectedValue = values[0];
                }
                if (values.Length > 1)
                {
                    ComboBox02.SelectedValue = values[1];
                }
                if (values.Length > 2)
                {
                    ComboBox03.SelectedValue = values[2];
                }
                if (values.Length > 3)
                {
                    ComboBox04.SelectedValue = values[3];
                }
                if (values.Length > 4)
                {
                    ComboBox05.SelectedValue = values[4];
                }
                if (values.Length > 5)
                {
                    ComboBox06.SelectedValue = values[5];
                }
                if (values.Length > 6)
                {
                    ComboBox07.SelectedValue = values[6];
                }
                if (values.Length > 7)
                {
                    ComboBox08.SelectedValue = values[7];
                }
            }
        }

        public List<KeyValuePair<string, string>> Items
        {
            get => items;
            set
            {
                items = value;
                if (items.All(x => x.Key != ""))
                {
                    items.Insert(0, new KeyValuePair<string, string>("", ""));
                }
                ComboBox01.DataSource = new List<KeyValuePair<string, string>>(items);
                ComboBox01.DisplayMember = "Value";
                ComboBox01.ValueMember = "Key";
                ComboBox02.DataSource = new List<KeyValuePair<string, string>>(items);
                ComboBox02.DisplayMember = "Value";
                ComboBox02.ValueMember = "Key";
                ComboBox03.DataSource = new List<KeyValuePair<string, string>>(items);
                ComboBox03.DisplayMember = "Value";
                ComboBox03.ValueMember = "Key";
                ComboBox04.DataSource = new List<KeyValuePair<string, string>>(items);
                ComboBox04.DisplayMember = "Value";
                ComboBox04.ValueMember = "Key";
                ComboBox05.DataSource = new List<KeyValuePair<string, string>>(items);
                ComboBox05.DisplayMember = "Value";
                ComboBox05.ValueMember = "Key";
                ComboBox06.DataSource = new List<KeyValuePair<string, string>>(items);
                ComboBox06.DisplayMember = "Value";
                ComboBox06.ValueMember = "Key";
                ComboBox07.DataSource = new List<KeyValuePair<string, string>>(items);
                ComboBox07.DisplayMember = "Value";
                ComboBox07.ValueMember = "Key";
                ComboBox08.DataSource = new List<KeyValuePair<string, string>>(items);
                ComboBox08.DisplayMember = "Value";
                ComboBox08.ValueMember = "Key";
            }
        }

        public MultipleComboBoxes()
        {
            InitializeComponent();
            MultipleComboBoxes_Resize(this, new EventArgs());
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var values = new []
                        {
                            ComboBox01.SelectedValue,
                            ComboBox02.SelectedValue,
                            ComboBox03.SelectedValue,
                            ComboBox04.SelectedValue,
                            ComboBox05.SelectedValue,
                            ComboBox06.SelectedValue,
                            ComboBox07.SelectedValue,
                            ComboBox08.SelectedValue,
                        };
            _value = string.Join(";", values.Where(x => x?.ToString() != "").ToArray());
            OnValueChanged(new EventArgs());
        }

        protected virtual void OnValueChanged(EventArgs eventargs)
        {
        }

        private void MultipleComboBoxes_Resize(object sender, EventArgs e)
        {
            var width = this.Width - SystemInformation.VerticalScrollBarWidth - 12;
            ComboBox01.Width = width / 2;
            ComboBox02.Width = width / 2;
            ComboBox03.Width = width / 2;
            ComboBox04.Width = width / 2;
            ComboBox05.Width = width / 2;
            ComboBox06.Width = width / 2;
            ComboBox07.Width = width / 2;
            ComboBox08.Width = width / 2;
        }
    }
}
