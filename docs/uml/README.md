## Coffee Shop Management — Documentation and UML

This document contains a detailed project README (English) and a Khmer translation below each section. It also points to the UML PlantUML sources in this folder and explains how to render them locally.

---

## 1. Project Overview (English)

Coffee Shop Management is a cross-platform desktop application built with .NET 9 and Avalonia UI. It provides small coffee shop owners with a lightweight interface to manage products, create and track orders, and seed an initial SQL Server database for development and testing.

Key goals:
- Simple product CRUD (create, read, update, delete)
- Create orders with line items and calculate totals
- Lightweight UI with an embedded Order overlay
- Idempotent database initialization and seed data for development

## 1. ការពិពណ៌នា​គម្រោង (ភាសាខ្មែរ)

កម្មវិធី "ប្រព័ន្ធគ្រប់គ្រងហាងកាហ្វេ" ជាកម្មវិធី desktop ប្រព័ន្ធឆ្លងប្រព័ន្ធ (cross-platform) ដែលបាន build ដោយ .NET 9 និង Avalonia UI។ វាផ្តល់ឱ្យម្ចាស់ហាងកាហ្វេតូចៗនូវចំណុចបច្ចេកទេសមូលដ្ឋានសម្រាប់គ្រប់គ្រងផលិតផល បង្កើត និងតាមដានការបញ្ជាទិញ និងដំណើរការការដំឡើងទិន្នន័យ SQL Server សម្រាប់ការអភិវឌ្ឍន៍ និងសាកល្បង។

គោលបំណងសំខាន់ៗ៖
- ការគ្រប់គ្រងផលិតផល (CRUD)
- បង្កើតការបញ្ជាទិញ  និងគណនាតម្លៃសរុប
- UI ស្រាល និងមាន Order overlay ដែលបញ្ចូលក្នុងផ្ទៃ
- ការដំឡើង និងបញ្ចូលទិន្នន័យដើមដែលអាចធ្វើឡើងបានច្រើនដង (idempotent)

---

## 2. Features (English)

- Product management: add, update, delete, search
- Orders: create, add items, calculate totals, save to SQL Server
- UI: liquid-glass theme, embedded OrderForm control, localization-ready fonts
- Database: automatic creation and seed (SQL scripts in Database/Scripts)

## 2. មុខងារ (ភាសាខ្មែរ)

- ការគ្រប់គ្រងផលិតផល៖ បន្ថែម កែសម្រួល លុប និងស្វែងរក
- ការបញ្ជាទិញ៖ បង្កើត បន្ថែមធាតុ គណនាសរុប និងរក្សាទុកទៅ SQL Server
- UI៖ រចនាប្លង់ liquid-glass, OrderForm ដែលបញ្ចូលក្នុងផ្ទៃ, មានការគាំទ្រភាសា
- Database៖ បង្កើត និងបញ្ចូលទិន្នន័យដើមដោយស្វ័យប្រវត្តិ (ស្គ្រីបនៅ Database/Scripts)

---

## 3. Technology Stack (English)

- .NET 9
- Avalonia UI 11.x
- CommunityToolkit.Mvvm (source generators for ObservableProperty and RelayCommand)
- Microsoft.Data.SqlClient
- SQL Server (Docker container configured in docker-compose.yml)

## 3. បច្ចេកវិទ្យា (ភាសាខ្មែរ)

- .NET 9
- Avalonia UI 11.x
- CommunityToolkit.Mvvm (សម្រាប់ ObservableProperty និង RelayCommand)
- Microsoft.Data.SqlClient
- SQL Server (រត់នៅក្នុង Docker container ដែលកំណត់នៅ docker-compose.yml)

---

## 4. Project Structure (English)

Top-level folders of interest:

- `Constants/` — central constants
- `Converters/` — XAML converters
- `Helpers/` — small UI helpers (MessageBox)
- `Models/` — domain models (Product, Order, OrderItem, User, Category)
- `Services/` — `DatabaseService` handles all DB operations and initialization
- `ViewModels/` — ViewModel classes using CommunityToolkit.Mvvm
- `Views/` — Avalonia XAML views and user controls
- `Database/Scripts/` — SQL creation and seed scripts

## 4. រចនាសម្ព័ន្ធគម្រោង (ភាសាខ្មែរ)

ថតដើមដែលមានប្រយោជន៍៖

- `Constants/` — ការកំណត់តម្លៃរួម
- `Converters/` — សម្រាប់ XAML
- `Helpers/` — ជំនួយតូច (MessageBox)
- `Models/` — លក្ខណៈ domain (Product, Order, OrderItem, User, Category)
- `Services/` — `DatabaseService` ដែលមើលឃើញប្រតិបត្តិការទាំងអស់ជាមួយ DB និងការចាប់ផ្ដើម
- `ViewModels/` — ViewModel ដែលប្រើ CommunityToolkit.Mvvm
- `Views/` — Avalonia XAML views និង user controls
- `Database/Scripts/` — ស្គ្រីប SQL សម្រាប់បង្កើត និងបញ្ចូលទិន្នន័យ

---

## 5. Setup & Running (English)

1. Install prerequisites: .NET 9 SDK and Docker Desktop.
2. Start SQL Server container (from project root):

```bash
docker-compose up -d
```

3. Run the application:

```bash
cd CoffeeShopManagement
dotnet run
```

The application will attempt to initialize the database on first run. Check `Database/Scripts/CreateDatabase.sql` and `Database/Scripts/SeedData.sql` for the schema and seed data.

Notes & security:
- Do not store real passwords in source. Use environment variables or configuration with secrets (User Secrets for dev, Docker secrets / environment variables for containers, or a secrets manager in production).

