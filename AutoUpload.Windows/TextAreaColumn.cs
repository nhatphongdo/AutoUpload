using System;
using System.Windows.Forms;

namespace AutoUpload.Windows
{
    public class TextAreaColumn : DataGridViewColumn
    {
        public int MaxInputLength
        {
            get { return (CellTemplate as TextAreaCell).MaxInputLength; }
            set { (CellTemplate as TextAreaCell).MaxInputLength = value; }
        }

        public TextAreaColumn()
            : base(new TextAreaCell())
        {
        }

        public override DataGridViewCell CellTemplate
        {
            get { return base.CellTemplate; }
            set
            {
                // Ensure that the cell used for the template is a CalendarCell.
                if (value != null && !value.GetType().IsAssignableFrom(typeof(TextAreaCell)))
                {
                    throw new InvalidCastException("Must be a TextAreaCell");
                }
                base.CellTemplate = value;
            }
        }
    }

    public class TextAreaCell : DataGridViewTextBoxCell
    {
        public int MaxInputLength
        {
            get
            {
                if (DataGridView == null)
                {
                    return 0;
                }
                return (DataGridView.EditingControl as TextAreaEditingControl).MaxLength;
            }
            set
            {
                if (DataGridView == null)
                {
                    return;
                }
                (DataGridView.EditingControl as TextAreaEditingControl).MaxLength = value;
            }
        }

        public TextAreaCell()
            : base()
        {
        }

        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            // Set the value of the editing control to the current cell value.
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            var control = DataGridView.EditingControl as TextAreaEditingControl;
            // Use the default row value when Value property is null.
            if (Value == null)
            {
                control.Text = "";
            }
            else
            {
                control.Text = Value.ToString();
            }
        }

        public override Type EditType => typeof(TextAreaEditingControl);

        public override object DefaultNewRowValue => "";

        public override object Clone()
        {
            var dataGridViewCell = new TextAreaCell();
            dataGridViewCell.ValueType = ValueType;
            if (HasStyle)
            {
                dataGridViewCell.Style = new DataGridViewCellStyle(this.Style);
            }
            dataGridViewCell.ErrorText = ErrorText;
            dataGridViewCell.ToolTipText = ToolTipText;
            dataGridViewCell.ContextMenuStrip = ContextMenuStrip;
            dataGridViewCell.Tag = Tag;
            return dataGridViewCell;
        }
    }

    public class TextAreaEditingControl : TextBox, IDataGridViewEditingControl
    {
        // Implements the IDataGridViewEditingControl.EditingControlFormattedValue 
        // property.
        public object EditingControlFormattedValue
        {
            get { return Text; }
            set { Text = value.ToString(); }
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
            Multiline = true;
            AcceptsReturn = true;
            ScrollBars = ScrollBars.Both;
            WordWrap = false;
        }

        // Implements the IDataGridViewEditingControl.EditingControlRowIndex 
        // property.
        public int EditingControlRowIndex { get; set; }

        // Implements the IDataGridViewEditingControl.EditingControlWantsInputKey 
        // method.
        public bool EditingControlWantsInputKey(Keys key, bool dataGridViewWantsInputKey)
        {
            // Let the NumericUpDown handle the keys listed.
            return true;
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

        protected override void OnTextChanged(EventArgs eventargs)
        {
            // Notify the DataGridView that the contents of the cell
            // have changed.
            EditingControlValueChanged = true;
            EditingControlDataGridView.NotifyCurrentCellDirty(true);
            base.OnTextChanged(eventargs);
        }
    }
}