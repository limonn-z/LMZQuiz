# LMZQuiz — Nhật ký thiết lập môi trường theo từng giai đoạn

> File này dùng để tự tra cứu: mỗi giai đoạn cần cài gì, cấu hình gì, tạo folder/file gì.
> Chỉ ghi chi tiết **đến giai đoạn hiện tại** — các giai đoạn sau sẽ bổ sung dần khi làm tới.

---

## Toàn bộ Roadmap

- [x] **Giai đoạn 0** — Chuẩn bị & Setup môi trường
- [x] **Giai đoạn 1** — Thiết kế Database & Models
- [x] **Giai đoạn 2** — Dựng khung project & EF Core
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

**Lưu ý về mặt thiết kế:**

- Các class xương sống của app phải đặt trong folder `Models/` để quản lý, còn về mặt thiết kế thì bạn phải tự thiết kế class theo phần mềm của bạn.
- Các class phía trên chỉ là xương sống của phần mềm hệ thống trắc nghiệm ! Nếu bạn cũng muốn làm thì đây là nơi tuyệt nhất để bạn kham khảo.

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

## **Cuối cùng** Build solution (Ctrl + shift + B)

## ⏳ Giai đoạn 2 — Dựng khung project & EF Core (Quan trọng)

**Thông tin SQL Server đã xác nhận:**

- Server Name: `.\SQLEXPRESS` hoặc tên server máy bạn
- Authentication: Windows Authentication (Trusted Connection)
- Connection string dự kiến:

  ```
  Server=.\SQLEXPRESS;Database=*****;Trusted_Connection=True;TrustServerCertificate=True;
  ```

  - 'Databate' bạn sẽ tự đặt để lưu trữ dữ liệu của bạn

**Việc làm trong giai đoạn này (khuyến nghị dùng A.I sẽ tối ưu hóa hơn):**

- [x] Tạo `QuizSystem.Data/AppDbContext.cs` — kế thừa `DbContext`, khai báo n `DbSet<T>` có trong `QuizSystem.Core` ('T' ở đây là các class xương sống khi thiết kế)
- [x] Tạo `QuizSystem.WPF/appsettings.json` — chứa connection string (đặt "Copy to Output Directory" thành "Copy if newer" trong mục property)
- [x] Cài thêm NuGet vào `QuizSystem.WPF: Microsoft.Extensions.Hosting`, `Microsoft.Extensions.Configuration.Json`
- [x] Thiết lập nơi lưu connection string (ví dụ `appsettings.json` trong `QuizSystem.WPF`)
- [x] Cấu hình Dependency Injection trong `App.xaml.cs` (tạo `AppHost`, đăng ký `AppDbContext` với connection string đọc từ `appsettings.json`)
- [x] Mở "Package manager console", mục "default project" chọn `QuizSystem.Data`:
  - Cài `Add-Migration InitialCreate` (sinh migration đầu tiên do EF core quản lý)
  - Cần cài thêm NuGet `Microsoft.EntityFrameworkCore.Design` vào `QuizSystem.WPF` (Startup Project) — Nếu thiếu nó thì `Add-Migration` báo lỗi ngay, không chạy được
  - Chạy `Update-Database` (tạo database + class bảng thật trong SQL Server)
    - Nếu chạy thất bại với lỗi **"may cause cycles or multiple cascade paths"** — nghĩa là 1 bảng trung gian có 2 khóa ngoại, mà cả 2 đều dẫn ngược về chung 1 bảng tổ tiên (ví dụ `ExamQuestion` có `ExamId` và `QuestionId`, cả 2 cùng dẫn về `Category`). SQL Server không cho phép cả 2 đường cùng tự xóa theo (cascade).

    ```
    Bảng trung gian     Bảng tổ tiên
          |                 |
          V                 V
    ExamQuestion ← Exam ← Category
    ExamQuestion ← Question ← Category
    ```

  - Quy tắc tổng quát tìm bảng tổ tiên trùng nhau:

    ```
    _ Với 2 khóa ngoại của bảng trung gian bị xung đột, mỗi khóa ngoại truy ngược liên tục (theo hướng
    "khóa ngoại → bảng cha → khóa ngoại của bảng cha đó → ...") cho tới khi hết đường đi (gặp bảng không
    còn khóa ngoại).
    Nếu 2 đường truy ngược đó gặp lại cùng 1 bảng bất kỳ (không nhất thiết phải là bảng gốc cuối cùng,
    chỉ cần trùng 1 điểm) → có xung đột, cần chặn Restrict ở 1 trong 2.

    ```

  - Cách sửa: trong `AppDbContext.cs`, thêm đoạn sau — chỉ cần đổi tên cho khớp bảng/cột thật của bạn:

  ```csharp
        // (2 khóa ngoại không cùng dẫn về 1 tổ tiên chung) thì không cần thêm code này
        // Nếu có thì thêm đúng khung code này, và đổi 4 chỗ theo hướng dẫn dưới đây:

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<T>()                    // Chỗ 1: <T>
                .HasOne(x => x.TBangMuonChan)           // Chỗ 2: TBangMuonChan
                .WithMany(y => y.TBangTrungGianList)    // Chỗ 3: TBangTrungGianList
                .HasForeignKey(x => x.TBangMuonChanId)  // Chỗ 4: TBangMuonChanId
                .OnDelete(DeleteBehavior.Restrict);
        }

  ```

  - Ví dụ dễ hiểu, trước tiên đây là bảng trung gian đang bị lỗi (`ExamQuestion.cs`), các dòng liên quan được đánh dấu:

  ```csharp
          public class ExamQuestion
          {
              public int Id { get; set; }
              public int ExamId { get; set; }
              public int QuestionId { get; set; }          // ← sẽ dùng ở Chỗ 4 (khóa ngoại)

              public Exam Exam { get; set; } = null!;
              public Question Question { get; set; } = null!;   // ← sẽ dùng ở Chỗ 2 (navigation 1-object)
          }
  ```

  - Và bên `Question.cs` có sẵn dòng này:

  ```csharp
          public List ExamQuestions { get; set; } = new List();  // ← sẽ dùng ở Chỗ 3 (navigation List<>)
  ```

  - Kết quả:

  ```csharp
          /*
          _ Ta nhận thấy ở trên có 2 khóa ngoại là ExamId và QuestionId, nếu chọn QuestionId thì tương ứng sẽ là Question như trên.
          _ Sau đó sẽ qua Question.cs để dùng đúng có sẵn dòng navigation properties <List>
          _ Áp dụng như code dưới đây thì sẽ phá vỡ xung đột giữa Exam và Question
          */

          protected override void OnModelCreating(ModelBuilder modelBuilder)
          {
              modelBuilder.Entity<ExamQuestion>()
                  .HasOne(eq => eq.Question)
                  .WithMany(q => q.ExamQuestions)
                  .HasForeignKey(eq => eq.QuestionId)
                  .OnDelete(DeleteBehavior.Restrict);
          }
  ```

