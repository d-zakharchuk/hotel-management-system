//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HotelDataEF
{
    using System;
    using System.Collections.Generic;
    
    public partial class Reservation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Reservation()
        {
            this.Res_serv = new HashSet<Res_serv>();
        }
    
        public string Res_id { get; set; }
        public string Client_id { get; set; }
        public int Room_num { get; set; }
        public System.DateTime Date_in { get; set; }
        public string Services { get; set; }
    
        public virtual Client Client { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Res_serv> Res_serv { get; set; }
        public virtual Room Room { get; set; }
    }
}
