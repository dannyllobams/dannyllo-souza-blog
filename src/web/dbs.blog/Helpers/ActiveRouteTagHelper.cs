using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace dbs.blog.Helpers
{
    [HtmlTargetElement(Attributes = "is-active-route")]
    public class ActiveRouteTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public ActiveRouteTagHelper(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        private IDictionary<string, string>? _routeValues;

        [HtmlAttributeName("asp-action")]
        public string? Action { get; set; }

        [HtmlAttributeName("asp-controller")]
        public string? Controller { get; set; }

        [HtmlAttributeName("asp-page")]
        public string? Page { get; set; }

        [HtmlAttributeName("asp-all-route-data", DictionaryAttributePrefix = "asp-route-")]
        public IDictionary<string, string> RouteValues
        {
            get
            {
                if (this._routeValues == null)
                    this._routeValues = (IDictionary<string, string>)new Dictionary<string, string>((IEqualityComparer<string>)StringComparer.OrdinalIgnoreCase);

                return this._routeValues;
            }
            set
            {
                this._routeValues = value;
            }
        }


        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext? ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            if (ShouldBeActive())
            {
                MakeActive(output);
            }

            output.Attributes.RemoveAll("is-active-route");
        }

        private bool ShouldBeActive()
        {
            // Obtém valores atuais da rota
            var routeData = ViewContext!.RouteData;
            var currentController = routeData.Values["Controller"]?.ToString() ?? string.Empty;
            var currentAction = routeData.Values["Action"]?.ToString() ?? string.Empty;

            // Verifica se há critérios especificados
            bool hasController = !string.IsNullOrWhiteSpace(Controller);
            bool hasAction = !string.IsNullOrWhiteSpace(Action);
            bool hasPage = !string.IsNullOrWhiteSpace(Page);
            bool hasRouteValues = RouteValues.Count > 0;

            // Se não há nenhum critério especificado, não deve ser ativo
            if (!hasController && !hasAction && !hasPage && !hasRouteValues)
            {
                return false;
            }

            // Verifica Controller
            if (hasController)
            {
                if (string.IsNullOrWhiteSpace(currentController))
                {
                    return false;
                }
                if (!string.Equals(Controller, currentController, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            // Verifica Action
            if (hasAction)
            {
                if (string.IsNullOrWhiteSpace(currentAction))
                {
                    return false;
                }
                if (!string.Equals(Action, currentAction, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            // Verifica Page
            if (hasPage)
            {
                var currentPath = _contextAccessor.HttpContext!.Request.Path.Value ?? string.Empty;
                if (!string.Equals(Page, currentPath, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            // Verifica RouteValues
            if (hasRouteValues)
            {
                foreach (var routeValue in RouteValues)
                {
                    if (!routeData.Values.ContainsKey(routeValue.Key))
                    {
                        return false;
                    }
                    var routeDataValue = routeData.Values[routeValue.Key]?.ToString() ?? string.Empty;
                    if (!string.Equals(routeValue.Value, routeDataValue, StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }
                }
            }

            // Se chegou até aqui, todos os critérios especificados corresponderam
            return true;
        }

        private void MakeActive(TagHelperOutput output)
        {
            var classAttr = output.Attributes.FirstOrDefault(a => a.Name == "class");
            if (classAttr == null)
            {
                classAttr = new TagHelperAttribute("class", "active");
                output.Attributes.Add(classAttr);
            }
            else if (classAttr.Value == null || classAttr.Value.ToString()!.IndexOf("active") < 0)
            {
                output.Attributes.SetAttribute("class", classAttr.Value == null
                    ? "active"
                    : classAttr.Value.ToString() + " active");
            }
        }
    }
}
