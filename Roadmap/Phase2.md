# Giai đoạn 2 — Thiết kế Database & Models

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
