using System;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;

namespace CheatSheet
{
    static class Network
    {
        static class Email
        {
            internal static string GetUser(string emailAddress) => new MailAddress(emailAddress).User;
            internal static string GetHost(string emailAddress) => new MailAddress(emailAddress).Host;
        }

        // If success, (Item1, Item2) is (non-null, null).
        // If failure, (Item1, Item2) is (null, non-null).
        static ValueTuple<string, string> Ping1(string address)
        {
            using (var p = new Ping())
            {
                try
                {
                    return ValueTuple.Create<string, string>(p.Send(address)?.Status.ToString(), null);
                }
                catch (Exception e)
                {
                    return ValueTuple.Create<string, string>(null, e.ToString());
                }
            }
        }

        // If success, (Item1, Item2) is (non-null, null).
        // If failure, (Item1, Item2) is (null, non-null).
        static ValueTuple<string, string> GetHttpResponse(string uri)
        {
            var hwr = (HttpWebRequest)WebRequest.Create(uri);

            hwr.UseDefaultCredentials = true;
            hwr.CookieContainer = new CookieContainer();

            try
            {
                using (var wr = hwr.GetResponse())
                {
                    using (var stream = wr.GetResponseStream())
                    {
                        return ValueTuple.Create<string, string>(Converter.ToString(stream), null);
                    }
                }
            }
            catch (Exception e)
            {
                return ValueTuple.Create<string, string>(null, e.ToString());
            }
        }

        // If success, (Item1, Item2) is (non-null, null).
        // If failure, (Item1, Item2) is (null, non-null).
        static ValueTuple<byte[], string> GetFtpResponse(string uri, string userName, string password)
        {
            var fwr = (FtpWebRequest)WebRequest.Create(uri);

            fwr.Credentials = new NetworkCredential(userName, password);

            try
            {
                using (var response = fwr.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        return ValueTuple.Create<byte[], string>(Converter.ToBytes(stream), null);
                    }
                }
            }
            catch (Exception e)
            {
                return ValueTuple.Create<byte[], string>(null, e.ToString());
            }
        }
    }
}