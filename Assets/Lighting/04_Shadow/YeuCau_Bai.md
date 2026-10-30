# 04 - Shadow

**Mục tiêu:** hiểu Shadow Type, Strength, Cast Shadows và chi phí của bóng.

## Đã set up sẵn
- Directional Soft Shadows, Strength 1. `Cube_Caster`, tấm mỏng `Plate_Thin`, và `Sphere_NoCast` (Cast Shadows = Off).
- URP asset toàn project KHÔNG bị sửa ở bài này.

## Bạn làm
1. Đổi Shadow Type: No / Hard / Soft và quan sát cạnh bóng.
2. Giảm Strength 1 -> 0.4; xem bóng nhạt dần.
3. Bật Cast Shadows cho `Sphere_NoCast`, so với trước khi bật.
4. Xoay Directional để bóng tấm mỏng dài/ngắn; thấy Shadow Distance ảnh hưởng (bài 12).

## Đúng khi
- Cube và Plate có bóng, Sphere_NoCast không có bóng trước khi bật Cast Shadows.
