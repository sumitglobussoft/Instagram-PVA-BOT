using Instagram_PVA_Bot.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccountCreation;
using PhoneVerification;

namespace Instagram_PVA_Bot
{
    public class GlobalDeclaration
    {
        public static AccountCreation.AccountCreationModule objAccountCreation = new AccountCreation.AccountCreationModule();
        public static PhoneVerification.PhoneVerification objPhoneVerification = new PhoneVerification.PhoneVerification();
        public static UploadPic.UploadImage objUploadImage = new UploadPic.UploadImage();
    }
}
