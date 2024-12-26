﻿using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO.Ports;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Xml.Linq;
using log4net;
using MissionController.Core;
using MissionController.Core.Logging;
using MissionController.Views;

namespace MissionController
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //ロガー
        private static readonly ILog log = LogManager.GetLogger(typeof(App));
        internal FlowDocumentManager SystemLogDocument { get; private set; } = new FlowDocumentManager();
        //UI関連
        internal MainWindow? mainWindow;
        //無線接続関連
        internal WirelessModule wirelessModule { get; private set; }

        //環境変数・プロファイルのクラス
        public ApplicationProfile Profile { get; private set; }
        public EnvironmentConfiguration Config { get; private set; }

        public App()
        {
            //log4netの初期化
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo("log4net.config"));
            log.Info("Initialized log4net");

            //環境設定ファイル読み込み
            Config = EnvironmentConfiguration.ReadConfiguration();
            log.Info("Read configuration file");

            //アプリのインスタンス情報を新規作成
            Profile = new ApplicationProfile();
            log.Debug("Created application profile");

            //ワイヤレスモジュールの制御クラスをインスタンス化し、イベントを設定
            wirelessModule = new WirelessModule()
            {
                DataReceivedEvent = WirelessModuleDataReceived,
                CommandResponceEvent = CommandResponceEventHandler
            };
            log.Debug("WirelessModuleクラスのインスタンスwirelessModuleを初期化");

            //コンストラクタの最後でMainWindowを初期化する。表示はしない。
            mainWindow = new MainWindow();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            mainWindow?.Show();
        }

        //イベントハンドラ
        /// <summary>
        /// シリアルポート受信時のイベント
        /// </summary>
        private void WirelessModuleDataReceived(Packet32 packet)
        {
            Debug.WriteLine($"{packet.Timestamp}\nノード番号は{packet.NodeNumber}\n信号強度は{packet.RSSI}\nカテゴリは{packet.Type}\n内容は{packet.Data}");
        }
        private void CommandResponceEventHandler(string responce)
        {
            Debug.WriteLine($"Received:at Command");
        }

        //メソッドとか
        public void Send(Packet32 packet)
        {
            Packet32Serializer.Serialize(packet);
        }





        
        /// <summary>
        /// コマンドレスポンスのイベントハンドラ
        /// </summary>
        /// <param name="responce"></param>
        
        
        //internal void LogAddEvent(LogData data)
        //{
        //    //ログ追加時の処理
        //    Paragraph log = new Paragraph();//ログの段落
        //    log.FontFamily = new FontFamily("Consolas");

        //    Run logAttribute = new Run(" " + data.Type.ToString() + " ");
        //    Run time = new Run(data.Time.ToString(" HH:mm:ss:fff "));//時間
        //    switch (data.Type)
        //    {
        //        case LogType.LOG:
        //            logAttribute.Background = Brushes.White;
        //            break;
        //        case LogType.DEBUG:
        //            logAttribute.Background = Brushes.Green;
        //            break;
        //    }
        //    log.Inlines.Add(logAttribute);
        //    log.Inlines.Add(time);
        //    log.Inlines.Add(data.Content);
        //    mainWindow.TextBox_DebugLog.Document.Blocks.Add(log);
        //    mainWindow.TextBox_DebugLog.ScrollToEnd();
        //}
    }

}
