using System.Net.Mail;
using System.Net;

namespace TaskManager;

public class EmailService
{
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _fromEmail;
    private readonly string _fromPassword;

    public EmailService(string smtpServer, int smtpPort, string fromEmail, string fromPassword)
    {
        _smtpServer = smtpServer;
        _smtpPort = smtpPort;
        _fromEmail = fromEmail;
        _fromPassword = fromPassword;
    }

    public void SendEmail(List<string> emaiList, string body, string subject)
    {
        // создаем объект для SMTP сервера
        SmtpClient smtp = new SmtpClient(_smtpServer, _smtpPort);
        smtp.Credentials = new NetworkCredential(_fromEmail, _fromPassword);
        smtp.EnableSsl = true;
        smtp.Timeout = 10000;
        
        // создаем объект сообщения
        MailMessage message = new MailMessage();
        message.From = new MailAddress(_fromEmail); // от кого
        // добавляем получаетелей
        
        if (emaiList.Count == 0)
        {
            Console.WriteLine("Не заданы получатели.");
            return;
        }
        
        foreach (var mail in emaiList)
        {
            message.To.Add(mail);
        }

        message.Subject = subject;
        message.Body = body;

        try
        {
            smtp.Send(message);
            Console.WriteLine($"Письмо отправлено");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при отправке: {ex.Message}");
        }
        
    }
}