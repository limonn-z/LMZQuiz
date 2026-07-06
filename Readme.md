# 🎓 LMZQuiz — Desktop Quiz Management System

> A powerful desktop application for creating, managing, and taking quizzes.  
> Built with C# WPF and SQL Server, designed with a layered architecture for future online scalability.

---

## 📋 Mục Lục

- [Giới thiệu](#-giới-thiệu)
- [Tính năng](#-tính-năng)
- [Công nghệ sử dụng](#-công-nghệ-sử-dụng)
- [Kiến trúc hệ thống](#-kiến-trúc-hệ-thống)
- [Cấu trúc project](#-cấu-trúc-project)
- [Yêu cầu hệ thống](#-yêu-cầu-hệ-thống)
- [Hướng dẫn cài đặt](#-hướng-dẫn-cài-đặt)
- [Roadmap](#-roadmap)
- [Tác giả](#-tác-giả)

---

## 🎯 Giới Thiệu

**LMZQuiz** là phần mềm quản lý và thi trắc nghiệm chạy trên desktop (Windows), cho phép người dùng:
- Xây dựng ngân hàng câu hỏi theo chủ đề và độ khó
- Tạo đề thi tự động hoặc thủ công
- Làm bài thi có đếm giờ
- Xem thống kê kết quả và tiến bộ theo thời gian

Dự án được xây dựng với mục tiêu **học tập và phát triển cá nhân**, đồng thời thiết kế sẵn kiến trúc để **dễ dàng nâng cấp lên hệ thống online** trong tương lai.

---

## ✨ Tính Năng

### Quản lý câu hỏi
- [ ] Thêm / sửa / xóa câu hỏi
- [ ] Phân loại theo chủ đề và độ khó
- [ ] Hỗ trợ 1 đáp án đúng hoặc nhiều đáp án đúng
- [ ] Import câu hỏi từ file Excel
- [ ] Tìm kiếm và lọc câu hỏi

### Quản lý đề thi
- [ ] Tạo đề thủ công (chọn từng câu)
- [ ] Tạo đề tự động (random theo chủ đề + độ khó)
- [ ] Cấu hình thời gian, số câu, thang điểm
- [ ] Trộn thứ tự câu hỏi và đáp án

### Làm bài thi
- [ ] Giao diện làm bài trực quan
- [ ] Đồng hồ đếm ngược
- [ ] Đánh dấu câu chưa làm / đang phân vân
- [ ] Tự động nộp bài khi hết giờ

### Kết quả & Thống kê
- [ ] Chấm điểm và xem đáp án ngay sau khi nộp
- [ ] Lưu lịch sử toàn bộ các lần làm bài
- [ ] Biểu đồ tiến bộ theo thời gian
- [ ] Thống kê tỷ lệ đúng/sai theo từng chủ đề
- [ ] Xuất kết quả ra Excel / PDF

---

## 🛠 Công Nghệ Sử Dụng

| Thành phần | Công nghệ | Phiên bản |
|---|---|---|
| Ngôn ngữ | C# | 12 |
| Framework | .NET | 8 |
| Giao diện | WPF | .NET 8 |
| Database | SQL Server Express | 2022 |
| ORM | Entity Framework Core | 8.x |
| MVVM Toolkit | CommunityToolkit.Mvvm | 8.x |
| Biểu đồ | LiveCharts2 | *(sắp dùng)* |
| Xuất Excel | ClosedXML | *(sắp dùng)* |
| Xuất PDF | QuestPDF | *(sắp dùng)* |

---

## 🏗 Kiến Trúc Hệ Thống

Dự án áp dụng kiến trúc **N-Layer (phân tầng)** kết hợp pattern **MVVM** ở tầng giao diện:

```
┌─────────────────────────────┐
│      QuizSystem.WPF         │  ← Giao diện (Views + ViewModels)
│       [MVVM Pattern]        │
└──────────────┬──────────────┘
               │ gọi
┌──────────────▼──────────────┐
│    QuizSystem.Business      │  ← Xử lý nghiệp vụ (Logic, Validate)
└──────────────┬──────────────┘
               │ gọi
┌──────────────▼──────────────┐
│      QuizSystem.Data        │  ← Truy cập dữ liệu (EF Core, Repository)
└──────────────┬──────────────┘
               │ dùng
┌──────────────▼──────────────┐
│      QuizSystem.Core        │  ← Định nghĩa Models, Interfaces
└─────────────────────────────┘
```

> **Lợi ích:** Khi nâng cấp lên online, chỉ cần thêm project `QuizSystem.API` và tái sử dụng toàn bộ `Core`, `Data`, `Business` mà không cần viết lại.

---

## 📁 Cấu Trúc Project

```
LMZQuiz/                             ← Repo GitHub
│
├── LMZQuiz.sln
├── README.md
│
├── src/
│   │
│   ├── QuizSystem.Core/             # Class Library — Tầng nền tảng
│   │   ├── Models/                  # Các class thực thể
│   │   │   ├── Question.cs
│   │   │   ├── Answer.cs
│   │   │   ├── Exam.cs
│   │   │   ├── ExamResult.cs
│   │   │   ├── Category.cs
│   │   │   └── User.cs
│   │   ├── Interfaces/              # Các interface (hợp đồng)
│   │   │   ├── IQuestionRepository.cs
│   │   │   └── IExamRepository.cs
│   │   ├── Enums/                   # Các kiểu liệt kê
│   │   │   ├── DifficultyLevel.cs  
│   │   │   └── QuestionType.cs
│   │   └── Constants/               # Các hằng số dùng chung
│   │       └── AppConstants.cs
│   │
│   ├── QuizSystem.Data/             # Class Library — Tầng dữ liệu
│   │   ├── QuizDbContext.cs         # EF Core DbContext
│   │   ├── Migrations/              # EF Core Migrations
│   │   └── Repositories/           # Triển khai Repository Pattern
│   │       ├── QuestionRepository.cs
│   │       └── ExamRepository.cs
│   │
│   ├── QuizSystem.Business/         # Class Library — Tầng nghiệp vụ
│   │   └── Services/
│   │       ├── ExamService.cs       # Logic tạo đề, tính điểm
│   │       └── QuestionService.cs   # Logic quản lý câu hỏi
│   │
│   └── QuizSystem.WPF/              # WPF Application — Tầng giao diện
│       ├── Views/                   # Các màn hình giao diện
│       │   ├── MainWindow.xaml
│       │   ├── QuestionManagerView.xaml
│       │   ├── ExamView.xaml
│       │   └── ResultView.xaml
│       ├── ViewModels/              # Logic giao diện (MVVM)
│       │   ├── MainViewModel.cs
│       │   ├── QuestionManagerViewModel.cs
│       │   └── ExamViewModel.cs
│       └── App.xaml
│
├── tests/                           # Unit Tests (sắp có)
│   └── QuizSystem.Tests/
│
└── docs/                            # Tài liệu (sắp có)
```
---

## 💻 Yêu Cầu Hệ Thống

| Yêu cầu | Chi tiết |
|---|---|
| Hệ điều hành | Windows 10 / 11 (64-bit) |
| .NET Runtime | .NET 8 trở lên |
| SQL Server | SQL Server 2022 Express trở lên |
| RAM | Tối thiểu 4GB |
| Dung lượng | ~200MB (chưa tính database) |

---

## 🚀 Hướng Dẫn Cài Đặt

### 1. Yêu cầu cài đặt trước

- [Visual Studio 2022](https://visualstudio.microsoft.com/) (Community hoặc cao hơn)
  - Workload: **.NET desktop development**
- [SQL Server 2022 Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [SQL Server Management Studio (SSMS)](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)
- [Git](https://git-scm.com/)

### 2. Clone project

```bash
git clone https://github.com/limonn-z/LMZQuiz.git
cd LMZQuiz
```

### 3. Cấu hình Database

Mở file `QuizSystem.Data/appsettings.json` và cập nhật connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=QuizSystemDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### 4. Chạy Migration để tạo Database

Mở **Package Manager Console** trong Visual Studio, chọn project `QuizSystem.Data`:

```
Update-Database
```

### 5. Build và chạy

- Đặt `QuizSystem.WPF` làm **Startup Project**
- Nhấn **F5** để chạy

---

## 🗺 Roadmap

- [x] **Giai đoạn 0** — Chuẩn bị & Setup môi trường
- [x] **Giai đoạn 1** — Thiết kế Database & Models & Model Relationships
- [ ] **Giai đoạn 2** — Dựng khung project & EF Core
- [ ] **Giai đoạn 3** — Quản lý ngân hàng câu hỏi
- [ ] **Giai đoạn 4** — Tạo đề thi
- [ ] **Giai đoạn 5** — Giao diện làm bài
- [ ] **Giai đoạn 6** — Chấm điểm & lưu kết quả
- [ ] **Giai đoạn 7** — Thống kê & báo cáo
- [ ] **Giai đoạn 8** — Đóng gói & phát hành
- [ ] **Giai đoạn 9** — Nâng cấp lên Online *(tương lai)*

---

## 👤 Tác Giả

**[Tên của bạn]**
- GitHub: [@limonn-z](https://github.com/limonn-z)

---

> 📌 *Dự án đang trong quá trình phát triển. Tính năng có thể thay đổi theo từng giai đoạn.*
