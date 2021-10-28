using System;
using System.Collections.Generic;

namespace YA.WebClient.Application.Models.ViewModels
{
    /// <summary>
    /// Tenant view model.
    /// </summary>
    public class TenantVm
    {
        /// <summary>
        /// Tenant unique identifier.
        /// </summary>
        public Guid TenantId { get; set; }

        /// <summary>
        /// Tenant name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// URL used to retrieve the resource conforming to REST'ful JSON http://restfuljson.org/.
        /// </summary>
        public Uri Url { get; set; }

        /// <summary>
        /// Current pricing tier identifier.
        /// </summary>
        public Guid PricingTierId { get; set; }

        /// <summary>
        /// Current pricing tier.
        /// </summary>
        public PricingTierVm PricingTier { get; set; }

        /// <summary>
        /// Date of pricing tier activation.
        /// </summary>
        public DateTime PricingTierActivatedDateTime { get; set; }

        /// <summary>
        /// Date the pricing tier is valid for.
        /// </summary>
        public DateTime PricingTierActivatedUntilDateTime { get; set; }

        /// <summary>
        /// Членства пользователей в арендаторе.
        /// </summary>
        public ICollection<MembershipVm> Memberships { get; set; }

        /// <summary>
        /// Приглашения пользователей в арендатор.
        /// </summary>
        public ICollection<InvitationVm> Invitations { get; set; }

        /// <summary>
        /// Отметка о том, что это текущий арендатор.
        /// </summary>
        public bool Current { get; set; }
    }
}
