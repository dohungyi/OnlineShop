using System.ComponentModel;

namespace SharedKernel.Domain;


public enum AuditAction
{
    [Description("Xem")]
    View,
    [Description("Thêm")]
    Insert,
    [Description("Sửa")]
    Update,
    [Description("Xóa")]
    Delete,
    [Description("Di chuyển")]
    Move,
    [Description("Tải lên")]
    Upload,
    [Description("Tải xuống")]
    Download,
    [Description("Tạo mật khẩu")]
    SetPassword,
    [Description("Đổi mật khẩu")]
    ChangePassword,
    [Description("Đăng nhập")]
    SignIn,
    [Description("Đăng xuất")]
    SignOut,
    [Description("Cập nhật")]
    Avatar,
    [Description("Cập nhật")]
    AppFavourite,
}
