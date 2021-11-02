using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab4.Models
{
    public class Student
    {
        /*
         * public integer Id for students
         */
        public int Id { get; set; }

        /*
         * Last name of a student, required, with strengthlength 50 and minimum length 1
         */
        [StringLength(50, MinimumLength = 1)]
        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }

        /*
         * First name of a student, required, with strengthlength 50 and minimum length
         */
        [StringLength(50, MinimumLength = 1)]
        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }

        /*
         * DateTime EnrollmentDate
         */
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Enrollment Date")]
        public DateTime EnrollmentDate { get; set; }

        /*
         * calculated field from: LastName + “, “ + FirstName
         */
        public string FullName
        {
            get
            {
                return LastName + ", " + FirstName;
            }
        }

        public ICollection<CommunityMembership> CommunityMemberships { get; set; }
    }
}
