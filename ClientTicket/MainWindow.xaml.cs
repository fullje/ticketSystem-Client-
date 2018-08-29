using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.Net;
using System.Data;

namespace ClientTicket
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadAll();
        }
        private void LoadAll()
        {
            userName.Text = Environment.UserName.ToString();
            computerName.Text = Environment.MachineName.ToString();
            string hname = Dns.GetHostName();
            address.Text = Dns.GetHostAddresses(hname)[1].ToString();
        }

        private void sendEvent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(telNumber.Text != "" || roomNumber.Text != "" || textBox.Text != "")
                {
                    connectDB conn = new connectDB("ticketSys", "root", "localhost", "");

                    conn.Insert(userName.Text.ToString(), computerName.Text.ToString(), address.Text.ToString(), textBox.Text.ToString(), telNumber.Text.ToString(), roomNumber.Text.ToString(), DateTime.Now.ToString(), true);
                    MessageBox.Show("Wiadomość została wysłana. Czekaj na kontakt informatyka");

                    telNumber.Text = "";
                    roomNumber.Text = "";
                    textBox.Text = "";
                }else
                {
                    MessageBox.Show("Wypelnij wszystkie pola");
                }
            }catch(Exception xe)
            {
                MessageBox.Show(xe.Message);
            }
        }
    }

    class connectDB
    {
        static string setConnection;
        static string nameDB = null;
        static string userNameDB = null;
        static string serverAddrDB = null;
        static string serverPassDB = null;
        MySqlConnection ticketDB;

        public void Connection(string saDB, string spDB)
        {
            setConnection = "SERVER=" + serverAddrDB + ";DATABASE=" + nameDB + "; User ID=" + userNameDB + ";Password=" + serverPassDB + ";SSLmode=none";
            ticketDB = new MySqlConnection(setConnection);
            ticketDB.Open();

            ticketDB.Close();
        }

        public connectDB(string nDB, string unDB, string saDB, string spDB)
        {
            nameDB = nDB;
            userNameDB = unDB;
            serverAddrDB = saDB;
            serverPassDB = spDB;
        }

        public void Insert(string userName, string computerName, string address, string text, string tel, string room, string date, bool status)
        {
            Connection(serverAddrDB, serverPassDB);
            string Query = "INSERT INTO events (UserName, ComputerName, Address, Text, Tel, RoomNumber, Date, Status) VALUES ('" + userName + "','" + computerName + "','" + address + "','" + text + "','" + tel + "','" + room + "','" + date + "'," + status +");";
            MySqlCommand commandInsert = new MySqlCommand(Query, ticketDB);
            MySqlDataReader reader;
            ticketDB.Open();

            reader = commandInsert.ExecuteReader();

            reader.Close();
            ticketDB.Close();
        }
    }
}
