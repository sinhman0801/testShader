# 06 - Realtime / Mixed / Baked

**Mục tiêu:** so sánh 3 chế độ Light Mode và vai trò của Static / Light Probe.

## Đã set up sẵn
- Scene này: Directional = **Mixed**, Floor + 2 cube **Static** (Contribute GI), Capsule **động**, Light Probe Group.
- Lighting Settings (`LightingSettings_Preview.asset`): Baked GI bật, Mixed = Baked Indirect, ProgressiveCPU.
- Để so sánh: scene `00_Lighting_Lab/A_Realtime`, `B_Baked`, `C_Mixed`.

## Bạn làm (BAKE do bạn chủ động chạy, tool không bake)
1. Mở scene, Window > Rendering > Lighting > Generate Lighting.
2. Chuyển Light Mode Realtime / Mixed / Baked, so sánh bóng của Capsule và của cube tĩnh.

## Đúng khi
- Baked: capsule không có bóng động nếu thiếu probe; Mixed: bóng realtime + ambient bake; Realtime: mọi thứ động.
