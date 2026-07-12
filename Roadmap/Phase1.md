# Giai đoạn 1 — Chuẩn bị & Setup môi trường

**Cài đặt:**

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

  ```
  Microsoft.EntityFrameworkCore.SqlServer
  Microsoft.EntityFrameworkCore.Tools
  ```

  - Chọn `QuizSystem.WPF`và cài package:

  ```
  Microsoft.EntityFrameworkCore.SqlServer
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
