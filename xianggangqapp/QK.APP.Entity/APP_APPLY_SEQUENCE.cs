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
    
    public partial class APP_APPLY_SEQUENCE : BasicEntity
    {
         public APP_APPLY_SEQUENCE(): base(false){ }
         public APP_APPLY_SEQUENCE(bool isGenerated = false) : base(isGenerated) { }
            
    	 [Sequence("SEQ_APP_APPLY_SEQUENCE")]
    		
         public long ID { get; set; }     
            
    		
         public string SEQ_CODE { get; set; }     
            
    		
         public string DESCR { get; set; }     
            
    		
         public long CURRENT_SEQVALUE { get; set; }     
            
    		
         public Nullable<System.DateTime> CREATE_TIME { get; set; }     
            
    		
         public Nullable<System.DateTime> EDIT_TIME { get; set; }     
            
    		
         public string DATA_FORMAT { get; set; }     
            
    		
         public long LENGTH { get; set; }     
            
    		
         public long INIT_VALUE { get; set; }     
            
    		
         public string RESET_TYPE { get; set; }     
            
    		
         public string LAST_DATE { get; set; }     
            
    		
         public string IS_RUNNING { get; set; }     
            
    		
         public string PREFIX { get; set; }     
            
    		
         public string OPE_TYPE { get; set; }     
            
    		
         public string CITY_CODE { get; set; }     
        
    }
}
