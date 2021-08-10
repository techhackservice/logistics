using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Dto.UseCaseRequests;
using Dapper;
using Web.Api.Core.Enums;
using Newtonsoft.Json;
using Web.Api.Core.Interfaces.Services;
using Web.Api.Core.Dto.UseCaseResponses;

namespace Web.Api.Infrastructure.Data.Repositories
{
    internal sealed class InvoiceRepository : IInvoiceRepository
    {
        private ICompanyRepository _companyRepository;
        private IOrderRepository _orderRepository;
        private IProductRepository _productRepository;
        private readonly AppDbContext _appDbContext;
        private readonly IHttpClientService _httpClientService;
        public InvoiceRepository(ICompanyRepository companyRepository, IOrderRepository orderRepository, AppDbContext appDbContext, IHttpClientService httpClientService)
        {
            _companyRepository = companyRepository;
            _orderRepository = orderRepository;
            _appDbContext = appDbContext;
            _httpClientService = httpClientService;
        }
        public async Task<List<InvoiceDetails>> GetInvoiceDetails(string companyId, string invoiceId, string invoiceType, string invoiceStatus, string filterStatus)
        {
            List<InvoiceDetails> retInvoiceDetailsList = new List<InvoiceDetails>();

            try
            {
                var tableName = $"Logisticmate.invoice_obj io, " + 
                                $"Logisticmate.tracking_obj t ";

                var ColumAssign = $"io.invoice_id as InvoiceId, io.order_id as OrderId, " + 
                                  $"io.company_id as CompanyId, " +
                                  $"io.invoice_type as InvoiceType, io.invoice_status as InvoiceStatus, " +
                                  $"io.convinience_fees_type as ConvinienceFeesType, " +
                                  $"io.convinience_fees_amt as ConvinienceFeesAmount, " +
                                  $"io.invoice_amt as InvoiceAmount, " +

                                  $"t.tracking_id as TrackingId, t.tracking_status as TrackingStatus, " +
                                  $"io.created_by as CreatedBy, io.modified_by as ModifiedBy";

                var selQuery = $"select " + ColumAssign + " from " + tableName;

                var whereCondi = $"where io.order_id = t.order_id";

                if(!string.IsNullOrEmpty(companyId))
                    whereCondi += " and io.company_id = '" + companyId + "'";

                if(!string.IsNullOrEmpty(invoiceId))
                    whereCondi += " and io.invoice_id = '" + invoiceId + "'";

                if(!string.IsNullOrEmpty(invoiceType))
                    whereCondi += " and io.invoice_type = '" + invoiceType + "'";

                if(!string.IsNullOrEmpty(invoiceStatus))
                    whereCondi += " and io.invoice_status = '" + invoiceStatus + "'";

                if(filterStatus != "all")
                {
                    if(!string.IsNullOrEmpty(filterStatus))
                        whereCondi += " and t.tracking_status = '" + filterStatus + "'";
                }

                var orderCond = $" order by io.created_by ASC ";

                var sqlSelQuery = selQuery + whereCondi + orderCond;
                using (var connection = _appDbContext.Connection)
                {
                    var sqlResult = await connection.QueryAsync<InvoiceDetails>(sqlSelQuery);
                    foreach(InvoiceDetails singleInvoiceDetails in sqlResult.ToList())
                    {
                        singleInvoiceDetails.OrderDetails = new OrderDetails();
                        List<OrderDetails> retOrderDet = await _orderRepository.GetOrderDetails(singleInvoiceDetails.CompanyId, singleInvoiceDetails.OrderId);
                        if(retOrderDet.Count > 0)
                            singleInvoiceDetails.OrderDetails = retOrderDet[0];

                        retInvoiceDetailsList.Add(singleInvoiceDetails);
                    }
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
            }
            return retInvoiceDetailsList;
        }
/*
        public async Task<List<InvoiceDetails>> GetInvoiceDetails(string companyId, string invoiceId, string invoiceType, string invoiceStatus, string filterStatus)
        {
            List<InvoiceDetails> retInvoiceDetailsList = new List<InvoiceDetails>();

            try
            {
                var tableName = $"Logisticmate.invoice_obj io, " + 
                                $"Logisticmate.order_obj oo, " +
                                $"Logisticmate.customer_obj co, " +
                                $"Logisticmate.address_obj ao, " +
                                $"Logisticmate.tracking_obj t ";//, " +
                                //$"Logisticmate.product_obj po ";

                var ColumAssign = $"io.invoice_id as InvoiceId, io.order_id as OrderId, " + 
                                  $"io.invoice_type as InvoiceType, io.invoice_status as InvoiceStatus, " +
                                  $"io.convinience_fees_type as ConvinienceFeesType, " +
                                  $"io.convinience_fees_amt as ConvinienceFeesAmount, " +
                                  $"io.invoice_amt as InvoiceAmount, " +

                                  $"oo.order_name as OrderName, oo.order_date as OrderDate, " +
                                  $"oo.expected_date as ExpectedDate, oo.delivery_date as DeliveredDate, " +
                                  $"oo.order_status as OrderStatus, oo.product_id as ProductId, " +
                                  $"oo.customer_id as CustomerId, co.customer_first_name as CustomerFirstName, " +

                                  $"co.customer_middle_name as CustomerMiddleName, co.customer_last_name as CustomerLastName, " +
                                  $"co.shipping_address_id as CustomerAddressId, ao.email as CustomerEmail, " +

                                  $"ao.std_code_landline_number as STDCodeLandlineNumber, ao.landline_number as CustomerLandlineNumber, " +
                                  $"ao.std_code_mobile_number as STDCodeMobileNumber, ao.mobile_number as CustomerMobileNumber, " +

                                  $"t.tracking_id as TrackingId, t.tracking_status as TrackingStatus, " +
                                  $"io.created_by as CreatedBy, io.modified_by as ModifiedBy";

                var selQuery = $"select " + ColumAssign + " from " + tableName;

                var whereCondi = $"io.order_id = oo.order_id" +
                                 $" and oo.order_id = t.order_id" +
                                 $" and oo.customer_id = co.customer_id" +
                                 $" and co.shipping_address_id = ao.address_id" +
                                 $" and co.status = 'Active' and ao.status = 'Active'";

                if(!string.IsNullOrEmpty(companyId) && String.IsNullOrEmpty(whereCondi))
                    whereCondi += "oo.company_id = '" + companyId + "'";
                else if(!string.IsNullOrEmpty(companyId) && !String.IsNullOrEmpty(whereCondi))
                    whereCondi += " and oo.company_id = '" + companyId + "'";

                if(!string.IsNullOrEmpty(invoiceId) && String.IsNullOrEmpty(whereCondi))
                    whereCondi += "io.invoice_id = '" + invoiceId + "'";
                else if(!string.IsNullOrEmpty(invoiceId) && !String.IsNullOrEmpty(whereCondi))
                    whereCondi += " and io.invoice_id = '" + invoiceId + "'";

                if(!string.IsNullOrEmpty(invoiceType) && String.IsNullOrEmpty(whereCondi))
                    whereCondi += "io.invoice_type = '" + invoiceType + "'";
                else if(!string.IsNullOrEmpty(invoiceType) && !String.IsNullOrEmpty(whereCondi))
                    whereCondi += " and io.invoice_type = '" + invoiceType + "'";

                if(!string.IsNullOrEmpty(invoiceStatus) && String.IsNullOrEmpty(whereCondi))
                    whereCondi += "io.invoice_status = '" + invoiceStatus + "'";
                else if(!string.IsNullOrEmpty(invoiceStatus) && !String.IsNullOrEmpty(whereCondi))
                    whereCondi += " and io.invoice_status = '" + invoiceStatus + "'";

                if(filterStatus != "all")
                {
                    if(!string.IsNullOrEmpty(filterStatus) && String.IsNullOrEmpty(whereCondi))
                        whereCondi += "t.tracking_status = '" + filterStatus + "'";
                    else if(!string.IsNullOrEmpty(filterStatus) && !String.IsNullOrEmpty(whereCondi))
                        whereCondi += " and t.tracking_status = '" + filterStatus + "'";
                }

                if(!string.IsNullOrEmpty(whereCondi))
                    whereCondi = "where " + whereCondi;

                var orderCond = $" order by io.invoice_id ASC ";

                var sqlSelQuery = selQuery + whereCondi + orderCond;
                using (var connection = _appDbContext.Connection)
                {
                    var sqlResult = await connection.QueryAsync<InvoiceDetails>(sqlSelQuery);
                    foreach(InvoiceDetails singleInvoiceDetails in sqlResult.ToList())
                    {
                        singleInvoiceDetails.ProductDetailsList = await _productRepository.GetOrderProductDetails(singleInvoiceDetails.OrderId);
                        if(singleInvoiceDetails.ProductDetailsList == null)
                            singleInvoiceDetails.ProductDetailsList = new List<ProductDetails>();

                        retInvoiceDetailsList.Add(singleInvoiceDetails);
                    }
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
            }
            return retInvoiceDetailsList;
        }
*/
        public async Task<bool> CreateInvoice(string companyId, string orderId, List<ProductDetails> productDetailsList, string createdBy)
        {
            var sqlResult = true;
            var invoiceType = "receivable";
            var invoiceStatus = "open";
            decimal invoiceTotalAmt = 0;
            decimal productTotalAmt = 0;
            decimal gstAmt = 0;

            var invoiceId = await GenerateInvoiceNo(companyId, invoiceType);

            ConvinienceFeesDetails convinienceFeesDetails = await _companyRepository.GetConvinienceFeesDetails(companyId);
            if(convinienceFeesDetails == null)
                convinienceFeesDetails = new ConvinienceFeesDetails();

            //calculate product list total amount
            string calculatedProductAmt = CalculateProductTotalAmt(productDetailsList);
            if(!String.IsNullOrEmpty(calculatedProductAmt))
            {
                string[] splitVal = calculatedProductAmt.Split(',');
                if(splitVal.Count() == 2)
                {
                    productTotalAmt = Convert.ToDecimal(splitVal[0]);
                    gstAmt = Convert.ToDecimal(splitVal[1]);
                }
            }

            //calculate the Convinience Amount
            decimal conveineAmt = 0;
            if(!String.IsNullOrEmpty(convinienceFeesDetails.FeesType))
            {
                if(convinienceFeesDetails.FeesType.Equals("fixed"))
                    conveineAmt = convinienceFeesDetails.FeesAmount;
                else
                {
                    conveineAmt = (productTotalAmt * convinienceFeesDetails.FeesAmount) / 100;
                }
            }

            //Calculate GST Amount//gst_percentage
            //gstAmt = ((productTotalAmt + conveineAmt) x GST%)/100;
            //decimal NetPrice = productTotalAmt  + gstAmt;

            invoiceTotalAmt = productTotalAmt + conveineAmt + gstAmt;

            try
            {
                var tableName = $"Logisticmate.invoice_obj";

                var colName = $"invoice_id, order_id, company_id, " +
                              $"invoice_type, invoice_status, " +
                              $"convinience_fees_type, convinience_fees_amt, " +
                              $"invoice_amt, " +
                              $"created_by, created_on";

                var colValueName = $"@InvoiceId, @OrderId, @CompanyId, " +
                                   $"@InvoiceType, @InvoiceStatus, " + 
                                   $"@ConvinienceFeesType, @ConvinienceFeesAmount, " +
                                   $"@InvoiceAmount, " +
                                   $"@CreatedBy, @CreatedOn";

                var sqlInsQuery = $"INSERT INTO "+ tableName + "( " + colName + " )" +
                                    $"VALUES ( " + colValueName + " )";

                object colValueParam = new
                {
                    InvoiceId = invoiceId,
                    OrderId = orderId,
                    CompanyId = companyId,
                    InvoiceType = invoiceType,
                    InvoiceStatus = invoiceStatus,
                    ConvinienceFeesType = convinienceFeesDetails.FeesType,
                    ConvinienceFeesAmount = convinienceFeesDetails.FeesAmount,
                    InvoiceAmount = invoiceTotalAmt.ToString(),
                    CreatedBy = createdBy,
                    CreatedOn = DateTime.UtcNow
                };

                using (var connection = _appDbContext.Connection)
                {
                    sqlResult = Convert.ToBoolean(await connection.ExecuteAsync(sqlInsQuery, colValueParam));
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
                sqlResult = false;
            }
            return sqlResult;
        }

        public async Task<bool> CreateInvoice(InvoiceRequest invoiceRequest)
        {
            var sqlResult = true;
            decimal invoiceTotalAmt = 0;
            decimal productTotalAmt = 0;
            decimal gstAmt = 0;
            try
            {
                var tableName = $"Logisticmate.invoice_obj";

                var colName = $"invoice_id, order_id, company_id, " +
                              $"invoice_type, invoice_status, " +
                              $"convinience_fees_type, convinience_fees_amt, " +
                              $"invoice_amt, " +
                              $"created_by, created_on";

                var colValueName = $"@InvoiceId, @OrderId, @CompanyId, " +
                                   $"@InvoiceType, @InvoiceStatus, " + 
                                   $"@ConvinienceFeesType, @ConvinienceFeesAmount, " +
                                   $"@InvoiceAmount, " +
                                   $"@CreatedBy, @CreatedOn";

                var sqlInsQuery = $"INSERT INTO "+ tableName + "( " + colName + " )" +
                                    $"VALUES ( " + colValueName + " )";

                ConvinienceFeesDetails convinienceFeesDetails = await _companyRepository.GetConvinienceFeesDetails(invoiceRequest.CompanyId);
                if(convinienceFeesDetails == null)
                    convinienceFeesDetails = new ConvinienceFeesDetails();

                //calculate product list total amount
                if(invoiceRequest.OrderDetails == null)
                {
                    invoiceRequest.OrderDetails = new OrderDetails();
                    invoiceRequest.OrderDetails.ProductDetailsList = new List<ProductDetails>();
                    List<OrderDetails> orderDetList = await _orderRepository.GetOrderDetails(invoiceRequest.CompanyId, invoiceRequest.OrderId);
                    if(orderDetList.Count > 0)
                        invoiceRequest.OrderDetails = orderDetList[0];
                }
                string calculatedProductAmt = CalculateProductTotalAmt(invoiceRequest.OrderDetails.ProductDetailsList);
                if(!String.IsNullOrEmpty(calculatedProductAmt))
                {
                    string[] splitVal = calculatedProductAmt.Split(',');
                    if(splitVal.Count() == 2)
                    {
                        productTotalAmt = Convert.ToDecimal(splitVal[0]);
                        gstAmt = Convert.ToDecimal(splitVal[1]);
                    }
                }

                //calculate the Convinience Amount
                decimal conveineAmt = 0;
                if(!String.IsNullOrEmpty(convinienceFeesDetails.FeesType))
                {
                    if(convinienceFeesDetails.FeesType.Equals("fixed"))
                        conveineAmt = convinienceFeesDetails.FeesAmount;
                    else
                    {
                        conveineAmt = (productTotalAmt * convinienceFeesDetails.FeesAmount) / 100;
                    }
                }

                invoiceTotalAmt = productTotalAmt + conveineAmt + gstAmt;

                object colValueParam = new
                {
                    InvoiceId = invoiceRequest.InvoiceId,
                    OrderId = invoiceRequest.OrderId,
                    CompanyId = invoiceRequest.CompanyId,
                    InvoiceType = invoiceRequest.InvoiceType,
                    InvoiceStatus = invoiceRequest.InvoiceStatus,
                    ConvinienceFeesType = convinienceFeesDetails.FeesType,
                    ConvinienceFeesAmount = convinienceFeesDetails.FeesAmount,
                    InvoiceAmount = invoiceTotalAmt.ToString(),
                    CreatedBy = invoiceRequest.CreatedBy,
                    CreatedOn = DateTime.UtcNow
                };

                using (var connection = _appDbContext.Connection)
                {
                    sqlResult = Convert.ToBoolean(await connection.ExecuteAsync(sqlInsQuery, colValueParam));
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
                sqlResult = false;
            }
            return sqlResult;
        }
        public async Task<int> GenerateInvoiceNo(string companyId, string invoiceType)
        {
            int invoiceNo = 0;
            try
            {
                var tableName = $"Logisticmate.invoice_obj ";

                var ColumAssign = $"invoice_id";

                var selQuery = $"select " + ColumAssign + " from " + tableName;

                var whereCondi = $"";

                if(!string.IsNullOrEmpty(companyId))
                    whereCondi += "company_id = '" + companyId + "'";

                if(!string.IsNullOrEmpty(invoiceType) && String.IsNullOrEmpty(whereCondi))
                    whereCondi += "invoice_type = '" + invoiceType + "'";
                else if(!string.IsNullOrEmpty(invoiceType) && !String.IsNullOrEmpty(whereCondi))
                    whereCondi += " and invoice_type = '" + invoiceType + "'";

                if(!string.IsNullOrEmpty(whereCondi))
                    whereCondi = "where " + whereCondi;

                var orderCond = $" order by invoice_id DESC ";

                var sqlSelQuery = selQuery + whereCondi + orderCond;
                using (var connection = _appDbContext.Connection)
                {
                    var sqlResult = await connection.QueryAsync<string>(sqlSelQuery);
                    invoiceNo = sqlResult.Count() + 1;//.FirstOrDefault();
                }
            }
            catch(Exception Err)
            {var Error = Err.Message.ToString();}
            return invoiceNo;
        }
        public async Task<bool> CheckInvoiceNoAvailability(string companyId, string invoiceType, string invoiceNo)
        {
            using (var connection = _appDbContext.Connection)
            {
                var tableName = $"Logisticmate.invoice_obj";
                var whereCond = $" where invoice_id = @InvoiceNo";

                if(!string.IsNullOrEmpty(companyId))
                    whereCond += $" and company_id != @CompanyId";

                if(!string.IsNullOrEmpty(invoiceType))
                    whereCond += $" and invoice_type = @InvoiceType";

                object colValueParam = new
                {
                    CompanyId = companyId,
                    InvoiceNo = invoiceNo,
                    InvoiceType = invoiceType
                };
                var sqlSelQuery = $"select * from " + tableName + whereCond;
                var sqlResult = await connection.QueryAsync<string>(sqlSelQuery, colValueParam);
                return sqlResult.ToList().Any();
            }
        }


        string CalculateProductTotalAmt(List<ProductDetails> productDetailsList)
        {
            string retVal = "";
            decimal productTotalAmt = 0;
            decimal gstTotalAmt = 0;
            try
            {
                foreach(ProductDetails singleProductDetails in productDetailsList)
                {
                    singleProductDetails.GSTDetails = new GSTDetails();
                    productTotalAmt += singleProductDetails.TotalAmount;
                    decimal gstPercent = singleProductDetails.GSTPercentage / 2;
                    singleProductDetails.GSTDetails.CGSTAmount = (singleProductDetails.TotalAmount * gstPercent) / 100;
                    singleProductDetails.GSTDetails.SGSTAmount = (singleProductDetails.TotalAmount * gstPercent) / 100;
                    singleProductDetails.GSTDetails.IGSTAmount = (singleProductDetails.TotalAmount * singleProductDetails.GSTPercentage) / 100;
                    //decimal singleProdGSTAmt = (singleProductDetails.TotalAmount * singleProductDetails.GSTPercentage) / 100;
                    gstTotalAmt += singleProductDetails.GSTDetails.IGSTAmount;
                }
                retVal = productTotalAmt + "," + gstTotalAmt;
            }
            catch(Exception Err)
            {var Error = Err.Message.ToString();}
            return retVal;
        }
    }
}