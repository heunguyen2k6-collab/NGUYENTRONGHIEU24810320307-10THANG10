using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace BAITAPSANG10THANG10
{
    public partial class Form3 : Form
    {
        private List<Product> listProducts = new List<Product>();
        private BindingSource bindingSource = new BindingSource();

        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            // Tránh văng popup lỗi DataGridView khi gõ sai định dạng
            dgvProducts.DataError += dgvProducts_DataError;

            // Nạp dữ liệu mẫu cho ComboBox Danh mục
            if (cboCategory != null)
            {
                cboCategory.Items.Clear();
                cboCategory.Items.AddRange(new string[] { "Điện thoại", "Laptop", "Phụ kiện", "Gia dụng" });
                if (cboCategory.Items.Count > 0)
                    cboCategory.SelectedIndex = 0;
            }

            // Gắn BindingSource
            bindingSource.DataSource = listProducts;
            dgvProducts.DataSource = bindingSource;

            // Cấu hình DataGridView an toàn
            dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.MultiSelect = false;
            dgvProducts.AllowUserToAddRows = false; // Tắt dòng trống cuối để tránh sinh dữ liệu null
        }

        private void dgvProducts_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Dữ liệu nhập vào không đúng định dạng!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            e.Cancel = true;
        }

        private void ClearInputs()
        {
            txtProductId.Clear();
            txtProductName.Clear();
            txtUnitPrice.Clear();
            txtQuantity.Clear();
            if (cboCategory != null && cboCategory.Items.Count > 0)
                cboCategory.SelectedIndex = 0;
            txtProductId.Focus();
        }

        // 1. NÚT THÊM
        private void btnThem_Click(object sender, EventArgs e)
        {
            string id = txtProductId.Text.Trim();
            string name = txtProductName.Text.Trim();

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Mã SP và Tên SP!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool isExist = listProducts.Any(x => x != null && string.Equals(x.ProductId, id, StringComparison.OrdinalIgnoreCase));
            if (isExist)
            {
                MessageBox.Show("Mã sản phẩm đã tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtUnitPrice.Text.Trim(), out decimal unitPrice) || unitPrice < 0)
            {
                MessageBox.Show("Đơn giá không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(txtQuantity.Text.Trim(), out int quantity) || quantity < 0)
            {
                MessageBox.Show("Số lượng không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Product p = new Product(
                id,
                name,
                unitPrice,
                quantity,
                cboCategory?.SelectedItem?.ToString() ?? ""
            );

            listProducts.Add(p);

            bindingSource.DataSource = null;
            bindingSource.DataSource = listProducts;

            ClearInputs();
            MessageBox.Show("Thêm sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // 2. CLICK DÒNG TRÊN DATAGRIDVIEW
        private void dgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvProducts.CurrentRow != null)
            {
                DataGridViewRow row = dgvProducts.Rows[e.RowIndex];

                txtProductId.Text = row.Cells["ProductId"].Value?.ToString() ?? "";
                txtProductName.Text = row.Cells["ProductName"].Value?.ToString() ?? "";
                txtUnitPrice.Text = row.Cells["UnitPrice"].Value?.ToString() ?? "0";
                txtQuantity.Text = row.Cells["Quantity"].Value?.ToString() ?? "0";

                string category = row.Cells["Category"].Value?.ToString();
                if (!string.IsNullOrEmpty(category) && cboCategory != null && cboCategory.Items.Contains(category))
                {
                    cboCategory.SelectedItem = category;
                }
            }
        }

        // 3. NÚT SỬA
        private void btnSua_Click(object sender, EventArgs e)
        {
            string id = txtProductId.Text.Trim();
            if (string.IsNullOrEmpty(id))
            {
                MessageBox.Show("Vui lòng chọn hoặc nhập Mã SP cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Product p = listProducts.FirstOrDefault(x => x != null && string.Equals(x.ProductId, id, StringComparison.OrdinalIgnoreCase));

            if (p != null)
            {
                if (!decimal.TryParse(txtUnitPrice.Text.Trim(), out decimal unitPrice) || !int.TryParse(txtQuantity.Text.Trim(), out int quantity))
                {
                    MessageBox.Show("Đơn giá hoặc số lượng không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                p.ProductName = txtProductName.Text.Trim();
                p.UnitPrice = unitPrice;
                p.Quantity = quantity;
                p.Category = cboCategory?.SelectedItem?.ToString() ?? "";

                bindingSource.ResetBindings(false);
                MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Không tìm thấy sản phẩm cần sửa (mã SP không trùng)!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 4. NÚT XÓA
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                string id = dgvProducts.CurrentRow.Cells["ProductId"].Value?.ToString();
                Product p = listProducts.FirstOrDefault(x => x != null && string.Equals(x.ProductId, id, StringComparison.OrdinalIgnoreCase));

                if (p != null)
                {
                    listProducts.Remove(p);

                    bindingSource.DataSource = null;
                    bindingSource.DataSource = listProducts;

                    ClearInputs();
                    MessageBox.Show("Xóa sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        // 5. NÚT TÌM KIẾM
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string kw = txtTimKiem.Text.Trim();

            if (string.IsNullOrEmpty(kw))
            {
                bindingSource.DataSource = listProducts;
            }
            else
            {
                var filtered = listProducts
                    .Where(x => x != null && x.ProductName != null && x.ProductName.IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();

                bindingSource.DataSource = filtered;
            }
        }
    }
}