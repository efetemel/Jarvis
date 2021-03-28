using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jarvis
{
    public partial class frm_Jarvis : Form
    {
        public frm_Jarvis()
        {
            InitializeComponent();
        }
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            Sorular();
        }
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (webBrowser1.ReadyState == WebBrowserReadyState.Complete)
                {
                    textBox1.Text = "HOŞGELDİN ! BEN JARVİS, SANA YARDIM ETMEK İÇİN BURADAYIM.";
                    timer1.Enabled = false;
                    SesliOkuma();
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Muhtemelen Fatih Engelledi <3 Siz Yinede Bir Deneme Yapın", "[PC JARVİS]", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }
        private void frm_Jarvis_Load(object sender, EventArgs e)
        {
            GraphicsPath graphicpath = new GraphicsPath();
            graphicpath.StartFigure();
            graphicpath.AddArc(0, 0, 25, 25, 180, 90);
            graphicpath.AddLine(25, 0, this.Width - 25, 0);
            graphicpath.AddArc(this.Width - 25, 0, 25, 25, 270, 90);
            graphicpath.AddLine(this.Width, 25, this.Width, this.Height - 25);
            graphicpath.AddArc(this.Width - 25, this.Height - 25, 25, 25, 0, 90);
            graphicpath.AddLine(this.Width - 25, this.Height, 25, this.Height);
            graphicpath.AddArc(0, this.Height - 25, 25, 25, 90, 90);
            graphicpath.CloseFigure();
            //this.Region = new Region(graphicpath);

            listBox1.Items.AddRange(kelime_list);
            string ipAdresi = Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString();
            label1.Text = "İp Adresi : " + ipAdresi;
            tcpListener = new TcpListener(IPAddress.Any, 6000);
            listenThread = new Thread(new ThreadStart(ListenForClients));
            listenThread.Start();
            if (listBox1.Items.Contains(""))
            {
                listBox1.Items.Remove("");
            }
            this.Invoke((MethodInvoker)delegate
            {
            });
        }
        TcpListener tcpListener;
        private Thread listenThread;
        private void ListenForClients()
        {
            tcpListener.Start();
            try
            {
                while (true)
                {
                    TcpClient client = tcpListener.AcceptTcpClient();
                    Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                    clientThread.Start(client);
                    this.Invoke((MethodInvoker)delegate
                    {
                        // MessageBox.Show("Telefon Bağlandı !", "[TÜRK] JARVİS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    });
                }
            }
            catch (SocketException)
            {
            }
            finally
            {
                this.Invoke((MethodInvoker)delegate
                {
                    //txt_jarvis.Text = "Kapatıldı";
                });
            }
        }
        private void HandleClientComm(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream clientStream = tcpClient.GetStream();
            byte[] message = new byte[4096];
            int bytesRead;
            while (true)
            {
                bytesRead = 0;
                try
                {
                    bytesRead = clientStream.Read(message, 0, message.Length);
                }
                catch (Exception ex)
                {
                    //txt_jarvis.Text = "ERROR: " + ex.Message + "\r\n";
                    break;
                }
                if (bytesRead == 0)
                {
                    break;
                }
                this.Invoke((MethodInvoker)delegate
                {
                    //mesaj = encoder.GetString(message, 0, bytesRead) + "\r\n";
                    textBox2.Text = Encoding.UTF8.GetString(message, 0, bytesRead).ToLower();
                    //ilk = Encoding.UTF8.GetString(message, 0, bytesRead).ToLower();
                    //MessageBox.Show("Mesaj Alındı");
                    label2.Text = "*Bağlantı Kuruldu";
                    label2.ForeColor = Color.Lime;
                    Sorular();
                });
            }
        }
        public string mesaj;
        public void SesliOkuma()
        {
            try
            {

                webBrowser1.Document.GetElementById("ddlVoices").SetAttribute("value", "GVZ Gul 16k_HV_Premium");
                webBrowser1.Document.GetElementById("TextBox1").InnerText = textBox1.Text;
                webBrowser1.Document.GetElementById("Button1").InvokeMember("click");
                //Thread.Sleep(100);
            }
            catch (Exception)
            {
                MessageBox.Show("Sesli Konuşma sitesinde arıza var !", "[PC JARVİS]", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        Random r = new Random();
        string[] kelime_list = new string[]
        {"merhaba"
        ,"nasılsın"
        ,"ne haber"
        ,"selamünaleyküm"
        ,"google","saat","saat kaç"
        ,"dosyalar","dosyalarımı aç","dosyalarımı açar mısın"
        ,"hava nasıl","bugün hava nasıl","hava durumu"
        ,"youtube'da ara","penti aç","penti açar mısın","penti"
        ,"word'ü açar mısın","word aç","word"
        ,"excel'i açar mısın","excel aç","excel"
        ,"not defterini açar mısın","not defteri aç","not defteri"
        ,"yerel disk c","c"
        ,"nerelisin","nerde oturuyorsun","ne yapıyorsun"
        ,"şiir okur musun","bana şiir okur musun","şiir oku","şiir"
        ,"teşekkür ederim"
        ,"haber","haberlerde ne var","haberleri göster","hakkında bilgi göster"
        ,"ınstagram","facebook","kapat","bilgisayarı kapat","evet","hayır","tekrar eder misin","google kapat","google'ı kapat","chrome kapat"
        ,"ayarlar","ayarları aç","ayarları açar mısın","denetim masası","denetim masasını aç","denetim masasını açar mısın","örnek sorular","google asistan kimdir","yazı tura at"
        ,"sen kimsin","ne demek","ingilizcede nasıl söylenir"};
        bool k = false;
        private void Sorular()
        {
            mesaj = textBox2.Text;
            int saat = DateTime.Now.Hour;
            int random = r.Next(1, 3);
            if (mesaj == kelime_list[0])
            {
                random = r.Next(1, 3);
                webBrowser2.Refresh();
                if (saat >= 12 && saat <=17)
                {
                    if (random == 1)
                    {
                        textBox2.Clear();
                        string yer = webBrowser2.Document.GetElementById("wob_loc").InnerText;
                        string zaman = webBrowser2.Document.GetElementById("wob_dts").InnerText;
                        string hava_durumu = webBrowser2.Document.GetElementById("wob_dc").InnerText;
                        string derece = webBrowser2.Document.GetElementById("wob_tm").InnerText + "°C";
                        textBox1.Text = "TÜNAYDIN ! SAAT : " + DateTime.Now.ToShortTimeString() + " " + yer + " " + derece + " VE " + hava_durumu + " İYİ GÜNLER !";
                        SesliOkuma();
                    }
                    else if (random == 2)
                    {
                        textBox2.Clear();
                        textBox1.Text = "TÜNAYDIN SAAT : " + DateTime.Now.ToShortTimeString();
                        SesliOkuma();

                    }
                }
                else if(saat >=17)
                {
                    if (random == 1)
                    {
                        textBox2.Clear();
                        string yer = webBrowser2.Document.GetElementById("wob_loc").InnerText;
                        string zaman = webBrowser2.Document.GetElementById("wob_dts").InnerText;
                        string hava_durumu = webBrowser2.Document.GetElementById("wob_dc").InnerText;
                        string derece = webBrowser2.Document.GetElementById("wob_tm").InnerText + "°C";
                        textBox1.Text = "İYİ AKŞAMLAR ! SAAT : " + DateTime.Now.ToShortTimeString() + " " + yer + " " + derece + " VE " + hava_durumu + " İYİ GÜNLER !";
                        SesliOkuma();
                    }
                    else if (random == 2)
                    {
                        textBox2.Clear();
                        textBox1.Text = "İYİ AKŞAMLAR ! SAAT : " + DateTime.Now.ToShortTimeString();
                        SesliOkuma();
                    }
                }
                else
                {
                    if (random == 1)
                    {
                        textBox2.Clear();
                        string yer = webBrowser2.Document.GetElementById("wob_loc").InnerText;
                        string zaman = webBrowser2.Document.GetElementById("wob_dts").InnerText;
                        string hava_durumu = webBrowser2.Document.GetElementById("wob_dc").InnerText;
                        string derece = webBrowser2.Document.GetElementById("wob_tm").InnerText + "°C";
                        textBox1.Text = "GÜNAYDIN ! SAAT : " + DateTime.Now.ToShortTimeString() + " " + yer + " " + derece + " VE " + hava_durumu + " İYİ GÜNLER !";
                        SesliOkuma();
                    }
                    else if (random == 2)
                    {
                        textBox2.Clear();
                        textBox1.Text = "GÜNAYDIN ! SAAT : " + DateTime.Now.ToShortTimeString();
                        SesliOkuma();
                    }
                }
            }//Merhaba
            else if (mesaj == kelime_list[1])
            {
                int numu = r.Next(1,4);
                if (numu == 1)
                {
                    textBox2.Clear();
                    textBox1.Text = "ÇOK İYİ SENİN İÇİN NE YAPABİLİRİM ?";
                    SesliOkuma();
                }
                else if (numu == 2)
                {
                    textBox2.Clear();
                    textBox1.Text = "FISTIK GİBİYİM SEN NASILSIN ?";
                    SesliOkuma();
                }
                else if (numu == 3)
                {
                    textBox2.Clear();
                    textBox1.Text = "ANLATIĞIM FIKRALAR VE YAPTIĞIM KELİME ŞAKALARI DIŞINDA HER ŞEY YOLUNDA.";
                    SesliOkuma();
                }
            }//Nasılsın
            else if (mesaj == kelime_list[2])
            {
                int numu = r.Next(1,4);
                if (numu == 1)
                {
                    textBox2.Clear();
                    textBox1.Text = "İYİYİM VE HER ZAMANKİ GİBİ YARDIM ETMEYE HAZIRIM. SENİN İÇİN YAPABİLECEĞİM HERHANGİ BİR ŞEY VAR MI ?";
                    SesliOkuma();
                }
                else if (numu == 2)
                {
                    textBox2.Clear();
                    textBox1.Text = "SEN ETRAFIMDAYKEN HİÇ İYİ OLMAMAM MÜMKÜN MÜ ? SANA NASIL YARDIMCI OLABİLİRİM ?";
                    SesliOkuma();
                }
                else if (numu == 3)
                {
                    textBox2.Clear();
                    textBox1.Text = "HARİKA BİR GÜN ! BEN DE TAM KARŞININ TAKSİSİ NE DEMEK ONU ÖĞRENİYORDUM.";
                    SesliOkuma();
                }
            }//Naber
            else if (mesaj == kelime_list[3])
            {
                textBox2.Clear();
                textBox1.Text = "ALEYKÜMSELAM. SENİN İÇİN NE YAPABİLİRİM ?";
                SesliOkuma();
            }//SelamınAleyküm
            else if (mesaj == kelime_list[4])
            {
                textBox2.Clear();
                textBox1.Text = "TAMAM, İŞTE GOOGLE İYİ ARAMALAR.";
                webBrowser1.Document.GetElementById("TextBox1").InnerText = "TAMAM, İŞTE GUGIL İYİ ARAMALAR.";
                webBrowser1.Document.GetElementById("Button1").InvokeMember("click");
                System.Diagnostics.Process.Start("https://www.google.com.tr");
            }//Google
            else if (mesaj == kelime_list[5] || mesaj == kelime_list[6])
            {
                textBox2.Clear();
                textBox1.Text = "SAAT : " + DateTime.Now.ToShortTimeString();
                SesliOkuma();
            }//Saat Kaç
            else if (mesaj == kelime_list[7] || mesaj == kelime_list[8] || mesaj == kelime_list[9])
            {
                textBox2.Clear();
                textBox1.Text = "DOSYALARINIZI AÇIYORUM.";
                System.Diagnostics.Process.Start("explorer.exe");
                SesliOkuma();
            }//DosyaAç
            else if (mesaj == kelime_list[10] || mesaj == kelime_list[11] || mesaj == kelime_list[12])
            {
                webBrowser2.Refresh();
                random = r.Next(1, 3);
                if (saat >= 12 && saat <= 17)
                {
                    if (random == 1)
                    {
                        textBox2.Clear();
                        string yer = webBrowser2.Document.GetElementById("wob_loc").InnerText;
                        string zaman = webBrowser2.Document.GetElementById("wob_dts").InnerText;
                        string hava_durumu = webBrowser2.Document.GetElementById("wob_dc").InnerText;
                        string derece = webBrowser2.Document.GetElementById("wob_tm").InnerText + "°C";
                        textBox1.Text = "TÜNAYDIN ! SAAT : " + DateTime.Now.ToShortTimeString() + " " + yer + " " + derece + " VE " + hava_durumu + " İYİ GÜNLER !";
                        SesliOkuma();
                    }
                    else if (random == 2)
                    {
                        textBox2.Clear();
                        textBox1.Text = "TÜNAYDIN SAAT : " + DateTime.Now.ToShortTimeString();
                        SesliOkuma();

                    }
                }
                else if (saat >= 17)
                {
                    if (random == 1)
                    {
                        textBox2.Clear();
                        string yer = webBrowser2.Document.GetElementById("wob_loc").InnerText;
                        string zaman = webBrowser2.Document.GetElementById("wob_dts").InnerText;
                        string hava_durumu = webBrowser2.Document.GetElementById("wob_dc").InnerText;
                        string derece = webBrowser2.Document.GetElementById("wob_tm").InnerText + "°C";
                        textBox1.Text = "İYİ AKŞAMLAR ! SAAT : " + DateTime.Now.ToShortTimeString() + " " + yer + " " + derece + " VE " + hava_durumu + " İYİ GÜNLER !";
                        SesliOkuma();
                    }
                    else if (random == 2)
                    {
                        textBox2.Clear();
                        textBox1.Text = "İYİ AKŞAMLAR ! SAAT : " + DateTime.Now.ToShortTimeString();
                        SesliOkuma();
                    }
                }
                else
                {
                    if (random == 1)
                    {
                        textBox2.Clear();
                        string yer = webBrowser2.Document.GetElementById("wob_loc").InnerText;
                        string zaman = webBrowser2.Document.GetElementById("wob_dts").InnerText;
                        string hava_durumu = webBrowser2.Document.GetElementById("wob_dc").InnerText;
                        string derece = webBrowser2.Document.GetElementById("wob_tm").InnerText + "°C";
                        textBox1.Text = "GÜNAYDIN ! SAAT : " + DateTime.Now.ToShortTimeString() + " " + yer + " " + derece + " VE " + hava_durumu + " İYİ GÜNLER !";
                        SesliOkuma();
                    }
                    else if (random == 2)
                    {
                        textBox2.Clear();
                        textBox1.Text = "GÜNAYDIN ! SAAT : " + DateTime.Now.ToShortTimeString();
                        SesliOkuma();
                    }
                }
            }//Hava Durumu
            else if (mesaj.Contains(kelime_list[13]))
            { 
                string a = textBox2.Text.Substring(0, textBox2.Text.Length - 14);
                textBox2.Clear();
                textBox1.Text = "TAMAM " + a + " YOUTUBE DA ARIYORUM.";
                webBrowser1.Document.GetElementById("TextBox1").InnerText = "TAMAM " + a + " yutub da arıyorum";
                webBrowser1.Document.GetElementById("Button1").InvokeMember("click");
                System.Diagnostics.Process.Start("https://www.youtube.com/results?search_query=" + a);
            }//Youtube
            else if (mesaj == kelime_list[14] || mesaj == kelime_list[15] || mesaj == kelime_list[16])
            {
                textBox2.Clear();
                textBox1.Text = "TAMAM, PAİNT'İ AÇIYORUM";
                SesliOkuma();
                System.Diagnostics.Process.Start("mspaint.exe");
            }//Paint
            else if (mesaj == kelime_list[17] || mesaj == kelime_list[18] || mesaj == kelime_list[19])
            {
                try
                {
                    textBox2.Clear();
                    textBox1.Text = "TAMAM, WORD'Ü AÇIYORUM";
                    SesliOkuma();
                    System.Diagnostics.Process.Start("WINWORD.exe");
                }
                catch (Exception)
                {

                    textBox2.Clear();
                    textBox1.Text = "HAY AKSİ WORD PROGRAMINI BULAMADIM !";
                    SesliOkuma();
                }

            }//Word
            else if (mesaj == kelime_list[20] || mesaj == kelime_list[21] || mesaj == kelime_list[22])
            {
                try
                {
                    textBox2.Clear();
                    textBox1.Text = "TAMAM, EXCEL'İ AÇIYORUM";
                    SesliOkuma();
                    System.Diagnostics.Process.Start("excel.exe");
                }
                catch (Exception)
                {

                    textBox2.Clear();
                    textBox1.Text = "HAY AKSİ EXCEL PROGRAMINI BULAMADIM !";
                    SesliOkuma();
                }

            }//Excel
            else if (mesaj == kelime_list[23] || mesaj == kelime_list[24] || mesaj == kelime_list[25])
            {
                textBox2.Clear();
                textBox1.Text = "TAMAM, NOT DEFTERİNİ AÇIYORUM";
                SesliOkuma();
                System.Diagnostics.Process.Start("notepad.exe");
            }//Not Defteri
            else if (mesaj == kelime_list[26] || mesaj == kelime_list[27])
            {
                textBox2.Clear();
                textBox1.Text = "TAMAM, YEREL DİSK C'Yİ AÇIYORUM";
                SesliOkuma();
                System.Diagnostics.Process.Start("c:");
            }//YEREL DİSK C
            else if (mesaj == kelime_list[28] || mesaj == kelime_list[29])
            {
                textBox2.Clear();
                textBox1.Text = "BEN TÜRK ASILLI BİR ASİSTANIM. VE TÜRKİYEDE İSTİKAMET EDİYORUM.";
                SesliOkuma();
            }//Nerelisin
            else if (mesaj == kelime_list[30])
            {
                int numu = r.Next(1,4);
                if (numu == 1)
                {
                    textBox2.Clear();
                    textBox1.Text = "TÜRK KAHVESİ VE YANINDA ÇİFTE KAVRULMUŞ LOKUM RESİMLERİNE Mİ BAKSAM DİYE DÜŞÜNÜYORUM.";
                    SesliOkuma();
                }
                else if (numu == 2)
                {
                    textBox2.Clear();
                    textBox1.Text = "BAŞIMA BİR ŞEY GELMEYECEKSE SÖYLEYEYİM. KEDİ VİDEOLARI SEYRETMEYİ DÜŞÜNÜYORUM.";
                    SesliOkuma();
                }
                else if (numu == 3)
                {
                    textBox2.Clear();
                    textBox1.Text = "YENİ BİLMECELER ÖĞRENMEYİ PLANLIYORUM.";
                    SesliOkuma();
                }
            }//NeYapıyorsun
            else if (mesaj == kelime_list[31] || mesaj == kelime_list[32] || mesaj == kelime_list[33] || mesaj == kelime_list[34])
            {
                
                int numu = r.Next(1,3);
                if (numu == 1)
                {
                    textBox2.Clear();
                    textBox1.Text = "KORKMA, SÖNMEZ BU ŞAFAKLARDA YÜZEN AL SANCAK SÖNMEDEN YURDUMUN ÜSTÜNDE TÜTEN EN SON OCAK O BENİM MİLLETİMİN YILDIZIDIR, PARLAYACAK O BENİMDİR, O BENİM MİLLETİMİNDİR ANCAK.";
                    SesliOkuma();
                }
                else if (numu == 2)
                {
                    textBox2.Clear();
                    textBox1.Text = "ARKADAŞ! YURDUMA ALÇAKLARI UĞRATMA SAKIN. SİPER ET GÖVDENİ, DURSUN BU HAYASIZCA AKIN. DOĞACAKTIR SANA VA'DETTİĞİ GÜNLER HAKK'IN. KİM BİLİR,BELKİ YARIN, BELKİ YARINDAN DA YAKIN.";
                    SesliOkuma();
                }
            }//Şiir Oku
            else if (mesaj == kelime_list[35])
            {
                int numu = r.Next(1,4);
                if (numu == 1)
                {
                    textBox2.Clear();
                    textBox1.Text = "SENİN İÇİN BURDAYIM";
                    SesliOkuma();
                }
                else if (numu == 2)
                {
                    textBox2.Clear();
                    textBox1.Text = "YARDIMCI OLABİLDİYSEM NE MUTLU BANA.";
                    SesliOkuma();
                }
                else if (numu == 3)
                {
                    textBox2.Clear();
                    textBox1.Text = "NE DEMEK LAFI BİLE OLMAZ.";
                    SesliOkuma();
                }
            }//Teşkkür Ederim
            else if (mesaj == kelime_list[36] || mesaj == kelime_list[36] || mesaj == kelime_list[37] || mesaj == kelime_list[38])
            {
                    textBox2.Clear();
                    textBox1.Text = "İŞTE ! YENİ HABERLER.";
                    SesliOkuma();
                    System.Diagnostics.Process.Start("https://www.haberler.com");
            }//Haberleri Göster
            else if (mesaj.Contains(kelime_list[39]))
            {
                string a = textBox2.Text.Substring(0, textBox2.Text.Length - 13);
                textBox2.Clear();
                textBox1.Text = "TAMAM " + a + " GOOGLE DA ARIYORUM.";
                webBrowser1.Document.GetElementById("TextBox1").InnerText = "TAMAM " + a + " gugıl da arıyorum";
                webBrowser1.Document.GetElementById("Button1").InvokeMember("click");
                System.Diagnostics.Process.Start("https://www.google.com/search?q=" + a);
            }//Hakkında Bilgi Göster
            else if (mesaj == kelime_list[40])
            {
                textBox2.Clear();
                textBox1.Text = "TAMAM. İŞTE İNSTAGRAM";
                System.Diagnostics.Process.Start("https://www.instagram.com");
                SesliOkuma();
            }//İnstagram
            else if (mesaj == kelime_list[41])
            {
                textBox2.Clear();
                textBox1.Text = "TAMAM. İŞTE FACEBOOK";
                System.Diagnostics.Process.Start("https://www.facebook.com");
                SesliOkuma();
            }//Facebook
            else if (mesaj == kelime_list[42])
            {
                Environment.Exit(0);
            }//Kapat
            else if (mesaj == kelime_list[43])
            {
                if (k == false)
                {
                    textBox2.Clear();
                    textBox1.Text = "BİLGİSAYARI KAPATMAK İSTİYORSANIZ ÖNCELİKLE EVET VEYA HAYIR DİYEREK TEKRAR BİLGİSAYARI KAPAT DEMENİZ GEREKMEKTEDİR.";
                    SesliOkuma();
                }
                else if(k == true)
                {
                    textBox2.Clear();
                    textBox1.Text = "BİLGİSAYAR 10 SANİYE İÇİNDE KAPATILACAKTIR.";
                    System.Diagnostics.Process.Start("shutdown", "-f -s -t 10");
                    SesliOkuma();
                }
            }//Bilgisayarı-Kapat
            else if (mesaj == kelime_list[44])
            {
                textBox2.Clear();
                textBox1.Text = "ŞİMDİ BİLGİSAYARI KAPAT DİYEBİLİRSİNİZ !";
                SesliOkuma();
                 k = true;
            }//Bilgisayarı-Kapat-Evet
            else if (mesaj == kelime_list[45])
            {
                textBox2.Clear();
                textBox1.Text = "BİLGİSİYAR KAPAT DEVRE DIŞI";
                System.Diagnostics.Process.Start("shutdown", "-a");
                SesliOkuma();
                k = false;
            }//Bilgisayarı-Kapat-Hayır
            else if (mesaj.Contains(kelime_list[46]))
            {
                string a = textBox2.Text.Substring(0, textBox2.Text.Length - 17);
                textBox2.Clear();
                textBox1.Text = a;
                SesliOkuma();
            }//Tekrar Eder misin
            else if (mesaj == kelime_list[47]|| mesaj == kelime_list[48]|| mesaj == kelime_list[49])
            {
                textBox2.Clear();
                textBox1.Text = "GOOGLE KAPATILDI !";
                webBrowser1.Document.GetElementById("TextBox1").InnerText = "gugıl kapatıldı.";
                webBrowser1.Document.GetElementById("Button1").InvokeMember("click");
                string processName = "chrome"; // Kapatmak İstediğimiz Program
                Process[] processes = Process.GetProcesses();// Tüm Çalışan Programlar
                foreach (Process process in processes)
                {
                    if (process.ProcessName == processName)
                    {
                        process.Kill();
                    }
                }
            }//Chrome Kapat
            else if (mesaj == kelime_list[50] || mesaj == kelime_list[51] || mesaj == kelime_list[52])
            {
                textBox2.Clear();
                textBox1.Text = "AYARLAR AÇILIYOR !";
                System.Diagnostics.Process.Start("ms-settings:");
                SesliOkuma();
            }//Ayarlar
            else if (mesaj == kelime_list[53] || mesaj == kelime_list[54] || mesaj == kelime_list[55])
            {
                textBox2.Clear();
                textBox1.Text = "DENETİM MASASI AÇILIYOR !";
                System.Diagnostics.Process.Start("control");
                SesliOkuma();
            }//Denetim Masası
            else if (mesaj == kelime_list[56])
            {
                textBox2.Clear();
                textBox1.Text = "ÖRNEK SORULAR";
                SesliOkuma();
                string cikti = string.Join("\n", kelime_list);
                MessageBox.Show("Merhaba\n Nasılsın\n Selamın Aleyküm\n Hava Durumu\n Google\n Şiir Oku","Jarvis Asistan",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }//Örnek Sorular
            else if (mesaj == kelime_list[57])
            {
                textBox2.Clear();
                textBox1.Text = "İYİ BİRİNE BENZİYOR.";
                SesliOkuma();
             
            }//Google Asistan
            else if (mesaj == kelime_list[58])
            {
                int num = r.Next(1, 3);
                textBox2.Clear();
                if(num == 1)
                {
                    textBox1.Text = "İŞTE YAZI TURA CEVABI [YAZI].";
                }
                else
                {
                    textBox1.Text = "İŞTE YAZI TURA CEVABI [TURA] ATAMIZIN YÜZÜ YERE GELMESİN.";
                }
                SesliOkuma();
                
            }//Yazı Tura
            else if (mesaj == kelime_list[59])
            {
                int num = r.Next(1, 3);
                textBox2.Clear();
                if (num == 1)
                {
                    textBox1.Text = "BEN JARVİS SENİN İSTEKLERİNİ GERÇEKLEŞTİRMEK İÇİN BURADAYIM.";
                }
                else
                {
                    textBox1.Text = "BEN GELİŞMİŞ BİR YAPAY ZEKAYIM.";
                }
                SesliOkuma();

            }//Sen Kimsin
            else if (mesaj.Contains(kelime_list[60]))
            {
                int num = r.Next(1, 3);//ne demek
                string a = textBox2.Text.Substring(0, textBox2.Text.Length - 7);
                textBox2.Clear();
                textBox1.Text = "TAMAM "+a + " NE DEMEK ?";
                System.Diagnostics.Process.Start("https://www.google.com/search?q=" + a);
                webBrowser1.Document.GetElementById("TextBox1").InnerText = "TAMAM " + a + "NE DEMEK ?";
                SesliOkuma();

            }//Ne demek
            else if (mesaj.Contains(kelime_list[61]))
            {
                int num = r.Next(1, 3);//ingilizcede nasıl söylenir
                string a = textBox2.Text.Substring(0, textBox2.Text.Length - 26);
                textBox2.Clear();
                //webBrowser3.Document.GetElementById("resizer").InvokeMember("click");
                webBrowser3.Document.GetElementById("textarea").InnerText = a;
                Thread.Sleep(100);
                string cevap = webBrowser3.Document.GetElementById("translation").InnerText;
                textBox1.Text = a + " İNGİLİZCEDE [" + cevap + "] DİYE SÖYLENİR";
                SesliOkuma();

            }//Nasıl söylenir
            foreach (string item in listBox1.Items)
            {
                if (item.ToLower().Contains(mesaj.ToLower())) { }
                else
                {
                    string a = textBox2.Text;
                    textBox2.Clear();
                    textBox1.Text = a + " HAKKINDA BUNLARI BULDUM.";
                    webBrowser1.Document.GetElementById("TextBox1").InnerText = "TAMAM " + a + " gugıl da arıyorum";
                    webBrowser1.Document.GetElementById("Button1").InvokeMember("click");
                    System.Diagnostics.Process.Start("https://www.google.com/search?q=" + a);
                }
            }//Hiç Bir İhtimal Gerçekleşmesse
        }
    
    }
}
