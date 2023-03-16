# ARでやる音楽ゲーム
## タイトル
- Building Block Beats
## ゲーム概要
- タイトル画面が表示されスタートボタンがクリックされると,位置情報の取得が開始される.位置情報から建物のオブジェクトを入手したら,建物が画面上に設置(オクルージョン)され,建物に沿ったレーンが作成され,ノーツが落ちてくる.ノーツをたたくとコンボカウントが増えていき,音楽ゲーム終了時にスコアが表示される.
## タイトル画面
* <img width="883" alt="title" src="https://user-images.githubusercontent.com/115143247/222949807-0e02121c-d0bd-4d47-8b9d-29840b14f959.png">
## ゲーム画面
* <img width="883" alt="title" src="https://user-images.githubusercontent.com/115143247/222949807-0e02121c-d0bd-4d47-8b9d-29840b14f959.png">
### 背景
- 身の回りの課題解決ではなく,なにかこう言ったことができたら面白いだろうなという案からゲームを作成した.様々なゲームの案から音楽ゲームを採用したが,世間一般にリリースされている音楽ゲームとは違いを出すためにAR上でやる音楽ゲームを作成することを考えた.
### プロダクトの説明
- タイトル画面が表示されると音楽が流れる.「ゲームを始める」を押すと,ゲーム画面に遷移する.ゲーム画面では,位置情報の位置情報取得まで待機する.位置情報の取得が完了すると,スマートフォンのカメラ起動し,ユーザのスマートフォンから写る景色とレーンとノーツが表示される.そしてゲームが開始する.音楽が流れ始め,ノーツが落ちてくるようになる.落ちていくノーツをタイミングよく叩くことでコンボ数が増えていく.コンボ数や叩いたノーツの数によってゲームのスコアが上昇していく.最終的なゲームのスコアによって,ゲーム上に成績が表示される.ゲームが終了するとタイトル画面に遷移しなおす.音楽は,友人が作成したものであり著作物ではない.
### 特徴
#### 1. 3Dでのゲームと比べ,迫力がある.
#### 2. AR上で,ゲームがプレイできるので単一のゲーム画面にはならず,景色によって変化がある.
### 今後の展望
- 音楽ゲーム1つではなく,
* 複数の機能を追加していきたい.例えば,音楽の欠けた部分を自分が演奏することで,1つの曲として完成させるゲーム.
* 単純なピアノの鍵盤を用意し,叩いて音楽を鳴らす子供向けゲーム.
### 構成
#### CityFBXフォルダ
- fbx形式の都市モデル
#### FromBlenderフォルダ
- fbx形式のBlnderによって作成されたモデル
#### Materialフォルダ
- fbx形式のノーツなどのオブジェクトにアタッチするモデル
#### Picturesフォルダ
- png形式のタイトル画面に使われるファイル
#### Prefabフォルダ
- prehab形式のものが集まったファイル
#### Scriptsフォルダ
- ノーツ,タイトル画面,ゲーム画面,画面遷移などに使われるスクリプト
#### Shaderフォルダ
- shaderファイル
#### Soundsフォルダ
- タイトル画面やゲーム画面で流れる音楽

## 開発技術
### 活用した技術
#### API・データ
* Geospatial API

#### フレームワーク・ライブラリ・モジュール
* Unity
* Blender
* Cubase
* ARCore Extensions
* AR foundation
* ARCore XR Plugin
* ARKit XR Plugin
* Universal RP
* DOTween
