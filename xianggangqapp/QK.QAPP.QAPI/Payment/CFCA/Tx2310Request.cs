namespace CFCA.Payment.Api
{
    using System;
    using System.Xml;
    using System.Web.Configuration;
    public class Tx2310Request : TxBaseRequest
    {
        private string institutionID;//机构编号
        private string bankID;
        private int accountType;
        private string accountName;
        private string accountNumber;
        private string identificationType;
        private string identificationNumber;
        private string validDate;//信用卡有效期格式：YYMM
        private string cvn2;//信用卡背面末3位数字
        private string note;//备注
        private string phoneNumber;
        private string email;

        private string reqXml; //请求报文
        

        public Tx2310Request()
        {
            // 交易编码
            base.txCode = WebConfigurationManager.AppSettings["TxCode"];
        }

        public string getTxCode()
        {
            return this.txCode;
        }
        public string getAccountName()
        {
            return this.accountName;
        }
        
        public string getAccountNumber()
        {
            return this.accountNumber;
        }
        
        public int getAccountType()
        {
            return this.accountType;
        }
        
        public string getBankID()
        {
            return this.bankID;
        }
        
        public string getCvn2()
        {
            return this.cvn2;
        }
        
        public string getEmail()
        {
            return this.email;
        }
        
        public string getIdentificationNumber()
        {
            return this.identificationNumber;
        }
        
        public string getIdentificationType()
        {
            return this.identificationType;
        }
        
        public string getInstitutionID()
        {
            return this.institutionID;
        }
        
        public string getNote()
        {
            return this.note;
        }
        
        public string getPhoneNumber()
        {
            return this.phoneNumber;
        }

        public string getValidDate()
        {
            return this.validDate;
        }
        public string GetReqXml()
        {
            return this.reqXml;
        }
        public override void process()
        {
            XmlDocument document = new XmlDocument();
            XmlNode newChild = document.CreateNode(XmlNodeType.XmlDeclaration, "", "");
            document.AppendChild(newChild);
            XmlElement element = document.CreateElement("", "Request", "");
            XmlElement element2 = document.CreateElement("Head");
            XmlElement element3 = document.CreateElement("Body");
            XmlElement element4 = document.CreateElement("InstitutionID");
            XmlElement element5 = document.CreateElement("TxCode");
            XmlElement element6 = document.CreateElement("BankID");
            XmlElement element7 = document.CreateElement("AccountType");
            XmlElement element8 = document.CreateElement("ValidDate");
            XmlElement element9 = document.CreateElement("CVN2");
            XmlElement element10 = document.CreateElement("AccountName");
            XmlElement element11 = document.CreateElement("AccountNumber");
            XmlElement element12 = document.CreateElement("IdentificationType");
            XmlElement element13 = document.CreateElement("IdentificationNumber");
            XmlElement element14 = document.CreateElement("Note");
            XmlElement element15 = document.CreateElement("PhoneNumber");
            XmlElement element16 = document.CreateElement("Email");
            element.SetAttribute("version", "", "2.1");
            document.AppendChild(element);
            element.AppendChild(element2);
            element.AppendChild(element3);
            element2.AppendChild(element4);
            element4.AppendChild(document.CreateTextNode(this.institutionID));
            element2.AppendChild(element5);
            element5.AppendChild(document.CreateTextNode(base.txCode));
            element3.AppendChild(element6);
            element6.AppendChild(document.CreateTextNode(this.bankID));
            element3.AppendChild(element7);
            element7.AppendChild(document.CreateTextNode(Convert.ToString(this.accountType)));
            element3.AppendChild(element8);
            element8.AppendChild(document.CreateTextNode(this.validDate));
            element3.AppendChild(element9);
            element9.AppendChild(document.CreateTextNode(this.cvn2));
            element3.AppendChild(element10);
            element10.AppendChild(document.CreateTextNode(this.accountName));
            element3.AppendChild(element11);
            element11.AppendChild(document.CreateTextNode(this.accountNumber));
            element3.AppendChild(element12);
            element12.AppendChild(document.CreateTextNode(this.identificationType));
            element3.AppendChild(element13);
            element13.AppendChild(document.CreateTextNode(this.identificationNumber));
            element3.AppendChild(element14);
            element14.AppendChild(document.CreateTextNode(this.note));
            element3.AppendChild(element15);
            element15.AppendChild(document.CreateTextNode(this.phoneNumber));
            element3.AppendChild(element16);
            element16.AppendChild(document.CreateTextNode(this.email));
            base.postProcess(document);
            this.reqXml = document.InnerXml;
        }

        public void setTxCode(string txCode)
        {
            this.txCode = txCode;
        }
        public void setAccountName(string accountName)
        {
            this.accountName = accountName;
        }
        
        public void setAccountNumber(string accountNumber)
        {
            this.accountNumber = accountNumber;
        }
        
        public void setAccountType(int accountType)
        {
            this.accountType = accountType;
        }
        
        public void setBankID(string bankID)
        {
            this.bankID = bankID;
        }
        
        public void setCvn2(string cvn2)
        {
            this.cvn2 = cvn2;
        }
        
        public void setEmail(string email)
        {
            this.email = email;
        }
        
        public void setIdentificationNumber(string identificationNumber)
        {
            this.identificationNumber = identificationNumber;
        }
        
        public void setIdentificationType(string identificationType)
        {
            this.identificationType = identificationType;
        }
        
        public void setInstitutionID(string institutionID)
        {
            this.institutionID = institutionID;
        }
        
        public void setNote(string note)
        {
            this.note = note;
        }
        
        public void setPhoneNumber(string phoneNumber)
        {
            this.phoneNumber = phoneNumber;
        }
        public void setValidDate(string validDate)
        {
            this.validDate = validDate;
        }
    }
}