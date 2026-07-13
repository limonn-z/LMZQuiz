# Bài 2: `Grid`

> Grid = 1 "panel" (hộp chứa) sắp xếp các control theo dạng bảng — chia hàng, chia cột, giống bảng Excel. Đây là panel mạnh nhất, dùng được cho hầu hết layout phức tạp.

## 1/ Cách khai báo

```xml
    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
        </Grid.RowDefinitions>

        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="200"/>
            <ColumnDefinition Width="*"/>
        </Grid.ColumnDefinitions>
    </Grid>
```

- Ý nghĩa:
  - `<Grid.RowDefinitions>` chia Grid thành n hàng `<RowDefinition />` có độ cao khác nhau nhưng tùy thiết kế.
  - `<Grid.ColumnDefinitions>` chia Grid thành nhiều cột `<ColumnDefinition>`.
  - Nếu **không khai báo** `RowDefinitions` và `ColumnDefinitions` thì Grid mặc định chỉ có **1 hàng và 1 cột**.
  - Có 3 loại kiểu cao khác nhau (áp dụng cả chiều rộng `Weight`):

    | Giá trị         | Ý nghĩa                                                                                    |
    | --------------- | ------------------------------------------------------------------------------------------ |
    | `Height="200"`  | Cố định đúng 200px, không đổi dù nội dung bên trong nhiều hay ít                           |
    | `Height="Auto"` | Cao **vừa đủ** với nội dung bên trong — nội dung ít thì hàng thấp, nhiều thì hàng cao theo |
    | `Height="*"`    | Chiếm **hết phần còn lại** sau khi các hàng khác đã lấy đủ chỗ của chúng                   |

## 2/ Khái niệm mới — **Attached Property**

```xml
    <TextBox
        Grid.Row="0"
        Grid.Column="1"
    />
```

- Ý nghĩa:
  - `Grid.Row` báo cho Grid biết control nằm ở **hàng nào**.
  - `Grid.Column` báo cho Grid biết control nằm ở **cột nào**.
  - Đây **không phải** là thuộc tính của `TextBox`, mà là **Attached Property** do `Grid` cung cấp.
  - Có thể gắn lên bất kỳ control nào bên trong Grid.

## 3/ Giá trị mặc định

Nếu không gán:

```xml
    <TextBox />
```

thì tương đương với:

```xml
    <TextBox
        Grid.Row="0"
        Grid.Column="0"
    />
```

Nghĩa là control sẽ nằm ở **hàng đầu tiên** và **cột đầu tiên**.

## 4/ `Grid` không tự vẽ đường viền

`Grid` chỉ là bảng **vô hình** dùng để định vị trí, không phải bảng có viền như Excel hay HTML. Khai `RowDefinitions`/`ColumnDefinitions` xong in ra sẽ **không thấy đường kẻ nào** giữa các ô.

- `Border` là control chuyên vẽ khung viền, có `BorderBrush` (màu viền) và `BorderThickness` (độ dày viền).
- Mỗi ô muốn có viền phải **tự bọc `Border`** — `Grid` không tự làm việc này.

## 5/ Ví dụ sử dụng

Ví dụ không viền:

```xml
    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
        </Grid.RowDefinitions>

        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="150"/>
            <ColumnDefinition Width="*"/>
        </Grid.ColumnDefinitions>

        <TextBlock Text="Username"
                Grid.Row="0"
                Grid.Column="0"/>

        <TextBlock Text = "Password"
                Grid.Row="0"
                Grid.Column="1"/>
    </Grid>
```

Ví dụ có viền:

```xml
    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
        </Grid.RowDefinitions>

        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="150"/>
            <ColumnDefinition Width="*"/>
        </Grid.ColumnDefinitions>

        <Border BorderBrush="Black" BorderThickness="1" Grid.Row="0" Grid.Column="0">
            <TextBlock Text="Username"/>
        </Border>

        <Border BorderBrush="Black" BorderThickness="1" Grid.Row="0" Grid.Column="1">
            <TextBlock Text="Password"/>
        </Border>

    </Grid>
```

Kết quả (dấu # là khoảng trống, đừng quan tâm):

| Username | Password                       |
| -------- | ------------------------------ |
| ######## | ############################## |
| ######## | ############################## |
| ######## | ############################## |
| ######## | ############################## |
| ######## | ############################## |
