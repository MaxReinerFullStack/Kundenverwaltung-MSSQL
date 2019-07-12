using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    [Table("Kunden")]
    public class Customer
    {
        public Customer() {
            User = new User();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [Column(TypeName = "nvarchar")]

        [Required]
        [DataType(DataType.Text, ErrorMessage = "Keine gültige Kundenbezeichnung")]
        public string Kundenbezeichnung { get; set; }
        public string Ort { get; set; }
        public string Bundesland { get; set; }
        public string Adresse { get; set; }

        [DataType(DataType.PhoneNumber, ErrorMessage = "Keine gültige Telephonnummer")]
        [Required]
        public string Telefonnummer1 { get; set; }


        public string Telefonnummer2 { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessage = "Keine gültige E-Mail Adresse")]
        public string Email { get; set; }


        public Status In_bearbeitung { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "Kein gültiges Datum")]
        public DateTime Änderungsdatum { get; set; }
        public Status Unterlagen_gesendet { get; set; }
        public Status Angebot_geschickt { get; set; }
        public Status Interesse_kooperationsvertrag { get; set; }
        public Status Abgeschlossen { get; set; }
        public string Notizen { get; set; }
        public int Angebotsnummer { get; set; }
        [StringLength(500)]
        [Column(TypeName = "nvarchar")]

        public string Abklärung { get; set; }


        [Required]

        public int UserId { get; set; }
        public string Contactperson_Firstname { get; set; }
        public string Contactperson_Lastname { get; set; }
        public string Contactperson_Title { get; set; }

        public static Customer getSampleCustomer()
        {
            Customer customer = new Customer()
            {
                Id = 1,
                Kundenbezeichnung = "Beispielkunde",
                Ort = "Beispielort",
                Bundesland = "",
                Adresse = "",
                Telefonnummer1 = "+43 666 113377",
                Telefonnummer2 = "",
                Email = "a.b@c.com",
                Contactperson_Title = "Herr",
                Contactperson_Firstname = "Josef",
                Contactperson_Lastname = "Maier",

                In_bearbeitung = Status.In_Arbeit,
                User = new User() { Id = 1, Firstname = "Markus", Lastname = "Müller" },
                Änderungsdatum = new DateTime(2017, 10, 19),
                Unterlagen_gesendet = Status.Erledigt,
                Angebot_geschickt = Status.Erledigt,
                Interesse_kooperationsvertrag = Status.Erledigt,
                Abgeschlossen = Status.In_Arbeit,
                Notizen = "Anrufen wegen Termin für Besichtigung.",
                Angebotsnummer = 1122,
                Abklärung = ""
            };
            return customer;

        }
    }
}
