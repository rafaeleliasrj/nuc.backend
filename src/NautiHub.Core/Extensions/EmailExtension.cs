using System.Net.Mail;

namespace NautiHub.Core.Extensions;

public static class EmailExtension
{
    public static bool EmailIsValid(this string email)
    {
        if (email == null)
            return false;

        try
        {
            MailAddress m = new MailAddress(email);

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
