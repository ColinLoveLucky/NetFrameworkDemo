//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QK.QAPP.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class APP_EXTEND_CONFIG : BasicEntity
    {
         public APP_EXTEND_CONFIG(): base(false){ }
         public APP_EXTEND_CONFIG(bool isGenerated = false) : base(isGenerated) { }
            
    	 [Sequence("SEQ_APP_EXTEND_CONFIG")]
    		
         public long ID { get; set; }     
            
    		
         public string ACTION_GROUP { get; set; }     
            
    		
         public string PRODUCT_CODE { get; set; }     
            
    		
         public string CITY_CODE { get; set; }     
            
    		
         public Nullable<int> PERIOD_AMOUNT_TOTAL { get; set; }     
            
    		
         public Nullable<int> PERIOD_AMOUNT { get; set; }     
            
    		
         public string SETTLEMENT_TYPE { get; set; }     
            
    		
         public string SETTLEMENT_AMOUNT { get; set; }     
            
    		
         public string SERVICE_CHARGE_TYPE { get; set; }     
            
    		
         public string SERVICE_CHARGE { get; set; }     
            
    		
         public string CONSULT_CHARGE_TYPE { get; set; }     
            
    		
         public string CONSULT_CHARGE { get; set; }     
            
    		
         public string TARGET_PRODUCT_CODE { get; set; }     
            
    		
         public string TARGET_DFORM_VERSION { get; set; }     
            
    		
         public string TARGET_LOGO { get; set; }     
        
    }
}
