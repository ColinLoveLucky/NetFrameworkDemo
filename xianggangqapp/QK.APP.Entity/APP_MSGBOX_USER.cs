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
    
    public partial class APP_MSGBOX_USER : BasicEntity
    {
         public APP_MSGBOX_USER(): base(false){ }
         public APP_MSGBOX_USER(bool isGenerated = false) : base(isGenerated) { }
            
    	 [Sequence("SEQ_APP_MSGBOX_USER")]
    		
         public long ID { get; set; }     
            
    		
         public string CONNECTIONID { get; set; }     
            
    		
         public string USERNAME { get; set; }     
            
    		
         public Nullable<System.DateTime> CREATETIME { get; set; }     
            
    		
         public string USERIP { get; set; }     
            
    		
         public string USERBROWSER { get; set; }     
            
    		
         public string USERBROWSERVERSION { get; set; }     
            
    		
         public Nullable<System.DateTime> LASTUPDATETIME { get; set; }     
            
    		
         public Nullable<decimal> ENABLE { get; set; }     
            
    		
         public string MACHINENAME { get; set; }     
        
    }
}
