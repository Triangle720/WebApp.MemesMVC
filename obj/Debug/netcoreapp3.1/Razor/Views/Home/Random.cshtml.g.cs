#pragma checksum "C:\Users\przslo\source\repos\WebApp\WebApp.MemesMVC\Views\Home\Random.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "e367af6cd1cc37225ea6a25dd6acd8dd9a7a4872"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Random), @"mvc.1.0.view", @"/Views/Home/Random.cshtml")]
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
#line 1 "C:\Users\przslo\source\repos\WebApp\WebApp.MemesMVC\Views\_ViewImports.cshtml"
using WebApp.MemesMVC;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\przslo\source\repos\WebApp\WebApp.MemesMVC\Views\_ViewImports.cshtml"
using WebApp.MemesMVC.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e367af6cd1cc37225ea6a25dd6acd8dd9a7a4872", @"/Views/Home/Random.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3dfd33285935f4964ebecdbcda97ba22c00c7589", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Random : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<WebApp.MemesMVC.Models.PictureModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "C:\Users\przslo\source\repos\WebApp\WebApp.MemesMVC\Views\Home\Random.cshtml"
  
    ViewData["Title"] = "Random";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
            WriteLiteral("\r\n<div class=\"image-list\">\r\n    <h1>Your random meme</h1>\r\n    <hr />\r\n    <div class=\"image-container\">\r\n        <h2>");
#nullable restore
#line 11 "C:\Users\przslo\source\repos\WebApp\WebApp.MemesMVC\Views\Home\Random.cshtml"
       Write(Model.Title);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h2>\r\n        <h6>Author: </h6>\r\n        <img class=\"image-main-style\"");
            BeginWriteAttribute("src", " src=\"", 289, "\"", 312, 1);
#nullable restore
#line 13 "C:\Users\przslo\source\repos\WebApp\WebApp.MemesMVC\Views\Home\Random.cshtml"
WriteAttributeValue("", 295, Model.UrlAddress, 295, 17, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" />\r\n    </div>\r\n\r\n</div>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<WebApp.MemesMVC.Models.PictureModel> Html { get; private set; }
    }
}
#pragma warning restore 1591