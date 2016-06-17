// <copyright file="MainWindow.xaml.cs" company="SLM Software GmbH">SLM Software 2016. All rights reserved.</copyright>
// All rights reserved.
// 
// Author: Raphael Stamminger
// Last Modify: 2016-06-17 11:17
// Created: 2016-06-01 08:19 

namespace MQTTClient
{
    using System.Diagnostics;
    using System.Net;
    using System.Text;
    using System.Windows;
    using uPLibrary.Networking.M2Mqtt;
    using uPLibrary.Networking.M2Mqtt.Messages;

    /// <summary>
    ///     Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
	{
		// MQTT
		private MqttClient client;

		public MainWindow()
		{
			InitializeComponent();
		}

		public void btn_connect_Click(object sender, RoutedEventArgs e)
		{
			var ipadress = tb_ipaddress.Text; // Get the ipaddress
			var port = tb_port.Text; // Get the port
			var clientId = tb_clientid.Text; // Get the clientid

			// Connect with the Client over IP-Address
			client = new MqttClient(IPAddress.Parse(ipadress));

			// register to message received 
			client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

			// Try to connect with the Mqtt Server
			try
			{
				// Get ClientID from TextBox and register
				client.Connect(clientId);
				grid.Visibility = Visibility.Visible;
				lb_error.Content = "Successfull Connected";
            }
			catch
			{
				lb_error.Content = "Connection Error";
			}
		}

		public void btn_send_Click(object sender, RoutedEventArgs e)
		{
			var topic = tb_topic.Text;
			var message = tb_message.Text;

			// Send a message
			client.Publish(topic, Encoding.UTF8.GetBytes(message));
		}

		public void btn_subscribe_Click(object sender, RoutedEventArgs e)
		{
			var topic_get = tb_topic_get.Text;

			// subscribe to the topic "/topic" 
			client.Subscribe(new[] {topic_get}, new[] {MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE});
		}

		public void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
		{
			// Writes the message into the Debug Line
			Debug.WriteLine("Received Data: " + Encoding.UTF8.GetString(e.Message));
		}
	}
}