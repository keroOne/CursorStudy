# PC Inventory Management API 仕様書

## 概要
PCの在庫管理を行うためのRESTful APIです。PC、ユーザー、OSタイプ、拠点の管理機能を提供します。

## データモデル

### PC (PC)
PCの基本情報を管理します。

```csharp
public class PC
{
    public int Id { get; set; }                    // PC ID
    public string ManagementNumber { get; set; }   // 管理番号（最大10文字）
    public string ModelName { get; set; }          // モデル名（最大100文字）
    public int OSTypeId { get; set; }              // OS種別ID
    public int? UserId { get; set; }               // 利用者ID
    public bool IsDeleted { get; set; }            // 削除フラグ
    public DateTime CreatedAt { get; set; }        // 作成日時
    public DateTime? UpdatedAt { get; set; }       // 更新日時

    // ナビゲーションプロパティ
    public virtual OSType? OSType { get; set; }    // OS種別
    public virtual User? User { get; set; }        // 利用者
}
```

### ユーザー (User)
PCの利用者情報を管理します。

```csharp
public class User
{
    public int Id { get; set; }                    // ユーザーID
    public string Name { get; set; }               // 氏名（最大50文字）
    public string ADAccount { get; set; }          // ADアカウント（最大50文字）
    public string DisplayName { get; set; }        // 表示名（最大100文字）
    public bool IsActive { get; set; }             // アクティブフラグ
    public bool IsDeleted { get; set; }            // 削除フラグ
    public DateTime CreatedAt { get; set; }        // 作成日時
    public DateTime? UpdatedAt { get; set; }       // 更新日時
    public int LocationId { get; set; }            // 拠点ID

    // ナビゲーションプロパティ
    public virtual Location? Location { get; set; } // 所属拠点
    public virtual ICollection<PC> PCs { get; set; } // 割り当てられたPC
}
```

### OS種別 (OSType)
PCのOS種別を管理します。

```csharp
public class OSType
{
    public int Id { get; set; }                    // OS種別ID
    public string Name { get; set; }               // OS名（最大50文字）
    public bool IsDeleted { get; set; }            // 削除フラグ
}
```

### 拠点 (Location)
ユーザーの所属拠点を管理します。

```csharp
public class Location
{
    public int Id { get; set; }                    // 拠点ID
    public string Code { get; set; }               // 拠点コード（最大10文字）
    public string Name { get; set; }               // 拠点名（最大20文字）
    public bool IsDeleted { get; set; }            // 削除フラグ

    // ナビゲーションプロパティ
    public ICollection<User> Users { get; set; }   // 所属ユーザー
}
```

## データベース設計

### テーブル定義

#### PCs テーブル
```sql
CREATE TABLE [PCs] (
    [Id] int NOT NULL IDENTITY,
    [ManagementNumber] nvarchar(10) NOT NULL,
    [ModelName] nvarchar(100) NOT NULL,
    [OSTypeId] int NOT NULL,
    [UserId] int NULL,
    [IsDeleted] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_PCs] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PCs_OSTypes_OSTypeId] FOREIGN KEY ([OSTypeId]) 
        REFERENCES [OSTypes] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_PCs_Users_UserId] FOREIGN KEY ([UserId]) 
        REFERENCES [Users] ([Id]) ON DELETE SET NULL
);
```

#### Users テーブル
```sql
CREATE TABLE [Users] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(50) NOT NULL,
    [ADAccount] nvarchar(50) NOT NULL,
    [DisplayName] nvarchar(100) NOT NULL,
    [IsActive] bit NOT NULL,
    [IsDeleted] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [LocationId] int NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Users_Locations_LocationId] FOREIGN KEY ([LocationId]) 
        REFERENCES [Locations] ([Id]) ON DELETE CASCADE
);
```

#### OSTypes テーブル
```sql
CREATE TABLE [OSTypes] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(50) NOT NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_OSTypes] PRIMARY KEY ([Id])
);
```

#### Locations テーブル
```sql
CREATE TABLE [Locations] (
    [Id] int NOT NULL IDENTITY,
    [Code] nvarchar(10) NOT NULL,
    [Name] nvarchar(20) NOT NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_Locations] PRIMARY KEY ([Id])
);
```

### インデックス
```sql
CREATE INDEX [IX_PCs_OSTypeId] ON [PCs] ([OSTypeId]);
CREATE INDEX [IX_PCs_UserId] ON [PCs] ([UserId]);
CREATE INDEX [IX_Users_LocationId] ON [Users] ([LocationId]);
```

## API エンドポイント

### PC管理

#### PC一覧の取得
```
GET /api/PCs
```

#### 特定のPCの取得
```
GET /api/PCs/{id}
```

#### PCの新規作成
```
POST /api/PCs
```

#### PCの更新
```
PUT /api/PCs/{id}
```

#### PCの削除（論理削除）
```
DELETE /api/PCs/{id}
```

### ユーザー管理

#### ユーザー一覧の取得
```
GET /api/Users
```

#### 特定のユーザーの取得
```
GET /api/Users/{id}
```

#### ユーザーの新規作成
```
POST /api/Users
```

#### ユーザーの更新
```
PUT /api/Users/{id}
```

#### ユーザーの削除（論理削除）
```
DELETE /api/Users/{id}
```

### OS種別管理

#### OS種別一覧の取得
```
GET /api/OSTypes
```

#### 特定のOS種別の取得
```
GET /api/OSTypes/{id}
```

#### OS種別の新規作成
```
POST /api/OSTypes
```

#### OS種別の更新
```
PUT /api/OSTypes/{id}
```

#### OS種別の削除（論理削除）
```
DELETE /api/OSTypes/{id}
```

### 拠点管理

#### 拠点一覧の取得
```
GET /api/Locations
```

#### 特定の拠点の取得
```
GET /api/Locations/{id}
```

#### 拠点の新規作成
```
POST /api/Locations
```

#### 拠点の更新
```
PUT /api/Locations/{id}
```

#### 拠点の削除（論理削除）
```
DELETE /api/Locations/{id}
```

## 認証・認可
現在の実装では認証・認可機能は実装されていません。

## エラーハンドリング
- 404 Not Found: リソースが見つからない場合
- 400 Bad Request: リクエストが不正な場合
- 500 Internal Server Error: サーバー内部でエラーが発生した場合 