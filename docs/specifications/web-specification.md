# PC Inventory Management Web 仕様書

## 概要
PCの在庫管理を行うためのWebアプリケーションです。PC、ユーザー、OS種別、拠点の管理機能を提供します。

## 技術スタック
- フレームワーク: Vue.js 3
- UIライブラリ: Vuetify 3
- 状態管理: Vue Composition API
- HTTP通信: Axios
- ビルドツール: Vite

## 画面構成

### 共通レイアウト
- ヘッダー
  - アプリケーションタイトル
  - ナビゲーションメニュー
- メインコンテンツ
- フッター

### PC管理画面
- PC一覧表示
  - 管理番号
  - モデル名
  - OS種別
  - 利用者
  - 作成日時
  - 更新日時
  - 操作ボタン（編集、削除）
- PC新規作成/編集ダイアログ
  - 管理番号入力
  - モデル名入力
  - OS種別選択
  - 利用者選択
  - 保存/キャンセルボタン

### ユーザー管理画面
- ユーザー一覧表示
  - 氏名
  - ADアカウント
  - 表示名
  - 所属拠点
  - アクティブ状態
  - 作成日時
  - 更新日時
  - 操作ボタン（編集、削除）
- ユーザー新規作成/編集ダイアログ
  - 氏名入力
  - ADアカウント入力
  - 表示名入力
  - 所属拠点選択
  - アクティブ状態設定
  - 保存/キャンセルボタン

### OS種別管理画面
- OS種別一覧表示
  - OS名
  - 操作ボタン（編集、削除）
- OS種別新規作成/編集ダイアログ
  - OS名入力
  - 保存/キャンセルボタン

### 拠点管理画面
- 拠点一覧表示
  - 拠点コード
  - 拠点名
  - 操作ボタン（編集、削除）
- 拠点新規作成/編集ダイアログ
  - 拠点コード入力
  - 拠点名入力
  - 保存/キャンセルボタン

## データ型定義

### PC
```typescript
interface PC {
  id: number;
  managementNumber: string;
  modelName: string;
  osTypeId: number;
  userId?: number;
  isDeleted: boolean;
  createdAt: string;
  updatedAt?: string;
  osType?: OSType;
  user?: User;
}
```

### ユーザー
```typescript
interface User {
  id: number;
  name: string;
  adAccount: string;
  displayName: string;
  isActive: boolean;
  isDeleted: boolean;
  createdAt: string;
  updatedAt?: string;
  locationId: number;
  location?: Location;
  pcs?: PC[];
}
```

### OS種別
```typescript
interface OSType {
  id: number;
  name: string;
  isDeleted: boolean;
}
```

### 拠点
```typescript
interface Location {
  id: number;
  code: string;
  name: string;
  isDeleted: boolean;
  users?: User[];
}
```

## API通信

### PC管理
```typescript
const pcApi = {
  getAll: () => api.get<PC[]>('/api/PCs'),
  getById: (id: number) => api.get<PC>(`/api/PCs/${id}`),
  create: (pc: Omit<PC, 'id'>) => api.post<PC>('/api/PCs', pc),
  update: (id: number, pc: Partial<PC>) => api.put<void>(`/api/PCs/${id}`, pc),
  delete: (id: number) => api.delete<void>(`/api/PCs/${id}`)
};
```

### ユーザー管理
```typescript
const userApi = {
  getAll: () => api.get<User[]>('/api/Users'),
  getById: (id: number) => api.get<User>(`/api/Users/${id}`),
  create: (user: Omit<User, 'id'>) => api.post<User>('/api/Users', user),
  update: (id: number, user: Partial<User>) => api.put<void>(`/api/Users/${id}`, user),
  delete: (id: number) => api.delete<void>(`/api/Users/${id}`)
};
```

### OS種別管理
```typescript
const osTypeApi = {
  getAll: () => api.get<OSType[]>('/api/OSTypes'),
  getById: (id: number) => api.get<OSType>(`/api/OSTypes/${id}`),
  create: (osType: Omit<OSType, 'id'>) => api.post<OSType>('/api/OSTypes', osType),
  update: (id: number, osType: Partial<OSType>) => api.put<void>(`/api/OSTypes/${id}`, osType),
  delete: (id: number) => api.delete<void>(`/api/OSTypes/${id}`)
};
```

### 拠点管理
```typescript
const locationApi = {
  getAll: () => api.get<Location[]>('/api/Locations'),
  getById: (id: number) => api.get<Location>(`/api/Locations/${id}`),
  create: (location: Omit<Location, 'id'>) => api.post<Location>('/api/Locations', location),
  update: (id: number, location: Partial<Location>) => api.put<void>(`/api/Locations/${id}`, location),
  delete: (id: number) => api.delete<void>(`/api/Locations/${id}`)
};
```

## エラーハンドリング
- APIエラーの表示
  - エラーメッセージをSnackbarで表示
  - ネットワークエラー時の再試行機能
- バリデーションエラーの表示
  - フォーム入力項目下部にエラーメッセージを表示
  - 必須項目のチェック
  - 文字数制限のチェック

## レスポンシブデザイン
- デスクトップ（1024px以上）
  - フル機能表示
  - データテーブルの全カラム表示
- タブレット（768px～1023px）
  - 一部カラムの省略表示
  - サイドメニューの折りたたみ
- モバイル（767px以下）
  - カード形式でのデータ表示
  - ハンバーガーメニュー
  - モーダル表示の最適化 