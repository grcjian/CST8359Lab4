﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Lab4.Models
{
    public class Community
    {
        /*
         * Community Id that is not database generated by setting the DatabaseGenerated attribute to DatabaseGeneratedOption.None
         * and Display name should read ‘Registration Number’ with Required attribute
         */
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Registration Number")]
        [Required]
        public string Id { get; set; }

        /*
         * title of the Community with ‘Required’ attribute, ‘StringLength’ attribute, with a value of max 50 and MinimumLength = 3
         */
        [StringLength(50, MinimumLength = 3)]
        [Display(Name = "Title")]
        [Required]
        public string Title { get; set; }

        /* 
         * decimal budget of Community with attributes to define data type currency and column type money
         */
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Budget { get; set; }

        public ICollection<CommunityMembership> CommunityMemberships { get; set; }
    }
}
