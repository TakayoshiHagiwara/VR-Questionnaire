# VR-Questionnaire

<img src="https://img.shields.io/badge/Unity-2021 or Later-blue?&logo=Unity"> <img src="https://img.shields.io/badge/License-MIT-green">

心理実験で使用するアンケートをUnity上で行うためのツールです。
「VR」とついていますが、VR空間に限らず、ディスプレイ上でも動作します。
著者が所属していたラボでの使用を想定しているため、用途は限られます。
また、興味本位で作成したツールなので、低クオリティで機能も少ないです。


# Environment
- Unity 2021 or Later

# Installation
- 一部のサンプルにOVRInputを使用しているため、事前にMeta XR Utilitiesを導入しておいてください
- Unity Package Managerを利用した導入方法
    1. Window -> Package Managerを開きます
    1. 左上のプラスアイコンをクリックし、「Add package from git URL...」をクリックします
    1. このリポジトリURLを入力し、addをクリックします

# Usage
1. QuestionnaireSampleというprefabを配置します
1. Managers -> ControllerInputManagerのInput modeを任意の入力に設定します
    - デフォルトではキーボード入力を受け付けます
1. 実行するとツールが動くと思います

# Description
Canvas内にある「7-PointLikertScale」および「VisualAnalogScale」という名前のオブジェクトが個別のアンケートになります。
アンケートを追加または削除する場合は、このオブジェクトをそのままコピーまたは削除します。
オブジェクト内のアンケート文章は適宜変更してください。
デフォルトでは7段階リッカート尺度用と、Visual analog scale用を用意しています。
追加または削除した場合は、後述のQuestionnaireManagerのQuestionnaireObjListを編集します。

## QuestionnaireManager
- アンケート全体を制御するクラスです
- InspectorのQuestionnaireObjListに該当するアンケートのGameObjectをアタッチします。
- Listのサイズを変更することでアンケートの個数を変えます
- デフォルトではListにアタッチされたアンケートをランダムな順番で提示します
- Listの上から順番に提示する場合は、isRandomizeOrderをオフにします

## ControllerInputManager
- ユーザの入力を制御するクラスです
- InspectorのInput modeから任意の入力装置を指定します
- デフォルトでは、何も制御を行わない`DEBUG`、キーボードからの入力を行う`KEYBOARD`、Meta Questコントローラなどからの入力を受け取る`OVR_CONTROLLER`を用意しています
### 入力装置を追加する場合
- スクリプトを編集、追加します
- 任意のProvider (入力された値、判定を提供するもの) を追加します
    - 適当な名前で新しくC#スクリプトを作成し、IControllerInputを継承してください (MonoBehaviourは削除してください)
    - 既存のProvider (OVRControllerInputProviderなど) を参考にして、メソッドを定義してください
- ControllerInputManagerのInput modeに任意の名前を追加してください
- ControllerInputManagerのChangeProviderに追加したInput modeに対応する分岐と、追加したProviderのインスタンス生成を行ってください
- InspectorのInput modeから任意の入力装置を指定します

## AnimationManager
- アンケートのアニメーションを制御するクラスです
- 次のアンケートに進む際に、なめらかに遷移するような見た目のアニメーションを作成しています


# Note
## アニメーション遷移の不具合
- アニメーション遷移中に次のアニメーションを実行しようとすると、うまく動作しない場合があります
- そのため、「次へ進む / 前へ戻る」ボタンを押す際は、短時間に連続で押さないようにしてください

## 複数種類のアンケートについて
- 一応リッカート尺度とアナログスケールを混ぜても動作します
- （おそらくそのような状況はあまりないと思います）

## 実行時のパフォーマンスについて
- 初回の読み込みに数msかかります
- アンケートを行う際に、正確な時間計測が重要になる、という場面は、通常の実験ではあまり起きないと思いますが、念のため記載しておきます

# References
- アニメーション遷移時に、完了まで待機するスクリプトは以下のサイトを参考にさせていただきました
- https://tsubakit1.hateblo.jp/entry/2016/02/11/021743

# Author
- Takayoshi Hagiwara
    - Toyohashi University of Technology


# License
- MIT License