# 11 - Post Processing

**Mục tiêu:** Tonemapping, Color Adjustments (Post Exposure) và Bloom.

## Đã set up sẵn
- Camera: Post Processing **bật**. 3 Global Volume: None / Neutral / ACES; chỉ **Neutral** đang bật.
- Mỗi profile có Tonemapping, Color Adjustments (Post Exposure 0), Bloom (Threshold 1, Intensity 0.5, Scatter 0.6).
- `Emissive_Bulb`: Emission HDR > 1 để có bloom.

## Bạn làm
1. Bật từng Volume (tắt các còn lại), so sánh vùng sáng cháy của 3 chế độ tonemapping.
2. Chỉnh Post Exposure +-1; chỉnh Bloom Threshold/Intensity.
3. Chú ý: Bloom và HDR tốn chi phí trên mobile (xem bài 12-13).

## Đúng khi
- ACES: tương phản cao, vùng sáng ít cháy; None: vùng sáng bị cắt; Bulb phát quang riêng khi Threshold 1.
