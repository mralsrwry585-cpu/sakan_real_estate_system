<div dir="rtl">

# 🏘️ سكن — نظام إدارة العقارات الشامل

> منصّة متكاملة لإدارة العقارات والتأجير، مبنية وفق **المعمارية النظيفة (Clean Architecture)** وتصميم **CQRS**، بواجهة عربية كاملة تدعم اتجاه **RTL**.

---

## ✨ نبذة عن المشروع

**سكن (SAKAN)** هو نظام إدارة عقارات احترافي يتكوّن من واجهة ويب وواجهة برمجية (API) منفصلة، يمكّن أصحاب العقارات (المُؤجِرون) من:

- إدارة العقارات ونشرها عبر **مساعد إدخال متعدد الخطوات (Wizard)**.
- إرفاق الوسائط المتعددة (صور) وترتيبها وتحديد الغلاف.
- استقبال وإدارة **طلبات المعاينة (Viewing Requests)**.
- استقبال وإدارة **طلبات الحجز (Booking Requests)**.
- متابعة الأداء عبر **لوحة تحكم وتحليلات (Dashboard & Analytics)**.
- إدارة **المرافق والخدمات (Amenities)** المرتبطة بالعقارات.

---

## 🚀 المميزات الرئيسية

| # | الميزة | الوصف |
|---|--------|-------|
| 1 | 🔐 المصادقة والتفويض | تسجيل دخول / إنشاء حساب مع صلاحيات (مؤجّر / مستأجر) وواجهة رفض وصول |
| 2 | 🏠 إدارة العقارات | إنشاء وتعديل وحذف العقارات، تغيير الحالة، بحث وتصفية صفحاتية |
| 3 | 🖼️ إدارة الوسائط | رفع الصور، حذف، إعادة ترتيب، تحديد صورة الغلاف |
| 4 | 🏷️ المرافق | كتالوج مرافق قابل للربط بكل عقار |
| 5 | 👁️ طلبات المعاينة | إنشاء طلبات معاينة، قبول / رفض، تحديد موعد ووقت |
| 6 | 📅 طلبات الحجز | إنشاء طلبات حجز، اعتماد / رفض، متابعة الحالة |
| 7 | 📊 لوحة التحكم | إحصائيات المالك، توزيع الحالات، أحدث الطلبات |

---

## 🛠️ التقنيات المستخدمة

| الطبقة | التقنية |
|--------|---------|
| اللغة | **C# / .NET 10** |
| الواجهة | **ASP.NET Core MVC (Razor)** — واجهة عربية RTL |
| الـ API | **ASP.NET Core Web API** مع **Swagger / OpenAPI** |
| المصادقة | **JWT Bearer** |
| قاعدة البيانات | **SQL Server** عبر **Entity Framework Core** |
| المعمارية | **Clean Architecture + CQRS** مع **MediatR** |
| التحقق من الصحة | **FluentValidation** |
| رسم الكائنات | **AutoMapper** |

---

## 📐 بنية المشروع (Clean Architecture)

```
SAKAN_PRO/
├── SAKAN.Domain/            # طبقة النطاق — الكيانات، التعدادات، إعدادات EF
├── SAKAN.Application/       # طبقة التطبيق — أوامر واستعلامات CQRS، التحقق، DTOs
├── SAKAN.Infrastructure/    # طبقة البنية التحتية — EF Core، الهجرات (Migrations)
├── SAKAN.API/               # واجهة برمجية REST — Controllers، JWT، Swagger
└── MyRealEstate.Web/        # واجهة الويب MVC — Views، السيرفرات، CSS/JS
```

### طبقات النطاق والتطبيق

| الوحدة | الوصف |
|--------|-------|
| `Features/Auth` | تسجيل الدخول وإنشاء الحسابات (Register/Login) |
| `Features/Properties` | إدارة العقارات (CRUD + تغيير الحالة) |
| `Features/Media` | إدارة وسائط العقار (رفع/حذف/ترتيب/غلاف) |
| `Features/Amenities` | جلب كتالوج المرافق |
| `Features/ViewingRequests` | طلبات المعاينة وإدارة حالتها |
| `Features/BookingRequests` | طلبات الحجز وإدارة حالتها |
| `Features/Analytics` | إحصائيات لوحة تحكم المالك |
| `Common/Behaviours` | وسيط التحقق من صحة الأوامر (ValidationBehaviour) |
| `Common/Mapping` | ملفات AutoMapper |

---

## ✅ المتطلبات الأساسية

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- **SQL Server** (Express أو أعلى)
- أي محرر أكواد (يُفضَّل **Visual Studio 2026** أو **VS Code**)

---

## 🚦 التشغيل المحلي

### 1) تجهيز قاعدة البيانات

ثبّت الحزم أولاً ثم طبّق الهجرات:

```bash
dotnet restore
dotnet build
cd SAKAN.Infrastructure
dotnet ef database update
```

### 2) تشغيل الـ API

```bash
cd SAKAN.API
dotnet run
```

- العنوان الافتراضي: `http://localhost:5226`
- واجهة Swagger: `http://localhost:5226/swagger`

### 3) تشغيل واجهة الويب

```bash
cd MyRealEstate.Web
dotnet run
```

- العنوان الافتراضي: `http://localhost:5000` (أو حسب إعدادات `launchSettings.json`)
- تعديل `ApiSettings:BaseUrl` في `appsettings.json` عند تغيير منفذ الـ API.

---

## 🔌 نقاط النهاية الرئيسية للـ API

| الوحدة | الطرق |
|--------|-------|
| المصادقة | `POST /api/auth/login`، `POST /api/auth/register` |
| العقارات | `GET/POST /api/properties`، `GET/PUT/DELETE /api/properties/{id}`، `PATCH .../status` |
| الوسائط | `POST /api/properties/{id}/media`، `DELETE .../media/{mediaId}`، `PUT .../media/cover`، `PUT .../media/reorder` |
| طلبات المعاينة | `GET/POST /api/viewingrequests`، `PATCH .../{id}/status` |
| طلبات الحجز | `GET/POST /api/bookingrequests`، `PATCH .../{id}/status` |
| المرافق | `GET /api/amenities` |
| التحليلات | `GET /api/analytics/owner/{ownerId}` |

---

## 🌿 سير عمل الفروع (Git Workflow)

يعتمد المشروع على سير عمل الفروع المنفصلة مع إنشاء Pull Requests حقيقية:

```
main (الإصدار المُستقر)
   └── develop (بيئة التطوير)
         ├── feature/authentication
         ├── feature/properties
         ├── feature/property-media
         ├── feature/amenities
         ├── feature/viewing-requests
         ├── feature/booking-requests
         └── feature/dashboard
```

> فرع `backup/current-complete` يحفظ نسخة أمان كاملة من المشروع الأصلي.

---

## 🗂️ مصدر المشروع

- **مستودع GitHub:** [mralsrwry585-cpu/sakan_real_estate_system](https://github.com/mralsrwry585-cpu/sakan_real_estate_system)

---

## 📄 الترخيص

حقوق المشروع محفوظة لأصحابها. يُرجى التواصل مع المطوّر الرئيسي قبل إعادة الاستخدام لأغراض تجارية.

</div>
<hr />
<p align="left">
<sub>🏗️ <b>SAKAN</b> — Real Estate Management System · Built with .NET 10 · Clean Architecture & CQRS</sub>
</p>