using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.DataProviders;
using Sitecore.Data.Managers;
using Sitecore.Diagnostics;
using Version = Sitecore.Data.Version;

namespace BasicDataProvider
{
    /// <summary>
    /// A very simple Sitecore DataProvider that just displays how a DataProvider works.
    /// </summary>
    public class BasicDataProvider3 : DataProvider
    {
        /// <summary>
        /// The ID of the item that is the parent of all data in this provider.
        /// </summary>
        private ID JoinParentId { get; set; }

        /// <summary>
        /// Holds the items that are exposed by this DataProvider.
        /// </summary>
        private Dictionary<ID,dynamic> Items { get; set; }

        private readonly ID _titleFieldId = ID.Parse("{456E6A92-7EC2-4F1A-A747-F5706A2472C5}");
        private readonly ID _textFieldId = ID.Parse("{165C1C9D-80C7-4EDE-8E43-54480E5B30CC}");
        private readonly ID _templateId = ID.Parse("{26120DE9-3AD3-4935-8294-C632B4DAC778}");

        public BasicDataProvider3(string joinParentId)
        {
            Assert.ArgumentNotNull(joinParentId, "joinParentId");
            Assert.IsTrue(ID.IsID(joinParentId), "joinParentId must be a valid Sitecore ID");

            JoinParentId = new ID(joinParentId);

            Items = new Dictionary<ID, dynamic>()
                {
                    { GetIdForIdentifierString("one"), new
                        {
                            Name = "First item",
                            Fields = new Dictionary<ID,string>
                                {
                                    {_titleFieldId, "First item title"},
                                    {_textFieldId, "First item text"}
                                }
                        }},
                    { GetIdForIdentifierString("two"), new
                        {
                            Name = "Second item",
                            Fields = new Dictionary<ID,string>
                                {
                                    {_titleFieldId, "Second item title"},
                                    {_textFieldId, "Second item text"}
                                }
                        }}
                };
        }

        public override ItemDefinition GetItemDefinition(ID itemId, CallContext context)
        {
            if (Items.ContainsKey(itemId))
            {
                return new ItemDefinition(itemId, Items[itemId].Name, _templateId, ID.Null);
            }
            return null;
        }

        public override VersionUriList GetItemVersions(ItemDefinition itemDefinition, CallContext context)
        {
            if (Items.ContainsKey(itemDefinition.ID))
            {
                VersionUriList versionUriList = new VersionUriList();
                versionUriList.Add(LanguageManager.DefaultLanguage, Version.Parse(1));
                return versionUriList;
            }
            return null;
        }

        public override FieldList GetItemFields(ItemDefinition itemDefinition, VersionUri versionUri, CallContext context)
        {
            if (Items.ContainsKey(itemDefinition.ID))
            {
                FieldList itemFields = new FieldList();
                foreach (KeyValuePair<ID,string> field in Items[itemDefinition.ID].Fields)
                {
                    itemFields.Add(field.Key, field.Value);
                }
                return itemFields;
            }
            return null;
        }

        public override IDList GetChildIDs(ItemDefinition itemDefinition, CallContext context)
        {
            if (itemDefinition != null && itemDefinition.ID == JoinParentId)
            {
                IDList childIDs = new IDList();
                foreach (ID id in Items.Keys)
                {
                    childIDs.Add(id);
                }
                return childIDs;
            }
            return null;
        }

        public override ID GetParentID(ItemDefinition itemDefinition, CallContext context)
        {
            if (Items.ContainsKey(itemDefinition.ID))
            {
                return JoinParentId;
            }
            return null;
        }

        public override LanguageCollection GetLanguages(CallContext context)
        {
            // we're not going to add any languages to the default ones
            return new LanguageCollection();
        }

        private static ID GetIdForIdentifierString(string identifierString)
        {
            using (MD5 md5 = MD5.Create())
            {
                return new ID(new Guid(md5.ComputeHash(Encoding.Default.GetBytes(identifierString))));
            }
        }

    }
}
