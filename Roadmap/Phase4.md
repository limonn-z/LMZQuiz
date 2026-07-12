# ⏳ Giai đoạn 4 — Quản lý ngân hàng câu hỏi _(đang làm)_

**Được chia theo 4 bước, đi từ trong ra ngoài (đúng chiều kiến trúc):**

| Bước | Tầng       | Ví như        | Việc làm                                         |
| ---- | ---------- | ------------- | ------------------------------------------------ |
| 4.1  | `Core`     | Giấy hợp đồng | Viết Interface cho Repository                    |
| 4.2  | `Data`     | Ký hợp đồng   | Viết Repository (code thật, dùng `AppDbContext`) |
| 4.3  | `Business` | Gác cổng      | Viết Service (luật nghiệp vụ)                    |
| 4.4  | `WPF`      |               | Viết ViewModel + View (giao diện thật)           |

### Bước 4.1 — Interface

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

=> Xong rồi thì CTRL + SHIFT + B xem ổn không.

### Bước 4.2 — Repository

Tạo `QuizSystem.Data/Repositories/`, mỗi file "ký hợp đồng" đúng interface tương ứng ở `bước 4.1`:

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
          Task RemoveCategoryAsync(Category category);                 // Xóa môn bằng id
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

            public async Task RemoveCategoryAsync(Category category)
            {
                var categoryObj = await _context.Categories.FindAsync(category.Id);
                if (categoryObj == null)
                {
                    throw new Exception($"Category with ID {category.Id} not found for removing!");
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
                return categoryObj;
            }
        }
    }

  ```

  - Ghi nhớ:
    - 1 interface có bao nhiêu tác vụ thì repository kí với nó phải thực hiện bấy nhiêu. Nếu không sẽ bị lỗi!
    - Vì giao tiếp và làm việc với database `SQL Server` nên tác vụ cần phải có thời gian để truy vấn và phản hồi. Vì vậy, `async + Task` luôn luôn đi đôi.
    - Tất cả những hàm như là `Add(), Remove(), ..` chỉ là đánh dấu và chưa tác động đến database. Nên muốn lưu lên database phải dùng lệnh `await _context.SaveChangesAsync();`, trong đó `await` là đợi chờ.

=> Xong rồi thì CTRL + SHIFT + B xem ổn không.

### Bước 4.3 — Service

Tạo `QuizSystem.Business/Services/`, mỗi Service chỉ nói chuyện qua `I{Tên_bảng}Repository`.

- Nhiệm vụ: đóng vai "người gác cổng" — kiểm tra dữ liệu có hợp lệ theo luật nghiệp vụ thật hay không (Data chỉ biết lưu/xóa/sửa thuần túy, không có quyền quyết định cái gì là "hợp lệ"). Nếu dữ liệu sai luật thì chặn lại bằng Exception rõ nghĩa, không cho lọt xuống Data.

**Khuôn mẫu áp dụng (chỉ đổi tên Model cho khớp)**

```csharp
public class {Ten}Service({Cac*Repository_can_tiem})
{
  private readonly I{Ten}Repository *{ten}Repository = {ten}Repository;

  public async Task<{Model}> Add{Ten}Async({Model} new{Ten})
  {
      Validate{Ten}(new{Ten});
      return await _{ten}Repository.Add{Ten}Async(new{Ten});
  }

  public async Task Edit{Ten}Async({Model} updated{Ten})
  {
      Validate{Ten}(updated{Ten});
      await _{ten}Repository.Edit{Ten}Async(updated{Ten});
  }

  public async Task Remove{Ten}Async({Model} {ten})
  {
      if ({ten} == null)
          throw new ArgumentNullException(nameof({ten}), "{Ten} cannot be null!");
      await _{ten}Repository.Remove{Ten}Async({ten});
  }

  public async Task<{Model}> Get{Ten}ByIdAsync(int id)
  {
      if (id <= 0)
          throw new ArgumentException("Id must be greater than 0!", nameof(id));
      return await _{ten}Repository.Get{Ten}ByIdAsync(id)
          ?? throw new KeyNotFoundException($"{Ten} with Id {id} not found!");
  }

  public async Task<List<{Model}>> GetAll{Ten}sAsync()
  {
      return await _{ten}Repository.GetAll{Ten}sAsync();
  }

  private static void ValidateCategory(Category category)
  {
    if (category == null)
      throw new ArgumentNullException(nameof(category), "Category cannot be null!");
    if (string.IsNullOrWhiteSpace(category.Name))
      throw new ArgumentException("Category name must not empty!", nameof(category));
  }
}

