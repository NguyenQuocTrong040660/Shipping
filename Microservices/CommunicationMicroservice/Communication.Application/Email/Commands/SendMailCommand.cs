﻿using Communication.Domain.AppSetting;
using Communication.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Communication.Application.Common.Results;

namespace Communication.Application.Email.Commands
{
    public class SendMailCommand : IRequest<Result>
    {
        public EmailModel Email { get; set; }
    }

    public class SendMailCommandHandler : IRequestHandler<SendMailCommand, Result>
    {
        private readonly IWebHostEnvironment _environment;
        private readonly EmailConfiguration _emailConfiguration;
        private readonly ILogger<SendCompletedEventHandler> _logger;
        public SendMailCommandHandler(IOptions<EmailConfiguration> emailConfiguration, ILogger<SendCompletedEventHandler> logger, IWebHostEnvironment environment)
        {
            _emailConfiguration = emailConfiguration.Value ?? throw new ArgumentNullException(nameof(emailConfiguration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _environment = environment;
        }

        public async Task<Result> Handle(SendMailCommand request, CancellationToken cancellationToken)
        {
            return await SendEmailForAdmin(request.Email);
        }

        private async Task<Result> SendEmailForAdmin(EmailModel email)
        {
            string subject = "Thông Tin Đặt Lịch";

            try
            {
                var loginInfo = new NetworkCredential(_emailConfiguration.SenderEmailAddress, _emailConfiguration.SenderEmailPassword);
              
                string messageContent = string.Format(
                      "<div style='margin-left:10%;'><h3 style='color:#004080'><i>Kính gửi:" + "Sale Department" + ",</i></h3></div>" +
                    "<div style='margin-left:10%'>" + "<h2><i>" + "Bạn vừa nhận được yêu cầu tư vấn sản phẩm!" + "</i></h2>" + "</div>" + "<br/>" +
                      //"<div style='margin-left:20%;'>" + "<img style='width:10px;height: 10px;margin-right:20px' src='https://i.pinimg.com/originals/e5/ec/87/e5ec8798013da1b72baef43420e1b2df.png' alt=''>" + "Nhân viên tư vấn sẽ liên hệ lại bạn trong giờ hành chính từ 08:00 đến 17:00 hàng ngày." + "</div>" + "<br/>" +
                      //"<div style='margin-left:20%;'>" + "<img style='width:10px;height: 10px;margin-right:20px' src='https://i.pinimg.com/originals/e5/ec/87/e5ec8798013da1b72baef43420e1b2df.png' alt=''>" + "Thông tin chi tiết đơn hàng và hình thức thanh toán sẽ hiển thị dưới đây để bạn biết thêm thông tin." + "</div>" + "<br/>" +
                      //"<div style='margin-left:20%;'>" + "<img style='width:10px;height: 10px;margin-right:20px' src='https://i.pinimg.com/originals/e5/ec/87/e5ec8798013da1b72baef43420e1b2df.png' alt=''>" + "Sau khi đơn hàng được xác định, chúng tôi sẽ vận chuyển sản phẩm về xưởng theo yêu cầu của quý khách." + "</div>" + "<br/>" +
                      //"<div style='margin-left:10%;'><h3><i>Chi Tiết Giỏ Hàng</i></h3></div>" + cart +
                      "<div style='margin-left:10%;'><h3><i>Thông tin khách hàng</i></h3></div>" +
                    "<div style='margin-left:20%;'>" + "<i>Tên khách hàng: </i><b>" + email.MsgEmailContactText + "</b></div>" +
                     //"<div style='margin-left:10%;'><h3><i>An Tâm mua hàng</i></h3></div>" +
                     // "<div style='margin-left:20%;'>" + "<img style='width:10px;height: 10px;margin-right:20px' src='https://i.pinimg.com/originals/e5/ec/87/e5ec8798013da1b72baef43420e1b2df.png' alt=''>" + "Thông tin của quý khách hàng sẽ được đảm bảo bảo mật khi mua hàng tại doanh nghiệp của chúng tôi." + "</div>" + "<br/>" +
                     "<div style='margin-left:10%;'><h3><i>Xin trân trọng cảm ơn!</i></h3></div>" +
                        "<div style='margin-left:10%;'><h3 style='color:#004080'>HAVANA FOOT MASSAGE</h3></div>" +
                     "<div style='margin-left:10%'><img style='width:300px;height: 200px;' src='https://api.hoakhoidulichvietnam.vn/images/14_1_202121_877900595logo-havana1.png' alt=''></div>" +

                       "<div style='margin-left:10%;'>" + "<i>Hotline: </i><b>" + "(024) 3939 2528 – 098 123 2299" + "</b></div>" +
                     "<div style='margin-left:10%;'>" + "<i>Email: </i><b>" + "cskh.havana@gmail.com" + "</b></div>" +
                     "<div style='margin-left:10%;'>" + "<i>Địa chỉ: </i><b>" + "Số 3 Đình Ngang – Hoàn Kiếm – Hà Nội" + "</b></div><br/>" +


                     //"<div style='margin-left:10%'>" + "<small><i>" + "Thông tin của quý khách hàng sẽ được đảm bảo bảo mật khi mua hàng tại doanh nghiệp của chúng tôi.." + "</i></small>" + "</div>" +
                     // "<div style='margin-left:10%'>" + "<small><i>" + "Quý khách nhận được thư điện tử này bởi vì Quý khách đã cung cấp địa chỉ email cho hệ thống chúng tôi, nếu đây là nhầm lẩn xin vui lòng bỏ qua email này." + "</i></small>" + "</div>" +
                     "<div style='margin-left:10%'><i>Đây là thư điện tử, vui lòng không trả lời email này!</i></div>"
                    );


                var msg = new MailMessage();

                using var smtpClient = new SmtpClient("smtp.gmail.com", 587);

                msg.From = new MailAddress(_emailConfiguration.SenderEmailAddress);
                msg.To.Add(new MailAddress(_emailConfiguration.RecieverEmailAddress));
                msg.Subject = subject;

                msg.Body = messageContent;
                msg.IsBodyHtml = true;
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = true;
                smtpClient.Credentials = loginInfo;

                await smtpClient.SendMailAsync(msg);

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), nameof(SendEmailForAdmin));
            }

            return Result.Failure("Failed to send email");
        }


