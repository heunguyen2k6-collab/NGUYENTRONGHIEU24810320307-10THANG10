namespace BAITAPSANG10THANG10
{
    public class ServiceItem
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

        public ServiceItem(string name, decimal price)
        {
            Name = name;
            Price = price;
        }

        // Đè hàm ToString để ListBox hiển thị đúng định dạng Tên - Giá
        public override string ToString()
        {
            return $"{Name} - {Price:N0} VNĐ";
        }
    }
}