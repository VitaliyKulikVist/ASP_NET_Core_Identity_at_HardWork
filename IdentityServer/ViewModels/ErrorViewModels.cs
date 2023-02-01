using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace IdentityServer.ViewModels
{
    public class ErrorViewModel
    {
        public IEnumerable<ModelError>? UserNameErrors { get; set; }

        public IEnumerable<ModelError>? PasswordErrors { get; set; }

        public IEnumerable<ModelError>? ConfirmPasswordErrors { get; set; }

        public IEnumerable<IdentityError>? IdentityError { get; set; }

        public bool DelateAllUsersDone { get; set; } = false;
        public bool CreateDefaultUsersDone { get; set; } = false;
    }
}
