#pragma checksum "E:\Сабина, курс\Задание 21\NotebookSecond\NotebookSecond\Views\Roles\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "b308ec1f33ee5af65224d9092a90c5f8a46dc609"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Roles_Index), @"mvc.1.0.view", @"/Views/Roles/Index.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b308ec1f33ee5af65224d9092a90c5f8a46dc609", @"/Views/Roles/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a9af4978b9c2bfca24ef48e96efe5f8573634464", @"/Views/_ViewImports.cshtml")]
    public class Views_Roles_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<Microsoft.AspNetCore.Identity.IdentityRole>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "E:\Сабина, курс\Задание 21\NotebookSecond\NotebookSecond\Views\Roles\Index.cshtml"
  
    ViewData["Title"] = "Список ролей системы";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"<div class=""page_content"">
    <h1>Список ролей пользователей</h1>
    <table class=""table caption-top"">
        <thead class=""table-light"">
            <tr>
                <th>#</th>
                <th>Id</th>
                <th>Название</th>
            </tr>
        </thead>
");
#nullable restore
#line 15 "E:\Сабина, курс\Задание 21\NotebookSecond\NotebookSecond\Views\Roles\Index.cshtml"
          int i = 0; 

#line default
#line hidden
#nullable disable
#nullable restore
#line 16 "E:\Сабина, курс\Задание 21\NotebookSecond\NotebookSecond\Views\Roles\Index.cshtml"
         foreach (var item in Model)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <tr>\r\n                <td>\r\n                    ");
#nullable restore
#line 20 "E:\Сабина, курс\Задание 21\NotebookSecond\NotebookSecond\Views\Roles\Index.cshtml"
                Write(i + 1);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </td>\r\n                <td>\r\n                    ");
#nullable restore
#line 23 "E:\Сабина, курс\Задание 21\NotebookSecond\NotebookSecond\Views\Roles\Index.cshtml"
               Write(item.Id);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </td>\r\n                <td>\r\n                    ");
#nullable restore
#line 26 "E:\Сабина, курс\Задание 21\NotebookSecond\NotebookSecond\Views\Roles\Index.cshtml"
               Write(item.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </td>\r\n            </tr>\r\n");
#nullable restore
#line 29 "E:\Сабина, курс\Задание 21\NotebookSecond\NotebookSecond\Views\Roles\Index.cshtml"
        }

#line default
#line hidden
#nullable disable
            WriteLiteral("    </table>\r\n</div>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<Microsoft.AspNetCore.Identity.IdentityRole>> Html { get; private set; }
    }
}
#pragma warning restore 1591