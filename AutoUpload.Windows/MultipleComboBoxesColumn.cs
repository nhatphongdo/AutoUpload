using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AutoUpload.Windows
{
    public class MultipleComboBoxesColumn : DataGridViewColumn
    {
        public string Value
        {
            get
            {
                return (base.CellTemplate as MultipleComboBoxesCell).Value as string;
            }
            set
            {
                (base.CellTemplate as MultipleComboBoxesCell).Value = value;
            }
        }

        public List<KeyValuePair<string, string>> Items
        {
            get
            {
                return (base.CellTemplate as MultipleComboBoxesCell).Items;
            }
            set
            {
                (base.CellTemplate as MultipleComboBoxesCell).Items = value;
            }
        }

        public MultipleComboBoxesColumn() : base(new MultipleComboBoxesCell())
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
                if (value != null && !value.GetType().IsAssignableFrom(typeof(MultipleComboBoxesCell)))
                {
                    throw new InvalidCastException("Must be a MultipleComboBoxesCell");
                }
                base.CellTemplate = value;
            }
        }
    }

    public class MultipleComboBoxesCell : DataGridViewTextBoxCell
    {
        public List<KeyValuePair<string, string>> Items { get; set; }

        public MultipleComboBoxesCell() : base()
        {

        }

        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            // Set the value of the editing control to the current cell value.
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            var control = DataGridView.EditingControl as MultipleComboBoxesEditingControl;
            control.Items = Items;

            // Use the default row value when Value property is null.
            if (Value == null)
            {
                control.Value = (string)DefaultNewRowValue;
            }
            else
            {
                control.Value = Value as string;
            }
        }

        public override Type EditType => typeof(MultipleComboBoxesEditingControl);

        public override object DefaultNewRowValue => "";

        public override object Clone()
        {
            var dataGridViewCell = new MultipleComboBoxesCell();
            dataGridViewCell.ValueType = ValueType;
            if (HasStyle)
            {
                dataGridViewCell.Style = new DataGridViewCellStyle(this.Style);
            }
            dataGridViewCell.ErrorText = ErrorText;
            dataGridViewCell.ToolTipText = ToolTipText;
            dataGridViewCell.ContextMenuStrip = ContextMenuStrip;
            dataGridViewCell.Tag = Tag;
            dataGridViewCell.Items = Items;
            dataGridViewCell.Value = Value;
            return dataGridViewCell;
        }
    }

    public class MultipleComboBoxesEditingControl : MultipleComboBoxes, IDataGridViewEditingControl
    {
        // Implements the IDataGridViewEditingControl.EditingControlFormattedValue 
        // property.
        public object EditingControlFormattedValue
        {
            get => this.Value;
            set => Value = (string)value;
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

