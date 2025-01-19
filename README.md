1. Demo tính năng AcessToken và RefreshToken, sử dụng khoá đối xứng SymmetricSecurityKey
- **Mô tả**: 
  + Khi user đăng nhập, server cấp 1 cặp AccessToken và RefreshToken cho client, AccessToken có Expiretime khoảng 60 giây nhằm demo tính năng RefreshToken.
  + RefreshToken được lưu vào Redis Cache nhằm cấp lại AccessToken mới và RefreshToken mới.
  + Trong project demo này chỉ sử dụng JWT với cơ chế stateless, không lưu thông tin người dùng bằng session nhằm tránh rủi ro về bảo mật và đáp ứng khả năng scalability của hệ thống trong tương lai.
  + Trong project demo này có sử dụng logging với NLog library nhằm ghi lại các trạng thái đăng nhập của người dùng.
  + Trong project demo này có sử dụng quản lý phiên bản API bằng API Versioning.
- **Các library sử dụng trong project demo**: Chạy trong Package Manage Consoler với "-v" là version của library, mình **KHÔNG** sử dụng cách cài trực tiếp thông qua Extension Manager.
  + install-package Microsoft.AspNetCore.Authentication.JwtBearer -v 9.0.0
  + install-package Microsoft.AspNetCore.Mvc.Versioning -v 5.1.0
  + install-package Microsoft.AspNetCore.OpenApi -v 9.0.0
  + install-package Microsoft.Extensions.Caching.StackExchangeRedis -v 9.0.1
  + install-package nlog.extensions.logging -v 5.3.15
  + install-package NLog.Web.AspNetCore -v 5.3.15
  + install-package StackExchange.Redis -v 2.8.24

2. Các lưu ý trong project
- Redis Cache chạy trên Docker với cổng mặc định 6379 (mình sẽ không hướng dẫn cách install Redis trên Docker, bạn đọc có thể tự học phần này).
- Sử dụng Frontend cùng với Visual Studio Code (không sử dụng Razor) và cài đặt server riêng cho Frontend, ở đây mình sử dụng Live Server cho Frontend với Domain http://127.0.0.1:5500
- Sau khi cài đặt Live Server xong, Chạy Frontend ở Visual Studio Code với Live Server bằng cách: chuột phải vào file index.html ở folder Frontend -> chọn Open With Live Server
- **Tuyệt đối không sử dụng file tĩnh để demo.**
- Phần Backend, chạy trên https (**Without using IIS Express**).
- Demo này chỉ tập trung vào việc Authentication, nên không sử dụng database như SqlServer mà chỉ sử dụng Redis Cache.
- Sau khi Authentication xong sẽ dễ dàng thực hiện Authorization thông qua .Net Core Identity với class RoleManager (phần này mình sẽ demo kỹ hơn trong dự án thực tế - sẽ build sau).
- Project demo này tương ứng với phần lý thuyết mình sẽ đăng trên trang cá nhân đính kèm https://www.facebook.com/dungvuphamtien
  
