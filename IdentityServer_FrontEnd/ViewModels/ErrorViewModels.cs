﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace IdentityServer_FrontEnd.ViewModels
{
    public class ErrorViewModel
    {
        public IEnumerable<ModelError>? UserNameErrors { get; set; }

        public IEnumerable<ModelError>? PasswordErrors { get; set; }

        public IEnumerable<ModelError>? ConfirmPasswordErrors { get; set; }
    }
}