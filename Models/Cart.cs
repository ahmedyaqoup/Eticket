﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eticket.Models
{
    [PrimaryKey("ApplicationUserId","MovieId")]
    public class Cart
    {
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }

        public int MovieId { get; set; }
        [ForeignKey("MovieId")]
        public Movie Movie { get; set; }
        public int Count { get; set; }
        public DateTime Time { get; set; }
    }
}
