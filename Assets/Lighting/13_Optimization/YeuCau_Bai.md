# 13 - Optimization

**Mục tiêu:** đo ảnh hưởng của đèn có bóng, post-process, reflection probe realtime lên frame time.

## Đã set up sẵn
- Lab có 3 đèn Point có bóng (`Toggle_ExtraLights`), 30 cube nhỏ (`Toggle_Props`).
- `Toggle_PostVolume` (TẮT) và `Toggle_RealtimeProbe` (Realtime, Every Frame, TẮT) để bật khi đo.

## Bạn làm (đo trên thiết bị đích)
1. Mở Profiler / Stats. Đo baseline (tắt hết các Toggle_*).
2. Bật từng Toggle_* một, ghi frame time, batches, SetPass.
3. Quyết định tắt/bake thứ gì trước theo bằng chứng.

## Đúng khi
- Bật mỗi Toggle làm số Batches/Shadow casters thay đổi rõ; baseline dùng để so sánh.
