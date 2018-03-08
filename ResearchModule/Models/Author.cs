﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Models
{
    public partial class Author
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Фамилия")]
        public string Surname { get; set; }

        [DisplayName("Имя")]
        public string Name { get; set; }

        [DisplayName("Отчество")]
        public string Lastname { get; set; }

        [DisplayName("День рождения")]
        public DateTime BDay { get; set; }

        [NotMapped]
        public bool Selected { get; set; }

        [NotMapped]
        [DisplayName("Вес")]
        public double Weight { get; set; }

        public ICollection<PA> PAs { get; set; }

        public string UserId { get; set; }

        public virtual User User { get; set; }

        public bool IsValid()
        {
            if (!(string.IsNullOrEmpty(Surname) && string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(Lastname)))
                return true;
            return false;
        }
    }
}
