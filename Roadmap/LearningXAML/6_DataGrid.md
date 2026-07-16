# Bài 6: `DataGrid`

> `DataGrid` = control hiển thị **danh sách dữ liệu dạng bảng** — mỗi phần tử trong collection là 1 dòng, mỗi property là 1 cột. Bài này học kỹ từng property để hiểu vì sao nó chạy được.

## 1/ Cách khai báo cơ bản

```xml
<DataGrid ItemsSource="{Binding Categories}"/>
```

`ItemsSource` = nguồn dữ liệu, phải là 1 collection (thường dùng `ObservableCollection<T>` để `DataGrid` tự cập nhật khi collection thêm/xóa phần tử — khác với `List<T>` là không tự cập nhật UI).

Chỉ với 1 dòng trên, `DataGrid` đã tự hiển thị bảng — vì mặc định `AutoGenerateColumns="True"`: nó tự soi hết property `public` của `Category`, mỗi property tự thành 1 cột.

## 2/ Khái niệm mới — `AutoGenerateColumns` và vì sao nên tắt đi

`AutoGenerateColumns="True"` (mặc định) tiện lúc test nhanh, nhưng có 3 vấn đề:

- Hiện **hết** mọi property `public` — kể cả `Id` mà người dùng không cần thấy.
- Tên cột lấy thẳng tên property (`Description` → cột tên "Description", không dịch tiếng Việt được).
- Không tự chỉnh được `Width`, định dạng, thứ tự cột theo ý muốn.

→ Trong project thật, gần như luôn tắt đi và tự khai `Columns`:

```xml
<DataGrid ItemsSource="{Binding Categories}" AutoGenerateColumns="False">
    <DataGrid.Columns>
        <DataGridTextColumn Header="Tên danh mục" Binding="{Binding Name}" Width="150"/>
        <DataGridTextColumn Header="Mô tả" Binding="{Binding Description}" Width="*"/>
    </DataGrid.Columns>
</DataGrid>
```

## 3/ Khái niệm mới — `DataContext` bên trong `Columns` đổi theo **từng dòng**

Đây là chỗ dễ nhầm nhất khi mới học. Nhìn kỹ dòng:

```xml
<DataGridTextColumn Header="Tên danh mục" Binding="{Binding Name}"/>
```

`{Binding Name}` — `Name` này **không phải** property của `CategoryViewModel`. Bên trong `DataGrid.Columns`, mỗi dòng dữ liệu tự động có `DataContext` là **1 phần tử trong `Categories`** (tức 1 object `Category`), không còn là ViewModel nữa. Vì `Category` có property `Name`, nên `{Binding Name}` ở đây hiểu là "lấy `Name` của object `Category` đang ở dòng này".

**So sánh để nhớ:**

| Vị trí Binding                                                        | `{Binding ...}` đang trỏ vào đâu                      |
| --------------------------------------------------------------------- | ----------------------------------------------------- |
| Ngoài `DataGrid.Columns` (ví dụ `ItemsSource="{Binding Categories}"`) | `CategoryViewModel`                                   |
| Bên trong `DataGrid.Columns` (ví dụ cột `Binding="{Binding Name}"`)   | Từng object `Category` — 1 phần tử trong `Categories` |

Đây gọi là **DataContext bị "đổi ngữ cảnh"** theo từng dòng — khái niệm này sẽ gặp lại y hệt ở bài `ItemsControl` sau này.

## 4/ `SelectedItem` — biết người dùng đang chọn dòng nào

```xml
<DataGrid ItemsSource="{Binding Categories}"
          SelectedItem="{Binding SelectedCategory}"
          AutoGenerateColumns="False">
    ...
</DataGrid>
```

Giống `TextBox.Text` ở bài 4, `SelectedItem` cũng là 1 trong số ít property **mặc định `TwoWay`**: người dùng click chọn dòng nào trên UI → `SelectedCategory` bên ViewModel tự cập nhật thành object `Category` của dòng đó. Đây chính là cơ chế đứng sau `RemoveCategoryAsync()`/`EditCategoryAsync()` ở bài 5 — 2 hàm đó đọc được `SelectedCategory` là nhờ dòng `SelectedItem="{Binding SelectedCategory}"` này.

## 5/ Vài property khác hay dùng

| Property                     | Ý nghĩa                                                                                                                                                                      |
| ---------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `CanUserAddRows="False"`     | Mặc định `DataGrid` tự chừa sẵn 1 dòng trống ở cuối để người dùng gõ thêm dòng mới — hầu hết project tắt đi vì đã có form riêng để "Thêm" (như `AddCategoryCommand` ở bài 5) |
| `CanUserDeleteRows="False"`  | Tắt khả năng nhấn phím Delete để tự xóa dòng ngay trên bảng — tương tự, thường xóa qua `Command` riêng cho kiểm soát chặt hơn (confirm, xử lý lỗi...)                        |
| `IsReadOnly="True"`          | Không cho sửa trực tiếp nội dung ô trên bảng (double-click vào ô để gõ) — bắt sửa qua form riêng thay vì sửa tùy tiện ngay trên bảng                                         |
| `HeadersVisibility="Column"` | Chỉ hiện header cột, ẩn header hàng (cột số thứ tự bên trái mặc định `DataGrid` tự vẽ)                                                                                       |

## 6/ Ví dụ tổng hợp — đúng với `CategoryViewModel` thật

```xml
<DataGrid ItemsSource="{Binding Categories}"
          SelectedItem="{Binding SelectedCategory}"
          AutoGenerateColumns="False"
          CanUserAddRows="False"
          CanUserDeleteRows="False"
          IsReadOnly="True"
          HeadersVisibility="Column"
          Margin="10">
    <DataGrid.Columns>
        <DataGridTextColumn Header="Tên danh mục" Binding="{Binding Name}" Width="150"/>
        <DataGridTextColumn Header="Mô tả" Binding="{Binding Description}" Width="*"/>
    </DataGrid.Columns>
</DataGrid>
```

Kết quả hiển thị:

```
┌─────────────────┬───────────────────────────────┐
│ Tên danh mục     │ Mô tả                          │
├─────────────────┼───────────────────────────────┤
│ Toán học         │ Câu hỏi về đại số, hình học     │  ← click dòng này
├─────────────────┼───────────────────────────────┤     → SelectedCategory
│ Lịch sử          │ Câu hỏi về sự kiện lịch sử       │     tự đổi thành object
└─────────────────┴───────────────────────────────┘     Category "Lịch sử"
```

Click chọn dòng "Lịch sử" → `SelectedCategory` tự đổi → nhờ mục 5-6 bài 5 (`NotifyCanExecuteChangedFor`), nút "Sửa"/"Xóa" tự sáng lên, sẵn sàng thao tác trên đúng dòng vừa chọn.
