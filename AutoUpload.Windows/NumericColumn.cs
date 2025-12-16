using System;
using System.Windows.Forms;

namespace AutoUpload.Windows
{
    public class NumericColumn : DataGridViewColumn
    {
        public decimal Minimum
        {
            get
            {
                return (base.CellTemplate as NumericCell).Minimum;
            }
            set
            {
                (base.CellTemplate as NumericCell).Minimum = value;
            }
        }

        public decimal Maximum
        {
            get
            {
                return (base.CellTemplate as NumericCell).Maximum;
            }
            set
            {
                (base.CellTemplate as NumericCell).Maximum = value;
            }
        }

        public NumericColumn() : base(new NumericCell())
        {
        }

        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                // Ensure that the cell used for the template is a CalendarCell.
                if (value != null && !value.GetType().IsAssignableFrom(typeof(NumericCell)))
                {
                    throw new InvalidCastException("Must be a NumericCell");
                }
                base.CellTemplate = value;
            }
        }
    }

    public class NumericCell : DataGridViewTextBoxCell
    {
        public decimal Minimum { get; set; }

        public decimal Maximum { get; set; }

        public NumericCell() : base()
        {

        }

        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            // Set the value of the editing control to the current cell value.
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            var control = DataGridView.EditingControl as NumericEditingControl;
            control.Minimum = Minimum;
            control.Maximum = Maximum;
            // Use the default row value when Value property is null.
            if (Value == null)
            {
                control.Value = (decimal)DefaultNewRowValue;
            }
            else
            {
                try
                {
                    control.Value = decimal.Parse((string)Value);
                }
                catch (Exception ex)
                {
                    control.Value = (decimal)DefaultNewRowValue;
                }
            }
        }

        public override Type EditType => typeof(NumericEditingControl);

        public override object DefaultNewRowValue => Minimum;

        public override object Clone()
        {
            var dataGridViewCell = new NumericCell();
            dataGridViewCell.ValueType = ValueType;
            if (HasStyle)
            {
                dataGridViewCell.Style = new DataGridViewCellStyle(this.Style);
            }
            dataGridViewCell.ErrorText = ErrorText;
            dataGridViewCell.ToolTipText = ToolTipText;
            dataGridViewCell.ContextMenuStrip = ContextMenuStrip;
            dataGridViewCell.Tag = Tag;
            dataGridViewCell.Minimum = Minimum;
            dataGridViewCell.Maximum = Maximum;
            return dataGridViewCell;
        }
    }

    public class NumericEditingControl : NumericUpDown, IDataGridViewEditingControl
    {
        // Implements the IDataGridViewEditingControl.EditingControlFormattedValue 
        // property.
        public object EditingControlFormattedValue
        {
            get { return this.Value.ToString(); }
            set
            {
                if (value is string)
                {
                    try
                    {
                        // This will throw an exception of the string is 
                        // null, empty, or not in the format of a date.
                        this.Value = decimal.Parse((string)value);
                    }
                    catch
                    {
                        // In the case of an exception, just use the 
                        // default value so we're not left with a null
                        // value.
                        this.Value = 0;
                    }
                }
            }
        }

        // Implements the 
        // IDataGridViewEditingControl.GetEditingControlFormattedValue method.
        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            return EditingControlFormattedValue;
        }

        // Implements the 
        // IDataGridViewEditingControl.ApplyCellStyleToEditingControl method.
        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {
            Font = dataGridViewCellStyle.Font;
            ForeColor = dataGridViewCellStyle.ForeColor;
        }

        // Implements the IDataGridViewEditingControl.EditingControlRowIndex 
        // property.
        public int EditingControlRowIndex { get; set; }

        // Implements the IDataGridViewEditingControl.EditingControlWantsInputKey 
        // method.
        public bool EditingControlWantsInputKey(Keys key, bool dataGridViewWantsInputKey)
        {
            // Let the NumericUpDown handle the keys listed.
            switch (key & Keys.KeyCode)
            {
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                case Keys.Right:
                case Keys.Home:
                case Keys.End:
                case Keys.PageDown:
                case Keys.PageUp:
                case Keys.Enter:
                    return true;
                default:
                    return true;
                //return !dataGridViewWantsInputKey;
            }
        }

        // Implements the IDataGridViewEditingControl.PrepareEditingControlForEdit 
        // method.
        public void PrepareEditingControlForEdit(bool selectAll)
        {
            // No preparation needs to be done.
        }

        // Implements the IDataGridViewEditingControl
        // .RepositionEditingControlOnValueChange property.
        public bool RepositionEditingControlOnValueChange => false;

        // Implements the IDataGridViewEditingControl
        // .EditingControlDataGridView property.
        public DataGridView EditingControlDataGridView { get; set; }

        // Implements the IDataGridViewEditingControl
        // .EditingControlValueChanged property.
        public bool EditingControlValueChanged { get; set; } = false;

        // Implements the IDataGridViewEditingControl
        // .EditingPanelCursor property.
        public Cursor EditingPanelCursor => Cursor;

        protected override void OnValueChanged(EventArgs eventargs)
        {
            // Notify the DataGridView that the contents of the cell
            // have changed.
            EditingControlValueChanged = true;
            EditingControlDataGridView.NotifyCurrentCellDirty(true);
            base.OnValueChanged(eventargs);
        }
    }
}

