# 14 - Advanced (cookie, light shaft, fog)

**Mục tiêu:** giả lập ánh sáng xuyên cửa sổ bằng Cookie, so sánh với hình học thật, và thử light shaft rẻ tiền.

## Đã set up sẵn
- Phòng tối (Directional TẮT, ambient rất thấp). `Light_Spot_Cookie` với `Cookie_Window.png` (Texture Type = Cookie).
- `Window_Frame_Compare` (khung thật) + `Light_Spot_Compare` (TẮT) để so sánh cookie với bóng hình học thật.
- `LightShaft_Fake`: nón trong suốt (Unlit transparent) làm tia sáng giả. Fog TẮT (Lighting > Environment).

## Bạn làm
1. Xem vệt cửa sổ trên tường sau; bật `Light_Spot_Compare`, tắt Cookie, so sánh.
2. Bật/tắt `LightShaft_Fake`; bật Fog để thử độ khó của tia sáng thật.
3. Ghi lại cái nào rẻ hơn trên mobile (cookie + fake > khung thật + fog).

## Đúng khi
- Vệt sáng trên tường có dạng cửa sổ với thanh ngang/dọc; tia sáng giả nhìn ra dạng nón.
