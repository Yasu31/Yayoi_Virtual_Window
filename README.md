![Virtual Window](https://github.com/Yasu31/Yayoi_Virtual_Window/blob/master/images/3.png)
# Introduction/紹介
created using [Virtual Window for Unity](https://github.com/Yasu31/Virtual-Window-for-Unity)(to be accurate, VW for Unity was created out of necessity while I was making Yayoi VW)- a Unity program, easily integrable to projects, that lets the computer screen act as a "virtual window"

[Virtual Window for Unity](https://github.com/Yasu31/Virtual-Window-for-Unity)という、私が作成したプログラムを用いて作りました。（正確には、この弥生VWを作るために、Virtual Window for Unityを作った）[大阪府立弥生文化博物館](http://www.kanku-city.or.jp/yayoi/)で2017年11月に行われたイベントで初披露しました。

そのため、こちらの博物館で所蔵されている、弥生式土器や銅鏡を見ることができます。
# User Manual/説明書
## Requirements/必要なもの
### webcam & [FaceOSC](https://github.com/kylemcdonald/ofxFaceTracker/releases) software/ウェブカメラと[FaceOSC](https://github.com/kylemcdonald/ofxFaceTracker/releases)ソフトウェア
to track user's face. This software is required to be running while Virtual Window is running. Sends face position & gesture data over [OSC](http://opensoundcontrol.org/introduction-osc), a network protocol for transferring data between apps and devices.

使用者の顔の位置を取得するために必要です。Yayoi Virtual Windowが動いている間、FaceOSCが背後で動いている必要があります。
## Control/操作

### switch between scenes/シーン切り替え
Enter(return) key

改行キー

Open mouth (like yawning) for 2 seconds (this may not work at times)

あくびをするように、口を2秒ほど開く（うまくいかない時があります）
### to look around scene/シーン内を見回す
move your head around to look around. The display changes accordingly.

頭を動かしてください。それに応じて画面が変わります。

### to move around scene/シーン内を動き回る
arrow keys

矢印キー

mouse (left&right button to go left&right, scroll wheel to go forward & back)

マウス（左右のボタンで左右に動き、スクロールすると前後に動く）

move your head around **more**.(the software sets up a virtual "fence" about 100cm sideways and 60cm front-back in front of the screen, of which if you pass through, the box which the camera is in moves in that direction too)(this feature may feel more of a nuisance, can toggle with "m" key)

頭をもっと動かす（スクリーンの前に、左右１メートル、前後６０センチほどの「囲い」を作っています。この「囲い」から頭が出ると、その方向に動き出すようになっています）（不便と感じたら、mを押すとこの機能をオンオフ切り替えられます）

### reset camera position/カメラの位置をリセット
spacebar

スペースキー
## Scenes/シーン
![Scenes](https://github.com/Yasu31/Yayoi_Virtual_Window/blob/master/images/2.png)

## How it works/しくみ
![How it works](https://github.com/Yasu31/Yayoi_Virtual_Window/blob/master/images/1.png)
