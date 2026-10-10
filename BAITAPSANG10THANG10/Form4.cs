using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BAITAPSANG10THANG10
{
    public partial class Form4 : Form
    {
        public class Employee
        {
            public string MaNV { get; set; }
            public string HoTen { get; set; }
            public string ChucVu { get; set; }
            public string NgayVaoLam { get; set; }
            public string GroupNodeKey { get; set; }
            public int ImageIndex { get; set; } // Chỉ số Icon cho Nhân viên
        }

        private List<Employee> listEmployees = new List<Employee>();

        // Khai báo các ImageList
        private ImageList imgListSmall = new ImageList();
        private ImageList imgListLarge = new ImageList();

        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            InitImageLists();          // 1. Tạo và nạp Icon vào ImageList
            SetupListViewColumns();     // 2. Cài đặt ListView
            SetupComboBoxViewMode();   // 3. Cài đặt ComboBox
            LoadTreeViewData();        // 4. Nạp TreeView
            LoadSampleEmployees();     // 5. Nạp danh sách nhân viên
        }

        // --- 1. KHỞI TẠO IMAGELIST VỚI ICON ĐƯỢC VẼ TỰ ĐỘNG ---
        private void InitImageLists()
        {
            // Thiết lập kích thước Icon
            imgListSmall.ImageSize = new Size(16, 16);
            imgListLarge.ImageSize = new Size(48, 48);

            // Tạo các Icon bằng Code (Không cần file ảnh bên ngoài)
            // Index 0: Icon Công ty (Màu Xanh Dương)
            // Index 1: Icon Phòng Ban (Màu Xanh Lá)
            // Index 2: Icon Nhóm/Thư mục con (Màu Cam)
            // Index 3: Icon Nhân viên Nam (Màu Xanh Da Trời)
            // Index 4: Icon Nhân viên Nữ (Màu Hồng)

            imgListSmall.Images.Add(CreateIcon(16, Color.DarkBlue, "🏢"));
            imgListSmall.Images.Add(CreateIcon(16, Color.SeaGreen, "📁"));
            imgListSmall.Images.Add(CreateIcon(16, Color.DarkOrange, "🗂️"));
            imgListSmall.Images.Add(CreateIcon(16, Color.DodgerBlue, "👤"));
            imgListSmall.Images.Add(CreateIcon(16, Color.DeepPink, "👩"));

            imgListLarge.Images.Add(CreateIcon(48, Color.DarkBlue, "🏢"));
            imgListLarge.Images.Add(CreateIcon(48, Color.SeaGreen, "📁"));
            imgListLarge.Images.Add(CreateIcon(48, Color.DarkOrange, "🗂️"));
            imgListLarge.Images.Add(CreateIcon(48, Color.DodgerBlue, "👤"));
            imgListLarge.Images.Add(CreateIcon(48, Color.DeepPink, "👩"));

            // Gán ImageList cho TreeView và ListView
            tvDepartments.ImageList = imgListSmall;
            lsvEmployees.SmallImageList = imgListSmall;
            lsvEmployees.LargeImageList = imgListLarge;
        }

        // Hàm hỗ trợ vẽ Icon bằng ký tự Unicode
        private Bitmap CreateIcon(int size, Color bgColor, string symbol)
        {
            Bitmap bmp = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Transparent);
                using (Font font = new Font("Segoe UI Emoji", size * 0.65f, FontStyle.Regular))
                {
                    TextRenderer.DrawText(g, symbol, font, new Rectangle(0, 0, size, size), bgColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }
            }
            return bmp;
        }

        // --- 2. CÀI ĐẶT LISTVIEW ---
        private void SetupListViewColumns()
        {
            lsvEmployees.View = View.Details;
            lsvEmployees.FullRowSelect = true;
            lsvEmployees.GridLines = true;

            lsvEmployees.Columns.Clear();
            lsvEmployees.Columns.Add("Mã NV", 100);
            lsvEmployees.Columns.Add("Họ Tên", 180);
            lsvEmployees.Columns.Add("Chức vụ", 140);
            lsvEmployees.Columns.Add("Ngày vào làm", 110);
        }

        // --- 3. CÀI ĐẶT COMBOBOX ---
        private void SetupComboBoxViewMode()
        {
            cboViewMode.Items.Clear();
            cboViewMode.Items.AddRange(new string[] { "Details", "LargeIcon", "SmallIcon", "List", "Tile" });
            cboViewMode.SelectedIndex = 0;
            cboViewMode.SelectedIndexChanged += cboViewMode_SelectedIndexChanged;
        }

        // --- 4. CẤU TRÚC TREEVIEW CÓ CHỈ SỐ ICON (ImageIndex) ---
        private void LoadTreeViewData()
        {
            tvDepartments.Nodes.Clear();

            // Node gốc (ImageIndex = 0)
            TreeNode rootNode = new TreeNode("Tập đoàn Công nghệ ABC", 0, 0) { Name = "Root" };

            // Khối Nhân sự (ImageIndex = 1)
            TreeNode pbNhanSu = new TreeNode("Khối Nhân Sự - Hành Chính", 1, 1) { Name = "PB_NS" };
            pbNhanSu.Nodes.Add(new TreeNode("Nhóm Tuyển Dụng & Đào Tạo", 2, 2) { Name = "N_NS_TD" });
            pbNhanSu.Nodes.Add(new TreeNode("Nhóm Lương Thưởng & C&B", 2, 2) { Name = "N_NS_CB" });

            // Khối Công nghệ (ImageIndex = 1)
            TreeNode pbKyThuat = new TreeNode("Khối Công Nghệ & Phần Mềm", 1, 1) { Name = "PB_CN" };
            pbKyThuat.Nodes.Add(new TreeNode("Nhóm Lập Trình Frontend", 2, 2) { Name = "N_CN_FE" });
            pbKyThuat.Nodes.Add(new TreeNode("Nhóm Lập Trình Backend", 2, 2) { Name = "N_CN_BE" });
            pbKyThuat.Nodes.Add(new TreeNode("Nhóm Kiểm Thử (QA/QC)", 2, 2) { Name = "N_CN_QA" });

            // Khối Kinh doanh (ImageIndex = 1)
            TreeNode pbKinhDoanh = new TreeNode("Khối Kinh Doanh & Marketing", 1, 1) { Name = "PB_KD" };
            pbKinhDoanh.Nodes.Add(new TreeNode("Nhóm Bán Hàng B2B", 2, 2) { Name = "N_KD_B2B" });
            pbKinhDoanh.Nodes.Add(new TreeNode("Nhóm Digital Marketing", 2, 2) { Name = "N_KD_MKT" });

            rootNode.Nodes.Add(pbNhanSu);
            rootNode.Nodes.Add(pbKyThuat);
            rootNode.Nodes.Add(pbKinhDoanh);

            tvDepartments.Nodes.Add(rootNode);
            tvDepartments.ExpandAll();

            tvDepartments.AfterSelect += tvDepartments_AfterSelect;
        }

        // --- 5. DANH SÁCH NHÂN VIÊN CÓ GÁN ICON NAM/NỮ ---
        private void LoadSampleEmployees()
        {
            // ImageIndex = 3 (Nam 👤), ImageIndex = 4 (Nữ 👩)
            listEmployees = new List<Employee>
            {
                new Employee { MaNV = "NV001", HoTen = "Phạm Thị Thúy Hằng", ChucVu = "Trưởng Nhóm Tuyển Dụng", NgayVaoLam = "01/06/2019", GroupNodeKey = "N_NS_TD", ImageIndex = 4 },
                new Employee { MaNV = "NV002", HoTen = "Lê Thị Mai Phương", ChucVu = "Chuyên Viên Tuyển Dụng", NgayVaoLam = "12/09/2021", GroupNodeKey = "N_NS_TD", ImageIndex = 4 },
                new Employee { MaNV = "NV003", HoTen = "Đặng Nhật Quang", ChucVu = "Chuyên Viên C&B", NgayVaoLam = "05/11/2020", GroupNodeKey = "N_NS_CB", ImageIndex = 3 },

                new Employee { MaNV = "NV004", HoTen = "Vũ Hoàng Gia Bảo", ChucVu = "Trưởng Nhóm Frontend", NgayVaoLam = "15/04/2020", GroupNodeKey = "N_CN_FE", ImageIndex = 3 },
                new Employee { MaNV = "NV005", HoTen = "Bùi Thị Khánh Linh", ChucVu = "Lập Trình Viên VueJS", NgayVaoLam = "10/10/2023", GroupNodeKey = "N_CN_FE", ImageIndex = 4 },
                new Employee { MaNV = "NV006", HoTen = "Ngô Thành Trung", ChucVu = "Trưởng Nhóm Backend", NgayVaoLam = "18/01/2019", GroupNodeKey = "N_CN_BE", ImageIndex = 3 },
                new Employee { MaNV = "NV007", HoTen = "Huỳnh Tấn Phát", ChucVu = "Chuyên Viên C# .NET", NgayVaoLam = "05/05/2021", GroupNodeKey = "N_CN_BE", ImageIndex = 3 },
                new Employee { MaNV = "NV008", HoTen = "Trịnh Thị Hồng Nhung", ChucVu = "Trưởng Nhóm QA/QC", NgayVaoLam = "11/11/2020", GroupNodeKey = "N_CN_QA", ImageIndex = 4 },

                new Employee { MaNV = "NV009", HoTen = "Lương Thái Bình", ChucVu = "Trưởng Phòng B2B", NgayVaoLam = "12/04/2019", GroupNodeKey = "N_KD_B2B", ImageIndex = 3 },
                new Employee { MaNV = "NV010", HoTen = "Dương Thủy Tiên", ChucVu = "Trưởng Nhóm Digital MKT", NgayVaoLam = "08/09/2020", GroupNodeKey = "N_KD_MKT", ImageIndex = 4 }
            };
        }

        // --- 6. HIỂN THỊ DỮ LIỆU KÈM ICON VÀO LISTVIEW ---
        private void tvDepartments_AfterSelect(object sender, TreeViewEventArgs e)
        {
            lsvEmployees.Items.Clear();

            List<string> validKeys = GetNodeKeysRecursive(e.Node);
            var filteredEmployees = listEmployees.Where(emp => validKeys.Contains(emp.GroupNodeKey)).ToList();

            foreach (var emp in filteredEmployees)
            {
                // Gán ImageIndex trực tiếp cho ListViewItem
                ListViewItem item = new ListViewItem(emp.MaNV, emp.ImageIndex);
                item.SubItems.Add(emp.HoTen);
                item.SubItems.Add(emp.ChucVu);
                item.SubItems.Add(emp.NgayVaoLam);

                lsvEmployees.Items.Add(item);
            }
        }

        private List<string> GetNodeKeysRecursive(TreeNode node)
        {
            List<string> keys = new List<string> { node.Name };
            foreach (TreeNode childNode in node.Nodes)
            {
                keys.AddRange(GetNodeKeysRecursive(childNode));
            }
            return keys;
        }

        private void cboViewMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboViewMode.SelectedItem == null) return;

            switch (cboViewMode.SelectedItem.ToString())
            {
                case "Details":
                    lsvEmployees.View = View.Details;
                    break;
                case "LargeIcon":
                    lsvEmployees.View = View.LargeIcon;
                    break;
                case "SmallIcon":
                    lsvEmployees.View = View.SmallIcon;
                    break;
                case "List":
                    lsvEmployees.View = View.List;
                    break;
                case "Tile":
                    lsvEmployees.View = View.Tile;
                    break;
            }
        }
    }
}