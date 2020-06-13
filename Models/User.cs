﻿using System.ComponentModel.DataAnnotations;

namespace netcore3_api_basicproject.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        [MaxLength(20, ErrorMessage = "Deve conter entre 3 e 20 caracteres")]
        [MinLength(3, ErrorMessage = "Deve conter no minimo 3 caracteres")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        [MaxLength(20, ErrorMessage = "Deve conter entre 3 e 20 caracteres")]
        [MinLength(3, ErrorMessage = "Deve conter no minimo 3 caracteres")]
        public string Password { get; set; }

        public string Role { get; set; }
    }
}
