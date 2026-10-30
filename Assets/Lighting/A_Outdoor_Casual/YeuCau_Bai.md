# A - Outdoor Casual (bài tổng hợp)

**Mục tiêu:** cảnh ngoài trời casual, 1 đèn chính Realtime, bóng cứng, rẻ cho mobile.

## Đã set up sẵn
- Sàn 6x6, 3 cube cao thấp khác nhau, 1 sphere sáng, 1 sphere kim loại để thử phản xạ.
- Directional ấm (Warm) Realtime, rotation (50, -30, 0), **Hard** shadows. Ambient Flat nhẹ.
- `URP_Baseline.asset`: shadow map 1024, Shadow Distance 15 m, 1 cascade, Soft Shadows tắt, không đèn phụ, HDR tắt. (chưa gán toàn project)

## Bạn làm
1. Gán `URP_Baseline` ở Graphics/Quality, thấy bóng cứng nhưng sạch.
2. Thử bật Soft Shadows và tăng cascade lên 2, so sánh chất lượng/chi phí.
3. Đo trên thiết bị đích và ghi số liệu.

## Đúng khi
- Bóng rõ, không vỡ cắt; Sphere kim loại không 'đen' (nhờ ambient/skybox).
