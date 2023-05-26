using AutoMapper;
using AutoMapper.Internal;
using Anil.Core.Configuration;
using Anil.Core.Infrastructure.Mapper;
using Anil.Data.Configuration;
using Anil.Web.Framework.Models;
namespace Anil.Web.Areas.Admin.Infrastructure.Mapper
{
    /// <summary>
    /// AutoMapper configuration for admin area models
    /// </summary>
    public partial class ApiMapperConfiguration : Profile, IOrderedMapperProfile
    {
        #region Ctor

        public ApiMapperConfiguration()
        {
            //create specific maps
            CreateCustomersMaps();
            

            //add some generic mapping rules
            this.Internal().ForAllMaps((mapConfiguration, map) =>
            {
                //exclude Form and CustomProperties from mapping BaseAnilModel
                if (typeof(BaseAnilModel).IsAssignableFrom(mapConfiguration.DestinationType))
                {
                    //map.ForMember(nameof(BaseAnilModel.Form), options => options.Ignore());
                    map.ForMember(nameof(BaseAnilModel.CustomProperties), options => options.Ignore());
                }

                //exclude ActiveStoreScopeConfiguration from mapping ISettingsModel
                if (typeof(ISettingsModel).IsAssignableFrom(mapConfiguration.DestinationType))
                    map.ForMember(nameof(ISettingsModel.ActiveStoreScopeConfiguration), options => options.Ignore());

                //exclude some properties from mapping configuration and models
                if (typeof(IConfig).IsAssignableFrom(mapConfiguration.DestinationType))
                    map.ForMember(nameof(IConfig.Name), options => options.Ignore());

            });
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Create customers maps 
        /// </summary>
        protected virtual void CreateCustomersMaps()
        {
            //CreateMap<ActivityLog, CustomerActivityLogModel>()
            //   .ForMember(model => model.CreatedOn, options => options.Ignore())
            //   .ForMember(model => model.ActivityLogTypeName, options => options.Ignore());


            #region CodeGen
			
            #endregion
        }

        #endregion

        #region Properties

        /// <summary>
        /// Order of this mapper implementation
        /// </summary>
        public int Order => 0;

        #endregion
    }
}