using System.Net.Mail;
using System.Web;
using SharedKernel.Core;

namespace SharedKernel.Libraries.Helpers.Models;

public class EmailOptionRequest
{
    public EmailOptionRequest()
    {
        
    }

    public EmailOptionRequest(string to, string subject, string body)
    {
        (To, Subject, Body) = (to, subject, body);
    }

    public EmailOptionRequest(string to, string subject, string body, string displayName)
    {
        (To, Subject, Body, DisplayName) = (to, subject, body, displayName);
    }
    
    public EmailOptionRequest(string sender, string to, string subject, string body, string displayName)
    {
        
        (_sender, To, Subject, Body, DisplayName) = (sender, to, subject, body, displayName);
    }
    
    private string _sender;
    
    private string _displayName;

    public string Sender
    {
        get
        {
            if (string.IsNullOrEmpty(_sender))
            {
                _sender = DefaultEmailConfig.From;    
            }
            return _sender;
        }
    }
    
    public string To { get; set; }

    public string Subject { get; set; }

    public string Body { get; set; }

    public string DisplayName
    {
        get
        {
            if (string.IsNullOrEmpty(_displayName))
            {
                _displayName = DefaultEmailConfig.DisplayName;
            }
            return _displayName;
        }
        set
        {
            _displayName = value;
        }
    }
    public MailPriority Priority { get; set; } = MailPriority.Normal;

    public bool IsBodyHTML => Body != HttpUtility.HtmlEncode(Body);
}