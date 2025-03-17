# PC Inventory Management System

## 概要
PCの在庫管理を行うためのシステムです。PC、ユーザー、OS種別、拠点の管理機能を提供します。

## システム構成
- バックエンド: ASP.NET Core Web API
- フロントエンド: Vue.js 3 + Vuetify 3
- データベース: SQL Server

## 機能一覧
- PC管理
  - PC一覧表示
  - PC詳細表示
  - PC新規登録
  - PC情報更新
  - PC削除（論理削除）
- ユーザー管理
  - ユーザー一覧表示
  - ユーザー詳細表示
  - ユーザー新規登録
  - ユーザー情報更新
  - ユーザー削除（論理削除）
- OS種別管理
  - OS種別一覧表示
  - OS種別新規登録
  - OS種別情報更新
  - OS種別削除（論理削除）
- 拠点管理
  - 拠点一覧表示
  - 拠点新規登録
  - 拠点情報更新
  - 拠点削除（論理削除）

## プロジェクト構成

### バックエンド（PCInventoryManagement.API）
```
PCInventoryManagement.API/
├── Controllers/         # APIコントローラー
├── Models/             # データモデル
├── Data/               # データベース関連
└── Properties/         # アプリケーション設定
```

### フロントエンド（PCInventoryManagement.Web）
```
PCInventoryManagement.Web/
├── src/
│   ├── api/           # API通信
│   ├── components/    # 共通コンポーネント
│   ├── views/         # 画面コンポーネント
│   ├── router/        # ルーティング
│   ├── types/         # 型定義
│   └── App.vue        # ルートコンポーネント
└── public/            # 静的ファイル
```

## セットアップ手順

### 必要環境
- .NET 8.0 SDK
- Node.js 18以上
- SQL Server 2019以上

### バックエンド
1. データベースの作成
```bash
cd PCInventoryManagement.API
dotnet ef database update
```

2. APIの起動
```bash
dotnet run
```

### フロントエンド
1. 依存パッケージのインストール
```bash
cd PCInventoryManagement.Web
npm install
```

2. 開発サーバーの起動
```bash
npm run dev
```

## 仕様書
- [API仕様書](docs/specifications/api-specification.md)
- [Web仕様書](docs/specifications/web-specification.md)

## 開発ガイドライン
- コーディング規約に従う
- 単体テストを作成する
- コミットメッセージは具体的に記述する
- プルリクエストは小さく保つ

## ライセンス
MIT License