        private string GenerateEmailContent(ContentEmailModel content)
        {
            string result = "";

            //DEAR
            result = "Gửi " + content.MsgDearText + ",";

            //INTRODUCE
            result = result + "<br><br>";
            result = result + content.MsgConcernText;

            //CONTACT BY PHONE
            result = result + "<br>";
            result = result + content.MsgPhoneContactText;

            //CONTACT BY EMAIL
            if (!string.IsNullOrWhiteSpace(content.MsgEmailContactText))
            {
                result = result + "<br><br>";
                result = result + content.MsgEmailContactText;
            }

            //ADD NOTED
            if (!string.IsNullOrWhiteSpace(content.MsgRemark))
            {
                result = result + "<br>";
                result = result + content.MsgRemark;
            }

            //BEST REGARDS
            result = result + "<br><br>";
            result = result + content.MsgRegards;

            //SIGNATURE
            //result = result + "<br>";
            //result = result + "<strong>" + content.MsgSignature + "</strong>";

            //COMMENTS
            //result = result + "<br><br>";
            //result = result + "<strong>-------------------------------";
            //result = result + _emailConfiguration.EmailNoted + _emailConfiguration.RecieveCompany;
            //result = result + "-------------------------------</strong>";

            return result;
        }

        private ContentEmailModel SetDefaultContentEmail(EmailModel emailInformation)
        {
            ContentEmailModel contentEmail = new ContentEmailModel();

            contentEmail.MsgDearText = _emailConfiguration.RecieveDepartment;
            contentEmail.MsgRegards = _emailConfiguration.RegardsText;
            contentEmail.MsgSignature = emailInformation.MsgSignature;
            contentEmail.SenderEmailAddress = emailInformation.SenderEmailAddress;
            contentEmail.RecieverEmailAddress = _emailConfiguration.RecieverEmailAddress;

            contentEmail.MsgConcernText = "Cám ơn bạn đã để lại thông tin cho chúng tôi, chúng tôi đã nhận được thông tin từ bạn và sẽ liên hệ lại cho bạn sớm nhất";

            if (!string.IsNullOrWhiteSpace(emailInformation.EmailAddress))
            {
                contentEmail.MsgEmailContactText = " ";
            }
            return contentEmail;
        }

        private static string ReverseDate(string s)
        {
            string[] arrListStr = s.Split('-');
            string year = arrListStr[0];
            string month = arrListStr[1];
            string day = arrListStr[2];
            return new string(day + "-" + month + "-" + year);
        }

        private static string ConvertSetDate(string s)
        {
            string dateCut = s.Substring(0, 10);
            string date = ReverseDate(dateCut);
            string time = s.Substring(11);
            return new string(date + " lúc " + time);
        }

        //public void SendEmail(Models.Order order)
        //{
        //    string address = "an.th@grex-solutions.com";
        //    string subject = "Mail Tư Vấn Sản Phẩm";
        //    string cart = "";
        //    string email = "tangan2215@gmail.com";
        //    string password = "antrinh2315";

        //    var loginInfo = new NetworkCredential(email, password);
        //    var msg = new MailMessage();
        //    var smtpClient = new SmtpClient("smtp.gmail.com", 587);

        //    if (order.OrderDetails.Count > 0)
        //    {
        //        foreach (var item in order.OrderDetails)
        //        {
        //            cart += "<br/>" + "Tên sản phẩm: " + item.ProductName;
        //        }
        //    }


        //    msg.From = new MailAddress(email);
        //    msg.To.Add(new MailAddress(address));
        //    msg.Subject = subject;
        //    msg.Body = string.Format("Bạn vừa nhận được liên hê từ: <b style='color:red'>{0}</b><br/>SĐT: {1}<br/>Email: {2}<br/>Nội dung: yêu cầu tư vấn sản phẩm  </br>" + cart, order.Customer.CustomerName, order.Customer.PhoneNumber, order.Customer.Email);
        //    msg.IsBodyHtml = true;

        //    smtpClient.EnableSsl = true;
        //    smtpClient.UseDefaultCredentials = false;
        //    smtpClient.Credentials = loginInfo;
        //    smtpClient.Send(msg);
        //}
    }
}
