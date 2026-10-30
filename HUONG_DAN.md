# Hướng dẫn mở và kiểm tra 17 bài Shader Unity

Project: `F:\Project\test1\ShaderUnity` (Unity 6000.3.26f1, URP 17.3, Shader Graph 17.3).
Mỗi bài nằm trong `Assets/Exercises/<tên bài>/` gồm: `<tên bài>.unity` (scene), `Shaders/` (Shader Graph), `Materials/`, và `Textures/` hoặc `Mesh/` nếu bài cần.

## Cách làm chung cho mọi bài

1. **Mở scene:** trong cửa sổ Project, vào `Assets/Exercises/<bài>/`, bấm đúp file `.unity`.
2. **Xem graph:** vào `Shaders/`, bấm đúp file `.shadergraph`. Trong cửa sổ Shader Graph, bật **Blackboard** (danh sách property) và **Graph Inspector** (nút trên thanh công cụ). Các node đã được xếp từ trái sang phải theo luồng dữ liệu. Với bài có vertex, nhánh vertex nằm hàng trên, nhánh fragment hàng dưới.
3. **Đổi tham số:** chọn một Material trong `Materials/`, chỉnh trong Inspector. Mỗi Material gán sẵn cho một vật thể trong scene, nên bạn thấy kết quả ngay ở Scene/Game view. Chỉnh tên property thì ghi đúng tên hiển thị bên dưới.
4. **Xem chuyển động** (bài 03, 09, 11, 15, B): bấm **Play**, hoặc ở Scene view mở menu **Effects** rồi chọn **Always Refresh**. Nếu không, node Time đứng yên.
5. **Quy tắc debug của PDF:** nối tạm một nhánh vào `Base Color` để nhìn trực tiếp. Mask hiện thành xám, UV thành đỏ/xanh lá. Tắt từng nhánh, đổi một tham số mỗi lần.
6. **Sửa graph:** nhớ bấm **Save Asset** ở góc trên cửa sổ Shader Graph. Không lưu thì Material không đổi.

---

## 01 Shader vs Material
- **Mở:** `01_Shader_vs_Material/01_Shader_vs_Material.unity`. Graph `SG_Tint`: BaseMap nhân Tint, nối vào Base Color.
- **Vật thể:** `Sphere_Blue`, `Cube_Orange`, `Capsule_Purple` dùng 3 Material khác nhau (`Tint_Blue`, `Tint_Orange`, `Tint_Purple`), cùng một shader.
- **Thử:** chọn `Tint_Blue`, đổi **Tint** sang màu khác. Sau đó mở `SG_Tint`, nhân thêm một hằng số vào Base Color, lưu lại.
- **Đúng khi:** đổi Tint của một Material thì chỉ vật thể đó đổi màu. Sửa graph thì cả ba cùng đổi. Đó là sự khác nhau giữa Material và Shader.
- **Kiểm tra thêm:** mở Window > Analysis > Frame Debugger, bật Enable, xem ba vật thể có được gom draw call hay không.

## 02 Vertex vs Fragment
- **Mở:** `02_Vertex_vs_Fragment/02_Vertex_vs_Fragment.unity`. Graph `SG_VertexOffset`: `Position (Object)` + `(0, Offset, 0)` nối vào Vertex Position.
- **Vật thể:** hai cặp sphere và cube. Cặp "Original" dùng Offset 0, cặp "Shifted" dùng Offset 0.2.
- **Thử:** chọn `Shifted_Offset0.2`, kéo **Offset** từ 0 đến 1. Xoay cube "Shifted" trước rồi kéo lại.
- **Đúng khi:** vật thể dịch lên theo trục Y **cục bộ** của nó (cube xoay thì dịch theo hướng mới). Đổi **Color** không làm mesh di chuyển. Vậy vertex đổi hình học, fragment chỉ đổi màu.

