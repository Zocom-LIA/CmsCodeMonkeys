using CodeMonkeys.CMS.Public.Components.Shared;
using CodeMonkeys.CMS.Public.Components.Shared.UI;
using CodeMonkeys.CMS.Public.Shared.Entities;

using Microsoft.AspNetCore.Components;

namespace CodeMonkeys.CMS.Public.Components.Pages.Sites
{
    public partial class SitesIndex : AuthenticationBaseComponent<SitesIndex>
    {
        [SupplyParameterFromForm]
        private SiteModel Site { get; set; } = new SiteModel();

        private List<Site> Sites = [];

        private ConfirmationDialog? Confirmation { get; set; }
        private string ConfirmationTitle = "Delete Content";
        private string? ConfirmationMessage = "Are you sure that you want to delete this content?";
        private string ConfirmationButtonText = "Yes";
        private string CancelButtonText = "Cancel";
        private EventCallback OnConfirm;
        private EventCallback OnCancel;
        private Dictionary<int, string> PageOptions = new();

        private const int NoLandingPage = -1; //Apparently, you cannot key a dictionary on a nullable type. Hopefully, we will never have to deal with a database that assigns this number to anything.

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            Sites = (await SiteService.GetUserSitesAsync(User!.Id)).ToList();

            if (Confirmation == null)
            {
                Confirmation = new ConfirmationDialog();
            }
        }

        private Func<Task> HandleValidSubmit = () => Task.CompletedTask;
        public Task AddOrUpdateSite(int? siteId = null)
        {
            ConfirmationMessage = null;
            PageOptions = new() { { NoLandingPage, "None" } };

            if (siteId == null)
            {
                ConfirmationTitle = "Add Site";

                Site = new SiteModel()
                {
                    Name = "A Site Name"
                };

                ConfirmationButtonText = "Add Site";
            }
            else
            {
                ConfirmationTitle = "Edit Site";
                var site = Sites.FirstOrDefault(site => site.SiteId == siteId);
                if (site == null)
                {
                    ErrorMessage = "Site not found";
                    return Task.CompletedTask;
                }

                Site = new SiteModel
                {
                    SiteId = site.SiteId,
                    Name = site.Name,
                    LandingPageId = site.LandingPageId ?? NoLandingPage
                };
                ConfirmationButtonText = "Update Site";
                foreach (WebPage page in site.Pages) PageOptions.Add(page.WebPageId, page.Title);
            }

            CancelButtonText = "Cancel";
            HandleValidSubmit = async () => await AddOrUpdateSiteConfirmed(siteId);
            OnConfirm = EventCallback.Factory.Create(this, HandleValidSubmit);
            OnCancel = EventCallback.Factory.Create(this, () => CloseConfirmation());

            ShowConfirmation();
            return Task.CompletedTask;
        }

        private async Task AddOrUpdateSiteConfirmed(int? siteId = null)
        {
            if (Site == null)
            {
                ErrorMessage = "Site is required";
                return;
            }

            if (string.IsNullOrEmpty(Site.Name))
            {
                ErrorMessage = "Title is required";
                return;
            }


            if (siteId == null)
            {
                var site = new Site
                {
                    Name = Site.Name,
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    Creator = User
                };

                Sites.Add(site);

                await SiteService.CreateSiteAsync(site);
            }
            else
            {
                var site = Sites.FirstOrDefault(site => site.SiteId == siteId);
                if (site == null)
                {
                    ErrorMessage = "Site not found";
                    return;
                }

                site.SiteId = Site.SiteId > 0 ? Site.SiteId : Sites.Aggregate((cur, max) => cur.SiteId > max.SiteId ? cur : max).SiteId + 1;
                site.Name = Site.Name;
                site.LastModifiedDate = DateTime.Now;
                site.LandingPageId = (Site.LandingPageId == NoLandingPage) ? null : Site.LandingPageId;

                await SiteService.UpdateSiteAsync(site);
            }

            CloseConfirmation();
        }

        public Task DeleteSite(int siteId)
        {
            var site = Sites.FirstOrDefault(site => site.SiteId == siteId);
            if (site == null)
            {
                ErrorMessage = "Site not found";
                return Task.CompletedTask;
            }

            ConfirmationTitle = "Delete Site";
            ConfirmationMessage = $"Are you sure that you want to delete the site '{site.Name}'?";
            ConfirmationButtonText = "Delete Site";
            CancelButtonText = "Cancel";
            HandleValidSubmit = async () => await DeleteSiteConfirmed(siteId);
            OnConfirm = EventCallback.Factory.Create(this, HandleValidSubmit);
            OnCancel = EventCallback.Factory.Create(this, () => CloseConfirmation());

            ShowConfirmation();
            return Task.CompletedTask;
        }

        private async Task DeleteSiteConfirmed(int siteId)
        {
            var site = Sites.FirstOrDefault(site => site.SiteId == siteId);
            if (site == null)
            {
                ErrorMessage = "Site not found";
                return;
            }

            Sites.Remove(site);
            await SiteService.DeleteSiteAsync(site);

            CloseConfirmation();
        }

        private void CloseConfirmation()
        {
            Confirmation?.Hide();
        }

        private void ShowConfirmation()
        {
            Confirmation?.Show();
        }


        private sealed class SiteModel
        {
            public int SiteId { get; set; }
            public string Name { get; set; }
            public int LandingPageId { get; set; } = NoLandingPage;
        }
    }
}
