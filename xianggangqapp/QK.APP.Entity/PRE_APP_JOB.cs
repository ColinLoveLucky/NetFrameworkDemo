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
    
    public partial class PRE_APP_JOB : BasicEntity
    {
         public PRE_APP_JOB(): base(false){ }
         public PRE_APP_JOB(bool isGenerated = false) : base(isGenerated) { }
            
    	 [Sequence("SEQ_PRE_APP_JOB")]
    		
         public long ID { get; set; }     
            
    		
         public long APP_ID { get; set; }     
            
    		
         public string COM_NAME { get; set; }     
            
    		
         public string COM_TEL_NO { get; set; }     
            
    		
         public Nullable<System.DateTime> DATE_JOIN { get; set; }     
            
    		
         public string STATION2_COM { get; set; }     
            
    		
         public Nullable<decimal> SHARE_RATIO { get; set; }     
            
    		
         public string STATION2_COM_OTHER { get; set; }     
            
    		
         public string CREATED_USER { get; set; }     
            
    		
         public Nullable<System.DateTime> CREATED_TIME { get; set; }     
            
    		
         public string CHANGED_USER { get; set; }     
            
    		
         public Nullable<System.DateTime> CHANGED_TIME { get; set; }     
        public virtual PRE_APP_MAIN PRE_APP_MAIN { get; set; }
        
    }
}