- Sau khi thêm, trong "package manager console" → chọn `Data`, chạy lại theo thứ tự: `Remove-Migration` → `Add-Migration InitialCreate` → `Update-Database` xem còn lỗi không.

- [x] Mở SSMS kiểm tra lại các bảng đã tạo đúng như thiết kế Giai đoạn 1 chưa.

---

## ⏳ Giai đoạn 3 — Quản lý ngân hàng câu hỏi _(đang làm)_

**Được chia theo 4 bước, đi từ trong ra ngoài (đúng chiều kiến trúc):**

| Bước | Tầng       | Ví như        | Việc làm                                         |
| ---- | ---------- | ------------- | ------------------------------------------------ |
| 3.1  | `Core`     | Giấy hợp đồng | Viết Interface cho Repository                    |
| 3.2  | `Data`     | Ký hợp đồng   | Viết Repository (code thật, dùng `AppDbContext`) |
| 3.3  | `Business` |               | Viết Service (luật nghiệp vụ)                    |
| 3.4  | `WPF`      |               | Viết ViewModel + View (giao diện thật)           |

### Bước 3.1 — Interface

Tạo `QuizSystem.Core/Repositories/`, mỗi bảng trong `Model` là 1 file interface.

- Nhiệm vụ của nó là: làm "tờ giấy hợp đồng" - quy định Repository (tầng `Data`) phải có khả năng làm được gì (vd: Thêm, sửa, xóa, lấy 1, lấy tất cả, ...), nhưng không viết code thật bên trong. Business chỉ cần biết tới interface này để gọi, nên tầng `Data` dù có thay đổi code hoặc sử dụng tool khác như nào vẫn không ảnh hưởng gì.

- Lưu ý:
  - Nếu là bảng trung gian `Junction` (ví dụ: `ExamQuestion`) thì ko làm interface riêng (để hiểu rõ hơn thì bạn nên tìm hiểu).
  - `Model` có bao nhiêu bảng thì làm bấy nhiêu Interface.

**Khuôn mẫu áp dụng (chỉ đổi tên Model cho khớp):**

