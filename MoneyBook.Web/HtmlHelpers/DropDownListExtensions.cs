using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace MoneyBook.Web.HtmlHelpers {
    public static class DropDownListExtensions {
        public static MvcHtmlString AjaxDropDownList(
            this HtmlHelper htmlHelper,
            string name,
            string group,
            string ajaxUrl,
            string textName = null,
            string optionLabel = null,
            string nonUsedLabel = null,
            string changeCallback = null,
            object extraParams = null,
            object htmlAttributes = null
        ) {
            return AjaxDropDownList(
                htmlHelper,
                name, group, ajaxUrl, textName, optionLabel, nonUsedLabel, changeCallback,
                extraParams, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)
            );
        }

        public static MvcHtmlString AjaxDropDownList(
            this HtmlHelper htmlHelper,
            string name,
            string group,
            string ajaxUrl,
            string textName,
            string optionLabel,
            string nonUsedLabel,
            string changeCallback,
            object extraParams,
            IDictionary<string, object> htmlAttributes
        ) {
            return AjaxDropDownListInternal(
                htmlHelper, null,
                name, group, ajaxUrl, textName, optionLabel, nonUsedLabel, changeCallback,
                HtmlHelper.AnonymousObjectToHtmlAttributes(extraParams), htmlAttributes
            );
        }

        public static MvcHtmlString AjaxDropDownListFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            string group,
            string ajaxUrl,
            string textName = null,
            string optionLabel = null,
            string nonUsedLabel = null,
            string changeCallback = null,
            object extraParams = null,
            object htmlAttributes = null
        ) {
            return AjaxDropDownListFor(
                htmlHelper,
                expression, group, ajaxUrl, textName, optionLabel, nonUsedLabel, changeCallback,
                extraParams, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)
            );
        }

        public static MvcHtmlString AjaxDropDownListFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            string group,
            string ajaxUrl,
            string textName,
            string optionLabel,
            string nonUsedLabel,
            string changeCallback,
            object extraParams,
            IDictionary<string, object> htmlAttributes
        ) {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            string name = ExpressionHelper.GetExpressionText(expression);

            return AjaxDropDownListInternal(
                htmlHelper, metadata,
                name, group, ajaxUrl, textName, optionLabel, nonUsedLabel, changeCallback,
                HtmlHelper.AnonymousObjectToHtmlAttributes(extraParams), htmlAttributes
            );
        }

        private static MvcHtmlString AjaxDropDownListInternal(
            HtmlHelper htmlHelper,
            ModelMetadata metadata,
            string name,
            string group,
            string ajaxUrl,
            string textName,
            string optionLabel,
            string nonUsedLabel,
            string changeCallback,
            IDictionary<string, object> extraParams,
            IDictionary<string, object> htmlAttributes
        ) {
            if (string.IsNullOrWhiteSpace(ajaxUrl)) {
                throw new ArgumentException("不可為Null或空字串。", nameof(ajaxUrl));
            }

            if (string.IsNullOrWhiteSpace(name)) {
                throw new ArgumentException("不可為Null或空字串。", nameof(name));
            }

            object defaultValue = metadata?.Model;

            TagBuilder select = new TagBuilder("select");
            select.MergeAttributes(htmlAttributes);
            select.GenerateId(name);
            select.MergeAttribute("name", name, true);
            select.MergeAttribute("data-ajax-url", ajaxUrl, true);
            select.MergeAttribute("data-default-value", defaultValue?.ToString(), true);

            if (group != null) {
                select.MergeAttribute("data-select-group", group, true);
            }

            if (!string.IsNullOrWhiteSpace(textName)) {
                select.MergeAttribute("data-text-name", textName, true);
            }

            if (optionLabel != null) {
                select.MergeAttribute("data-option-label", optionLabel, true);
            }

            if (nonUsedLabel != null) {
                select.MergeAttribute("data-non-used-label", nonUsedLabel, true);
            }

            if (!string.IsNullOrWhiteSpace(changeCallback)) {
                select.MergeAttribute("data-change-callback", changeCallback, true);
            }

            if (extraParams != null && extraParams.Count() > 0) {
                select.MergeAttribute("data-extra-params", JsonConvert.SerializeObject(extraParams), true);
            }

            return MvcHtmlString.Create(select.ToString(TagRenderMode.Normal));
        }
    }
}