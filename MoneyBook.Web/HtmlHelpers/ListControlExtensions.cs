using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;

namespace MoneyBook.Web.HtmlHelpers {

    public static class ListControlExtensions {

        private enum ListControlType {
            Checkbox,
            Radio
        }

        public static MvcHtmlString CheckBoxList(
            this HtmlHelper htmlHelper,
            string name,
            IEnumerable<SelectListItem> selectList,
            object htmlAttributes
        ) {
            return CheckBoxList(
                htmlHelper, name, selectList, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)
            );
        }

        public static MvcHtmlString CheckBoxList(
            this HtmlHelper htmlHelper,
            string name,
            IEnumerable<SelectListItem> selectList,
            IDictionary<string, object> htmlAttributes = null
        ) {
            return CheckBoxList(htmlHelper, name, selectList, null, htmlAttributes);
        }

        public static MvcHtmlString CheckBoxList(
            this HtmlHelper htmlHelper,
            string name,
            IEnumerable<SelectListItem> selectList,
            string optionLabel,
            object htmlAttributes
        ) {
            return CheckBoxList(
                htmlHelper, name, selectList, optionLabel, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)
            );
        }

        public static MvcHtmlString CheckBoxList(
            this HtmlHelper htmlHelper,
            string name,
            IEnumerable<SelectListItem> selectList,
            string optionLabel,
            IDictionary<string, object> htmlAttributes = null
        ) {
            return CheckBoxListInternal(htmlHelper, null, name, selectList, optionLabel, htmlAttributes);
        }

        public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> selectList,
            object htmlAttributes
        ) {
            return CheckBoxListFor(
                htmlHelper, expression, selectList, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)
            );
        }

        public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> selectList,
            IDictionary<string, object> htmlAttributes = null
        ) {

            return CheckBoxListFor(htmlHelper, expression, selectList, null, htmlAttributes);
        }

        public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> selectList,
            string optionLabel,
            object htmlAttributes
        ) {
            return CheckBoxListFor(
                htmlHelper, expression, selectList,
                optionLabel, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)
            );
        }

        public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> selectList,
            string optionLabel,
            IDictionary<string, object> htmlAttributes = null
        ) {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            string name = ExpressionHelper.GetExpressionText(expression);

            return CheckBoxListInternal(
                htmlHelper, metadata, name, selectList, optionLabel, htmlAttributes
            );
        }

        private static MvcHtmlString CheckBoxListInternal(
            HtmlHelper htmlHelper,
            ModelMetadata metadata,
            string name,
            IEnumerable<SelectListItem> selectList,
            string optionLabel,
            IDictionary<string, object> htmlAttributes
        ) {
            if (string.IsNullOrEmpty(name)) {
                throw new ArgumentNullException(nameof(name));
            }

            if (selectList == null) {
                throw new ArgumentNullException(nameof(selectList));
            }

            if (string.IsNullOrEmpty(name)) {
                throw new ArgumentException("格式錯誤。", nameof(name));
            }

            object defaultValue = metadata.Model;

            if (defaultValue != null) {
                selectList = GetCheckBoxSelectListWithDefaultValue(selectList, defaultValue);
            }

            int index = 0;

            StringBuilder sb = new StringBuilder();
            if (optionLabel != null) {
                sb.Append(CreateEmptyItem(ListControlType.Checkbox, $"{name}{index++}", name, optionLabel, htmlAttributes));
            }

            foreach (SelectListItem item in selectList) {
                sb.Append(CreateItem(ListControlType.Checkbox, item, $"{name}{index++}", name, htmlAttributes));
            }

            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString RadioButtonList(
            this HtmlHelper htmlHelper,
            string name,
            IEnumerable<SelectListItem> selectList,
            object htmlAttributes
        ) {
            return RadioButtonList(
                htmlHelper, name, selectList, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)
            );
        }

        public static MvcHtmlString RadioButtonList(
            this HtmlHelper htmlHelper,
            string name,
            IEnumerable<SelectListItem> selectList,
            IDictionary<string, object> htmlAttributes = null
        ) {
            return RadioButtonList(htmlHelper, name, selectList, null, htmlAttributes);
        }

        public static MvcHtmlString RadioButtonList(
            this HtmlHelper htmlHelper,
            string name,
            IEnumerable<SelectListItem> selectList,
            string optionLabel,
            object htmlAttributes
        ) {
            return RadioButtonList(
                htmlHelper, name, selectList, optionLabel, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)
            );
        }

        public static MvcHtmlString RadioButtonList(
            this HtmlHelper htmlHelper,
            string name,
            IEnumerable<SelectListItem> selectList,
            string optionLabel,
            IDictionary<string, object> htmlAttributes = null
        ) {
            return RadioButtonInternal(
                htmlHelper, null, name, selectList, optionLabel, htmlAttributes
            );
        }

        public static MvcHtmlString RadioButtonListFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> selectList,
            object htmlAttributes
        ) {
            return RadioButtonListFor(
                htmlHelper, expression, selectList, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)
            );
        }

        public static MvcHtmlString RadioButtonListFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> selectList,
            IDictionary<string, object> htmlAttributes = null
        ) {

            return RadioButtonListFor(htmlHelper, expression, selectList, null, htmlAttributes);
        }

        public static MvcHtmlString RadioButtonListFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> selectList,
            string optionLabel,
            object htmlAttributes
        ) {
            return RadioButtonListFor(
                htmlHelper, expression, selectList,
                optionLabel, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)
            );
        }

        public static MvcHtmlString RadioButtonListFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> selectList,
            string optionLabel,
            IDictionary<string, object> htmlAttributes = null
        ) {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            string name = ExpressionHelper.GetExpressionText(expression);

            return RadioButtonInternal(
                htmlHelper, metadata, name, selectList, optionLabel, htmlAttributes
            );
        }

        private static MvcHtmlString RadioButtonInternal(
            HtmlHelper htmlHelper,
            ModelMetadata metadata,
            string name,
            IEnumerable<SelectListItem> selectList,
            string optionLabel,
            IDictionary<string, object> htmlAttributes
        ) {
            if (string.IsNullOrEmpty(name)) {
                throw new ArgumentNullException(nameof(name));
            }

            if (selectList == null) {
                throw new ArgumentNullException(nameof(selectList));
            }

            if (string.IsNullOrEmpty(name)) {
                throw new ArgumentException("格式錯誤。", nameof(name));
            }

            object defaultValue = metadata.Model;

            if (defaultValue != null) {
                selectList = GetRadioButtonSelectListWithDefaultValue(selectList, defaultValue);
            }

            int index = 0;

            StringBuilder sb = new StringBuilder();
            if (optionLabel != null) {
                sb.Append(CreateEmptyItem(ListControlType.Radio, $"{name}{index++}", name, optionLabel, htmlAttributes));
            }

            foreach (SelectListItem item in selectList) {
                sb.Append(CreateItem(ListControlType.Radio, item, $"{name}{index++}", name, htmlAttributes));
            }

            return MvcHtmlString.Create(sb.ToString());
        }

        /// <summary>
        /// Gets the select list with default value.
        /// 從MVC Source Code挖出來的東西
        /// </summary>
        /// <param name="selectList">The select list.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private static IEnumerable<SelectListItem> GetCheckBoxSelectListWithDefaultValue(
            IEnumerable<SelectListItem> selectList, object defaultValue
        ) {
            IEnumerable defaultValues = defaultValue as IEnumerable;
            if (defaultValues == null || defaultValues is string) {
                throw new InvalidOperationException();
            }
            
            IEnumerable<string> values = from object value in defaultValues
                                         select Convert.ToString(value, CultureInfo.CurrentCulture);

            HashSet<string> selectedValues = new HashSet<string>(values, StringComparer.OrdinalIgnoreCase);
            List<SelectListItem> newSelectList = new List<SelectListItem>();

            foreach (SelectListItem item in selectList) {
                item.Selected = (item.Value != null) ? selectedValues.Contains(item.Value) : selectedValues.Contains(item.Text);
                newSelectList.Add(item);
            }
            return newSelectList;
        }

        private static IEnumerable<SelectListItem> GetRadioButtonSelectListWithDefaultValue(
            IEnumerable<SelectListItem> selectList, object defaultValue
        ) {
            string value = Convert.ToString(defaultValue, CultureInfo.CurrentCulture);

            List<SelectListItem> newSelectList = new List<SelectListItem>();
            foreach (SelectListItem item in selectList) {
                item.Selected = (item.Value != null) ? item.Value == value : item.Text == value;
                newSelectList.Add(item);
            }
            return newSelectList;
        }

        private static string CreateEmptyItem(
            ListControlType type, string id, string name, string optionLabel, IDictionary<string, object> htmlAttributes
        ) {
            SelectListItem emptyItem = new SelectListItem() {
                Text = optionLabel,
                Value = ""
            };
            return CreateItem(type, emptyItem, id, name, htmlAttributes);
        }

        private static string CreateItem(
            ListControlType type, SelectListItem item, string id, string name, IDictionary<string, object> htmlAttributes
        ) {
            TagBuilder container = new TagBuilder("div");
            if (htmlAttributes != null) {
                container.MergeAttributes(htmlAttributes);
                container.Attributes.Remove("inputClass");
                container.Attributes.Remove("labelClass");
            }

            object inputClass, labelClass;
            htmlAttributes.TryGetValue("inputClass", out inputClass);
            htmlAttributes.TryGetValue("labelClass", out labelClass);

            TagBuilder input = CreateItemInput(type, id, name, item, Convert.ToString(inputClass));
            // input設定Id時會修正，所以label的for要輸入修正過的Id
            TagBuilder label = CreateItemLabel(input.Attributes["id"], item.Text, Convert.ToString(labelClass));

            container.InnerHtml = input.ToString(TagRenderMode.Normal) + label.ToString(TagRenderMode.Normal);

            return container.ToString(TagRenderMode.Normal);
        }

        private static TagBuilder CreateItemInput(
            ListControlType type, string id, string name, SelectListItem item, string cssClass
        ) {
            TagBuilder input = new TagBuilder("input");
            input.GenerateId(id);
            input.MergeAttribute("type", type.ToString().ToLower());
            input.MergeAttribute("name", name);
            input.MergeAttribute("value", item.Value);
            input.MergeAttribute("class", cssClass);
            
            if (item.Selected) {
                input.MergeAttribute("checked", "checked");
            }

            if (item.Disabled) {
                input.MergeAttribute("disabled", "disabled");
            }

            return input;
        }

        private static TagBuilder CreateItemLabel(string forId, string text, string cssClass) {
            TagBuilder label = new TagBuilder("label");
            label.MergeAttribute("for", forId);
            label.InnerHtml = text;

            return label;
        }
    }
}