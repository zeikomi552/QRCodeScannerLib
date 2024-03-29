﻿using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QRCodeScannerLib.ScannerControlParameters;

namespace QRCodeScannerLib
{
	public class ScannerDataRecieveEventArgs : EventArgs
	{
		public SerialDataReceivedEventArgs OrignalArgs { get; set; }
		public string Message { get; set; }

	}
	public class ScannerManager
	{

		#region Connect状態かどうかを表すフラグ(true:接続中 false:未接続)[IsConnect]プロパティ
		/// <summary>
		/// Connect状態かどうかを表すフラグ(true:接続中 false:未接続)[IsConnect]プロパティ用変数
		/// </summary>
		bool _IsConnect = false;
		/// <summary>
		/// Connect状態かどうかを表すフラグ(true:接続中 false:未接続)[IsConnect]プロパティ
		/// </summary>
		public bool IsConnect
		{
			get
			{
				return _IsConnect;
			}
			set
			{
				if (!_IsConnect.Equals(value))
				{
					_IsConnect = value;
				}
			}
		}
		#endregion

		#region COMポート[ComPort]プロパティ
		/// <summary>
		/// COMポート[ComPort]プロパティ用変数
		/// </summary>
		int _ComPort = 3;
		/// <summary>
		/// COMポート[ComPort]プロパティ
		/// </summary>
		public int ComPort
		{
			get
			{
				return _ComPort;
			}
			set
			{
				if (!_ComPort.Equals(value))
				{
					_ComPort = value;
				}
			}
		}
		#endregion

		#region ポート名
		/// <summary>
		/// ポート名
		/// </summary>
		public string PortName
		{
			get
			{
				return "COM" + this._ComPort.ToString();
			}
		}
		#endregion

		#region シリアル通信用オブジェクト[SerialPort]プロパティ
		/// <summary>
		/// シリアル通信用オブジェクト[SerialPort]プロパティ用変数
		/// </summary>
		SerialPort _SerialPort = null;
		/// <summary>
		/// シリアル通信用オブジェクト[SerialPort]プロパティ
		/// </summary>
		public SerialPort SerialPort
		{
			get
			{
				return _SerialPort;
			}
			set
			{
				if (_SerialPort == null || !_SerialPort.Equals(value))
				{
					_SerialPort = value;
				}
			}
		}
		#endregion

		#region データの受信処理
		/// <summary>
		/// データの受信処理
		/// </summary>
		public EventHandler DataReceived;
		#endregion

		#region コンストラクタ
		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="com_port">COMポート</param>
		/// <param name="enc">エンコード</param>
		public ScannerManager(int com_port)
		{
			this.ComPort = com_port;    // COMポートの設定
		}
		#endregion

		#region AT(ハンディスキャナ)接続処理
		///// <summary>
		///// AT(ハンディスキャナ)接続処理
		///// </summary>
		/// <summary>
		/// AT(ハンディスキャナ)接続処理
		/// </summary>
		/// <param name="mode">モード指定(デフォルト：オートオフモード)</param>
		/// <param name="wait">設定毎の安定までの待ち時間：100ミリ秒×3回</param>
		public void Connect(string mode = "U1", int wait = 100)
		{
			try
			{
				this._SerialPort = new SerialPort(this.PortName);

				this._SerialPort.DataReceived -= _SerialPort_DataReceived;
				this._SerialPort.DataReceived += _SerialPort_DataReceived;
				this._SerialPort.Open();

				this._SerialPort.DtrEnable = true;  // DTRの有効化
				this._SerialPort.RtsEnable = true;  // RTSの有効化

				this.IsConnect = true;  // 接続中に変更
				System.Threading.Thread.Sleep(wait); // 通信安定まで一瞬待つ

				// モードのセット
				ModeSet(mode, wait);
			}
			catch
			{
				this.SerialPort.Close();
				this.IsConnect = false;  // 未接続に変更
				throw;
			}
		}
		#endregion

		#region AT(ハンディスキャナ)切断処理
		/// <summary>
		/// AT(ハンディスキャナ)切断処理
		/// </summary>
		public void Disconnect()
		{
			try
			{
				if (this.SerialPort != null)
				{
					this.SerialPort.Close();
					this.IsConnect = false;  // 未接続に変更
				}
			}
			catch
			{
				throw;
			}
		}
		#endregion

		#region オブジェクトの破棄
		/// <summary>
		/// オブジェクトの破棄
		/// </summary>
		public void Dispose()
        {
			try
			{
				if (this.SerialPort != null)
				{
					this.SerialPort.Dispose();
					this.SerialPort = null;
					this.IsConnect = false;  // 未接続に変更
				}
			}
			catch
			{
				throw;
			}
		}
		#endregion

