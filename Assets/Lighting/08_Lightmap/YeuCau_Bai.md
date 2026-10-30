# 08 - Lightmap

**Mục tiêu:** hiểu Lightmap Resolution, UV lightmap, Receive GI và dung lượng.

## Đã set up sẵn
- Sàn + 3 tường **Static (Contribute GI)**, Receive Global Illumination = **Lightmaps**. Capsule là vật động.
- Directional = Mixed. Lighting Settings preview: 10 texel/unit, atlas tối đa 1024.

## Bạn làm (bake do bạn chủ động chạy)
1. Generate Lighting, mở Lighting Explorer / Baked Lightmaps xem số atlas và kích thước.
2. Đổi Lightmap Resolution 10 -> 20 -> 40; ghi dung lượng và thời gian bake.
3. Bật Scene view > Lightmap Resolution để xem mật độ texel.

## Đúng khi
- Sau bake có lightmap texture trong thư mục scene, vật tĩnh dùng lightmap, capsule thì không.
