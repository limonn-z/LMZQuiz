# Bài 5: `Button` + `Command` (dùng `CommunityToolkit.Mvvm`)

> `Button` = control cho người dùng **bấm để thực hiện 1 hành động**. Ở bài này ta sẽ dùng thư viện `CommunityToolkit.Mvvm`, nên thay vì tự viết `ICommand`/`RelayCommand` tay, bạn dùng **source generator** qua 2 attribute `[ObservableProperty]` và `[RelayCommand]` để tự sinh code. Bài này giải thích theo đúng kiến trúc MVVM, ví dụ lấy thẳng từ `CategoryViewModel`.

## 1/ Cách khai báo Button cơ bản

```xml
<Button Content="Thêm" Width="80" Height="30"/>
```

Chú ý: `Button` dùng property **`Content`**, không phải `Text` — vì `Button` kế thừa `ContentControl`, `Content` chứa được bất kỳ thứ gì (chữ, ảnh, cả 1 `StackPanel`...), không chỉ riêng text.

## 2/ Cách 1 — `Click` event (code-behind) — không dùng trong project MVVM

```xml
<Button Content="Thêm" Click="AddButton_Click"/>
```

Logic nằm trong `.xaml.cs` → vi phạm MVVM, ViewModel không biết gì, nhưng vì ta theo kiến trúc MVVM nên cách này sẽ không thực hiện.

## 3/ Cách 2 — `[RelayCommand]` tự sinh Command (khuyên dùng)

Trong `CategoryViewModel`, bạn không viết `ICommand` tay, mà đánh dấu method bằng `[RelayCommand]`:

```csharp
[RelayCommand]
private async Task AddCategoryAsync()
{
    ...
}
```

Source generator của `CommunityToolkit.Mvvm` tự đọc attribute này và **sinh ra** 1 property public kiểu `IAsyncRelayCommand` tên là `AddCategoryCommand` — bạn không thấy property đó viết ở đâu trong file `.cs`, nhưng nó có thật, bind trong XAML bình thường.

**Quy tắc đặt tên (bắt buộc phải nhớ):** bỏ hậu tố `Async` của method, thêm hậu tố `Command`.

| Method (private, trong `CategoryViewModel`) | Command property được sinh ra (bind trong XAML) |
| ------------------------------------------- | ----------------------------------------------- |
| `LoadCategoriesAsync()`                     | `LoadCategoriesCommand`                         |
| `AddCategoryAsync()`                        | `AddCategoryCommand`                            |
| `RemoveCategoryAsync()`                     | `RemoveCategoryCommand`                         |
| `EditCategoryAsync()`                       | `EditCategoryCommand`                           |

XAML bind vào:

```xml
<Button Content="Thêm" Command="{Binding AddCategoryCommand}"/>
<Button Content="Xóa" Command="{Binding RemoveCategoryCommand}"/>
<Button Content="Sửa" Command="{Binding EditCategoryCommand}"/>
```

**Lỗi hay gặp nhất:** bind nhầm `Command="{Binding AddCategoryAsync}"` (tên method, `private`) — XAML không thấy được, phải bind đúng `AddCategoryCommand` (tên property, `public`, do generator sinh ra).

## 4/ `[ObservableProperty]` — vì Command hay phụ thuộc vào nó

```csharp
[ObservableProperty]
private Category? selectedCategory = null;
```

Field `selectedCategory` (chữ thường) → generator tự sinh property public `SelectedCategory` (chữ hoa), tự báo `PropertyChanged` khi đổi giá trị. Đây chính là property mà `RemoveCategoryCommand`/`EditCategoryCommand` sẽ cần kiểm tra ở mục tiếp theo.

## 5/ Khái niệm mới — `CanExecute` với source generator

Nhìn code hiện tại của bạn: `RemoveCategoryAsync` đang tự `throw` lỗi nếu `SelectedCategory == null`, rồi hiện `MessageBox` báo lỗi. Cách này chạy được, nhưng đúng ra nút "Xóa"/"Sửa" nên **tự mờ đi** ngay từ đầu khi chưa chọn dòng nào — người dùng không bấm được thì cũng không cần hiện popup lỗi nữa. Đây là lúc dùng `CanExecute`:

```csharp
[RelayCommand(CanExecute = nameof(CanEditOrRemove))]
private async Task RemoveCategoryAsync()
{
    await _categoryService.RemoveCategoryAsync(SelectedCategory!);
    await LoadCategoriesAsync();
}

private bool CanEditOrRemove() => SelectedCategory != null;
```

`CanExecute = nameof(CanEditOrRemove)` báo cho generator biết: trước khi cho phép bấm `RemoveCategoryCommand`, phải gọi `CanEditOrRemove()` kiểm tra trước. Trả về `false` → nút tự `IsEnabled="False"`.

## 6/ Cái bẫy hay gặp nhất — `NotifyCanExecuteChangedFor`