## 03 Texture + UV
- **Mở:** `03_Texture_UV/03_Texture_UV.unity`. Graph `SG_ScrollUV`: UV → Rotate (tâm 0.5, 0.5) → Tiling And Offset (Offset = Time × Speed) → Sample Texture 2D.
- **Vật thể:** `Sphere_ScrollX` (Speed 0.1, 0), `Cube_ScrollY` (Speed 0, 0.1), `Capsule_Rotated` (xoay 0.785 rad, không cuộn). Texture checker có một ô đỏ ở góc trên trái để dễ nhìn hướng.
- **Thử:** Play. Đổi **Tiling** thành (4, 4), đổi **Speed** thành (0, 0.1), đổi **Rotation (rad)**.
- **Đúng khi:** họa tiết cuộn đúng hướng bạn dự đoán trước khi đổi Speed. Tiling tăng thì ô nhỏ lại và nhiều hơn, texture không mờ thêm. Xoay quay quanh giữa ảnh, không quanh góc.

## 04 Normal
- **Mở:** `04_Normal/04_Normal.unity`. Graph `SG_NormalMap` (Lit): Sample Texture 2D (Type Normal) → Normal Strength → Normal (Tangent Space).
- **Vật thể:** ba sphere với **Strength** 0, 0.5, 1 (`Strength_0`, `Strength_0.5`, `Strength_1`).
- **Thử:** chọn Directional Light trong Hierarchy và xoay nó. Đổi **Strength** lên 2.
- **Đúng khi:** sphere Strength 0 trơn, sphere Strength 1 có các nốt tròn, sáng tối đổi theo hướng đèn. Viền ngoài (silhouette) của ba sphere **giống nhau**: normal map không thêm hình học.

## 05 Coordinate Space
- **Mở:** `05_Coordinate_Space/05_Coordinate_Space.unity`. Hai graph giống nhau chỉ khác Space của Position: `SG_Space_Object` và `SG_Space_World` (sọc `sin(x · K)`, K = 12).
- **Vật thể:** `Sphere_Object`, `Cube_Object` (dùng Object), `Sphere_World`, `Cube_World` (dùng World).
- **Thử:** chọn từng vật thể, dùng Move và Rotate trong Scene view. Đổi **K** trong Material.
- **Đúng khi:** vật thể dùng Object: sọc đi theo vật thể khi di chuyển/xoay. Vật thể dùng World: sọc đứng yên trong cảnh, vật thể trượt qua các sọc.
- **Tự kiểm tra (chưa dựng sẵn, bài tập cho bạn):** tạo vùng tô màu theo độ cao world Y: `Position (World)` → Split G → Remap hoặc Smoothstep → Lerp. Hai vật thể cùng độ cao thì cùng mức màu dù pivot khác nhau.

## 06 Math cơ bản
- **Mở:** `06_Math/06_Math.unity`. Graph `SG_Math`: UV.x qua Add, Multiply, Saturate, Power, và hai nhánh Smoothstep / Step trộn bằng Lerp.
- **Vật thể:** 6 quad từ trái sang phải: `0_Original`, `1_Add0.2`, `2_Multiply2`, `3_Power3`, `4_Smoothstep`, `5_Step`. Quad 0 là gradient gốc để so sánh.
- **Thử:** mỗi Material chỉnh một tham số (Add, Multiply, Power, Smoothstep Mix/Edge1/Edge2, Step Mix/Step Edge).
- **Đúng khi:** Add 0.2 làm sáng đều, Multiply 2 làm gradient dốc hơn và bị kẹp ở trắng, Power 3 giữ vùng tối lâu hơn, Smoothstep (0.4 đến 0.6) cho chuyển tiếp mềm, Step 0.5 cho ranh giới cứng.
- **Tự kiểm tra của PDF (làm trên Material 1 hoặc 2):** để gradient đen → trắng chỉ trong đoạn U = 0.25 đến 0.75, đặt **Add = -0.25** và **Multiply = 2**. Ngoài đoạn này giá trị bị kẹp về 0 và 1.

