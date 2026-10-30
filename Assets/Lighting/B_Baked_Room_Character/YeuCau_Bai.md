# B - Baked Room + Character (bài tổng hợp)

**Mục tiêu:** phòng tĩnh được bake đẹp, nhân vật động khớp ánh sáng nhờ probe.

## Đã set up sẵn
- Nhóm `Static_Environment` (Contribute GI, Receive GI = Lightmaps) và `Dynamic_Character` (Blend Probes).
- Màu tường/sàn dịu. Directional = **Mixed** chiếu qua cửa (Baked Indirect); `Light_Area_Baked` trên trần.
- Light Probe Group dày hơn gần cửa; `RP_Room` Baked 128 với Box Projection. Lighting Settings preview.

## Bạn làm (bake do bạn chủ động chạy)
1. Generate Lighting, bake `RP_Room` riêng.
2. Di chuyển `Character_Body` qua cửa: màu sáng phải đổi mượt.
3. Giữ bóng realtime cho nhân vật, bóng của môi trường đã được bake.

## Đúng khi
- Nhân vật không bị 'đen' hoặc 'cháy sáng' khi vào phòng; phòng không có vệt nối lightmap rõ.
