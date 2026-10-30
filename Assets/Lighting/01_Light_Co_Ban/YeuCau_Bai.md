# 01 - Light cơ bản

**Mục tiêu:** phân biệt 4 loại đèn: Directional, Point, Spot, Area.

## Đã set up sẵn
- Floor + 1 Sphere xám. Camera nhìn Sphere từ góc chéo.
- 4 đèn, chỉ `Light_Directional` đang bật; `Light_Point`, `Light_Spot`, `Light_Area_Baked` đang tắt.

## Bạn làm
1. Bật từng đèn (tắt các đèn còn lại), quan sát hình dạng vùng sáng và hướng bóng.
2. Xoay/di chuyển từng đèn, ghi lại khác biệt (Directional: chỉ hướng; Point/Spot: vị trí + range).
3. Area light (Rectangle) là Baked-only: không thấy gì ở Scene cho tới khi bake (chú ý này kiểm tra, không bake trong bài này).

## Đúng khi
- Bạn nói được mỗi loại đèn đúng khi nào, và vì sao Area light không hiện khi Play mà chưa bake.
