# プロジェクト基盤仕様書 (Infrastructure & Tech Stack)

開発にはASP.NET Core Blazor WebAssembly (PWA) 技術スタックを使用する。

## 1. 開発環境 (Development Environment)
* **フレームワーク**: ASP.NET Core Blazor WebAssembly, .NET 10.0
* **実行環境**: WebAssembly (Wasm) 上で動作する .NET ランタイム
* **主要言語**: C#, HTML, CSS (Razor 構文)

#### 2. PWA機能統合
* **Service Worker**: `service-worker.js` / `service-worker.published.js` 
  (.NETビルドプロセスと統合され、オフラインキャッシュを管理)
* **Manifest**: `manifest.json` (インストール要件、アイコン、表示モードの定義)
* **オフラインサポート**: Blazor PWA テンプレートによる標準キャッシュ戦略 (Cache-first アプローチ等)

#### 3. 開発・ビルドツール
* **SDK**: .NET SDK (v10.0 以降を推奨)
* **IDE / エディタ**: Visual Studio 2022, JetBrains Rider, または Visual Studio Code (C# Dev Kit 拡張機能を利用)
* **パッケージ管理**: NuGet (フロントエンド用コンポーネントからバックエンドの計算ライブラリまで統一管理)

#### 4. 拡張機能・相互運用性
* **JS相互運用 (JS Interop)**: C# から JavaScript API (ブラウザ固有機能など) へのアクセス、およびその逆を行うブリッジ機構
* **CSS分離 (CSS Isolation)**: コンポーネント単位でのスコープ付きCSS管理 (`[コンポーネント名].razor.css`)

## 5. バージョン管理とデプロイ (CI/CD)
* **バージョン管理:** Git / GitHub (`main` ブランチを本番環境とする)
* **ホスティング:** GitHub Pages (静的ファイルホスティング)
* **自動化パイプライン:** GitHub Actions
  * `main` へのプッシュをトリガーに自動配信する。

## 4. 実行環境とハードウェア要件 (Execution Platform)
* **アプリケーション形態:** PWA (Progressive Web App)
  * Webブラウザ（Chrome等）から端末の「ホーム画面に追加」することで、フルスクリーンかつオフライン感覚でネイティブアプリとして起動させる。
* **対象端末:** Android 8〜11インチタブレット
* **必須インターフェース:** BluetoothまたはUSB接続の物理キーボード
  * *※制約事項: ソフトウェアキーボードの使用は、画面レイアウトの崩壊およびF・Jキーのアンカー（触覚的絶対座標）の喪失を招くため、本アプリの学習効果を得られない環境と定義し、使用を不可とする。*

## 5. データ保存と永続化 (Data Storage)
* **アーキテクチャ:** サーバーサイド（バックエンド）を持たない静的ホスティング環境を前提とする。
* **保存方式:** 児童のプレイ履歴（コンボ数、クリア状況）や指導者の分析ログ（エラーマトリクス、待機時間）は、実行しているタブレット端末内部の `IndexedDB` または `localStorage` に保存される。
* **制約事項:** 1台の端末を複数児童で共有することは想定しない（シングルユーザー前提）。完全なローカル保存であるため、端末の変更によるデータの引き継ぎ、および複数端末間（例えば児童のタブレットと指導者のPC間）でのログのリモート同期は行えない仕様とする。

