# 07 - Global Illumination

**Mục tiêu:** thấy ánh sáng đổi màu (color bleeding) từ vật phản xạ và emissive ánh sáng lên vật khác.

## Đã set up sẵn
- `Wall_Red` bên trái, Sàn + Sphere xám, đều **Contribute GI**. `Wall_Blue` mặt vật liệu sẵn để bạn thử đổi tường.
- Directional và Spot đều **Baked**; Spot hướng vào Wall_Red để màu đỏ bật lên sàn/sphere khi bake.
- `Emissive_Sign` (Emission HDR cam) làm nguồn sáng bake. `Volume_Bloom` TẮT (bật để xem Bloom).
- Lighting Settings preview: 10 texel/unit, atlas 1024, ít màu, 2 bounce, ProgressiveCPU.

## Bạn làm (bake do bạn chủ động chạy)
1. Generate Lighting; quan sát vệt màu đỏ trên Sàn và Sphere.
2. Đổi Wall_Red sang vật liệu `Wall_Blue` và bake lại, so sánh.
3. Bật `Volume_Bloom` để thấy bloom trên Emissive_Sign.

## Đúng khi
- Sphere xám bị nhuộm đỏ nhạt ở phía gần tường đỏ sau khi bake.
