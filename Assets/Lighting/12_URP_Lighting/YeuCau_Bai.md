# 12 - URP Lighting (mobile)

**Mục tiêu:** so sánh 3 cấu hình URP theo bảng trong PDF (không cam kết FPS).

## Đã set up sẵn (3 URP Asset trong thư mục này, chưa gán toàn project)
| Hạng mục | `URP_TietKiem` | `URP_CanBang` | `URP_ThuChatLuong` |
|---|---|---|---|
| Main shadow map | 1024 | 2048 | 2048 |
| Shadow Distance | 12 | 20 | 25 |
| Cascade | 1 | 2 | 2 |
| Soft Shadows | Tắt | Bật | Bật |
| Additional Lights | Disabled | Per Pixel, limit 2 | Per Pixel, limit 4 |
| Additional Shadows | Tắt | Tắt | Bật |
| HDR | Tắt | Bật | Bật |
| Render Scale | 1.0 | 1.0 | 1.0 |

## Bạn làm
1. Project Settings > Graphics (và Quality) gán từng asset, ghi lại hình ảnh + frame time.
2. Chọn 1 cấu hình làm baseline cho game, KHÔNG bật nhiều đèn chỉ vì bảng cho phép.

## Đúng khi
- Đổi asset thấy rõ khác biệt bóng (độ phân giải/khoảng cách) và có/không đèn phụ.
