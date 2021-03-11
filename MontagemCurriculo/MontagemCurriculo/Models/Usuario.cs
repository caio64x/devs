﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace MontagemCurriculo.Models
{
    public class Usuario
    {   
        public int UsuaioID { get; set; }
        [Required(ErrorMessage = "Campo obrigtório")]
        [StringLength(50, ErrorMessage = "Use menos caracteres")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo obrigtório")]
        [StringLength(50, ErrorMessage = "Use menos caracteres")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        public ICollection<InformacaoLogin>InformacoesLogin { get; set; }
        public ICollection<Curriculo> Curriculos { get; set; }
    }
}
