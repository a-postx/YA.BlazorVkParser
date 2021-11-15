using Microsoft.AspNetCore.JsonPatch;

namespace YA.WebClient.Application;

public static class PatchProducer
{
    public static JsonPatchDocument<UserSm> GetPatchForUser(UserSm sourceModel, UserSm destinationModel)
    {
        JsonPatchDocument<UserSm> patch = new JsonPatchDocument<UserSm>();

        if (!sourceModel.Equals(destinationModel))
        {
            if (sourceModel.Name != destinationModel.Name)
            {
                patch.Replace(p => p.Name, destinationModel.Name);
            }

            if (sourceModel.Email != destinationModel.Email)
            {
                patch.Replace(p => p.Email, destinationModel.Email);
            }

            if (!sourceModel.Settings.Equals(destinationModel.Settings))
            {
                patch.Replace(p => p.Settings.ShowGettingStarted, destinationModel.Settings.ShowGettingStarted);
            }
        }

        return patch;
    }

    public static JsonPatchDocument<TenantSm> GetPatchForTenant(TenantSm sourceModel, TenantSm destinationModel)
    {
        JsonPatchDocument<TenantSm> patch = new JsonPatchDocument<TenantSm>();

        if (!sourceModel.Equals(destinationModel))
        {
            if (sourceModel.Name != destinationModel.Name)
            {
                patch.Replace(p => p.Name, destinationModel.Name);
            }
        }

        return patch;
    }

