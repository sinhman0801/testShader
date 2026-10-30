# C - Puzzle Fixed Camera (bài tổng hợp)

**Mục tiêu:** ánh sáng cho puzzle camera cố định: đọc được khối hình, rẻ, không flicker.

## Đã set up sẵn
- Camera **Orthographic** cố định (size 4) nhìn chéo xuống; cụm 4x4 cube xám nhạt (Metallic 0, Smoothness 0.3/0.4).
- 1 Directional duy nhất (Hard shadows), ambient Flat; **Bloom/Post tắt**; Sàn Static, các cube động (để chuyển).
- `Arrow_Marks`: 3 mũi tên phẳng (mesh sinh ra) chỉ hướng di chuyển.

## Bạn làm
1. Thử giảm Shadow Distance quanh vùng chơi trước khi tăng resolution (xem bài 12).
2. Xoay Directional để mỗi mặt cube có độ sáng khác nhau -> dễ phân biệt khối.
3. Bật Bloom (nếu muốn) và đo tác động.

## Đúng khi
- Ba mặt cube có 3 mức sáng rõ ràng, mũi tên đọc được, không có bóng 'vỡ' ở rìa của camera cố định.
