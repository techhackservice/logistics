using System;
using AutoMapper;
using Web.Api.Core.Dto.UseCaseRequests;

namespace Web.Api.Models.Settings
{
    public class MapperSettings :Profile
    {
        public MapperSettings()
        {
            CreateMap<Models.Request.LoginRequest, LoginRequest>();
            CreateMap<Models.Request.CompanyRequest, CompanyRequest>();
            CreateMap<Models.Request.CompanyBranchRequest, CompanyBranchRequest>();
            CreateMap<Models.Request.ContactDetailsRequest, ContactDetailsRequest>();
            CreateMap<Models.Request.AddressDetailsRequest, AddressDetailsRequest>();
            CreateMap<Models.Request.ConvinienceFeesDetails, ConvinienceFeesDetails>();
            CreateMap<Models.Request.InvoiceRequest, InvoiceRequest>();
            CreateMap<Models.Request.OrderDetails, OrderDetails>();
            CreateMap<Models.Request.ProductDetails, ProductDetails>();
            CreateMap<Models.Request.CustomerDetails, CustomerDetails>();
            CreateMap<Models.Request.AddressDetails, AddressDetails>();
            CreateMap<Models.Request.DeleteRequest, DeleteRequest>();
        }
    }
}