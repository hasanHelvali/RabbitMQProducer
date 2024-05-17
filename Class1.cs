using RabbitMQ.Client;
using System.Text;

namespace RabbitMQ_Producer
{
    public class Producer
    {
        static void Main(string[] args)
        {
            //-----------------Baglantı Olusturma----------------------
            ConnectionFactory connectionFactory=new ConnectionFactory();
            //Bir connection baslatmak icin ConnectionFactory nesnesine ihtiyacımız vardır. Burada bir baglantı nesnesi olusturuyoruz.

            //connectionFactory.HostName = "localhost...";
            //connectionFactory.Password= "12345";
            //Hostname ve password elimizdeyse bu sekilde bildirebiliriz.

            //connectionFactory.Uri = new Uri("..."); 
            //Bu sekilde AMQP protokolünü de kullanabiliriz. Bu bilgileri RabbitMQ nun arayuzunden aldım.

            connectionFactory.Uri =  new Uri("amqp://guest:guest@localhost:5672/");// RabbitMQ URI


            //---------------Baglantıyı Aktiflestirme ve Kanal Acma-----------------------

            using IConnection connection = connectionFactory.CreateConnection();//Burada connection i olusturuyorum.
            /*IConnection bir IDisposable arayuzunden türeyen bir arayüzdür bu yüzden using ile isaretliyorum ki bu
            islem tamamlandıktan sonra bellekten dispose edilip bellekte gerekli  location lar temizlenmis olsun.
            Bunu da optimizasyon icin yapıyoruz.*/

            using IModel channel = connection.CreateModel();//Burada da bir kanal olusturuyorum. Bu yapıda bir IDisposable yapılanmadır.



            //----------------Queue Olusturma ----------------------
            channel.QueueDeclare(queue:"example-queue",exclusive:false);//Bir queue tanımlandı ve example-queue ismi verildi.
            /*exclusive parametresi true olursa cunsomer bu kuyruga erisemeden bu kuyruk silinir. Deatylarına bakılabilir.
            exclusive parametresi bu kuyruun ozel olmadıgını, birden fazla channel in bu kuyruga ulasabilecegini belirtir.
            Buradaki bask akullanılabilecek parametreler de vardır.*/


            //--------------Queue ya Mesaj Gonderme---------------------
            /*Burada bilmemiz gereken en onemli sey rabbitmq kuyruga alacagı mesajları byte tipinde kabul eder. Haliyle bizim 
             message ları byte a donusturmemiz gerekecektir.*/
            byte[] message = Encoding.UTF8.GetBytes("Selamun Aleykum");//Bir mesaj olusturuldu.
            channel.BasicPublish(exchange:"",routingKey: "example-queue",body:message);
            /*
            Channel uzerinden ilgili fonksiyonla mesajımı gonderebilirim.
             Burada ilk parametrede exchange ismini ister. Biz suan bunu bos gectik. Yani default exchange i yani direct exchange i 
             kullanacagımızı belirttik. Direct exchange de mesaj kuyruguna mesaj gondereceksem routing key in ismi queue nun ismiyle aynı 
            olmalıydı. Bu sebeple kuyrugun ismini routing key olarak bildiriyorum.
            Body parametresine ise mesajın kendisini veriyorum. Bu sekilde bir konfigurasyon tercih ettik. Bu islemler sonucunda rabbitmq da 
            bir message i basarılı bir sekilde publish etmis oluyoruz.*/

            Console.Read();//konsol uygulamasını aktif bir sekilde tutalım.
        }


    }
}
