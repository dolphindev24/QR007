using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QR007
{
    public class DatagridviewCustom
    {
        public void AddCheckboxToDatagridview(DataGridView dataGridView)
        {
            DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
            checkBoxColumn.HeaderText = "Select"; // Tiêu đề cột
            checkBoxColumn.Name = "checkBoxColumn"; // Tên cột
            checkBoxColumn.Width = 50; // Chiều rộng của cột
            checkBoxColumn.ReadOnly = false; // Cho phép chỉnh sửa

            // Thêm cột Checkbox vào DataGridView
            dataGridView.Columns.Add(checkBoxColumn);
        }

        

    }
}
