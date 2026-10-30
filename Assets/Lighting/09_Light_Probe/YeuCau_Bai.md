# 09 - Light Probe

**Mục tiêu:** đặt Light Probe Group đúng cho vật động (Capsule) đi qua vùng sáng/tối.

## Đã set up sẵn
- Phòng có cửa (khe giữa hai tường sau). Directional = Mixed.
- Light Probe Group: 3 tầng (y = 0.3 / 1.2 / 2.1), lưới 6x6, **dày hơn gần cửa**; đã loại các điểm nằm trong hình học.
- `Dynamic_Capsule`/`Capsule_Moving`: Light Probes = Blend Probes.
- Lighting Settings preview gắn sẵn.

## Bạn làm (bake do bạn chủ động chạy)
1. Generate Lighting, Play, kéo Capsule qua cửa: màu sáng đổi mượt theo probe.
2. Bỏ bớt probe ở gần cửa -> thấy Capsule 'nhảy' sáng.
3. Xóa một số probe trong tường (nếu có) -> thấy kết quả sai.

## Đúng khi
- Capsule sáng lên mượt khi đi vào vùng nắng cửa, không nhảy.
