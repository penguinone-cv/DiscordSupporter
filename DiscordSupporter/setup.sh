#!/bin/bash
if [ "$#" -ne 2 ]; then
	echo "引数の数が一致しません。"
	exit 1
fi

echo '{
  "Token":"'$1'",
  "GuildId":"'$2'"
}' > config.json

echo "config.json の設定が完了しました。"

