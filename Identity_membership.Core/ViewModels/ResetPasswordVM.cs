﻿using System.ComponentModel.DataAnnotations;

namespace Identity_membership.Core.ViewModels
{
    public class ResetPasswordVM
    {

        public ResetPasswordVM() { }

        public ResetPasswordVM(string password, string passwordConfirm)
        {
            Password = password;
            PasswordConfirm = passwordConfirm;
        }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz.")]
        [Display(Name = "Yeni Şifre :")]
        [MinLength(6, ErrorMessage = "Şifreniz en az 6 karakter olabilir")]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Şifre aynı değildir.")]
        [Required(ErrorMessage = "Şifre tekrar alanı boş bırakılamaz")]
        [Display(Name = "Yeni Şifre Tekrar :")]
        [MinLength(6, ErrorMessage = "Şifreniz en az 6 karakter olabilir")]
        public string PasswordConfirm { get; set; } = null!; 

    }
}