## 07 Lerp
- **Mở:** `07_Lerp/07_Lerp.unity`. Graph `SG_Lerp`: `Lerp(ColorA, ColorB, T)`, T = `Lerp(Blend, UV.x, Use UV.x Gradient)`.
- **Vật thể:** ba sphere Blend 0, 0.5, 1 và một cube dùng gradient UV.x (`Gradient_UVx`).
- **Thử:** kéo **Blend** của một Material, đổi **ColorA/ColorB**, kéo **Use UV.x Gradient** từ 0 đến 1.
- **Đúng khi:** Blend 0 là màu A, 1 là màu B, 0.5 là màu trộn giữa. Hình học không đổi. Cube chuyển màu theo chiều ngang mỗi mặt.

## 08 Mask
- **Mở:** `08_Mask/08_Mask.unity`. Graph `SG_Mask`: `1 - smoothstep(Edge1, Edge2, distance(UV, 0.5))`.
- **Vật thể:** `Quad_MaskView` (Show Mask = 1, nhìn mask đen trắng), `Quad_Colored` (Lerp hai màu theo mask), `Quad_Ring` (Ring Mix = 1, thành vành), `Sphere_Colored`.
- **Thử:** đổi **Edge1/Edge2** (viền cứng hay mềm), kéo **Ring Mix**, kéo **Show Mask**.
- **Đúng khi:** mask là hình tròn mềm: trắng ở tâm, đen ngoài rìa. Quad Ring có tâm và phần ngoài cùng màu A (xanh), chỉ vành tròn là màu B (cam). Trên sphere hình tròn bị méo thành elip vì UV của sphere méo (đúng như PDF cảnh báo).

## 09 Time + Sine
- **Mở:** `09_Time_Sine/09_Time_Sine.unity`. Graph `SG_Pulse`: `0.5 + 0.5·sin(2π · f · Time + Phase)`, rồi `Lerp(Min Brightness, 1, pulse)` nhân Color.
- **Vật thể:** bốn sphere: `A_0.5Hz_Phase0`, `B_0.5Hz_PhasePi`, `C_1Hz`, `D_2Hz`.
- **Thử:** **bấm Play** (hoặc Always Refresh), dùng đồng hồ bấm giờ.
- **Đúng khi:** sphere A và B (0.5 Hz) luân phiên: A sáng thì B tối. Mỗi quả hoàn thành một chu kỳ trong khoảng 2 giây. C chu kỳ 1 giây, D chu kỳ 0.5 giây. Không quả nào tắt hẳn vì có Min Brightness.

## 10 Fresnel
- **Mở:** `10_Fresnel/10_Fresnel.unity`. Graph `SG_Fresnel` (Lit): Normal(World) + View Direction(World) → Fresnel Effect → nhân (Rim Color × Intensity) → Emission. Base Color tối.
- **Vật thể:** ba sphere với **Power** 1, 3, 8.
- **Thử:** xoay camera quanh sphere trong Scene view. Đổi **Intensity**. Tắt Directional Light.
- **Đúng khi:** viền cyan nằm ở mép nhìn nghiêng, Power càng cao thì viền càng mỏng và sát mép. Xoay camera thì vùng sáng đi theo góc nhìn. Tắt đèn thì viền **vẫn sáng** (vì là Emission).

## 11 Noise
- **Mở:** `11_Noise/11_Noise.unity`. Graph `SG_Noise`: Gradient Noise cuộn theo Time, nhánh Step, nhánh méo UV của texture checker.
- **Vật thể:** bốn quad: `Noise_Gray` (noise xám), `Noise_Step` (noise cắt đen trắng), `Distort_0.05` (checker bị méo), `Distort_0` (checker không méo).
- **Thử:** Play. Đổi **Noise Scale**, **Step Threshold**, **Distort Strength** từ 0 lên 0.05 rồi 0.1.
- **Đúng khi:** Scale lớn thì chi tiết nhỏ, Scale nhỏ thì mảng to. Noise trượt mượt, không nhấp nháy ngẫu nhiên từng khung hình. Quad `Distort_0` bằng đúng checker gốc, nghĩa là **Strength = 0 trả về UV ban đầu**.

