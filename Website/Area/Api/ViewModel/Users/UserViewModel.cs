using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Anil.Web.Framework.Mvc.ModelBinding;
using Anil.Web.Framework.Models;

namespace Anil.Web.Areas.Admin.Models.Users
{
    /// <summary>
    /// Represents a customer model
    /// </summary>
    public partial record UserViewModel : BaseAnilEntityModel, IAclSupportedModel
    {
        #region Ctor

        public UserViewModel()
        {

            SelectedCustomerRoleIds = new List<int>();
            AvailableCustomerRoles = new List<SelectListItem>();

            AvailableCountries = new List<SelectListItem>();
            AvailableStates = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        public bool UsernamesEnabled { get; set; }

        [AnilResourceDisplayName("Api.Users.Users.Fields.Username")]
        public string Username { get; set; }

        [DataType(DataType.EmailAddress)]
        [AnilResourceDisplayName("Api.Users.Users.Fields.Email")]
        public string Email { get; set; }

        [AnilResourceDisplayName("Api.Users.Users.Fields.Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [AnilResourceDisplayName("Api.Users.Users.Fields.Gender")]
        public string Gender { get; set; }

        [AnilResourceDisplayName("Api.Users.Users.Fields.FirstName")]
        public string FirstName { get; set; }

        [AnilResourceDisplayName("Api.Users.Users.Fields.LastName")]
        public string LastName { get; set; }

        [AnilResourceDisplayName("Api.Users.Users.Fields.FullName")]
        public string FullName { get; set; }

        [UIHint("DateNullable")]
        [AnilResourceDisplayName("Api.Users.Users.Fields.DateOfBirth")]
        public DateTime? DateOfBirth { get; set; }

        [AnilResourceDisplayName("Api.Users.Users.Fields.Company")]
        public string Company { get; set; }

        [AnilResourceDisplayName("Api.Users.Users.Fields.StreetAddress")]
        public string StreetAddress { get; set; }

        [AnilResourceDisplayName("Api.Users.Users.Fields.StreetAddress2")]
        public string StreetAddress2 { get; set; }

        [AnilResourceDisplayName("Api.Users.Users.Fields.ZipPostalCode")]
        public string ZipPostalCode { get; set; }

        [AnilResourceDisplayName("Api.Users.Users.Fields.City")]
        public string City { get; set; }

        [AnilResourceDisplayName("Api.Users.Users.Fields.County")]
        public string County { get; set; }

        [AnilResourceDisplayName("Api.Users.Users.Fields.Country")]
        public int CountryId { get; set; }

        public IList<SelectListItem> AvailableCountries { get; set; }

        [AnilResourceDisplayName("Api.Users.Users.Fields.StateProvince")]
        public int StateProvinceId { get; set; }

        public IList<SelectListItem> AvailableStates { get; set; }

        [DataType(DataType.PhoneNumber)]
        [AnilResourceDisplayName("Api.Users.Users.Fields.Phone")]
        public string Phone { get; set; }

        [AnilResourceDisplayName("Api.Users.Users.Fields.AdminComment")]
        public string AdminComment { get; set; }

        [AnilResourceDisplayName("Api.Users.Users.Fields.Active")]
        public bool Active { get; set; }

        //registration date
        [AnilResourceDisplayName("Api.Users.Users.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        [AnilResourceDisplayName("Api.Users.Users.Fields.LastActivityDate")]
        public DateTime LastActivityDate { get; set; }

        //IP address
        [AnilResourceDisplayName("Api.Users.Users.Fields.IPAddress")]
        public string LastIpAddress { get; set; }

        [AnilResourceDisplayName("Api.Users.Users.Fields.LastVisitedPage")]
        public string LastVisitedPage { get; set; }
        
        public string AvatarUrl { get; internal set; }

        public IList<SelectListItem> AvailableCustomerRoles { get; set; }

        [AnilResourceDisplayName("Api.Users.Users.Fields.CustomerRoles")]
        public IList<int> SelectedCustomerRoleIds { get; set; }

        #endregion

        #region Nested classes

        public partial record SendEmailModel : BaseAnilModel
        {
            [AnilResourceDisplayName("Api.Users.Users.SendEmail.Subject")]
            public string Subject { get; set; }

            [AnilResourceDisplayName("Api.Users.Users.SendEmail.Body")]
            public string Body { get; set; }

            [AnilResourceDisplayName("Api.Users.Users.SendEmail.SendImmediately")]
            public bool SendImmediately { get; set; }

            [AnilResourceDisplayName("Api.Users.Users.SendEmail.DontSendBeforeDate")]
            [UIHint("DateTimeNullable")]
            public DateTime? DontSendBeforeDate { get; set; }
        }

        public partial record SendPmModel : BaseAnilModel
        {
            [AnilResourceDisplayName("Api.Users.Users.SendPM.Subject")]
            public string Subject { get; set; }

            [AnilResourceDisplayName("Api.Users.Users.SendPM.Message")]
            public string Message { get; set; }
        }

        public partial record CustomerAttributeModel : BaseAnilEntityModel
        {
            public CustomerAttributeModel()
            {
                Values = new List<CustomerAttributeValueModel>();
            }

            public string Name { get; set; }

            public bool IsRequired { get; set; }

            /// <summary>
            /// Default value for textboxes
            /// </summary>
            public string DefaultValue { get; set; }

            public IList<CustomerAttributeValueModel> Values { get; set; }
        }

        public partial record CustomerAttributeValueModel : BaseAnilEntityModel
        {
            public string Name { get; set; }

            public bool IsPreSelected { get; set; }
        }

        #endregion
    }
}