    public static JsonPatchDocument<VkPeriodicParsingTaskSm> GetPatchForVkPeriodicParsingTask(VkPeriodicParsingTaskSm sourceModel, VkPeriodicParsingTaskSm destinationModel)
    {
        JsonPatchDocument<VkPeriodicParsingTaskSm> patch = new JsonPatchDocument<VkPeriodicParsingTaskSm>();

        if (!sourceModel.Equals(destinationModel))
        {
            if (sourceModel.Title != destinationModel.Title)
            {
                patch.Replace(p => p.Title, destinationModel.Title);
            }

            if (sourceModel.ExecutionOption != destinationModel.ExecutionOption)
            {
                patch.Replace(p => p.ExecutionOption, destinationModel.ExecutionOption);
            }

            //надо запретить изменять тип результата из-за нетестированных последствий
            if (sourceModel.ResultType != destinationModel.ResultType)
            {
                patch.Replace(p => p.ResultType, destinationModel.ResultType);
            }

            if (!sourceModel.Options.Equals(destinationModel.Options))
            {
                if (sourceModel.Options.RawLinkSources != destinationModel.Options.RawLinkSources)
                {
                    patch.Replace(p => p.Options.RawLinkSources, destinationModel.Options.RawLinkSources);
                }

                if (sourceModel.Options.RawTaskSources != destinationModel.Options.RawTaskSources)
                {
                    patch.Replace(p => p.Options.RawTaskSources, destinationModel.Options.RawTaskSources);
                }

                if (sourceModel.Options.ProfilesResultSubType != destinationModel.Options.ProfilesResultSubType)
                {
                    patch.Replace(p => p.Options.ProfilesResultSubType, destinationModel.Options.ProfilesResultSubType);
                }

                if (sourceModel.Options.TopProfilesOptions != null && destinationModel.Options.TopProfilesOptions != null)
                {
                    if (!sourceModel.Options.TopProfilesOptions.Equals(destinationModel.Options.TopProfilesOptions))
                    {
                        if (sourceModel.Options.TopProfilesOptions.TopType != destinationModel.Options.TopProfilesOptions.TopType)
                        {
                            patch.Replace(p => p.Options.TopProfilesOptions.TopType, destinationModel.Options.TopProfilesOptions.TopType);
                        }

                        if (sourceModel.Options.TopProfilesOptions.CommunitiesCount != destinationModel.Options.TopProfilesOptions.CommunitiesCount)
                        {
                            patch.Replace(p => p.Options.TopProfilesOptions.CommunitiesCount, destinationModel.Options.TopProfilesOptions.CommunitiesCount);
                        }
                    }
                }
                else
                {
                    if (sourceModel.Options.TopProfilesOptions != destinationModel.Options.TopProfilesOptions)
                    {
                        patch.Replace(p => p.Options.TopProfilesOptions, destinationModel.Options.TopProfilesOptions);
                    }
                }

                if (sourceModel.Options.ActiveProfilesOptions != null && destinationModel.Options.ActiveProfilesOptions != null)
                {
                    if (!sourceModel.Options.ActiveProfilesOptions.Equals(destinationModel.Options.ActiveProfilesOptions))
                    {
                        if (sourceModel.Options.ActiveProfilesOptions.ActivityStartDateTime != destinationModel.Options.ActiveProfilesOptions.ActivityStartDateTime)
                        {
                            patch.Replace(p => p.Options.ActiveProfilesOptions.ActivityStartDateTime, destinationModel.Options.ActiveProfilesOptions.ActivityStartDateTime);
                        }

                        if (sourceModel.Options.ActiveProfilesOptions.ActivityEndDateTime != destinationModel.Options.ActiveProfilesOptions.ActivityEndDateTime)
                        {
                            patch.Replace(p => p.Options.ActiveProfilesOptions.ActivityEndDateTime, destinationModel.Options.ActiveProfilesOptions.ActivityEndDateTime);
                        }

                        if (sourceModel.Options.ActiveProfilesOptions.ActivityCountFrom != destinationModel.Options.ActiveProfilesOptions.ActivityCountFrom)
                        {
                            patch.Replace(p => p.Options.ActiveProfilesOptions.ActivityCountFrom, destinationModel.Options.ActiveProfilesOptions.ActivityCountFrom);
                        }

                        if (!sourceModel.Options.ActiveProfilesOptions.ActivitySource.Equals(destinationModel.Options.ActiveProfilesOptions.ActivitySource))
                        {
                            if (sourceModel.Options.ActiveProfilesOptions.ActivitySource.Posts != destinationModel.Options.ActiveProfilesOptions.ActivitySource.Posts)
                            {
                                patch.Replace(p => p.Options.ActiveProfilesOptions.ActivitySource.Posts, destinationModel.Options.ActiveProfilesOptions.ActivitySource.Posts);
                            }

                            if (sourceModel.Options.ActiveProfilesOptions.ActivitySource.Discussions != destinationModel.Options.ActiveProfilesOptions.ActivitySource.Discussions)
                            {
                                patch.Replace(p => p.Options.ActiveProfilesOptions.ActivitySource.Discussions, destinationModel.Options.ActiveProfilesOptions.ActivitySource.Discussions);
                            }
                        }

                        if (!sourceModel.Options.ActiveProfilesOptions.ActivityType.Equals(destinationModel.Options.ActiveProfilesOptions.ActivityType))
                        {
                            if (sourceModel.Options.ActiveProfilesOptions.ActivityType.Likes != destinationModel.Options.ActiveProfilesOptions.ActivityType.Likes)
                            {
                                patch.Replace(p => p.Options.ActiveProfilesOptions.ActivityType.Likes, destinationModel.Options.ActiveProfilesOptions.ActivityType.Likes);
                            }

                            if (sourceModel.Options.ActiveProfilesOptions.ActivityType.LikesInComments != destinationModel.Options.ActiveProfilesOptions.ActivityType.LikesInComments)
                            {
                                patch.Replace(p => p.Options.ActiveProfilesOptions.ActivityType.LikesInComments, destinationModel.Options.ActiveProfilesOptions.ActivityType.LikesInComments);
                            }

                            if (sourceModel.Options.ActiveProfilesOptions.ActivityType.Comments != destinationModel.Options.ActiveProfilesOptions.ActivityType.Comments)
                            {
                                patch.Replace(p => p.Options.ActiveProfilesOptions.ActivityType.Comments, destinationModel.Options.ActiveProfilesOptions.ActivityType.Comments);
                            }

                            if (sourceModel.Options.ActiveProfilesOptions.ActivityType.Reposts != destinationModel.Options.ActiveProfilesOptions.ActivityType.Reposts)
                            {
                                patch.Replace(p => p.Options.ActiveProfilesOptions.ActivityType.Reposts, destinationModel.Options.ActiveProfilesOptions.ActivityType.Reposts);
                            }

                            if (sourceModel.Options.ActiveProfilesOptions.ActivityType.PostAuthors != destinationModel.Options.ActiveProfilesOptions.ActivityType.PostAuthors)
                            {
                                patch.Replace(p => p.Options.ActiveProfilesOptions.ActivityType.PostAuthors, destinationModel.Options.ActiveProfilesOptions.ActivityType.PostAuthors);
                            }
                        }

                        if (sourceModel.Options.ActiveProfilesOptions.LimitWallPostsCount != destinationModel.Options.ActiveProfilesOptions.LimitWallPostsCount)
                        {
                            patch.Replace(p => p.Options.ActiveProfilesOptions.LimitWallPostsCount, destinationModel.Options.ActiveProfilesOptions.LimitWallPostsCount);
                        }
                    }
                }
                else
                {
                    if (sourceModel.Options.ActiveProfilesOptions != destinationModel.Options.ActiveProfilesOptions)
                    {
                        patch.Replace(p => p.Options.ActiveProfilesOptions, destinationModel.Options.ActiveProfilesOptions);
                    }
                }

                if (sourceModel.Options.GroupIntersectionProfilesOptions != null && destinationModel.Options.GroupIntersectionProfilesOptions != null)
                {
                    if (!sourceModel.Options.GroupIntersectionProfilesOptions.Equals(destinationModel.Options.GroupIntersectionProfilesOptions))
                    {
                        if (sourceModel.Options.GroupIntersectionProfilesOptions.CountFrom != destinationModel.Options.GroupIntersectionProfilesOptions.CountFrom)
                        {
                            patch.Replace(p => p.Options.GroupIntersectionProfilesOptions.CountFrom, destinationModel.Options.GroupIntersectionProfilesOptions.CountFrom);
                        }

                        if (sourceModel.Options.GroupIntersectionProfilesOptions.CountTo != destinationModel.Options.GroupIntersectionProfilesOptions.CountTo)
                        {
                            patch.Replace(p => p.Options.GroupIntersectionProfilesOptions.CountTo, destinationModel.Options.GroupIntersectionProfilesOptions.CountTo);
                        }
                    }
                }
                else
                {
                    if (sourceModel.Options.GroupIntersectionProfilesOptions != destinationModel.Options.GroupIntersectionProfilesOptions)
                    {
                        patch.Replace(p => p.Options.GroupIntersectionProfilesOptions, destinationModel.Options.GroupIntersectionProfilesOptions);
                    }
                }

                if (sourceModel.Options.FriendsProfilesOptions != null && destinationModel.Options.FriendsProfilesOptions != null)
                {
                    if (!sourceModel.Options.FriendsProfilesOptions.Equals(destinationModel.Options.FriendsProfilesOptions))
                    {
                        if (sourceModel.Options.FriendsProfilesOptions.GetFriends != destinationModel.Options.FriendsProfilesOptions.GetFriends)
                        {
                            patch.Replace(p => p.Options.FriendsProfilesOptions.GetFriends, destinationModel.Options.FriendsProfilesOptions.GetFriends);
                        }

                        if (sourceModel.Options.FriendsProfilesOptions.GetFollowers != destinationModel.Options.FriendsProfilesOptions.GetFollowers)
                        {
                            patch.Replace(p => p.Options.FriendsProfilesOptions.GetFollowers, destinationModel.Options.FriendsProfilesOptions.GetFollowers);
                        }

                        if (sourceModel.Options.FriendsProfilesOptions.GetPeopleSubscriptions != destinationModel.Options.FriendsProfilesOptions.GetPeopleSubscriptions)
                        {
                            patch.Replace(p => p.Options.FriendsProfilesOptions.GetPeopleSubscriptions, destinationModel.Options.FriendsProfilesOptions.GetPeopleSubscriptions);
                        }
                    }
                }
                else
                {
                    if (sourceModel.Options.FriendsProfilesOptions != destinationModel.Options.FriendsProfilesOptions)
                    {
                        patch.Replace(p => p.Options.FriendsProfilesOptions, destinationModel.Options.FriendsProfilesOptions);
                    }
                }

                if (sourceModel.Options.TaCommunitiesOptions != null && destinationModel.Options.TaCommunitiesOptions != null)
                {
                    if (!sourceModel.Options.TaCommunitiesOptions.Equals(destinationModel.Options.TaCommunitiesOptions))
                    {
                        if (sourceModel.Options.TaCommunitiesOptions.TopType != destinationModel.Options.TaCommunitiesOptions.TopType)
                        {
                            patch.Replace(p => p.Options.TaCommunitiesOptions.TopType, destinationModel.Options.TaCommunitiesOptions.TopType);
                        }

                        if (sourceModel.Options.TaCommunitiesOptions.CommunitiesCount != destinationModel.Options.TaCommunitiesOptions.CommunitiesCount)
                        {
                            patch.Replace(p => p.Options.TaCommunitiesOptions.CommunitiesCount, destinationModel.Options.TaCommunitiesOptions.CommunitiesCount);
                        }
                    }
                }
                else
                {
                    if (sourceModel.Options.TaCommunitiesOptions != destinationModel.Options.TaCommunitiesOptions)
                    {
                        patch.Replace(p => p.Options.TaCommunitiesOptions, destinationModel.Options.TaCommunitiesOptions);
                    }
                }

                if (sourceModel.Options.CommunitiesSearchOptions != null && destinationModel.Options.CommunitiesSearchOptions != null)
                {
                    if (!sourceModel.Options.CommunitiesSearchOptions.Equals(destinationModel.Options.CommunitiesSearchOptions))
                    {
                        if (sourceModel.Options.CommunitiesSearchOptions.SearchType != destinationModel.Options.CommunitiesSearchOptions.SearchType)
                        {
                            patch.Replace(p => p.Options.CommunitiesSearchOptions.SearchType, destinationModel.Options.CommunitiesSearchOptions.SearchType);
                        }

                        if (sourceModel.Options.CommunitiesSearchOptions.GroupType != destinationModel.Options.CommunitiesSearchOptions.GroupType)
                        {
                            patch.Replace(p => p.Options.CommunitiesSearchOptions.GroupType, destinationModel.Options.CommunitiesSearchOptions.GroupType);
                        }

                        if (sourceModel.Options.CommunitiesSearchOptions.ResultSort != destinationModel.Options.CommunitiesSearchOptions.ResultSort)
                        {
                            patch.Replace(p => p.Options.CommunitiesSearchOptions.ResultSort, destinationModel.Options.CommunitiesSearchOptions.ResultSort);
                        }

                        if (sourceModel.Options.CommunitiesSearchOptions.Market != destinationModel.Options.CommunitiesSearchOptions.Market)
                        {
                            patch.Replace(p => p.Options.CommunitiesSearchOptions.Market, destinationModel.Options.CommunitiesSearchOptions.Market);
                        }

                        if (sourceModel.Options.CommunitiesSearchOptions.MembersMin != destinationModel.Options.CommunitiesSearchOptions.MembersMin)
                        {
                            patch.Replace(p => p.Options.CommunitiesSearchOptions.MembersMin, destinationModel.Options.CommunitiesSearchOptions.MembersMin);
                        }

                        if (sourceModel.Options.CommunitiesSearchOptions.MembersMax != destinationModel.Options.CommunitiesSearchOptions.MembersMax)
                        {
                            patch.Replace(p => p.Options.CommunitiesSearchOptions.MembersMax, destinationModel.Options.CommunitiesSearchOptions.MembersMax);
                        }

                        if (sourceModel.Options.CommunitiesSearchOptions.PhraseSearch != destinationModel.Options.CommunitiesSearchOptions.PhraseSearch)
                        {
                            patch.Replace(p => p.Options.CommunitiesSearchOptions.PhraseSearch, destinationModel.Options.CommunitiesSearchOptions.PhraseSearch);
                        }

                        if (sourceModel.Options.CommunitiesSearchOptions.MinusWords != destinationModel.Options.CommunitiesSearchOptions.MinusWords)
                        {
                            patch.Replace(p => p.Options.CommunitiesSearchOptions.MinusWords, destinationModel.Options.CommunitiesSearchOptions.MinusWords);
                        }

                        if (sourceModel.Options.CommunitiesSearchOptions.Trending != destinationModel.Options.CommunitiesSearchOptions.Trending)
                        {
                            patch.Replace(p => p.Options.CommunitiesSearchOptions.Trending, destinationModel.Options.CommunitiesSearchOptions.Trending);
                        }

                        if (sourceModel.Options.CommunitiesSearchOptions.Verified != destinationModel.Options.CommunitiesSearchOptions.Verified)
                        {
                            patch.Replace(p => p.Options.CommunitiesSearchOptions.Verified, destinationModel.Options.CommunitiesSearchOptions.Verified);
                        }
                    }
                }
                else
                {
                    if (sourceModel.Options.CommunitiesSearchOptions != destinationModel.Options.CommunitiesSearchOptions)
                    {
                        patch.Replace(p => p.Options.CommunitiesSearchOptions, destinationModel.Options.CommunitiesSearchOptions);
                    }
                }
            }

            if (!sourceModel.FilterOptions.Equals(destinationModel.FilterOptions))
            {
                if (sourceModel.FilterOptions.FilterEnabled != destinationModel.FilterOptions.FilterEnabled)
                {
                    patch.Replace(p => p.FilterOptions.FilterEnabled, destinationModel.FilterOptions.FilterEnabled);
                }

                if (sourceModel.FilterOptions.CommunitiesFilterOptions != null && destinationModel.FilterOptions.CommunitiesFilterOptions != null)
                {
                    if (!sourceModel.FilterOptions.CommunitiesFilterOptions.Equals(destinationModel.FilterOptions.CommunitiesFilterOptions))
                    {
                        if (sourceModel.FilterOptions.CommunitiesFilterOptions.LastPostPeriodStart != destinationModel.FilterOptions.CommunitiesFilterOptions.LastPostPeriodStart)
                        {
                            patch.Replace(p => p.FilterOptions.CommunitiesFilterOptions.LastPostPeriodStart, destinationModel.FilterOptions.CommunitiesFilterOptions.LastPostPeriodStart);
                        }

                        if (sourceModel.FilterOptions.CommunitiesFilterOptions.LastPostPeriodEnd != destinationModel.FilterOptions.CommunitiesFilterOptions.LastPostPeriodEnd)
                        {
                            patch.Replace(p => p.FilterOptions.CommunitiesFilterOptions.LastPostPeriodEnd, destinationModel.FilterOptions.CommunitiesFilterOptions.LastPostPeriodEnd);
                        }
                    }
                }
                else
                {
                    if (sourceModel.FilterOptions.CommunitiesFilterOptions != destinationModel.FilterOptions.CommunitiesFilterOptions)
                    {
                        patch.Replace(p => p.FilterOptions.CommunitiesFilterOptions, destinationModel.FilterOptions.CommunitiesFilterOptions);
                    }
                }
            }

            if (!sourceModel.AutomationOptions.Equals(destinationModel.AutomationOptions))
            {
                if (sourceModel.AutomationOptions.TaskExecutionRate != destinationModel.AutomationOptions.TaskExecutionRate)
                {
                    patch.Replace(p => p.AutomationOptions.TaskExecutionRate, destinationModel.AutomationOptions.TaskExecutionRate);
                }
            }

            if (!sourceModel.VkAdsExportOptions.Equals(destinationModel.VkAdsExportOptions))
            {
                if (sourceModel.VkAdsExportOptions.ExportToVkAds != destinationModel.VkAdsExportOptions.ExportToVkAds)
                {
                    patch.Replace(p => p.VkAdsExportOptions.ExportToVkAds, destinationModel.VkAdsExportOptions.ExportToVkAds);
                }

                if (sourceModel.VkAdsExportOptions.VkAdsAccount != null && destinationModel.VkAdsExportOptions.VkAdsAccount != null)
                {
                    if (!sourceModel.VkAdsExportOptions.VkAdsAccount.Equals(destinationModel.VkAdsExportOptions.VkAdsAccount))
                    {
                        if (sourceModel.VkAdsExportOptions.VkAdsAccount.Id != destinationModel.VkAdsExportOptions.VkAdsAccount.Id)
                        {
                            patch.Replace(p => p.VkAdsExportOptions.VkAdsAccount.Id, destinationModel.VkAdsExportOptions.VkAdsAccount.Id);
                        }

                        if (sourceModel.VkAdsExportOptions.VkAdsAccount.Name != destinationModel.VkAdsExportOptions.VkAdsAccount.Name)
                        {
                            patch.Replace(p => p.VkAdsExportOptions.VkAdsAccount.Name, destinationModel.VkAdsExportOptions.VkAdsAccount.Name);
                        }
                    }
                }
                else
                {
                    if (sourceModel.VkAdsExportOptions.VkAdsAccount != destinationModel.VkAdsExportOptions.VkAdsAccount)
                    {
                        patch.Replace(p => p.VkAdsExportOptions.VkAdsAccount, destinationModel.VkAdsExportOptions.VkAdsAccount);
                    }
                }

                if (sourceModel.VkAdsExportOptions.VkAdsTargetGroup != null && destinationModel.VkAdsExportOptions.VkAdsTargetGroup != null)
                {
                    if (!sourceModel.VkAdsExportOptions.VkAdsTargetGroup.Equals(destinationModel.VkAdsExportOptions.VkAdsTargetGroup))
                    {
                        if (sourceModel.VkAdsExportOptions.VkAdsTargetGroup.Id != destinationModel.VkAdsExportOptions.VkAdsTargetGroup.Id)
                        {
                            patch.Replace(p => p.VkAdsExportOptions.VkAdsTargetGroup.Id, destinationModel.VkAdsExportOptions.VkAdsTargetGroup.Id);
                        }

                        if (sourceModel.VkAdsExportOptions.VkAdsTargetGroup.Name != destinationModel.VkAdsExportOptions.VkAdsTargetGroup.Name)
                        {
                            patch.Replace(p => p.VkAdsExportOptions.VkAdsTargetGroup.Name, destinationModel.VkAdsExportOptions.VkAdsTargetGroup.Name);
                        }
                    }
                }
                else
                {
                    if (sourceModel.VkAdsExportOptions.VkAdsTargetGroup != destinationModel.VkAdsExportOptions.VkAdsTargetGroup)
                    {
                        patch.Replace(p => p.VkAdsExportOptions.VkAdsTargetGroup, destinationModel.VkAdsExportOptions.VkAdsTargetGroup);
                    }
                }
            }
        }

        return patch;
    }
}