		#region U1～U8のモードを指定します
		/// <summary>
		/// U1～U8のモードを指定します
		/// </summary>
		/// <param name="mode">U1～U8のScannerControlParameters.TriggerSwitchControl以下のモードを指定します</param>
		/// <param name="wait">設定後の安定までの待ち時間(ミリ秒)</param>
		public void ModeSet(string mode, int wait = 100)
		{
			try
			{
				this._SerialPort.WriteLine(mode.ToString() + ScannerControlParameters.CR);
				System.Threading.Thread.Sleep(wait);

				this._SerialPort.WriteLine(WaitForRead.Z + ScannerControlParameters.CR);
				System.Threading.Thread.Sleep(wait);

				this._SerialPort.WriteLine(ReadEnable.R + ScannerControlParameters.CR);

				// シリアルポート上に残っているバッファをクリア
				BufferClear();
			}
			catch
			{
				this.SerialPort.Close();
				this.SerialPort.Dispose();
				this.SerialPort = null;
				this.IsConnect = false;  // 未接続に変更
				throw;
			}
		}
		#endregion

		#region バッファのクリア処理
		/// <summary>
		/// バッファのクリア処理
		/// </summary>
		public void BufferClear()
		{
			this.SerialPort.DiscardInBuffer();
		}
		#endregion

		#region 末尾のキャリッジリターンを削除
		/// <summary>
		/// 末尾のキャリッジリターンを削除
		/// </summary>
		/// <param name="msg">受信データ</param>
		/// <returns>キャリッジリターンの削除</returns>
		private static string TrimCarriageReturn(string msg)
		{

			if (msg.Length > 0 && msg.Substring(msg.Length - 1).Equals(ScannerControlParameters.CR.ToString()))
			{
				return msg.Substring(0, msg.Length - 1);
			}
			else
			{
				return msg;
			}
		}
        #endregion

        #region イベント処理
        /// <summary>
        /// イベント処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			if (this.DataReceived != null)
			{
				int index = 0;
				bool bFind = false;
				string msg = string.Empty;

				while (true)
				{
					// シリアルポートの電圧が安定するのを少し待つ
					System.Threading.Thread.Sleep(50);

					// シリアルポートからのデータ吸出し
					string tmp_msg = this.SerialPort.ReadExisting();
					msg = msg + tmp_msg;    // 文字列の結合

					if (tmp_msg.Length > 0 && tmp_msg.Substring(tmp_msg.Length - 1).Equals(ScannerControlParameters.CR))
					{
						bFind = true;
					}
					else
					{
						// キャリッジリターンを見つけた && 取得した文字列が空
						if (bFind && string.IsNullOrEmpty(tmp_msg))
						{
							msg = GetRecieveData(msg);

							// イベントの打ち上げ(TODO)
							ScannerDataRecieveEventArgs args = new ScannerDataRecieveEventArgs();
							args.OrignalArgs = e;

							// キャリッジリターンの削除
							args.Message = TrimCarriageReturn(msg);

							if (this.DataReceived != null)
							{
								this.DataReceived(sender, args);
							}

							break;
						}
						else
						{
							bFind = false;
						}
					}

					// リトライ回数が規定を超えた場合強制的に抜ける
					if (index >= 10)
					{
						ScannerDataRecieveEventArgs args = new ScannerDataRecieveEventArgs();
						args.OrignalArgs = e;

						// キャリッジリターンの削除
						args.Message = TrimCarriageReturn(msg);

						if (this.DataReceived != null)
						{
							this.DataReceived(sender, args);
						}

						break;
					}
					index++;
				}
			}
		}
		#endregion

		#region データの受信処理
		/// <summary>
		/// データの受信処理
		/// </summary>
		/// <param name="msg">受信データ</param>
		/// <returns>連結したデータ</returns>
		private string GetRecieveData(string msg)
		{
			int index = msg.IndexOf(ScannerControlParameters.CR);

			if (index >= 0 && index + 1 < msg.Length)
			{
				string tmp_msg1 = msg.Substring(0, index + 1);
				string tmp_msg2 = msg.Substring(index + 1);

				if (tmp_msg2.Substring(0, 1).Equals("\n"))
				{
					return tmp_msg1 + GetRecieveData(tmp_msg2);
				}
				else
				{
					return tmp_msg1;
				}
			}
			else
			{
				return msg;
			}
		}
		#endregion
	}
}
