// See https://aka.ms/new-console-template for more information

using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

var factory = new ConnectionFactory
{
    Uri = new Uri("amqp://guest:guest@localhost:5672")
};

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();
channel.QueueDeclare("demo-queue",durable:true,
    exclusive:false,autoDelete:false,arguments:null);
var message = new {Name="Producer",Message="Hello !"};
var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
channel.BasicPublish("","demo-queue",null,body);

