using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
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
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace MQTTClient
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MqttClient client;                      //MQTT Client

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_connect_Click(object sender, RoutedEventArgs e)
        {
            String ipadress = tb_ipaddress.Text;    //Get the ipaddress
            String port = tb_port.Text;             //Get the port
            String clientId = tb_clientid.Text;     //Get the clientid

            //Connect with the Client over IP-Address
            client = new MqttClient(IPAddress.Parse(ipadress));

            // register to message received 
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            //Try to connect with the Mqtt Server
            try
            {
                //Get ClientID from TextBox and register
                client.Connect(clientId);
                grid.Visibility = Visibility.Visible;
                lb_error.Content = "Successfull Connected";
            }
            catch
            {
                lb_error.Content = "Connection Error";
            }
        }

        private void btn_send_Click(object sender, RoutedEventArgs e)
        {
            String topic = tb_topic.Text;
            String message = tb_message.Text;

            //Send a message
            client.Publish(topic, Encoding.UTF8.GetBytes(message));
        }

        private void btn_subscribe_Click(object sender, RoutedEventArgs e)
        {
            String topic_get = tb_topic_get.Text;

            // subscribe to the topic "/topic" 
            client.Subscribe(new string[] { topic_get }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        }

        private void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            //Writes the message into the Debug Line
            Debug.WriteLine("Received Data: " + Encoding.UTF8.GetString(e.Message));
        }
    }
}

