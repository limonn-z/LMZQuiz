# Bài 2: `Grid`

> Grid = 1 "panel" (hộp chứa) sắp xếp các control theo dạng bảng — chia hàng, chia cột, giống bảng Excel. Đây là panel mạnh nhất, dùng được cho hầu hết layout phức tạp.

## 1/ Cách khai báo — so sánh với C#

```xml
    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
        </Grid.RowDefinitions>
    </Grid>
```
