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
    
    public partial class FL_LIST : BasicEntity
    {
         public FL_LIST(): base(false){ }
         public FL_LIST(bool isGenerated = false) : base(isGenerated) { }
            
    	 [Sequence("SEQ_FL_LIST")]
    		
         public long ID { get; set; }     
            
    		
         public Nullable<long> FL_ID { get; set; }     
            
    		
         public string FL_TYPE { get; set; }     
            
    		
         public Nullable<long> FL_SIZE { get; set; }     
            
    		
         public Nullable<long> FL_WIDTH { get; set; }     
            
    		
         public Nullable<long> FL_HEIGHTH { get; set; }     
            
    		
         public string CREATED_USER { get; set; }     
            
    		
         public Nullable<System.DateTime> CREATED_TIME { get; set; }     
            
    		
         public string CHANGED_USER { get; set; }     
            
    		
         public Nullable<System.DateTime> CHANGED_TIME { get; set; }     
            
    		
         public string FL_PATH { get; set; }     
            
    		
         public string FL_MEMO { get; set; }     
            
    		
         public string FL_NAME { get; set; }     
            
    		
         public Nullable<int> FL_SEQ { get; set; }     
            
    		
         public string STATUS { get; set; }     
            
    		
         public string SL_FL_PATH { get; set; }     
        
    }
}
