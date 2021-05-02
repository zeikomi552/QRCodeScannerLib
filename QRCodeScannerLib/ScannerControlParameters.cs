using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeScannerLib
{
    public class ScannerControlParameters
    {
        /// <summary>
        /// 読み取り待機
        /// トリガスイッチが連続読み取りモード 1 または 2 に設定されている時に限り、「Z」、「READOFF」、「LOFF」コマンドが受信されるとスキャナは読み取り待機状態に入ります。
        /// </summary>
        public class WaitForRead
        {
            /// <summary>
            /// 読み取り待機
            /// </summary>
            public const string Z = "Z";

            /// <summary>
            /// 読み取り待機
            /// </summary>
            public const string READOFF = "READOFF";

            /// <summary>
            /// 読み取り待機
            /// </summary>
            public const string LOFF = "LOFF";
        }
        /// <summary>
        /// 読み取り可能トリガスイッチが連続読み取りモード 1 または 2 に設定されている時に限り、「R」、「READON」、「LON」コマンドが受信されると、スキャナは読み取り可能状態に入ります。
        /// </summary>
        public class ReadEnable
        {
            public const string R = "R";
            public const string READON = "READON";
            public const string LON = "LON";
        }

        /// <summary>
        /// ブザー鳴動
        /// コマンド受信後 100ms 以内に、決められた時間だけブザーを鳴動させます。
        /// 「B1」: 約 60ms，約 80ms，約 120ms または約 140ms 間鳴動
        /// 「B2」: 約 120ms 間鳴動
        /// 「B3」: 約 240ms 間鳴動
        /// ブザーの鳴動が禁止されている場合や、読み取り待機状態でも鳴動可能です。
        /// </summary>
        public class Buzzer
        {
            public const string B1 = "B1";
            public const string B2 = "B2";
            public const string B3 = "B3";
        }

        /// <summary>
        /// ブザー鳴動(音色指定)
        /// 上記 B1～B3 コマンドに、この音色オプションを追加すること
        /// で、音色別にブザーを鳴動させます。
        /// 「H」: 高音(4.0 kHz)で鳴動
        /// 「M」: 中音(3.0 kHz で鳴動
        /// 「L」: 低音(2.8 kHz)で鳴動
        /// </summary>
        public class BuzzerSound
        {
            public const string BH1 = "BH1"; public const string BM1 = "BM1"; public const string BL1 = "BL1";
            public const string BH2 = "BH2"; public const string BM2 = "BM2"; public const string BL2 = "BL2";
            public const string BH3 = "BH3"; public const string BM3 = "BM3"; public const string BL3 = "BL3";
        }

        /// <summary>
        /// 青色表示 LED 点灯
        /// コマンド受信後 100ms 以内に、約 500ms 間点灯します。
        /// </summary>
        public const string LB = "LB";

        /// <summary>
        /// 緑色表示 LED 点灯
        /// コマンド受信後 100ms 以内に、約 500ms 間点灯します。
        /// </summary>
        public const string LG = "LG";
        /// <summary>
        /// 赤色表示 LED 点灯
        /// コマンド受信後 100ms 以内に、約 500ms 間点灯します。
        /// </summary>
        public const string LR = "LR";

        /// <summary>
        /// トリガスイッチコントロール
        /// </summary>
        public class TriggerSwitchControl
        {
            /// <summary>
            /// オートオフモード
            /// </summary>
            public const string U1 = "U1";
            /// <summary>
            /// モメンタリスイッチモード
            /// </summary>
            public const string U2 = "U2";
            /// <summary>
            /// オルタネートスイッチモード
            /// </summary>
            public const string U3 = "U3";
            /// <summary>
            /// 連続読み取りモード1
            /// </summary>
            public const string U4 = "U4";
            /// <summary>
            /// 連続読み取りモード2
            /// </summary>
            public const string U5 = "U5";
            /// <summary>
            /// オートセンスモード
            /// </summary>
            public const string U6 = "U6";
            /// <summary>
            /// オートスタンドモード
            /// </summary>
            public const string U7 = "U7";
            /// <summary>
            /// モメンタリスイッチモード（反転タイプ）
            /// </summary>
            public const string U8 = "U8";
        }

        /// <summary>
        /// パラメータ記憶
        /// U1～U8 コマンドで設定した値をフラッシュ ROM へ記憶します。
        /// このコマンドを与えないと、U1～U8 コマンドで設定した値は電
        /// 源を切断すると元に戻ります。
        /// </summary>
        public const string PW = "PW";

        /// <summary>
        /// ソフトウエアバージョンの要求
        /// ＜スキャナ応答＞「Ver.n.nn」
        /// n.nn：バージョン No. （例：Ver.1.00）
        /// </summary>
        public const string VER = "VER";

        /// <summary>
        /// 設定パラメータバージョンの要求
        /// 設定ソフトとの接続時にスキャナの設定パラメータのバージョン No.を確認できます。
        /// ＜スキャナ応答＞「Ver.n.nn.mm」
        /// n.nn.mm：バージョン No. （例：Ver.1.00.00）
        /// mm：設定パラメータバージョン No.
        /// </summary>
        public const string VERF = "VERF";
        /// <summary>
        /// スキャンエントリーモードの要求
        /// n 点照合読み取り時、「マスター読み取り登録」状態に移行し、
        /// マスターコードの読み取りを行うことにより登録をすること
        /// ができます。登録したマスターコードデータはフラッシュ ROMへ記憶します。
        /// </summary>
        public const string E = "E";
        /// <summary>
        /// スキャナ ID (シリアルナンバー)の要求
        /// ＜スキャナ応答＞「ID.XXXXXXXXXXnnnnnn」
        /// XXXXXXXXXX：固有ナンバー nnnnnn：シリアルナンバー
        /// </summary>
        public const string ID = "ID";
        /// <summary>
        /// トリガスイッチ機能の許可
        /// トリガスイッチコントロールを有効状態にします。
        /// </summary>
        public const string TMON = "TMON";
        /// <summary>
        /// トリガスイッチ機能の無効
        /// トリガスイッチコントロールを無効状態にし、読み取り待機状
        /// 態にします。
        /// </summary>
        public const string TMOFF = "TMOFF";
        /// <summary>
        /// 読み取り失敗
        /// 連続読み取りモード 1 または 2 に設定されている場合、読み取
        /// り可能状態で読み取りを完了せずに読み取り待機状態となっ
        /// たとき送信します。転送の要否を選択できます。
        /// </summary>
        public const string ERROR = "ERROR";
        /// <summary>
        /// 照合一致
        /// 照合一致時のデータ出力を「OK」にした場合に限り、照合モー
        /// ドで読み取りし、マスターデータとデータ照合が一致したとき送信します。
        /// </summary>
        public const string OK = "OK";
        /// <summary>
        /// 照合不一致
        /// 照合不一致時のデータ出力を「NG」にした場合に限り、照合モ
        /// ードで読み取りし、マスターデータとデータ照合が一致しなか
        /// ったとき送信します。
        /// </summary>
        public const string NG = "NG";


    }
}