```

- Ghi nhớ:
  - Bảng nào giữ khóa ngoại thì bảng đó phải tiêm repository của bảng bị trỏ tới để hỏi thẳng DB xem khóa ngoại đó có tồn tại thật không.

=> Xong rồi thì CTRL + SHIFT + B xem ổn không.

### Bước 4.4 — WPF (ViewModel + View)

Viết `ViewModel` (gọi qua Service ở 4.3) và `View` (XAML) thật cho `Category`, `Question`, `Answer`, `User`.

- Nhiệm vụ của nó là: điều phối giữa View (giao diện) và Service (nghiệp vụ) — lấy dữ liệu, expose ra cho View bind, nhận lệnh (Command) từ người dùng rồi gọi đúng Service tương ứng. **Không chứa luật nghiệp vụ** (luật đã nằm hết ở Service, 4.3 làm rồi).

- Trước khi viết bất kỳ ViewModel nào, phải đăng ký DI cho Repository + Service trước — nếu không, ViewModel không có cách nào lấy được Service qua constructor.

  ### Bước 4.4.1: Đăng kí DI (_đang làm_)

  Vào `QuizSystem.WPF/app.xaml.cs theo khung như sau`:

  ```csharp
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.Configuration;
  using Microsoft.Extensions.DependencyInjection;
  using Microsoft.Extensions.Hosting;
  using QuizSystem.Data;
  using System.Windows;
  using QuizSystem.Business.Services;
  using QuizSystem.Core.Repositories;
  using QuizSystem.Data.Repositories;

  namespace QuizSystem.WPF
  {
      public partial class App : Application
      {
          public static IHost AppHost { get; private set; } = null!;

          protected override void OnStartup(StartupEventArgs e)
          {
              AppHost = Host.CreateDefaultBuilder()
                  .ConfigureAppConfiguration(config =>
                  {
                      config.AddJsonFile("appsettings.json", optional: false);
                  })
                  .ConfigureServices((context, services) =>
                  {
                      var connectionString = context.Configuration.GetConnectionString("DefaultConnection");

                      // Từ giờ, mỗi khi ai cần AppDbContext, tự động tạo với connection string này
                      services.AddDbContext(options => options.UseSqlServer(connectionString));

                      // Repository: đăng ký khớp Interface (Core) ↔ Class thật (Data)
                      services.AddScoped();
                      services.AddScoped();
                      services.AddScoped();
                      services.AddScoped();

                      // Service: không có Interface, đăng ký thẳng class thật
                      services.AddScoped();
                      services.AddScoped();
                      services.AddScoped();
                      services.AddScoped();
                  })
                  .Build();

              base.OnStartup(e);
          }
      }
  }

  ```

  - Ví dụ:

  ```csharp
  protected override void OnStartup(StartupEventArgs e)
  {
      AppHost = Host.CreateDefaultBuilder()

          .ConfigureAppConfiguration(config =>
          {
              config.AddJsonFile("appsettings.json", optional: false);
          })

          .ConfigureServices((context, services) =>
          {
              var connectionString = context.Configuration.GetConnectionString("DefaultConnection");

              // Từ giờ, mỗi khi ai cần AppDbContext, tự động tạo với connection string này
              services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

              // Hễ ai cần interface(core) thì đưa họ một repository(data) thật => data giao tiếp với core
              services.AddScoped<ICategoryRepository, CategoryRepository>();
              services.AddScoped<IQuestionRepository, QuestionRepository>();
              services.AddScoped<IAnswerRepository, AnswerRepository>();
              services.AddScoped<IUserRepository, UserRepository>();

              // Hễ ai cần service(business) thì tạo cho họ 1 cái
              // Khi tạo, tự động nhìn vào constructor của service class cần gì (chính là interface)
              // DI sẽ tự nối class service và interface giao tiếp với nhau mà ko cần qua data
              services.AddScoped<CategoryService>();
              services.AddScoped<QuestionService>();
              services.AddScoped<AnswerService>();
              services.AddScoped<UserService>();
          })

          .Build();

      base.OnStartup(e);
  }
  ```

  ### Bước 4.4.2: Viết ViewModel cho từng màn hình

  Viết ViewModel cho từng màn hình, theo đúng thứ tự đã làm ở 3.3 (từ đơn giản → phức tạp): CategoryViewModel → QuestionViewModel → AnswerViewModel (thường gộp chung màn với Question) → UserViewModel. Mỗi ViewModel chỉ gọi qua Service (đã xong ở 3.3), expose data + lệnh (Command) cho View dùng, không chứa luật nghiệp vụ (luật đã nằm hết bên Service rồi).

  ### Bước 4.4.3: Viết View (XAML) khớp với từng ViewModel

  Viết View (XAML) khớp với từng ViewModel — giao diện thật để nhập/xem/sửa/xóa dữ liệu.
