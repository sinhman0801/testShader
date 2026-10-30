# 10 - Reflection Probe

**Mục tiêu:** để kim loại/bóng phản xạ đúng môi trường trong phòng, không phản xạ bầu trời.

## Đã set up sẵn
- `RP_Room`: Baked, resolution 128, Box 5.6 x 3 x 5.6, Box Projection **bật**. `RP_Outside` phía trước phòng.
- Sàn, tường, cube là Reflection Probe Static. Sphere kim loại + `Sphere_Mirror` smoothness 0.95.

## Bạn làm (bake do bạn chủ động chạy)
1. Chọn RP_Room > Bake. Sphere phản xạ tường/sàn thay vì bầu trời.
2. Tắt Box Projection để so sánh độ lệch phản xạ.
3. Giảm resolution 128 -> 32 để xem chất lượng và dung lượng.

## Đúng khi
- Sau bake, Sphere kim loại phản xạ các bức tường của phòng.
