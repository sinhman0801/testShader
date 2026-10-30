# 03 - Material và Light

**Mục tiêu:** thấy Metallic / Smoothness (URP/Lit) thay đổi cách vật phản ứng với đèn.

## Đã set up sẵn
- 9 sphere: cột x = Smoothness 0.1 / 0.5 / 0.9; hàng z = nhựa trắng / nhựa đỏ / kim loại (Metallic 1).
- 1 sphere kết hợp Metallic 1, Smoothness 0.75 ở góc.
- 1 Directional soft shadows (50, -30, 0).

## Bạn làm
1. So sánh 3 cột: smoothness tăng -> điểm highlight nhỏ và sắc hơn.
2. So sánh nhựa vs kim loại: kim loại hầu như không có màu khuếch tán, chỉ phản xạ môi trường.
3. Đổi hướng Directional và xem highlight di chuyển trên mỗi sphere.

## Đúng khi
- Hàng kim loại trông như gương mờ (S0.1) tới gương (S0.9); hàng nhựa có highlight, không phản xạ rõ.
