using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BackEnd.Models
{
    [Serializable]
    public class Subforum
    {
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [StringLength(2048)]
        public string Description { get; set; }

        [Required]
        [StringLength(2048)]
        public string IconURL { get; set; }

        [StringLength(2048)]
        public string Rules { get; set; }

        [Required]
        public string LeadModeratorUsername { get; set; }

    }
}