```csharp
public interface I{Tên_bảng}Repository
{
  // Kiểu 1: Tác vụ không trả về (giống void)
  Task {Tên_hàm}Async();

  // Kiểu 2: Tác vụ trả về và có tham số
  Task<Kiểu_trả_về> {Tên_hàm}Async(Tham_số);
}
```

- Ví dụ:

  ```csharp
  public interface IAnswerRepository
  {
    // Xóa đáp án bằng id của câu đó
    Task RemoveAnswerAsync(int id);

    // Thêm đáp án mới và trả về object đã có Id
    Task<Answer> AddAnswerAsync(Answer newAnswer);
  }
  ```

### Bước 3.2 — Repository

Tạo `QuizSystem.Data/Repositories/`, mỗi file "ký hợp đồng" đúng interface tương ứng ở `bước 3.1`:

- Ví dụ kham khảo:
  - Ở `QuizSystem.Core/Repositories/ICategoryRepository.cs`:

  ```csharp
  using QuizSystem.Core.Models;

  namespace QuizSystem.Core.Repositories
  {
      public interface ICategoryRepository
      {
          Task<Category> AddCategoryAsync(Category newCategory);       // Thêm môn mới
          Task EditCategoryAsync(Category updatedCategory);            // Chỉnh sửa môn
          Task RemoveCategoryByIdAsync(int id);                        // Xóa môn bằng id
          Task<Category> GetCategoryByIdAsync(int id);                 // Lấy môn theo Id
          Task<List<Category>> GetAllCategoriesAsync();                // Lấy tất cả môn
      }
  }

  ```

  - Tạo ``QuizSystem.Core/Repositories/ICategoryRepository.cs`:

  ```csharp
    using Microsoft.EntityFrameworkCore;
    using QuizSystem.Core.Models;
    using QuizSystem.Core.Repositories;

    namespace QuizSystem.Data.Repositories
    {
        public class CategoryRepository : ICategoryRepository
        {
            // Biến tạo 1 lần duy nhất, cất vào _context, rồi các hàm khác dùng lại
            private readonly AppDbContext _context;

            // Hàm khởi tạo nhận vào một đối tượng AppDbContext để tương tác với cơ sở dữ liệu.
            public CategoryRepository(AppDbContext context)
            {
                _context = context;
            }

            // Hoàn thiện phần định nghĩa của các phương thức trong ICategoryRepository
            public async Task<Category> AddCategoryAsync(Category newCategory)
            {
                await _context.Categories.AddAsync(newCategory);
                await _context.SaveChangesAsync();
                return newCategory;
            }

            public async Task EditCategoryAsync(Category updaterCategory)
            {
                var categoryObj = await _context.Categories.FindAsync(updaterCategory.Id);
                if (categoryObj == null)
                {
                    throw new Exception($"Category with ID {updaterCategory.Id} not found for editing!");
                }
                categoryObj.Name = updaterCategory.Name;
                categoryObj.Description = updaterCategory.Description;
                await _context.SaveChangesAsync();
            }

            public async Task RemoveCategoryByIdAsync(int categoryId)
            {
                var categoryObj = await _context.Categories.FindAsync(categoryId);
                if (categoryObj == null)
                {
                    throw new Exception($"Category with ID {categoryId} not found for removing!");
                }
                _context.Categories.Remove(categoryObj);
                await _context.SaveChangesAsync();
            }

            public async Task<List<Category>> GetAllCategoriesAsync()
            {
                IQueryable<Category> query = _context.Categories;
                List<Category> list = await query.ToListAsync();
                return list;
            }

            public async Task<Category> GetCategoryByIdAsync(int categoryId)
            {
                var categoryObj = await _context.Categories.FindAsync(categoryId);
                if (categoryObj == null) throw new Exception($"Category with ID {categoryId} not found for getting!");
                return categoryObj;
            }
        }
    }

  ```

  - Ghi nhớ:
    - 1 interface có bao nhiêu tác vụ thì repository kí với nó phải thực hiện bấy nhiêu. Nếu không sẽ bị lỗi!
    - Vì giao tiếp và làm việc với database `SQL Server` nên tác vụ cần phải có thời gian để truy vấn và phản hồi. Vì vậy, `async + Task` luôn luôn đi đôi.
    - Tất cả những hàm như là `Add(), Remove(), ..` chỉ là đánh dấu và chưa tác động đến database. Nên muốn lưu lên database phải dùng lệnh `await _context.SaveChangesAsync();`, trong đó `await` là đợi chờ.

---

_(Các giai đoạn 4 → 9 sẽ được bổ sung chi tiết khi thực hiện tới, theo đúng cấu trúc mục ở trên.)_
