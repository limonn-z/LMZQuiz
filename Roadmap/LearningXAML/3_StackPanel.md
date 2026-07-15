# Bài 3: `StackPanel`

> StackPanel = 1 "panel" xếp các control liên tiếp theo **1 hàng dọc** hoặc **1 hàng ngang** — control sau nối ngay tiếp control trước theo đúng thứ tự khai báo trong XAML. Đơn giản hơn `Grid` nhiều: không chia hàng/cột, không cần `Grid.Row`/`Grid.Column`.

## 1/ Cách khai báo

```xml
<StackPanel>
    <TextBlock Text="Username"/>
    <TextBox />
    <TextBlock Text="Password"/>
    <PasswordBox />
    <Button Content="Login"/>
</StackPanel>
```

Mặc định `StackPanel` xếp theo **chiều dọc** — từng control nằm chồng lên nhau từ trên xuống dưới, đúng theo thứ tự viết trong XAML.

## 2/ Property quan trọng nhất — `Orientation`

```xml
<StackPanel Orientation="Horizontal">
    <Button Content="OK"/>
    <Button Content="Cancel"/>
</StackPanel>
```

| Giá trị               | Ý nghĩa                                         |
| --------------------- | ----------------------------------------------- |
| `Vertical` (mặc định) | Xếp control theo chiều dọc, từ trên xuống       |
| `Horizontal`          | Xếp control theo chiều ngang, từ trái sang phải |

## 3/ StackPanel không dùng Attached Property như Grid

Nhắc lại bài 2: `Grid` dùng `Grid.Row`/`Grid.Column` (Attached Property) để báo control nằm ở đâu. `StackPanel` **không cần** — vì mỗi control tự động nối tiếp control trước, theo đúng **thứ tự khai báo trong XAML**. Muốn đổi vị trí control, chỉ cần đổi thứ tự dòng code, không sửa property nào cả.

## 4/ StackPanel không tự chừa khoảng cách giữa các control

Khác với `Grid` (mỗi hàng/cột đã tự ngăn cách sẵn), `StackPanel` để các control dính sát nhau. Muốn có khoảng cách, phải tự gán `Margin` cho từng control:

```xml
<StackPanel>
    <TextBox Margin="0,0,0,8"/>
    <TextBox Margin="0,0,0,8"/>
    <Button Content="Login"/>
</StackPanel>
```

- `Margin="0,0,0,8"` = trái, trên, phải, dưới → ở đây nghĩa là chừa **8px phía dưới** mỗi `TextBox` để cách control kế tiếp ra.

## 5/ `StackPanel` vs `Grid` — khi nào dùng cái nào?

| `Tiêu chí`             | `Grid`                                     | `StackPanel`                                |
| ---------------------- | ------------------------------------------ | ------------------------------------------- |
| Layout phù hợp         | Phức tạp, nhiều hàng/cột, form nhiều field | Đơn giản, 1 dãy nút/field liên tiếp nhau    |
| ---------------------- | ------------------------------------------ | ------------------------------------------- |
| Cần khai Row/Column?   | Có                                         | Không                                       |
| ---------------------- | ------------------------------------------ | ------------------------------------------- |
| Tự co giãn theo cửa sổ | Có, nhờ `Height="*"` / `Width="*"`         | Không — chỉ co theo đúng nội dung bên trong |
| ---------------------- | ------------------------------------------ | ------------------------------------------- |
| Hay dùng để làm gì     | Form đăng nhập nhiều field, dashboard      | Thanh nút bấm (toolbar), danh sách item dọc |

## 6/ Ví dụ tổng hợp — lồng StackPanel bên trong Grid

Trong thực tế, `StackPanel` thường **lồng bên trong `Grid`**, chứ không thay thế hẳn `Grid`:

```xml
<Grid>
    <Grid.RowDefinitions>
        <RowDefinition Height="*"/>
        <RowDefinition Height="Auto"/>
    </Grid.RowDefinitions>

    <TextBlock Grid.Row="0" Text="Nội dung chính"/>

    <StackPanel Grid.Row="1" Orientation="Horizontal" HorizontalAlignment="Right">
        <Button Content="OK" Margin="0,0,8,0"/>
        <Button Content="Cancel"/>
    </StackPanel>
</Grid>
```

→ `Grid` lo layout tổng thể (chia 2 hàng: nội dung chính chiếm hết chỗ còn lại, hàng nút vừa đủ cao). `StackPanel` lo xếp 2 nút OK/Cancel nằm cạnh nhau, dồn về góc dưới bên phải — đúng kiểu bố cục hay gặp trong form thật.

**Lưu ý:** `StackPanel` khi lồng trong `Grid` vẫn nhận `Grid.Row`/`Grid.Column` bình thường như bất kỳ control nào khác — vì đó là Attached Property của Grid gắn lên nó, không phải property riêng của StackPanel.
