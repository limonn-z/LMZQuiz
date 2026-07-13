# Đã học

- ✅ **Bài 1** — `Window`
- ✅ **Bài 2** — `Grid` (RowDefinitions, Height Auto/\*/số, Attached Property)

## Còn thiếu — Cấp 1 (cơ bản, nền tảng chung)

| #   | Chủ đề               | Vì sao cần                                                                                                              |
| --- | -------------------- | ----------------------------------------------------------------------------------------------------------------------- |
| 3   | `StackPanel`         | Panel còn lại đang dùng trong `MainWindow.xaml` (hàng nút bấm) — chưa học                                               |
| 4   | `TextBox`            | Mới dùng qua binding, chưa học riêng property/hành vi của nó                                                            |
| 5   | `Button` + `Command` | Đã dùng, nhưng chưa học kỹ cơ chế `Command` hoạt động thế nào (khác `Click` ra sao)                                     |
| 6   | `DataGrid`           | Đã dùng (`ItemsSource`, `SelectedItem`, `DataGridTextColumn`) nhưng chưa học bài riêng                                  |
| 7   | `{Binding ...}`      | Đã học sơ buổi trước ("giải thích tất cả"), nhưng chưa có bài bài bản, chưa học `Mode`, `ElementName`, `RelativeSource` |

## Còn thiếu — Cấp 2 (cần cho QuestionView — việc kế tiếp trong dự án)

| #   | Chủ đề                                                               |
| --- | -------------------------------------------------------------------- |
| 8   | `ComboBox` (chọn Category)                                           |
| 9   | Binding vào enum (`QuestionType`, `DifficultyLevel`)                 |
| 10  | `RadioButton` + `GroupName` (chọn đáp án đúng — SingleChoice)        |
| 11  | `CheckBox` (chọn đáp án đúng — MultipleChoice)                       |
| 12  | `ItemsControl` + `ItemTemplate` (danh sách `Answer` động — khó nhất) |

## Còn thiếu — Cấp 3 (cần cho UserView)

| #   | Chủ đề                                                      |
| --- | ----------------------------------------------------------- |
| 13  | `DatePicker` (`DateOfBirth`)                                |
| 14  | `PasswordBox` (không bind trực tiếp được — cần xử lý riêng) |

## Còn thiếu — Cấp 4 (nâng cao, làm sau cùng)

| #   | Chủ đề                                                                                       |
| --- | -------------------------------------------------------------------------------------------- |
| 15  | `Style` / `Resources`                                                                        |
| 16  | `IValueConverter`                                                                            |
| 17  | `DataTemplate` cho cột `DataGrid`                                                            |
| 18  | Tách `UserControl` riêng cho từng màn hình + `TabControl` (đã bàn ở phần refactor, chưa làm) |
| 19  | Validate lỗi ngay trên UI (`INotifyDataErrorInfo`)                                           |
