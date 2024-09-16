# DiscordSupporter
自分用の汎用DiscortBot
## セットアップ手順
### Linux(Ubuntu 22.04で確認)
#### 実行環境の整備
gitは入っていてリポジトリのクローンは終わっているものとします。  
  
本アプリケーションの実行にはDotNet8.0 SDKが必要です。以下のURLを参考にインストールしてください。  
https://learn.microsoft.com/ja-jp/dotnet/core/install/linux-ubuntu-install?tabs=dotnet8&pivots=os-linux-ubuntu-2204#install-the-sdk  

次に、Discordのトークンと実行したいサーバーのIDを取得します。  
* **トークン**  
以下のURL辺りを参考にBotを作成し、トークンを取得してください。  
取得したトークンはどっかにメモっといてください。  
https://discord.com/developers/docs/intro  
* **サーバーID**  
Discordのサーバーアイコン右クリック -> サーバーIDをコピー から取得できます。

次に、ボットを起動したい環境で`bash setup.sh $TOKEN $ID`を実行します。  
`$TOKEN`、`$ID`はそれぞれ取得したトークンとサーバーIDに置き換えてください。
