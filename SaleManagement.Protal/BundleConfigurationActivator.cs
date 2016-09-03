using System.Web.Optimization;

[assembly: WebActivator.PostApplicationStartMethod(typeof(SaleManagement.Protal.BundleConfigurationActivator), "Activate")]
namespace SaleManagement.Protal
{
    public static class BundleConfigurationActivator
    {
        public static void Activate()
        {
            BundleTable.Bundles.RegisterConfigurationBundles();
        }
    }
}