# Bài 1: `Window`

> `Window` = 1 class có sẵn trong WPF, đại diện cho **1 cửa sổ** hiện lên màn hình máy tính. Mọi app WPF đều có ít nhất 1 `Window` — chính là `MainWindow` bạn đang có.

## Cách so sánh - khai báo tối giản nhất:

    ```xml
    <Window Title="App của tôi" Height="450" Width="800">

    </Window>
    ```

    ```csharp
    var w = new Window();
    w.Title = "App của tôi";
    w.Height = 450;
    w.Width = 800;
    ```

## 3 property hay dùng nhất

| Property | Ý nghĩa                                                |
| -------- | ------------------------------------------------------ |
| `Title`  | Chữ hiện trên thanh tiêu đề cửa sổ (góc trên bên trái) |
| `Height` | Chiều cao cửa sổ, tính bằng pixel                      |
| `Width`  | Chiều rộng cửa sổ, tính bằng pixel                     |

## `Window` chỉ chứa được **đúng 1 thứ** bên trong

Đây là điều quan trọng nhất cần nhớ: `Window` có property `Content` (thẻ lồng bên trong = gán vào `Content`), nhưng `Content` **chỉ nhận 1 object duy nhất**. Không viết được kiểu này:

    ```xml
    <!-- SAI — Window không chứa được 2 thứ cùng lúc -->
    <Window>
        <TextBox />
        <Button />
    </Window>
    ```

Đây là lý do **bắt buộc phải có `Grid`/`StackPanel` bọc ở ngoài** — để "gộp nhiều thứ lại thành 1 gói", rồi nhét cả gói đó vào `Content` của `Window`:

```xml
<!-- ĐÚNG — Grid là "1 gói", chứa được nhiều thứ bên trong nó -->
<Window>
    <Grid>
        <TextBox />
        <Button />
    </Grid>
</Window>
```
