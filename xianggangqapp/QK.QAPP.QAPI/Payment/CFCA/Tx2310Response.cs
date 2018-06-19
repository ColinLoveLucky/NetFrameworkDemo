namespace CFCA.Payment.Api
{
    using System;
    using System.Xml;
    /// <summary>
    /// Tx2310Response 的摘要说明
    /// </summary>
    public class Tx2310Response : TxBaseResponse
    { 
        private string institutionID;
        private int status;

        public Tx2310Response(string responseMessage, string responseSignature)
            : base(responseMessage, responseSignature)
        {
        }
      
        public string getInstitutionID()
        {
            return this.institutionID;
        }
        
        public int getStatus()
        {
            return this.status;
        }
       
        protected override void process(XmlDocument document)
        {
            //if ("2000".Equals(base.code))
            //{
                this.institutionID = XmlUtil.getNodeText(document, "InstitutionID");
                this.status = Convert.ToInt32(XmlUtil.getNodeText(document, "Status"));
           // }
        }
    }
}