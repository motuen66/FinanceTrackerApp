# FinanceTrackerApp — Hướng dẫn nhanh (INSTRUCTION)

Tệp này tóm tắt cách thiết lập, chạy, migrate và debug dự án FinanceTrackerApp (API ASP.NET Core + MongoDB).

---
## Tổng quan
- Backend: ASP.NET Core (net8.0). Project chính: `FinanceTracker.API`.
- Data: MongoDB (driver chính thức .NET). Các collection: `users`, `categories`, `transactions`, `budgets`, `savinggoals`, `reports`.
- Kiến trúc: Repository/Service/Controller, Domain models trong `FinanceTracker.Domain`.
- Migrations: các script mongosh (.js) nằm trong `FinanceTracker.Infrastructure/Database/migrations/`.
- Examples/seed: `FinanceTracker.Infrastructure/Database/migrations/examples/`

---
## Yêu cầu (Prerequisites)
- .NET SDK (>= 8.0). Kiểm tra: `dotnet --version`.
- MongoDB server (local) hoặc MongoDB Atlas. Nếu dùng mongod locally, cần MongoDB Database Tools (mongodump, mongoimport, mongosh).
- (Tuỳ chọn) MongoDB Compass để import/inspect documents dễ dàng.
- PowerShell (Windows) hoặc terminal tương đương.

---
## Cấu hình
1. Tệp config dev: `FinanceTracker.API/appsettings.Development.json`.
   - `MongoDbSettings.ConnectionString` và `DatabaseName` phải trỏ tới DB bạn muốn dùng.
   - `JwtConfig` (Issuer, Audience, Key, TokenValidityMins) — sửa `Key` bằng chuỗi đủ mạnh cho dev.
2. Bảo mật: KHÔNG commit secrets thật vào repo. Dùng `appsettings.Development.json` cho dev và thêm file vào `.gitignore` nếu cần.

---
## Chạy dự án (local)
1. Mở terminal ở root repo.
2. Build:

```powershell
dotnet build FinanceTracker.API/FinanceTracker.API.csproj
```

3. Chạy API:

```powershell
dotnet run --project FinanceTracker.API/FinanceTracker.API.csproj
```

4. Mở Swagger UI: `https://localhost:<port>/` (port từ `launchSettings.json` hoặc output khi chạy).

---
## Database: backup, migrate, seed
Luôn backup trước khi thay đổi DB quan trọng.

### Backup (mongodump)
```powershell
mongodump --uri="<YOUR_CONNECTION_STRING>/<DBNAME>" --archive="C:\backups\financetracker-$(Get-Date -Format yyyy-MM-dd).gz" --gzip
```

### Migrate (script)
- Các script mongosh có sẵn trong `FinanceTracker.Infrastructure/Database/migrations/`:
  - `migration_cleanup.js` — unset embedded objects, convert string ids -> ObjectId, convert numeric strings -> Decimal128, normalize dates.
  - `db_validators.js` — JSON Schema validators (apply sau khi migrate).
  - `create_indexes.js` — tạo index khuyến nghị.

Chạy script:
```powershell
mongosh "<CONNECTION_STRING_WITH_DB>" --file "<repoPath>\FinanceTracker.Infrastructure\Database\migrations\migration_cleanup.js"
```

### Seed / reseed dev DB (import examples)
Ví dụ import file example (Extended JSON) có sẵn:
```powershell
mongoimport --uri="<CONNECTION_STRING>/<DBNAME>" --collection=users --file="<repoPath>\FinanceTracker.Infrastructure\Database\migrations\examples\users.json" --jsonArray --mode=upsert --upsertFields=_id
```
Lặp cho `categories.json`, `transactions.json`, `budgets.json`, `savinggoals.json`, `reports.json`.

Nếu bạn không có `mongoimport`, dùng MongoDB Compass để import file JSON (Compass hỗ trợ Extended JSON tokens như `$oid`, `$date`, `$numberDecimal`).

### Áp validator và index
Sau migrate/seed, chạy:
```powershell
mongosh "<CONNECTION_STRING>/<DBNAME>" --file "<repoPath>\FinanceTracker.Infrastructure\Database\migrations\db_validators.js"
mongosh "<CONNECTION_STRING>/<DBNAME>" --file "<repoPath>\FinanceTracker.Infrastructure\Database\migrations\create_indexes.js"
```

