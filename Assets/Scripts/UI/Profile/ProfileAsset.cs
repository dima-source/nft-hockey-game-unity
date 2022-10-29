using UI.Profile.Rewards;
using UnityEngine;

namespace UI.Profile
{
    [CreateAssetMenu(menuName = "Assets/Asset Profile", fileName = "Asset Profile")]
    public class ProfileAsset: ScriptableObject
    {
            public RewardView rewardView;
    }
}