using System;
using System.Windows.Forms;

namespace BAITAPSANG10THANG10
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Sự kiện khi nhấn nút Đăng Ký
        

        // Sự kiện khi nhấn nút Làm Mới
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTenDangNhap.Clear();
            txtMatKhau.Clear();
            txtXacNhanMatKhau.Clear();
            dtpNgaySinh.Value = DateTime.Now;
            rdoNam.Checked = true;
            chkDieuKhoan.Checked = false;

            epCheck.Clear();
            txtTenDangNhap.Focus();
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            // 1. Xóa thông báo lỗi cũ
            epCheck.Clear();
            bool isValid = true;

            // 2. Kiểm tra các điều kiện
            if (string.IsNullOrWhiteSpace(txtTenDangNhap.Text))
            {
                epCheck.SetError(txtTenDangNhap, "Tên đăng nhập không được để trống!");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtMatKhau.Text))
            {
                epCheck.SetError(txtMatKhau, "Mật khẩu không được để trống!");
                isValid = false;
            }

            if (txtXacNhanMatKhau.Text != txtMatKhau.Text)
            {
                epCheck.SetError(txtXacNhanMatKhau, "Mật khẩu xác nhận không khớp!");
                isValid = false;
            }

            DateTime ngaySinh = dtpNgaySinh.Value;
            DateTime nay = DateTime.Now;
            int tuoi = nay.Year - ngaySinh.Year;
            if (ngaySinh.Date > nay.AddYears(-tuoi)) tuoi--;

            if (tuoi < 18)
            {
                epCheck.SetError(dtpNgaySinh, "Bạn phải đủ 18 tuổi trở lên để đăng ký!");
                isValid = false;
            }

            if (!chkDieuKhoan.Checked)
            {
                epCheck.SetError(chkDieuKhoan, "Bạn phải đồng ý với Điều khoản dịch vụ!");
                isValid = false;
            }

            // 3. NẾU HỢP LỆ -> HIỂN THỊ THÔNG TIN ĐÃ ĐĂNG KÝ
            if (isValid)
            {
                // Lấy giới tính từ RadioButton
                string gioiTinh = rdoNam.Checked ? "Nam" : "Nữ";

                // Chuỗi thông tin tổng hợp
                string thongTin = $"ĐĂNG KÝ THÀNH CÔNG!\n\n" +
                                  $"• Tên đăng nhập: {txtTenDangNhap.Text}\n" +
                                  $"• Mật khẩu: {txtMatKhau.Text}\n" +
                                  $"• Ngày sinh: {dtpNgaySinh.Value:dd/MM/yyyy}\n" +
                                  $"• Giới tính: {gioiTinh}\n" +
                                  $"• Điều khoản: Đã đồng ý";

                // Hiển thị hộp thông báo
                MessageBox.Show(thongTin, "Thông Tin Đăng Ký", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnLamMoi_Click_1(object sender, EventArgs e)
        {

        }
    }
}