namespace QLTS.Domain.Entities.Identity;

public class CompanyConfig
{
    public virtual int ComID { get; set; }
    public virtual decimal NhacHetHanHopDong { get; set; } //Nhắc hết hạn hợp đồng trước
    public virtual decimal GioLamViecChuan { get; set; } = 8; //Giờ làm việc/ngày, mặc định 8h
    public virtual TimeSpan? GioLamTu { get; set; }
    public virtual TimeSpan? GioLamDen { get; set; }
    public virtual decimal GioCongThang { get; set; } = 192;
    public virtual decimal GioCongNam { get; set; } = 2000;
    public virtual decimal NgayCongThang { get; set; } = 24;
    public virtual int NgayNghiChuan { get; set; } = 12; // Ngày nghỉ phép hưởng lương tiêu chuẩn năm

}
