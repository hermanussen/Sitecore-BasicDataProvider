using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.DataProviders;
using Sitecore.Diagnostics;

namespace BasicDataProvider
{
    /// <summary>
    /// A very simple Sitecore DataProvider that just displays how a DataProvider works.
    /// </summary>
    public class BasicDataProvider2 : DataProvider
    {
        /// <summary>
        /// The ID of the item that is the parent of all data in this provider.
        /// </summary>
        private ID JoinParentId { get; set; }

        /// <summary>
        /// Holds the items that are exposed by this DataProvider.
        /// </summary>
        private List<ID> Items { get; set; } 

        public BasicDataProvider2(string joinParentId)
        {
            Assert.ArgumentNotNull(joinParentId, "joinParentId");
            Assert.IsTrue(ID.IsID(joinParentId), "joinParentId must be a valid Sitecore ID");
            
            JoinParentId = new ID(joinParentId);

            Items = new List<ID>();
        }

        public override ItemDefinition GetItemDefinition(ID itemId, CallContext context)
        {
            if (Items.Contains(itemId))
            {
                Log.Info(string.Format("GetItemDefinition called with itemId = {0} (BasicDataProvider)", itemId), this);
            }
            return null;
        }

        public override VersionUriList GetItemVersions(ItemDefinition itemDefinition, CallContext context)
        {
            if (Items.Contains(itemDefinition.ID))
            {
                Log.Info(string.Format("GetItemVersions called with itemDefinition id = {0}, name = {1} (BasicDataProvider)", itemDefinition.ID, itemDefinition.Name), this);
            }
            return null;
        }

        public override FieldList GetItemFields(ItemDefinition itemDefinition, VersionUri versionUri, CallContext context)
        {
            if (Items.Contains(itemDefinition.ID))
            {
                Log.Info(string.Format("GetItemFields called with itemDefinition id = {0}, name = {1}, version = {2}, language = {3} (BasicDataProvider)", itemDefinition.ID, itemDefinition.Name, versionUri.Version, versionUri.Language.Name), this);
            }
            return null;
        }

        public override IDList GetChildIDs(ItemDefinition itemDefinition, CallContext context)
        {
            if (itemDefinition != null && itemDefinition.ID == JoinParentId)
            {
                Log.Info(string.Format("GetChildIDs called with itemDefinition id = {0}, name = {1} (BasicDataProvider)", itemDefinition.ID, itemDefinition.Name), this);
            }
            return null;
        }

        public override ID GetParentID(ItemDefinition itemDefinition, CallContext context)
        {
            if (Items.Contains(itemDefinition.ID))
            {
                Log.Info(string.Format("GetParentID called with itemDefinition id = {0}, name = {1} (BasicDataProvider)", itemDefinition.ID, itemDefinition.Name), this);
            }
            return null;
        }

        public override LanguageCollection GetLanguages(CallContext context)
        {
            // we're not going to add any languages to the default ones
            return new LanguageCollection();
        }

    }
}
