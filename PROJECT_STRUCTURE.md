# TicketApi - Cấu trúc thư mục

Tài liệu này mô tả nhanh cấu trúc thư mục hiện tại của project `TicketApi`.

## 1. Tổng quan cây thư mục

```text
TicketApi/
├─ appsettings.Development.json
├─ appsettings.json
├─ Program.cs
├─ TicketApi.csproj
├─ TicketApi.http
├─ TicketApi.sln
├─ Properties/
│  └─ launchSettings.json
├─ Infrastructure/
│  ├─ Context/
│  │  ├─ ApplicationDbContext.cs
│  │  └─ ApplicationDbContextFactory.cs
│  └─ Seeding/
│     └─ DatabaseSeeder.cs
├─ Migrations/
│  ├─ 20260401081502_InitCreate.cs
│  ├─ 20260401081502_InitCreate.Designer.cs
│  └─ ApplicationDbContextModelSnapshot.cs
├─ Modules/
│  ├─ Identity/
│  │  ├─ DependencyInjection.cs
│  │  ├─ Authorization/
│  │  │  ├─ CheckResourceOwnerAttribute.cs
│  │  │  ├─ HasPermissionAttribute.cs
│  │  │  └─ PermissionConstants.cs
│  │  ├─ Configuration/
│  │  │  └─ JwtOptions.cs
│  │  ├─ Controllers/
│  │  │  ├─ AuthController.cs
│  │  │  └─ UserController.cs
│  │  ├─ DTOs/
│  │  │  ├─ AuthResponseDto.cs
│  │  │  ├─ LoginRequestDto.cs
│  │  │  ├─ PermissionDto.cs
│  │  │  ├─ RegisterRequestDto.cs
│  │  │  ├─ RoleDto.cs
│  │  │  ├─ UpdateUserDto.cs
│  │  │  ├─ UpdateUserStatusDto.cs
│  │  │  └─ UserDto.cs
│  │  ├─ Entities/
│  │  │  ├─ Permission.cs
│  │  │  ├─ Role.cs
│  │  │  ├─ RolePermission.cs
│  │  │  ├─ User.cs
│  │  │  ├─ UserPermission.cs
│  │  │  └─ UserRole.cs
│  │  ├─ Repositories/
│  │  │  ├─ Implementations/
│  │  │  └─ Interfaces/
│  │  └─ Services/
│  │     ├─ Implementations/
│  │     └─ Interfaces/
│  └─ Orders/
│     ├─ DependencyInjection.cs
│     ├─ Entities/
│     │  ├─ Concert.cs
│     │  ├─ Order.cs
│     │  └─ Ticket.cs
│     ├─ Enums/
│     │  ├─ OrderStatus.cs
│     │  ├─ TicketStatus.cs
│     │  └─ TicketZone.cs
│     └─ Repositories/
│        ├─ Implementations/
│        └─ Interfaces/
├─ bin/
│  └─ Debug/
│     └─ net10.0/
│        └─ ...
└─ obj/
   └─ ...
```

## 2. Ý nghĩa các khu vực chính

- `Program.cs`: Entry point, cấu hình DI, middleware và map endpoint.
- `appsettings*.json`: Cấu hình môi trường (connection string, logging, options).
- `Infrastructure/`: Tầng hạ tầng dùng chung, gồm DbContext và dữ liệu seed.
- `Migrations/`: Lịch sử migration của Entity Framework Core.
- `Modules/Identity/`: Module quản lý xác thực, phân quyền, user, role, permission.
- `Modules/Orders/`: Module nghiệp vụ đặt vé (concert, order, ticket).
- `Properties/launchSettings.json`: Cấu hình profile chạy local.
- `bin/`, `obj/`: Thư mục build artifacts tự sinh, không chỉnh sửa thủ công.

## 3. Gợi ý quy ước tổ chức hiện tại

- Mỗi module được tách theo feature (`Identity`, `Orders`).
- Trong mỗi module chia theo vai trò: `Controllers`, `DTOs`, `Entities`, `Repositories`, `Services`.
- `DependencyInjection.cs` ở từng module để gom đăng ký service/repository.
- Tầng `Infrastructure` chứa thành phần kỹ thuật liên quan dữ liệu dùng chung toàn hệ thống.
