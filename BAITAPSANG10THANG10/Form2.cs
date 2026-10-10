using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BAITAPSANG10THANG10
{
    public partial class Form2 : Form
    {
        // Dictionary lưu danh sách dịch vụ theo từng loại
        private Dictionary<string, List<ServiceItem>> serviceData;

        public Form2()
        {
            InitializeComponent();
        }

        // Sự kiện khi Form vừa khởi chạy
        private void Form2_Load(object sender, EventArgs e)
        {
            InitData();
        }

        private void InitData()
        {
            // 1. Tạo dữ liệu các dịch vụ
            serviceData = new Dictionary<string, List<ServiceItem>>
            {
                {
                    "Khám bệnh", new List<ServiceItem>
                    {
                        new ServiceItem("Khám tổng quát", 150000),
                        new ServiceItem("Khám chuyên khoa", 250000),
                        new ServiceItem("Tái khám", 100000)
                    }
                },
                {
                    "Xét nghiệm", new List<ServiceItem>
                    {
                        new ServiceItem("Xét nghiệm máu toàn bộ", 200000),
                        new ServiceItem("Xét nghiệm đường huyết", 80000),
                        new ServiceItem("Xét nghiệm nước tiểu", 100000)
                    }
                },
                {
                    "Chụp X-Quang", new List<ServiceItem>
                    {
                        new ServiceItem("Chụp X-Quang Phổi", 180000),
                        new ServiceItem("Chụp X-Quang Cột sống", 220000),
                        new ServiceItem("Chụp X-Quang Chân/Tay", 150000)
                    }
                },
                {
                    "Vắc-xin", new List<ServiceItem>
                    {
                        new ServiceItem("Vắc-xin Cúm", 350000),
                        new ServiceItem("Vắc-xin Viêm gan B", 250000),
                        new ServiceItem("Vắc-xin Dại", 400000)
                    }
                }
            };

            // 2. Nạp các loại dịch vụ vào ComboBox
            cboCategory.DataSource = new List<string>(serviceData.Keys);
            cboCategory.SelectedIndex = 0;

            // Hiển thị danh sách ban đầu
            LoadAvailableServices();
        }

        // Load danh sách dịch vụ khi đổi loại ở ComboBox
        private void LoadAvailableServices()
        {
            if (cboCategory.SelectedItem == null) return;

            string selectedCategory = cboCategory.SelectedItem.ToString();
            lstAvailableServices.Items.Clear();

            if (serviceData.ContainsKey(selectedCategory))
            {
                foreach (var item in serviceData[selectedCategory])
                {
                    lstAvailableServices.Items.Add(item);
                }
            }
        }

        // Khi thay đổi mục chọn trên ComboBox (Yêu cầu 1)
        private void cboCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadAvailableServices();
        }

        // Hàm hỗ trợ chuyển item sang danh sách đã chọn (Yêu cầu 2)
        private void MoveToSelectedList()
        {
            if (lstAvailableServices.SelectedItem != null)
            {
                ServiceItem item = (ServiceItem)lstAvailableServices.SelectedItem;

                // Kiểm tra nếu chưa có trong danh sách đã chọn thì mới thêm
                if (!lstSelectedServices.Items.Contains(item))
                {
                    lstSelectedServices.Items.Add(item);
                    CalculateTotal(); // Tính lại tiền
                }
                else
                {
                    MessageBox.Show("Dịch vụ này đã được thêm trước đó!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        // Nút >
        private void btnSelect_Click(object sender, EventArgs e)
        {
            MoveToSelectedList();
        }

        // Double Click vào item ở ListBox trái (Yêu cầu 2)
        private void lstAvailableServices_DoubleClick(object sender, EventArgs e)
        {
            MoveToSelectedList();
        }

        // Nút < (Xóa 1 item đã chọn)
        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lstSelectedServices.SelectedItem != null)
            {
                lstSelectedServices.Items.Remove(lstSelectedServices.SelectedItem);
                CalculateTotal(); // Tính lại tiền
            }
        }

        // Nút << (Xóa tất cả)
        private void btnClearAll_Click(object sender, EventArgs e)
        {
            lstSelectedServices.Items.Clear();
            CalculateTotal(); // Tính lại tiền
        }

        // Tính toán tổng tiền (Yêu cầu 3)
        private void CalculateTotal()
        {
            decimal totalBeforeDiscount = 0;

            // Cộng dồn đơn giá của các dịch vụ trong lstSelectedServices
            foreach (ServiceItem item in lstSelectedServices.Items)
            {
                totalBeforeDiscount += item.Price;
            }

            // Lấy tỉ lệ giảm giá từ TextBox
            decimal discountPercent = 0;
            decimal.TryParse(txtDiscountPercent.Text, out discountPercent);

            if (discountPercent < 0) discountPercent = 0;
            if (discountPercent > 100) discountPercent = 100;

            // Tính tiền giảm và thành tiền
            decimal discountAmount = totalBeforeDiscount * (discountPercent / 100);
            decimal finalTotal = totalBeforeDiscount - discountAmount;

            // Hiển thị ra các Label
            lblTotalBeforeDiscount.Text = $"{totalBeforeDiscount:N0} VNĐ";
            lblFinalTotal.Text = $"{finalTotal:N0} VNĐ";
        }

        // Tự động tính lại tiền khi nhập/sửa % giảm giá
        private void txtDiscountPercent_TextChanged(object sender, EventArgs e)
        {
            CalculateTotal();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}