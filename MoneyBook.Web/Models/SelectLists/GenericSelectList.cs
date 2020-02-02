using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MoneyBook.Web.Models.SelectLists {
    public class GenericSelectList : IEnumerable<SelectListItem>, IEnumerable {
        private readonly IEnumerable items;
        private readonly string dataValueField;
        private readonly string dataTextField;
        private readonly IEnumerable disabledValues;
        private readonly IList<SelectListItem> forwardItems = new List<SelectListItem>();
        private readonly IList<SelectListItem> backItems = new List<SelectListItem>();

        public GenericSelectList(IEnumerable items, string dataValueField, string dataTextField, IEnumerable disabledValues = null) {
            this.items = items;
            this.dataValueField = dataValueField;
            this.dataTextField = dataTextField;
            this.disabledValues = disabledValues;
        }

        public virtual IEnumerable Items => items;

        public virtual string DataValueField => dataValueField;

        public virtual string DataTextField => dataTextField;

        public virtual IEnumerable DisabledValues => disabledValues;

        public void AddFirstSelectItem(string text, string value, bool isDisabled = false) {
            forwardItems.Add(new SelectListItem() {
                Text = text,
                Value = value,
                Disabled = isDisabled
            });
        }

        public void AddLastSelectItem(string text, string value, bool isDisabled = false) {
            backItems.Add(new SelectListItem() {
                Text = text,
                Value = value,
                Disabled = isDisabled
            });
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable<SelectListItem>)this).GetEnumerator();
        }

        public IEnumerator<SelectListItem> GetEnumerator() {

            SelectList selectList =
                new SelectList(Items ?? Enumerable.Empty<SelectListItem>(), DataValueField, dataTextField, null, null, DisabledValues);

            return forwardItems.Concat(selectList).Concat(backItems).GetEnumerator();
        }
    }
}