#pragma checksum "C:\Users\Blackerback\OneDrive\Masaüstü\MicroservicesArc\Microservices\Frontends\Microservices.Web\Views\Order\CheckoutHistory.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "979855c1ac14d4d3fed255576f3d14b42b1f6831"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Order_CheckoutHistory), @"mvc.1.0.view", @"/Views/Order/CheckoutHistory.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\Blackerback\OneDrive\Masaüstü\MicroservicesArc\Microservices\Frontends\Microservices.Web\Views\_ViewImports.cshtml"
using Microservices.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Blackerback\OneDrive\Masaüstü\MicroservicesArc\Microservices\Frontends\Microservices.Web\Views\_ViewImports.cshtml"
using Microservices.Web.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\Blackerback\OneDrive\Masaüstü\MicroservicesArc\Microservices\Frontends\Microservices.Web\Views\_ViewImports.cshtml"
using Microservices.Web.Models.Catalog;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\Blackerback\OneDrive\Masaüstü\MicroservicesArc\Microservices\Frontends\Microservices.Web\Views\_ViewImports.cshtml"
using Microservices.Web.Models.Auth;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\Blackerback\OneDrive\Masaüstü\MicroservicesArc\Microservices\Frontends\Microservices.Web\Views\_ViewImports.cshtml"
using Microservices.Web.Models.Basket;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\Users\Blackerback\OneDrive\Masaüstü\MicroservicesArc\Microservices\Frontends\Microservices.Web\Views\_ViewImports.cshtml"
using Microservices.Web.Models.Order;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"979855c1ac14d4d3fed255576f3d14b42b1f6831", @"/Views/Order/CheckoutHistory.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"41b890eaa6f73bb0f57b6be95f5aba42912a5fed", @"/Views/_ViewImports.cshtml")]
    public class Views_Order_CheckoutHistory : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<List<OrderViewModel>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n<div class=\"row\">\r\n    <div class=\"col-md-12\">\r\n        <div class=\"card\">\r\n            <div class=\"card-body\">\r\n                <h5 class=\"card-title\">Ödeme Geçmişim</h5>\r\n\r\n");
#nullable restore
#line 9 "C:\Users\Blackerback\OneDrive\Masaüstü\MicroservicesArc\Microservices\Frontends\Microservices.Web\Views\Order\CheckoutHistory.cshtml"
                 if (Model.Any())
                {

#line default
#line hidden
#nullable disable
            WriteLiteral(@"                    <table class=""table table-striped table-bordered"">
                        <tr>
                            <th>Sipariş Numarası</th>
                            <th>Satın Alma Tarihi</th>
                            <th>Detaylar</th>
                        </tr>
");
#nullable restore
#line 17 "C:\Users\Blackerback\OneDrive\Masaüstü\MicroservicesArc\Microservices\Frontends\Microservices.Web\Views\Order\CheckoutHistory.cshtml"
                         foreach (var order in Model)
                        {

#line default
#line hidden
#nullable disable
            WriteLiteral("                            <tr>\r\n                                <td>");
#nullable restore
#line 20 "C:\Users\Blackerback\OneDrive\Masaüstü\MicroservicesArc\Microservices\Frontends\Microservices.Web\Views\Order\CheckoutHistory.cshtml"
                               Write(order.Id);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                <td>");
#nullable restore
#line 21 "C:\Users\Blackerback\OneDrive\Masaüstü\MicroservicesArc\Microservices\Frontends\Microservices.Web\Views\Order\CheckoutHistory.cshtml"
                               Write(order.CreatedDate.ToShortDateString());

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</td>
                                <td>
                                    <table class=""table table-striped table-sm"">
                                        <tr>
                                            <th>Kurs Ismi</th>
                                            <th>Kurs Fiyat</th>
                                        </tr>
");
#nullable restore
#line 28 "C:\Users\Blackerback\OneDrive\Masaüstü\MicroservicesArc\Microservices\Frontends\Microservices.Web\Views\Order\CheckoutHistory.cshtml"
                                         foreach (var item in order.OrderItems)
                                        {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                            <tr>\r\n                                                <td>");
#nullable restore
#line 31 "C:\Users\Blackerback\OneDrive\Masaüstü\MicroservicesArc\Microservices\Frontends\Microservices.Web\Views\Order\CheckoutHistory.cshtml"
                                               Write(item.ProductName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                                <td>");
#nullable restore
#line 32 "C:\Users\Blackerback\OneDrive\Masaüstü\MicroservicesArc\Microservices\Frontends\Microservices.Web\Views\Order\CheckoutHistory.cshtml"
                                               Write(item.Price);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                            </tr>\r\n");
#nullable restore
#line 34 "C:\Users\Blackerback\OneDrive\Masaüstü\MicroservicesArc\Microservices\Frontends\Microservices.Web\Views\Order\CheckoutHistory.cshtml"
                                        }

#line default
#line hidden
#nullable disable
            WriteLiteral("                                    </table>\r\n                                </td>\r\n                            </tr>\r\n");
#nullable restore
#line 38 "C:\Users\Blackerback\OneDrive\Masaüstü\MicroservicesArc\Microservices\Frontends\Microservices.Web\Views\Order\CheckoutHistory.cshtml"
                        }

#line default
#line hidden
#nullable disable
            WriteLiteral("                    </table>\r\n");
#nullable restore
#line 40 "C:\Users\Blackerback\OneDrive\Masaüstü\MicroservicesArc\Microservices\Frontends\Microservices.Web\Views\Order\CheckoutHistory.cshtml"
                }
                else
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <div class=\"alert alert-info\">\r\n                        Satın aldığınız herhangi bir kurs bulunmamaktadır\r\n                    </div>\r\n");
#nullable restore
#line 46 "C:\Users\Blackerback\OneDrive\Masaüstü\MicroservicesArc\Microservices\Frontends\Microservices.Web\Views\Order\CheckoutHistory.cshtml"
                }

#line default
#line hidden
#nullable disable
            WriteLiteral("            </div>\r\n        </div>\r\n    </div>\r\n</div>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<List<OrderViewModel>> Html { get; private set; }
    }
}
#pragma warning restore 1591
