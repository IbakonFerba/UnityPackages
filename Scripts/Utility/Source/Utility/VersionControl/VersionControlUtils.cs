using UnityEditor.VersionControl;

namespace FK.Utility.VersionControl
{
    /// <summary>
    /// <para>Contains useful functions for working with Version Control</para>
    ///
    /// v1.1 10/2019
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    public static class VersionControlUtils
    {
        /// <summary>
        /// Checks out an asset from version control and can add it autoamtically if it is not yet under VC
        /// </summary>
        /// <param name="path">Unity path to the asset</param>
        /// <param name="automaticAdd">If TRUE, assets that are not yet added to version control will be added automatically</param>
        /// <returns>FALSE if the Asset could not be checked out because it does not exist, the provider is not active or it was not added yet</returns>
        public static bool CheckoutAsset(string path, bool automaticAdd = false)
        {
            if (!Provider.isActive)
                return false;

            Asset asset = Provider.GetAssetByPath(path);

            // if the asset is null it means it does not exist
            if (asset == null)
                return false;

            // make sure the asset is not updating
            do
            {
                Provider.Status(asset).Wait();
            } while (asset.IsState(Asset.States.Updating));

            // if the asset is in one of these states, it is defenitely under VC
            Asset.States[] vcStates =
            {
                Asset.States.Conflicted, Asset.States.Synced, Asset.States.AddedLocal, Asset.States.LockedLocal, Asset.States.LockedRemote, Asset.States.MovedLocal, Asset.States.MovedRemote,
                Asset.States.CheckedOutLocal, Asset.States.CheckedOutRemote, Asset.States.OutOfSync, Asset.States.AddedRemote
            };
            
            // we might need to add the asset
            if (!asset.IsOneOfStates(vcStates) || asset.state == Asset.States.None)
            {
                if (automaticAdd)
                    return AddToVersionControl(path);
                return false;
            }

            if (!Provider.IsOpenForEdit(asset))
                Provider.Checkout(asset, CheckoutMode.Asset).Wait();

            return true;
        }

        /// <summary>
        /// Adds an Asset to version control
        /// </summary>
        /// <param name="path">Unity path to the asset</param>
        /// <param name="recursive"></param>
        /// <returns>FALSE if the Asset could not be checked out because it does not exist or the provider is not active</returns>
        public static bool AddToVersionControl(string path, bool recursive = true)
        {
            if (!Provider.isActive)
                return false;

            Asset asset = Provider.GetAssetByPath(path);

            if (asset == null)
                return false;

            Provider.Add(asset, recursive).Wait();
            return true;
        }
    }
}