Chỉ viết `CanExecute` như mục 5 là **chưa đủ** — nút sẽ không tự cập nhật mờ/sáng khi `SelectedCategory` đổi (ví dụ người dùng click chọn dòng khác trong `DataGrid`), vì generator không tự biết `CanEditOrRemove()` phụ thuộc vào property nào. Phải khai báo rõ:

```csharp
[ObservableProperty]
[NotifyCanExecuteChangedFor(nameof(RemoveCategoryCommand))]
[NotifyCanExecuteChangedFor(nameof(EditCategoryCommand))]
private Category? selectedCategory = null;
```

→ Giờ mỗi lần `SelectedCategory` đổi giá trị, 2 nút Xóa/Sửa **tự động** được kiểm tra lại `CanExecute` và tự enable/disable theo — không cần code tay ở đâu cả.

## 7/ `CommandParameter` (vẫn dùng được, ít cần hơn khi có `[ObservableProperty]`)

```xml
<Button Content="Xóa" Command="{Binding RemoveCategoryCommand}" CommandParameter="{Binding SelectedCategory}"/>
```

Trong `CategoryViewModel` hiện tại bạn **không cần** cách này, vì `RemoveCategoryAsync()` đã tự đọc thẳng `SelectedCategory` (property của chính ViewModel) — không cần truyền qua `CommandParameter`. Cách này chỉ thật sự cần khi 1 Command dùng chung cho **nhiều dòng** trong `ItemsControl` mà mỗi dòng có 1 `DataContext` khác nhau (sẽ gặp lại ở bài `ItemsControl`).

## 8/ So sánh `Click` vs `[RelayCommand]`

|                                      | `Click` event                   | `[RelayCommand]`                                    |
| ------------------------------------ | ------------------------------- | --------------------------------------------------- |
| Logic nằm ở đâu                      | code-behind (`.xaml.cs`)        | ViewModel                                           |
| Đúng chuẩn MVVM?                     | Không                           | Có                                                  |
| Tự enable/disable nút theo điều kiện | Không — phải tự set `IsEnabled` | Có, qua `CanExecute` + `NotifyCanExecuteChangedFor` |
| Cần viết `ICommand` tay?             | Không áp dụng                   | Không — generator tự sinh                           |
| Project bạn đang dùng                | Không                           | **Có**                                              |

## 9/ Property khác đáng chú ý — `IsDefault` / `IsCancel`

```xml
<Button Content="Thêm" IsDefault="True" Command="{Binding AddCategoryCommand}"/>
```

`IsDefault="True"` → nhấn **Enter** ở bất kỳ `TextBox` nào trong Window sẽ trigger nút này (liên quan tới điều đã nhắc ở bài 4: `TextBox` nhiều dòng cần `AcceptsReturn="True"`, nếu không Enter sẽ bị nút `IsDefault` "cướp" mất).

## 10/ Ví dụ tổng hợp — bind đúng với `CategoryViewModel` thật

```xml
<StackPanel Margin="10">
    <TextBlock Text="Tên danh mục:"/>
    <TextBox Width="250" Margin="0,4,0,8"
             Text="{Binding NewCategoryName, UpdateSourceTrigger=PropertyChanged}"/>

    <TextBlock Text="Mô tả:"/>
    <TextBox Width="250" Height="60" Margin="0,4,0,12"
             TextWrapping="Wrap" AcceptsReturn="True"
             Text="{Binding NewCategoryDescription}"/>

    <StackPanel Orientation="Horizontal" HorizontalAlignment="Right">
        <Button Content="Thêm" Width="80" Height="30" Margin="0,0,8,0"
                Command="{Binding AddCategoryCommand}"/>
        <Button Content="Sửa" Width="80" Height="30" Margin="0,0,8,0"
                Command="{Binding EditCategoryCommand}"/>
        <Button Content="Xóa" Width="80" Height="30"
                Command="{Binding RemoveCategoryCommand}"/>
    </StackPanel>
</StackPanel>
```

Kết quả hiển thị:

```
Tên danh mục:
┌───────────────────────────┐
└───────────────────────────┘

Mô tả:
┌───────────────────────────┐
│                           │
└───────────────────────────┘

                    ┌──────┐ ┌──────┐ ┌──────┐
                    │ Thêm │ │ Sửa  │ │ Xóa  │   ← Sửa/Xóa tự mờ
                    └──────┘ └──────┘ └──────┘      khi chưa chọn dòng nào
```

- `Thêm` luôn bấm được (không có `CanExecute`, đúng như code gốc bạn viết — thêm mới thì không cần chọn sẵn dòng nào).
- `Sửa`/`Xóa` nếu áp dụng mục 5–6 ở trên → tự mờ khi `SelectedCategory == null`, tự sáng lại ngay khi người dùng click chọn 1 dòng trong `DataGrid`.
