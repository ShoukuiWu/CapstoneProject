using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;


using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading; 

namespace WcfServiceConeInfo
{
    /* Test object data and operation with string */
    [DataContract]
    public class Customer
    {
        [DataMember]
        public string Name;

        [DataMember]
        public string Address;
    }

    /* For creating searchObject */
    [DataContract]
    public class SearchResult
    {
        [DataMember]
        public string question;

        [DataMember]
        public string answer;
    }

    [ServiceContract(Namespace = "JsonpAjaxService")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ServiceConeInfo
    {
        /* Test object data and operation with string */
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        public Customer GetCustomer()
        {
            return new Customer() { Name = "Provided WCF Service", Address = "Cambridge" };
        }

        ////////////////////////////////////////////////////////////////
        /* For Keyword related operations in ServiceConeInfo service */
        ////////////////////////////////////////////////////////////////
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        public List<string> GetKeywordNameAll()
        {
            try
            {
                using (CONEINFOContext entities = new CONEINFOContext())
                {
                    return entities.Keywords.Select(r => r.name).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException("GetKeywordNameAll went wrong: " + ex.GetBaseException());
            }
        }

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        public List<string> GetKeywordByQuery(string query)
        {
            try
            {
                using (CONEINFOContext entities = new CONEINFOContext())
                {
                    return entities.Keywords.Where(r => r.name.Contains(query)).Select(r => r.name).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException("GetKeywordByQuery went wrong: " + ex.GetBaseException());
            }
        }

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        public string GetKeywordById(int id)
        {
            try
            {
                using (CONEINFOContext entities = new CONEINFOContext())
                {
                    return entities.Keywords.Where(r => r.type == id).Select(r => r.name).Single().ToString();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException("GetKeywordById went wrong: " + ex.GetBaseException());
            }
        }

        ////////////////////////////////////////////////////////////////
        /* For Search related operations in ServiceConeInfo service */
        ////////////////////////////////////////////////////////////////
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        public List<SearchResult> GetTopTenAll()
        {
            try
            {
                using (CONEINFOContext entities = new CONEINFOContext())
                {
                    List<SearchResult> searchResults = new List<SearchResult>();

                    var search = from r in entities.Searches
                                 where r.top10 != 0
                                 orderby r.top10
                                 select new SearchResult
                                 {
                                     question = r.question,
                                     answer = r.answer,
                                 };

                    foreach (var a in search)
                    {
                        SearchResult searchResult = new SearchResult();
                        searchResult.question = a.question;
                        searchResult.answer = a.answer;
                        searchResults.Add(searchResult);
                    }

                    return searchResults;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException("GetQuestionAnswerByQuery went wrong: " + ex.GetBaseException());
            }
        }
        

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        public List<SearchResult> GetQuestionAnswerByQuery(string query)
        {
            try
            {
                using (CONEINFOContext entities = new CONEINFOContext())
                {
                    //return entities.Searches.Where(r => r.Keyword.name.Contains(query)).Select(r => r.answer).ToList();

                    List<SearchResult> searchResults = new List<SearchResult>();

                    var search = from r in entities.Searches
                                // where (r.Keyword.name.Contains(query) || r.question.Contains(query))
                                 where r.Keyword.name.Contains(query)
                                 select new SearchResult
                                 {
                                     question = r.question,
                                     answer = r.answer
                                 };

                    foreach (var a in search)
                    {
                        SearchResult searchResult = new SearchResult();
                        searchResult.question = a.question;
                        searchResult.answer = a.answer;
                        searchResults.Add(searchResult);
                    }

                    return searchResults;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException("GetQuestionAnswerByQuery went wrong: " + ex.GetBaseException());
            }
        }

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        public List<string> GetQuestionAnswerByTopTen(int id)
        {
            try
            {
                using (CONEINFOContext entities = new CONEINFOContext())
                {
                    return entities.Searches.Where(r => r.top10 == id).Select(r => r.question).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException("GetQuestionAnswerByTopTen went wrong: " + ex.GetBaseException());
            }
        }

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        public string SendEmail(string emailToAddress, string name, string address, string body)
        {
            /* For configuration of setting with mail server */
            string SMTPSERVER = "smtp.gmail.com";
            int PORTNO = 587;
            string gmailUserName = "jjhuhgroup2@gmail.com";
            string gmailUserPassword = "jjhuhgroup2cad";
            string[] ccemailTo=null;
            string subject = "Conestoga College Search Mail from: " + name + "(" + address + ")";
            bool isBodyHtml=true;

            if (gmailUserName == null || gmailUserName.Trim().Length == 0)
            {
                return "User Name Empty";
            }
            if (gmailUserPassword == null || gmailUserPassword.Trim().Length == 0)
            {
                return "Email Password Empty";
            }
            if (emailToAddress == null || emailToAddress.Length == 0)
            {
                return "Email To Address Empty";
            }

            List<string> tempFiles = new List<string>();

            SmtpClient smtpClient = new SmtpClient(SMTPSERVER, PORTNO);
            smtpClient.EnableSsl = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(gmailUserName, gmailUserPassword);
            using (MailMessage message = new MailMessage())
            {
                message.From = new MailAddress(gmailUserName);
                message.Subject = subject == null ? "" : subject;
                message.Body = body == null ? "" : body;
                message.IsBodyHtml = isBodyHtml;

                /*
                foreach (string email in emailToAddress)
                {
                    message.To.Add(email);
                }*/
                //message.To.Add(emailToAddress);
               // message.To.Add(mailData[0]);

                message.To.Add(emailToAddress);
                
                if (ccemailTo != null && ccemailTo.Length > 0)
                {
                    foreach (string emailCc in ccemailTo)
                    {
                        message.CC.Add(emailCc);
                    }
                }
                try
                {
                    smtpClient.Send(message);
                    return "Email Send SuccessFully";
                }
                catch
                {
                    return "Email Send failed";
                }
            }
        }     
   
    }
}
