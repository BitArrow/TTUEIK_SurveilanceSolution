#r "SendGrid"
using System;
using SendGrid.Helpers.Mail;

public static void Run(string myQueueItem, TraceWriter log, out Mail message)
{
    log.Info($"C# Event Hub trigger function processed a message: {myQueueItem}");
    SendMail(myQueueItem, log, out message);
}

public static void SendMail(string input, TraceWriter log, out Mail message)
{
    log.Info($"Sending email at {DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")}");
    message = new Mail
    {        
        Subject = "Alert!"          
    };

    var personalization = new Personalization();
    // change to email of recipient
    personalization.AddTo(new Email("tauribusch@hotmail.com"));

    Content content = new Content
    {
        Type = "text/html",
        Value = $"<h1>There was an alert!</h1><br /><p>{input}</p>"
    };
    message.AddContent(content);
    message.AddPersonalization(personalization);
    log.Info($"Email sent at {DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")}");
}