## 5. ការតំឡើង និងដំណើរការ (ភាសាខ្មែរ)

1. តំឡើងអ្វីដែលត្រូវការ: .NET 9 SDK និង Docker Desktop។
2. ចាប់ផ្ដើម SQL Server container (ពីថតគម្រោង):

```bash
docker-compose up -d
```

3. ដំណើរការ​កម្មវិធី ៖

```bash
cd CoffeeShopManagement
dotnet run
```

កម្មវិធីនឹងព្យាយាម initialize database នៅលើការរត់លើកដំបូង។ សូមពិនិត្យ `Database/Scripts/CreateDatabase.sql` និង `Database/Scripts/SeedData.sql` សម្រាប់ schema និងទិន្នន័យដំបូង។

ចំណាំទំនាក់ទំនងសុវត្ថិភាព៖
- កុំផ្ទុកពាក្យសម្ងាត់ពិតប្រាកដក្នុង source។ ប្រើ environment variables ឬ configuration ដែលមាន secret management (User Secrets សម្រាប់ dev, Docker secrets / environment variables សម្រាប់ containers, ឬ secrets manager នៅ production)।

---

## 6. UML diagrams (English)

This folder contains PlantUML `.puml` sources for the core class diagram and some flow/sequence diagrams. To render them locally with Docker:

```bash
# from this docs/uml folder
docker run --rm -v $(pwd):/workspace plantuml/plantuml -verbose *.puml
```

The rendered PNG/SVG files will appear in the same folder.

## 6. រាបរាល់ UML (ភាសាខ្មែរ)

ថតនេះមានឯកសារ PlantUML `.puml` សម្រាប់ class diagram មួយចំនួន និង flow/sequence diagrams។ ដើម្បីបង្ហាញវាលើកមូលដ្ឋានដោយប្រើ Docker:

```bash
# ពីថត docs/uml
docker run --rm -v $(pwd):/workspace plantuml/plantuml -verbose *.puml
```

រូបភាព PNG/SVG។

---

## 7. Development Notes & Recommendations (English)

- Security: move DB credentials out of source and into environment/config secrets.
- DI & MVVM: keep Views thin; push logic into ViewModels or services. For dialogs, introduce an IDialogService so ViewModels don't instantiate Views directly.
- Validation: prefer in-form validation over MessageBox popups. Use DataAnnotations + UI binding for validation messages.
- Categories: consider normalizing categories into a `Categories` table instead of free-text category strings.
- Tests: add unit tests for `DatabaseService` (use a local test DB or in-memory test double) and for key ViewModel logic.

## 7. កំណត់ចំណាំអភិវឌ្ឍន៍ និងអនុសាសន៍ (ភាសាខ្មែរ)

- សុវត្ថិភាព: ទុកឯកសារ DB credentials ក្រៅពី source ហើយប្រើ environment/config secrets។
- DI & MVVM: រក្សា Views ឲ្យស្រួល; ផ្ទេរតក្កវិជ្ជាទៅ ViewModels ឬ services។ សម្រាប់ dialogs, បង្កើត `IDialogService` ដើម្បីឲ្យ ViewModels មិនបង្កើត Views ដោយផ្ទាល់។
- ការត្រួតពិនិត្យទិន្នន័យ: ប្រើ validation នៅក្នុង form ជាជម្រើសល្អជាងការបង្ហាញ MessageBox។ ប្រើ DataAnnotations + binding UI សម្រាប់សារ validation ។
- ក្រុមប្រភេទ៖ គួរតែពិចារណាការធ្វើ normalizing categories ទៅក្នុងតារាង `Categories` ជំនួសលទ្ធផល free-text ។
- ការ​តេស្ត៖ បន្ថែម unit tests សម្រាប់ `DatabaseService` (ប្រើ test DB មូលដ្ឋាន ឬ in-memory test double) និងសម្រាប់ logic ផ្លូវការ ViewModel ជាចម្បង ।

---

## 8. Troubleshooting (English)

- If the app fails to connect to SQL Server, confirm the Docker container is running and the host/port in `DatabaseService` connection string match the `docker-compose.yml` mapping.
- For XAML/AVLN type resolution warnings about DI-resolved Views, ensure Views have the same constructor signature expected by DI or register view factories in the DI container.

## 8. ការដោះស្រាយបញ្ហា (ភាសាខ្មែរ)

- ប្រសិនបើកម្មវិធីមិនអាចភ្ជាប់ទៅ SQL Server បាន សូមបញ្ជាក់ថា Docker container ចាប់ផ្ដើមហើយ និង host/port ក្នុង connection string របស់ `DatabaseService` ត្រូវគ្នានឹង mappings នៅ `docker-compose.yml` ។
- សម្រាប់ការព្រមាន XAML/AVLN អំពី type resolution ដែលទាក់ទងនឹង DI, សូមធ្វើឲ្យ Views មាន constructor signature ដូចម្តេចដែល DI ត្រូវការ ឬចុះបញ្ចាំង view factories នៅក្នុង DI container ។

---

## 9. Next steps (English)

- Replace hard-coded secrets with secure configuration
- Implement IDialogService and move view creation out of ViewModels
- Convert product categories to a normalized table
- Add unit and integration tests

## 9. ជំហានបន្ទាប់ (ភាសាខ្មែរ)

- ជំនួស secrets ដែលសរសេរដោយផ្ទាល់ជាមួយ configuration សុវត្ថិភាព
- អនុវត្ត `IDialogService` និងផ្លាស់ចេញការបង្កើត View ពី ViewModels
- បម្លែង categories ជាតារាង normalized
- បន្ថែម unit និង integration tests

---
