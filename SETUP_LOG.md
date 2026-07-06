# LMZQuiz — Nhật ký thiết lập môi trường theo từng giai đoạn

> File này dùng để tự tra cứu: mỗi giai đoạn cần cài gì, cấu hình gì, tạo folder/file gì.
> Chỉ ghi chi tiết **đến giai đoạn hiện tại** — các giai đoạn sau sẽ bổ sung dần khi làm tới.

---

## Toàn bộ Roadmap

- [x] **Giai đoạn 0** — Chuẩn bị & Setup môi trường
- [x] **Giai đoạn 1** — Thiết kế Database & Models
- [ ] **Giai đoạn 2** — Dựng khung project & EF Core _(đang làm)_
- [ ] **Giai đoạn 3** — Quản lý ngân hàng câu hỏi
- [ ] **Giai đoạn 4** — Tạo đề thi
- [ ] **Giai đoạn 5** — Giao diện làm bài
- [ ] **Giai đoạn 6** — Chấm điểm & lưu kết quả
- [ ] **Giai đoạn 7** — Thống kê & báo cáo
- [ ] **Giai đoạn 8** — Đóng gói & phát hành
- [ ] **Giai đoạn 9** — Nâng cấp lên Online _(tương lai)_

---

## ✅ Giai đoạn 0 — Chuẩn bị & Setup môi trường

**Đã cài / tạo:**

- Visual Studio 2022
- .NET SDK — dự án thực tế đang chạy **net10.0** (README ghi .NET 8 ban đầu, đã nâng cấp)
- Cài Solution `LMZQuiz.slnx` và trong đó tạo 4 project như sau:
  - `QuizSystem.Core` (Class Library)
  - `QuizSystem.Data` (Class Library)
  - `QuizSystem.Business` (Class Library)
  - `QuizSystem.WPF` (WPF App)
- Lưu ý:
  - Bạn được đặt tên solution cho riêng phần mềm bạn, `LMZQuiz.slnx` chỉ là tên kham khảo.
  - Gợi ý: hãy nhất quán khi đặt tên (vd: solution app là `ABC.slnx`, thì các 4 tầng còn lại, đặt là `ABC.Core`, ...)

**Cài nuget package:**

- Mở solution của bạn lại và vào `Packet Manager Console`, bấm vào `Default Project` và làm như sau:
  - Chọn `QuizSystem.Data` và cài package:

  ```Microsoft.EntityFrameworkCore.SqlServer
  Microsoft.EntityFrameworkCore.Tools
  ```

  - Chọn `QuizSystem.WPF`và cài package:

  ```Microsoft.EntityFrameworkCore.SqlServer
  CommunityToolkit.Mvvm
  ```

**Project Reference đã thiết lập (chiều phụ thuộc):**

```
Core   → (không tham chiếu ai)
Data   → Core
Business → Core, Data
WPF    → Core, Business   (KHÔNG tham chiếu Data)
```

---

## ✅ Giai đoạn 1 — Thiết kế Database & Models

**Folder/file đã tạo trong `QuizSystem.Core`:**

```
QuizSystem.Core/
├── Models/
│   ├── Category.cs
│   ├── Question.cs
│   ├── Answer.cs
│   ├── Exam.cs
│   ├── User.cs
│   ├── ExamResult.cs
│   └── Junctions/
│       └── ExamQuestion.cs      ← bảng trung gian N-N giữa Exam và Question
└── Enums/
    ├── QuestionType.cs          ← SingleChoice = 0, MultipleChoice = 1
    └── DifficultyLevel.cs       ← Easy = 0, Medium = 1, Hard = 2
```

**Quy tắc đã thống nhất và áp dụng xuyên suốt:**

- `Id` → khóa chính. `TênClassKhác + Id` → khóa ngoại.
- Enum lưu database → luôn gán số tường minh (`= 0, = 1, ...`), tránh lỗi khi thêm giá trị mới về sau.
- Danh sách cố định không đổi (độ khó, loại câu hỏi) → dùng `enum`. Danh sách người dùng tự thêm được (môn học) → tách bảng riêng.
- Dữ liệu tính ra được từ bảng khác (số câu hỏi trong đề) → không lưu cột riêng.
- Mỗi quan hệ 1-nhiều có đủ Navigation Property 2 chiều (`Category.Questions` ↔ `Question.Category`).

**Quan hệ đã hoàn thiện:**

```
Category 1 --- * Question
Category 1 --- * Exam
Question 1 --- * Answer
Exam     1 --- * ExamQuestion
Question 1 --- * ExamQuestion
Exam     1 --- * ExamResult
User     1 --- * ExamResult
```

**Kết quả:** Build solution thành công (4/4 project, 0 lỗi).

---

## ⏳ Giai đoạn 2 — Dựng khung project & EF Core _(đang làm)_

**Thông tin SQL Server đã xác nhận:**

- Server Name: `.\SQLEXPRESS` hoặc tên server máy bạn
- Authentication: Windows Authentication (Trusted Connection)
- Connection string dự kiến:
  ```
  Server=.\SQLEXPRESS;Database=*****;Trusted_Connection=True;TrustServerCertificate=True;
  ```

**Việc cần làm trong giai đoạn này:**

- [ ] Tạo `QuizSystem.Data/AppDbContext.cs` — kế thừa `DbContext`, khai báo 7 `DbSet<>`
- [ ] Thiết lập nơi lưu connection string (ví dụ `appsettings.json` trong `QuizSystem.WPF`)
- [ ] Cài `Add-Migration InitialCreate` (sinh migration đầu tiên)
- [ ] Chạy `Update-Database` (tạo database + 7 bảng thật trong SQL Server)
- [ ] Mở SSMS kiểm tra lại 7 bảng đã tạo đúng như thiết kế Giai đoạn 1

**NuGet package cần cài thêm (nếu chưa có), sẽ xác nhận khi tới bước dùng đến:**

- Có thể cần `Microsoft.Extensions.Configuration.Json` (đọc file `appsettings.json`) — chưa xác nhận, sẽ cập nhật khi tới bước đó.

---

_(Các giai đoạn 3 → 9 sẽ được bổ sung chi tiết khi thực hiện tới, theo đúng cấu trúc mục ở trên.)_