---
## Model & Document shape (tóm tắt quan trọng)
- Các model lưu tham chiếu theo `userId`, `categoryId` kiểu ObjectId (driver map sang string do sử dụng `[BsonRepresentation(BsonType.ObjectId)]`).
- Các trường tiền tệ (`amount`, `limitAmount`, `goalAmount`, `currentAmount`, report totals) được annotate `[BsonRepresentation(BsonType.Decimal128)]` — DB nên lưu kiểu Decimal128.
- Tránh nhúng toàn bộ đối tượng (như `user`, `category`) trong document; nếu có, migration script sẽ unset field nhúng và chuyển sang `userId`/`categoryId`.

---
## JWT & Auth
- Config tại `appsettings.Development.json` trong khóa `JwtConfig` (Issuer, Audience, Key, TokenValidityMins).
- `JwtService` tạo token sử dụng `Key` làm symmetric key; đảm bảo `Key` có độ dài và entropy đủ.
- Đăng ký `JwtService` đã được thêm vào DI container trong `Program.cs`.

---
## Passwords
- Passwords được hash bằng PBKDF2 (PasswordHashHandler). Stored format: `{iterations}.{saltBase64}.{hashBase64}`.
- Tuyệt đối không lưu password plain-text. Nếu phải migrate plain-text users, viết migration job C# sử dụng `PasswordHashHandler.HashPassword` để re-hash.

---
## Debugging nhanh (các lỗi hay gặp)
1. FormatException khi insert/update (ví dụ `string is not a valid 24 digit hex string`):
   - Nguyên nhân: gửi giá trị không phải ObjectId cho trường annotate `[BsonRepresentation(BsonType.ObjectId)]`.
   - Fix: gửi giá trị id hợp lệ 24-hex hoặc validate/convert server-side trước khi insert.

2. Không thấy dữ liệu xuất hiện trong DB:
   - Kiểm tra `appsettings.*.json` connection string (app có thể ghi vào Atlas trong khi bạn kiểm tra local).
   - Thêm logging tạm vào `GenericRepository.AddAsync` để in collection + id sau insert (debug).

3. Mongodump/mongoimport không tìm thấy lệnh:
   - Cài MongoDB Database Tools cho Windows (theo docs MongoDB).

---
## Thực hành tốt & ghi chú
- Khi update 1 hoặc 2 field, dùng `UpdateOne` với `$set` (partial update) thay vì `ReplaceOne` để tránh mất các field khác.
- Viết migration idempotent: có thể chạy nhiều lần mà không gây hỏng.
- Test migration trên staging copy trước khi chạy production.

---
## Useful commands (tóm tắt)
- Kiểm tra 10 docs trong collection:
```powershell
mongosh "<CONN_STRING>/<DBNAME>" --eval "db.categories.find().limit(10).pretty()"
```
- Convert string id -> ObjectId (ví dụ budgets):
```javascript
db.budgets.updateMany({ userId: { $type: 'string' } }, [{ $set: { userId: { $convert: { input: '$userId', to: 'objectId', onError: '$userId', onNull: '$userId' } } } }]);
```
- Unset embedded `category` object -> set `categoryId`:
```javascript
db.budgets.find({ category: { $exists: true } }).forEach(function(doc){ if(doc.category && doc.category._id){ db.budgets.updateOne({ _id: doc._id }, { $set: { categoryId: doc.category._id }, $unset: { category: '' } }); } });
```

---
## Chạy tests / CI
- Hiện tại repo chưa có integration tests tự động cho migration. Nên thêm test để validate sample documents khớp với JSON schema.

---
## Nếu cần giúp thêm
- Tôi có thể:
  - Thêm script PowerShell `scripts/reseed-dev.ps1` (drop + import + validator + index).
  - Viết migration C# idempotent để re-hash password hoặc chuyển đổi phức tạp.
  - Thêm logging tạm để debug insert/connection.

Ghi chú: đọc kỹ file `FinanceTracker.Infrastructure/Database/migrations/` để biết scripts hiện có. Nếu bạn muốn tôi commit thêm tệp (script/ps1/validators), nói rõ và tôi sẽ tạo.

---
Cảm ơn — nếu bạn muốn mình tạo `scripts/reseed-dev.ps1` hoặc apply thêm validator cho `reports`, trả lời và tôi sẽ tiếp tục.