## 12 Alpha / Alpha Clip
- **Mở:** `12_Alpha_Clip/12_Alpha_Clip.unity`. Ba graph: `SG_AlphaBlend` (Transparent, mask radial → Alpha), `SG_AlphaClip` (Opaque + Alpha Clipping, Threshold 0.5), `SG_DissolveBasic` (noise → Alpha, Threshold 0 đến 1). Nền sáng phía sau để thấy xuyên thấu.
- **Vật thể:** hai quad transparent chồng lên nhau (cam và xanh), một quad Clip, một quad Dissolve.
- **Thử:** chọn `Dissolve_0.5`, kéo **Threshold** từ 0 lên 1. Chọn `Clip_Orange`, kéo **Threshold**.
- **Đúng khi:** quad transparent cho thấy nền xuyên qua vùng mờ, và vùng hai quad chồng nhau trộn màu. Quad Clip có mép **cứng**, không xuyên mờ: vùng alpha 0.7 vẫn đặc. Dissolve thì phần còn lại **co dần** khi Threshold tăng.
- **Tự kiểm tra của PDF:** hãy giải thích vì sao vùng alpha 0.7 trong bản Opaque Clip vẫn không nhìn xuyên được (clip chỉ giữ hoặc bỏ, không trộn).

## 13 Emission
- **Mở:** `13_Emission/13_Emission.unity`. Graph `SG_Emission` (Lit): Emission = Glow Color (HDR) × Intensity × mask tròn. Scene có sẵn **Global Volume** với Bloom và Post Processing đã bật trên camera.
- **Vật thể:** ba cube có Intensity 0, 2, 5.
- **Thử:** chọn Global Volume, tắt Bloom (bỏ dấu tick) rồi bật lại. Giảm cường độ Directional Light về 0.
- **Đúng khi:** Intensity 0 thì chỉ thấy vòng tối. Intensity 2 và 5 thì vòng tròn sáng rõ, Intensity 5 có quầng lan ra ngoài (Bloom). **Tắt Bloom thì quầng biến mất nhưng vòng sáng vẫn còn**: đó là sự khác nhau giữa màu Emission và hiệu ứng hậu kỳ Bloom. Giảm đèn chính thì vòng vẫn sáng.

## 14 Lighting
- **Mở:** `14_Lighting/14_Lighting.unity`. Graph `SG_ManualLight` (Unlit, tự tính ánh sáng): `saturate(dot(N, L))`, Ambient, toon bằng Step, và Flat Mix để so sánh.
- **Vật thể:** `Sphere_Flat`, `Sphere_Lambert`, `Sphere_Toon`, `Cube_Lambert`.
- **Thử:** đổi **Light Direction** trong Material (ví dụ (1, 0, 0), rồi (0, 1, 0)). Xoay Directional Light của scene.
- **Đúng khi:** Flat đều một màu, Lambert chuyển sắc mượt theo hướng, Toon chỉ có hai mức sáng tối. Mặt hướng về Light Direction sáng nhất. **Xoay Directional Light của scene không ảnh hưởng gì**, vì graph này dùng hướng sáng tự khai báo (đúng như PDF nói: chưa kết nối đèn thật).

## 15 Vertex Displacement
- **Mở:** `15_Vertex_Displacement/15_Vertex_Displacement.unity`. Graph `SG_Wave`: dịch Y theo `sin(x · Frequency + Time · Speed) · Amplitude`, nhân mask `UV.y`. Mesh `Mesh/DensePlane.asset` là plane 64×64 ô (cần nhiều đỉnh thì sóng mới mượt).
- **Vật thể:** `Plane_Wave` (Mask Strength = 0, cả mặt đều gợn) và `Plane_RootLocked` (Mask Strength = 1, một mép đứng yên).
- **Thử:** Play. Hạ camera thấp, nhìn gần ngang mặt phẳng. Đổi **Amplitude** lên 0.3, **Frequency** lên 10.
- **Đúng khi:** nhìn ngang thấy **hình bóng (silhouette) đổi**, không chỉ màu hay UV chạy. Plane thứ hai có mép UV.y = 0 đứng yên, mép đối diện lay mạnh nhất.
- **Lưu ý của PDF:** collider và bounds culling không tự theo hình dạng GPU. Vật thể biến mất khi camera ở rìa nghĩa là bounds nhỏ quá.

