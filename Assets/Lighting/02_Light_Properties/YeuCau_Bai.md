# 02 - Light Properties

**Mục tiêu:** hiểu Intensity, Range, Spot Angle, Color.

## Đã set up sẵn
- Directional TẮT (để chỉ thấy đèn đang thử). Spot ở (0, 3, 0) hướng xuống, Spot Angle 37, Range 3.2, Intensity 2.
- `Zone_2x2`: tấm đánh dấu 2x2 trên sàn; vùng sáng của spot đủ phủ vừa nó (độ dài 3 m x tan(18.5 độ) ~ 1 m bán kính).
- `Probe_Sphere` để kéo ra/vào vùng sáng; `Light_Point_RangeTest` (tắt) để thử Range.

## Bạn làm
1. Chỉnh Spot Angle / Range sao cho vùng sáng vừa khít `Zone_2x2`; ghi giá trị tìm được.
2. Đổi Intensity (0.5, 2, 5) và Color; xem mật độ sáng trên Sàn và Sphere.
3. Bật `Light_Point_RangeTest`, đổi Range 1 -> 6, xem độ mờ của vùng sáng.

## Đúng khi
- Viền vùng sáng chạm đúng mép `Zone_2x2`, Probe_Sphere ngoài vùng gần như tối.
