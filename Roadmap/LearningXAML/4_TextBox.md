# Bài 4: `TextBox`

> TextBox = control cho người dùng **gõ / sửa văn bản** (1 dòng hoặc nhiều dòng). Là control input cơ bản nhất — gần như form nào cũng có ít nhất 1 cái.

## 1/ Cách khai báo

```xml
<TextBox Width="200" Height="30" Text="Nhập tên..."/>
```

## 2/ Property hay dùng nhất

| Property         | Ý nghĩa                                                                                           |
| ----------------- | --------------------------------------------------------------------------------------------------- |
| `Text`            | Nội dung chữ đang có trong TextBox                                                                 |
| `Width` / `Height`| Kích thước                                                                                          |
| `MaxLength`       | Giới hạn số ký tự tối đa được gõ                                                                    |
| `IsReadOnly`      | `True` = chỉ xem, không sửa được — nhưng vẫn chọn/copy được (khác với `IsEnabled="False"` là mờ hẳn, không tương tác được gì) |
| `TextWrapping`    | `Wrap` = tự xuống dòng khi chữ dài hơn bề ngang TextBox                                            |
| `AcceptsReturn`   | `True` = cho phép nhấn Enter để xuống dòng (dùng khi cần ô nhập nhiều dòng)                        |

## 3/ Khái niệm mới — Binding vào `Text` mặc định là **TwoWay**

Hầu hết property khi binding đều mặc định `OneWay` (ViewModel đổi → UI tự cập nhật, nhưng gõ trên UI thì **không** đẩy ngược về ViewModel). Riêng property `Text` của `TextBox` là **ngoại lệ** — mặc định sẵn `TwoWay`:

```xml
<TextBox Text="{Binding Username}"/>
<!-- Tương đương, không cần viết tay -->
<TextBox Text="{Binding Username, Mode=TwoWay}"/>
```

Nghĩa là: ViewModel đổi `Username` → TextBox tự đổi chữ hiển thị. Và ngược lại, người dùng gõ vào TextBox → `Username` bên ViewModel cũng tự cập nhật theo — không cần khai `Mode=TwoWay` thủ công.

## 4/ Khái niệm mới — `UpdateSourceTrigger`

Quyết định: gõ trên TextBox thì **khi nào** giá trị mới thật sự được đẩy ngược về ViewModel.

| Giá trị                                   | Ý nghĩa                                                                          |
| ------------------------------------------- | ----------------------------------------------------------------------------------- |
| `LostFocus` (mặc định của `TextBox.Text`)  | Chỉ cập nhật về ViewModel khi TextBox **mất focus** (click ra ngoài, Tab sang ô khác) |
| `PropertyChanged`                          | Cập nhật về ViewModel **ngay lập tức, từng ký tự gõ vào**                          |

```xml
<TextBox Text="{Binding Username, UpdateSourceTrigger=PropertyChanged}"/>
```

- Cần validate/tìm kiếm theo thời gian thực (ô search gõ tới đâu lọc tới đó) → dùng `PropertyChanged`.
- Form bình thường (đăng nhập, đăng ký) → để mặc định `LostFocus` là đủ, đỡ tốn hiệu năng vì không phải cập nhật liên tục.

## 5/ Ví dụ — TextBox nhiều dòng (multi-line)

```xml
<TextBox
    Width="300"
    Height="100"
    TextWrapping="Wrap"
    AcceptsReturn="True"
    Text="{Binding Description, UpdateSourceTrigger=PropertyChanged}"/>
```

→ Nếu **không** có `TextWrapping="Wrap"` + `AcceptsReturn="True"`, TextBox mặc định chỉ nhận **1 dòng**: chữ dài sẽ tràn ra ngoài chứ không tự xuống dòng, và Enter sẽ không xuống dòng được (Enter lúc đó thường trigger nút mặc định của form, nếu form có khai `IsDefault="True"` cho 1 Button nào đó).

## 6/ Lưu ý — Event `TextChanged` vs Binding

Ngoài binding ở trên, `TextBox` còn có event `TextChanged`, dùng được ở code-behind:

```csharp
private void MyTextBox_TextChanged(object sender, TextChangedEventArgs e)
{
    // xử lý mỗi khi text đổi
}
```

Nhưng vì project đang theo mô hình **MVVM**, nên **ưu tiên Binding + `UpdateSourceTrigger`** thay vì bắt event `TextChanged` trong `.xaml.cs` — giữ code UI sạch, toàn bộ logic nằm ở ViewModel, không rải rác qua 2 nơi.

## 7/ Ví dụ tổng hợp — Code và kết quả hiển thị

```xml
<StackPanel Margin="10">
    <TextBlock Text="Họ tên:"/>
    <TextBox Width="250"
             Text="{Binding FullName, UpdateSourceTrigger=PropertyChanged}"
             Margin="0,4,0,12"/>

    <TextBlock Text="Ghi chú:"/>
    <TextBox Width="250" Height="60"
             TextWrapping="Wrap"
             AcceptsReturn="True"
             MaxLength="100"
             Text="{Binding Note}"/>
</StackPanel>
```

Kết quả hiển thị trên cửa sổ:

```
Họ tên:
┌───────────────────────────┐
│ Nguyễn Văn A               │   ← gõ tới đâu, FullName bên
└───────────────────────────┘      ViewModel đổi tới đó (PropertyChanged)

Ghi chú:
┌───────────────────────────┐
│ Ghi chú dài sẽ tự động     │   ← nhờ TextWrapping="Wrap"
│ xuống dòng khi hết bề      │      + AcceptsReturn="True"
│ ngang, không tràn ra ngoài │
└───────────────────────────┘
```

Giải thích 2 TextBox trong ví dụ:

- **Họ tên** — chỉ 1 dòng, có `UpdateSourceTrigger=PropertyChanged` nên gõ tới ký tự nào, `FullName` bên ViewModel cập nhật ngay ký tự đó, không cần click ra ngoài.
- **Ghi chú** — cao 60px, `TextWrapping="Wrap"` + `AcceptsReturn="True"` nên chữ dài tự xuống dòng và Enter xuống dòng được; giới hạn tối đa 100 ký tự (`MaxLength`); vì **không** khai `UpdateSourceTrigger` nên dùng mặc định `LostFocus` — `Note` chỉ cập nhật khi click ra khỏi ô này.
