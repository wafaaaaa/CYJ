//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CYJ.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class GOAL
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GOAL()
        {
            this.CONNECTIONs = new HashSet<CONNECTION>();
        }
    
        public int goalID { get; set; }
        public string goalName { get; set; }
        public Nullable<int> goalNum { get; set; }
        public Nullable<System.DateTime> goalDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CONNECTION> CONNECTIONs { get; set; }
    }
}
