﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eKarton.Model.Request
{
    public class KorisnikUpdateRequest
    {
        [Required(AllowEmptyStrings = false)]
        public string Ime { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Prezime { get; set; }
        [EmailAddress]
        [Required]
        [MinLength(4)]
        public string Email { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Telefon { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string KorisnickoIme { get; set; }
        public string Password { get; set; }
        public string PasswordPotvrda { get; set; }
        public List<int> Uloge { get; set; } = new List<int>();
    }
}