---

## A Dissolve (bài tổng hợp)
- **Mở:** `A_Dissolve/A_Dissolve.unity`. Graph `SG_Dissolve` (Lit, Opaque, Alpha Clipping): noise → Alpha, Threshold → Alpha Clip Threshold, viền = `Step(T, n) - Step(T + EdgeWidth, n)` nhân EdgeColor × Intensity → Emission.
- **Vật thể:** `Sphere_T0.2`, `Sphere_T0.5`, `Sphere_T0.8` (Threshold 0.2, 0.5, 0.8), `Capsule_EdgeWide` (viền 0.15), `Cube_EdgeThin` (viền 0.02). Có Bloom.
- **Đúng khi (nghiệm thu của PDF):** Threshold 0.2, 0.5, 0.8 thì diện tích còn lại **giảm dần**. Đổi **Edge Width** làm viền dày hoặc mỏng. **Không có viền sáng trên phần đã bị loại**. Viền sáng nằm ở mép các lỗ trên phần còn lại; vì giá trị HDR lớn hơn 1 nên lõi viền có thể hiện vàng trắng, ngoài rìa là cam.
- **Kiểm tra thêm của PDF (chưa làm, để bạn thử):** bóng đổ của vật thể dissolve, camera ở xa, và nhiều vật dissolve cùng lúc.

## B Cỏ lay (bài tổng hợp)
- **Mở:** `B_Co_Lay/B_Co_Lay.unity`. Graph `SG_Grass` (Unlit, hiển thị hai mặt): mask gốc `Power(Saturate(UV.y), 2)`, dao động `sin(y · Frequency + Time · Speed + Phase)`, dịch theo `normalize(WindDirection)` và **cộng** vào Position gốc. Mesh `Mesh/GrassTuft.asset` là cụm 9 lá.
- **Vật thể:** 5 cụm cỏ, hai Material (`Grass_Phase0`, `Grass_Phase1.7`) xen kẽ. Cụm cuối xoay 90°.
- **Thử:** Play. Đổi **Amplitude**, **Speed**, **Wind Direction**, **Root Lock Power**.
- **Đúng khi (nghiệm thu của PDF):** gốc **không trượt**, ngọn lay. Nhìn ngang vẫn thấy silhouette đổi. Các cụm không lay đồng bộ (khác Phase). Cụm xoay 90° lay theo hướng đã xoay, vì Wind Direction nằm trong **object space**. Nếu cần gió theo thế giới thì phải chuyển hướng sang object space.
- **Thử "quên cộng":** trong graph, bỏ nối Position gốc vào Add và chỉ nối offset thẳng vào Vertex Position. Cả mesh co về gốc tọa độ, đúng lỗi PDF cảnh báo.

---

## Kiểm tra hiệu năng mobile (checklist cuối PDF)
Các bài này chưa đo trên điện thoại, nên đây là việc bạn làm tiếp:
- Build lên thiết bị đại diện, cùng scene, cùng góc camera, cùng độ phân giải.
- Dùng Window > Analysis > **Profiler** (CPU/GPU) và **Frame Debugger** (xem pass, draw call). Frame Debugger không thay được đo thời gian GPU.
- Chú ý bài có **transparent** (12) và **Bloom** (13, A): tốn fill rate/overdraw nhất.
- Ghi lại mỗi lần đổi: thiết bị/API, độ phân giải, số instance, CPU ms, GPU ms nếu đo được, ảnh trước/sau.
